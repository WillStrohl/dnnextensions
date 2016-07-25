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
using System.Net.Http.Formatting;
using System.Web;
using System.Web.Http;
using DotNetNuke.Security;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Web.Api;
using WillStrohl.Modules.CodeCamp.Components;
using WillStrohl.Modules.CodeCamp.Entities;

namespace DNNCommunity.Modules.UserGroupSuite.Services
{
    public partial class EventController
    {
        /// <summary>
        /// Get all speakers
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// GET: http://dnndev.me/DesktopModules/CodeCamp/API/Event/GetSpeakers
        /// </remarks>
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.View)]
        [HttpGet]
        public HttpResponseMessage GetSpeakers(int codeCampId)
        {
            try
            {
                var speakers = SpeakerDataAccess.GetItems(codeCampId);
                var response = new ServiceResponse<List<SpeakerInfo>> { Content = speakers.ToList() };

                if (speakers == null)
                {
                    ServiceResponseHelper<List<SpeakerInfo>>.AddNoneFoundError("speakers", ref response);
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
        /// Get a speaker
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// GET: http://dnndev.me/DesktopModules/CodeCamp/API/Event/GetSpeaker
        /// </remarks>
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.View)]
        [HttpGet]
        public HttpResponseMessage GetSpeaker(int itemId, int codeCampId)
        {
            try
            {
                var speaker = SpeakerDataAccess.GetItem(itemId, codeCampId);
                var response = new ServiceResponse<SpeakerInfo> { Content = speaker };

                if (speaker == null)
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
        /// Get a speaker by their registration id
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// GET: http://dnndev.me/DesktopModules/CodeCamp/API/Event/GetSpeakerByRegistrationId
        /// </remarks>
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.View)]
        [HttpGet]
        public HttpResponseMessage GetSpeakerByRegistrationId(int codeCampId, int registrationId)
        {
            try
            {
                var speaker = SpeakerDataAccess.GetItemByRegistrationId(codeCampId, registrationId);
                var response = new ServiceResponse<SpeakerInfo> { Content = speaker };

                if (speaker == null)
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
        /// Delete a speaker
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// DELETE: http://dnndev.me/DesktopModules/CodeCamp/API/Event/DeleteSpeaker
        /// </remarks>
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.Edit)]
        [ValidateAntiForgeryToken]
        [HttpDelete]
        public HttpResponseMessage DeleteSpeaker(int itemId, int codeCampId)
        {
            try
            {
                SpeakerDataAccess.DeleteItem(itemId, codeCampId);

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
        /// POST: http://dnndev.me/DesktopModules/CodeCamp/API/Event/CreateSpeaker
        /// </remarks>
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.View)]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public HttpResponseMessage CreateSpeaker(SpeakerInfo speaker)
        {
            try
            {
                var timeStamp = DateTime.Now;

                speaker.CreatedByDate = timeStamp;
                speaker.CreatedByUserId = UserInfo.UserID;
                speaker.LastUpdatedByDate = timeStamp;
                speaker.LastUpdatedByUserId = UserInfo.UserID;

                if (speaker.RegistrationId == 0)
                {
                    speaker.RegistrationId = null;
                }

                SpeakerDataAccess.CreateItem(speaker);

                var speakers = SpeakerDataAccess.GetItems(speaker.CodeCampId);

                var savedSpeaker = speakers.OrderByDescending(s => s.CreatedByDate).FirstOrDefault(s => s.Email == speaker.Email);

                var response = new ServiceResponse<SpeakerInfo> { Content = savedSpeaker };

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
        /// POST: http://dnndev.me/DesktopModules/CodeCamp/API/Event/UpdateSpeaker
        /// </remarks>
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.View)]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public HttpResponseMessage UpdateSpeaker(SpeakerInfo speaker)
        {
            try
            {
                var updatesToProcess = false;
                var originalSpeaker = SpeakerDataAccess.GetItem(speaker.SpeakerId, speaker.CodeCampId);

                if (!string.Equals(speaker.SpeakerName, originalSpeaker.SpeakerName))
                {
                    originalSpeaker.SpeakerName = speaker.SpeakerName;
                    updatesToProcess = true;
                }

                if (!string.Equals(speaker.URL, originalSpeaker.URL))
                {
                    originalSpeaker.URL = speaker.URL;
                    updatesToProcess = true;
                }

                if (!string.Equals(speaker.Email, originalSpeaker.Email))
                {
                    originalSpeaker.Email = speaker.Email;
                    updatesToProcess = true;
                }

                if (!string.Equals(speaker.CompanyName, originalSpeaker.CompanyName))
                {
                    originalSpeaker.CompanyName = speaker.CompanyName;
                    updatesToProcess = true;
                }

                if (!string.Equals(speaker.CompanyTitle, originalSpeaker.CompanyTitle))
                {
                    originalSpeaker.CompanyTitle = speaker.CompanyTitle;
                    updatesToProcess = true;
                }

                if (!string.Equals(speaker.Bio, originalSpeaker.Bio))
                {
                    originalSpeaker.Bio = speaker.Bio;
                    updatesToProcess = true;
                }

                if (speaker.IsAuthor != originalSpeaker.IsAuthor)
                {
                    originalSpeaker.IsAuthor = speaker.IsAuthor;
                    updatesToProcess = true;
                }

                if (originalSpeaker.CustomProperties != null)
                {
                    // parse custom properties for updates
                    foreach (var property in originalSpeaker.CustomPropertiesObj)
                    {
                        if (speaker.CustomPropertiesObj.Any(p => p.Name == property.Name))
                        {
                            // see if the existing property needs to be updated
                            var prop = speaker.CustomPropertiesObj.FirstOrDefault(p => p.Name == property.Name);
                            if (!string.Equals(prop.Value, property.Value))
                            {
                                property.Value = prop.Value;
                                updatesToProcess = true;
                            }
                        }
                        else
                        {
                            // delete the property
                            originalSpeaker.CustomPropertiesObj.Remove(property);
                            updatesToProcess = true;
                        }
                    }
                }

                if (speaker.CustomPropertiesObj != null)
                {
                    // add any new properties
                    if (originalSpeaker.CustomProperties == null)
                    {
                        foreach (var property in speaker.CustomPropertiesObj)
                        {
                            originalSpeaker.CustomPropertiesObj.Add(property);
                            updatesToProcess = true;
                        }
                    }
                    else
                    {
                        foreach (var property in speaker.CustomPropertiesObj.Where(property => !originalSpeaker.CustomPropertiesObj.Contains(property)))
                        {
                            originalSpeaker.CustomPropertiesObj.Add(property);
                            updatesToProcess = true;
                        }
                    }
                }

                if (updatesToProcess)
                {
                    SpeakerDataAccess.UpdateItem(originalSpeaker);
                }

                var savedSpeaker = SpeakerDataAccess.GetItem(speaker.SpeakerId, speaker.CodeCampId);

                var response = new ServiceResponse<SpeakerInfo> { Content = savedSpeaker };

                return Request.CreateResponse(HttpStatusCode.OK, response.ObjectToJson());
            }
            catch (Exception ex)
            {
                Exceptions.LogException(ex);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ERROR_MESSAGE);
            }
        }

        /// <summary>
        /// Saves a new or updates an existing avatar for a speaker
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// POST: http://dnndev.me/DesktopModules/CodeCamp/API/Event/UpdateSpeakerAvatar
        /// </remarks>
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.Edit)]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public HttpResponseMessage UpdateSpeakerAvatar(int codeCampId, int speakerId)
        {
            try
            {
                var result = NO_FILES;
                var httpRequest = HttpContext.Current.Request;

                if (httpRequest.Files.Count > 0)
                {
                    var folderPath = string.Format("~/Portals/{0}/CodeCamps/{1}/SpeakerAvatars/", PortalSettings.PortalId, codeCampId);

                    foreach (string file in httpRequest.Files)
                    {
                        var postedFile = httpRequest.Files[file];
                        var filePath = HttpContext.Current.Server.MapPath(string.Concat(folderPath, postedFile.FileName));

                        // TODO: append "-ORIGINAL" to the file name
                        // TODO: parse the image and resize as required
                        // TODO: allow avatars to be cropepd and saved
                        // TODO: update speaker profile with file id

                        postedFile.SaveAs(filePath);
                        // NOTE: To store in memory use postedFile.InputStream

                        var folderInfo = DotNetNuke.Services.FileSystem.FolderManager.Instance.GetFolder(PortalSettings.PortalId, folderPath);
                        var fileExists = DotNetNuke.Services.FileSystem.FileManager.Instance.FileExists(folderInfo, postedFile.FileName);

                        if (fileExists)
                        {
                            result = DotNetNuke.Services.FileSystem.FileManager.Instance.GetFile(folderInfo, postedFile.FileName).FileId.ToString();

                            if (!string.IsNullOrEmpty(result))
                            {
                                var speaker = SpeakerDataAccess.GetItem(speakerId, codeCampId);

                                if (speaker != null)
                                {
                                    speaker.IconFile = result;

                                    SpeakerDataAccess.UpdateItem(speaker);
                                }
                            }
                        }
                    }
                }

                var response = new ServiceResponse<string> { Content = result };

                return Request.CreateResponse(HttpStatusCode.OK, response.ObjectToJson());
            }
            catch (Exception ex)
            {
                Exceptions.LogException(ex);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.GetBaseException().Message);
            }
        }
    }
}