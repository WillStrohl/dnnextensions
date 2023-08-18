/*
Copyright © Upendo Ventures, LLC

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and 
associated documentation files (the "Software"), to deal in the Software without restriction, 
including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, 
and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, 
subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial 
portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT 
NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND 
NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES 
OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN 
CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 */

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