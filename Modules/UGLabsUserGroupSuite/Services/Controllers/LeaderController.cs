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
        /// Get all leaders for the group
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// GET: http://dnndev.me/DesktopModules/UserGroupSuite/API/GroupManagement/GetLeaders
        /// </remarks>
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.View)]
        [HttpGet]
        public HttpResponseMessage GetLeaders(int groupID)
        {
            try
            {
                var leaders = LeaderDataAccess.GetItems(groupID);
                var response = new ServiceResponse<List<LeaderInfo>> { Content = leaders.ToList() };

                return Request.CreateResponse(HttpStatusCode.OK, response.ObjectToJson());
            }
            catch (Exception ex)
            {
                Exceptions.LogException(ex);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ERROR_MESSAGE);
            }
        }

        /// <summary>
        /// Get a leader
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// GET: http://dnndev.me/DesktopModules/UserGroupSuite/API/GroupManagement/GetLeader
        /// </remarks>
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.View)]
        [HttpGet]
        public HttpResponseMessage GetLeader(int itemId, int groupID)
        {
            try
            {
                var leader = LeaderDataAccess.GetItem(itemId, groupID);
                var response = new ServiceResponse<LeaderInfo> { Content = leader };

                return Request.CreateResponse(HttpStatusCode.OK, response.ObjectToJson());
            }
            catch (Exception ex)
            {
                Exceptions.LogException(ex);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ERROR_MESSAGE);
            }
        }

        /// <summary>
        /// Delete a leader
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// DELETE: http://dnndev.me/DesktopModules/UserGroupSuite/API/GroupManagement/DeleteLeader
        /// </remarks>
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.Edit)]
        [ValidateAntiForgeryToken]
        [HttpDelete]
        public HttpResponseMessage DeleteLeader(int itemId, int groupID)
        {
            try
            {
                LeaderDataAccess.DeleteItem(itemId, groupID);

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
        /// Create a leader
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// POST: http://dnndev.me/DesktopModules/UserGroupSuite/API/GroupManagement/CreateLeader
        /// </remarks>
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.View)]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public HttpResponseMessage CreateLeader(LeaderInfo leader)
        {
            try
            {
                var response = new ServiceResponse<LeaderInfo>();

                leader.CreatedOn = DateTime.Now;
                leader.CreatedBy = UserInfo.UserID;
                leader.LastUpdatedOn = DateTime.Now;
                leader.LastUpdatedBy = UserInfo.UserID;

                LeaderDataAccess.CreateItem(leader);

                // TODO: Find a more consistent way to do this
                var leaders = LeaderDataAccess.GetItems(leader.GroupID).OrderByDescending(r => r.GroupLeaderID);
                var savedLeader = leaders.FirstOrDefault(r => r.CreatedBy == leader.CreatedBy);

                response.Content = savedLeader;

                if (savedLeader == null)
                {
                    ServiceResponseHelper<LeaderInfo>.AddNoneFoundError("leader", ref response);
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
        /// Update a leader
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// POST: http://dnndev.me/DesktopModules/UserGroupSuite/API/GroupManagement/UpdateLeader
        /// </remarks>
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.View)]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public HttpResponseMessage UpdateLeader(LeaderInfo leader)
        {
            try
            {
                var originalLeader = LeaderDataAccess.GetItem(leader.GroupLeaderID, leader.GroupID);
                // only update the fields that would be updated from the UI to keep the DB clean
                var updatesToProcess = LeaderHasUpdates(ref originalLeader, ref leader);
                
                if (updatesToProcess)
                {
                    originalLeader.LastUpdatedOn = DateTime.Now;
                    originalLeader.LastUpdatedBy = UserInfo.UserID;

                    LeaderDataAccess.UpdateItem(originalLeader);
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

        private bool LeaderHasUpdates(ref LeaderInfo originalLeader, ref LeaderInfo newLeader)
        {
            var updatesToProcess = false;

            if (originalLeader.MemberID != newLeader.MemberID)
            {
                originalLeader.MemberID = newLeader.MemberID;
                updatesToProcess = true;
            }

            if (!string.Equals(originalLeader.Title, newLeader.Title))
            {
                originalLeader.Title = newLeader.Title;
                updatesToProcess = true;
            }

            if (originalLeader.IsPrimary != newLeader.IsPrimary)
            {
                originalLeader.IsPrimary = newLeader.IsPrimary;
                updatesToProcess = true;
            }

            return updatesToProcess;
        }

        #endregion
    }
}