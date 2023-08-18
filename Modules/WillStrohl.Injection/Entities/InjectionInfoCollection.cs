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
