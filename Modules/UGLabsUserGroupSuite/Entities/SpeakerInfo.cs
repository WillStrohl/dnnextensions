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
using System.Web.Caching;
using DNNCommunity.Modules.UserGroupSuite.Entities.Interfaces;
using DotNetNuke.Common.Utilities;
using DotNetNuke.ComponentModel.DataAnnotations;

namespace DNNCommunity.Modules.UserGroupSuite.Entities
{
    [TableName("UG_Speaker")]
    [PrimaryKey("SpeakerID", AutoIncrement = true)]
    [Cacheable("UG_Speaker", CacheItemPriority.Default, 20)]
    [Scope("UserID")]
    public class SpeakerInfo : ISpeakerInfo
    {
        public SpeakerInfo()
        {
            SpeakerID = Null.NullInteger;
            UserID = Null.NullInteger;
            SpeakerName = string.Empty;
            Website = string.Empty;
            Bio = string.Empty;
            Email = string.Empty;
            Avatar = string.Empty;
            TravelPreference = Null.NullInteger;
            CreatedOn = DateTime.MinValue;
            CreatedBy = Null.NullInteger;
            LastUpdatedOn = DateTime.MinValue;
            LastUpdatedBy = Null.NullInteger;
        }

        public int SpeakerID { get; set; }
        public int UserID { get; set; }
        public string SpeakerName { get; set; }
        public string Website { get; set; }
        public string Bio { get; set; }
        public string Email { get; set; }
        public string Avatar { get; set; }
        public int TravelPreference { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public DateTime LastUpdatedOn { get; set; }
        public int LastUpdatedBy { get; set; }
    }
}