//
// DotNetNuke® - http://www.dotnetnuke.com
// Copyright (c) 2002-2013
// by DotNetNuke Corporation
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated 
// documentation files (the "Software"), to deal in the Software without restriction, including without limitation 
// the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and 
// to permit persons to whom the Software is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or substantial portions 
// of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
// TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
// DEALINGS IN THE SOFTWARE.
//

//INSTANT C# NOTE: Formerly VB project-level imports:
using DotNetNuke;
using DotNetNuke.Common;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Journal;
using DotNetNuke.Services.Localization;
using System;
using System.Collections;
using System.Data;
using System.Diagnostics;
using System.Linq;

using DotNetNuke.Entities.Controllers;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Modules.Definitions;
using DotNetNuke.Entities.Portals;
using DotNetNuke.Entities.Users;
using DotNetNuke.Services.FileSystem;
using DotNetNuke.Services.Search;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using WillStrohl.API.oEmbed;

namespace DotNetNuke.Modules.Media
{

    /// -----------------------------------------------------------------------------
    /// Namespace:  DotNetNuke.Modules.Media
    /// Project:    DotNetNuke
    /// Class:      MediaController
    /// -----------------------------------------------------------------------------
    /// <summary>
    /// The MediaController is the Controller class for the Media Module
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <history>
    ///		[lpointer]	31.10.2005	documented
    /// </history>
    /// -----------------------------------------------------------------------------
    public class MediaController : DotNetNuke.Entities.Modules.IPortable, DotNetNuke.Entities.Modules.IUpgradeable, DotNetNuke.Entities.Modules.ISearchable
    {

        #region  Constants

        // constants for UI elements to filter supported file types
        public const string MEDIA_FILE_TYPES_LEGACY = "swf,avi,wmv,midi,wav,asx,mp3,mpg,mpeg,asf,wma,ram,rpm,rm,mov,qt,mp4";
        public const string MEDIA_FILE_TYPES_CURRENT = ".swf,.avi,.wmv,.midi,.wav,.asx,.mp3,.mpg,.mpeg,.asf,.wma,.ram,.rpm,.rm,.mov,.qt,.mp4";

        // constants for module settings
        public const string SETTING_POSTTOJOURNAL = "DNNMedia-PostToJournal";
        public const string SETTING_POSTTOJOURNALSITEWIDE = "DNNMedia-PostToJournalSiteWide";
        public const string SETTING_OVERRIDEJOURNALSETTING = "DNNMedia-OverrideJournalSetting";
        public const string SETTING_NOTIFYONUPDATE = "DNNMedia-NotifyOnUpdate";

        // constants for the journal
        public const string KEY_JOURNALTYPE = "DNNMedia";
        private const string JOURNAL_OBJECTKEY_FORMAT = "{0}-{1}-{2}-{3}";
        private const string MEDIA_JOURNALTYPE = "link";
        private const int JOURNAL_TYPE_ID = 2;

        #endregion

        #region  Properties

        /// <summary>
        /// MEDIA_FILE_TYPES - in conjunction with the DNN global images, these are the file types that this module will work with.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks>
        /// This property had to be created to replace the constant, because DNN 5.x handles the file extensions differently than 4.x.
        /// </remarks>
        /// <history>
        /// [wstrohl] - 20100523 - Created from a constant.
        /// [wstrohl] - 20101124 - Changed the application version reference from the glbAppVersion; Added support for MP4
        /// [wstrohl] - 20101127 - Moved from the EditMedia view
        /// </history>
        public static string MEDIA_FILE_TYPES
        {
            get
            {
                if (DotNetNuke.Application.DotNetNukeContext.Current.Application.Version.Major < 5 | DotNetNuke.Application.DotNetNukeContext.Current.Application.Version.Major >= 6)
                {
                    return MEDIA_FILE_TYPES_LEGACY;
                }
                else
                {
                    return MEDIA_FILE_TYPES_CURRENT;
                }
            }
        }

