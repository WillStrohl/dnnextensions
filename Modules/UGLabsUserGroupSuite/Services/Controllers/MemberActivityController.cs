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
        /// Get all member activities for the module
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// GET: http://dnndev.me/DesktopModules/UserGroupSuite/API/GroupManagement/GetMemberActivities
        /// </remarks>
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.View)]
        [HttpGet]
        public HttpResponseMessage GetMemberActivities()
        {
            try
            {
                return GetMemberActivities(ActiveModule.ModuleID);
            }
            catch (Exception ex)
            {
                Exceptions.LogException(ex);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ERROR_MESSAGE);
            }
        }

        /// <summary>
        /// Get all member activities for the module
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// GET: http://dnndev.me/DesktopModules/UserGroupSuite/API/GroupManagement/GetAddresses
        /// </remarks>
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.View)]
        [HttpGet]
        public HttpResponseMessage GetMemberActivities(int moduleID)
        {
            try
            {
                var memberActivities = MemberActivityDataAccess.GetItems(moduleID);
                var response = new ServiceResponse<List<MemberActivityInfo>> { Content = memberActivities.ToList() };

                return Request.CreateResponse(HttpStatusCode.OK, response.ObjectToJson());
            }
            catch (Exception ex)
            {
                Exceptions.LogException(ex);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ERROR_MESSAGE);
            }
        }

        /// <summary>
        /// Get a member activity
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// GET: http://dnndev.me/DesktopModules/UserGroupSuite/API/GroupManagement/GetMemberActivity
        /// </remarks>
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.View)]
        [HttpGet]
        public HttpResponseMessage GetMemberActivity(int itemId)
        {
            try
            {
                return GetMemberActivity(itemId, ActiveModule.ModuleID);
            }
            catch (Exception ex)
            {
                Exceptions.LogException(ex);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ERROR_MESSAGE);
            }
        }

        /// <summary>
        /// Get a member activity
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// GET: http://dnndev.me/DesktopModules/UserGroupSuite/API/GroupManagement/GetMemberActivity
        /// </remarks>
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.View)]
        [HttpGet]
        public HttpResponseMessage GetMemberActivity(int itemId, int moduleID)
        {
            try
            {
                var memberActivity = MemberActivityDataAccess.GetItem(itemId, moduleID);
                var response = new ServiceResponse<MemberActivityInfo> { Content = memberActivity };

                return Request.CreateResponse(HttpStatusCode.OK, response.ObjectToJson());
            }
            catch (Exception ex)
            {
                Exceptions.LogException(ex);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ERROR_MESSAGE);
            }
        }

        /// <summary>
        /// Delete a member activity
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// DELETE: http://dnndev.me/DesktopModules/UserGroupSuite/API/GroupManagement/DeleteMemberActivity
        /// </remarks>
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.Edit)]
        [ValidateAntiForgeryToken]
        [HttpDelete]
        public HttpResponseMessage DeleteMemberActivity(int itemId)
        {
            try
            {
                return DeleteMemberActivity(itemId, ActiveModule.ModuleID);
            }
            catch (Exception ex)
            {
                Exceptions.LogException(ex);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ERROR_MESSAGE);
            }
        }

        /// <summary>
        /// Delete a member activity
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// DELETE: http://dnndev.me/DesktopModules/UserGroupSuite/API/GroupManagement/DeleteMemberActivity
        /// </remarks>
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.Edit)]
        [ValidateAntiForgeryToken]
        [HttpDelete]
        public HttpResponseMessage DeleteMemberActivity(int itemId, int moduleID)
        {
            try
            {
                MemberActivityDataAccess.DeleteItem(itemId, moduleID);

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
        /// Create a member activity
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// POST: http://dnndev.me/DesktopModules/UserGroupSuite/API/GroupManagement/CreateMemberActivity
        /// </remarks>
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.View)]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public HttpResponseMessage CreateMemberActivity(MemberActivityInfo memberActivity)
        {
            try
            {
                var response = new ServiceResponse<MemberActivityInfo>();

                memberActivity.CreatedOn = DateTime.Now;
                memberActivity.CreatedBy = UserInfo.UserID;
                memberActivity.LastUpdatedOn = DateTime.Now;
                memberActivity.LastUpdatedBy = UserInfo.UserID;
                memberActivity.ModuleID = ActiveModule.ModuleID;

                MemberActivityDataAccess.CreateItem(memberActivity);

                // TODO: Find a more consistent way to do this
                var memberActivities = MemberActivityDataAccess.GetItems(memberActivity.ModuleID).OrderByDescending(r => r.ActivityID);
                var savedMemberActivity = memberActivities.FirstOrDefault(r => r.CreatedBy == memberActivity.CreatedBy);

                response.Content = savedMemberActivity;

                if (savedMemberActivity == null)
                {
                    ServiceResponseHelper<MemberActivityInfo>.AddNoneFoundError("memberActivity", ref response);
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
        /// Update a member activity
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// POST: http://dnndev.me/DesktopModules/UserGroupSuite/API/GroupManagement/UpdateMemberActivity
        /// </remarks>
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.Edit)]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public HttpResponseMessage UpdateMemberActivity(MemberActivityInfo memberActivity)
        {
            try
            {
                var originalMemberActivity = MemberActivityDataAccess.GetItem(memberActivity.ActivityID, memberActivity.ModuleID);
                // only update the fields that would be updated from the UI to keep the DB clean
                var updatesToProcess = MemberActivityHasUpdates(ref originalMemberActivity, ref memberActivity);
                
                if (updatesToProcess)
                {
                    originalMemberActivity.LastUpdatedOn = DateTime.Now;
                    originalMemberActivity.LastUpdatedBy = UserInfo.UserID;

                    MemberActivityDataAccess.UpdateItem(originalMemberActivity);
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

        private bool MemberActivityHasUpdates(ref MemberActivityInfo originalMemberActivity, ref MemberActivityInfo newMemberActivity)
        {
            var updatesToProcess = false;

            // intentionally ignoring MemberID

            if (originalMemberActivity.GroupID != newMemberActivity.GroupID)
            {
                originalMemberActivity.GroupID = newMemberActivity.GroupID;
                updatesToProcess = true;
            }

            if (originalMemberActivity.MeetingID != newMemberActivity.MeetingID)
            {
                originalMemberActivity.MeetingID = newMemberActivity.MeetingID;
                updatesToProcess = true;
            }

            if (!string.Equals(originalMemberActivity.ActivityType, newMemberActivity.ActivityType))
            {
                originalMemberActivity.ActivityType = newMemberActivity.ActivityType;
                updatesToProcess = true;
            }

            if (originalMemberActivity.Score != newMemberActivity.Score)
            {
                originalMemberActivity.Score = newMemberActivity.Score;
                updatesToProcess = true;
            }

            return updatesToProcess;
        }

        #endregion
    }
}