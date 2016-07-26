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
using System.Text;
using System.Net;
using System.IO;
using DNNCommunity.Modules.UserGroupSuite.Components;
using DNNCommunity.Modules.UserGroupSuite.Services;

namespace DNNCommunity.Modules.UserGroupSuite.Tests
{
    public class ServiceHelper
    {
        public static T GetRequest<T>(string uri)
            where T : class, new()
        {
            var result = SendGet<T>(uri);
            return result;
        }

        public static T PostRequest<T>(string uri, string data)
            where T : class, new()
        {
            var result = SendWithData<T>(uri, "POST", data);
            return result;
        }

        public static T PutRequest<T>(string uri, string data)
            where T : class, new()
        {
            var result = SendWithData<T>(uri, "PUT", data);
            return result;
        }

        public static T DeleteRequest<T>(string uri, string data)
            where T : class, new()
        {
            var result = SendWithData<T>(uri, "DELETE", data);
            return result;
        }

        private const int DefaultTimeout = 100000;

        private static T SendGet<T>(string uri)
            where T : class, new()
        {
            try
            {
                var response = SendRequest(uri, "GET", null);
                var result = JsonHelper.ObjectFromJson<T>(response);

                return result;
            }
            catch (Exception ex)
            {
                var result = new T();
                var apiResponse = result as IServiceResponse;

                if (apiResponse != null)
                {
                    apiResponse.Errors.Add(new ServiceError("EXCEPTION", ex.Message + " | " + ex.StackTrace));
                }

                return result;
            }
        }

        private static T SendWithData<T>(string uri, string method, string data)
            where T : class, new()
        {
            try
            {
                var response = SendRequest(uri, method, data);
                var result = JsonHelper.ObjectFromJson<T>(response);
                return result;
            }
            catch (Exception ex)
            {
                var result = new T();
                var apiResponse = result as IServiceResponse;

                if (apiResponse != null)
                {
                    apiResponse.Errors.Add(new ServiceError("EXCEPTION", ex.Message + " | " + ex.StackTrace));
                }

                return result;
            }
        }

        private static string SendRequest(string serviceUrl, string method, string data, WebProxy proxy = null, int timeout = DefaultTimeout)
        {
            WebResponse objResp;
            WebRequest objReq;
            var strResp = string.Empty;
            byte[] byteReq;

            try
            {
                byteReq = data == null ? null : Encoding.UTF8.GetBytes(data);
                objReq = WebRequest.Create(serviceUrl);
                objReq.Method = method.ToUpperInvariant();

                if (byteReq != null)
                {
                    objReq.ContentLength = byteReq.Length;
                    objReq.ContentType = "application/x-www-form-urlencoded";
                }

                objReq.Timeout = timeout;
                
                if (proxy != null)
                {
                    objReq.Proxy = proxy;
                }

                if (byteReq != null)
                {
                    var OutStream = objReq.GetRequestStream();

                    OutStream.Write(byteReq, 0, byteReq.Length);
                    OutStream.Close();
                }
                
                objResp = objReq.GetResponse();
                
                var sr = new StreamReader(objResp.GetResponseStream(), Encoding.UTF8, true);
                
                strResp += sr.ReadToEnd();
                sr.Close();
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Error SendRequest: " + ex.Message + " " + ex.Source);
            }

            return strResp;
        }
    }
}