        /// <summary>
        /// SUPPORTED_MEDIA_FILE_TYPES - These are the file types that this module will allow, per the Host Settings.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks>
        /// This property is a reflection of the Host Settings configuration.
        /// </remarks>
        /// <history>
        /// [wstrohl] - 20101127 - Created
        /// </history>
        public static string SUPPORTED_MEDIA_FILE_TYPES
        {
            get
            {
                Dictionary<string, string> settingsDictionary = null;
                settingsDictionary = HostController.Instance.GetSettingsDictionary();

                foreach (KeyValuePair<string, string> kvp in settingsDictionary)
                {

                    if (string.Equals(kvp.Key, "FileExtensions", StringComparison.InvariantCultureIgnoreCase))
                    {

                        return kvp.Value;

                    }

                }

                return string.Empty;
            }
        }

        #endregion

        #region  Data Access

        /// <summary>
        /// AddMedia adds a MediaInfo object to the Database
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name="objMedia">The MediaInfo object</param>
        /// <history>
        ///		[lpointer]	31.10.2005	documented
        /// </history>
        public void AddMedia(MediaInfo objMedia)
        {
            DataProvider.Instance().AddMedia(objMedia.ModuleID, objMedia.Src, objMedia.Alt, objMedia.Width, objMedia.Height, objMedia.NavigateUrl, objMedia.MediaAlignment, objMedia.AutoStart, objMedia.MediaLoop, objMedia.NewWindow, objMedia.TrackClicks, objMedia.MediaType, objMedia.MediaMessage, objMedia.LastUpdatedBy);
        }

        /// <summary>
        /// GetMedia gets the MediaInfo object from the Database
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name="moduleId">The Id of the module</param>
        /// <history>
        ///		[lpointer]	31.10.2005	documented
        /// </history>
        public MediaInfo GetMedia(int ModuleID)
        {
            MediaInfo objMedia = new MediaInfo();
            objMedia.Fill(DataProvider.Instance().GetMedia(ModuleID));
            return objMedia;
        }

        /// <summary>
        /// UpdateMedia saves the MediaInfo object to the Database
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name="objMedia">The MediaInfo object</param>
        /// <history>
        ///		[lpointer]	31.10.2005	documented
        /// </history>
        public void UpdateMedia(MediaInfo objMedia)
        {
            DataProvider.Instance().UpdateMedia(objMedia.ModuleID, objMedia.Src, objMedia.Alt, objMedia.Width, objMedia.Height, objMedia.NavigateUrl, objMedia.MediaAlignment, objMedia.AutoStart, objMedia.MediaLoop, objMedia.NewWindow, objMedia.TrackClicks, objMedia.MediaType, objMedia.MediaMessage, objMedia.LastUpdatedBy);
        }

        /// <summary>
        /// UpgradeMedia updates the image control references to Media in the Database
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name="OldModuleDefID">The Id of the old module</param>
        /// <param name="NewModuleDefID">The Id of the new module</param>
        /// <history>
        ///		[lpointer]	31.10.2005	documented
        /// </history>
        public void UpgradeMedia(int OldModuleDefID, int NewModuleDefID)
        {
            // only proceed if the expected module ids are passed
            if (OldModuleDefID > Null.NullInteger & NewModuleDefID > Null.NullInteger)
            {
                DataProvider.Instance().UpgradeMedia(OldModuleDefID, NewModuleDefID);
            }
        }

        /// <summary>
        /// DeleteMedia deletes the media associated with the specified module instance
        /// </summary>
        /// <param name="ModuleId">Integer - the id of the specific module instance</param>
        /// <remarks></remarks>
        /// <history>
        /// [wstrohl] - 20100602 - Created.
        /// </history>
        public void DeleteMedia(int ModuleId)
        {
            DataProvider.Instance().DeleteMedia(ModuleId);
        }

        #endregion

        #region  Public Methods

        /// <summary>
        /// EncodeUrl - this method switches any ampersands with the encoded versions of the ampersands for XHTML compliance
        /// </summary>
        /// <param name="URL">String - the URL to encode</param>
        /// <returns></returns>
        /// <remarks>This method does not check to see if the encoding has already been done.</remarks>
        /// <history>
        /// [wstrohl] - 20100605 - Created.
        /// </history>
        public static string EncodeUrl(string URL)
        {
            return Regex.Replace(URL, "&", "&amp;");
        }

