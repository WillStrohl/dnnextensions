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
using DotNetNuke.Common.Utilities;
using DotNetNuke.Services.Exceptions;

namespace DNNCommunity.Modules.UserGroupSuite.Entities
{
    public class MeetingInfoController
    {
        private readonly MeetingInfoRepository _repo = null;

        public MeetingInfoController() 
        {
            _repo = new MeetingInfoRepository();
        }

        public void CreateItem(MeetingInfo i)
        {
            ValidateMeetingObject(i);

            _repo.CreateItem(i);

            MarkGroupUpdated(i);
        }

        public void DeleteItem(int itemID, int groupID)
        {
            Requires.NotNegative("itemID", itemID);
            Requires.NotNegative("groupID", groupID);

            _repo.DeleteItem(itemID, groupID);
        }

        public void DeleteItem(MeetingInfo i)
        {
            Requires.NotNull("i", i);
            Requires.PropertyNotNegative(i.MeetingID, "MeetingID");
            Requires.PropertyNotNegative(i.GroupID, "GroupID");

            _repo.DeleteItem(i);
        }

        public IEnumerable<MeetingInfo> GetItems(int groupID)
        {
            Requires.NotNegative("groupID", groupID);

            var items = _repo.GetItems(groupID);
            return items;
        }

        public MeetingInfo GetItem(int itemID, int groupID)
        {
            Requires.NotNegative("itemID", itemID);
            Requires.NotNegative("groupID", groupID);

            var item = _repo.GetItem(itemID, groupID);
            return item;
        }

        public void UpdateItem(MeetingInfo i)
        {
            ValidateMeetingObject(i, true);

            _repo.UpdateItem(i);

            MarkGroupUpdated(i);
        }

        #region Helper Methods

        private void ValidateMeetingObject(MeetingInfo i, bool checkPrimaryKey = false)
        {
            Requires.NotNull("i", i);

            if (checkPrimaryKey)
            {
                Requires.PropertyNotNegative(i.MeetingID, "MeetingID");
            }

            Requires.PropertyNotNegative(i.CreatedBy, "CreatedBy");
            Requires.NotNull("CreatedOn", i.CreatedOn);
            Requires.PropertyNotNullOrEmpty(i.Description, "Description");
            Requires.PropertyNotNegative(i.GroupID, "GroupID");
            Requires.NotNull("HeldOn", i.HeldOn);
            Requires.PropertyNotNegative(i.LastUpdatedBy, "LastUpdatedBy");
            Requires.NotNull("LastUpdatedOn", i.LastUpdatedOn);
            Requires.PropertyNotNegative(i.ModuleID, "ModuleID");
            Requires.PropertyNotNullOrEmpty(i.Slug, "Slug");
            Requires.PropertyNotNullOrEmpty(i.Title, "Title");
        }

        private void MarkGroupUpdated(MeetingInfo meeting)
        {
            try
            {
                var ctlGroup = new GroupInfoController();
                var group = ctlGroup.GetItem(meeting.GroupID, meeting.ModuleID);

                if (meeting.MeetingID == Null.NullInteger)
                {
                    group.LastUpdatedType = (int) GroupUpdateType.MeetingAdded;
                }
                else
                {
                    var originalMeeting = GetItem(meeting.MeetingID, meeting.GroupID);

                    if (originalMeeting.VirtualAddressID != meeting.VirtualAddressID ||
                        originalMeeting.PhysicalAddressID != meeting.PhysicalAddressID)
                    {
                        group.LastUpdatedType = (int) GroupUpdateType.LocationChanged;
                    }
                    else
                    {
                        group.LastUpdatedType = (int) GroupUpdateType.MeetingUpdated;
                    }
                }

                group.LastUpdatedBy = meeting.LastUpdatedBy;
                group.LastUpdatedOn = meeting.LastUpdatedOn;

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