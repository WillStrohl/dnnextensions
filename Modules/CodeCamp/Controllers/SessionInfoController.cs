/*
 * Copyright (c) 2015, Will Strohl
 * All rights reserved.
 * 
 * Redistribution and use in source and binary forms, with or without modification, 
 * are permitted provided that the following conditions are met:
 * 
 * Redistributions of source code must retain the above copyright notice, this list 
 * of conditions and the following disclaimer.
 * 
 * Redistributions in binary form must reproduce the above copyright notice, this 
 * list of conditions and the following disclaimer in the documentation and/or 
 * other materials provided with the distribution.
 * 
 * Neither the name of Will Strohl, nor the names of its contributors may be used 
 * to endorse or promote products derived from this software without specific prior 
 * written permission.
 * 
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND 
 * ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED 
 * WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. 
 * IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, 
 * INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, 
 * BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, 
 * DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF 
 * LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE 
 * OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF 
 * ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/

using System.Collections.Generic;
using System.Linq;

namespace WillStrohl.Modules.CodeCamp.Entities
{
    public class SessionInfoController
    {
        private readonly SessionInfoRepository repo = null;
        private readonly SessionRegistrationInfoController registrantRepo = null;
        private readonly SpeakerInfoController speakerRepo = null;
        private readonly SessionSpeakerInfoController sessionSpeakerRepo = null;

        public SessionInfoController() 
        {
            repo = new SessionInfoRepository();
            registrantRepo = new SessionRegistrationInfoController();
            speakerRepo = new SpeakerInfoController();
            sessionSpeakerRepo = new SessionSpeakerInfoController();
        }

        public void CreateItem(SessionInfo i)
        {
            repo.CreateItem(i);
        }

        public void DeleteItem(int itemId, int codeCampId)
        {
            repo.DeleteItem(itemId, codeCampId);
        }

        public void DeleteItem(SessionInfo i)
        {
            repo.DeleteItem(i);
        }

        public IEnumerable<SessionInfo> GetItems(int codeCampId)
        {
            var items = repo.GetItems(codeCampId);

            items.Select(s => { s.RegistrantCount = GetRegistrantCount(s.SessionId); return s; });

            foreach (var item in items)
            {
                item.Speakers = GetSpeakers(item.SessionId, item.CodeCampId);
            }

            return items;
        }

        public IEnumerable<SessionInfo> GetItemsUnassigned(int codeCampId)
        {
            var items = repo.GetItems(codeCampId).Where(s => !s.TrackId.HasValue);

            items.Select(s => { s.RegistrantCount = GetRegistrantCount(s.SessionId); return s; });

            foreach (var item in items)
            {
                item.Speakers = GetSpeakers(item.SessionId, item.CodeCampId);
            }

            return items;
        }

        public IEnumerable<SessionInfo> GetItemsByTrackId(int trackId, int codeCampId)
        {
            var items = repo.GetItems(codeCampId).Where(t => t.TrackId == trackId);

            items.Select(s => { s.RegistrantCount = GetRegistrantCount(s.SessionId); return s; });

            foreach (var item in items)
            {
                item.Speakers = GetSpeakers(item.SessionId, item.CodeCampId);
            }

            return items;
        }

        public SessionInfo GetItem(int itemId, int codeCampId)
        {
            var item = repo.GetItem(itemId, codeCampId);

            UpdateSessionWithRegistrantCount(ref item);
            UpdateSessionWithSpeakers(ref item);

            return item;
        }

        public void UpdateItem(SessionInfo i)
        {
            repo.UpdateItem(i);
        }

        #region Private Helper Methods

        private int GetRegistrantCount(int sessionId)
        {
            var registrants = registrantRepo.GetItems(sessionId);

            return registrants.Any() ? registrants.Count() : 0;
        }

        private void UpdateSessionWithRegistrantCount(ref SessionInfo item)
        {
            item.RegistrantCount = GetRegistrantCount(item.SessionId);
        }

        private List<SpeakerInfoLite> GetSpeakers(int sessionId, int codeCampId)
        {
            var speakerList = sessionSpeakerRepo.GetItems(sessionId).Select(s => s.SpeakerId);
            var speakers = speakerRepo.GetItems(codeCampId).Where(s => speakerList.Contains(s.SpeakerId)).ToList();

            return speakers.Select(speaker => new SpeakerInfoLite(speaker)).ToList();
        }

        private void UpdateSessionWithSpeakers(ref SessionInfo item)
        {
            item.Speakers = GetSpeakers(item.SessionId, item.CodeCampId);
        }

        #endregion
    }
}