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
using DotNetNuke.Security.Permissions;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Web.Api;
using DNNCommunity.Modules.UserGroupSuite.Components;
using DNNCommunity.Modules.UserGroupSuite.Entities;

namespace DNNCommunity.Modules.UserGroupSuite.Services
{
    // TODO: add validation catches and formal error responses for all end points

    /// <summary>
    /// This is a partial class that spans multiple class files, in order to keep the code manageable. Each method is necessary to support the front end SPA implementation.
    /// </summary>
    /// <remarks>
    /// The SupportModules attribute will require that all API calls set and include module headers, event GET requests. Even Fiddler will return 401 Unauthorized errors.
    /// SupportedModules is defined by the moduleName in the manifest.
    /// </remarks>
    [SupportedModules("UserGroupSuite")]
    public partial class GroupManagementController : ServiceBase
    {
        /// <summary>
        /// Get a group
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// GET: http://dnndev.me/DesktopModules/UserGroupSuite/API/GroupManagement/GetGroups
        /// </remarks>
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.View)]
        [HttpGet]
        public HttpResponseMessage GetGroups()
        {
            try
            {
                var userGroups = GroupDataAccess.GetItems(ActiveModule.ModuleID);
                var response = new ServiceResponse<List<GroupInfo>> { Content = userGroups.ToList() };

                if (userGroups == null)
                {
                    ServiceResponseHelper<List<GroupInfo>>.AddNoneFoundError("GroupInfo", ref response);
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
        /// Get a group
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// GET: http://dnndev.me/DesktopModules/UserGroupSuite/API/GroupManagement/GetGroup
        /// </remarks>
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.View)]
        [HttpGet]
        public HttpResponseMessage GetGroup(int itemId)
        {
            try
            {
                var userGroup = GroupDataAccess.GetItem(itemId, ActiveModule.ModuleID);
                var response = new ServiceResponse<GroupInfo> { Content = userGroup };

                if (userGroup == null)
                {
                    ServiceResponseHelper<GroupInfo>.AddNoneFoundError("GroupInfo", ref response);
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
        /// Delete a group
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// DELETE: http://dnndev.me/DesktopModules/UserGroupSuite/API/GroupManagement/DeleteGroup
        /// </remarks>
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.Edit)]
        [ValidateAntiForgeryToken]
        [HttpDelete]
        public HttpResponseMessage DeleteGroup(int itemId)
        {
            try
            {
                GroupDataAccess.DeleteItem(itemId, ActiveModule.ModuleID);
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
        /// Create a group
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// POST: http://dnndev.me/DesktopModules/UserGroupSuite/API/GroupManagement/CreateGroup
        /// </remarks>
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.Edit)]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public HttpResponseMessage CreateGroup(GroupInfo newGroup)
        {
            try
            {
                newGroup.CreatedOn = DateTime.Now;
                newGroup.CreatedBy = UserInfo.UserID;
                newGroup.LastUpdatedOn = DateTime.Now;
                newGroup.LastUpdatedBy = UserInfo.UserID;
                newGroup.ModuleID = ActiveModule.ModuleID;

                GroupDataAccess.CreateItem(newGroup);

                var response = new ServiceResponse<string> { Content = Globals.RESPONSE_SUCCESS };

                return Request.CreateResponse(HttpStatusCode.OK, response.ObjectToJson());
            }
            catch (Exception ex)
            {
                Exceptions.LogException(ex);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ERROR_MESSAGE);
            }
        }

        /// <summary>
        /// Update a group
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// POST: http://dnndev.me/DesktopModules/UserGroupSuite/API/GroupManagement/UpdateGroup
        /// </remarks>
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.Edit)]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public HttpResponseMessage UpdateGroup(GroupInfo userGroup)
        {
            try
            {
                var originalUserGroup = GroupDataAccess.GetItem(userGroup.GroupID, userGroup.ModuleID);
                var updatesToProcess = GroupHasUpdates(ref originalUserGroup, ref userGroup);

                if (updatesToProcess)
                {
                    originalUserGroup.LastUpdatedOn = DateTime.Now;
                    originalUserGroup.LastUpdatedBy = UserInfo.UserID;

                    GroupDataAccess.UpdateItem(originalUserGroup);
                }

                var savedGroup = GroupDataAccess.GetItem(originalUserGroup.GroupID, originalUserGroup.ModuleID);

                var response = new ServiceResponse<GroupInfo> { Content = savedGroup };

                return Request.CreateResponse(HttpStatusCode.OK, response.ObjectToJson());
            }
            catch (Exception ex)
            {
                Exceptions.LogException(ex);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ERROR_MESSAGE);
            }
        }

        /// <summary>
        /// Use to determine if the user has edit permissions
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// GET: http://dnndev.me/DesktopModules/UserGroupSuite/API/GroupManagement/UserCanEditGroup
        /// </remarks>
        [AllowAnonymous]
        [HttpGet]
        public HttpResponseMessage UserCanEditGroup(int itemId)
        {
            // TODO: Update this to allow other leaders to edit a group
            ServiceResponse<string> response = null;

            if (UserInfo.IsSuperUser || UserInfo.IsInRole(PortalSettings.AdministratorRoleName) || ModulePermissionController.HasModulePermission(ActiveModule.ModulePermissions, "Edit"))
            {
                response = new ServiceResponse<string>() { Content = Globals.RESPONSE_SUCCESS };
            }
            else
            {
                var userGroup = GroupDataAccess.GetItem(itemId, ActiveModule.ModuleID);

                if (userGroup != null && userGroup.CreatedBy == UserInfo.UserID || userGroup.LastUpdatedBy == UserInfo.UserID)
                {
                    response = new ServiceResponse<string>() {Content = Globals.RESPONSE_SUCCESS };
                }
                else
                {
                    response = new ServiceResponse<string>() { Content = Globals.RESPONSE_FAILURE };
                }
            }

            return Request.CreateResponse(HttpStatusCode.OK, response.ObjectToJson());
        }

        #region Private Helper Methods

        private bool GroupHasUpdates(ref GroupInfo originalUserGroup, ref GroupInfo newUserGroup)
        {
            var updatesToProcess = false;

            if (!string.Equals(newUserGroup.GroupName, originalUserGroup.GroupName))
            {
                originalUserGroup.GroupName = newUserGroup.GroupName;
                updatesToProcess = true;
            }

            if (newUserGroup.CountryID != originalUserGroup.CountryID)
            {
                originalUserGroup.CountryID = newUserGroup.CountryID;
                updatesToProcess = true;
            }

            if (newUserGroup.RegionID != originalUserGroup.RegionID)
            {
                originalUserGroup.RegionID = newUserGroup.RegionID;
                updatesToProcess = true;
            }

            if (!string.Equals(newUserGroup.City, originalUserGroup.City))
            {
                originalUserGroup.City = newUserGroup.City;
                updatesToProcess = true;
            }

            if (newUserGroup.LanguageID != originalUserGroup.LanguageID)
            {
                originalUserGroup.LanguageID = newUserGroup.LanguageID;
                updatesToProcess = true;
            }

            if (!string.Equals(newUserGroup.Description, originalUserGroup.Description))
            {
                originalUserGroup.Description = newUserGroup.Description;
                updatesToProcess = true;
            }

            if (!string.Equals(newUserGroup.Website, originalUserGroup.Website))
            {
                originalUserGroup.Website = newUserGroup.Website;
                updatesToProcess = true;
            }

            if (!string.Equals(newUserGroup.Avatar, originalUserGroup.Avatar))
            {
                originalUserGroup.Avatar = newUserGroup.Avatar;
                updatesToProcess = true;
            }

            if (newUserGroup.IsActive != originalUserGroup.IsActive)
            {
                originalUserGroup.IsActive = newUserGroup.IsActive;
                updatesToProcess = true;
            }

            if (!string.Equals(newUserGroup.Slug, originalUserGroup.Slug))
            {
                originalUserGroup.Slug = newUserGroup.Slug;
                updatesToProcess = true;
            }

            if (originalUserGroup.CustomProperties != null)
            {
                // parse custom properties for updates
                foreach (var property in originalUserGroup.CustomPropertiesObj)
                {
                    if (newUserGroup.CustomPropertiesObj.Any(p => p.Name == property.Name))
                    {
                        // see if the existing property needs to be updated
                        var prop = newUserGroup.CustomPropertiesObj.FirstOrDefault(p => p.Name == property.Name);
                        if (!string.Equals(prop.Value, property.Value))
                        {
                            property.Value = prop.Value;
                            updatesToProcess = true;
                        }
                    }
                    else
                    {
                        // delete the property
                        newUserGroup.CustomPropertiesObj.Remove(property);
                        updatesToProcess = true;
                    }
                }
            }

            if (newUserGroup.CustomPropertiesObj != null)
            {
                // add any new properties
                if (originalUserGroup.CustomProperties == null)
                {
                    foreach (var property in newUserGroup.CustomPropertiesObj)
                    {
                        originalUserGroup.CustomPropertiesObj.Add(property);
                        updatesToProcess = true;
                    }
                }
                else
                {
                    var camp = originalUserGroup;
                    foreach (var property in newUserGroup.CustomPropertiesObj.Where(property => !camp.CustomPropertiesObj.Contains(property)))
                    {
                        camp.CustomPropertiesObj.Add(property);
                        updatesToProcess = true;
                    }
                }
            }

            return updatesToProcess;
        }

        #endregion
    }
}