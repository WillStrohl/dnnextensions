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
        /// Get all speakers for the user
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// GET: http://dnndev.me/DesktopModules/UserGroupSuite/API/GroupManagement/GetSpeakers
        /// </remarks>
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.View)]
        [HttpGet]
        public HttpResponseMessage GetSpeakers(int userID)
        {
            try
            {
                var speakers = SpeakerDataAccess.GetItems(userID);
                var response = new ServiceResponse<List<SpeakerInfo>> { Content = speakers.ToList() };

                return Request.CreateResponse(HttpStatusCode.OK, response.ObjectToJson());
            }
            catch (Exception ex)
            {
                Exceptions.LogException(ex);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ERROR_MESSAGE);
            }
        }

        /// <summary>
        /// Get a speaker
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// GET: http://dnndev.me/DesktopModules/UserGroupSuite/API/GroupManagement/GetSpeaker
        /// </remarks>
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.View)]
        [HttpGet]
        public HttpResponseMessage GetSpeaker(int itemId, int userID)
        {
            try
            {
                var speaker = SpeakerDataAccess.GetItem(itemId, userID);
                var response = new ServiceResponse<SpeakerInfo> { Content = speaker };

                return Request.CreateResponse(HttpStatusCode.OK, response.ObjectToJson());
            }
            catch (Exception ex)
            {
                Exceptions.LogException(ex);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ERROR_MESSAGE);
            }
        }

        /// <summary>
        /// Delete a speaker
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// DELETE: http://dnndev.me/DesktopModules/UserGroupSuite/API/GroupManagement/DeleteSpeaker
        /// </remarks>
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.Edit)]
        [ValidateAntiForgeryToken]
        [HttpDelete]
        public HttpResponseMessage DeleteSpeaker(int itemId, int userID)
        {
            try
            {
                SpeakerDataAccess.DeleteItem(itemId, userID);

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
        /// Create a speaker
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// POST: http://dnndev.me/DesktopModules/UserGroupSuite/API/GroupManagement/CreateSpeaker
        /// </remarks>
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.View)]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public HttpResponseMessage CreateSpeaker(SpeakerInfo speaker)
        {
            try
            {
                var response = new ServiceResponse<SpeakerInfo>();

                speaker.CreatedOn = DateTime.Now;
                speaker.CreatedBy = UserInfo.UserID;
                speaker.LastUpdatedOn = DateTime.Now;
                speaker.LastUpdatedBy = UserInfo.UserID;

                SpeakerDataAccess.CreateItem(speaker);

                // TODO: Find a more consistent way to do this
                var speakers = SpeakerDataAccess.GetItems(speaker.UserID).OrderByDescending(r => r.SpeakerID);
                var savedSpeaker = speakers.FirstOrDefault(r => r.CreatedBy == speaker.CreatedBy);

                response.Content = savedSpeaker;

                if (savedSpeaker == null)
                {
                    ServiceResponseHelper<SpeakerInfo>.AddNoneFoundError("speaker", ref response);
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
        /// Update a speaker
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// POST: http://dnndev.me/DesktopModules/UserGroupSuite/API/GroupManagement/UpdateSpeaker
        /// </remarks>
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.View)]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public HttpResponseMessage UpdateSpeaker(SpeakerInfo speaker)
        {
            try
            {
                var originalSpeaker = SpeakerDataAccess.GetItem(speaker.SpeakerID, speaker.UserID);
                // only update the fields that would be updated from the UI to keep the DB clean
                var updatesToProcess = SpeakerHasUpdates(ref originalSpeaker, ref speaker);
                
                if (updatesToProcess)
                {
                    originalSpeaker.LastUpdatedOn = DateTime.Now;
                    originalSpeaker.LastUpdatedBy = UserInfo.UserID;

                    SpeakerDataAccess.UpdateItem(originalSpeaker);
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

        private bool SpeakerHasUpdates(ref SpeakerInfo originalSpeaker, ref SpeakerInfo newSpeaker)
        {
            var updatesToProcess = false;

            if (!string.Equals(originalSpeaker.SpeakerName, newSpeaker.SpeakerName))
            {
                originalSpeaker.SpeakerName = newSpeaker.SpeakerName;
                updatesToProcess = true;
            }

            if (!string.Equals(originalSpeaker.Website, newSpeaker.Website))
            {
                originalSpeaker.Website = newSpeaker.Website;
                updatesToProcess = true;
            }

            if (!string.Equals(originalSpeaker.Bio, newSpeaker.Bio))
            {
                originalSpeaker.Bio = newSpeaker.Bio;
                updatesToProcess = true;
            }

            if (!string.Equals(originalSpeaker.Email, newSpeaker.Email))
            {
                originalSpeaker.Email = newSpeaker.Email;
                updatesToProcess = true;
            }

            if (!string.Equals(originalSpeaker.Avatar, newSpeaker.Avatar))
            {
                originalSpeaker.Avatar = newSpeaker.Avatar;
                updatesToProcess = true;
            }

            return updatesToProcess;
        }

        #endregion
    }
}