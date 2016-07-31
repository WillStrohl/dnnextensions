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
using DotNetNuke.Security;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Web.Api;
using DNNCommunity.Modules.UserGroupSuite.Components;
using DNNCommunity.Modules.UserGroupSuite.Entities;

namespace DNNCommunity.Modules.UserGroupSuite.Services
{
    public partial class GroupManagementController
    {
        /// <summary>
        /// Get all meetings for the group
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// GET: http://dnndev.me/DesktopModules/UserGroupSuite/API/GroupManagement/GetMeetings
        /// </remarks>
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.View)]
        [HttpGet]
        public HttpResponseMessage GetMeetings(int groupID)
        {
            try
            {
                var meetings = MeetingDataAccess.GetItems(groupID);
                var response = new ServiceResponse<List<MeetingInfo>> { Content = meetings.ToList() };

                return Request.CreateResponse(HttpStatusCode.OK, response.ObjectToJson());
            }
            catch (Exception ex)
            {
                Exceptions.LogException(ex);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ERROR_MESSAGE);
            }
        }

        /// <summary>
        /// Get a meeting
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// GET: http://dnndev.me/DesktopModules/UserGroupSuite/API/GroupManagement/GetMeeting
        /// </remarks>
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.View)]
        [HttpGet]
        public HttpResponseMessage GetMeeting(int itemId, int groupID)
        {
            try
            {
                var meeting = MeetingDataAccess.GetItem(itemId, groupID);
                var response = new ServiceResponse<MeetingInfo> { Content = meeting };

                return Request.CreateResponse(HttpStatusCode.OK, response.ObjectToJson());
            }
            catch (Exception ex)
            {
                Exceptions.LogException(ex);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ERROR_MESSAGE);
            }
        }

        /// <summary>
        /// Delete a meeting
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// DELETE: http://dnndev.me/DesktopModules/UserGroupSuite/API/GroupManagement/DeleteMeeting
        /// </remarks>
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.Edit)]
        [ValidateAntiForgeryToken]
        [HttpDelete]
        public HttpResponseMessage DeleteMeeting(int itemId, int groupID)
        {
            try
            {
                MeetingDataAccess.DeleteItem(itemId, groupID);

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
        /// Create a meeting
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// POST: http://dnndev.me/DesktopModules/UserGroupSuite/API/GroupManagement/CreateMeeting
        /// </remarks>
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.View)]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public HttpResponseMessage CreateMeeting(MeetingInfo meeting)
        {
            try
            {
                var response = new ServiceResponse<MeetingInfo>();

                meeting.CreatedOn = DateTime.Now;
                meeting.CreatedBy = UserInfo.UserID;
                meeting.LastUpdatedOn = DateTime.Now;
                meeting.LastUpdatedBy = UserInfo.UserID;

                MeetingDataAccess.CreateItem(meeting);

                // TODO: Find a more consistent way to do this
                var meetings = MeetingDataAccess.GetItems(meeting.GroupID).OrderByDescending(r => r.MeetingID);
                var savedMeeting = meetings.FirstOrDefault(r => r.CreatedBy == meeting.CreatedBy);

                response.Content = savedMeeting;

                if (savedMeeting == null)
                {
                    ServiceResponseHelper<MeetingInfo>.AddNoneFoundError("meeting", ref response);
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
        /// Update a meeting
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// POST: http://dnndev.me/DesktopModules/UserGroupSuite/API/GroupManagement/UpdateMeeting
        /// </remarks>
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.View)]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public HttpResponseMessage UpdateMeeting(MeetingInfo meeting)
        {
            try
            {
                var originalMeeting = MeetingDataAccess.GetItem(meeting.MeetingID, meeting.GroupID);
                // only update the fields that would be updated from the UI to keep the DB clean
                var updatesToProcess = MeetingHasUpdates(ref originalMeeting, ref meeting);
                
                if (updatesToProcess)
                {
                    originalMeeting.LastUpdatedOn = DateTime.Now;
                    originalMeeting.LastUpdatedBy = UserInfo.UserID;

                    MeetingDataAccess.UpdateItem(originalMeeting);
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

        private bool MeetingHasUpdates(ref MeetingInfo originalMeeting, ref MeetingInfo newMeeting)
        {
            var updatesToProcess = false;

            if (!string.Equals(originalMeeting.Title, newMeeting.Title))
            {
                originalMeeting.Title = newMeeting.Title;
                updatesToProcess = true;
            }

            if (!string.Equals(originalMeeting.Description, newMeeting.Description))
            {
                originalMeeting.Description = newMeeting.Description;
                updatesToProcess = true;
            }

            if (originalMeeting.HeldOn != newMeeting.HeldOn)
            {
                originalMeeting.HeldOn = newMeeting.HeldOn;
                updatesToProcess = true;
            }

            if (originalMeeting.PhysicalAddressID != newMeeting.PhysicalAddressID)
            {
                originalMeeting.PhysicalAddressID = newMeeting.PhysicalAddressID;
                updatesToProcess = true;
            }

            if (originalMeeting.VirtualAddressID != newMeeting.VirtualAddressID)
            {
                originalMeeting.VirtualAddressID = newMeeting.VirtualAddressID;
                updatesToProcess = true;
            }

            if (originalMeeting.IsActive != newMeeting.IsActive)
            {
                originalMeeting.IsActive = newMeeting.IsActive;
                updatesToProcess = true;
            }

            if (!string.Equals(originalMeeting.Slug, newMeeting.Slug))
            {
                originalMeeting.Slug = newMeeting.Slug;
                updatesToProcess = true;
            }

            if (originalMeeting.CustomProperties != null)
            {
                // parse custom properties for updates
                foreach (var property in originalMeeting.CustomPropertiesObj)
                {
                    if (newMeeting.CustomPropertiesObj.Any(p => p.Name == property.Name))
                    {
                        // see if the existing property needs to be updated
                        var prop = newMeeting.CustomPropertiesObj.FirstOrDefault(p => p.Name == property.Name);
                        if (!string.Equals(prop.Value, property.Value))
                        {
                            property.Value = prop.Value;
                            updatesToProcess = true;
                        }
                    }
                    else
                    {
                        // delete the property
                        newMeeting.CustomPropertiesObj.Remove(property);
                        updatesToProcess = true;
                    }
                }
            }

            if (newMeeting.CustomPropertiesObj != null)
            {
                // add any new properties
                if (originalMeeting.CustomProperties == null)
                {
                    foreach (var property in newMeeting.CustomPropertiesObj)
                    {
                        originalMeeting.CustomPropertiesObj.Add(property);
                        updatesToProcess = true;
                    }
                }
                else
                {
                    var meeting = originalMeeting;
                    foreach (var property in newMeeting.CustomPropertiesObj.Where(property => !meeting.CustomPropertiesObj.Contains(property)))
                    {
                        meeting.CustomPropertiesObj.Add(property);
                        updatesToProcess = true;
                    }
                }
            }

            return updatesToProcess;
        }

        #endregion
    }
}