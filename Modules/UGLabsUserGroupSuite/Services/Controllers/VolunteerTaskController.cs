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
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Web.Api;
using WillStrohl.Modules.CodeCamp.Components;
using WillStrohl.Modules.CodeCamp.Entities;

namespace WillStrohl.Modules.CodeCamp.Services
{
    public partial class EventController
    {
        /// <summary>
        /// Get all volunteer tasks
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// GET: http://dnndev.me/DesktopModules/CodeCamp/API/Event/GetVolunteerTasks
        /// </remarks>
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.View)]
        [HttpGet]
        public HttpResponseMessage GetVolunteerTasks(int volunteerId)
        {
            try
            {
                var tasks = VolunteerTaskDataAccess.GetItems(volunteerId);
                var response = new ServiceResponse<List<VolunteerTaskInfo>> { Content = tasks.ToList() };

                if (tasks == null)
                {
                    ServiceResponseHelper<List<VolunteerTaskInfo>>.AddNoneFoundError("tasks", ref response);
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
        /// Get a task
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// GET: http://dnndev.me/DesktopModules/CodeCamp/API/Event/GetVolunteerTask
        /// </remarks>
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.View)]
        [HttpGet]
        public HttpResponseMessage GetVolunteerTask(int itemId, int volunteerId)
        {
            try
            {
                var task = VolunteerTaskDataAccess.GetItem(itemId, volunteerId);
                var response = new ServiceResponse<VolunteerTaskInfo> { Content = task };

                if (task == null)
                {
                    ServiceResponseHelper<VolunteerTaskInfo>.AddNoneFoundError("task", ref response);
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
        /// Mark a task as being completed
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// GET: http://dnndev.me/DesktopModules/CodeCamp/API/Event/MarkVolunteerTaskComplete
        /// </remarks>
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.View)]
        [HttpGet]
        public HttpResponseMessage MarkVolunteerTaskComplete(int itemId, int volunteerId)
        {
            try
            {
                var task = VolunteerTaskDataAccess.GetItem(itemId, volunteerId);

                task.Completed = true;
                task.LastUpdatedByDate = DateTime.Now;
                task.LastUpdatedByUserId = UserInfo.UserID;

                VolunteerTaskDataAccess.UpdateItem(task);

                var response = new ServiceResponse<VolunteerTaskInfo> { Content = task };

                return Request.CreateResponse(HttpStatusCode.OK, response.ObjectToJson());
            }
            catch (Exception ex)
            {
                Exceptions.LogException(ex);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ERROR_MESSAGE);
            }
        }

        /// <summary>
        /// Mark a task as being completed
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// GET: http://dnndev.me/DesktopModules/CodeCamp/API/Event/MarkVolunteerTaskIncomplete
        /// </remarks>
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.View)]
        [HttpGet]
        public HttpResponseMessage MarkVolunteerTaskIncomplete(int itemId, int volunteerId)
        {
            try
            {
                var task = VolunteerTaskDataAccess.GetItem(itemId, volunteerId);

                task.Completed = false;
                task.LastUpdatedByDate = DateTime.Now;
                task.LastUpdatedByUserId = UserInfo.UserID;

                VolunteerTaskDataAccess.UpdateItem(task);

                var response = new ServiceResponse<VolunteerTaskInfo> { Content = task };

                return Request.CreateResponse(HttpStatusCode.OK, response.ObjectToJson());
            }
            catch (Exception ex)
            {
                Exceptions.LogException(ex);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ERROR_MESSAGE);
            }
        }

        /// <summary>
        /// Delete a task
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// DELETE: http://dnndev.me/DesktopModules/CodeCamp/API/Event/DeleteVolunteerTask
        /// </remarks>
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.Edit)]
        [ValidateAntiForgeryToken]
        [HttpDelete]
        public HttpResponseMessage DeleteVolunteerTask(int itemId, int volunteerId)
        {
            try
            {
                VolunteerTaskDataAccess.DeleteItem(itemId, volunteerId);

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
        /// Create a task
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// POST: http://dnndev.me/DesktopModules/CodeCamp/API/Event/CreateVolunteerTask
        /// </remarks>
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.View)]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public HttpResponseMessage CreateVolunteerTask(VolunteerTaskInfo task)
        {
            try
            {
                var timeStamp = DateTime.Now;

                task.CreatedByDate = timeStamp;
                task.CreatedByUserId = UserInfo.UserID;
                task.LastUpdatedByDate = timeStamp;
                task.LastUpdatedByUserId = UserInfo.UserID;

                VolunteerTaskDataAccess.CreateItem(task);

                var savedTask =
                    VolunteerTaskDataAccess.GetItems(task.VolunteerId)
                        .OrderByDescending(t => t.CreatedByDate)
                        .FirstOrDefault();

                var response = new ServiceResponse<VolunteerTaskInfo> { Content = savedTask };

                return Request.CreateResponse(HttpStatusCode.OK, response.ObjectToJson());
            }
            catch (Exception ex)
            {
                Exceptions.LogException(ex);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ERROR_MESSAGE);
            }
        }

        /// <summary>
        /// Update a task
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// POST: http://dnndev.me/DesktopModules/CodeCamp/API/Event/UpdateVolunteerTask
        /// </remarks>
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.View)]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public HttpResponseMessage UpdateVolunteerTask(VolunteerTaskInfo task)
        {
            try
            {
                var updatesToProcess = false;
                var originalTask = VolunteerTaskDataAccess.GetItem(task.VolunteerTaskId, task.VolunteerId);

                if (!string.Equals(originalTask.Title, task.Title))
                {
                    originalTask.Title = task.Title;
                    updatesToProcess = true;
                }

                if (originalTask.BeginDate != task.BeginDate)
                {
                    originalTask.BeginDate = task.BeginDate;
                    updatesToProcess = true;
                }

                if (originalTask.EndDate != task.EndDate)
                {
                    originalTask.EndDate = task.EndDate;
                    updatesToProcess = true;
                }

                if (originalTask.Completed != task.Completed)
                {
                    originalTask.Completed = task.Completed;
                    updatesToProcess = true;
                }

                if (originalTask.CustomProperties != null)
                {
                    // parse custom properties for updates
                    foreach (var property in originalTask.CustomPropertiesObj)
                    {
                        if (task.CustomPropertiesObj.Any(p => p.Name == property.Name))
                        {
                            // see if the existing property needs to be updated
                            var prop = task.CustomPropertiesObj.FirstOrDefault(p => p.Name == property.Name);
                            if (!string.Equals(prop.Value, property.Value))
                            {
                                property.Value = prop.Value;
                                updatesToProcess = true;
                            }
                        }
                        else
                        {
                            // delete the property
                            originalTask.CustomPropertiesObj.Remove(property);
                            updatesToProcess = true;
                        }
                    }
                }

                if (task.CustomPropertiesObj != null)
                {
                    // add any new properties
                    if (originalTask.CustomProperties == null)
                    {
                        foreach (var property in task.CustomPropertiesObj)
                        {
                            originalTask.CustomPropertiesObj.Add(property);
                            updatesToProcess = true;
                        }
                    }
                    else
                    {
                        foreach (var property in task.CustomPropertiesObj.Where(property => !originalTask.CustomPropertiesObj.Contains(property)))
                        {
                            originalTask.CustomPropertiesObj.Add(property);
                            updatesToProcess = true;
                        }
                    }
                }

                if (updatesToProcess)
                {
                    originalTask.LastUpdatedByDate = DateTime.Now;
                    originalTask.LastUpdatedByUserId = UserInfo.UserID;

                    VolunteerTaskDataAccess.UpdateItem(originalTask);
                }

                var savedTask = VolunteerTaskDataAccess.GetItem(task.VolunteerTaskId, task.VolunteerId);

                var response = new ServiceResponse<VolunteerTaskInfo> { Content = savedTask };

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