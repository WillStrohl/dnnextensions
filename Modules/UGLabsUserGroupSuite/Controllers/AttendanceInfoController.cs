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
using DotNetNuke.Common;

namespace DNNCommunity.Modules.UserGroupSuite.Entities
{
    public class AttendanceInfoController
    {
        private readonly AttendanceInfoRepository _repo = null;

        public AttendanceInfoController() 
        {
            _repo = new AttendanceInfoRepository();
        }

        public void CreateItem(AttendanceInfo i)
        {
            ValidateAttendanceObject(i);

            _repo.CreateItem(i);
        }

        public void DeleteItem(int itemID, int meetingID)
        {
            Requires.NotNegative("itemID", itemID);
            Requires.NotNegative("meetingID", meetingID);

            _repo.DeleteItem(itemID, meetingID);
        }

        public void DeleteItem(AttendanceInfo i)
        {
            Requires.NotNull("i", i);
            Requires.PropertyNotNegative(i.AttendanceID, "AttendanceID");
            Requires.PropertyNotNegative(i.MeetingID, "meetingID");

            _repo.DeleteItem(i);
        }

        public IEnumerable<AttendanceInfo> GetItems(int meetingID)
        {
            Requires.NotNegative("meetingID", meetingID);

            var items = _repo.GetItems(meetingID);
            return items;
        }

        public AttendanceInfo GetItem(int itemID, int meetingID)
        {
            Requires.NotNegative("itemID", itemID);

            var item = _repo.GetItem(itemID, meetingID);
            return item;
        }

        public void UpdateItem(AttendanceInfo i)
        {
            ValidateAttendanceObject(i, true);

            _repo.UpdateItem(i);
        }

        #region Helper Methods

        private void ValidateAttendanceObject(AttendanceInfo i, bool checkPrimaryKey = false)
        {
            Requires.NotNull("i", i);

            if (checkPrimaryKey)
            {
                Requires.PropertyNotNegative(i.AttendanceID, "AddressID");
            }

            Requires.PropertyNotNegative(i.AttendanceIntent, "AttendanceIntent");
            Requires.PropertyNotNegative(i.CreatedBy, "CreatedBy");
            Requires.NotNull("CreatedOn", i.CreatedOn);
            Requires.PropertyNotNegative(i.LastUpdatedBy, "LastUpdatedBy");
            Requires.NotNull("LastUpdatedOn", i.LastUpdatedOn);
            Requires.PropertyNotNegative(i.MeetingID, "MeetingID");
            Requires.PropertyNotNegative(i.MemberID, "MemberID");
        }

        #endregion
    }
}