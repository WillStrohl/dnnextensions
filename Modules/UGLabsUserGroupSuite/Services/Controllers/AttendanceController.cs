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
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.UI;
using DotNetNuke.Entities.Users;
using DotNetNuke.Security;
using DotNetNuke.Security.Membership;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Web.Api;
using DNNCommunity.Modules.UserGroupSuite.Components;
using DNNCommunity.Modules.UserGroupSuite.Controllers;
using DNNCommunity.Modules.UserGroupSuite.Entities;

namespace DNNCommunity.Modules.UserGroupSuite.Services
{
    public partial class GroupManagementController
    {
        /// <summary>
        /// Get all attendances for the module
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// GET: http://dnndev.me/DesktopModules/UserGroupSuite/API/GroupManagement/GetAttendances
        /// </remarks>
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.View)]
        [HttpGet]
        public HttpResponseMessage GetAttendances(int meetingID)
        {
            try
            {
                var attendances = AttendanceDataAccess.GetItems(meetingID);
                var response = new ServiceResponse<List<AttendanceInfo>> { Content = attendances.ToList() };

                return Request.CreateResponse(HttpStatusCode.OK, response.ObjectToJson());
            }
            catch (Exception ex)
            {
                Exceptions.LogException(ex);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ERROR_MESSAGE);
            }
        }

        /// <summary>
        /// Get an attendance
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// GET: http://dnndev.me/DesktopModules/UserGroupSuite/API/GroupManagement/GetAttendance
        /// </remarks>
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.View)]
        [HttpGet]
        public HttpResponseMessage GetAttendance(int itemId, int meetingID)
        {
            try
            {
                var attendance = AttendanceDataAccess.GetItem(itemId, meetingID);
                var response = new ServiceResponse<AttendanceInfo> { Content = attendance };

                return Request.CreateResponse(HttpStatusCode.OK, response.ObjectToJson());
            }
            catch (Exception ex)
            {
                Exceptions.LogException(ex);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ERROR_MESSAGE);
            }
        }

        /// <summary>
        /// Delete an attendance
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// DELETE: http://dnndev.me/DesktopModules/UserGroupSuite/API/GroupManagement/DeleteAttendance
        /// </remarks>
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.Edit)]
        [ValidateAntiForgeryToken]
        [HttpDelete]
        public HttpResponseMessage DeleteAttendance(int itemId, int meetingID)
        {
            try
            {
                AttendanceDataAccess.DeleteItem(itemId, meetingID);

                var response = new ServiceResponse<string> { Content = SUCCESS_MESSAGE };

                return Request.CreateResponse(HttpStatusCode.OK, response.ObjectToJson());
            }
            catch (Exception ex)
            {
                Exceptions.LogException(ex);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ERROR_MESSAGE);
            }
        }

        /// <summary>
        /// Create an attendance
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// POST: http://dnndev.me/DesktopModules/UserGroupSuite/API/GroupManagement/CreateAttendance
        /// </remarks>
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.View)]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public HttpResponseMessage CreateAttendance(AttendanceInfo attendance)
        {
            try
            {
                var response = new ServiceResponse<AttendanceInfo>();

                attendance.CreatedOn = DateTime.Now;
                attendance.CreatedBy = UserInfo.UserID;
                attendance.LastUpdatedOn = DateTime.Now;
                attendance.LastUpdatedBy = UserInfo.UserID;

                AttendanceDataAccess.CreateItem(attendance);

                // TODO: Find a more consistent way to do this
                var attendances = AttendanceDataAccess.GetItems(attendance.MeetingID).OrderByDescending(r => r.AttendanceID);
                var savedAttendance = attendances.FirstOrDefault(r => r.CreatedBy == attendance.CreatedBy);

                response.Content = savedAttendance;

                if (savedAttendance == null)
                {
                    ServiceResponseHelper<AttendanceInfo>.AddNoneFoundError("attendance", ref response);
                }

                return Request.CreateResponse(HttpStatusCode.OK, response.ObjectToJson());
            }
            catch (Exception ex)
            {
                Exceptions.LogException(ex);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ERROR_MESSAGE);
            }
        }

        /// <summary>
        /// Update an attendance
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// POST: http://dnndev.me/DesktopModules/UserGroupSuite/API/GroupManagement/UpdateAttendance
        /// </remarks>
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.View)]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public HttpResponseMessage UpdateAttendance(AttendanceInfo attendance)
        {
            try
            {
                var originalAttendance = AttendanceDataAccess.GetItem(attendance.AttendanceID, attendance.MeetingID);
                // only update the fields that would be updated from the UI to keep the DB clean
                var updatesToProcess = AttendanceHasUpdates(ref originalAttendance, ref attendance);
                
                if (updatesToProcess)
                {
                    originalAttendance.LastUpdatedOn = DateTime.Now;
                    originalAttendance.LastUpdatedBy = UserInfo.UserID;

                    AttendanceDataAccess.UpdateItem(originalAttendance);
                }

                var response = new ServiceResponse<string> { Content = SUCCESS_MESSAGE };

                return Request.CreateResponse(HttpStatusCode.OK, response.ObjectToJson());
            }
            catch (Exception ex)
            {
                Exceptions.LogException(ex);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ERROR_MESSAGE);
            }
        }

        #region Private Helper Methods

        private bool AttendanceHasUpdates(ref AttendanceInfo originalAttendance, ref AttendanceInfo newAttendance)
        {
            var updatesToProcess = false;

            // left out MemberID intentionally

            if (originalAttendance.AttendOnline != newAttendance.AttendOnline)
            {
                originalAttendance.AttendOnline = newAttendance.AttendOnline;
                updatesToProcess = true;
            }

            if (originalAttendance.AttendInPerson != newAttendance.AttendInPerson)
            {
                originalAttendance.AttendInPerson = newAttendance.AttendInPerson;
                updatesToProcess = true;
            }

            if (originalAttendance.Attended != newAttendance.Attended)
            {
                originalAttendance.Attended = newAttendance.Attended;
                updatesToProcess = true;
            }

            return updatesToProcess;
        }

        #endregion
    }
}