        public List<string> DisplayMedia(int moduleId, int tabId, bool isEditable, Entities.Modules.ModuleInfo config, Entities.Portals.PortalSettings settings)
        {
            MediaInfo objMedia = GetMedia(moduleId);
            List<string> lstMedia = new List<string> { string.Empty, string.Empty, string.Empty };
            string HTMLTag = string.Empty;

            if (objMedia != null && objMedia.ModuleID > Null.NullInteger)
            {

                if (objMedia.MediaType == 0) // standard file system
                {
                    // 20120822 - WStrohl
                    // old way of retrieving files from the file system
                    // no longer needed with folder providers
                    //objMedia.Src = Globals.LinkClick(objMedia.Src.ToLower(), tabId, moduleId, false);

                    //if (objMedia.Src.IndexOf("://") == 0)
                    //{
                    //    objMedia.Src = string.Concat(settings.HomeDirectory, objMedia.Src);
                    //}

                    MediaMarkUpUtility utilMedia = new MediaMarkUpUtility();

                    // Check extension
                    switch (utilMedia.GetMediaType(objMedia))
                    {
                        case MediaType.Flash:
                            HTMLTag = utilMedia.GetFlashMarkUp(objMedia, config);
                            break;
                        case MediaType.WindowsMedia:
                            HTMLTag = utilMedia.GetWindowsMediaMarkUp(objMedia, config);
                            break;
                        case MediaType.RealPlayer:
                            HTMLTag = utilMedia.GetRealPlayerMarkUp(objMedia, config);
                            break;
                        case MediaType.Quicktime:
                            HTMLTag = utilMedia.GetQuicktimeMarkUp(objMedia, config);
                            break;
                        default:
                            HTMLTag = utilMedia.GetImageMarkUp(objMedia, config);
                            break;
                    }

                    lstMedia[0] = HTMLTag;

                }
                else if (objMedia.MediaType == 1) // embed code
                {

                    lstMedia[0] = System.Web.HttpUtility.HtmlDecode(objMedia.Src);

                }
                else if (objMedia.MediaType == 2) // oembed
                {

                    try
                    {
                        Wrapper ctlOEmbed = new Wrapper();
                        if (objMedia.Width > 0 & objMedia.Height > 0)
                        {
                            lstMedia[0] = ctlOEmbed.GetContent(new RequestInfo(objMedia.Src));
                        }
                        else
                        {
                            lstMedia[0] = ctlOEmbed.GetContent(new RequestInfo(objMedia.Src, objMedia.Width, objMedia.Height));
                        }
                    }
                    catch (Exception ex)
                    {
                        Exceptions.LogException(ex);
                        lstMedia[2] = "oEmbed.ErrorMessage";
                    }

                }

            }

            return lstMedia;
        }

        #endregion

        #region Journal Integration

        /// <summary>
        /// GetObjectKeyForJournal - a centralized way to always get a properly formatted key for journal items
        /// </summary>
        /// <param name="moduleId">The module ID (not TabModuleId)</param>
        /// <returns>Valid objectKey for use with the journal</returns>
        public static string GetObjectKeyForJournal(int moduleId, int userId)
        {
            return GetObjectKeyForJournal(KEY_JOURNALTYPE, moduleId, userId);
        }

        /// <summary>
        /// GetObjectKeyForJournal - a centralized way to always get a properly formatted key for journal items
        /// </summary>
        /// <param name="contentType">The type of content being saved to the journal</param>
        /// <param name="moduleId">The module ID (not TabModuleId)</param>
        /// <returns>Valid objectKey for use with the journal</returns>
        public static string GetObjectKeyForJournal(string contentType, int moduleId, int userId)
        {
            return string.Format(JOURNAL_OBJECTKEY_FORMAT, contentType, moduleId, userId, DateTime.Now.ToString("yyyyMMdd"));
        }

        /// <summary>
        /// Returns a journal type associated with voting (using one of the core built in journal types).
        /// </summary>
        /// <param name="portalId"></param>
        /// <returns></returns>
        public static int GetMediaJournalTypeID(int portalId)
        {
            var colJournalTypes = (from t in JournalController.Instance.GetJournalTypes(portalId) where t.JournalType == MEDIA_JOURNALTYPE select t);
            int journalTypeId;

            if (colJournalTypes.Count() > 0)
            {
                var journalType = colJournalTypes.Single();
                journalTypeId = journalType.JournalTypeId;
            }
            else
            {
                journalTypeId = JOURNAL_TYPE_ID;
            }

            return journalTypeId;
        }

