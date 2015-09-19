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

using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WillStrohl.Modules.CodeCamp.Services
{
    public partial class EventController
    {
        /// <summary>
        /// Use to test a successful response
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// GET: http://dnndev.me/DesktopModules/CodeCamp/API/Event/Ping
        /// </remarks>
        [AllowAnonymous]
        [HttpGet]
        public HttpResponseMessage Ping()
        {
            var response = new ServiceResponse<string>() { Content = "Success" };

            return Request.CreateResponse(HttpStatusCode.OK, response.ObjectToJson());
        }

        /// <summary>
        /// Use to test a failed response
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// GET: http://dnndev.me/DesktopModules/CodeCamp/API/CodeCamp/PingError
        /// </remarks>
        [AllowAnonymous]
        [HttpGet]
        public HttpResponseMessage PingError()
        {
            var errors = new List<ServiceError>();

            errors.Add(new ServiceError()
            {
                Code = "NULL",
                Description = "NullReferenceException stack trace"
            });

            var response = new ServiceResponse<string>()
            {
                Errors = errors
            };

            return Request.CreateResponse(HttpStatusCode.OK, response.ObjectToJson());
        }

        /// <summary>
        /// Use to test a failed response
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// GET: http://dnndev.me/DesktopModules/CodeCamp/API/CodeCamp/PingException
        /// </remarks>
        [AllowAnonymous]
        [HttpGet]
        public HttpResponseMessage PingException()
        {
            return Request.CreateResponse(HttpStatusCode.InternalServerError);
        }

        /// <summary>
        /// Use to test a failed response
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// GET: http://dnndev.me/DesktopModules/CodeCamp/API/CodeCamp/PingNotFound
        /// </remarks>
        [AllowAnonymous]
        [HttpGet]
        public HttpResponseMessage PingNotFound()
        {
            return Request.CreateResponse(HttpStatusCode.NotFound);
        }
    }
}