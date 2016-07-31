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
using DotNetNuke.Common;

namespace DNNCommunity.Modules.UserGroupSuite.Entities
{
    public class GroupInfoController
    {
        private readonly GroupInfoRepository _repo = null;

        public GroupInfoController() 
        {
            _repo = new GroupInfoRepository();
        }

        public void CreateItem(GroupInfo i)
        {
            ValidateGroupObject(i);

            i.LastUpdatedType = (int) GroupUpdateType.New;

            _repo.CreateItem(i);
        }

        public void DeleteItem(int itemID, int moduleID)
        {
            Requires.NotNegative("itemID", itemID);
            Requires.NotNegative("moduleID", moduleID);

            _repo.DeleteItem(itemID, moduleID);
        }

        public void DeleteItem(GroupInfo i)
        {
            Requires.NotNull("i", i);
            Requires.PropertyNotNegative(i.GroupID, "GroupID");
            Requires.PropertyNotNegative(i.ModuleID, "ModuleID");

            _repo.DeleteItem(i);
        }

        public IEnumerable<GroupInfo> GetItems(int moduleID)
        {
            Requires.NotNegative("moduleID", moduleID);

            var items = _repo.GetItems(moduleID);
            return items;
        }

        public IEnumerable<GroupInfo> GetItemsUpcoming()
        {
            var items = _repo.GetItemsWithUpcomingMeetings();
            return items;
        }

        public GroupInfo GetItem(int itemID, int moduleID)
        {
            Requires.NotNegative("itemID", itemID);
            Requires.NotNegative("moduleID", moduleID);

            var item = _repo.GetItem(itemID, moduleID);
            return item;
        }

        public void UpdateItem(GroupInfo i)
        {
            ValidateGroupObject(i, true);

            i.LastUpdatedType = (int) DetermineUpdateType(i);

            _repo.UpdateItem(i);
        }

        public IEnumerable<GroupInfo> GetItemsByUser(int userID, int moduleID)
        {
            Requires.NotNegative("userID", userID);
            Requires.NotNegative("moduleID", moduleID);

            var ctlMember = new MemberInfoController();
            var members = ctlMember.GetItems(userID);

            var groups = members.Select(member => GetItem(member.GroupID, moduleID)).ToList();

            return groups;
        } 

        #region Helper Methods

        private void ValidateGroupObject(GroupInfo i, bool checkPrimaryKey = false)
        {
            Requires.NotNull("i", i);

            if (checkPrimaryKey)
            {
                Requires.PropertyNotNegative(i.GroupID, "GroupID");
            }

            Requires.PropertyNotNegative(i.ModuleID, "ModuleID");
            Requires.PropertyNotNullOrEmpty(i.GroupName, "GroupName");
            Requires.PropertyNotNullOrEmpty(i.Avatar, "ModuleID");
            Requires.PropertyNotNullOrEmpty(i.City, "City");
            Requires.PropertyNotNegative(i.CountryID, "City");
            Requires.PropertyNotNegative(i.CreatedBy, "CreatedBy");
            Requires.NotNull("CreatedOn", i.CreatedOn);
            Requires.PropertyNotNegative(i.LastUpdatedType, "LastUpdatedType");
            Requires.PropertyNotNegative(i.LastUpdatedBy, "LastUpdatedBy");
            Requires.NotNull("LastUpdatedOn", i.LastUpdatedOn);
            Requires.PropertyNotNullOrEmpty(i.Description, "Description");
            Requires.PropertyNotNegative(i.LanguageID, "LanguageID");
            Requires.PropertyNotNullOrEmpty(i.Slug, "Slug");
            Requires.PropertyNotNullOrEmpty(i.Website, "Website");
            Requires.PropertyNotNullOrEmpty(i.GroupName, "GroupName");
        }

        private GroupUpdateType DetermineUpdateType(GroupInfo updatedGroup)
        {
            var originalGroup = GetItem(updatedGroup.GroupID, updatedGroup.ModuleID);

            if (!string.Equals(originalGroup.City, updatedGroup.City) ||
                originalGroup.RegionID != updatedGroup.RegionID ||
                originalGroup.CountryID != updatedGroup.CountryID)
            {
                return GroupUpdateType.LocationChanged;
            }

            return GroupUpdateType.Profile;
        }

        #endregion
    }
}