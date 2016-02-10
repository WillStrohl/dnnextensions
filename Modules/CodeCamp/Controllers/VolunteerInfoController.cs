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

 int*/

using System.Collections.Generic;
using System.Linq;
using WillStrohl.Modules.CodeCamp.Components;

namespace WillStrohl.Modules.CodeCamp.Entities
{
    public class VolunteerInfoController
    {
        private readonly VolunteerInfoRepository repo = null;
        private readonly RegistrationInfoRepository registrationRepo = null;

        public VolunteerInfoController() 
        {
            repo = new VolunteerInfoRepository();
            registrationRepo = new RegistrationInfoRepository();
        }

        public void CreateItem(VolunteerInfo i)
        {
            repo.CreateItem(i);
        }

        public void DeleteItem(int itemId, int codeCampId)
        {
            repo.DeleteItem(itemId, codeCampId);
        }

        public void DeleteItem(VolunteerInfo i)
        {
            repo.DeleteItem(i);
        }

        public IEnumerable<VolunteerInfo> GetItems(int codeCampId)
        {
            var items = repo.GetItems(codeCampId);

            return items;
        }

        public VolunteerInfo GetItemByRegistrationId(int registrationId, int codeCampId)
        {
            var item = repo.GetItems(codeCampId).FirstOrDefault(v => v.RegistrationId == registrationId);

            return item;
        }

        public VolunteerInfo GetItem(int itemId, int codeCampId)
        {
            var item = repo.GetItem(itemId, codeCampId);

            return item;
        }

        public string GetItemFullName(int itemId, int codeCampId, int portalId)
        {
            var item = repo.GetItem(itemId, codeCampId);

            var registration = registrationRepo.GetItem(item.RegistrationId, codeCampId);

            var userInfo = DotNetNuke.Entities.Users.UserController.GetUserById(portalId, registration.UserId);

            var fullName = userInfo.DisplayName;

            // ISSUE 96: DNN 8 isn't assigning the newly created the user the same as in DNN 7
            if (!string.IsNullOrEmpty(userInfo.FirstName) && !string.IsNullOrEmpty(userInfo.LastName))
            {
                fullName = string.Concat(userInfo.FirstName, Globals.SPACE, userInfo.LastName);
            }
            else if (!string.IsNullOrEmpty(userInfo.Profile.FirstName) && !string.IsNullOrEmpty(userInfo.Profile.LastName))
            {
                fullName = string.Concat(userInfo.Profile.FirstName, Globals.SPACE, userInfo.Profile.LastName);
            }

            return fullName;
        }

        public void UpdateItem(VolunteerInfo i)
        {
            repo.UpdateItem(i);
        }
    }
}