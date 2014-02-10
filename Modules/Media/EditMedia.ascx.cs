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

using DotNetNuke.Common;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Security.Roles;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Journal;
using DotNetNuke.Services.Social.Notifications;
using System;
using DotNetNuke.Entities.Users;
using DotNetNuke.Services.FileSystem;
using DotNetNuke.Security;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;
using WillStrohl.API.oEmbed;

namespace DotNetNuke.Modules.Media
{
    public partial class EditMedia : MediaModuleBase
	{
        #region  Private Members

        protected bool p_isNew = true;
        private bool? _isAdmin = null;
        private string p_UseModuleSettings = Null.NullString;
        private string p_None = Null.NullString;
        private string p_Left = Null.NullString;
        private string p_Center = Null.NullString;
        private string p_Right = Null.NullString;
        private string p_LastUpdated = Null.NullString;

        private string p_SupportedMediaFileTypes = Null.NullString;

        private const string SUPPORTED_IMAGE = "<img src=\"/images/grant.gif\" alt=\"{0}\" title=\"{0}\" />";
        private const string UNSUPPORTED_IMAGE = "<img src=\"/images/deny.gif\" alt=\"{0}\" title=\"{0}\" />";
        private const string FILE_TYPES_CACHE_KEY = "Media-SupportedFileTypes";

        private const string YOUTUBE_MATCH = "http://.*\\.youtube\\.com/.*";
        private const string YOUTUBE_OPAQUE = "wmode=opaque";
        private const string YOUTUBE_OPAQUE_MATCH = "(\\?|&)wmode=opaque";
        private const string YOUTUBE_EMBED_URL_MATCH = "(https*://(www\\.)*youtube\\.com/embed/[A-Za-z0-9\\?&=]+)";
        private const string YOUTUBE_EMBED_MATCH = "src=\"(https*://(www\\.)*youtube\\.com/embed/[A-Za-z0-9\\?&=]+)\"";

        public const string NOTIFICATION_TYPE = "DNNMedia_Module_Updated";

        #endregion

        #region  Properties

        private string SupportedMediaFileTypes
        {
            get
            {
                if (!(string.IsNullOrEmpty(this.p_SupportedMediaFileTypes)))
                {
                    return this.p_SupportedMediaFileTypes;
                }

                this.p_SupportedMediaFileTypes = string.Concat(Globals.glbImageFileTypes, ",", MediaController.MEDIA_FILE_TYPES);

                return this.p_SupportedMediaFileTypes;
            }
        }

        protected string SupportedImage
        {
            get
            {
                return string.Format(SUPPORTED_IMAGE, this.GetLocalizedString("SupportedImage.Text"));
            }
        }

        protected string UnsupportedImage
        {
            get
            {
                return string.Format(UNSUPPORTED_IMAGE, this.GetLocalizedString("UnsupportedImage.Text"));
            }
        }

        protected string LastUpdated
        {
            get
            {
                if (!(string.IsNullOrEmpty(this.p_LastUpdated)))
                {
                    return this.p_LastUpdated;
                }

                MediaController objMediaController = new MediaController();
                MediaInfo objMedia = objMediaController.GetMedia(ModuleId);

                if (objMedia != null)
                {
                    UserInfo user = UserController.GetUserById(this.PortalId, objMedia.LastUpdatedBy);

                    if (user != null)
                    {
                        this.p_LastUpdated = string.Format(this.GetLocalizedString("lblLastUpdated.Text"), user.DisplayName, objMedia.LastUpdatedDate.ToString("MM/dd/yyyy hh:mm:ss tt"));
                    }
                    else
                    {
                        this.p_LastUpdated = string.Format(this.GetLocalizedString("lblLastUpdated.Text"), this.GetLocalizedString("Unknown.Text"), objMedia.LastUpdatedDate.ToString("MM/dd/yyyy hh:mm:ss tt"));
                    }
                }

                return this.p_LastUpdated;
            }
        }

