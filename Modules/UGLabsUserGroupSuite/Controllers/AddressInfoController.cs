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
    public class AddressInfoController
    {
        private readonly AddressInfoRepository _repo = null;

        public AddressInfoController() 
        {
            _repo = new AddressInfoRepository();
        }

        public void CreateItem(AddressInfo i)
        {
            ValidateAddressObject(i);

            _repo.CreateItem(i);
        }

        public void DeleteItem(int itemID, int moduleID)
        {
            Requires.NotNegative("itemID", itemID);
            Requires.NotNegative("moduleId", moduleID);

            _repo.DeleteItem(itemID, moduleID);
        }

        public void DeleteItem(AddressInfo i)
        {
            Requires.NotNull("i", i);
            Requires.PropertyNotNegative(i.AddressID, "AddressID");
            Requires.PropertyNotNegative(i.ModuleID, "ModuleID");

            _repo.DeleteItem(i);
        }

        public IEnumerable<AddressInfo> GetItems(int moduleID)
        {
            Requires.NotNegative("moduleID", moduleID);

            var items = _repo.GetItems(moduleID);
            return items;
        }

        public AddressInfo GetItem(int itemID, int moduleID)
        {
            Requires.NotNull("itemID", itemID);
            Requires.NotNull("moduleID", moduleID);

            var item = _repo.GetItem(itemID, moduleID);
            return item;
        }

        public void UpdateItem(AddressInfo i)
        {
            ValidateAddressObject(i, true);

            _repo.UpdateItem(i);
        }

        #region Helper Methods

        private void ValidateAddressObject(AddressInfo i, bool checkPrimaryKey = false)
        {
            Requires.NotNull("address", i);

            if (checkPrimaryKey)
            {
                Requires.PropertyNotNegative(i.AddressID, "AddressID");
            }

            Requires.PropertyNotNegative(i.ModuleID, "ModuleID");
            Requires.PropertyNotNullOrEmpty(i.City, "City");
            Requires.PropertyNotNullOrEmpty(i.Country, "Country");
            Requires.PropertyNotNullOrEmpty(i.CountryCode, "CountryCode");
            Requires.PropertyNotNegative(i.CreatedBy, "CreatedBy");
            Requires.NotNull("address.CreatedOn", i.CreatedOn);
            Requires.PropertyNotNegative(i.LastUpdatedBy, "LastUpdatedBy");
            Requires.NotNull("address.LastUpdatedOn", i.LastUpdatedOn);
            Requires.PropertyNotNullOrEmpty(i.Line1, "Line1");
            Requires.PropertyNotNullOrEmpty(i.Nickname, "Nickname");
        }

        #endregion
    }
}