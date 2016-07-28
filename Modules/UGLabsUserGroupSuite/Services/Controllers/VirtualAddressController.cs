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
        /// Get all virtual addresses for the module
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// GET: http://dnndev.me/DesktopModules/UserGroupSuite/API/GroupManagement/GetVirtualAddresses
        /// </remarks>
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.View)]
        [HttpGet]
        public HttpResponseMessage GetVirtualAddresses()
        {
            try
            {
                return GetVirtualAddresses(ActiveModule.ModuleID);
            }
            catch (Exception ex)
            {
                Exceptions.LogException(ex);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ERROR_MESSAGE);
            }
        }

        /// <summary>
        /// Get all virtual addresses for the module
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// GET: http://dnndev.me/DesktopModules/UserGroupSuite/API/GroupManagement/GetVirtualAddresses
        /// </remarks>
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.View)]
        [HttpGet]
        public HttpResponseMessage GetVirtualAddresses(int moduleID)
        {
            try
            {
                var virtualAddresses = VirtualAddressDataAccess.GetItems(moduleID);
                var response = new ServiceResponse<List<VirtualAddressInfo>> { Content = virtualAddresses.ToList() };

                return Request.CreateResponse(HttpStatusCode.OK, response.ObjectToJson());
            }
            catch (Exception ex)
            {
                Exceptions.LogException(ex);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ERROR_MESSAGE);
            }
        }

        /// <summary>
        /// Get a virtual address
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// GET: http://dnndev.me/DesktopModules/UserGroupSuite/API/GroupManagement/GetVirtualAddress
        /// </remarks>
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.View)]
        [HttpGet]
        public HttpResponseMessage GetVirtualAddress(int itemId)
        {
            try
            {
                return GetVirtualAddress(itemId, ActiveModule.ModuleID);
            }
            catch (Exception ex)
            {
                Exceptions.LogException(ex);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ERROR_MESSAGE);
            }
        }

        /// <summary>
        /// Get a virtual address
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// GET: http://dnndev.me/DesktopModules/UserGroupSuite/API/GroupManagement/GetVirtualAddress
        /// </remarks>
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.View)]
        [HttpGet]
        public HttpResponseMessage GetVirtualAddress(int itemId, int moduleID)
        {
            try
            {
                var virtualAddress = VirtualAddressDataAccess.GetItem(itemId, moduleID);
                var response = new ServiceResponse<VirtualAddressInfo> { Content = virtualAddress };

                return Request.CreateResponse(HttpStatusCode.OK, response.ObjectToJson());
            }
            catch (Exception ex)
            {
                Exceptions.LogException(ex);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ERROR_MESSAGE);
            }
        }

        /// <summary>
        /// Delete a virtual address
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// DELETE: http://dnndev.me/DesktopModules/UserGroupSuite/API/GroupManagement/DeleteVirtualAddress
        /// </remarks>
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.Edit)]
        [ValidateAntiForgeryToken]
        [HttpDelete]
        public HttpResponseMessage DeleteVirtualAddress(int itemId)
        {
            try
            {
                return DeleteVirtualAddress(itemId, ActiveModule.ModuleID);
            }
            catch (Exception ex)
            {
                Exceptions.LogException(ex);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ERROR_MESSAGE);
            }
        }

        /// <summary>
        /// Delete a virtual address
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// DELETE: http://dnndev.me/DesktopModules/UserGroupSuite/API/GroupManagement/DeleteVirtualAddress
        /// </remarks>
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.Edit)]
        [ValidateAntiForgeryToken]
        [HttpDelete]
        public HttpResponseMessage DeleteVirtualAddress(int itemId, int moduleID)
        {
            try
            {
                VirtualAddressDataAccess.DeleteItem(itemId, moduleID);

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
        /// Create a virtual address
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// POST: http://dnndev.me/DesktopModules/UserGroupSuite/API/GroupManagement/CreateVirtualAddress
        /// </remarks>
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.View)]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public HttpResponseMessage CreateVirtualAddress(VirtualAddressInfo virtualAddress)
        {
            try
            {
                var response = new ServiceResponse<VirtualAddressInfo>();

                virtualAddress.CreatedOn = DateTime.Now;
                virtualAddress.CreatedBy = UserInfo.UserID;
                virtualAddress.LastUpdatedOn = DateTime.Now;
                virtualAddress.LastUpdatedBy = UserInfo.UserID;
                virtualAddress.ModuleID = ActiveModule.ModuleID;

                VirtualAddressDataAccess.CreateItem(virtualAddress);

                // TODO: Find a more consistent way to do this
                var virtualAddresses = VirtualAddressDataAccess.GetItems(virtualAddress.ModuleID).OrderByDescending(r => r.AddressID);
                var savedVirtualAddress = virtualAddresses.FirstOrDefault(r => r.CreatedBy == virtualAddress.CreatedBy);

                response.Content = savedVirtualAddress;

                if (savedVirtualAddress == null)
                {
                    ServiceResponseHelper<VirtualAddressInfo>.AddNoneFoundError("virtualAddress", ref response);
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
        /// Update a virtual address
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// POST: http://dnndev.me/DesktopModules/UserGroupSuite/API/GroupManagement/UpdateVirtualAddress
        /// </remarks>
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.View)]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public HttpResponseMessage UpdateVirtualAddress(VirtualAddressInfo virtualAddress)
        {
            try
            {
                var originalVirtualAddress = VirtualAddressDataAccess.GetItem(virtualAddress.AddressID, virtualAddress.ModuleID);
                // only update the fields that would be updated from the UI to keep the DB clean
                var updatesToProcess = VirtualAddressHasUpdates(ref originalVirtualAddress, ref virtualAddress);
                
                if (updatesToProcess)
                {
                    originalVirtualAddress.LastUpdatedOn = DateTime.Now;
                    originalVirtualAddress.LastUpdatedBy = UserInfo.UserID;

                    VirtualAddressDataAccess.UpdateItem(originalVirtualAddress);
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

        private bool VirtualAddressHasUpdates(ref VirtualAddressInfo originalVirtualAddress, ref VirtualAddressInfo newVirtualAddress)
        {
            var updatesToProcess = false;

            if (!string.Equals(originalVirtualAddress.AddressType, newVirtualAddress.AddressType))
            {
                originalVirtualAddress.AddressType = newVirtualAddress.AddressType;
                updatesToProcess = true;
            }

            if (!string.Equals(originalVirtualAddress.Description, newVirtualAddress.Description))
            {
                originalVirtualAddress.Description = newVirtualAddress.Description;
                updatesToProcess = true;
            }

            return updatesToProcess;
        }

        #endregion
    }
}