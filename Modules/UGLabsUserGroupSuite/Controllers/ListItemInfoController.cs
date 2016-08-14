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

using System.Collections.Generic;
using System.Linq;
using DotNetNuke.Common;
using DotNetNuke.Common.Lists;
using DotNetNuke.Common.Utilities;
using Globals = DNNCommunity.Modules.UserGroupSuite.Components.Globals;

namespace DNNCommunity.Modules.UserGroupSuite.Entities
{
    public class ListItemInfoController
    {
        public List<ListItemInfo> GetCountries(bool clearCache = false)
        {
            var countries = new List<ListItemInfo>();
            var cachedCountries = DataCache.GetCache(Globals.CACHE_KEY_COUNTRY);

            if (cachedCountries == null || clearCache)
            {
                var countryList = new ListController().GetListEntryInfoItems("Country");
                countries = ParseDnnList(ref countryList);

                DataCache.SetCache(Globals.CACHE_KEY_COUNTRY, countries);
            }
            else
            {
                countries = (List<ListItemInfo>)cachedCountries;
            }

            return countries;
        }
        
        public List<ListItemInfo> GetRegions(string parentKey, bool clearCache = false)
        {
            Requires.NotNullOrEmpty("parentKey", parentKey);

            var regions = new List<ListItemInfo>();
            var cachedRegions = DataCache.GetCache(string.Format(Globals.CACHE_KEY_REGION_FORMAT, parentKey));

            if (cachedRegions == null || clearCache)
            {
                var formattedKey = string.Concat("Country.", parentKey);
                var regionList = new ListController().GetListEntryInfoItems("Region", formattedKey);
                regions = ParseDnnList(ref regionList);

                DataCache.SetCache(string.Format(Globals.CACHE_KEY_REGION_FORMAT, parentKey), regions);
            }
            else
            {
                regions = (List<ListItemInfo>) cachedRegions;
            }

            return regions;
        }

        #region Helper Methods

        private List<ListItemInfo> ParseDnnList(ref IEnumerable<ListEntryInfo> list)
        {
            return list.Select(lii => new ListItemInfo(lii.Value, lii.Text)).ToList();
        }

        #endregion
    }
}