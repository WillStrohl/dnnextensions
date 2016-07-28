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
    public class VirtualAddressInfoController
    {
        private readonly VirtualAddressInfoRepository _repo = null;

        public VirtualAddressInfoController() 
        {
            _repo = new VirtualAddressInfoRepository();
        }

        public void CreateItem(VirtualAddressInfo i)
        {
            ValidateVirtualAddressObject(i);

            _repo.CreateItem(i);
        }

        public void DeleteItem(int itemID, int moduleID)
        {
            Requires.NotNegative("itemID", itemID);
            Requires.NotNegative("moduleID", moduleID);

            _repo.DeleteItem(itemID, moduleID);
        }

        public void DeleteItem(VirtualAddressInfo i)
        {
            Requires.NotNull("i", i);
            Requires.PropertyNotNegative(i.AddressID, "AddressID");
            Requires.PropertyNotNegative(i.ModuleID, "ModuleID");

            _repo.DeleteItem(i);
        }

        public IEnumerable<VirtualAddressInfo> GetItems(int moduleID)
        {
            Requires.NotNegative("moduleID", moduleID);

            var items = _repo.GetItems(moduleID);
            return items;
        }

        public VirtualAddressInfo GetItem(int itemID, int moduleID)
        {
            Requires.NotNegative("itemID", itemID);
            Requires.NotNegative("moduleID", moduleID);

            var item = _repo.GetItem(itemID, moduleID);
            return item;
        }

        public void UpdateItem(VirtualAddressInfo i)
        {
            ValidateVirtualAddressObject(i, true);

            _repo.UpdateItem(i);
        }

        #region Helper Methods

        private void ValidateVirtualAddressObject(VirtualAddressInfo i, bool checkPrimaryKey = false)
        {
            Requires.NotNull("i", i);

            if (checkPrimaryKey)
            {
                Requires.PropertyNotNegative(i.AddressID, "AddressID");
            }

            Requires.PropertyNotNullOrEmpty(i.AddressType, "AddressType");
            Requires.PropertyNotNullOrEmpty(i.Description, "Description");
            Requires.PropertyNotNegative(i.ModuleID, "ModuleID");
            Requires.PropertyNotNegative(i.CreatedBy, "CreatedBy");
            Requires.NotNull("CreatedOn", i.CreatedOn);
            Requires.PropertyNotNegative(i.LastUpdatedBy, "LastUpdatedBy");
            Requires.NotNull("LastUpdatedOn", i.LastUpdatedOn);
        }

        #endregion
    }
}