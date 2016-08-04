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

using System.Net.Http;
using System.Web;
using DNNCommunity.Modules.UserGroupSuite.Controllers;
using DotNetNuke.Web.Api;
using DNNCommunity.Modules.UserGroupSuite.Entities;

namespace DNNCommunity.Modules.UserGroupSuite.Services
{
    public class ServiceBase : DnnApiController
    {
        protected const string SUCCESS_MESSAGE = "SUCCESS";
        protected const string ERROR_MESSAGE = "An error occurred. Please check the event log or contact your site administrator for more information.";
        protected const string NO_FILES = "NO FILES";

        protected AddressInfoController AddressDataAccess { get; set; }
        protected AttendanceInfoController AttendanceDataAccess { get; set; }
        protected DnnUserController DnnUserDataAccess { get; set; }
        protected GroupInfoController GroupDataAccess { get; set; }
        protected KeywordInfoController KeywordDataAccess { get; set; }
        protected LanguageInfoController LanguageDataAccess { get; set; }
        protected LeaderInfoController LeaderDataAccess { get; set; }
        protected MaterialInfoController MaterialDataAccess { get; set; }
        protected MeetingInfoController MeetingDataAccess { get; set; }
        protected MemberActivityInfoController MemberActivityDataAccess { get; set; }
        protected MemberInfoController MemberDataAccess { get; set; }
        protected SocialSiteInfoController SocialSiteDataAccess { get; set; }
        protected SpeakerInfoController SpeakerDataAccess { get; set; }
        protected SpeakerMeetingInfoController SpeakerMeetingDataAccess { get; set; }
        protected VirtualAddressInfoController VirtualAddressDataAccess { get; set; }

        public ServiceBase()
        {
            AddressDataAccess = new AddressInfoController();
            AttendanceDataAccess = new AttendanceInfoController();
            DnnUserDataAccess = new DnnUserController();
            GroupDataAccess = new GroupInfoController();
            KeywordDataAccess = new KeywordInfoController();
            LanguageDataAccess = new LanguageInfoController();
            LeaderDataAccess = new LeaderInfoController();
            MaterialDataAccess = new MaterialInfoController();
            MeetingDataAccess = new MeetingInfoController();
            MemberActivityDataAccess = new MemberActivityInfoController();
            MemberDataAccess = new MemberInfoController();
            SocialSiteDataAccess = new SocialSiteInfoController();
            SpeakerDataAccess = new SpeakerInfoController();
            SpeakerMeetingDataAccess = new SpeakerMeetingInfoController();
            VirtualAddressDataAccess = new VirtualAddressInfoController();
        }
        
        protected string GetClientIpAddress()
        {
            var clientIpAddress = HttpContext.Current != null ? HttpContext.Current.Request.UserHostAddress : string.Empty;

            if (clientIpAddress == "127.0.0.1")
            {
                // example IP address to use for testing
                clientIpAddress = "199.83.131.63";
                // this is the IP address for the project's official hosting sponsor, AppliedI.Net
            }

            return clientIpAddress;
        }
    }
}