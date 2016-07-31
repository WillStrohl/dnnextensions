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
using DotNetNuke.Common;
using DotNetNuke.Services.Exceptions;

namespace DNNCommunity.Modules.UserGroupSuite.Entities
{
    public class LeaderInfoController
    {
        private readonly LeaderInfoRepository _repo = null;

        public LeaderInfoController() 
        {
            _repo = new LeaderInfoRepository();
        }

        public void CreateItem(LeaderInfo i)
        {
            ValidateLeaderObject(i);

            _repo.CreateItem(i);

            MarkGroupUpdated(i);
        }

        public void DeleteItem(int itemId, int groupID)
        {
            _repo.DeleteItem(itemId, groupID);
        }

        public void DeleteItem(LeaderInfo i)
        {
            _repo.DeleteItem(i);
        }

        public IEnumerable<LeaderInfo> GetItems(int groupID)
        {
            var items = _repo.GetItems(groupID);
            return items;
        }

        public LeaderInfo GetItem(int itemId, int groupID)
        {
            var item = _repo.GetItem(itemId, groupID);
            return item;
        }

        public void UpdateItem(LeaderInfo i)
        {
            ValidateLeaderObject(i, true);

            _repo.UpdateItem(i);

            MarkGroupUpdated(i);
        }

        #region Helper Methods

        private void ValidateLeaderObject(LeaderInfo i, bool checkPrimaryKey = false)
        {
            Requires.NotNull("i", i);

            if (checkPrimaryKey)
            {
                Requires.PropertyNotNegative(i.GroupLeaderID, "GroupLeaderID");
            }

            Requires.PropertyNotNullOrEmpty(i.Title, "Title");
            Requires.PropertyNotNegative(i.CreatedBy, "CreatedBy");
            Requires.NotNull("CreatedOn", i.CreatedOn);
            Requires.PropertyNotNegative(i.GroupID, "GroupID");
            Requires.PropertyNotNegative(i.LastUpdatedBy, "LastUpdatedBy");
            Requires.NotNull("LastUpdatedOn", i.LastUpdatedOn);
            Requires.PropertyNotNegative(i.MemberID, "MemberID");
            Requires.PropertyNotNegative(i.ModuleID, "ModuleID");
        }

        private void MarkGroupUpdated(LeaderInfo leader)
        {
            try
            {
                var ctlGroup = new GroupInfoController();
                var group = ctlGroup.GetItem(leader.GroupID, leader.ModuleID);

                group.LastUpdatedType = (int)GroupUpdateType.Leadership;
                group.LastUpdatedBy = leader.LastUpdatedBy;
                group.LastUpdatedOn = leader.LastUpdatedOn;

                ctlGroup.UpdateItem(group);
            }
            catch (Exception ex)
            {
                Exceptions.LogException(ex);
            }
        }

        #endregion
    }
}