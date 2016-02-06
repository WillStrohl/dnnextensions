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

using System;
using System.Collections.Generic;
using System.Linq;

namespace WillStrohl.Modules.CodeCamp.Entities
{
    public class SessionInfoController
    {
        private readonly SessionInfoRepository repo = null;
        private readonly SessionRegistrationInfoController registrantRepo = null;
        private readonly SpeakerInfoController speakerRepo = null;
        //private readonly SessionSpeakerInfoController sessionSpeakerRepo = null;
        private readonly TimeSlotInfoController timeSlotRepo = null;

        public SessionInfoController() 
        {
            repo = new SessionInfoRepository();
            registrantRepo = new SessionRegistrationInfoController();
            speakerRepo = new SpeakerInfoController();
            //sessionSpeakerRepo = new SessionSpeakerInfoController();
            timeSlotRepo = new TimeSlotInfoController();
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
                item.Speakers = speakerRepo.GetSpeakersForCollection(item.SessionId, item.CodeCampId);
            }

            return items;
        }

        public IEnumerable<SessionInfo> GetItemsUnassigned(int codeCampId)
        {
            var items = repo.GetItems(codeCampId).Where(s => !s.TrackId.HasValue);

            items.Select(s => { s.RegistrantCount = GetRegistrantCount(s.SessionId); return s; });

            foreach (var item in items)
            {
                item.Speakers = speakerRepo.GetSpeakersForCollection(item.SessionId, item.CodeCampId);
            }

            return items;
        }

        public IEnumerable<SessionInfo> GetItemsByTrackId(int trackId, int codeCampId)
        {
            var items = repo.GetItems(codeCampId).Where(t => t.TrackId == trackId);

            items.Select(s => { s.RegistrantCount = GetRegistrantCount(s.SessionId); return s; });

            foreach (var item in items)
            {
                item.Speakers = speakerRepo.GetSpeakersForCollection(item.SessionId, item.CodeCampId);
            }

            SortSessions(ref items, codeCampId);

            return items;
        }

        public IEnumerable<SessionInfo> GetItemsByTimeSlotId(int timeSlotId, int codeCampId)
        {
            return GetItemsByTimeSlotIdByPage(timeSlotId, codeCampId, 1, int.MaxValue);
        }

        public IEnumerable<SessionInfo> GetItemsByTimeSlotIdByPage(int timeSlotId, int codeCampId, int pageNumber, int pageSize)
        {
            var items = repo.GetItems(codeCampId).Where(t => t.TimeSlotId == timeSlotId);

            items.Select(s => { s.RegistrantCount = GetRegistrantCount(s.SessionId); return s; });

            var resultSet = items.Skip(pageSize * (pageNumber - 1)).Take(pageSize);

            foreach (var item in resultSet)
            {
                item.Speakers = speakerRepo.GetSpeakersForCollection(item.SessionId, item.CodeCampId);
            }

            return resultSet;
        }

        public void UpdateItemSortOrder(IEnumerable<SessionInfo> sessions, int codeCampId)
        {
            var availableTimeSlots = timeSlotRepo.GetItems(codeCampId).Where(t => !t.SpanAllTracks);

            // re-order timeslots by time only
            foreach (var timeSlot in availableTimeSlots)
            {
                timeSlot.BeginTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, timeSlot.BeginTime.Hour, timeSlot.BeginTime.Minute, 0);
            }
            var timeSlotList = availableTimeSlots.OrderBy(t => t.BeginTime).ToList();

            var index = 0;
            foreach (var session in sessions)
            {
                var timeSlot = timeSlotList[index];

                if (timeSlot != null)
                {
                    session.TimeSlotId = timeSlot.TimeSlotId;
                    repo.UpdateItem(session);
                }

                index++;
            }
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

        private void UpdateSessionWithSpeakers(ref SessionInfo item)
        {
            item.Speakers = speakerRepo.GetSpeakersForCollection(item.SessionId, item.CodeCampId);
        }

        private void SortSessions(ref IEnumerable<SessionInfo> sessions, int codeCampId)
        {
            var availableTimeSlots = timeSlotRepo.GetItems(codeCampId);
            var index = 0;

            foreach (var timeSlot in availableTimeSlots)
            {
                foreach (var session in sessions.Where(session => session.TimeSlotId == timeSlot.TimeSlotId))
                {
                    session.SortOrder = index;
                }

                index++;
            }

            sessions.OrderBy(s => s.SortOrder);
        }

        #endregion
    }
}