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
using DotNetNuke.Common;

namespace DNNCommunity.Modules.UserGroupSuite.Entities
{
    public class LanguageInfoController
    {
        private readonly LanguageInfoRepository _repo = null;

        public LanguageInfoController() 
        {
            _repo = new LanguageInfoRepository();
        }

        public void CreateItem(LanguageInfo i)
        {
            ValidateLanguageObject(i);

            _repo.CreateItem(i);
        }

        public void DeleteItem(int itemID, int portalID)
        {
            Requires.NotNegative("itemID", itemID);
            Requires.NotNegative("portalID", portalID);

            _repo.DeleteItem(itemID, portalID);
        }

        public void DeleteItem(LanguageInfo i)
        {
            Requires.NotNull("i", i);
            Requires.PropertyNotNegative(i.GroupLanguageID, "GroupLanguageID");
            Requires.PropertyNotNegative(i.PortalID, "PortalID");

            _repo.DeleteItem(i);
        }

        public IEnumerable<LanguageInfo> GetItems(int portalID)
        {
            Requires.NotNegative("portalID", portalID);

            var items = _repo.GetItems(portalID);
            return items;
        }

        public LanguageInfo GetItem(int itemID, int portalID)
        {
            Requires.NotNegative("itemID", itemID);
            Requires.NotNegative("portalID", portalID);

            var item = _repo.GetItem(itemID, portalID);
            return item;
        }

        public void UpdateItem(LanguageInfo i)
        {
            ValidateLanguageObject(i, true);

            _repo.UpdateItem(i);
        }

        #region Helper Methods

        private void ValidateLanguageObject(LanguageInfo i, bool checkPrimaryKey = false)
        {
            Requires.NotNull("i", i);

            if (checkPrimaryKey)
            {
                Requires.PropertyNotNegative(i.GroupLanguageID, "GroupLanguageID");
            }

            Requires.PropertyNotNullOrEmpty(i.Language, "Language");
            Requires.PropertyNotNegative(i.PortalID, "PortalID");
        }

        #endregion
    }
}