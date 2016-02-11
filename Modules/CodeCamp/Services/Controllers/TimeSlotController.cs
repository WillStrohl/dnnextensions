/*
 * Copyright (c) 2015, Will Strohl
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
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Web.Api;
using WillStrohl.Modules.CodeCamp.Components;
using WillStrohl.Modules.CodeCamp.Entities;

namespace WillStrohl.Modules.CodeCamp.Services
{
    public partial class EventController
    {
        /// <summary>
        /// Get all time slots
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// GET: http://dnndev.me/DesktopModules/CodeCamp/API/Event/GetTimeSlots
        /// </remarks>
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.View)]
        [HttpGet]
        public HttpResponseMessage GetTimeSlots(int codeCampId)
        {
            try
            {
                var slotsToOrder = TimeSlotDataAccess.GetItems(codeCampId);
                var timeSlots = SortTimeSlots(slotsToOrder);

                var response = new ServiceResponse<List<TimeSlotInfo>> { Content = timeSlots.ToList() };

                if (timeSlots == null)
                {
                    ServiceResponseHelper<List<TimeSlotInfo>>.AddNoneFoundError("timeSlots", ref response);
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
        /// Get a time slot
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// GET: http://dnndev.me/DesktopModules/CodeCamp/API/Event/GetTimeSlot
        /// </remarks>
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.View)]
        [HttpGet]
        public HttpResponseMessage GetTimeSlot(int itemId, int codeCampId)
        {
            try
            {
                var timeSlot = TimeSlotDataAccess.GetItem(itemId, codeCampId);

                // removing this prevented the saved/retrieved time from being offset to being about 4 hours off
                //if (timeSlot != null)
                //{
                //    timeSlot.BeginTime = timeSlot.BeginTime.ToLocalTime();
                //    timeSlot.EndTime = timeSlot.EndTime.ToLocalTime();
                //}

                var response = new ServiceResponse<TimeSlotInfo> { Content = timeSlot };

                if (timeSlot == null)
                {
                    ServiceResponseHelper<TimeSlotInfo>.AddNoneFoundError("timeSlot", ref response);
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
        /// Delete a time slot
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// DELETE: http://dnndev.me/DesktopModules/CodeCamp/API/Event/DeleteTimeSlot
        /// </remarks>
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.Edit)]
        [ValidateAntiForgeryToken]
        [HttpDelete]
        public HttpResponseMessage DeleteTimeSlot(int itemId, int codeCampId)
        {
            try
            {
                var sessions = SessionDataAccess.GetItemsByTimeSlotId(itemId, codeCampId);

                if (sessions.Any())
                {
                    foreach (var session in sessions)
                    {
                        session.TimeSlotId = null;
                        session.LastUpdatedByDate = DateTime.Now;
                        session.LastUpdatedByUserId = UserInfo.UserID;

                        SessionDataAccess.UpdateItem(session);
                    }
                }

                TimeSlotDataAccess.DeleteItem(itemId, codeCampId);

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
        /// Create a time slot
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// POST: http://dnndev.me/DesktopModules/CodeCamp/API/Event/CreateTimeSlot
        /// </remarks>
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.Edit)]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public HttpResponseMessage CreateTimeSlot(TimeSlotInfo timeSlot)
        {
            try
            {
                var timeStamp = DateTime.Now;

                timeSlot.CreatedByDate = timeStamp;
                timeSlot.CreatedByUserId = UserInfo.UserID;
                timeSlot.LastUpdatedByDate = timeStamp;
                timeSlot.LastUpdatedByUserId = UserInfo.UserID;

                TimeSlotDataAccess.CreateItem(timeSlot);

                var timeSlots = TimeSlotDataAccess.GetItems(timeSlot.CodeCampId);

                var savedTimeSlot = timeSlots.OrderByDescending(s => s.CreatedByDate).FirstOrDefault(s => s.BeginTime == timeSlot.BeginTime);

                // removing this prevented the saved/retrieved time from being offset to being about 4 hours off
                //if (savedTimeSlot != null)
                //{
                //    savedTimeSlot.BeginTime = savedTimeSlot.BeginTime.ToLocalTime();
                //    savedTimeSlot.EndTime = savedTimeSlot.EndTime.ToLocalTime();
                //}

                var response = new ServiceResponse<TimeSlotInfo> { Content = savedTimeSlot };

                return Request.CreateResponse(HttpStatusCode.OK, response.ObjectToJson());
            }
            catch (Exception ex)
            {
                Exceptions.LogException(ex);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ERROR_MESSAGE);
            }
        }

        /// <summary>
        /// Update a time slot
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// POST: http://dnndev.me/DesktopModules/CodeCamp/API/Event/UpdateTimeSlot
        /// </remarks>
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.Edit)]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public HttpResponseMessage UpdateTimeSlot(TimeSlotInfo timeSlot)
        {
            try
            {
                var updatesToProcess = false;
                var originalTimeSlot = TimeSlotDataAccess.GetItem(timeSlot.TimeSlotId, timeSlot.CodeCampId);

                updatesToProcess = TimeSlotHasUpdates(ref originalTimeSlot, ref timeSlot);

                if (updatesToProcess)
                {
                    originalTimeSlot.LastUpdatedByDate = DateTime.Now;
                    originalTimeSlot.LastUpdatedByUserId = UserInfo.UserID;

                    TimeSlotDataAccess.UpdateItem(originalTimeSlot);
                }

                var savedTimeSlot = TimeSlotDataAccess.GetItem(timeSlot.TimeSlotId, timeSlot.CodeCampId);

                // removing this prevented the saved/retrieved time from being offset to being about 4 hours off
                //if (savedTimeSlot != null)
                //{
                //    savedTimeSlot.BeginTime = savedTimeSlot.BeginTime.ToLocalTime();
                //    savedTimeSlot.EndTime = savedTimeSlot.EndTime.ToLocalTime();
                //}

                var response = new ServiceResponse<TimeSlotInfo> { Content = savedTimeSlot };

                return Request.CreateResponse(HttpStatusCode.OK, response.ObjectToJson());
            }
            catch (Exception ex)
            {
                Exceptions.LogException(ex);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ERROR_MESSAGE);
            }
        }

        /// <summary>
        /// Sets the time slot as the official time for the session
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// GET: http://dnndev.me/DesktopModules/CodeCamp/API/Event/AssignSessionToTimeSlot
        /// </remarks>
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.Edit)]
        [HttpPost]
        public HttpResponseMessage AssignSessionToTimeSlot(int sessionId, int timeSlotId, int codeCampId)
        {
            try
            {
                var session = SessionDataAccess.GetItem(sessionId, codeCampId);

                if (session != null)
                {
                    session.TimeSlotId = timeSlotId;
                    session.LastUpdatedByDate = DateTime.Now;
                    session.LastUpdatedByUserId = UserInfo.UserID;

                    SessionDataAccess.UpdateItem(session);
                }

                var response = new ServiceResponse<string> { Content = SUCCESS_MESSAGE };

                if (session == null)
                {
                    ServiceResponseHelper<string>.AddNoneFoundError("session", ref response);
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
        /// Unassignes the time slot from the session
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// GET: http://dnndev.me/DesktopModules/CodeCamp/API/Event/UnassignTimeSlotFromSession
        /// </remarks>
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.Edit)]
        [HttpGet]
        public HttpResponseMessage UnassignTimeSlotFromSession(int sessionId, int timeSlotId, int codeCampId)
        {
            try
            {
                var session = SessionDataAccess.GetItem(sessionId, codeCampId);

                if (session != null)
                {
                    session.TimeSlotId = null;
                    session.LastUpdatedByDate = DateTime.Now;
                    session.LastUpdatedByUserId = UserInfo.UserID;

                    SessionDataAccess.UpdateItem(session);
                }

                var response = new ServiceResponse<string> { Content = SUCCESS_MESSAGE };

                if (session == null)
                {
                    ServiceResponseHelper<string>.AddNoneFoundError("session", ref response);
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

        private bool TimeSlotHasUpdates(ref TimeSlotInfo originalTimeSlot, ref TimeSlotInfo timeSlot)
        {
            var updatesToProcess = false;

            if (timeSlot.BeginTime.ToUniversalTime() != originalTimeSlot.BeginTime)
            {
                originalTimeSlot.BeginTime = timeSlot.BeginTime;
                updatesToProcess = true;
            }

            if (timeSlot.EndTime.ToUniversalTime() != originalTimeSlot.EndTime)
            {
                originalTimeSlot.EndTime = timeSlot.EndTime;
                updatesToProcess = true;
            }

            if (!string.Equals(timeSlot.AgendaText, originalTimeSlot.AgendaText))
            {
                originalTimeSlot.AgendaText = timeSlot.AgendaText;
                updatesToProcess = true;
            }

            if (timeSlot.SpanAllTracks != originalTimeSlot.SpanAllTracks)
            {
                originalTimeSlot.SpanAllTracks = timeSlot.SpanAllTracks;
                updatesToProcess = true;
            }

            if (timeSlot.IncludeInDropDowns != originalTimeSlot.IncludeInDropDowns)
            {
                originalTimeSlot.IncludeInDropDowns = timeSlot.IncludeInDropDowns;
                updatesToProcess = true;
            }

            if (originalTimeSlot.CustomProperties != null)
            {
                // parse custom properties for updates
                foreach (var property in originalTimeSlot.CustomPropertiesObj)
                {
                    if (timeSlot.CustomPropertiesObj.Any(p => p.Name == property.Name))
                    {
                        // see if the existing property needs to be updated
                        var prop = timeSlot.CustomPropertiesObj.FirstOrDefault(p => p.Name == property.Name);
                        if (!string.Equals(prop.Value, property.Value))
                        {
                            property.Value = prop.Value;
                            updatesToProcess = true;
                        }
                    }
                    else
                    {
                        // delete the property
                        originalTimeSlot.CustomPropertiesObj.Remove(property);
                        updatesToProcess = true;
                    }
                }
            }

            if (timeSlot.CustomPropertiesObj != null)
            {
                // add any new properties
                if (originalTimeSlot.CustomProperties == null)
                {
                    foreach (var property in timeSlot.CustomPropertiesObj)
                    {
                        originalTimeSlot.CustomPropertiesObj.Add(property);
                        updatesToProcess = true;
                    }
                }
                else
                {
                    TimeSlotInfo slot = originalTimeSlot;
                    foreach (var property in timeSlot.CustomPropertiesObj.Where(property => !slot.CustomPropertiesObj.Contains(property)))
                    {
                        slot.CustomPropertiesObj.Add(property);
                        updatesToProcess = true;
                    }
                }
            }

            return updatesToProcess;
        }

        protected IEnumerable<TimeSlotInfo> SortTimeSlots(IEnumerable<TimeSlotInfo> timeSlots)
        {
            var index = 0;

            // first, ensure that the times all have the same dates
            foreach (var timeSlot in timeSlots)
            {
                var beginTime = timeSlot.BeginTime;
                var endTime = timeSlot.EndTime;

                timeSlot.BeginTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, beginTime.Hour, beginTime.Minute, 0);
                timeSlot.EndTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, endTime.Hour, endTime.Minute, 0);
            }

            // now sort by the time
            foreach (var timeSlot in timeSlots.OrderBy(t => t.BeginTime))
            {
                timeSlot.SortOrder = index;

                index++;
            }

            return timeSlots;
        }

        #endregion
    }
}