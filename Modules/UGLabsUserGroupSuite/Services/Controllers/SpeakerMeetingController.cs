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
        /// Get all speaker meetings for the meeting
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// GET: http://dnndev.me/DesktopModules/UserGroupSuite/API/GroupManagement/GetSpeakerMeetings
        /// </remarks>
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.View)]
        [HttpGet]
        public HttpResponseMessage GetSpeakerMeetings(int meetingID)
        {
            try
            {
                var speakerMeetings = SpeakerMeetingDataAccess.GetItems(meetingID);
                var response = new ServiceResponse<List<SpeakerMeetingInfo>> { Content = speakerMeetings.ToList() };

                return Request.CreateResponse(HttpStatusCode.OK, response.ObjectToJson());
            }
            catch (Exception ex)
            {
                Exceptions.LogException(ex);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ERROR_MESSAGE);
            }
        }

        /// <summary>
        /// Get a speaker meeting
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// GET: http://dnndev.me/DesktopModules/UserGroupSuite/API/GroupManagement/GetSpeakerMeeting
        /// </remarks>
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.View)]
        [HttpGet]
        public HttpResponseMessage GetSpeakerMeeting(int itemId, int meetingID)
        {
            try
            {
                var speakerMeeting = SpeakerMeetingDataAccess.GetItem(itemId, meetingID);
                var response = new ServiceResponse<SpeakerMeetingInfo> { Content = speakerMeeting };

                return Request.CreateResponse(HttpStatusCode.OK, response.ObjectToJson());
            }
            catch (Exception ex)
            {
                Exceptions.LogException(ex);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ERROR_MESSAGE);
            }
        }

        /// <summary>
        /// Delete a speaker meeting
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// DELETE: http://dnndev.me/DesktopModules/UserGroupSuite/API/GroupManagement/DeleteSpeakerMeeting
        /// </remarks>
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.Edit)]
        [ValidateAntiForgeryToken]
        [HttpDelete]
        public HttpResponseMessage DeleteSpeakerMeeting(int itemId, int meetingID)
        {
            try
            {
                SpeakerMeetingDataAccess.DeleteItem(itemId, meetingID);

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
        /// Create a speaker meeting
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// POST: http://dnndev.me/DesktopModules/UserGroupSuite/API/GroupManagement/CreateSpeakerMeeting
        /// </remarks>
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.View)]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public HttpResponseMessage CreateSpeakerMeeting(SpeakerMeetingInfo speakerMeeting)
        {
            try
            {
                var response = new ServiceResponse<SpeakerMeetingInfo>();

                speakerMeeting.CreatedOn = DateTime.Now;
                speakerMeeting.CreatedBy = UserInfo.UserID;
                speakerMeeting.LastUpdatedOn = DateTime.Now;
                speakerMeeting.LastUpdatedBy = UserInfo.UserID;

                SpeakerMeetingDataAccess.CreateItem(speakerMeeting);

                // TODO: Find a more consistent way to do this
                var speakerMeetings = SpeakerMeetingDataAccess.GetItems(speakerMeeting.MeetingID).OrderByDescending(r => r.SpeakerMeetingID);
                var savedSpeakerMeeting = speakerMeetings.FirstOrDefault(r => r.CreatedBy == speakerMeeting.CreatedBy);

                response.Content = savedSpeakerMeeting;

                if (savedSpeakerMeeting == null)
                {
                    ServiceResponseHelper<SpeakerMeetingInfo>.AddNoneFoundError("speakerMeeting", ref response);
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
        /// Update a speaker meeting
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// POST: http://dnndev.me/DesktopModules/UserGroupSuite/API/GroupManagement/UpdateSpeakerMeeting
        /// </remarks>
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.View)]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public HttpResponseMessage UpdateSpeakerMeeting(SpeakerMeetingInfo speakerMeeting)
        {
            try
            {
                var originalSpeakerMeeting = SpeakerMeetingDataAccess.GetItem(speakerMeeting.SpeakerMeetingID, speakerMeeting.MeetingID);
                // only update the fields that would be updated from the UI to keep the DB clean
                var updatesToProcess = SpeakerMeetingHasUpdates(ref originalSpeakerMeeting, ref speakerMeeting);
                
                if (updatesToProcess)
                {
                    originalSpeakerMeeting.LastUpdatedOn = DateTime.Now;
                    originalSpeakerMeeting.LastUpdatedBy = UserInfo.UserID;

                    SpeakerMeetingDataAccess.UpdateItem(originalSpeakerMeeting);
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

        private bool SpeakerMeetingHasUpdates(ref SpeakerMeetingInfo originalSpeakerMeeting, ref SpeakerMeetingInfo newSpeakerMeeting)
        {
            var updatesToProcess = false;

            if (originalSpeakerMeeting.SpeakerID != newSpeakerMeeting.SpeakerID)
            {
                originalSpeakerMeeting.SpeakerID = newSpeakerMeeting.SpeakerID;
                updatesToProcess = true;
            }

            return updatesToProcess;
        }

        #endregion
    }
}