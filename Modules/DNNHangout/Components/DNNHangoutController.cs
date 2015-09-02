/*
' Copyright (c) 2015 Will Strohl
'  All rights reserved.
' 
' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED
' TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
' THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF
' CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
' DEALINGS IN THE SOFTWARE.
' 
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Content;
using DotNetNuke.Entities.Content.Common;
using DotNetNuke.Entities.Portals;
using DotNetNuke.Entities.Users;
using DotNetNuke.Services.Localization;
using DotNetNuke.Services.Tokens;
using WillStrohl.Modules.DNNHangout.Entities;

namespace WillStrohl.Modules.DNNHangout.Components
{

    /// -----------------------------------------------------------------------------
    /// <summary>
    /// The Controller class for UGLabsMetaData
    /// 
    /// The FeatureController class is defined as the BusinessController in the manifest file (.dnn)
    /// DotNetNuke will poll this class to find out which Interfaces the class implements. 
    /// 
    /// The IPortable interface is used to import/export content from a DNN module
    /// 
    /// The ISearchable interface is used by DNN to index the content of a module
    /// 
    /// The IUpgradeable interface allows module developers to execute code during the upgrade 
    /// process for a module.
    /// 
    /// Below you will find stubbed out implementations of each, uncomment and populate with your own data
    /// </summary>
    /// -----------------------------------------------------------------------------
    public class DNNHangoutController //: IPortable, ISearchable, IUpgradeable
    {
        #region Constants

        public const string MODULE_TYPE_NAME = "WillStrohl.Modules.DNNHangout";
        private const string CONTENT_KEY_FORMAT = "ghid={0}";
        private const string CONTENT_KEY_FORMAT_PATTERN = @"^ghid=\d+$";

        public const string SETTINGS_HANGOUT_ID = "ghid";
        public const string SETTINGS_TEMPLATE = "ghtemplate";
        public const string SETTINGS_TEMPLATE_SCOPE = "ghtemplatescope";

        public const string TOKEN_TITLE = @"\[Hangout:Title\]";
        public const string TOKEN_STARTDATE = @"\[Hangout:StartDate\]";
        public const string TOKEN_DURATION = @"\[Hangout:Duration\]";
        public const string TOKEN_DURATIONUNITS = @"\[Hangout:DurationUnits\]";
        public const string TOKEN_DESCRIPTION = @"\[Hangout:Description\]";
        public const string TOKEN_ADDRESS = @"\[Hangout:Address\]";

        public const string TEMPLATE_CACHE_KEY_FORMAT = "WillStrohl.DNNHangout.Template.{0}";

        #endregion

        #region Private Members

        private int contentTypeId = Null.NullInteger;

        #endregion

        #region Content Item Helpers

        /// <summary>
        /// Returns a ContentTypeID for use in the content item logic in this class
        /// </summary>
        public int DNNHangoutContentTypeId
        {
            get
            {
                // just return the contentTypeId if it is already populated
                if (contentTypeId != Null.NullInteger) return contentTypeId;

                // create or query for a contentTypeId
                var typeController = new ContentTypeController();
                var colContentTypes =(from t in typeController.GetContentTypes()
                     where t.ContentType == MODULE_TYPE_NAME
                     select t);

                // check to see if the content type exists
                if (colContentTypes.Any())
                {
                    // return the existing content type id
                    var contentType = colContentTypes.Single();
                    contentTypeId = contentType == null ? CreateContentType() : contentType.ContentTypeId;
                }
                else
                {
                    // create a content type, then return the new content type id
                    contentTypeId = CreateContentType();
                }

                return contentTypeId;
            }
        }

        #endregion

        #region Data Access

        /// <summary>
        /// This method creates new DNN Hangout entries in the content item data store
        /// </summary>
        /// <param name="tabId"></param>
        /// <param name="moduleId"></param>
        /// <param name="bookmark"></param>
        /// <returns></returns>
        public int CreateContentItem(int tabId, int moduleId, HangoutInfo hangout)
        {
            // create a new content item
            var objContent = new ContentItem
            {
                Content = JsonExtensionsWeb.ToJsonString(hangout),
                ContentTypeId = DNNHangoutContentTypeId,
                Indexed = false,
                ModuleID = moduleId,
                TabID = tabId
            };

            // save the content item to the database
            objContent.ContentItemId = Util.GetContentController().AddContentItem(objContent);

            // update the objects with the new content item id retrieved in the line above
            hangout.ContentItemId = objContent.ContentItemId;

            // update the content item properties with proper content
            objContent.Content = JsonExtensionsWeb.ToJsonString(hangout);
            objContent.ContentKey = string.Format(CONTENT_KEY_FORMAT, hangout.ContentItemId);

            // update the content item again since we now have a contentItemId for the properties
            Util.GetContentController().UpdateContentItem(objContent);

            // return the content item id
            return objContent.ContentItemId;
        }

        /// <summary>
        /// This method updates an existing content item
        /// </summary>
        /// <param name="tabId"></param>
        /// <param name="moduleId"></param>
        /// <param name="contentItemId"></param>
        /// <param name="bookmark"></param>
        /// <remarks>This method will only be present in DNN 6.2.3 and newer</remarks>
        public int UpdateContentItem(int tabId, int moduleId, int contentItemId, HangoutInfo hangout)
        {
            // get a reference to the original content item
            var objContent = Util.GetContentController().GetContentItem(contentItemId);

            // if the content item doesn't exist, send back to the calling method
            // NOTE:  Probably should just throw an exception here
            if (objContent == null) return Null.NullInteger;

            // update the relevant properties
            objContent.Content = JsonExtensionsWeb.ToJsonString(hangout);

            // these are just for data posterity and probably don't need to be done
            objContent.ModuleID = moduleId;
            objContent.TabID = tabId;

            // data integrity check for legacy data (just in case stuff)
            if (!Regex.IsMatch(objContent.ContentKey, CONTENT_KEY_FORMAT_PATTERN, RegexOptions.IgnoreCase))
            {
                objContent.ContentKey = string.Format(CONTENT_KEY_FORMAT, objContent.ContentItemId);
            }

            // execute the update on the database
            Util.GetContentController().UpdateContentItem(objContent);

            return contentItemId;
        }

        /// <summary>
        /// This method deletes a content item
        /// </summary>
        /// <param name="contentItemId"></param>
        public void DeleteContentItem(int contentItemId)
        {
            // execute the deletion
            Util.GetContentController().DeleteContentItem(contentItemId);
        }

        /// <summary>
        /// This method deletes a content item
        /// </summary>
        /// <param name="contentItem"></param>
        public void DeleteContentItem(ContentItem contentItem)
        {
            // execute the deletion
            Util.GetContentController().DeleteContentItem(contentItem);
        }

        /// <summary>
        /// This method retrieves a content item from the database
        /// </summary>
        /// <param name="contentItemId"></param>
        /// <returns></returns>
        public HangoutInfo GetContentItem(int contentItemId)
        {
            // get a reference to the content item
            var objContent = Util.GetContentController().GetContentItem(contentItemId);

            // parse the content item and return a bookmark object
            return ParseContentItemForHangout(objContent);
        }

        /// <summary>
        /// This method retrieves a list of content items from the database
        /// </summary>
        /// <param name="moduleId"></param>
        /// <returns></returns>
        public IList<HangoutInfo> GetContentItems(int moduleId)
        {
            // get a collection of content items for this module that match this content type id
            var collContent = (
                from t in Util.GetContentController().GetContentItemsByModuleId(moduleId)
                where t.ContentTypeId == DNNHangoutContentTypeId
                orderby t.Content
                select t);

            //
            // NOTE:  The above is necessary because there could be multiple content types 
            // for a single module, for things like taxonomy and any number of other things 
            // that might be extending content items.
            //

            // parse the content item collection and return a bookmark collection
            return ParseContentItemsForHangoutCollection(collContent);
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Parse the content item content to populate a DNN Hangout object
        /// </summary>
        /// <param name="contentItem"></param>
        /// <returns></returns>
        private HangoutInfo ParseContentItemForHangout(ContentItem contentItem)
        {
            var hangout = new HangoutInfo();
            if (contentItem != null && !string.IsNullOrEmpty(contentItem.Content))
            {
                hangout = contentItem.Content.FromJson<HangoutInfo>();
            }

            return hangout;
        }

        /// <summary>
        /// Iterates through a collection of content items and parses them each to popular a bookmark collection
        /// </summary>
        /// <param name="contentItems"></param>
        /// <returns></returns>
        private IList<HangoutInfo> ParseContentItemsForHangoutCollection(IQueryable<ContentItem> contentItems)
        {
            var bookmarks = new List<HangoutInfo>(contentItems.Count());

            foreach (ContentItem contentItem in contentItems)
            {
                var bookmark = ParseContentItemForHangout(contentItem);
                bookmarks.Add(bookmark);
            }

            return bookmarks;
        }

        /// <summary>
        /// Creates a unique content type for this module
        /// </summary>
        /// <returns></returns>
        private static int CreateContentType()
        {
            var typeController = new ContentTypeController();
            var objContentType = new ContentType { ContentType = MODULE_TYPE_NAME };

            return typeController.AddContentType(objContentType);
        }

        #endregion

        #region Token Replacement

        public void ClearCachedTemplate(int contentItemId)
        {
            var cachedTemplate = DataCache.GetCache(string.Format(TEMPLATE_CACHE_KEY_FORMAT, contentItemId));

            if (cachedTemplate != null)
            {
                DataCache.RemoveCache(string.Format(TEMPLATE_CACHE_KEY_FORMAT, contentItemId));
            }
        }

        public string ReplaceTokens(string Template, HangoutInfo Hangout, PortalSettings Settings, int ModuleId, string LocalResourceFile)
        {
            var cachedTemplate = DataCache.GetCache(string.Format(TEMPLATE_CACHE_KEY_FORMAT, Hangout.ContentItemId));

            // check for a cached template first
            if (cachedTemplate != null)
            {
                return cachedTemplate.ToString();
            }

            // begin replacing tokens
            Template = Regex.Replace(Template, TOKEN_TITLE, Hangout.Title);
            Template = Regex.Replace(Template, TOKEN_ADDRESS, Hangout.HangoutAddress);
            Template = Regex.Replace(Template, TOKEN_DESCRIPTION, HttpUtility.HtmlDecode(Hangout.Description));
            Template = Regex.Replace(Template, TOKEN_DURATION, Hangout.Duration.ToString());
            Template = Regex.Replace(Template, TOKEN_STARTDATE, Hangout.StartDate.ToLocalTime().ToString());

            if (Hangout.DurationUnits == DurationType.Minutes)
            {
                Template = Regex.Replace(Template, TOKEN_DURATIONUNITS, Localization.GetString("Minutes.Text", LocalResourceFile));
            }
            else
            {
                Template = Regex.Replace(Template, TOKEN_DURATIONUNITS, Hangout.Duration > 1 ? Localization.GetString("Hours.Text", LocalResourceFile) : Localization.GetString("Hour.Text", LocalResourceFile));
            }

            var tr = new TokenReplace
            {
                AccessingUser = UserController.GetCurrentUserInfo(),
                DebugMessages = Settings.UserMode != PortalSettings.Mode.View,
                ModuleId = ModuleId,
                PortalSettings = Settings
            };

            // replace DNN tokens
            var template = tr.ReplaceEnvironmentTokens(Template);

            // cache the template
           DataCache.SetCache(string.Format(TEMPLATE_CACHE_KEY_FORMAT, Hangout.ContentItemId), template, DateTime.Now.AddMinutes(10));

            return template;
        }

        #endregion

        #region Optional Interfaces

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// ExportModule implements the IPortable ExportModule Interface
        /// </summary>
        /// <param name="ModuleID">The Id of the module to be exported</param>
        /// -----------------------------------------------------------------------------
        //public string ExportModule(int ModuleID)
        //{
        //string strXML = "";

        //List<UGLabsMetaDataInfo> colUGLabsMetaDatas = GetUGLabsMetaDatas(ModuleID);
        //if (colUGLabsMetaDatas.Count != 0)
        //{
        //    strXML += "<UGLabsMetaDatas>";

        //    foreach (UGLabsMetaDataInfo objUGLabsMetaData in colUGLabsMetaDatas)
        //    {
        //        strXML += "<UGLabsMetaData>";
        //        strXML += "<content>" + DotNetNuke.Common.Utilities.XmlUtils.XMLEncode(objUGLabsMetaData.Content) + "</content>";
        //        strXML += "</UGLabsMetaData>";
        //    }
        //    strXML += "</UGLabsMetaDatas>";
        //}

        //return strXML;

        //	throw new System.NotImplementedException("The method or operation is not implemented.");
        //}

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// ImportModule implements the IPortable ImportModule Interface
        /// </summary>
        /// <param name="ModuleID">The Id of the module to be imported</param>
        /// <param name="Content">The content to be imported</param>
        /// <param name="Version">The version of the module to be imported</param>
        /// <param name="UserId">The Id of the user performing the import</param>
        /// -----------------------------------------------------------------------------
        //public void ImportModule(int ModuleID, string Content, string Version, int UserID)
        //{
        //XmlNode xmlUGLabsMetaDatas = DotNetNuke.Common.Globals.GetContent(Content, "UGLabsMetaDatas");
        //foreach (XmlNode xmlUGLabsMetaData in xmlUGLabsMetaDatas.SelectNodes("UGLabsMetaData"))
        //{
        //    UGLabsMetaDataInfo objUGLabsMetaData = new UGLabsMetaDataInfo();
        //    objUGLabsMetaData.ModuleId = ModuleID;
        //    objUGLabsMetaData.Content = xmlUGLabsMetaData.SelectSingleNode("content").InnerText;
        //    objUGLabsMetaData.CreatedByUser = UserID;
        //    AddUGLabsMetaData(objUGLabsMetaData);
        //}

        //	throw new System.NotImplementedException("The method or operation is not implemented.");
        //}

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// GetSearchItems implements the ISearchable Interface
        /// </summary>
        /// <param name="ModInfo">The ModuleInfo for the module to be Indexed</param>
        /// -----------------------------------------------------------------------------
        //public DotNetNuke.Services.Search.SearchItemInfoCollection GetSearchItems(DotNetNuke.Entities.Modules.ModuleInfo ModInfo)
        //{
        //SearchItemInfoCollection SearchItemCollection = new SearchItemInfoCollection();

        //List<UGLabsMetaDataInfo> colUGLabsMetaDatas = GetUGLabsMetaDatas(ModInfo.ModuleID);

        //foreach (UGLabsMetaDataInfo objUGLabsMetaData in colUGLabsMetaDatas)
        //{
        //    SearchItemInfo SearchItem = new SearchItemInfo(ModInfo.ModuleTitle, objUGLabsMetaData.Content, objUGLabsMetaData.CreatedByUser, objUGLabsMetaData.CreatedDate, ModInfo.ModuleID, objUGLabsMetaData.ItemId.ToString(), objUGLabsMetaData.Content, "ItemId=" + objUGLabsMetaData.ItemId.ToString());
        //    SearchItemCollection.Add(SearchItem);
        //}

        //return SearchItemCollection;

        //	throw new System.NotImplementedException("The method or operation is not implemented.");
        //}

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// UpgradeModule implements the IUpgradeable Interface
        /// </summary>
        /// <param name="Version">The current version of the module</param>
        /// -----------------------------------------------------------------------------
        //public string UpgradeModule(string Version)
        //{
        //	throw new System.NotImplementedException("The method or operation is not implemented.");
        //}

        #endregion
    }
}