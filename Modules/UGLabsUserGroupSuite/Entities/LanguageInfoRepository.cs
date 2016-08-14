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
using System.Data;
using DotNetNuke.Data;

namespace DNNCommunity.Modules.UserGroupSuite.Entities
{
    public class LanguageInfoRepository
    {
        public void CreateItem(LanguageInfo i)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<LanguageInfo>();
                rep.Insert(i);
            }
        }

        public void DeleteItem(int itemId, int portalID)
        {
            var i = GetItem(itemId, portalID);
            DeleteItem(i);
        }

        public void DeleteItem(LanguageInfo i)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<LanguageInfo>();
                rep.Delete(i);
            }
        }

        public IEnumerable<LanguageInfo> GetItems(int portalID)
        {
            IEnumerable<LanguageInfo> i;
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<LanguageInfo>();
                i = rep.Get(portalID);
            }
            return i;
        }

        public LanguageInfo GetItem(int itemId, int portalID)
        {
            LanguageInfo i = null;
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<LanguageInfo>();
                i = rep.GetById(itemId, portalID);
            }
            return i;
        }

        public void UpdateItem(LanguageInfo i)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<LanguageInfo>();
                rep.Update(i);
            }
        }

        public void AddDefaultItems(int portalID)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                ctx.Execute(CommandType.StoredProcedure, "UG_AddDefaultLanguages", portalID);
            }
        }
    }
}