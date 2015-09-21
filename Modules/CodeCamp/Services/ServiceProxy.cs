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
using WillStrohl.Modules.CodeCamp.Components;
using WillStrohl.Modules.CodeCamp.Entities;
using WillStrohl.Modules.CodeCamp.Tests;

namespace WillStrohl.Modules.CodeCamp.Services
{
    public class ServiceProxy : ServiceProxyBase
    {
        public ServiceProxy(string baseWebSiteUri)
        {
            baseUri = baseWebSiteUri;
            
            if (!baseUri.EndsWith("/"))
            {
                baseUri += "/";
            }

            fullApiUri = System.IO.Path.Combine(baseUri, "DesktopModules/CodeCamp/API/Event/");
        }

        public ServiceResponse<string> CreateEvent(CodeCampInfo codeCamp)
        {
            var result = new ServiceResponse<string>();

            result = ServiceHelper.PostRequest<ServiceResponse<string>>(fullApiUri + "CeateEvent", codeCamp.ObjectToJson());
            
            return result;   
        }

        public ServiceResponse<List<CodeCampInfo>> GetEvents(int moduleId)
        {
            var result = new ServiceResponse<List<CodeCampInfo>>();

            result = ServiceHelper.GetRequest<ServiceResponse<List<CodeCampInfo>>>(fullApiUri + "GetEvents?moduleId=" + moduleId);

            return result;
        }

        public ServiceResponse<CodeCampInfo> GetEvent(int itemId)
        {
            var result = new ServiceResponse<CodeCampInfo>();

            result = ServiceHelper.GetRequest<ServiceResponse<CodeCampInfo>>(fullApiUri + "GetEvent?itemId=" + itemId);

            return result;
        }
        
        public ServiceResponse<string> UpdateEvent(CodeCampInfo codeCamp)
        {
            var result = new ServiceResponse<string>();

            result = ServiceHelper.PostRequest<ServiceResponse<string>>(fullApiUri + "UpdateEvent", codeCamp.ObjectToJson());

            return result;
        }

        public ServiceResponse<string> DeleteEvent(int itemId)
        {
            var result = new ServiceResponse<string>();

            result = ServiceHelper.DeleteRequest<ServiceResponse<string>>(fullApiUri + "DeleteEvent?itemId=" + itemId, string.Empty);

            return result;
        }
    }
}