        #endregion

        #region File Access

        /// <summary>
        /// GetFileFromProvider - returns a API-based file reference to files, regardless of FolderProvider
        /// </summary>
        /// <param name="portalId">the ID of the portal</param>
        /// <param name="folderName">the name of the folder</param>
        /// <param name="fileName">the name of the file</param>
        /// <returns></returns>
        public IFileInfo GetImageFromProvider(int portalId, string folderName, string fileName)
        {
            IFolderInfo oFolder = FolderManager.Instance.GetFolder(portalId, folderName);
            IFileInfo oFile = FileManager.Instance.GetFile(oFolder, fileName);

            return oFile;
        }

        /// <summary>
        /// GetFileFromProvider - returns a API-based file reference to files, regardless of FolderProvider
        /// </summary>
        /// <param name="portalId">the ID of the portal</param>
        /// <param name="folder">the reference object for the folder</param>
        /// <param name="fileName">the name of the file</param>
        /// <returns></returns>
        public IFileInfo GetFileFromProvider(int portalId, IFolderInfo oFolder, string fileName)
        {
            IFileInfo oFile = FileManager.Instance.GetFile(oFolder, fileName);
            return oFile;
        }

        /// <summary>
        /// GetImageFileUrl - this method returns the valid URL for any file, regardless to folder or folder provider in use
        /// </summary>
        /// <param name="oFile">Fully loaded IFileInfo object</param>
        /// <returns></returns>
        /// <remarks>
        /// WARNING!!! This method can return exceptions. They should be caught and processed in the UI though.
        /// </remarks>
        public string GetFileUrl(IFileInfo oFile)
        {
            /*******************************************************
            ' WARNING!!!
            ' This method can return exceptions. They should be 
            ' caught and processed in the UI though.
            '*******************************************************/
            FolderMappingInfo mapFolder = FolderMappingController.Instance.GetFolderMapping(oFile.FolderMappingID);
            return FolderProvider.Instance(mapFolder.FolderProviderType).GetFileUrl(oFile);
        }

        #endregion

        #region  IPortable

        /// <summary>
        /// ExportModule implements the IPortable ExportModule Interface
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name="ModuleID">The Id of the module to be exported</param>
        /// <history>
        ///		[lpointer]	31.10.2005	documented
        /// </history>
        public string ExportModule(int ModuleID)
        {

            try
            {

                StringBuilder sbXML = new StringBuilder();

                MediaInfo objMedia = GetMedia(ModuleID);
                if (objMedia != null)
                {
                    sbXML.Append("<image>");

                    // maintain the following xml tags as-is for backwards compatibility with old export files
                    if (objMedia.MediaType == 0)
                    {
                        int intFileId = FileManager.Instance.GetFile(DotNetNuke.Entities.Portals.PortalController.GetCurrentPortalSettings().PortalId, objMedia.Src).FileId;
                        sbXML.AppendFormat("<src>FileId={0}</src>", intFileId.ToString());
                    }
                    else
                    {
                        sbXML.AppendFormat("<src><![CDATA[{0}]]></src>", objMedia.Src);
                    }
                    sbXML.AppendFormat("<alt>{0}</alt>", objMedia.Alt);
                    sbXML.AppendFormat("<width>{0}</width>", objMedia.Width.ToString());
                    sbXML.AppendFormat("<height>{0}</height>", objMedia.Height.ToString());
                    sbXML.AppendFormat("<navigateUrl>{0}</navigateUrl>", objMedia.NavigateUrl);

                    // new/missing properties added for version 03.03.00
                    sbXML.AppendFormat("<AutoStart>{0}</AutoStart>", objMedia.AutoStart.ToString());
                    sbXML.AppendFormat("<MediaAlignment>{0}</MediaAlignment>", objMedia.MediaAlignment.ToString());
                    sbXML.AppendFormat("<MediaLoop>{0}</MediaLoop>", objMedia.MediaLoop.ToString());

                    // new/missing properties added for version 04.00.00
                    sbXML.AppendFormat("<NewWindow>{0}</NewWindow>", objMedia.NewWindow.ToString());
                    sbXML.AppendFormat("<TrackClicks>{0}</TrackClicks>", objMedia.TrackClicks.ToString());
                    sbXML.AppendFormat("<MediaType>{0}</MediaType>", objMedia.MediaType.ToString());

                    // new/missing properties added for version 04.01.00
                    sbXML.AppendFormat("<MediaMessage><![CDATA[{0}]]></MediaMessage>", objMedia.MediaMessage);
                    sbXML.AppendFormat("<LastUpdatedBy>{0}</LastUpdatedBy>", objMedia.LastUpdatedBy);
                    sbXML.AppendFormat("<LastUpdatedDate>{0}</LastUpdatedDate>", objMedia.LastUpdatedDate.ToString("MM/dd/yyyy hh:mm:ss tt"));

                    sbXML.Append("</image>");
                }

                //
                // Add settings here
                //

                return sbXML.ToString();

            }
            catch (Exception ex)
            {
                Exceptions.LogException(ex);
                throw ex;
            }

            //INSTANT C# NOTE: Inserted the following 'return' since all code paths must return a value in C#:
            return null;
        }

