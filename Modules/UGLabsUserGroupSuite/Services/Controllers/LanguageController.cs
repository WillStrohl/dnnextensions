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
        /// Get all languages for the module
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// GET: http://dnndev.me/DesktopModules/UserGroupSuite/API/GroupManagement/GetLanguages
        /// </remarks>
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.View)]
        [HttpGet]
        public HttpResponseMessage GetLanguages()
        {
            try
            {
                return GetLanguages(ActiveModule.PortalID);
            }
            catch (Exception ex)
            {
                Exceptions.LogException(ex);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ERROR_MESSAGE);
            }
        }

        /// <summary>
        /// Get all languages for the module
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// GET: http://dnndev.me/DesktopModules/UserGroupSuite/API/GroupManagement/GetLanguages
        /// </remarks>
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.View)]
        [HttpGet]
        public HttpResponseMessage GetLanguages(int portalID)
        {
            try
            {
                var languages = LanguageDataAccess.GetItems(portalID);
                var response = new ServiceResponse<List<LanguageInfo>> { Content = languages.ToList() };

                return Request.CreateResponse(HttpStatusCode.OK, response.ObjectToJson());
            }
            catch (Exception ex)
            {
                Exceptions.LogException(ex);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ERROR_MESSAGE);
            }
        }

        /// <summary>
        /// Get a language
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// GET: http://dnndev.me/DesktopModules/UserGroupSuite/API/GroupManagement/GetLanguage
        /// </remarks>
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.View)]
        [HttpGet]
        public HttpResponseMessage GetLanguage(int itemId)
        {
            try
            {
                return GetLanguage(itemId, ActiveModule.PortalID);
            }
            catch (Exception ex)
            {
                Exceptions.LogException(ex);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ERROR_MESSAGE);
            }
        }

        /// <summary>
        /// Get a language
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// GET: http://dnndev.me/DesktopModules/UserGroupSuite/API/GroupManagement/GetLanguage
        /// </remarks>
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.View)]
        [HttpGet]
        public HttpResponseMessage GetLanguage(int itemId, int portalID)
        {
            try
            {
                var language = LanguageDataAccess.GetItem(itemId, portalID);
                var response = new ServiceResponse<LanguageInfo> { Content = language };

                return Request.CreateResponse(HttpStatusCode.OK, response.ObjectToJson());
            }
            catch (Exception ex)
            {
                Exceptions.LogException(ex);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ERROR_MESSAGE);
            }
        }

        /// <summary>
        /// Delete a language
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// DELETE: http://dnndev.me/DesktopModules/UserGroupSuite/API/GroupManagement/DeleteLanguage
        /// </remarks>
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.Edit)]
        [ValidateAntiForgeryToken]
        [HttpDelete]
        public HttpResponseMessage DeleteLanguage(int itemId)
        {
            try
            {
                return DeleteLanguage(itemId, ActiveModule.PortalID);
            }
            catch (Exception ex)
            {
                Exceptions.LogException(ex);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ERROR_MESSAGE);
            }
        }

        /// <summary>
        /// Delete a language
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// DELETE: http://dnndev.me/DesktopModules/UserGroupSuite/API/GroupManagement/DeleteLanguage
        /// </remarks>
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.Edit)]
        [ValidateAntiForgeryToken]
        [HttpDelete]
        public HttpResponseMessage DeleteLanguage(int itemId, int portalID)
        {
            try
            {
                LanguageDataAccess.DeleteItem(itemId, portalID);

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
        /// Create a language
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// POST: http://dnndev.me/DesktopModules/UserGroupSuite/API/GroupManagement/CreateLanguage
        /// </remarks>
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.View)]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public HttpResponseMessage CreateLanguage(LanguageInfo language)
        {
            try
            {
                var response = new ServiceResponse<LanguageInfo>();

                language.PortalID = ActiveModule.PortalID;

                LanguageDataAccess.CreateItem(language);

                // TODO: Find a more consistent way to do this
                var languages = LanguageDataAccess.GetItems(language.PortalID).OrderByDescending(r => r.GroupLanguageID);
                var savedLanguage = languages.FirstOrDefault();

                response.Content = savedLanguage;

                if (savedLanguage == null)
                {
                    ServiceResponseHelper<LanguageInfo>.AddNoneFoundError("language", ref response);
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
        /// Update a language
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// POST: http://dnndev.me/DesktopModules/UserGroupSuite/API/GroupManagement/UpdateLanguage
        /// </remarks>
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.View)]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public HttpResponseMessage UpdateLanguage(LanguageInfo language)
        {
            try
            {
                var originalLanguage = LanguageDataAccess.GetItem(language.GroupLanguageID, language.PortalID);
                // only update the fields that would be updated from the UI to keep the DB clean
                var updatesToProcess = LanguageHasUpdates(ref originalLanguage, ref language);
                
                if (updatesToProcess)
                {
                    LanguageDataAccess.UpdateItem(originalLanguage);
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

        private bool LanguageHasUpdates(ref LanguageInfo originalLanguage, ref LanguageInfo newLanguage)
        {
            var updatesToProcess = false;

            if (!string.Equals(originalLanguage.Language, newLanguage.Language))
            {
                originalLanguage.Language = newLanguage.Language;
                updatesToProcess = true;
            }

            if (originalLanguage.PortalID != newLanguage.PortalID)
            {
                originalLanguage.PortalID = newLanguage.PortalID;
                updatesToProcess = true;
            }

            return updatesToProcess;
        }

        #endregion
    }
}