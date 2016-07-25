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

namespace DNNCommunity.Modules.UserGroupSuite.Entities
{
    public class VolunteerTaskInfoController
    {
        private readonly VolunteerTaskInfoRepository repo = null;
        private readonly VolunteerInfoRepository volunteerRepo = null;

        public VolunteerTaskInfoController() 
        {
            repo = new VolunteerTaskInfoRepository();
            volunteerRepo=new VolunteerInfoRepository();
        }

        public void CreateItem(VolunteerTaskInfo i)
        {
            repo.CreateItem(i);
        }

        public void DeleteItem(int itemId, int volunteerId)
        {
            repo.DeleteItem(itemId, volunteerId);
        }

        public void DeleteItem(VolunteerTaskInfo i)
        {
            repo.DeleteItem(i);
        }

        public IEnumerable<VolunteerTaskInfo> GetItems(int volunteerId)
        {
            var items = repo.GetItems(volunteerId);

            return items;
        }

        public List<VolunteerTaskInfo> GetItemsAll(int codeCampId)
        {
            var volunteerIds = volunteerRepo.GetItems(codeCampId).Select(v => v.VolunteerId);
            var items = new List<VolunteerTaskInfo>();

            foreach (var queriedItems in volunteerIds.Select(id => repo.GetItems(id)).Where(queriedItems => queriedItems.Any()))
            {
                items.AddRange(queriedItems);
            }

            return items;
        }

        public VolunteerTaskInfo GetItem(int itemId, int volunteerId)
        {
            var item = repo.GetItem(itemId, volunteerId);

            return item;
        }

        public void UpdateItem(VolunteerTaskInfo i)
        {
            repo.UpdateItem(i);
        }

        public int GetVolunteerTaskCount(int volunteerId, string taskState)
        {
            var tasks = repo.GetItems(volunteerId);
            var count = 0;

            switch (taskState)
            {
                case "open":
                    count = tasks.Count(t => !t.Completed);
                    break;
                case "closed":
                    count = tasks.Count(t => t.Completed);
                    break;
                case "overdue":
                    count = tasks.Count(t => t.EndDate < DateTime.Now && !t.Completed);
                    break;
            }

            return count;
        }
    }
}