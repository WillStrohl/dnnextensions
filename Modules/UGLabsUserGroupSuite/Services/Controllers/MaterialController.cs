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
        /// Get all materials for the meeting
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// GET: http://dnndev.me/DesktopModules/UserGroupSuite/API/GroupManagement/GetMaterials
        /// </remarks>
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.View)]
        [HttpGet]
        public HttpResponseMessage GetMaterials(int meetingID)
        {
            try
            {
                var materials = MaterialDataAccess.GetItems(meetingID);
                var response = new ServiceResponse<List<MaterialInfo>> { Content = materials.ToList() };

                return Request.CreateResponse(HttpStatusCode.OK, response.ObjectToJson());
            }
            catch (Exception ex)
            {
                Exceptions.LogException(ex);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ERROR_MESSAGE);
            }
        }

        /// <summary>
        /// Get a material
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// GET: http://dnndev.me/DesktopModules/UserGroupSuite/API/GroupManagement/GetMaterial
        /// </remarks>
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.View)]
        [HttpGet]
        public HttpResponseMessage GetMaterial(int itemId, int meetingID)
        {
            try
            {
                var material = MaterialDataAccess.GetItem(itemId, meetingID);
                var response = new ServiceResponse<MaterialInfo> { Content = material };

                return Request.CreateResponse(HttpStatusCode.OK, response.ObjectToJson());
            }
            catch (Exception ex)
            {
                Exceptions.LogException(ex);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ERROR_MESSAGE);
            }
        }

        /// <summary>
        /// Delete a material
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// DELETE: http://dnndev.me/DesktopModules/UserGroupSuite/API/GroupManagement/DeleteMaterial
        /// </remarks>
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.Edit)]
        [ValidateAntiForgeryToken]
        [HttpDelete]
        public HttpResponseMessage DeleteMaterial(int itemId, int meetingID)
        {
            try
            {
                MaterialDataAccess.DeleteItem(itemId, meetingID);

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
        /// Create a material
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// POST: http://dnndev.me/DesktopModules/UserGroupSuite/API/GroupManagement/CreateMaterial
        /// </remarks>
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.View)]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public HttpResponseMessage CreateMaterial(MaterialInfo material)
        {
            try
            {
                var response = new ServiceResponse<MaterialInfo>();

                material.CreatedOn = DateTime.Now;
                material.CreatedBy = UserInfo.UserID;
                material.LastUpdatedOn = DateTime.Now;
                material.LastUpdatedBy = UserInfo.UserID;

                MaterialDataAccess.CreateItem(material);

                // TODO: Find a more consistent way to do this
                var materials = MaterialDataAccess.GetItems(material.MeetingID).OrderByDescending(r => r.MaterialID);
                var savedMaterial = materials.FirstOrDefault(r => r.CreatedBy == material.CreatedBy);

                response.Content = savedMaterial;

                if (savedMaterial == null)
                {
                    ServiceResponseHelper<MaterialInfo>.AddNoneFoundError("material", ref response);
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
        /// Update a material
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// POST: http://dnndev.me/DesktopModules/UserGroupSuite/API/GroupManagement/UpdateMaterial
        /// </remarks>
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.View)]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public HttpResponseMessage UpdateMaterial(MaterialInfo material)
        {
            try
            {
                var originalMaterial = MaterialDataAccess.GetItem(material.MaterialID, material.MeetingID);
                // only update the fields that would be updated from the UI to keep the DB clean
                var updatesToProcess = MaterialHasUpdates(ref originalMaterial, ref material);
                
                if (updatesToProcess)
                {
                    originalMaterial.LastUpdatedOn = DateTime.Now;
                    originalMaterial.LastUpdatedBy = UserInfo.UserID;

                    MaterialDataAccess.UpdateItem(originalMaterial);
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

        private bool MaterialHasUpdates(ref MaterialInfo originalMaterial, ref MaterialInfo newMaterial)
        {
            var updatesToProcess = false;

            if (!string.Equals(originalMaterial.Title, newMaterial.Title))
            {
                originalMaterial.Title = newMaterial.Title;
                updatesToProcess = true;
            }

            if (!string.Equals(originalMaterial.Description, newMaterial.Description))
            {
                originalMaterial.Description = newMaterial.Description;
                updatesToProcess = true;
            }

            if (!string.Equals(originalMaterial.Source, newMaterial.Source))
            {
                originalMaterial.Source = newMaterial.Source;
                updatesToProcess = true;
            }

            return updatesToProcess;
        }

        #endregion
    }
}