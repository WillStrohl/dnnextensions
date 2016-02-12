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

using System;
using System.Collections.Generic;
using System.Linq;

namespace WillStrohl.Modules.CodeCamp.Entities
{
    public class TimeSlotInfoController
    {
        #region Private Properties

        private readonly TimeSlotInfoRepository repo = null;

        #endregion

        public TimeSlotInfoController() 
        {
            repo = new TimeSlotInfoRepository();
        }

        public void CreateItem(TimeSlotInfo i)
        {
            repo.CreateItem(i);
        }

        public void DeleteItem(int itemId, int codeCampId)
        {
            repo.DeleteItem(itemId, codeCampId);
        }

        public void DeleteItem(TimeSlotInfo i)
        {
            repo.DeleteItem(i);
        }

        public IEnumerable<TimeSlotInfo> GetItems(int codeCampId)
        {
            var items = repo.GetItems(codeCampId);

            // re-order timeslots by time only
            //ConvertTimeSlotTimes(ref items);
            //SortTimeSlots(ref items);

            return items;
        }

        public TimeSlotInfo GetItem(int itemId, int codeCampId)
        {
            var item = repo.GetItem(itemId, codeCampId);

            return item;
        }

        public void UpdateItem(TimeSlotInfo i)
        {
            repo.UpdateItem(i);
        }

        #region Static Helper Methods
        
        public static IEnumerable<TimeSlotInfo> SortTimeSlots(IEnumerable<TimeSlotInfo> timeSlots)
        {
            var index = 0;

            // first, ensure that the times all have the same dates
            foreach (var timeSlot in timeSlots)
            {
                var beginTime = timeSlot.BeginTime;
                var endTime = timeSlot.EndTime;

                timeSlot.BeginTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, beginTime.Hour, beginTime.Minute, 0);
                timeSlot.EndTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, endTime.Hour, endTime.Minute, 0);
            }

            // now sort by the time
            foreach (var timeSlot in timeSlots.OrderBy(t => t.BeginTime))
            {
                timeSlot.SortOrder = index;

                index++;
            }

            return timeSlots;
        }

        #endregion
    }
}