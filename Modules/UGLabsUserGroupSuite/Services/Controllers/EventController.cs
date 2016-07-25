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

namespace DNNCommunity.Modules.UserGroupSuite.Services
{
    // TODO: add validation catches and formal error responses for all end points

    /// <summary>
    /// This is a partial class that spans multiple class files, in order to keep the code manageable. Each method is necessary to support the front end SPA implementation.
    /// </summary>
    /// <remarks>
    /// The SupportModules attribute will require that all API calls set and include module headers, event GET requests. Even Fiddler will return 401 Unauthorized errors.
    /// </remarks>
    [SupportedModules("CodeCampEvents")]
    public partial class EventController : ServiceBase
    {
        /// <summary>
        /// Get an event
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// GET: http://dnndev.me/DesktopModules/CodeCamp/API/Event/GetEvents
        /// </remarks>
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.View)]
        [HttpGet]
        public HttpResponseMessage GetEvents()
        {
            try
            {
                var codeCamps = CodeCampDataAccess.GetItems(ActiveModule.ModuleID);
                var response = new ServiceResponse<List<CodeCampInfo>> { Content = codeCamps.ToList() };

                if (codeCamps == null)
                {
                    ServiceResponseHelper<List<CodeCampInfo>>.AddNoneFoundError("CodeCampInfo", ref response);
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
        /// Get an event
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// GET: http://dnndev.me/DesktopModules/CodeCamp/API/Event/GetEventByModuleId
        /// </remarks>
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.View)]
        [HttpGet]
        public HttpResponseMessage GetEventByModuleId()
        {
            try
            {
                var codeCamp = CodeCampDataAccess.GetItemByModuleId(ActiveModule.ModuleID);

                var response = new ServiceResponse<CodeCampInfo> { Content = codeCamp };

                if (codeCamp == null)
                {
                    ServiceResponseHelper<CodeCampInfo>.AddNoneFoundError("CodeCampInfo", ref response);
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
        /// Get an event
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// GET: http://dnndev.me/DesktopModules/CodeCamp/API/Event/GetEvent
        /// </remarks>
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.View)]
        [HttpGet]
        public HttpResponseMessage GetEvent(int itemId)
        {
            try
            {
                var codeCamp = CodeCampDataAccess.GetItem(itemId, ActiveModule.ModuleID);
                var response = new ServiceResponse<CodeCampInfo> { Content = codeCamp };

                if (codeCamp == null)
                {
                    ServiceResponseHelper<CodeCampInfo>.AddNoneFoundError("CodeCampInfo", ref response);
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
        /// Delete an event
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// DELETE: http://dnndev.me/DesktopModules/CodeCamp/API/Event/DeleteEvent
        /// </remarks>
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.Edit)]
        [ValidateAntiForgeryToken]
        [HttpDelete]
        public HttpResponseMessage DeleteEvent(int itemId)
        {
            try
            {
                CodeCampDataAccess.DeleteItem(itemId, ActiveModule.ModuleID);
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
        /// Create an event
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// POST: http://dnndev.me/DesktopModules/CodeCamp/API/Event/CeateEvent
        /// </remarks>
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.Edit)]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public HttpResponseMessage CreateEvent(CodeCampInfo newEvent)
        {
            try
            {
                newEvent.CreatedByDate = DateTime.Now;
                newEvent.CreatedByUserId = UserInfo.UserID;
                newEvent.LastUpdatedByDate = DateTime.Now;
                newEvent.LastUpdatedByUserId = UserInfo.UserID;
                newEvent.ModuleId = ActiveModule.ModuleID;

                CodeCampDataAccess.CreateItem(newEvent);

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
        /// Update an event
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// POST: http://dnndev.me/DesktopModules/CodeCamp/API/Event/UpdateEvent
        /// </remarks>
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.Edit)]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public HttpResponseMessage UpdateEvent(CodeCampInfo codeCamp)
        {
            try
            {
                var originalEvent = CodeCampDataAccess.GetItem(codeCamp.CodeCampId, codeCamp.ModuleId);
                var updatesToProcess = EventHasUpdates(ref originalEvent, ref codeCamp);

                if (updatesToProcess)
                {
                    originalEvent.LastUpdatedByDate = DateTime.Now;
                    originalEvent.LastUpdatedByUserId = UserInfo.UserID;

                    CodeCampDataAccess.UpdateItem(originalEvent);
                }

                var savedEvent = CodeCampDataAccess.GetItem(originalEvent.CodeCampId, originalEvent.ModuleId);

                var response = new ServiceResponse<CodeCampInfo> { Content = savedEvent };

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
        /// GET: http://dnndev.me/DesktopModules/CodeCamp/API/Event/UserCanEditEvent
        /// </remarks>
        [AllowAnonymous]
        [HttpGet]
        public HttpResponseMessage UserCanEditEvent(int itemId)
        {
            ServiceResponse<string> response = null;

            if (UserInfo.IsSuperUser || UserInfo.IsInRole(PortalSettings.AdministratorRoleName) || ModulePermissionController.HasModulePermission(ActiveModule.ModulePermissions, "Edit"))
            {
                response = new ServiceResponse<string>() { Content = Globals.RESPONSE_SUCCESS };
            }
            else
            {
                var codeCamp = CodeCampDataAccess.GetItem(itemId, ActiveModule.ModuleID);

                if (codeCamp != null && codeCamp.CreatedByUserId == UserInfo.UserID || codeCamp.LastUpdatedByUserId == UserInfo.UserID)
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

        /// <summary>
        /// Get the agenda for the event
        /// </summary>
        /// <returns>This will return the event itself, the days of the event, timeslots in each day, sessions in each timeslot, and speakers for each session</returns>
        /// <remarks>
        /// GET: http://dnndev.me/DesktopModules/CodeCamp/API/Event/GetAgenda
        /// </remarks>
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.View)]
        [HttpGet]
        public HttpResponseMessage GetAgenda(int codeCampId)
        {
            try
            {
                var agenda = new AgendaInfo();
                agenda.CodeCamp = CodeCampDataAccess.GetItem(codeCampId, ActiveModule.ModuleID);

                if (agenda.CodeCamp != null)
                {
                    var slotsToOrder = TimeSlotDataAccess.GetItems(codeCampId);
                    var timeSlots = TimeSlotInfoController.SortTimeSlots(slotsToOrder);
                    var timeSlotCount = timeSlots.Count();

                    // determine how many days the event lasts for
                    agenda.NumberOfDays = (int)(agenda.CodeCamp.EndDate - agenda.CodeCamp.BeginDate).TotalDays + 1;

                    // iterate through each day
                    agenda.EventDays = new List<EventDayInfo>();

                    var dayCount = 0;
                    while (dayCount <= agenda.NumberOfDays - 1)
                    {
                        var eventDate = agenda.CodeCamp.BeginDate.AddDays(dayCount);

                        var eventDay = new EventDayInfo()
                        {
                            Index = dayCount,
                            Day = eventDate.Day,
                            Month = eventDate.Month,
                            Year = eventDate.Year,
                            TimeStamp = eventDate
                        };

                        eventDay.TimeSlots = new List<AgendaTimeSlotInfo>();

                        // iterate through each timeslot
                        foreach (var timeSlot in timeSlots)
                        {
                            var slot = new AgendaTimeSlotInfo(timeSlot);

                            if (!timeSlot.SpanAllTracks)
                            {
                                // iterate through each session
                                slot.Sessions = SessionDataAccess.GetItemsByTimeSlotIdByPage(slot.TimeSlotId, codeCampId, dayCount + 1, timeSlotCount).ToList();

                                // iterate through each speaker
                                foreach (var session in slot.Sessions)
                                {
                                    session.Speakers = SpeakerDataAccess.GetSpeakersForCollection(session.SessionId, codeCampId);
                                }
                            }
                            else
                            {
                                // add the full span session item
                                // TODO: allow for items to be added to full span timeslots
                            }

                            eventDay.TimeSlots.Add(slot);
                        }

                        agenda.EventDays.Add(eventDay);

                        dayCount++;
                    }
                }

                var response = new ServiceResponse<AgendaInfo> { Content = agenda };

                if (agenda.CodeCamp == null)
                {
                    ServiceResponseHelper<AgendaInfo>.AddNoneFoundError("AgendaInfo", ref response);
                }

                return Request.CreateResponse(HttpStatusCode.OK, response.ObjectToJson());
            }
            catch (Exception ex)
            {
                Exceptions.LogException(ex);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ERROR_MESSAGE);
            }
        }

        #region Private Helper Methods

        private bool EventHasUpdates(ref CodeCampInfo originalCodeCamp, ref CodeCampInfo newCodeCamp)
        {
            var updatesToProcess = false;

            if (!string.Equals(newCodeCamp.Title, originalCodeCamp.Title))
            {
                originalCodeCamp.Title = newCodeCamp.Title;
                updatesToProcess = true;
            }

            if (!string.Equals(newCodeCamp.Description, originalCodeCamp.Description))
            {
                originalCodeCamp.Description = newCodeCamp.Description;
                updatesToProcess = true;
            }

            if (newCodeCamp.MaximumCapacity != originalCodeCamp.MaximumCapacity)
            {
                originalCodeCamp.MaximumCapacity = newCodeCamp.MaximumCapacity;
                updatesToProcess = true;
            }

            if (newCodeCamp.BeginDate != originalCodeCamp.BeginDate)
            {
                originalCodeCamp.BeginDate = newCodeCamp.BeginDate;
                updatesToProcess = true;
            }

            if (newCodeCamp.EndDate != originalCodeCamp.EndDate)
            {
                originalCodeCamp.EndDate = newCodeCamp.EndDate;
                updatesToProcess = true;
            }

            if (newCodeCamp.EndDate != originalCodeCamp.EndDate)
            {
                originalCodeCamp.EndDate = newCodeCamp.EndDate;
                updatesToProcess = true;
            }

            if (newCodeCamp.ShowShirtSize != originalCodeCamp.ShowShirtSize)
            {
                originalCodeCamp.ShowShirtSize = newCodeCamp.ShowShirtSize;
                updatesToProcess = true;
            }

            if (newCodeCamp.ShowAuthor != originalCodeCamp.ShowAuthor)
            {
                originalCodeCamp.ShowAuthor = newCodeCamp.ShowAuthor;
                updatesToProcess = true;
            }

            if (newCodeCamp.RegistrationActive != originalCodeCamp.RegistrationActive)
            {
                originalCodeCamp.RegistrationActive = newCodeCamp.RegistrationActive;
                updatesToProcess = true;
            }

            if (originalCodeCamp.CustomProperties != null)
            {
                // parse custom properties for updates
                foreach (var property in originalCodeCamp.CustomPropertiesObj)
                {
                    if (newCodeCamp.CustomPropertiesObj.Any(p => p.Name == property.Name))
                    {
                        // see if the existing property needs to be updated
                        var prop = newCodeCamp.CustomPropertiesObj.FirstOrDefault(p => p.Name == property.Name);
                        if (!string.Equals(prop.Value, property.Value))
                        {
                            property.Value = prop.Value;
                            updatesToProcess = true;
                        }
                    }
                    else
                    {
                        // delete the property
                        originalCodeCamp.CustomPropertiesObj.Remove(property);
                        updatesToProcess = true;
                    }
                }
            }

            if (newCodeCamp.CustomPropertiesObj != null)
            {
                // add any new properties
                if (originalCodeCamp.CustomProperties == null)
                {
                    foreach (var property in newCodeCamp.CustomPropertiesObj)
                    {
                        originalCodeCamp.CustomPropertiesObj.Add(property);
                        updatesToProcess = true;
                    }
                }
                else
                {
                    var camp = originalCodeCamp;
                    foreach (var property in newCodeCamp.CustomPropertiesObj.Where(property => !camp.CustomPropertiesObj.Contains(property)))
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