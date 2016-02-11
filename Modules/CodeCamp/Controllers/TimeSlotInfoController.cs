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
    public class TimeSlotInfoController
    {
        private readonly TimeSlotInfoRepository repo = null;

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

        #region Private Helper Methods

        //private void ConvertTimeSlotTimes(ref IEnumerable<TimeSlotInfo> timeSlots)
        //{
        //    if (!timeSlots.Any()) return;

        //    foreach (var timeSlot in timeSlots)
        //    {
        //        timeSlot.BeginTime = timeSlot.BeginTime.ToLocalTime();
        //        timeSlot.EndTime = timeSlot.EndTime.ToLocalTime();
        //    }
        //}

        private void SortTimeSlots(ref IEnumerable<TimeSlotInfo> timeSlots)
        {
            if (!timeSlots.Any()) return;
            
            var index = 0;

            foreach (var timeSlot in timeSlots.OrderBy(t => t.BeginTime.ToLocalTime()))
            {
                //var beginTime = timeSlot.BeginTime.ToLocalTime();
                //var endTime = timeSlot.EndTime.ToLocalTime();
                //var today = DateTime.Now;

                //timeSlot.BeginTime = new DateTime(today.Year, today.Month, today.Day, beginTime.Hour, beginTime.Minute, 0);
                //timeSlot.EndTime = new DateTime(today.Year, today.Month, today.Day, endTime.Hour, endTime.Minute, 0);

                timeSlot.SortOrder = index;

                index++;
            }
        }

        #endregion
    }
}