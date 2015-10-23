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
using System.Diagnostics;
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
        /// Get all rooms
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// GET: http://dnndev.me/DesktopModules/CodeCamp/API/Event/GetRooms
        /// </remarks>
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.View)]
        [HttpGet]
        public HttpResponseMessage GetRooms(int codeCampId)
        {
            try
            {
                var rooms = RoomDataAccess.GetItems(codeCampId);
                var response = new ServiceResponse<List<RoomInfo>> { Content = rooms.ToList() };

                if (rooms == null)
                {
                    ServiceResponseHelper<List<RoomInfo>>.AddNoneFoundError("rooms", ref response);
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
        /// Get all rooms
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// GET: http://dnndev.me/DesktopModules/CodeCamp/API/Event/GetUnassignedRooms
        /// </remarks>
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.Edit)]
        [HttpGet]
        public HttpResponseMessage GetUnassignedRooms(int codeCampId)
        {
            try
            {
                var trackList = TrackDataAccess.GetItems(codeCampId).Select(t => t.RoomId);
                var rooms = RoomDataAccess.GetItems(codeCampId).Where(r => !trackList.Contains(r.RoomId));

                var response = new ServiceResponse<List<RoomInfo>> { Content = rooms.ToList() };

                if (rooms == null)
                {
                    ServiceResponseHelper<List<RoomInfo>>.AddNoneFoundError("rooms", ref response);
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
        /// Get the room assigned to the specified track
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// GET: http://dnndev.me/DesktopModules/CodeCamp/API/Event/GetRoomByTrackId
        /// </remarks>
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.View)]
        [HttpGet]
        public HttpResponseMessage GetRoomByTrackId(int trackId, int codeCampId)
        {
            try
            {
                RoomInfo room = null;
                var track = TrackDataAccess.GetItems(codeCampId).Where(t => t.TrackId == trackId).FirstOrDefault();

                if (track != null)
                {
                    room = RoomDataAccess.GetItem(track.RoomId.Value, codeCampId);
                }

                var response = new ServiceResponse<RoomInfo> { Content = room };

                if (room == null)
                {
                    ServiceResponseHelper<RoomInfo>.AddNoneFoundError("rooms", ref response);
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
        /// Get a room
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// GET: http://dnndev.me/DesktopModules/CodeCamp/API/Event/GetRoom
        /// </remarks>
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.View)]
        [HttpGet]
        public HttpResponseMessage GetRoom(int itemId, int codeCampId)
        {
            try
            {
                var room = RoomDataAccess.GetItem(itemId, codeCampId);
                var response = new ServiceResponse<RoomInfo> { Content = room };

                if (room == null)
                {
                    ServiceResponseHelper<RoomInfo>.AddNoneFoundError("room", ref response);
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
        /// Delete a room
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// DELETE: http://dnndev.me/DesktopModules/CodeCamp/API/Event/DeleteRoom
        /// </remarks>
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.Edit)]
        [ValidateAntiForgeryToken]
        [HttpDelete]
        public HttpResponseMessage DeleteRoom(int itemId, int codeCampId)
        {
            try
            {
                var tracks = TrackDataAccess.GetItems(codeCampId).Where(t => t.RoomId == itemId);

                if (tracks.Any())
                {
                    foreach (var track in tracks)
                    {
                        track.RoomId = null;
                        track.LastUpdatedByDate = DateTime.Now;
                        track.LastUpdatedByUserId = UserInfo.UserID;

                        TrackDataAccess.UpdateItem(track);
                    }
                }

                RoomDataAccess.DeleteItem(itemId, codeCampId);

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
        /// Create a room
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// POST: http://dnndev.me/DesktopModules/CodeCamp/API/Event/CreateRoom
        /// </remarks>
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.Edit)]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public HttpResponseMessage CreateRoom(RoomInfo room)
        {
            try
            {
                room.CreatedByDate = DateTime.Now;
                room.CreatedByUserId = UserInfo.UserID;
                room.LastUpdatedByDate = DateTime.Now;
                room.LastUpdatedByUserId = UserInfo.UserID;

                RoomDataAccess.CreateItem(room);

                var savedRoom = RoomDataAccess.GetItems(room.CodeCampId).OrderByDescending(r => r.CreatedByDate).FirstOrDefault();

                var response = new ServiceResponse<RoomInfo> { Content = savedRoom };

                return Request.CreateResponse(HttpStatusCode.OK, response.ObjectToJson());
            }
            catch (Exception ex)
            {
                Exceptions.LogException(ex);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ERROR_MESSAGE);
            }
        }

        /// <summary>
        /// Update a room
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// POST: http://dnndev.me/DesktopModules/CodeCamp/API/Event/UpdateRoom
        /// </remarks>
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.Edit)]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public HttpResponseMessage UpdateRoom(RoomInfo room)
        {
            try
            {
                var originalRoom = RoomDataAccess.GetItem(room.RoomId, room.CodeCampId);
                var updatesToProcess = false;

                if (!string.Equals(originalRoom.RoomName, room.RoomName))
                {
                    originalRoom.RoomName = room.RoomName;
                    updatesToProcess = true;
                }

                if (!string.Equals(originalRoom.Description, room.Description))
                {
                    originalRoom.Description = room.Description;
                    updatesToProcess = true;
                }

                if (originalRoom.MaximumCapacity != room.MaximumCapacity)
                {
                    originalRoom.MaximumCapacity = room.MaximumCapacity;
                    updatesToProcess = true;
                }

                if (updatesToProcess)
                {
                    room.LastUpdatedByDate = DateTime.Now;
                    room.LastUpdatedByUserId = UserInfo.UserID;

                    RoomDataAccess.UpdateItem(room);
                }

                var savedRoom = RoomDataAccess.GetItem(room.RoomId, room.CodeCampId);

                var response = new ServiceResponse<RoomInfo> { Content = savedRoom };

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