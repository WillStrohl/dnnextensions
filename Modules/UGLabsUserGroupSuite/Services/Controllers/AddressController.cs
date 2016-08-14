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
        /// Get all addresses for the module
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// GET: http://dnndev.me/DesktopModules/UserGroupSuite/API/GroupManagement/GetAddresses
        /// </remarks>
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.View)]
        [HttpGet]
        public HttpResponseMessage GetAddresses()
        {
            try
            {
                return GetAddresses(ActiveModule.ModuleID);
            }
            catch (Exception ex)
            {
                Exceptions.LogException(ex);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ERROR_MESSAGE);
            }
        }

        /// <summary>
        /// Get all addresses for the module
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// GET: http://dnndev.me/DesktopModules/UserGroupSuite/API/GroupManagement/GetAddresses
        /// </remarks>
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.View)]
        [HttpGet]
        public HttpResponseMessage GetAddresses(int moduleID)
        {
            try
            {
                var addresses = AddressDataAccess.GetItems(moduleID);
                var response = new ServiceResponse<List<AddressInfo>> { Content = addresses.ToList() };

                return Request.CreateResponse(HttpStatusCode.OK, response.ObjectToJson());
            }
            catch (Exception ex)
            {
                Exceptions.LogException(ex);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ERROR_MESSAGE);
            }
        }

        /// <summary>
        /// Get an address
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// GET: http://dnndev.me/DesktopModules/UserGroupSuite/API/GroupManagement/GetAddress
        /// </remarks>
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.View)]
        [HttpGet]
        public HttpResponseMessage GetAddress(int itemId)
        {
            try
            {
                return GetAddress(itemId, ActiveModule.ModuleID);
            }
            catch (Exception ex)
            {
                Exceptions.LogException(ex);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ERROR_MESSAGE);
            }
        }

        /// <summary>
        /// Get an address
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// GET: http://dnndev.me/DesktopModules/UserGroupSuite/API/GroupManagement/GetAddress
        /// </remarks>
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.View)]
        [HttpGet]
        public HttpResponseMessage GetAddress(int itemId, int moduleID)
        {
            try
            {
                var address = AddressDataAccess.GetItem(itemId, moduleID);
                var response = new ServiceResponse<AddressInfo> { Content = address };

                return Request.CreateResponse(HttpStatusCode.OK, response.ObjectToJson());
            }
            catch (Exception ex)
            {
                Exceptions.LogException(ex);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ERROR_MESSAGE);
            }
        }

        /// <summary>
        /// Delete an address
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// DELETE: http://dnndev.me/DesktopModules/UserGroupSuite/API/GroupManagement/DeleteAddress
        /// </remarks>
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.Edit)]
        [ValidateAntiForgeryToken]
        [HttpDelete]
        public HttpResponseMessage DeleteAddress(int itemId)
        {
            try
            {
                return DeleteAddress(itemId, ActiveModule.ModuleID);
            }
            catch (Exception ex)
            {
                Exceptions.LogException(ex);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ERROR_MESSAGE);
            }
        }

        /// <summary>
        /// Delete an address
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// DELETE: http://dnndev.me/DesktopModules/UserGroupSuite/API/GroupManagement/DeleteAddress
        /// </remarks>
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.Edit)]
        [ValidateAntiForgeryToken]
        [HttpDelete]
        public HttpResponseMessage DeleteAddress(int itemId, int moduleID)
        {
            try
            {
                AddressDataAccess.DeleteItem(itemId, moduleID);

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
        /// Create an address
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// POST: http://dnndev.me/DesktopModules/UserGroupSuite/API/GroupManagement/CreateAddress
        /// </remarks>
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.View)]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public HttpResponseMessage CreateAddress(AddressInfo address)
        {
            try
            {
                var response = new ServiceResponse<AddressInfo>();

                address.CreatedOn = DateTime.Now;
                address.CreatedBy = UserInfo.UserID;
                address.LastUpdatedOn = DateTime.Now;
                address.LastUpdatedBy = UserInfo.UserID;
                address.ModuleID = ActiveModule.ModuleID;

                AddressDataAccess.CreateItem(address);

                // TODO: Find a more consistent way to do this
                var addresses = AddressDataAccess.GetItems(address.ModuleID).OrderByDescending(r => r.AddressID);
                var savedAddress = addresses.FirstOrDefault(r => r.CreatedBy == address.CreatedBy);

                response.Content = savedAddress;

                if (savedAddress == null)
                {
                    ServiceResponseHelper<AddressInfo>.AddNoneFoundError("address", ref response);
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
        /// Update an address
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// POST: http://dnndev.me/DesktopModules/UserGroupSuite/API/GroupManagement/UpdateAddress
        /// </remarks>
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.View)]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public HttpResponseMessage UpdateAddress(AddressInfo address)
        {
            try
            {
                var originalAddress = AddressDataAccess.GetItem(address.AddressID, address.ModuleID);
                // only update the fields that would be updated from the UI to keep the DB clean
                var updatesToProcess = AddressHasUpdates(ref originalAddress, ref address);
                
                if (updatesToProcess)
                {
                    originalAddress.LastUpdatedOn = DateTime.Now;
                    originalAddress.LastUpdatedBy = UserInfo.UserID;

                    AddressDataAccess.UpdateItem(originalAddress);
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

        private bool AddressHasUpdates(ref AddressInfo originalAddress, ref AddressInfo newAddress)
        {
            var updatesToProcess = false;

            if (!string.Equals(originalAddress.Nickname, newAddress.Nickname))
            {
                originalAddress.Nickname = newAddress.Nickname;
                updatesToProcess = true;
            }

            if (!string.Equals(originalAddress.Line1, newAddress.Line1))
            {
                originalAddress.Line1 = newAddress.Line1;
                updatesToProcess = true;
            }

            if (!string.Equals(originalAddress.Line2, newAddress.Line2))
            {
                originalAddress.Line2 = newAddress.Line2;
                updatesToProcess = true;
            }

            if (!string.Equals(originalAddress.City, newAddress.City))
            {
                originalAddress.City = newAddress.City;
                updatesToProcess = true;
            }

            if (!string.Equals(originalAddress.Region, newAddress.Region))
            {
                originalAddress.Region = newAddress.Region;
                updatesToProcess = true;
            }

            if (!string.Equals(originalAddress.Country, newAddress.Country))
            {
                originalAddress.Country = newAddress.Country;
                updatesToProcess = true;
            }

            if (!string.Equals(originalAddress.PostalCode, newAddress.PostalCode))
            {
                originalAddress.PostalCode = newAddress.PostalCode;
                updatesToProcess = true;
            }

            return updatesToProcess;
        }

        #endregion
    }
}