        private bool IsCurrentUserAdmin
        {
            get
            {
                if (!_isAdmin.HasValue)
                {
                    try
                    {
                        _isAdmin = (bool)UserInfo.IsInRole(PortalSettings.AdministratorRoleName);
                    }
                    catch (Exception ex)
                    {
                        Exceptions.LogException(ex);
                        _isAdmin = false;
                    }
                }

                return _isAdmin.Value;
            }
        }

        #endregion

        #region  Event Handlers

        /// <summary>
        /// OnInit - initialization of the module
        /// </summary>
        /// <param name="e"></param>
        override protected void OnInit(EventArgs e)
        {
            InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            //INSTANT C# NOTE: Converted event handler wireups:
            this.Load += new System.EventHandler(Page_Load);
            cmdCancel.Click += new System.EventHandler(cmdCancel_Click);
            cmdUpdate.Click += new System.EventHandler(cmdUpdate_Click);
            cvMediaType.ServerValidate += new System.Web.UI.WebControls.ServerValidateEventHandler(cvMediaType_ServerValidate);
            radMediaType.SelectedIndexChanged += new System.EventHandler(radMediaType_SelectedIndexChanged);
            lnkOEmbed.Click += new System.EventHandler(lnkOEmbed_Click);
            chkOverrideJournalSetting.CheckedChanged += new System.EventHandler(chkOverrideJournalSetting_CheckedChanged);
        }

        /// <summary>
        /// Page_Load runs when the control is loaded
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <history>
        /// </history>
        private void Page_Load(object sender, System.EventArgs e)
        {
            try
            {
                DotNetNuke.Framework.jQuery.RequestDnnPluginsRegistration();

                //Get the IsNew state from the ViewState
                p_isNew = Convert.ToBoolean(ViewState["IsNew"]);

                if (!Page.IsPostBack)
                {
                    this.BindData();
                }

                //Save the IsNew state to the ViewState
                ViewState["IsNew"] = p_isNew;

            }
            catch (Exception exc) //Module failed to load
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }

        }

        /// <summary>
        /// cmdCancel_Click runs when the cancel button is clicked
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <history>
        /// </history>
        private void cmdCancel_Click(object sender, EventArgs e)
        {

            this.SendBackToModule();

        }

        /// <summary>
        /// cmdUpdate_Click runs when the update button is clicked
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <history>
        /// </history>
        private void cmdUpdate_Click(object sender, EventArgs e)
        {

            if (Page.IsValid)
            {
                this.SaveMedia();
                this.SendBackToModule();
            }

        }

        private void cvMediaType_ServerValidate(object source, System.Web.UI.WebControls.ServerValidateEventArgs args)
        {

            switch (this.radMediaType.SelectedIndex)
            {
                case 0:
                    if (string.IsNullOrEmpty(this.ctlURL.Url))
                    {
                        args.IsValid = false;
                    }
                    break;
                case 1:
                    if (string.IsNullOrEmpty(this.txtEmbed.Text))
                    {
                        args.IsValid = false;
                    }
                    break;
                case 2:
                    if (string.IsNullOrEmpty(this.txtOEmbed.Text))
                    {
                        args.IsValid = false;
                    }
                    break;
            }

        }

