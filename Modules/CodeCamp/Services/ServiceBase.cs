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

using DotNetNuke.Web.Api;
using WillStrohl.Modules.CodeCamp.Entities;

namespace WillStrohl.Modules.CodeCamp.Services
{
    public class ServiceBase : DnnApiController
    {
        protected const string SUCCESS_MESSAGE = "SUCCESS";
        protected const string ERROR_MESSAGE = "An error occurred. Please check the event log or contact your site administrator for more information";
        protected const string NO_FILES = "NO FILES";

        protected CodeCampInfoController CodeCampDataAccess { get; set; }
        protected RegistrationInfoController RegistrationDataAccess { get; set; }
        protected RoomInfoController RoomDataAccess { get; set; }
        protected SessionInfoController SessionDataAccess { get; set; }
        protected SessionRegistrationInfoController SessionRegistrationDataAccess { get; set; }
        protected SessionSpeakerInfoController SessionSpeakerDataAccess { get; set; }
        protected SpeakerInfoController SpeakerDataAccess { get; set; }
        protected TrackInfoController TrackDataAccess { get; set; }
        protected TimeSlotInfoController TimeSlotDataAccess { get; set; }
        protected VolunteerInfoController VolunteerDataAccess { get; set; }
        protected VolunteerTaskInfoController VolunteerTaskDataAccess { get; set; }

        public ServiceBase()
        {
            CodeCampDataAccess = new CodeCampInfoController();
            RegistrationDataAccess = new RegistrationInfoController();
            RoomDataAccess = new RoomInfoController();
            SessionDataAccess = new SessionInfoController();
            SessionRegistrationDataAccess = new SessionRegistrationInfoController();
            SessionSpeakerDataAccess = new SessionSpeakerInfoController();
            SpeakerDataAccess = new SpeakerInfoController();
            TrackDataAccess = new TrackInfoController();
            TimeSlotDataAccess = new TimeSlotInfoController();
            VolunteerDataAccess = new VolunteerInfoController();
            VolunteerTaskDataAccess = new VolunteerTaskInfoController();
        }
    }
}