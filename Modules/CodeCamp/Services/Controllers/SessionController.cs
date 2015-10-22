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
        /// Get all sessions
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// GET: http://dnndev.me/DesktopModules/CodeCamp/API/Event/GetSessions
        /// </remarks>
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.View)]
        [HttpGet]
        public HttpResponseMessage GetSessions(int codeCampId)
        {
            try
            {
                var sessions = SessionDataAccess.GetItems(codeCampId);
                var response = new ServiceResponse<List<SessionInfo>> { Content = sessions.ToList() };

                if (sessions == null)
                {
                    ServiceResponseHelper<List<SessionInfo>>.AddNoneFoundError("sessions", ref response);
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
        /// Get all sessions unassigned to tracks
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// GET: http://dnndev.me/DesktopModules/CodeCamp/API/Event/GetSessionsUnassigned
        /// </remarks>
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.View)]
        [HttpGet]
        public HttpResponseMessage GetSessionsUnassigned(int codeCampId)
        {
            try
            {
                var sessions = SessionDataAccess.GetItemsUnassigned(codeCampId);
                var response = new ServiceResponse<List<SessionInfo>> { Content = sessions.ToList() };

                if (sessions == null)
                {
                    ServiceResponseHelper<List<SessionInfo>>.AddNoneFoundError("sessions", ref response);
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
        /// Get all sessions by the ID of the speaker
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// GET: http://dnndev.me/DesktopModules/CodeCamp/API/Event/GetSessionsBySpeakerId
        /// </remarks>
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.View)]
        [HttpGet]
        public HttpResponseMessage GetSessionsBySpeakerId(int codeCampId, int speakerId)
        {
            try
            {
                var allSessions = SessionDataAccess.GetItems(codeCampId);
                var sessionSpeakers = SessionSpeakerDataAccess.GetItemsBySpeakerId(speakerId).Select(s => s.SessionId);
                var sessions = allSessions.Where(s => sessionSpeakers.Contains(s.SessionId));

                var response = new ServiceResponse<List<SessionInfo>> { Content = sessions.ToList() };

                if (!sessions.Any())
                {
                    ServiceResponseHelper<List<SessionInfo>>.AddNoneFoundError("sessions", ref response);
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
        /// Get all sessions by the ID of the track
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// GET: http://dnndev.me/DesktopModules/CodeCamp/API/Event/GetSessionsByTrackId
        /// </remarks>
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.View)]
        [HttpGet]
        public HttpResponseMessage GetSessionsByTrackId(int trackId, int codeCampId)
        {
            try
            {
                var sessions = SessionDataAccess.GetItemsByTrackId(trackId, codeCampId);

                var response = new ServiceResponse<List<SessionInfo>> { Content = sessions.ToList() };

                if (!sessions.Any())
                {
                    ServiceResponseHelper<List<SessionInfo>>.AddNoneFoundError("sessions", ref response);
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
        /// Get the count of all sessions by the ID of the speaker
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// GET: http://dnndev.me/DesktopModules/CodeCamp/API/Event/GetSessionCountBySpeakerId
        /// </remarks>
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.View)]
        [HttpGet]
        public HttpResponseMessage GetSessionCountBySpeakerId(int codeCampId, int speakerId)
        {
            try
            {
                var allSessions = SessionDataAccess.GetItems(codeCampId);
                var sessionSpeakers = SessionSpeakerDataAccess.GetItemsBySpeakerId(speakerId).Select(s => s.SessionId);
                var sessions = allSessions.Where(s => sessionSpeakers.Contains(s.SessionId));

                var response = new ServiceResponse<int> { Content = sessions.Any() ? sessions.Count() : 0 };

                if (!sessions.Any())
                {
                    ServiceResponseHelper<int>.AddNoneFoundError("sessions", ref response);
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
        /// Get a session
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// GET: http://dnndev.me/DesktopModules/CodeCamp/API/Event/GetSession
        /// </remarks>
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.View)]
        [HttpGet]
        public HttpResponseMessage GetSession(int itemId)
        {
            try
            {
                var session = SessionDataAccess.GetItem(itemId, ActiveModule.ModuleID);
                var response = new ServiceResponse<SessionInfo> { Content = session };

                if (session == null)
                {
                    ServiceResponseHelper<SessionInfo>.AddNoneFoundError("session", ref response);
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
        /// Delete a session
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// DELETE: http://dnndev.me/DesktopModules/CodeCamp/API/Event/DeleteSession
        /// </remarks>
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.View)]
        [ValidateAntiForgeryToken]
        [HttpDelete]
        public HttpResponseMessage DeleteSession(int itemId)
        {
            try
            {
                SessionDataAccess.DeleteItem(itemId, ActiveModule.ModuleID);

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
        /// Create a session
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// POST: http://dnndev.me/DesktopModules/CodeCamp/API/Event/CreateSession
        /// </remarks>
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.View)]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public HttpResponseMessage CreateSession(SessionInfo session)
        {
            try
            {
                var timeStamp = DateTime.Now;

                session.CreatedByDate = timeStamp;
                session.CreatedByUserId = UserInfo.UserID;
                session.LastUpdatedByDate = timeStamp;
                session.LastUpdatedByUserId = UserInfo.UserID;

                if (session.TrackId == 0)
                {
                    session.TrackId = null;
                }
                
                if (session.TimeSlotId == 0)
                {
                    session.TimeSlotId = null;
                }

                // adding a date/time placeholder because DAL doesn't know how to handle a null value
                session.ApprovedByDate = Globals.NULL_DATE;

                SessionDataAccess.CreateItem(session);

                var sessions = SessionDataAccess.GetItems(session.CodeCampId);

                var savedSession = sessions.OrderByDescending(s => s.CreatedByDate).FirstOrDefault(s => s.Title == session.Title);

                var response = new ServiceResponse<SessionInfo> { Content = savedSession };

                return Request.CreateResponse(HttpStatusCode.OK, response.ObjectToJson());
            }
            catch (Exception ex)
            {
                Exceptions.LogException(ex);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ERROR_MESSAGE);
            }
        }

        /// <summary>
        /// Update a session
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// POST: http://dnndev.me/DesktopModules/CodeCamp/API/Event/UpdateSession
        /// </remarks>
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.View)]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public HttpResponseMessage UpdateSession(SessionInfo session)
        {
            try
            {
                var updatesToProcess = false;
                var originalSession = SessionDataAccess.GetItem(session.SessionId, session.CodeCampId);

                if (!string.Equals(session.Title, originalSession.Title))
                {
                    originalSession.Title = session.Title;
                    updatesToProcess = true;
                }

                if (!string.Equals(session.Description, originalSession.Description))
                {
                    originalSession.Description = session.Description;
                    updatesToProcess = true;
                }

                if (session.AudienceLevel != originalSession.AudienceLevel)
                {
                    originalSession.AudienceLevel = session.AudienceLevel;
                    updatesToProcess = true;
                }

                if (session.TrackId != originalSession.TrackId)
                {
                    originalSession.TrackId = session.TrackId;
                    updatesToProcess = true;
                }

                if (originalSession.CustomProperties != null)
                {
                    // parse custom properties for updates
                    foreach (var property in originalSession.CustomPropertiesObj)
                    {
                        if (session.CustomPropertiesObj.Any(p => p.Name == property.Name))
                        {
                            // see if the existing property needs to be updated
                            var prop = session.CustomPropertiesObj.FirstOrDefault(p => p.Name == property.Name);
                            if (!string.Equals(prop.Value, property.Value))
                            {
                                property.Value = prop.Value;
                                updatesToProcess = true;
                            }
                        }
                        else
                        {
                            // delete the property
                            originalSession.CustomPropertiesObj.Remove(property);
                            updatesToProcess = true;
                        }
                    }
                }

                if (session.CustomPropertiesObj != null)
                {
                    // add any new properties
                    if (originalSession.CustomProperties == null)
                    {
                        foreach (var property in session.CustomPropertiesObj)
                        {
                            originalSession.CustomPropertiesObj.Add(property);
                            updatesToProcess = true;
                        }
                    }
                    else
                    {
                        foreach (var property in session.CustomPropertiesObj.Where(property => !originalSession.CustomPropertiesObj.Contains(property)))
                        {
                            originalSession.CustomPropertiesObj.Add(property);
                            updatesToProcess = true;
                        }
                    }
                }

                if (updatesToProcess)
                {
                    originalSession.LastUpdatedByDate = DateTime.Now;
                    originalSession.LastUpdatedByUserId = UserInfo.UserID;

                    SessionDataAccess.UpdateItem(session);
                }

                var savedSession = SessionDataAccess.GetItem(session.SessionId, session.CodeCampId);

                var response = new ServiceResponse<SessionInfo> { Content = savedSession };

                return Request.CreateResponse(HttpStatusCode.OK, response.ObjectToJson());
            }
            catch (Exception ex)
            {
                Exceptions.LogException(ex);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ERROR_MESSAGE);
            }
        }

        /// <summary>
        /// Assigns the session to the specified track
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// POST: http://dnndev.me/DesktopModules/CodeCamp/API/Event/AssignSessionToTrack
        /// </remarks>
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.Edit)]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public HttpResponseMessage AssignSessionToTrack(int sessionId, int trackId, int codeCampId)
        {
            try
            {
                var session = SessionDataAccess.GetItem(sessionId, codeCampId);

                session.TrackId = trackId;

                session.LastUpdatedByDate = DateTime.Now;
                session.LastUpdatedByUserId = UserInfo.UserID;

                SessionDataAccess.UpdateItem(session);

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
        /// Unassign the session from the track
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// POST: http://dnndev.me/DesktopModules/CodeCamp/API/Event/UnassignSessionFromTrack
        /// </remarks>
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.Edit)]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public HttpResponseMessage UnassignSessionFromTrack(int sessionId, int codeCampId)
        {
            try
            {
                var session = SessionDataAccess.GetItem(sessionId, codeCampId);

                session.TrackId = null;

                session.LastUpdatedByDate = DateTime.Now;
                session.LastUpdatedByUserId = UserInfo.UserID;

                SessionDataAccess.UpdateItem(session);

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