        private void radMediaType_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            this.ToggleFileTypeView(this.radMediaType.SelectedIndex);
        }

        private void lnkOEmbed_Click(object sender, System.EventArgs e)
        {

            if (!(string.IsNullOrEmpty(this.txtOEmbed.Text)))
            {
                ProviderFormat ctlOEmbedProvider = new ProviderFormat();
                if (ctlOEmbedProvider.IsUrlSupported(this.txtOEmbed.Text))
                {
                    this.lblOEmbedCheck.Text = string.Concat("<div class=\"dnnFormMessage dnnFormSuccess\">", this.GetLocalizedString("lblOEmbedCheck.Text.Supported"), "</div>");
                }
                else
                {
                    this.lblOEmbedCheck.Text = string.Concat("<div class=\"dnnFormMessage dnnFormWarning\">", this.GetLocalizedString("lblOEmbedCheck.Text.Unsupported"), "</div>");
                }
            }
            else
            {
                this.lblOEmbedCheck.Text = string.Concat("<div class=\"dnnFormMessage dnnFormWarning\">", this.GetLocalizedString("lblOEmbedCheck.Text.EmptyString"), "</div>");
            }

        }

        private void chkOverrideJournalSetting_CheckedChanged(object sender, EventArgs e)
        {
            HandleOverrideCheckboxEvent();
        }

        #endregion

        #region Databinding

        private void BindData()
        {

            this.LocalizeModule();

            // Obtain a single row of text information
            MediaController objMediaController = new MediaController();
            MediaInfo objMedia = objMediaController.GetMedia(ModuleId);

            this.BindControls();

            if (objMedia != null && objMedia.ModuleID > Null.NullInteger)
            {
                p_isNew = false;
                this.ddlImageAlignment.SelectedValue = objMedia.MediaAlignment.ToString();
                this.ToggleFileTypeView(objMedia.MediaType);
                switch (objMedia.MediaType)
                {
                    case 0: // local file system
                        this.ctlURL.Url = objMedia.Src;
                        break;
                    case 1: // embed code
                        this.txtEmbed.Text = System.Web.HttpUtility.HtmlDecode(objMedia.Src);
                        break;
                    case 2: // embedable url
                        this.txtOEmbed.Text = objMedia.Src;
                        break;
                }
                this.txtAlt.Text = objMedia.Alt;
                if (objMedia.Width != Null.NullInteger)
                {
                    this.txtWidth.Text = objMedia.Width.ToString();
                }
                if (objMedia.Height != Null.NullInteger)
                {
                    this.txtHeight.Text = objMedia.Height.ToString();
                }
                this.ctlNavigateUrl.Url = objMedia.NavigateUrl;
                this.ctlTracking.URL = objMedia.NavigateUrl;
                this.ctlTracking.ModuleID = ModuleId;
                this.chkAutoStart.Checked = objMedia.AutoStart;
                this.chkLoop.Checked = objMedia.MediaLoop;

                this.txtMessage.Text = objMedia.MediaMessage;
            }
            else
            {
                p_isNew = true;
            }

            // populate the setting controls
            BindSettings();

            // populate the support file types in the UI
            this.BindSupportedFileTypes();

            ToggleSettingViews();

            // only show the side-wide settings when Admnistrators are logged in
            liSideWideJournalSetting.Visible = UserInfo.IsInRole(PortalSettings.AdministratorRoleName);
            liNotifyOnUpdate.Visible = liSideWideJournalSetting.Visible;
        }

        private void BindControls()
        {

            // Load the Alignment list with localised values
            ddlImageAlignment.Items.Add(new ListItem(p_UseModuleSettings, "0"));
            ddlImageAlignment.Items.Add(new ListItem(p_None, "1"));
            ddlImageAlignment.Items.Add(new ListItem(p_Left, "2"));
            ddlImageAlignment.Items.Add(new ListItem(p_Center, "3"));
            ddlImageAlignment.Items.Add(new ListItem(p_Right, "4"));

            ctlURL.FileFilter = string.Concat(Globals.glbImageFileTypes, ",", MediaController.MEDIA_FILE_TYPES);
            ctlNavigateUrl.ShowNewWindow = true;
            ctlNavigateUrl.ShowTrack = true;
            ctlNavigateUrl.ShowTabs = true;
            ctlNavigateUrl.ShowFiles = true;

            this.ToggleFileTypeView(-1);

        }

        private void BindSupportedFileTypes()
        {

            List<FileTypeInfo> collFiles = new List<FileTypeInfo>();
            object objCache = DotNetNuke.Services.Cache.CachingProvider.Instance().GetItem(FILE_TYPES_CACHE_KEY);

            if (objCache != null)
            {
                collFiles = (List<FileTypeInfo>)objCache;
            }
            else
            {

                ICollection<string> arrModuleSupport = (ICollection<string>)(MediaController.MEDIA_FILE_TYPES.Replace(".", string.Empty).Replace(" ", string.Empty).Split(','));
                ICollection<string> arrHostSupport = (ICollection<string>)(MediaController.SUPPORTED_MEDIA_FILE_TYPES.Split(','));

                foreach (string oString in arrModuleSupport)
                {
                    FileTypeInfo oFile = new FileTypeInfo();
                    oFile.FileType = oString.ToLower();
                    oFile.ModuleSupport = true;
                    oFile.HostSupport = arrHostSupport.Contains(oString);
                    collFiles.Add(oFile);
                }

                collFiles.Sort((p1, p2) => p1.FileType.CompareTo(p2.FileType));

                DotNetNuke.Services.Cache.CachingProvider.Instance().Insert(FILE_TYPES_CACHE_KEY, collFiles);

            }

            this.rptMediaFileTypes.DataSource = collFiles;
            this.rptMediaFileTypes.DataBind();

        }

        private void BindSettings()
        {
            chkPostToJournal.Checked = PostToJournal;
            chkPostToJournalSiteWide.Checked = PostToJournalSiteWide;
            chkOverrideJournalSetting.Checked = OverrideJournalSetting;
            chkNotifyOnUpdate.Checked = NotifyOnUpdate;
        }

        private void LocalizeModule()
        {
            p_UseModuleSettings = this.GetLocalizedString("UseModuleSettings.Text");
            p_None = this.GetLocalizedString("None.Text");
            p_Left = this.GetLocalizedString("Left.Text");
            p_Center = this.GetLocalizedString("Center.Text");
            p_Right = this.GetLocalizedString("Right.Text");

            if (radMediaType.Items.Count == 0)
            {
                this.radMediaType.Items.Insert(0, new ListItem(this.GetLocalizedString("radMediaType.Items.0"), "0"));
                this.radMediaType.Items.Insert(1, new ListItem(this.GetLocalizedString("radMediaType.Items.1"), "1"));
                this.radMediaType.Items.Insert(2, new ListItem(this.GetLocalizedString("radMediaType.Items.2"), "2"));
            }

            this.txtEmbed.Attributes["title"] = this.GetLocalizedString("plEmbed.Help");
            this.txtOEmbed.Attributes["title"] = this.GetLocalizedString("plOEmbed.Help");
            this.txtAlt.Attributes["title"] = this.GetLocalizedString("plAlt.Help");
            this.txtWidth.Attributes["title"] = this.GetLocalizedString("plWidth.Help");
            this.txtHeight.Attributes["title"] = this.GetLocalizedString("plHeight.Help");
            this.ddlImageAlignment.Attributes["title"] = this.GetLocalizedString("plAlignment.Help");
            this.chkAutoStart.Attributes["title"] = this.GetLocalizedString("lblAutoStart.Help");
            this.chkLoop.Attributes["title"] = this.GetLocalizedString("lblLoop.Help");

            this.valAltText.ErrorMessage = this.GetLocalizedString("valAltText.ErrorMessage");
            this.valHeight.ErrorMessage = this.GetLocalizedString("valHeight.ErrorMessage");
            this.valWidth.ErrorMessage = this.GetLocalizedString("valWidth.ErrorMessage");
            this.cvMediaType.ErrorMessage = this.GetLocalizedString("valMediaType.ErrorMessage");

            this.lnkOEmbed.Text = this.GetLocalizedString("lnkOEmbed.Text");
        }

        #endregion

        #region Saving Methods

        private void SaveMedia()
        {

            var objMediaController = new MediaController();
            var objMedia = new MediaInfo();

            try
            {
                // Update settings in the database
                if (this.radMediaType.SelectedIndex == 0) // standard file system
                {
                    if (string.Equals(ctlURL.UrlType, "F"))
                    {
                        IFileInfo objFile = FileManager.Instance.GetFile(int.Parse(Regex.Match(this.ctlURL.Url, "\\d+").Value, System.Globalization.NumberStyles.Integer));
                        if (objFile != null)
                        {
                            if (string.IsNullOrEmpty(this.txtWidth.Text))
                            {
                                this.txtWidth.Text = objFile.Width.ToString();
                            }
                            if (string.IsNullOrEmpty(this.txtHeight.Text))
                            {
                                this.txtHeight.Text = objFile.Height.ToString();
                            }
                        }
                    }
                }

                var sec = new PortalSecurity();

                objMedia.ModuleID = ModuleId;
                objMedia.MediaType = this.radMediaType.SelectedIndex;
                switch (this.radMediaType.SelectedIndex)
                {
                    case 0: // standard file system
                        objMedia.Src = this.ctlURL.Url;
                        break;
                    case 1: // embed code
                        objMedia.Src = this.txtEmbed.Text;
                        break;
                    case 2: // oembed url
                        objMedia.Src = this.txtOEmbed.Text;
                        break;
                }

                // ensure that youtube gets formatted correctly
                objMedia.Src = ReformatForYouTube(objMedia.Src);

                objMedia.Alt = sec.InputFilter(this.txtAlt.Text, PortalSecurity.FilterFlag.NoMarkup);
                if (!(string.IsNullOrEmpty(this.txtWidth.Text)))
                {
                    objMedia.Width = int.Parse(sec.InputFilter(this.txtWidth.Text, PortalSecurity.FilterFlag.NoMarkup), System.Globalization.NumberStyles.Integer);
                }
                if (!(string.IsNullOrEmpty(this.txtHeight.Text)))
                {
                    objMedia.Height = int.Parse(sec.InputFilter(this.txtHeight.Text, PortalSecurity.FilterFlag.NoMarkup), System.Globalization.NumberStyles.Integer);
                }
                objMedia.NavigateUrl = sec.InputFilter(this.ctlNavigateUrl.Url, PortalSecurity.FilterFlag.NoMarkup);
                objMedia.MediaAlignment = int.Parse(sec.InputFilter(this.ddlImageAlignment.SelectedValue, PortalSecurity.FilterFlag.NoMarkup), System.Globalization.NumberStyles.Integer);
                objMedia.AutoStart = this.chkAutoStart.Checked;
                objMedia.MediaLoop = this.chkLoop.Checked;
                objMedia.LastUpdatedBy = this.UserId;
                objMedia.MediaMessage = sec.InputFilter(this.txtMessage.Text, PortalSecurity.FilterFlag.NoScripting);

                // url tracking
                var objUrls = new UrlController();
                objUrls.UpdateUrl(PortalId, this.ctlNavigateUrl.Url, this.ctlNavigateUrl.UrlType, this.ctlNavigateUrl.Log, this.ctlNavigateUrl.Track, ModuleId, this.ctlNavigateUrl.NewWindow);

                // update settings/preferences
                SaveMediaSettings();

                // add/update
                if (p_isNew)
                {
                    // add new media
                    objMediaController.AddMedia(objMedia);
                }
                else
                {
                    // update existing media
                    objMediaController.UpdateMedia(objMedia);
                }

                // save the update into the journal
                if (PostToJournal) AddMediaUpdateToJournal(objMedia);

                // notify the site administrators
                if (NotifyOnUpdate) SendNotificationToMessageCenter(objMedia);

            }
            catch (Exception exc) //Module failed to load
            {
                Exceptions.LogException(exc);
                //ProcessModuleLoadException(Me, exc)
            }

        }

        /// <summary>
        /// SaveMediaSettings - this allows you to abstract the saving of the settings that can be defined
        /// </summary>
        /// <remarks>
        /// 20120820 - Created.
        /// </remarks>
        private void SaveMediaSettings()
        {
            Entities.Modules.ModuleController ctlModule = new Entities.Modules.ModuleController();

            // save settings to the module settings data store
            ctlModule.UpdateModuleSetting(ModuleId, MediaController.SETTING_POSTTOJOURNAL, chkPostToJournal.Checked.ToString());
            ctlModule.UpdateModuleSetting(ModuleId, MediaController.SETTING_OVERRIDEJOURNALSETTING, chkOverrideJournalSetting.Checked.ToString());

            ///
            /// TODO: Need to add logic to take the setting changes below into account before leaving the page
            /// Example: Notify users before leaving this view if that setting just got saved.
            /// 

            if (IsCurrentUserAdmin)
            {
                // save settings to the portal settings data store
                Entities.Portals.PortalController.UpdatePortalSetting(PortalId, MediaController.SETTING_POSTTOJOURNAL, chkPostToJournal.Checked.ToString(), true, PortalSettings.CultureCode);
                Entities.Portals.PortalController.UpdatePortalSetting(PortalId, MediaController.SETTING_POSTTOJOURNALSITEWIDE, chkPostToJournalSiteWide.Checked.ToString(), true, PortalSettings.CultureCode);

                Entities.Portals.PortalController.UpdatePortalSetting(PortalId, MediaController.SETTING_NOTIFYONUPDATE, chkNotifyOnUpdate.Checked.ToString(), true, PortalSettings.CultureCode);
            }

            // force the module to use the new settings
            Entities.Modules.ModuleController.SynchronizeModule(ModuleId);
        }

        #endregion

        #region Navigation

        private void SendBackToModule()
        {

            try
            {
                Response.Redirect(Globals.NavigateURL(), true);
            }
            catch
            {
                // do nothing
            }

        }

        #endregion

        #region Utility Helpers

        protected string GetSupportedImage(object oValue)
        {

            if (oValue != null)
            {
                var strValue = oValue.ToString();
                if (string.Equals(strValue.ToLower(), "true"))
                {
                    return SupportedImage;
                }
                else
                {
                    return UnsupportedImage;
                }
            }
            else
            {
                return UnsupportedImage;
            }

        }

        private void ToggleFileTypeView(int Selected)
        {

            if (Selected < 0 | Selected > 2)
            {
                Selected = 0;
            }

            this.radMediaType.SelectedIndex = Selected;

            this.liFileSystem.Visible = (Selected == 0);
            this.liEmbed.Visible = (Selected == 1);
            this.liOEmbed.Visible = (Selected == 2);

        }

        private string ReformatForYouTube(string embedCode)
        {

            string strReturn = Server.HtmlDecode(embedCode);

            if (Regex.IsMatch(strReturn, YOUTUBE_EMBED_MATCH, RegexOptions.IgnoreCase))
            {

                // this is an embed code
                string strUrl = Regex.Match(embedCode, YOUTUBE_EMBED_MATCH, RegexOptions.IgnoreCase).Groups[1].Value;

                if (!(Regex.IsMatch(strUrl, YOUTUBE_OPAQUE_MATCH, RegexOptions.IgnoreCase)))
                {
                    strUrl =  (strUrl.Contains("?")) ? string.Concat(strUrl, "&", YOUTUBE_OPAQUE) : string.Concat(strUrl, "?", YOUTUBE_OPAQUE);
                }

                strReturn = Regex.Replace(embedCode, YOUTUBE_EMBED_URL_MATCH, strUrl, RegexOptions.IgnoreCase);

            }
            else if (Regex.IsMatch(strReturn, YOUTUBE_MATCH, RegexOptions.IgnoreCase))
            {

                // this is a URL
                if (!(Regex.IsMatch(strReturn, YOUTUBE_OPAQUE_MATCH, RegexOptions.IgnoreCase)))
                {
                    strReturn = (strReturn.Contains("?")) ? string.Concat(strReturn, "&", YOUTUBE_OPAQUE) : string.Concat(strReturn, "?", YOUTUBE_OPAQUE);
                }

            }

            var sec = new PortalSecurity();

            return sec.InputFilter(strReturn, PortalSecurity.FilterFlag.NoMarkup);

        }

        private void ToggleSettingViews()
        {
            // enable the ability to specify post to journal when:
            // 1. Site-wide setting is enabled
            // ... AND ...
            // 2. Override journal setting is disabled
            if (PostToJournalSiteWide && OverrideJournalSetting == false)
            {
                chkPostToJournal.Enabled = false;
            }
            else
            {
                chkPostToJournal.Enabled = true;
            }


            // enabke the override capability with:
            // 1. Site-wide setting is enabled
            chkOverrideJournalSetting.Enabled = PostToJournalSiteWide;
        }

        private void HandleOverrideCheckboxEvent()
        {
            if (PostToJournalSiteWide)
            {
                chkPostToJournal.Enabled = chkOverrideJournalSetting.Checked;
            }
            else
            {
                chkPostToJournal.Enabled = true;
            }
        }

        #endregion

        #region Social Integration

        /// <summary>
        /// Adds the media update to the journal
        /// </summary>
        /// <param name="oMedia"></param>
        private void AddMediaUpdateToJournal(MediaInfo oMedia)
        {
            MediaController ctlMedia = new MediaController();

            //
            // Object Keys - great to GET data out of the journal
            // Create an oject key scheme unique to your data that will likely never change
            //
            var objectKey = MediaController.GetObjectKeyForJournal(ModuleId, UserId);

            //
            // Defensive coding to prevent duplicate journal entries
            //

            // attempt to get a reference to a journal item
            var ji = JournalController.Instance.GetJournalItemByKey(PortalId, objectKey);

            // delete the journal item if it already exists
            if (ji != null) JournalController.Instance.DeleteJournalItemByKey(PortalId, objectKey);

            // ensure we have a valid title for the journal status
            string tabTitle = PortalSettings.ActiveTab.Title;

            // if there isn't a Page Title in the Page Settings, use the Page Name
            if (string.IsNullOrEmpty(tabTitle)) tabTitle = PortalSettings.ActiveTab.TabName;

            // Method unique to the Media Module to generate Media markup for images, videos, music, etc.
            List<string> lstMedia = ctlMedia.DisplayMedia(ModuleId, TabId, IsEditable, ModuleConfiguration, PortalSettings);

            //
            // Create a journal item object
            //

            // If valid media was returned, create a new journal item object
            if (!string.IsNullOrEmpty(lstMedia[0]))
            {
                // ensure that an acceptable description is generated for the journal
                string desc = string.Empty;
                if (!Regex.IsMatch(oMedia.ContentType, @"^image/", RegexOptions.IgnoreCase))
                {
                    // use a generic placeholder for non-image media as a click-through
                    desc = string.Format("<a href=\"{0}\"><img src=\"/DesktopModules/Media/Images/play-button.png\" alt=\"{1}\" /></a>", 
                        Common.Globals.NavigateURL(TabId), 
                        oMedia.Alt);
                }
                else
                {
                    // use the saved media
                    desc = lstMedia[0];
                }

                // create a new journal item object
                ji = new JournalItem
                {
                    PortalId = PortalId,  // site id number
                    ProfileId = UserId,  // profile id that this should be posted under
                    UserId = UserId,  // id of the user account that generated the journal item
                    ContentItemId = ModuleId,  // ordinarily, this will be a contentID
                    Title = oMedia.Alt,  // title of the journal item in the meta data
                    ItemData = new ItemData { 
                        ImageUrl = "\"/DesktopModules/Media/Images/play-button.png\"", 
                        Description = desc, 
                        Title = oMedia.Alt, 
                        Url = Common.Globals.NavigateURL(TabId) },  // used to populate the journal item template, depending on the journal type
                    Summary = string.Format(GetLocalizedString("Journal.Status.Media.Updated"), 
                        oMedia.Alt, 
                        Common.Globals.NavigateURL(TabId), 
                        tabTitle),  // the text shown in the journal status
                    Body = null,  // not really used in the default templates, but could be in your own
                    JournalTypeId = MediaController.GetMediaJournalTypeID(PortalId),  // local method to choose the journal type globally
                    ObjectKey = objectKey,  // your object key from above
                    SecuritySet = "E,"  // valid values include:  E = Everyone, F = Friends Only, U = Private, P = Profile Only, R[n] = Role Id
                };
            }
            else
            {
                // create a generic journal item object for the media
                ji = new JournalItem
                {
                    PortalId = PortalId,
                    ProfileId = UserId,
                    UserId = UserId,
                    ContentItemId = ModuleId,  // ordinarily, this will be a contentID
                    Title = oMedia.Alt,
                    Summary = string.Format(GetLocalizedString("Journal.Status.Media.Updated"), 
                        oMedia.Alt, 
                        Common.Globals.NavigateURL(TabId), 
                        tabTitle),
                    Body = null,
                    JournalTypeId = MediaController.GetMediaJournalTypeID(PortalId),
                    ObjectKey = objectKey,
                    SecuritySet = "E,"
                };
            }

            // send your new journal item to the feed
            JournalController.Instance.SaveJournalItem(ji, TabId);
        }

        /// <summary>
        /// Sends a notification to the message center
        /// </summary>
        /// <param name="oMedia"></param>
        private void SendNotificationToMessageCenter(MediaInfo oMedia)
        {
            var notificationType = NotificationsController.Instance.GetNotificationType(NOTIFICATION_TYPE);
            if (notificationType == null)
            {
                // add the required notification type
                AddNotificationTypes();
                // repopulate the local variable
                notificationType = NotificationsController.Instance.GetNotificationType(NOTIFICATION_TYPE);
            }

            // generate a notification key
            var notificationKey = string.Format("{0}:{1}:{2}", NOTIFICATION_TYPE, TabId, ModuleId);

            // generate a body for the notification message
            var messsageBody = string.Format(GetLocalizedString("Notification.Body.ModuleUpdated"), 
                    oMedia.Alt,
                    Globals.NavigateURL(), 
                    PortalSettings.ActiveTab.TabName);

            // create a new notification object
            var objNotification = new Notification
            {
                NotificationTypeID = notificationType.NotificationTypeId,
                Subject = GetLocalizedString("Notification.Subject.ModuleUpdated"),
                Body = messsageBody,
                IncludeDismissAction = true,
                SenderUserID = UserId,
                Context = notificationKey
            };

            var colRoles = new List<RoleInfo>();
            var ctlRole = new RoleController();

            // get admin role
            colRoles.Add(ctlRole.GetRoleByName(PortalId, PortalSettings.AdministratorRoleName));

            // notify admins
            NotificationsController.Instance.SendNotification(objNotification, PortalId, colRoles, null);
        }

        /// <summary>
        /// This will create a notification type used by the module and also handle the actions that must be associated with it.
        /// </summary>
        static internal void AddNotificationTypes()
        {
            // part of the notification custom actions logic
            //var actions = new List<NotificationTypeAction>();

            var deskModuleId = DesktopModuleController.GetDesktopModuleByFriendlyName("Media").DesktopModuleID;

            var objNotificationType = new NotificationType
            {
                Name = NOTIFICATION_TYPE,
                Description = "DotNetNuke Media Settings Updated",
                DesktopModuleId = deskModuleId
            };

            if (NotificationsController.Instance.GetNotificationType(objNotificationType.Name) != null) return;

            // part of the notification custom actions logic
            // Code like this would allow us to call back our own code to perform an action
            //var objAction = new NotificationTypeAction
            //{
            //    NameResourceKey = "Informed",
            //    DescriptionResourceKey = "InformedDnnMediaUpdate",
            //    APICall = "DesktopModules/Media/ServiceName.ashx/ActionMethod",
            //    Order = 1
            //};
            //actions.Add(objAction);

            NotificationsController.Instance.CreateNotificationType(objNotificationType);

            // part of the notification custom actions logic
            //NotificationsController.Instance.SetNotificationTypeActions(actions, objNotificationType.NotificationTypeId);
        }

        #endregion
    }
}