        /// <summary>
        /// ImportModule implements the IPortable ImportModule Interface
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name="ModuleID">The Id of the module to be imported</param>
        /// <history>
        ///		[lpointer]	31.10.2005	documented
        /// </history>
        public void ImportModule(int ModuleID, string Content, string Version, int UserId)
        {

            try
            {

                // Check to see if the module already has media. If it does, get rid of it.
                MediaInfo oMedia = new MediaInfo();
                oMedia = GetMedia(ModuleID);

                if (oMedia != null)
                {
                    DeleteMedia(ModuleID);
                }

                XmlNode xmlImage = Globals.GetContent(Content, "image");
                MediaInfo objImage = new MediaInfo();

                objImage.ModuleID = ModuleID;
                objImage.Src = xmlImage.SelectSingleNode("src").InnerText; //ImportUrl(ModuleID, xmlImage.SelectSingleNode("src").InnerText)
                objImage.Alt = xmlImage.SelectSingleNode("alt").InnerText;
                objImage.Width = int.Parse(xmlImage.SelectSingleNode("width").InnerText);
                objImage.Height = int.Parse(xmlImage.SelectSingleNode("height").InnerText);
                objImage.NavigateUrl = xmlImage.SelectSingleNode("navigateUrl").InnerText;

                // new/missing properties to assign in v03.03.00

                if (xmlImage.SelectSingleNode("AutoStart") != null)
                {
                    System.Boolean tempVar = false;
                    bool.TryParse(xmlImage.SelectSingleNode("AutoStart").InnerText, out tempVar);
                    objImage.AutoStart = tempVar;
                }
                if (xmlImage.SelectSingleNode("MediaAlignment") != null)
                {
                    System.Int32 tempVar2 = 0;
                    int.TryParse(xmlImage.SelectSingleNode("MediaAlignment").InnerText, out tempVar2);
                    objImage.MediaAlignment = tempVar2;
                }
                if (xmlImage.SelectSingleNode("MediaLoop") != null)
                {
                    System.Boolean tempVar3 = false;
                    bool.TryParse(xmlImage.SelectSingleNode("MediaLoop").InnerText, out tempVar3);
                    objImage.MediaLoop = tempVar3;
                }

                // new/missing properties to assign in v04.00.00

                if (xmlImage.SelectSingleNode("NewWindow") != null)
                {
                    System.Boolean tempVar4 = false;
                    bool.TryParse(xmlImage.SelectSingleNode("NewWindow").InnerText, out tempVar4);
                    objImage.NewWindow = tempVar4;
                }
                if (xmlImage.SelectSingleNode("TrackClicks") != null)
                {
                    System.Boolean tempVar5 = false;
                    bool.TryParse(xmlImage.SelectSingleNode("TrackClicks").InnerText, out tempVar5);
                    objImage.TrackClicks = tempVar5;
                }
                if (xmlImage.SelectSingleNode("MediaType") != null)
                {
                    System.Int32 tempVar6 = 0;
                    int.TryParse(xmlImage.SelectSingleNode("MediaType").InnerText, out tempVar6);
                    objImage.MediaType = tempVar6;
                }

                // new/missing properties to assign in v04.01.00

                if (xmlImage.SelectSingleNode("MediaMessage") != null)
                {
                    objImage.MediaMessage = xmlImage.SelectSingleNode("MediaMessage").InnerText;
                }

                UserInfo currentUser = UserController.GetCurrentUserInfo();

                if (currentUser != null && currentUser.UserID > 0)
                {
                    objImage.LastUpdatedBy = currentUser.UserID;
                }
                else
                {
                    PortalSettings portalSettings = PortalController.GetCurrentPortalSettings();

                    if (portalSettings != null)
                    {
                        objImage.LastUpdatedBy = portalSettings.AdministratorId;
                    }
                }

                objImage.LastUpdatedDate = DateTime.Now;


                AddMedia(objImage);

                // calling this method to clear the module cache, and ensure all is wired up from the DAL for the next page load
                DotNetNuke.Entities.Modules.ModuleController.SynchronizeModule(ModuleID);

            }
            catch (Exception ex)
            {
                Exceptions.LogException(ex);
                throw ex;
            }

        }

