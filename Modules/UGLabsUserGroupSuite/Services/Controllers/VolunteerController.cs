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
using WillStrohl.Modules.CodeCamp.Components;
using WillStrohl.Modules.CodeCamp.Entities;

namespace WillStrohl.Modules.CodeCamp.Services
{
    public partial class EventController
    {
        /// <summary>
        /// Get all volunteers
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// GET: http://dnndev.me/DesktopModules/CodeCamp/API/Event/GetVolunteers
        /// </remarks>
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.View)]
        [HttpGet]
        public HttpResponseMessage GetVolunteers(int codeCampId)
        {
            try
            {
                var volunteers = VolunteerDataAccess.GetItems(codeCampId);

                volunteers = LoadSupplementalProperties(volunteers);

                var response = new ServiceResponse<List<VolunteerInfo>> { Content = volunteers.ToList() };

                if (volunteers == null)
                {
                    ServiceResponseHelper<List<VolunteerInfo>>.AddNoneFoundError("volunteers", ref response);
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
        /// Get a volunteer
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// GET: http://dnndev.me/DesktopModules/CodeCamp/API/Event/GetVolunteer
        /// </remarks>
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.View)]
        [HttpGet]
        public HttpResponseMessage GetVolunteer(int itemId, int codeCampId)
        {
            try
            {
                var volunteer = VolunteerDataAccess.GetItem(itemId, codeCampId);
                var response = new ServiceResponse<VolunteerInfo> { Content = volunteer };

                if (volunteer == null &&
                    (UserInfo.IsSuperUser || UserInfo.IsInRole(PortalSettings.AdministratorRoleName) ||
                     ModulePermissionController.HasModulePermission(ActiveModule.ModulePermissions, "Edit")))
                {
                    // automatically make superusers, admins, and editors a volunteer
                    response.Content = GenerateOrganizerVolunteerInfo(codeCampId);
                }
                else if(volunteer == null)
                {
                    ServiceResponseHelper<VolunteerInfo>.AddNoneFoundError("volunteer", ref response);
                }

                if (volunteer != null)
                {
                    LoadSupplementalProperties(ref volunteer);
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
        /// Get a volunteer by their registration Id
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// GET: http://dnndev.me/DesktopModules/CodeCamp/API/Event/GetVolunteerByRegistrationId
        /// </remarks>
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.View)]
        [HttpGet]
        public HttpResponseMessage GetVolunteerByRegistrationId(int registrationId, int codeCampId)
        {
            try
            {
                var volunteer = VolunteerDataAccess.GetItemByRegistrationId(registrationId, codeCampId);

                if (volunteer != null)
                {
                    LoadSupplementalProperties(ref volunteer);
                }

                var response = new ServiceResponse<VolunteerInfo> { Content = volunteer };

                if (volunteer == null &&
                    (UserInfo.IsSuperUser || UserInfo.IsInRole(PortalSettings.AdministratorRoleName) ||
                     ModulePermissionController.HasModulePermission(ActiveModule.ModulePermissions, "Edit")))
                {
                    // automatically make superusers, admins, and editors a volunteer
                    response.Content = GenerateOrganizerVolunteerInfo(codeCampId);
                }
                else if (volunteer == null)
                {
                    ServiceResponseHelper<VolunteerInfo>.AddNoneFoundError("volunteer", ref response);
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
        /// Delete a volunteer
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// DELETE: http://dnndev.me/DesktopModules/CodeCamp/API/Event/DeleteVolunteer
        /// </remarks>
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.Edit)]
        [ValidateAntiForgeryToken]
        [HttpDelete]
        public HttpResponseMessage DeleteVolunteer(int itemId, int codeCampId)
        {
            try
            {
                VolunteerDataAccess.DeleteItem(itemId, codeCampId);

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
        /// Create a volunteer
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// POST: http://dnndev.me/DesktopModules/CodeCamp/API/Event/CreateVolunteer
        /// </remarks>
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.View)]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public HttpResponseMessage CreateVolunteer(VolunteerInfo volunteer)
        {
            try
            {
                VolunteerDataAccess.CreateItem(volunteer);

                var savedVolunteer = VolunteerDataAccess.GetItemByRegistrationId(volunteer.RegistrationId, volunteer.CodeCampId);

                var response = new ServiceResponse<VolunteerInfo> { Content = savedVolunteer };

                return Request.CreateResponse(HttpStatusCode.OK, response.ObjectToJson());
            }
            catch (Exception ex)
            {
                Exceptions.LogException(ex);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ERROR_MESSAGE);
            }
        }

        /// <summary>
        /// Update a volunteer
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// POST: http://dnndev.me/DesktopModules/CodeCamp/API/Event/UpdateVolunteer
        /// </remarks>
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.View)]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public HttpResponseMessage UpdateVolunteer(VolunteerInfo volunteer)
        {
            try
            {
                var updatesToProcess = false;
                var originalVolunteer = VolunteerDataAccess.GetItem(volunteer.VolunteerId, volunteer.CodeCampId);

                if (!string.Equals(volunteer.Notes, originalVolunteer.Notes))
                {
                    originalVolunteer.Notes = volunteer.Notes;
                    updatesToProcess = true;
                }

                if (originalVolunteer.CustomProperties != null)
                {
                    // parse custom properties for updates
                    foreach (var property in originalVolunteer.CustomPropertiesObj)
                    {
                        if (volunteer.CustomPropertiesObj.Any(p => p.Name == property.Name))
                        {
                            // see if the existing property needs to be updated
                            var prop = volunteer.CustomPropertiesObj.FirstOrDefault(p => p.Name == property.Name);
                            if (!string.Equals(prop.Value, property.Value))
                            {
                                property.Value = prop.Value;
                                updatesToProcess = true;
                            }
                        }
                        else
                        {
                            // delete the property
                            originalVolunteer.CustomPropertiesObj.Remove(property);
                            updatesToProcess = true;
                        }
                    }
                }

                if (volunteer.CustomPropertiesObj != null)
                {
                    // add any new properties
                    if (originalVolunteer.CustomProperties == null)
                    {
                        foreach (var property in volunteer.CustomPropertiesObj)
                        {
                            originalVolunteer.CustomPropertiesObj.Add(property);
                            updatesToProcess = true;
                        }
                    }
                    else
                    {
                        foreach (var property in volunteer.CustomPropertiesObj.Where(property => !originalVolunteer.CustomPropertiesObj.Contains(property)))
                        {
                            originalVolunteer.CustomPropertiesObj.Add(property);
                            updatesToProcess = true;
                        }
                    }
                }

                if (updatesToProcess)
                {
                    VolunteerDataAccess.UpdateItem(originalVolunteer);
                }

                var savedVolunteer = VolunteerDataAccess.GetItem(volunteer.VolunteerId, volunteer.CodeCampId);

                var response = new ServiceResponse<VolunteerInfo> { Content = savedVolunteer };

                return Request.CreateResponse(HttpStatusCode.OK, response.ObjectToJson());
            }
            catch (Exception ex)
            {
                Exceptions.LogException(ex);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ERROR_MESSAGE);
            }
        }

        #region Helper Methods

        private VolunteerInfo GenerateOrganizerVolunteerInfo(int codeCampId)
        {
            var registration = RegistrationDataAccess.GetItemByUserId(UserInfo.UserID, codeCampId);

            if (registration == null)
            {
                RegistrationDataAccess.CreateItem(new RegistrationInfo()
                {
                    CodeCampId = codeCampId,
                    IsRegistered = true,
                    Notes = "Registered automatically by Volunteer view.",
                    RegistrationDate = DateTime.Now,
                    ShirtSize = "XL",
                    UserId = UserInfo.UserID
                });

                registration = RegistrationDataAccess.GetItemByUserId(UserInfo.UserID, codeCampId);
            }

            VolunteerDataAccess.CreateItem(new VolunteerInfo()
            {
                CodeCampId = codeCampId,
                Notes = "Automatically generated by the Volunteer view.",
                RegistrationId = registration.RegistrationId
            });

            return VolunteerDataAccess.GetItemByRegistrationId(registration.RegistrationId, codeCampId);
        }

        private void LoadSupplementalProperties(ref VolunteerInfo volunteer)
        {
            volunteer.TasksClosed = VolunteerTaskDataAccess.GetVolunteerTaskCount(volunteer.VolunteerId, Globals.TASKSTATE_CLOSED);
            volunteer.TasksOpen = VolunteerTaskDataAccess.GetVolunteerTaskCount(volunteer.VolunteerId, Globals.TASKSTATE_OPEN);
            volunteer.TasksOverdue = VolunteerTaskDataAccess.GetVolunteerTaskCount(volunteer.VolunteerId, Globals.TASKSTATE_OVERDUE);
            volunteer.FullName = VolunteerDataAccess.GetItemFullName(volunteer.VolunteerId, volunteer.CodeCampId, PortalSettings.PortalId);
        }

        private List<VolunteerInfo> LoadSupplementalProperties(IEnumerable<VolunteerInfo> volunteers)
        {
            var newList = new List<VolunteerInfo>();

            foreach (var volunteer in volunteers)
            {
                var newItem = volunteer;
                LoadSupplementalProperties(ref newItem);
                newList.Add(newItem);
            }

            return newList;
        }

        #endregion
    }
}