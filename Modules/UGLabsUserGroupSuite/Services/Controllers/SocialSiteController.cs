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
        /// Get all social sites for the group
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// GET: http://dnndev.me/DesktopModules/UserGroupSuite/API/GroupManagement/GetSocialSites
        /// </remarks>
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.View)]
        [HttpGet]
        public HttpResponseMessage GetSocialSites(int groupID)
        {
            try
            {
                var socialSites = SocialSiteDataAccess.GetItems(groupID);
                var response = new ServiceResponse<List<SocialSiteInfo>> { Content = socialSites.ToList() };

                return Request.CreateResponse(HttpStatusCode.OK, response.ObjectToJson());
            }
            catch (Exception ex)
            {
                Exceptions.LogException(ex);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ERROR_MESSAGE);
            }
        }

        /// <summary>
        /// Get a social site
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// GET: http://dnndev.me/DesktopModules/UserGroupSuite/API/GroupManagement/GetSocialSite
        /// </remarks>
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.View)]
        [HttpGet]
        public HttpResponseMessage GetSocialSite(int itemId, int groupID)
        {
            try
            {
                var socialSite = SocialSiteDataAccess.GetItem(itemId, groupID);
                var response = new ServiceResponse<SocialSiteInfo> { Content = socialSite };

                return Request.CreateResponse(HttpStatusCode.OK, response.ObjectToJson());
            }
            catch (Exception ex)
            {
                Exceptions.LogException(ex);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ERROR_MESSAGE);
            }
        }

        /// <summary>
        /// Delete a social site
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// DELETE: http://dnndev.me/DesktopModules/UserGroupSuite/API/GroupManagement/DeleteSocialSite
        /// </remarks>
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.Edit)]
        [ValidateAntiForgeryToken]
        [HttpDelete]
        public HttpResponseMessage DeleteSocialSite(int itemId, int groupID)
        {
            try
            {
                SocialSiteDataAccess.DeleteItem(itemId, groupID);

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
        /// Create a social site
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// POST: http://dnndev.me/DesktopModules/UserGroupSuite/API/GroupManagement/CreateSocialSite
        /// </remarks>
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.View)]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public HttpResponseMessage CreateSocialSite(SocialSiteInfo socialSite)
        {
            try
            {
                var response = new ServiceResponse<SocialSiteInfo>();

                socialSite.CreatedOn = DateTime.Now;
                socialSite.CreatedBy = UserInfo.UserID;
                socialSite.LastUpdatedOn = DateTime.Now;
                socialSite.LastUpdatedBy = UserInfo.UserID;

                SocialSiteDataAccess.CreateItem(socialSite);

                // TODO: Find a more consistent way to do this
                var socialSites = SocialSiteDataAccess.GetItems(socialSite.GroupID).OrderByDescending(r => r.GroupSocialSiteID);
                var savedSocialSite = socialSites.FirstOrDefault(r => r.CreatedBy == socialSite.CreatedBy);

                response.Content = savedSocialSite;

                if (savedSocialSite == null)
                {
                    ServiceResponseHelper<SocialSiteInfo>.AddNoneFoundError("socialSite", ref response);
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
        /// Update a social site
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// POST: http://dnndev.me/DesktopModules/UserGroupSuite/API/GroupManagement/UpdateSocialSite
        /// </remarks>
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.View)]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public HttpResponseMessage UpdateSocialSite(SocialSiteInfo socialSite)
        {
            try
            {
                var originalSocialSite = SocialSiteDataAccess.GetItem(socialSite.GroupSocialSiteID, socialSite.GroupID);
                // only update the fields that would be updated from the UI to keep the DB clean
                var updatesToProcess = SocialSiteHasUpdates(ref originalSocialSite, ref socialSite);
                
                if (updatesToProcess)
                {
                    originalSocialSite.LastUpdatedOn = DateTime.Now;
                    originalSocialSite.LastUpdatedBy = UserInfo.UserID;

                    SocialSiteDataAccess.UpdateItem(originalSocialSite);
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

        private bool SocialSiteHasUpdates(ref SocialSiteInfo originalSocialSite, ref SocialSiteInfo newSocialSite)
        {
            var updatesToProcess = false;

            if (originalSocialSite.SocialID != newSocialSite.SocialID)
            {
                originalSocialSite.SocialID = newSocialSite.SocialID;
                updatesToProcess = true;
            }

            if (!string.Equals(originalSocialSite.SocialSiteURL, newSocialSite.SocialSiteURL))
            {
                originalSocialSite.SocialSiteURL = newSocialSite.SocialSiteURL;
                updatesToProcess = true;
            }

            return updatesToProcess;
        }

        #endregion
    }
}