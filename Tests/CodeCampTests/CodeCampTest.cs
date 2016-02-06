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

using System;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WillStrohl.Modules.CodeCamp.Entities;
using WillStrohl.Modules.CodeCamp.Services;

namespace WillStrohl.Modules.CodeCamp.Tests
{
    [TestClass]
    public class CodeCampTest : TestBase
    {
        [TestMethod]
        public void CodeCamp_CreateFindDeleteEvent_Test()
        {
            var service = new ServiceProxy(ApiBaseUrl);

            var newEvent = new CodeCampInfo()
            {
                Title = "New Test Code Camp",
                Description = "Test code camp description.",
                CreatedByDate = DateTime.Now,
                CreatedByUserId = 1,
                LastUpdatedByDate = DateTime.Now,
                LastUpdatedByUserId = 1,
                BeginDate = DateTime.Now.AddDays(30),
                EndDate = DateTime.Now.AddDays(32),
                ModuleId = ModuleId
            };

            var createResponse = service.CreateEvent(newEvent);

            CheckErrors(createResponse);

            var findResponse = service.GetEvents(ModuleId);

            CheckErrors(findResponse);

            var actualEvent = findResponse.Content.FirstOrDefault(e => e.Title == "New Test Code Camp");

            Assert.AreEqual(newEvent.BeginDate, actualEvent.BeginDate);
            Assert.AreEqual(newEvent.EndDate, actualEvent.EndDate);
            Assert.AreEqual(newEvent.ModuleId, actualEvent.ModuleId);

            var deleteResponse = service.DeleteEvent(newEvent.CodeCampId);

            CheckErrors(deleteResponse);
        }
    }
}