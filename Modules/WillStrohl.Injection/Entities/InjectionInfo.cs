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
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using DotNetNuke.Entities.Modules;

namespace WillStrohl.Modules.Injection.Entities
{
    [Serializable]
    public sealed class InjectionInfo : IInjectionInfo, IHydratable
    {
        #region Private Members

        private string p_InjectName = string.Empty;
        private string p_InjectContent = string.Empty;
        private int p_InjectionId = Null.NullInteger;
        private bool p_InjectTop = Null.NullBoolean;
        private bool p_IsEnabled = Null.NullBoolean;
        private int p_ModuleId = Null.NullInteger;
        private int p_OrderShown = Null.NullInteger;
        private List<CustomPropertyInfo> p_CustomProperties = null;

        #endregion

        #region Public Properties

        public string InjectName
        {
            get { return p_InjectName; }
            set { p_InjectName = value; }
        }

        public string InjectContent
        {
            get { return p_InjectContent; }
            set { p_InjectContent = value; }
        }

        public int InjectionId
        {
            get { return p_InjectionId; }
            set { p_InjectionId = value; }
        }

        public bool InjectTop
        {
            get { return p_InjectTop; }
            set { p_InjectTop = value; }
        }

        public bool IsEnabled
        {
            get { return p_IsEnabled; }
            set { p_IsEnabled = value; }
        }

        public int ModuleId
        {
            get { return p_ModuleId; }
            set { p_ModuleId = value; }
        }

        public int OrderShown
        {
            get { return p_OrderShown; }
            set { p_OrderShown = value; }
        }

        public List<CustomPropertyInfo> CustomProperties
        {
            get { return p_CustomProperties; }
            set { p_CustomProperties = value; }
        }

        #endregion

        public InjectionInfo()
        {
            p_CustomProperties = new List<CustomPropertyInfo>();
        }

        #region IHydratable Implementation

        public int KeyID
        {
            get { return p_InjectionId; }
            set { p_InjectionId = value; }
        }

        public void Fill(IDataReader dr)
        {
            try
            {
                while (dr.Read())
                {
                    if (dr[InjectionInfoMembers.InjectionIdField] != null)
                    {
                        p_InjectionId = int.Parse(dr[InjectionInfoMembers.InjectionIdField].ToString(), NumberStyles.Integer);
                    }

                    if (dr[InjectionInfoMembers.InjectNameField] != null)
                    {
                        p_InjectName = dr[InjectionInfoMembers.InjectNameField].ToString();
                    }

                    if (dr[InjectionInfoMembers.InjectContentField] != null)
                    {
                        p_InjectContent = dr[InjectionInfoMembers.InjectContentField].ToString();
                    }

                    if (dr[InjectionInfoMembers.InjectTopField] != null)
                    {
                        p_InjectTop = bool.Parse(dr[InjectionInfoMembers.InjectTopField].ToString());
                    }

                    if (dr[InjectionInfoMembers.IsEnabledField] != null)
                    {
                        p_IsEnabled = bool.Parse(dr[InjectionInfoMembers.IsEnabledField].ToString());
                    }

                    if (dr[InjectionInfoMembers.ModuleIdField] != null)
                    {
                        p_ModuleId = int.Parse(dr[InjectionInfoMembers.ModuleIdField].ToString(), NumberStyles.Integer);
                    }

                    if (dr[InjectionInfoMembers.OrderShownField] != null)
                    {
                        p_OrderShown = int.Parse(dr[InjectionInfoMembers.OrderShownField].ToString(), NumberStyles.Integer);
                    }

                    try
                    {
                        if (dr[InjectionInfoMembers.CustomPropertiesField] != null)
                        {
                            var properties = dr[InjectionInfoMembers.CustomPropertiesField].ToString();
                            p_CustomProperties = properties.FromJson<List<CustomPropertyInfo>>();
                        }
                    }
                    catch
                    {
                    }
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

        #endregion
    }
}