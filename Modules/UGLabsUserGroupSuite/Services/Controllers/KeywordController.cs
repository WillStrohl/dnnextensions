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
        /// Get all keywords for the module
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// GET: http://dnndev.me/DesktopModules/UserGroupSuite/API/GroupManagement/GetKeywords
        /// </remarks>
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.View)]
        [HttpGet]
        public HttpResponseMessage GetKeywords()
        {
            try
            {
                return GetKeywords(ActiveModule.ModuleID);
            }
            catch (Exception ex)
            {
                Exceptions.LogException(ex);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ERROR_MESSAGE);
            }
        }

        /// <summary>
        /// Get all keywords for the module
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// GET: http://dnndev.me/DesktopModules/UserGroupSuite/API/GroupManagement/GetKeywords
        /// </remarks>
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.View)]
        [HttpGet]
        public HttpResponseMessage GetKeywords(int moduleID)
        {
            try
            {
                var keywords = KeywordDataAccess.GetItems(moduleID);
                var response = new ServiceResponse<List<KeywordInfo>> { Content = keywords.ToList() };

                return Request.CreateResponse(HttpStatusCode.OK, response.ObjectToJson());
            }
            catch (Exception ex)
            {
                Exceptions.LogException(ex);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ERROR_MESSAGE);
            }
        }

        /// <summary>
        /// Get a keyword
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// GET: http://dnndev.me/DesktopModules/UserGroupSuite/API/GroupManagement/GetKeyword
        /// </remarks>
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.View)]
        [HttpGet]
        public HttpResponseMessage GetKeyword(int itemId)
        {
            try
            {
                return GetKeyword(itemId, ActiveModule.ModuleID);
            }
            catch (Exception ex)
            {
                Exceptions.LogException(ex);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ERROR_MESSAGE);
            }
        }

        /// <summary>
        /// Get a keyword
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// GET: http://dnndev.me/DesktopModules/UserGroupSuite/API/GroupManagement/GetKeyword
        /// </remarks>
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.View)]
        [HttpGet]
        public HttpResponseMessage GetKeyword(int itemId, int moduleID)
        {
            try
            {
                var keyword = KeywordDataAccess.GetItem(itemId, moduleID);
                var response = new ServiceResponse<KeywordInfo> { Content = keyword };

                return Request.CreateResponse(HttpStatusCode.OK, response.ObjectToJson());
            }
            catch (Exception ex)
            {
                Exceptions.LogException(ex);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ERROR_MESSAGE);
            }
        }

        /// <summary>
        /// Delete a keyword
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// DELETE: http://dnndev.me/DesktopModules/UserGroupSuite/API/GroupManagement/DeleteKeyword
        /// </remarks>
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.Edit)]
        [ValidateAntiForgeryToken]
        [HttpDelete]
        public HttpResponseMessage DeleteKeyword(int itemId)
        {
            try
            {
                return DeleteKeyword(itemId, ActiveModule.ModuleID);
            }
            catch (Exception ex)
            {
                Exceptions.LogException(ex);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ERROR_MESSAGE);
            }
        }

        /// <summary>
        /// Delete a keyword
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// DELETE: http://dnndev.me/DesktopModules/UserGroupSuite/API/GroupManagement/DeleteKeyword
        /// </remarks>
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.Edit)]
        [ValidateAntiForgeryToken]
        [HttpDelete]
        public HttpResponseMessage DeleteKeyword(int itemId, int moduleID)
        {
            try
            {
                KeywordDataAccess.DeleteItem(itemId, moduleID);

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
        /// Create a keyword
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// POST: http://dnndev.me/DesktopModules/UserGroupSuite/API/GroupManagement/CreateKeyword
        /// </remarks>
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.View)]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public HttpResponseMessage CreateKeyword(KeywordInfo keyword)
        {
            try
            {
                var response = new ServiceResponse<KeywordInfo>();

                keyword.CreatedOn = DateTime.Now;
                keyword.CreatedBy = UserInfo.UserID;
                keyword.LastUpdatedOn = DateTime.Now;
                keyword.LastUpdatedBy = UserInfo.UserID;
                keyword.ModuleID = ActiveModule.ModuleID;

                KeywordDataAccess.CreateItem(keyword);

                // TODO: Find a more consistent way to do this
                var keywords = KeywordDataAccess.GetItems(keyword.ModuleID).OrderByDescending(r => r.KeywordID);
                var savedKeyword = keywords.FirstOrDefault(r => r.CreatedBy == keyword.CreatedBy);

                response.Content = savedKeyword;

                if (savedKeyword == null)
                {
                    ServiceResponseHelper<KeywordInfo>.AddNoneFoundError("keyword", ref response);
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
        /// Update a keyword
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// POST: http://dnndev.me/DesktopModules/UserGroupSuite/API/GroupManagement/UpdateKeyword
        /// </remarks>
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.View)]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public HttpResponseMessage UpdateKeyword(KeywordInfo keyword)
        {
            try
            {
                var originalKeyword = KeywordDataAccess.GetItem(keyword.KeywordID, keyword.ModuleID);
                // only update the fields that would be updated from the UI to keep the DB clean
                var updatesToProcess = KeywordHasUpdates(ref originalKeyword, ref keyword);
                
                if (updatesToProcess)
                {
                    originalKeyword.LastUpdatedOn = DateTime.Now;
                    originalKeyword.LastUpdatedBy = UserInfo.UserID;

                    KeywordDataAccess.UpdateItem(originalKeyword);
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

        private bool KeywordHasUpdates(ref KeywordInfo originalKeyword, ref KeywordInfo newKeyword)
        {
            var updatesToProcess = false;

            if (originalKeyword.TermID != newKeyword.TermID)
            {
                originalKeyword.TermID = newKeyword.TermID;
                updatesToProcess = true;
            }

            if (originalKeyword.GroupID != newKeyword.TermID)
            {
                originalKeyword.TermID = newKeyword.TermID;
                updatesToProcess = true;
            }

            if (originalKeyword.MeetingID != newKeyword.MeetingID)
            {
                originalKeyword.MeetingID = newKeyword.MeetingID;
                updatesToProcess = true;
            }

            return updatesToProcess;
        }

        #endregion
    }
}