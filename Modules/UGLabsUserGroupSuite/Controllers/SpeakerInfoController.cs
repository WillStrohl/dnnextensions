/*
 * Copyright (c) 2016, Will Strohl
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
    public class SpeakerInfoController
    {
        private readonly SpeakerInfoRepository repo = null;
        private readonly SessionInfoRepository sessionRepo = null;
        private readonly SessionSpeakerInfoRepository sessionSpeakerRepo = null;

        public SpeakerInfoController() 
        {
            repo = new SpeakerInfoRepository();
            sessionRepo = new SessionInfoRepository();
            sessionSpeakerRepo = new SessionSpeakerInfoRepository();
        }

        public void CreateItem(SpeakerInfo i)
        {
            repo.CreateItem(i);
        }

        public void DeleteItem(int itemId, int codeCampId)
        {
            repo.DeleteItem(itemId, codeCampId);
        }

        public void DeleteItem(SpeakerInfo i)
        {
            repo.DeleteItem(i);
        }

        public IEnumerable<SpeakerInfo> GetItems(int codeCampId)
        {
            var items = repo.GetItems(codeCampId);

            items.Select(c => { c.Sessions = GetSessionsForSpeaker(c.CodeCampId,c.SpeakerId); return c; }).ToList();
            
            return items;
        }

        public SpeakerInfo GetItem(int itemId, int codeCampId)
        {
            var item = repo.GetItem(itemId, codeCampId);

            UpdateSpeakerWithSessions(ref item);

            return item;
        }

        public SpeakerInfo GetItemByRegistrationId(int codeCampId, int registrationId)
        {
            var item = repo.GetItemByRegistrationId(codeCampId, registrationId);

            UpdateSpeakerWithSessions(ref item);

            return item;
        }

        public void UpdateItem(SpeakerInfo i)
        {
            repo.UpdateItem(i);
        }

        public List<SpeakerInfoLite> GetSpeakersForCollection(int sessionId, int codeCampId)
        {
            var speakerList = sessionSpeakerRepo.GetItems(sessionId).Select(s => s.SpeakerId);
            var speakers = repo.GetItems(codeCampId).Where(s => speakerList.Contains(s.SpeakerId)).ToList();

            return speakers.Select(speaker => new SpeakerInfoLite(speaker)).ToList();
        }

        #region Private Helper Methods

        private List<SessionInfo> GetSessionsForSpeaker(int codeCampId, int speakerId)
        {
            var allSessions = sessionRepo.GetItems(codeCampId);
            var sessionSpeakers = sessionSpeakerRepo.GetItemsBySpeakerId(speakerId).Select(s => s.SessionId);
            var sessions = allSessions.Where(s => sessionSpeakers.Contains(s.SessionId));

            return sessions.ToList();
        }

        private void UpdateSpeakerWithSessions(ref SpeakerInfo item)
        {
            if (item != null)
            {
                item.Sessions = GetSessionsForSpeaker(item.CodeCampId, item.SpeakerId);
            }
        }

        #endregion
    }
}