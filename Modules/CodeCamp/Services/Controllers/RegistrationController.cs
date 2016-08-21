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
using WillStrohl.Modules.CodeCamp.Components;
using WillStrohl.Modules.CodeCamp.Controllers;
using WillStrohl.Modules.CodeCamp.Entities;

namespace WillStrohl.Modules.CodeCamp.Services
{
    public partial class EventController
    {
        /// <summary>
        /// Get all registrations
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// GET: http://dnndev.me/DesktopModules/CodeCamp/API/Event/GetRegistrations
        /// </remarks>
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.View)]
        [HttpGet]
        public HttpResponseMessage GetRegistrations(int codeCampId)
        {
            try
            {
                var registrations = RegistrationDataAccess.GetItems(codeCampId);
                var response = new ServiceResponse<List<RegistrationInfo>> { Content = registrations.ToList() };

                return Request.CreateResponse(HttpStatusCode.OK, response.ObjectToJson());
            }
            catch (Exception ex)
            {
                Exceptions.LogException(ex);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ERROR_MESSAGE);
            }
        }

        /// <summary>
        /// Get a registration
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// GET: http://dnndev.me/DesktopModules/CodeCamp/API/Event/GetRegistration
        /// </remarks>
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.View)]
        [HttpGet]
        public HttpResponseMessage GetRegistration(int itemId, int codeCampId)
        {
            try
            {
                var registration = RegistrationDataAccess.GetItem(itemId, codeCampId);
                var response = new ServiceResponse<RegistrationInfo> { Content = registration };

                return Request.CreateResponse(HttpStatusCode.OK, response.ObjectToJson());
            }
            catch (Exception ex)
            {
                Exceptions.LogException(ex);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ERROR_MESSAGE);
            }
        }

        /// <summary>
        /// Get a registration
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// GET: http://dnndev.me/DesktopModules/CodeCamp/API/Event/GetRegistrationByUserId
        /// </remarks>
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.View)]
        [HttpGet]
        public HttpResponseMessage GetRegistrationByUserId(int userId, int codeCampId)
        {
            try
            {
                var response = new ServiceResponse<RegistrationInfo>();
                RegistrationInfo registration = null;

                if (userId > 0)
                {
                    registration = RegistrationDataAccess.GetItemByUserId(userId, codeCampId);
                    response.Content = registration;
                }

                if (registration == null)
                {
                    ServiceResponseHelper<RegistrationInfo>.AddNoneFoundError("registration", ref response);
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
        /// Delete a registration
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// DELETE: http://dnndev.me/DesktopModules/CodeCamp/API/Event/DeleteRegistration
        /// </remarks>
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.Edit)]
        [ValidateAntiForgeryToken]
        [HttpDelete]
        public HttpResponseMessage DeleteRegistration(int itemId, int codeCampId)
        {
            try
            {
                RegistrationDataAccess.DeleteItem(itemId, codeCampId);

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
        /// Create a registration
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// POST: http://dnndev.me/DesktopModules/CodeCamp/API/Event/CreateRegistration
        /// </remarks>
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.View)]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public HttpResponseMessage CreateRegistration(RegistrationInfo registration)
        {
            try
            {
                var response = new ServiceResponse<RegistrationInfo>();

                if (registration.UserId <= 0)
                {
                    // user isn't logged in and therefore doesn't likely have a user account on the site
                    // we need to register them into DNN for them

                    var firstName = registration.CustomPropertiesObj.FirstOrDefault(p => p.Name == "FirstName").Value;
                    var lastName = registration.CustomPropertiesObj.FirstOrDefault(p => p.Name == "LastName").Value;
                    var email = registration.CustomPropertiesObj.FirstOrDefault(p => p.Name == "Email").Value;
                    var portalId = int.Parse(registration.CustomPropertiesObj.FirstOrDefault(p => p.Name == "PortalId").Value);

                    var ctlUser = new DnnUserController();
                    var status = ctlUser.CreateNewUser(firstName, lastName, email, portalId);

                    switch (status)
                    {
                        case UserCreateStatus.DuplicateEmail:
                            ServiceResponseHelper<RegistrationInfo>.AddUserCreateError("DuplicateEmail", ref response);
                            break;
                        case UserCreateStatus.DuplicateUserName:
                            ServiceResponseHelper<RegistrationInfo>.AddUserCreateError("DuplicateUserName", ref response);
                            break;
                        case UserCreateStatus.Success:
                            var user = UserController.GetUserByName(email);
                            registration.UserId = user.UserID;

                            UserController.UserLogin(portalId, user, PortalSettings.PortalName, string.Empty, false);
                            break;
                        case UserCreateStatus.UnexpectedError:
                            ServiceResponseHelper<RegistrationInfo>.AddUserCreateError("UnexpectedError", ref response);
                            break;
                        case UserCreateStatus.UsernameAlreadyExists:
                            ServiceResponseHelper<RegistrationInfo>.AddUserCreateError("UsernameAlreadyExists", ref response);
                            break;
                        case UserCreateStatus.UserAlreadyRegistered:
                            ServiceResponseHelper<RegistrationInfo>.AddUserCreateError("UserAlreadyRegistered", ref response);
                            break;
                        default:
                            ServiceResponseHelper<RegistrationInfo>.AddUnknownError(ref response);
                            break;
                    }
                }

                if (response.Errors.Count == 0)
                {
                    registration.RegistrationDate = DateTime.Now;

                    RegistrationDataAccess.CreateItem(registration);

                    var registrations = RegistrationDataAccess.GetItems(registration.CodeCampId).OrderByDescending(r => r.RegistrationId);
                    var savedRegistration = registrations.FirstOrDefault(r => r.UserId == registration.UserId);

                    response.Content = savedRegistration;

                    if (savedRegistration == null)
                    {
                        ServiceResponseHelper<RegistrationInfo>.AddNoneFoundError("registration", ref response);
                    }
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
        /// Update a registration
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// POST: http://dnndev.me/DesktopModules/CodeCamp/API/Event/UpdateRegistration
        /// </remarks>
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.View)]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public HttpResponseMessage UpdateRegistration(RegistrationInfo registration)
        {
            try
            {
                var originalRegistration = RegistrationDataAccess.GetItemByUserId(registration.UserId, registration.CodeCampId);
                var updatesToProcess = false;

                // only update the fields that would be udpdated from the UI to keep the DB clean

                if (originalRegistration.TwitterHandle != registration.TwitterHandle)
                {
                    originalRegistration.TwitterHandle = registration.TwitterHandle;
                    updatesToProcess = true;
                }

                if (originalRegistration.ShirtSize != registration.ShirtSize)
                {
                    originalRegistration.ShirtSize = registration.ShirtSize;
                    updatesToProcess = true;
                }

                if (originalRegistration.HasDietaryRequirements != registration.HasDietaryRequirements)
                {
                    originalRegistration.HasDietaryRequirements = registration.HasDietaryRequirements;
                    updatesToProcess = true;
                }

                if (originalRegistration.Notes != registration.Notes)
                {
                    originalRegistration.Notes = registration.Notes;
                    updatesToProcess = true;
                }

                if (originalRegistration.CustomProperties != null)
                {
                    // parse custom properties for updates
                    foreach (var property in originalRegistration.CustomPropertiesObj)
                    {
                        if (registration.CustomPropertiesObj.Any(p => p.Name == property.Name))
                        {
                            // see if the existing property needs to be updated
                            var prop = registration.CustomPropertiesObj.FirstOrDefault(p => p.Name == property.Name);
                            if (!string.Equals(prop.Value, property.Value))
                            {
                                property.Value = prop.Value;
                                updatesToProcess = true;
                            }
                        }
                        else
                        {
                            // delete the property
                            originalRegistration.CustomPropertiesObj.Remove(property);
                            updatesToProcess = true;
                        }
                    }
                }

                if (registration.CustomPropertiesObj != null)
                {
                    // add any new properties
                    if (originalRegistration.CustomProperties == null)
                    {
                        foreach (var property in registration.CustomPropertiesObj)
                        {
                            originalRegistration.CustomPropertiesObj.Add(property);
                            updatesToProcess = true;
                        }
                    }
                    else
                    {
                        foreach (var property in registration.CustomPropertiesObj.Where(property => !originalRegistration.CustomPropertiesObj.Contains(property)))
                        {
                            originalRegistration.CustomPropertiesObj.Add(property);
                            updatesToProcess = true;
                        }
                    }
                }

                if (updatesToProcess)
                {
                    RegistrationDataAccess.UpdateItem(originalRegistration);
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
    }
}