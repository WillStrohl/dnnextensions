/*
  * Copyright (c) 2011-2019, Will Strohl
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
 * Neither the name of Will Strohl, Disqus Module, nor the names of its contributors may be used 
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
using System.Collections;
//using System.Data;
using System.IO;
using System.Net;
//using System.Xml;
using System.Text.RegularExpressions;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Services.Exceptions;

namespace DotNetNuke.Modules.WillStrohlDisqus.Components
{

    /// -----------------------------------------------------------------------------
    /// <summary>
    /// The Controller class for WillStrohlDisqus
    /// </summary>
    /// -----------------------------------------------------------------------------
    public sealed class FeatureController /*: IPortable, ISearchable, IUpgradeable*/
    {

        #region Constants

        public const string DISQUS_GENERAL_URL = "https://disqus.com/api/{0}/{1}.{2}?forum={3}&api_secret={4}{5}";
        public const string DISQUS_VERSION = "3.0";
        public const string DISQUS_OUTPUT_TYPE = "json";
        public const string DISQUS_POST_LIST = "posts/list";

        public const string DISQUS_COMMENT_SCHEDULE_NAME = "WillStrohl Disqus Module Comments";
        public const string DISQUS_COMMENT_SCHEDULE_TYPE = "DotNetNuke.Modules.WillStrohlDisqus.ImportDisqusComments, WillStrohlDisqus";

        public const string HOST_SETTING_COMMENT_SCHEDULE = "wnsDisqus-ScheduleEnabled";

        private const string DISQUS_TYPE = "DotNetNuke.Modules.WillStrohlDisqus.DisqusInfo, WillStrohlDisqus";

        #endregion

        #region Public Methods



        #endregion

        #region Data Access

        public int AddDisqus(int PortalId, int TabId, int TabModuleId, string CommentPath, string DisqusComment, string CreatedAt)
        {
            return DataProvider.Instance().AddDisqus(PortalId, TabId, TabModuleId, CommentPath, DisqusComment, CreatedAt);
        }

        public void UpdateDisqus(int LocalDbId, int PortalId, int TabId, int TabModuleId, string CommentPath, string DisqusComment, string CreatedAt)
        {
            DataProvider.Instance().UpdateDisqus(LocalDbId, PortalId, TabId, TabModuleId, CommentPath, DisqusComment, CreatedAt);
        }

        public void DeleteDisqus(int LocalDbId)
        {
            DataProvider.Instance().DeleteDisqus(LocalDbId);
        }

        public void DeleteDisqusbyPortal(int PortalId)
        {
            DataProvider.Instance().DeleteDisqusbyPortal(PortalId);
        }

        public void DeleteDisqusByTab(int TabId)
        {
            DataProvider.Instance().DeleteDisqusByTab(TabId);
        }

        public DisqusInfo GetDisqus()
        {
            return (DisqusInfo)CBO.FillObject(DataProvider.Instance().GetDisqus(), Type.GetType(DISQUS_TYPE, true));
        }

        public DisqusInfo GetDisqusByPortal(int PortalId)
        {
            return (DisqusInfo)CBO.FillObject(DataProvider.Instance().GetDisqusByPortal(PortalId), Type.GetType(DISQUS_TYPE, true));
        }

        public DisqusInfo GetDisqusByTab(int TabId)
        {
            return (DisqusInfo)CBO.FillObject(DataProvider.Instance().GetDisqusByTab(TabId), Type.GetType(DISQUS_TYPE, true));
        }

        public DisqusInfo GetDisqusByModule(int TabModuleId)
        {
            return (DisqusInfo)CBO.FillObject(DataProvider.Instance().GetDisqusByModule(TabModuleId), Type.GetType(DISQUS_TYPE, true));
        }

        #endregion

        public string GenericDisqusUrl(string DisqusForumName, string DisqusApiSecret)
        {
            return string.Format(DISQUS_GENERAL_URL, DISQUS_VERSION, "{0}", DISQUS_OUTPUT_TYPE, DisqusForumName, DisqusApiSecret, "&order=asc&related=thread");
        }

        /// <summary>
        /// GetContent - a method that imports REST content
        /// </summary>
        /// <param name="requestUrl">the REST location</param>
        /// <returns></returns>
        public string GetContent(string requestUrl)
        {
            return GetContent(requestUrl, string.Empty, -1);
        }

        /// <summary>
        /// GetContent - a method that imports REST content
        /// </summary>
        /// <param name="requestUrl">the REST location</param>
        /// <param name="proxyAddress">address for the proxy server</param>
        /// <param name="proxyPort">port for the proxy server</param>
        /// <returns></returns>
        public string GetContent(string requestUrl, string proxyAddress, int proxyPort)
        {

            string strReturn = string.Empty;
            WebRequest request = null;
            HttpWebResponse response = null;
            Stream dataStream = null;
            StreamReader reader = null;

            try
            {

                // Create a request for the URL.         
                Exceptions.LogException(new Exception(requestUrl));
                request = WebRequest.Create(requestUrl);
                if (!string.IsNullOrEmpty(proxyAddress) && proxyPort > -1)
                {
                    request.Proxy = new WebProxy(string.Concat(proxyAddress, ":", proxyPort.ToString()), true);
                }
                else
                {
                    request.Proxy = null;
                }

                // If required by the server, set the credentials.
                request.Credentials = CredentialCache.DefaultCredentials;

                // Get the response.
                response = (HttpWebResponse)request.GetResponse();

                // Display the status.
                // Console.WriteLine (response.StatusDescription);

                // Get the stream containing content returned by the server.
                dataStream = response.GetResponseStream();

                // Open the stream using a StreamReader for easy access.
                reader = new StreamReader(dataStream);

                strReturn = reader.ReadToEnd();

                // Cleanup the streams and the response.
                reader.Close();
                dataStream.Close();
                response.Close();

            }
            catch (Exception ex)
            {
                Exceptions.LogException(ex);
                throw ex;
            }

            return strReturn;

        }

        #region Search Engines

        public bool IsSearchEngine(string UserAgent)
        {
            return UserAgents().Contains(UserAgent);
        }

        private ArrayList UserAgents()
        {
            ArrayList lstUserAgents = new ArrayList();

            lstUserAgents.Add("Mozilla/5.0 (compatible; Googlebot/2.1; +http://www.google.com/bot.html)");
            lstUserAgents.Add("Googlebot/2.1 (+http://www.google.com/bot.html)");
            lstUserAgents.Add("(compatible; Googlebot-Mobile/2.1; +http://www.google.com/bot.html)");
            lstUserAgents.Add("Googlebot-News");
            lstUserAgents.Add("Mozilla/5.0 (compatible; bingbot/2.0; +http://www.bing.com/bingbot.htm)");
            lstUserAgents.Add("Mozilla/5.0 (compatible; bingbot/2.0 +http://www.bing.com/bingbot.htm)");
            lstUserAgents.Add("msnbot/2.1");
            lstUserAgents.Add("msnbot/1.1 (+http://search.msn.com/msnbot.htm)");
            lstUserAgents.Add("msnbot/1.1");
            lstUserAgents.Add("msnbot/1.0 (+http://search.msn.com/msnbot.htm)");
            lstUserAgents.Add("Mozilla/5.0 (compatible; Yahoo! Slurp; http://help.yahoo.com/help/us/ysearch/slurp)");

            return lstUserAgents;
        }

        #endregion

        #region Identifiers

        public string CreateUniqueIdentifier(int TabId, int TabModuleId, Guid GUID, string RawUrl)
        {
            /*
            * wnsdisqus-59::115::79368271-68a8-47c4-81e4-027e0be54f17::/NewsPromotions.aspx
            * string strIdentifier = string.Format("var disqus_identifier = 'wnsdisqus-{0}::{1}::{2}::{3}'; ", this.TabId, this.TabModuleId, this.PortalSettings.GUID, Request.RawUrl);
            * {0} == this.TabId
            * {1} == this.TabModuleId
            * {2} == this.PortalSettings.GUID
            * {3} == Request.RawUrl
            */

            return string.Format("wnsdisqus-{0}::{1}::{2}::{3}", TabId, TabModuleId, GUID, RawUrl);
        }

        public DisqusIdentifierInfo GetIdentifier(string Identifier)
        {
            if (!Identifier.StartsWith("wnsdisqus-")) return null;

            Identifier = Regex.Replace(Identifier, "wnsdisqus-", string.Empty, RegexOptions.IgnoreCase);
            Identifier = Regex.Replace(Identifier, "::", ":", RegexOptions.IgnoreCase);

            string strSplitter = ":";
            string[] arrValues = Identifier.Split(strSplitter.ToCharArray());

            DisqusIdentifierInfo oInfo = new DisqusIdentifierInfo();
            oInfo.TabId = Convert.ToInt32(arrValues[0]);
            oInfo.TabModuleId = Convert.ToInt32(arrValues[1]);
            oInfo.GUID = Convert.ToString(arrValues[2]);
            oInfo.RawUrl = Convert.ToString(arrValues[3]);

            return oInfo;
        }

        #endregion

        /*
        private void ExportToXml(DataTable dtSource)
        {
            var objDoc = new XmlDocument();
            var objRoot = objDoc.CreateElement("root");

            for (int i = 0; i < dtSource.Rows.Count; i++)
            {
                try
                {
                    var objItem = objDoc.CreateElement("dnnBlogComments");
                    var dr = dtSource.Rows[i];
                    for (int j = 0; j < dtSource.Columns.Count; j++)
                    {
                        var objAttr = objDoc.CreateAttribute(dtSource.Columns[j].ColumnName);
                        objAttr.Value = String.Format("{0}", dr[j]);
                        objItem.Attributes.Append(objAttr);
                    }
                    objRoot.AppendChild(objItem);
                }
                catch
                {
                    // do nothing
                }
            }
            objDoc.AppendChild(objRoot);

            var strFileDate = DateTime.Now.ToString("-yyyyMMddhhmmss");

            this.ResponseWrite(objDoc.InnerXml, string.Format("dnnBlogComments{0}.xml", strFileDate), "text/xml");
        }

        public void ResponseWrite(string result, string fileName , string contentType)
        {
            var response = System.Web.HttpContext.Current.Response;

                var lstByte = System.Text.Encoding.UTF8.GetBytes(result);

                response.ClearHeaders();
                response.ClearContent();
                response.ContentType = String.Format("{0}; charset=utf-8", contentType);
                response.AppendHeader("Content-Disposition", String.Format("attachment; filename=\"{0}\"", fileName));
                response.AppendHeader("Content-Length", lstByte.Length.ToString());
                response.BinaryWrite(lstByte);
                response.Flush();
                response.End();
        }
        */

        /*
        #region Optional Interfaces

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// ExportModule implements the IPortable ExportModule Interface
        /// </summary>
        /// <param name="ModuleID">The Id of the module to be exported</param>
        /// -----------------------------------------------------------------------------
        public string ExportModule(int ModuleID)
        {
            //string strXML = "";

            //List<WillStrohlDisqusInfo> colWillStrohlDisquss = GetWillStrohlDisquss(ModuleID);
            //if (colWillStrohlDisquss.Count != 0)
            //{
            //    strXML += "<WillStrohlDisquss>";

            //    foreach (WillStrohlDisqusInfo objWillStrohlDisqus in colWillStrohlDisquss)
            //    {
            //        strXML += "<WillStrohlDisqus>";
            //        strXML += "<content>" + DotNetNuke.Common.Utilities.XmlUtils.XMLEncode(objWillStrohlDisqus.Content) + "</content>";
            //        strXML += "</WillStrohlDisqus>";
            //    }
            //    strXML += "</WillStrohlDisquss>";
            //}

            //return strXML;

            throw new System.NotImplementedException("The method or operation is not implemented.");
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// ImportModule implements the IPortable ImportModule Interface
        /// </summary>
        /// <param name="ModuleID">The Id of the module to be imported</param>
        /// <param name="Content">The content to be imported</param>
        /// <param name="Version">The version of the module to be imported</param>
        /// <param name="UserId">The Id of the user performing the import</param>
        /// -----------------------------------------------------------------------------
        public void ImportModule(int ModuleID, string Content, string Version, int UserID)
        {
            //XmlNode xmlWillStrohlDisquss = DotNetNuke.Common.Globals.GetContent(Content, "WillStrohlDisquss");
            //foreach (XmlNode xmlWillStrohlDisqus in xmlWillStrohlDisquss.SelectNodes("WillStrohlDisqus"))
            //{
            //    WillStrohlDisqusInfo objWillStrohlDisqus = new WillStrohlDisqusInfo();
            //    objWillStrohlDisqus.ModuleId = ModuleID;
            //    objWillStrohlDisqus.Content = xmlWillStrohlDisqus.SelectSingleNode("content").InnerText;
            //    objWillStrohlDisqus.CreatedByUser = UserID;
            //    AddWillStrohlDisqus(objWillStrohlDisqus);
            //}

            throw new System.NotImplementedException("The method or operation is not implemented.");
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// GetSearchItems implements the ISearchable Interface
        /// </summary>
        /// <param name="ModInfo">The ModuleInfo for the module to be Indexed</param>
        /// -----------------------------------------------------------------------------
        public DotNetNuke.Services.Search.SearchItemInfoCollection GetSearchItems(DotNetNuke.Entities.Modules.ModuleInfo ModInfo)
        {
            //SearchItemInfoCollection SearchItemCollection = new SearchItemInfoCollection();

            //List<WillStrohlDisqusInfo> colWillStrohlDisquss = GetWillStrohlDisquss(ModInfo.ModuleID);

            //foreach (WillStrohlDisqusInfo objWillStrohlDisqus in colWillStrohlDisquss)
            //{
            //    SearchItemInfo SearchItem = new SearchItemInfo(ModInfo.ModuleTitle, objWillStrohlDisqus.Content, objWillStrohlDisqus.CreatedByUser, objWillStrohlDisqus.CreatedDate, ModInfo.ModuleID, objWillStrohlDisqus.ItemId.ToString(), objWillStrohlDisqus.Content, "ItemId=" + objWillStrohlDisqus.ItemId.ToString());
            //    SearchItemCollection.Add(SearchItem);
            //}

            //return SearchItemCollection;

            throw new System.NotImplementedException("The method or operation is not implemented.");
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// UpgradeModule implements the IUpgradeable Interface
        /// </summary>
        /// <param name="Version">The current version of the module</param>
        /// -----------------------------------------------------------------------------
        public string UpgradeModule(string Version)
        {
            throw new System.NotImplementedException("The method or operation is not implemented.");
        }

        #endregion
        */

    }

}
