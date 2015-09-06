//
// Will Strohl (will.strohl@gmail.com)
// http://www.willstrohl.com
//
//Copyright (c) 2009-2015, Will Strohl
//All rights reserved.
//
//Redistribution and use in source and binary forms, with or without modification, are 
//permitted provided that the following conditions are met:
//
//Redistributions of source code must retain the above copyright notice, this list of 
//conditions and the following disclaimer.
//
//Redistributions in binary form must reproduce the above copyright notice, this list 
//of conditions and the following disclaimer in the documentation and/or other 
//materials provided with the distribution.
//
//Neither the name of Will Strohl, Content Injection, nor the names of its contributors may be 
//used to endorse or promote products derived from this software without specific prior 
//written permission.
//
//THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY 
//EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES 
//OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT 
//SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, 
//INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED 
//TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR 
//BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN 
//CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN 
//ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH 
//DAMAGE.
//

using DotNetNuke.Common.Utilities;
using DotNetNuke.Services.Exceptions;
using System;
using System.Data;
using System.Collections.Generic;

namespace WillStrohl.Modules.Injection.Entities
{
    [Serializable]
    public sealed class InjectionInfoCollection : List<InjectionInfo>
    {
        #region Constructors

        public InjectionInfoCollection()
            : base()
        {
        }

        public InjectionInfoCollection(int capacity)
            : base(capacity)
        {
        }

        public InjectionInfoCollection(IEnumerable<InjectionInfo> collection)
            : base(collection)
        {
        }

        #endregion

        public void Fill(IDataReader dr)
        {
            try
            {
                while (dr.Read())
                {
                    var obj = new InjectionInfo();

                    if (dr[InjectionInfoMembers.InjectionIdField] != null)
                    {
                        obj.InjectionId = int.Parse(dr[InjectionInfoMembers.InjectionIdField].ToString(), System.Globalization.NumberStyles.Integer);
                    }

                    if (dr[InjectionInfoMembers.InjectNameField] != null)
                    {
                        obj.InjectName = dr[InjectionInfoMembers.InjectNameField].ToString();
                    }

                    if (dr[InjectionInfoMembers.InjectContentField] != null)
                    {
                        obj.InjectContent = dr[InjectionInfoMembers.InjectContentField].ToString();
                    }

                    if (dr[InjectionInfoMembers.InjectTopField] != null)
                    {
                        obj.InjectTop = bool.Parse(dr[InjectionInfoMembers.InjectTopField].ToString());
                    }

                    if (dr[InjectionInfoMembers.IsEnabledField] != null)
                    {
                        obj.IsEnabled = bool.Parse(dr[InjectionInfoMembers.IsEnabledField].ToString());
                    }

                    if (dr[InjectionInfoMembers.ModuleIdField] != null)
                    {
                        obj.ModuleId = int.Parse(dr[InjectionInfoMembers.ModuleIdField].ToString(), System.Globalization.NumberStyles.Integer);
                    }

                    if (dr[InjectionInfoMembers.OrderShownField] != null)
                    {
                        obj.OrderShown = int.Parse(dr[InjectionInfoMembers.OrderShownField].ToString(), System.Globalization.NumberStyles.Integer);
                    }

                    try
                    {
                        if (dr[InjectionInfoMembers.CustomPropertiesField] != null)
                        {
                            var properties = dr[InjectionInfoMembers.CustomPropertiesField].ToString();
                            obj.CustomProperties = properties.FromJson<List<CustomPropertyInfo>>();
                        }
                    }
                    catch
                    {
                    }

                    Add(obj);
                }
            }
            catch (Exception ex)
            {
                Exceptions.LogException(ex);
            }
            finally
            {
                if (!dr.IsClosed)
                {
                    dr.Close();
                }
            }
        }
    }

}