        #endregion

        #region  IUpgradeable

        public string UpgradeModule(string Version)
        {

            // Upgrade 03.02.00
            // Sets the Modules with Image to Media

            if (string.Equals(Version, "03.02.00"))
            {

                try
                {

                    MediaController objMC = new MediaController();
                    objMC.UpgradeMedia(GetModuleDefID("DNN_Image"), GetModuleDefID("DNN_Media"));
                    // clear entire cache
                    DataCache.ClearHostCache(true);

                    return "True";

                }
                catch (Exception ex)
                {
                    Exceptions.LogException(ex);
                    return "False";
                }

            }

            return "True";

        }

        #endregion

        #region  ISearchable

        public DotNetNuke.Services.Search.SearchItemInfoCollection GetSearchItems(DotNetNuke.Entities.Modules.ModuleInfo ModInfo)
        {

            SearchItemInfoCollection itmSearchColl = new SearchItemInfoCollection();
            MediaInfo itm = GetMedia(ModInfo.ModuleID);

            if (itm != null)
            {

                string content = string.Concat(itm.Alt, " :: ", StripTags(itm.MediaMessage));

                SearchItemInfo itmSearch = new SearchItemInfo(ModInfo.ModuleTitle, content, itm.LastUpdatedBy, itm.LastUpdatedDate, ModInfo.ModuleID, itm.ModuleID.ToString(), content);
                itmSearchColl.Add(itmSearch);

            }

            return itmSearchColl;

        }

        private string StripTags(string html)
        {

            string newHtml = html;
            string replaceHtml = "(<(.|\\n)*?>|&lt;(.|\\n)*?&gt;)";
            string replaceSpaces = "&(amp;)*nbsp;";
            string replaceQuotes = "&(amp;)*quot;";

            newHtml = System.Web.HttpUtility.HtmlDecode(newHtml);
            newHtml = Regex.Replace(newHtml, replaceHtml, "");
            newHtml = Regex.Replace(newHtml, replaceSpaces, "");
            newHtml = Regex.Replace(newHtml, replaceQuotes, "");

            // for troubleshooting
            //LogException(New Exception(String.Concat("newHtml = ", newHtml)))

            return newHtml;

        }

        #endregion

        #region  Private Helper Functions

        private int GetModuleDefID(string ModuleName)
        {

            try
            {

                DesktopModuleInfo desktopInfo = DesktopModuleController.GetDesktopModuleByModuleName(ModuleName, DotNetNuke.Entities.Portals.PortalController.GetCurrentPortalSettings().PortalId);
                ModuleDefinitionInfo modDefInfo = ModuleDefinitionController.GetModuleDefinitionByFriendlyName(ModuleName, desktopInfo.DesktopModuleID);

                return modDefInfo.ModuleDefID;

            }
            catch
            {
                // WStrohl:
                // do nothing - an expected nullreference exception should happen here if the module is not going through the expected upgrade
                return Null.NullInteger;
            }

        }

        #endregion

    }

}
