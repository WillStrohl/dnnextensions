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

namespace DNNCommunity.Modules.UserGroupSuite.Services
{
    public static class ServiceResponseHelper<T>
    {
        public static void AddNoneFoundError(string objectName, ref ServiceResponse<T> response)
        {
            response.Errors.Add(new ServiceError()
            {
                Code = "NONE-FOUND",
                Description = string.Format("Unable to find any {0} to return.", objectName)
            });
        }

        public static void AddUserCreateError(string errorName, ref ServiceResponse<T> response)
        {
            response.Errors.Add(new ServiceError()
            {
                Code = "USER-CREATE-ERROR",
                Description = string.Format("{0}", errorName)
            });
        }

        public static void AddUnknownError(ref ServiceResponse<T> response)
        {
            response.Errors.Add(new ServiceError()
            {
                Code = "UNKNOWN-ERROR",
                Description = "An unknown error occurred. Check the event viewer or contact your site administrator"
            });
        }

        public static void AddErrorMessage(string message, ref ServiceResponse<T> response)
        {
            response.Errors.Add(new ServiceError()
            {
                Code = "MESSAGE",
                Description = message
            });
        }
    }
}