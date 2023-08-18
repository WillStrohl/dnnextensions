/*
Copyright � Upendo Ventures, LLC

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
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using DotNetNuke.Common;
using DotNetNuke.Entities.Modules;
using WillStrohl.Modules.Injection.Entities;
using JsonExtensionsWeb = DotNetNuke.Common.Utilities.JsonExtensionsWeb;

namespace WillStrohl.Modules.Injection.Components
{
    public sealed class InjectionController : IPortable
    {
        private const string CSS_PATTERN = @".*\.(cs{2}|les{2}|sas{2})$";
        private const string JS_PATTERN = @".*\.js$";

        private const string DnnPageHeaderProvider = "DnnPageHeaderProvider";
        private const string DnnBodyProvider = "DnnBodyProvider";
        private const string DnnFormBottomProvider = "DnnFormBottomProvider";

        #region Data Access

        public int AddInjectionContent(InjectionInfo objInjection)
        {
            return Convert.ToInt32(DataProvider.Instance().AddInjectionContent(objInjection.ModuleId, objInjection.InjectTop, objInjection.InjectName, objInjection.InjectContent, objInjection.IsEnabled, objInjection.OrderShown, objInjection.CustomProperties.ToJson()));
        }

        public void UpdateInjectionContent(InjectionInfo objInjection)
        {
            DataProvider.Instance().UpdateInjectionContent(objInjection.InjectionId, objInjection.ModuleId, objInjection.InjectTop, objInjection.InjectName, objInjection.InjectContent, objInjection.IsEnabled, objInjection.OrderShown, objInjection.CustomProperties.ToJson());
        }

        public void DisableInjectionContent(int InjectionContentId)
        {
            DataProvider.Instance().DisableInjectionContent(InjectionContentId);
        }

        public void EnableInjectionContent(int InjectionContentId)
        {
            DataProvider.Instance().EnableInjectionContent(InjectionContentId);
        }

        public void DeleteInjectionContent(int InjectionContentId)
        {
            DataProvider.Instance().DeleteInjectionContent(InjectionContentId);
        }

        public InjectionInfo GetInjectionContent(int InjectionContentId)
        {
            InjectionInfo objInj = new InjectionInfo();
            objInj.Fill(DataProvider.Instance().GetInjectionContent(InjectionContentId));
            return objInj;
        }

        public InjectionInfoCollection GetActiveInjectionContents(int moduleId)
        {
            var collInj = new InjectionInfoCollection();
            collInj.Fill(DataProvider.Instance().GetActiveInjectionContents(moduleId));
            return collInj;
        }

        public InjectionInfoCollection GetInjectionContents(int ModuleId)
        {
            InjectionInfoCollection collInj = new InjectionInfoCollection();
            collInj.Fill(DataProvider.Instance().GetInjectionContents(ModuleId));
            return collInj;
        }

        public int GetNextOrderNumber(int ModuleId)
        {
            return DataProvider.Instance().GetNextOrderNumber(ModuleId);
        }

        public void ChangeOrder(int InjectionId, string Direction)
        {
            if (!Regex.IsMatch(Direction, "^(moveup|movedown)$", RegexOptions.IgnoreCase))
            {
                throw new ArgumentOutOfRangeException("Direction");
            }

            DataProvider.Instance().ChangeOrder(InjectionId, Direction);
        }

        public bool DoesInjectionNameExist(string InjectionName, int ModuleId)
        {
            return DataProvider.Instance().DoesInjectionNameExist(InjectionName, ModuleId);
        }

        #endregion

        #region Injection Type

        private static bool IsHtmlInjectionType(InjectionInfo injection)
        {
            if (injection.CustomProperties.Any(p => p.Name == InjectionInfoMembers.InjectionTypeField))
            {
                return
                    injection.CustomProperties.FirstOrDefault(p => p.Name == InjectionInfoMembers.InjectionTypeField)
                        .Value == "1";
            }

            return true;
        }

        public static InjectionType GetInjectionType(InjectionInfo injection)
        {
            Requires.NotNull("injection", injection);

            var isHtmlType = IsHtmlInjectionType(injection);

            if (isHtmlType && injection.InjectTop)
            {
                return InjectionType.HtmlTop;
            }
            else if (isHtmlType && !injection.InjectTop)
            {
                return InjectionType.HtmlBottom;
            }

            if (injection.InjectContent.EndsWith(".js"))
            {
                return InjectionType.JavaScript;
            }

            if (Regex.IsMatch(injection.InjectContent, CSS_PATTERN, RegexOptions.IgnoreCase))
            {
                return InjectionType.CSS;
            }

            throw new ArgumentOutOfRangeException("InjectionType", "InjectionType parameters do not match a known injection type.");
        }

        public static bool IsValidCssInjectionType(string content)
        {
            Requires.NotNullOrEmpty("content", content);

            return Regex.IsMatch(content, CSS_PATTERN, RegexOptions.IgnoreCase);
        }

        public static bool IsValidJavaScriptInjectionType(string content)
        {
            Requires.NotNullOrEmpty("content", content);

            return Regex.IsMatch(content, JS_PATTERN, RegexOptions.IgnoreCase);
        }

        #endregion

        #region IPortable Implementation

        public string ExportModule(int ModuleID)
        {
            StringBuilder sb = new StringBuilder(150);
            InjectionInfoCollection collInj = new InjectionInfoCollection();
            collInj = GetInjectionContents(ModuleID);

            sb.Append("<WillStrohl><injectionContents>");
            foreach (InjectionInfo obj in collInj)
            {
                sb.Append("<injectionContent>");
                sb.AppendFormat("<injectionId>{0}</injectionId>", XmlUtils.XMLEncode(obj.InjectionId.ToString()));
                sb.AppendFormat("<moduleId>{0}</moduleId>", XmlUtils.XMLEncode(obj.ModuleId.ToString()));
                sb.AppendFormat("<injectTop>{0}</injectTop>", XmlUtils.XMLEncode(obj.InjectTop.ToString()));
                sb.AppendFormat("<injectName>{0}</injectName>", XmlUtils.XMLEncode(obj.InjectName));
                sb.AppendFormat("<injectContent>{0}</injectContent>", XmlUtils.XMLEncode(obj.InjectContent));
                sb.AppendFormat("<isEnabled>{0}</isEnabled>", XmlUtils.XMLEncode(obj.IsEnabled.ToString()));
                sb.AppendFormat("<orderShown>{0}</orderShown>", XmlUtils.XMLEncode(obj.OrderShown.ToString()));
                sb.AppendFormat("<customProperties>{0}</customProperties>", XmlUtils.XMLEncode(obj.CustomProperties.ToJson()));
                sb.Append("</injectionContent>");
            }
            sb.Append("</injectionContents>");
            // later on, will probably need to add module settings here
            sb.Append("</WillStrohl>");

            return sb.ToString();
        }


        public void ImportModule(int ModuleID, string Content, string Version, int UserID)
        {

            try
            {
                var injContents = DotNetNuke.Common.Globals.GetContent(Content, "//injectionContents");

                foreach (XmlNode injContent in injContents.SelectNodes("//injectionContent"))
                {
                    var objInj = new InjectionInfo();

                    objInj.ModuleId = ModuleID;

                    if ((injContent.SelectSingleNode("injectTop") != null))
                    {
                        objInj.InjectTop = bool.Parse(injContent.SelectSingleNode("injectTop").InnerText);
                    }
                    
                    if ((injContent.SelectSingleNode("injectName") != null))
                    {
                        objInj.InjectName = injContent.SelectSingleNode("injectName").InnerText;
                    }
                    
                    if ((injContent.SelectSingleNode("injectContent") != null))
                    {
                        objInj.InjectContent = injContent.SelectSingleNode("injectContent").InnerText;
                    }
                    
                    if ((injContent.SelectSingleNode("isEnabled") != null))
                    {
                        objInj.IsEnabled = bool.Parse(injContent.SelectSingleNode("isEnabled").InnerText);
                    }
                    
                    if ((injContent.SelectSingleNode("orderShown") != null))
                    {
                        objInj.OrderShown = int.Parse(injContent.SelectSingleNode("orderShown").InnerText, System.Globalization.NumberStyles.Integer);
                    }
                    
                    if ((injContent.SelectSingleNode("customProperties") != null))
                    {
                        objInj.CustomProperties = (List<CustomPropertyInfo>)injContent.SelectSingleNode("customProperties").InnerText.FromJson(typeof(CustomPropertyInfo));
                    }

                    AddInjectionContent(objInj);
                }
            }
            catch (Exception ex)
            {
                Exceptions.LogException(ex);
            }
        }

        #endregion

        #region Custom Properties

        public static int GetCrmPriority(InjectionInfo injection)
        {
            Requires.NotNull("injection", injection);
            Requires.NotNull("injection.CustomProperties", injection.CustomProperties);

            if (!injection.CustomProperties.Any(p => p.Name == InjectionInfoMembers.CrmPriorityField)) return Null.NullInteger;

            var priorityInput = injection.CustomProperties.FirstOrDefault(p => p.Name == InjectionInfoMembers.CrmPriorityField).Value;

            return GetCrmPriority(priorityInput);
        }

        public static int GetCrmPriority(string injectionPriority)
        {
            if (string.IsNullOrEmpty(injectionPriority)) return Null.NullInteger;

            var priorityLevel = Null.NullInteger;

            var result = int.TryParse(injectionPriority, out priorityLevel);

            return result ? priorityLevel : Null.NullInteger;
        }

        public static string GetCrmProvider(InjectionInfo injection)
        {
            Requires.NotNull("injection", injection);
            Requires.NotNull("injection.CustomProperties", injection.CustomProperties);

            if (!injection.CustomProperties.Any(p => p.Name == InjectionInfoMembers.CrmProviderField)) return string.Empty;

            var provider = injection.CustomProperties.FirstOrDefault(p => p.Name == InjectionInfoMembers.CrmProviderField).Value;

            return GetCrmProvider(provider);
        }

        public static string GetCrmProvider(string provider)
        {
            switch (provider)
            {
                case "1":
                    return DnnPageHeaderProvider;
                case "2":
                    return DnnBodyProvider;
                case "3":
                    return DnnFormBottomProvider;
                default:
                    return string.Empty;
            }
        }

        public static string GetCrmProviderDefault(InjectionType type)
        {
            switch (type)
            {
                case InjectionType.CSS:
                    return DnnPageHeaderProvider;
                case InjectionType.JavaScript:
                    return DnnBodyProvider;
                default:
                    return string.Empty;
            }
        }

        #endregion

        public static bool IsValidFilePath(string filePath)
        {
            Requires.NotNullOrEmpty("filePath", filePath);

            if (!filePath.StartsWith("http"))
            {
                throw new ArgumentOutOfRangeException("filePath");
            }

            try
            {
                var request = (HttpWebRequest)WebRequest.Create(filePath);
                
                var response = (HttpWebResponse)request.GetResponse();

                return (response.StatusCode == HttpStatusCode.OK);
            }
            catch (WebException webEx)
            {
                // this is generally a non-200 response
                return false;
            }
            catch (Exception ex)
            {
                Exceptions.LogException(ex);
                return false;
            }
        }
    }
}