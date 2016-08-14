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
using System.Web.Caching;
using DNNCommunity.Modules.UserGroupSuite.Entities.Interfaces;
using DotNetNuke.Common.Utilities;
using DotNetNuke.ComponentModel.DataAnnotations;

namespace DNNCommunity.Modules.UserGroupSuite.Entities
{
    [TableName("UG_Group")]
    [PrimaryKey("GroupID", AutoIncrement = true)]
    [Cacheable("UG_Group", CacheItemPriority.Default, 20)]
    [Scope("ModuleID")]
    public class GroupInfo : IGroupInfo
    {
        public GroupInfo()
        {
            GroupID = Null.NullInteger;
            ModuleID = Null.NullInteger;
            GroupName = string.Empty;
            Country = string.Empty;
            City = string.Empty;
            LanguageID = Null.NullInteger;
            Description = string.Empty;
            Website = string.Empty;
            Avatar = string.Empty;
            IsActive = false;
            Slug = string.Empty;
            CustomProperties = string.Empty;
            CustomPropertiesObj = new List<CustomPropertyInfo>();
            CreatedBy = Null.NullInteger;
            CreatedOn = Null.NullDate;
            LastUpdatedType = 0;
            LastUpdatedOn = Null.NullDate;
            LastUpdatedBy = Null.NullInteger;
            NextMeeting = Null.NullDate;
            Where = string.Empty;
            Streaming = string.Empty;
            Attending = string.Empty;
        }

        public int GroupID { get; set; }
        public int ModuleID { get; set; }
        public string GroupName { get; set; }
        public string Country { get; set; }
        public string CountryCode { get; set; }
        public string Region { get; set; }
        public string RegionCode { get; set; }
        public string City { get; set; }
        public int LanguageID { get; set; }
        public string Description { get; set; }
        public string Website { get; set; }
        public string Avatar { get; set; }
        public bool IsActive { get; set; }
        public string Slug { get; set; }
        public string CustomProperties { get; set; }
        [IgnoreColumn]
        public List<CustomPropertyInfo> CustomPropertiesObj { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public int LastUpdatedType { get; set; }
        public DateTime LastUpdatedOn { get; set; }
        public int LastUpdatedBy { get; set; }

        /* used for front end views only */
        [IgnoreColumn]
        public DateTime NextMeeting { get; set; }
        [IgnoreColumn]
        public string Where { get; set; }
        [IgnoreColumn]
        public string Streaming { get; set; }
        [IgnoreColumn]
        public string Attending { get; set; }
    }
}