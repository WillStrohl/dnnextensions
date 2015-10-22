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
        /// Get all tracks
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// GET: http://dnndev.me/DesktopModules/CodeCamp/API/Event/GetTracks
        /// </remarks>
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.View)]
        [HttpGet]
        public HttpResponseMessage GetTracks(int codeCampId)
        {
            try
            {
                var tracks = TrackDataAccess.GetItems(codeCampId);
                var response = new ServiceResponse<List<TrackInfo>> { Content = tracks.ToList() };

                if (tracks == null)
                {
                    ServiceResponseHelper<List<TrackInfo>>.AddNoneFoundError("tracks", ref response);
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
        /// Get a track
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// GET: http://dnndev.me/DesktopModules/CodeCamp/API/Event/GetTrack
        /// </remarks>
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.View)]
        [HttpGet]
        public HttpResponseMessage GetTrack(int itemId, int codeCampId)
        {
            try
            {
                var track = TrackDataAccess.GetItem(itemId, codeCampId);
                var response = new ServiceResponse<TrackInfo> { Content = track };

                if (track == null)
                {
                    ServiceResponseHelper<TrackInfo>.AddNoneFoundError("track", ref response);
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
        /// Delete a track
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// DELETE: http://dnndev.me/DesktopModules/CodeCamp/API/Event/DeleteTrack
        /// </remarks>
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.Edit)]
        [ValidateAntiForgeryToken]
        [HttpDelete]
        public HttpResponseMessage DeleteTrack(int itemId, int codeCampId)
        {
            try
            {
                var sessions = SessionDataAccess.GetItemsByTrackId(itemId, codeCampId);

                if (sessions.Any())
                {
                    foreach (var session in sessions)
                    {
                        session.TrackId = null;
                        SessionDataAccess.UpdateItem(session);
                    }
                }

                TrackDataAccess.DeleteItem(itemId, codeCampId);

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
        /// Create a track
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// POST: http://dnndev.me/DesktopModules/CodeCamp/API/Event/CreateTrack
        /// </remarks>
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.Edit)]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public HttpResponseMessage CreateTrack(TrackInfo track)
        {
            try
            {
                var timeStamp = DateTime.Now;

                track.CreatedByDate = timeStamp;
                track.CreatedByUserId = UserInfo.UserID;
                track.LastUpdatedByDate = timeStamp;
                track.LastUpdatedByUserId = UserInfo.UserID;

                TrackDataAccess.CreateItem(track);

                var tracks = TrackDataAccess.GetItems(track.CodeCampId);

                var savedTrack = tracks.OrderByDescending(s => s.CreatedByDate).FirstOrDefault(s => s.Title == track.Title);

                var response = new ServiceResponse<TrackInfo> { Content = savedTrack };

                return Request.CreateResponse(HttpStatusCode.OK, response.ObjectToJson());
            }
            catch (Exception ex)
            {
                Exceptions.LogException(ex);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ERROR_MESSAGE);
            }
        }

        /// <summary>
        /// Update a track
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// POST: http://dnndev.me/DesktopModules/CodeCamp/API/Event/UpdateTrack
        /// </remarks>
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.Edit)]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public HttpResponseMessage UpdateTrack(TrackInfo track)
        {
            try
            {
                var updatesToProcess = false;
                var originalTrack = TrackDataAccess.GetItem(track.TrackId, track.CodeCampId);

                if (!string.Equals(track.Title, originalTrack.Title))
                {
                    originalTrack.Title = track.Title;
                    updatesToProcess = true;
                }

                if (!string.Equals(track.Description, originalTrack.Description))
                {
                    originalTrack.Description = track.Description;
                    updatesToProcess = true;
                }

                if (originalTrack.CustomProperties != null)
                {
                    // parse custom properties for updates
                    foreach (var property in originalTrack.CustomPropertiesObj)
                    {
                        if (track.CustomPropertiesObj.Any(p => p.Name == property.Name))
                        {
                            // see if the existing property needs to be updated
                            var prop = track.CustomPropertiesObj.FirstOrDefault(p => p.Name == property.Name);
                            if (!string.Equals(prop.Value, property.Value))
                            {
                                property.Value = prop.Value;
                                updatesToProcess = true;
                            }
                        }
                        else
                        {
                            // delete the property
                            originalTrack.CustomPropertiesObj.Remove(property);
                            updatesToProcess = true;
                        }
                    }
                }

                if (track.CustomPropertiesObj != null)
                {
                    // add any new properties
                    if (originalTrack.CustomProperties == null)
                    {
                        foreach (var property in track.CustomPropertiesObj)
                        {
                            originalTrack.CustomPropertiesObj.Add(property);
                            updatesToProcess = true;
                        }
                    }
                    else
                    {
                        foreach (var property in track.CustomPropertiesObj.Where(property => !originalTrack.CustomPropertiesObj.Contains(property)))
                        {
                            originalTrack.CustomPropertiesObj.Add(property);
                            updatesToProcess = true;
                        }
                    }
                }

                if (updatesToProcess)
                {
                    originalTrack.LastUpdatedByDate = DateTime.Now;
                    originalTrack.LastUpdatedByUserId = UserInfo.UserID;

                    TrackDataAccess.UpdateItem(originalTrack);
                }

                var savedTrack = TrackDataAccess.GetItem(track.TrackId, track.CodeCampId);

                var response = new ServiceResponse<TrackInfo> { Content = savedTrack };

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