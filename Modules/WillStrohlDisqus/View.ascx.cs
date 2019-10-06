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

using DotNetNuke.Services.Exceptions;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Modules.Actions;
using DotNetNuke.Services.FileSystem;
using DotNetNuke.Services.Localization;
using DotNetNuke.Security;
using DotNetNuke.UI.Skins.Controls;
using DotNetNuke.Modules.WillStrohlDisqus.Components;
using System;
using System.Globalization;
using System.IO;
using System.Security.Cryptography;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;

namespace DotNetNuke.Modules.WillStrohlDisqus
{

    /// -----------------------------------------------------------------------------
    /// <summary>
    /// The ViewWillStrohlDisqus class displays the content
    /// </summary>
    /// -----------------------------------------------------------------------------
    public partial class View : WillStrohlDisqusModuleBase, IActionable
    {
        private const string c_EditModule = "EditModule";
        private const string c_AboutMe = "AboutMe";

        #region Private Properties

        private int AttachedModule { get; set; }
        private string DisqusView { get; set; }
        private int DisplayItems { get; set; }
        private bool ShowModerators { get; set; }
        private string ColorTheme { get; set; }
        private string DefaultTab { get; set; }
        private int CommentLength { get; set; }
        private bool ShowAvatar { get; set; }
        private int AvatarSize { get; set; }

        #endregion

        #region Event Handlers

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
            this.Load += new System.EventHandler(this.PageLoad);
        }
        
        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Page_Load runs when the control is loaded
        /// </summary>
        /// -----------------------------------------------------------------------------
        private void PageLoad(object sender, System.EventArgs e)
        {
            try
            {
                if (!this.IsPostBack) this.BindData();
            }
            catch (Exception exc) //Module failed to load
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        #endregion

        #region Private Helper Methods

        private void BindData()
        {
            this.LocalizeModule();

            this.LoadSettings();

            this.ParseModuleState();

            phScript.Visible = DisqusVisibility();

            if (phScript.Visible == false)
            {
                divComments.Visible = true;
                divComments.Attributes["class"] = "wns-clear dnnClear";
                divNoLogin.Visible = true;
            }
            else
            {
                divComments.Visible = false;
                divComments.Attributes["class"] = "wns-disqus-comments";
                divNoLogin.Visible = false;   
            }

        }

        private void LocalizeModule()
        {
            // do nothing
        }

        private void LoadSettings()
        {

            if (this.Settings["AttachedModuleId"] != null)
                this.AttachedModule = int.Parse(this.Settings["AttachedModuleId"].ToString(), NumberStyles.Integer);

            if (this.Settings["DisqusView"] != null)
            {
                this.DisqusView = this.Settings["DisqusView"].ToString();
            }
            else
            {
                ModuleController ctlModule = new ModuleController();
                ctlModule.UpdateTabModuleSetting(this.TabModuleId, "DisqusView", "comments");
                ModuleController.SynchronizeModule(this.ModuleId);
                this.DisqusView = "comments";
            }

            if (this.Settings["DisplayItems"] != null)
                this.DisplayItems = int.Parse(this.Settings["DisplayItems"].ToString(), NumberStyles.Integer);

            if (this.Settings["ShowModerators"] != null)
                this.ShowModerators = bool.Parse(this.Settings["ShowModerators"].ToString());

            if (this.Settings["ColorTheme"] != null)
                this.ColorTheme = this.Settings["ColorTheme"].ToString();

            if (this.Settings["DefaultTab"] != null)
                this.DefaultTab = this.Settings["DefaultTab"].ToString();

            if (this.Settings["CommentLength"] != null)
                this.CommentLength = int.Parse(this.Settings["CommentLength"].ToString(), NumberStyles.Integer);

            if (this.Settings["ShowAvatar"] != null)
                this.ShowAvatar = bool.Parse(this.Settings["ShowAvatar"].ToString());

            if (this.Settings["AvatarSize"] != null)
                this.AvatarSize = int.Parse(this.Settings["AvatarSize"].ToString(), NumberStyles.Integer);
        }

        #region Login Helpers

        protected string LoginMessage()
        {
            if (PortalSettings.UserRegistration == 0)
            {
                return string.Format(GetLocalizedString("LoginMessage.Text"), GetLoginUrl());
            }
            else
            {
                return string.Format(GetLocalizedString("LoginMessage.Register.Text"), GetLoginUrl(), GetRegisterUrl());
            }
        }

        private string GetLoginUrl()
        {
            string returnUrl = Request.RawUrl;
            returnUrl = HttpUtility.UrlEncode(returnUrl);

            return Common.Globals.LoginURL(returnUrl, false);
        }

        private string GetRegisterUrl()
        {
            return Common.Globals.RegisterURL(HttpUtility.UrlEncode(Common.Globals.NavigateURL()), string.Empty);
        }

        #endregion

        #region View Helpers

        private bool DisqusVisibility()
        {
            if (string.Equals(DisqusView, "comments"))
            {
                if ((UserId == -1 && RequireDnnLogin) || (UserId == -1 && DisqusSsoEnabled))
                {
                    return false;
                }
            }

            return true;
        }

        private void ParseModuleState()
        {
            switch(this.DisqusView)
            {
                case "comments":
                    this.ParseCommentsView();
                    break;
                case "combination":
                    this.GetDisqusCombinationScript();
                    break;
                case "recent-comments":
                    this.GetDisqusRecentCommentsScript();
                    break;
                case "popular-threads":
                    this.GetDisqusPopularThreadScript();
                    break;
                case "top-commenters":
                    this.GetDisqusTopCommentersScript();
                    break;
            }
        }

        private void ParseCommentsView()
        {
            // if a setting isn't in place, let the end-user know

            if (this.AttachedModule > 0 && string.IsNullOrEmpty(DisqusApplicationName) == false && string.IsNullOrEmpty(DisqusApiSecret) == false)
            {
                // add the view
                this.phScript.Controls.Add(new LiteralControl(this.GetDisqusCommentsScript()));

                FeatureController ctlModule = new FeatureController();
                DisqusInfo oComment = ctlModule.GetDisqusByModule(TabModuleId);
                divComments.Visible = ctlModule.IsSearchEngine(Request.UserAgent) && (oComment != null);

                if (divComments.Visible == true)
                {
                    // generate & inject the HTML markup
                    divComments.InnerHtml = GetCommentsForSearchEngines(oComment);
                }
            }
            else
            {
                // tell the user why we cannot add the view
                if (DisqusView == "comments" && this.AttachedModule < 1)
                {
                    UI.Skins.Skin.AddModuleMessage(this, this.GetLocalizedString("NoModuleAttached.Text"), ModuleMessage.ModuleMessageType.YellowWarning);
                }

                if (DisqusView == "comments" && string.IsNullOrEmpty(DisqusApplicationName))
                {
                    UI.Skins.Skin.AddModuleMessage(this, this.GetLocalizedString("NoApplicationName.Text"), ModuleMessage.ModuleMessageType.YellowWarning);
                }

                if (DisqusView == "comments" && string.IsNullOrEmpty(DisqusApiSecret))
                {
                    UI.Skins.Skin.AddModuleMessage(this, this.GetLocalizedString("NoApiSecret.Text"), ModuleMessage.ModuleMessageType.YellowWarning);
                }
            }

            AttachModuleScript();
        }

        #endregion

        #region Script Helpers

        private string GetDisqusCommentsScript()
        {
            var sb = new StringBuilder(50);
            FeatureController ctlModule = new FeatureController();

            AppendToStringBuilder(ref sb, "<div id=\"disqus_thread\"></div> ");
            AppendToStringBuilder(ref sb, "<script type=\"text/javascript\" language=\"javascript\"> ");

            // generate SSO stub for Disqus
            if (DisqusSsoEnabled && !string.IsNullOrEmpty(DisqusSsoApiKey))
            {
                //AppendToStringBuilder(ref sb, "<script type=\"text/javascript\" language=\"javascript\"> ");
                AppendToStringBuilder(ref sb, "var disqus_config = function() { ");

                OAuthHelper auth = new OAuthHelper();
                string strMessage = GetSsoMessage();
                string strHmac = GetSsoHmac(strMessage, auth.GenerateTimeStamp());
                string strPayload = string.Format("this.page.remote_auth_s3 = '{0} {1} {2}'; ", strMessage, strHmac, auth.GenerateTimeStamp());

                AppendToStringBuilder(ref sb, strPayload);

                string strApiKey = string.Format("this.page.api_key = '{0}'; ", DisqusSsoApiKey);
                AppendToStringBuilder(ref sb, strApiKey);

                AppendToStringBuilder(ref sb, "}; ");
                //AppendToStringBuilder(ref sb, "</script> ");
            }
            
            //sb.Append("/* * * CONFIGURATION VARIABLES: EDIT BEFORE PASTING INTO YOUR WEBPAGE * * */");
            if (DisqusDeveloperMode == true)
            {
                AppendToStringBuilder(ref sb, "var disqus_developer = 1; ");
            }
            else
            {
                AppendToStringBuilder(ref sb, "var disqus_developer = 0; ");
            }
            string strShortName = string.Format("var disqus_shortname = '{0}'; ", DisqusApplicationName);
            AppendToStringBuilder(ref sb, strShortName);

            //sb.Append("// The following are highly recommended additional parameters. Remove the slashes in front to use.");
            // need to parse for isolated views in DNN and add them to this identifier
            string strIdentifier = string.Format("var disqus_identifier = '{0}'; ", ctlModule.CreateUniqueIdentifier(TabId, TabModuleId, PortalSettings.GUID, Request.RawUrl));
            AppendToStringBuilder(ref sb, strIdentifier);
            AppendToStringBuilder(ref sb, "var disqus_url = document.URL; ");

            //sb.Append("/* * * DON'T EDIT BELOW THIS LINE * * */");
            AppendToStringBuilder(ref sb, "(function() { ");
            AppendToStringBuilder(ref sb, "var dsq = document.createElement('script'); dsq.type = 'text/javascript'; dsq.async = true; ");
            AppendToStringBuilder(ref sb, "dsq.src = '//' + disqus_shortname + '.disqus.com/embed.js'; ");
            AppendToStringBuilder(ref sb, "(document.getElementsByTagName('head')[0] || document.getElementsByTagName('body')[0]).appendChild(dsq); ");
            AppendToStringBuilder(ref sb, "})();");
            AppendToStringBuilder(ref sb, "</script>");
            AppendToStringBuilder(ref sb, "<noscript>Please enable JavaScript to view the <a href=\"http://disqus.com/?ref_noscript\">comments powered by Disqus.</a></noscript>");
            AppendToStringBuilder(ref sb, "<!--<a href=\"http://disqus.com\" class=\"dsq-brlink\">blog comments powered by <span class=\"logo-disqus\">Disqus</span></a>-->");

            return sb.ToString();
        }

        private void GetDisqusCombinationScript()
        {
            var sb = new StringBuilder(20);

            /*
             EXAMPLE SCRIPT:
             * 
             * <script type="text/javascript" 
             *  src="http://wns-dev-site.disqus.com/combination_widget.js?num_items=10&hide_mods=0&color=blue&default_tab=people&excerpt_length=200"></script>
             * <a href="http://disqus.com/">Powered by Disqus</a>
             * 
             */

            AppendToStringBuilder(ref sb, "<script language=\"javascript\" type=\"text/javascript\"> ");
            if (DisqusDeveloperMode == true)
            {
                AppendToStringBuilder(ref sb, "var disqus_developer = 1; ");   
            }
            else
            {
                AppendToStringBuilder(ref sb, "var disqus_developer = 0; ");
            }
            AppendToStringBuilder(ref sb, "</script> ");

            string strModerators = (this.ShowModerators) ? strModerators = "0" : strModerators = "1";
            string strUrl =
                string.Format(
                    "<script type=\"text/javascript\" src=\"//{0}.disqus.com/combination_widget.js?num_items={1}&hide_mods={2}&color={3}&default_tab={4}&excerpt_length={5}\"></script>",
                    DisqusApplicationName, this.DisplayItems, strModerators, this.ColorTheme, this.DefaultTab,
                    this.CommentLength);

            this.AppendToStringBuilder(ref sb, strUrl);
            var strPoweredBy = string.Format("<a href=\"http://disqus.com/\">{0}</a>", this.GetLocalizedString("PoweredBy.Text"));
            this.AppendToStringBuilder(ref sb, strPoweredBy);

            this.phScript.Controls.Add(new LiteralControl(sb.ToString()));
        }

        private void GetDisqusRecentCommentsScript()
        {
            /*
             * <div id="recentcomments" class="dsq-widget">
             * <h2 class="dsq-widget-title">Recent Comments</h2>
             * <script type="text/javascript" src="http://wns-dev-site.disqus.com/recent_comments_widget.js?num_items=10&hide_avatars=0&avatar_size=32&excerpt_length=200"></script>
             * </div>
             * <a href="http://disqus.com/">Powered by Disqus</a>
             */

            var sb = new StringBuilder(20);

            AppendToStringBuilder(ref sb, "<script language=\"javascript\" type=\"text/javascript\"> ");
            if (DisqusDeveloperMode == true)
            {
                AppendToStringBuilder(ref sb, "var disqus_developer = 1; ");
            }
            else
            {
                AppendToStringBuilder(ref sb, "var disqus_developer = 0; ");
            }
            AppendToStringBuilder(ref sb, "</script> ");

            this.AppendToStringBuilder(ref sb, "<div id=\"recentcomments\" class=\"dsq-widget\">");
            var strHeader = string.Format("<h2 class=\"dsq-widget-title\">{0}</h2>", this.GetLocalizedString("RecentComments.Text"));
            this.AppendToStringBuilder(ref sb, strHeader);
            string strAvatar = (this.ShowAvatar) ? strAvatar = "1" : strAvatar="0";
            string strUrl = string.Format("<script type=\"text/javascript\" src=\"//{0}.disqus.com/recent_comments_widget.js?num_items={1}&hide_avatars={2}&avatar_size={3}&excerpt_length={4}\"></script>", DisqusApplicationName, this.DisplayItems, strAvatar, this.AvatarSize, this.CommentLength);
            this.AppendToStringBuilder(ref sb, strUrl);
            var strPoweredBy = string.Format("</div><a href=\"http://disqus.com/\">{0}</a>", this.GetLocalizedString("PoweredBy.Text"));
            this.AppendToStringBuilder(ref sb, strPoweredBy);

            this.phScript.Controls.Add(new LiteralControl(sb.ToString()));
        }

        private void GetDisqusPopularThreadScript()
        {
            /*
             * <div id="popularthreads" class="dsq-widget">
             * <h2 class="dsq-widget-title">Popular Threads</h2>
             * <script type="text/javascript" src="http://wns-dev-site.disqus.com/popular_threads_widget.js?num_items=10"></script>
             * </div><a href="http://disqus.com/">Powered by Disqus</a>
             */

            var sb = new StringBuilder(20);

            AppendToStringBuilder(ref sb, "<script language=\"javascript\" type=\"text/javascript\"> ");
            if (DisqusDeveloperMode == true)
            {
                AppendToStringBuilder(ref sb, "var disqus_developer = 1; ");
            }
            else
            {
                AppendToStringBuilder(ref sb, "var disqus_developer = 0; ");
            }
            AppendToStringBuilder(ref sb, "</script> ");

            this.AppendToStringBuilder(ref sb, "<div id=\"popularthreads\" class=\"dsq-widget\">");
            var strHeader = string.Format("<h2 class=\"dsq-widget-title\">{0}</h2>", this.GetLocalizedString("PopularThreads.Text"));
            this.AppendToStringBuilder(ref sb, strHeader);
            var strUrl = string.Format("<script type=\"text/javascript\" src=\"//{0}.disqus.com/popular_threads_widget.js?num_items={1}\"></script>", DisqusApplicationName, this.DisplayItems);
            this.AppendToStringBuilder(ref sb, strUrl);
            var strPoweredBy = string.Format("</div><a href=\"http://disqus.com/\">{0}</a>", this.GetLocalizedString("PoweredBy.Text"));
            this.AppendToStringBuilder(ref sb, strPoweredBy);

            this.phScript.Controls.Add(new LiteralControl(sb.ToString()));
        }

        private void GetDisqusTopCommentersScript()
        {
            /*
             * 
             * <h2 class="dsq-widget-title">Top Commenters</h2>
             * <script type="text/javascript" src="http://wns-dev-site.disqus.com/top_commenters_widget.js?num_items=10&hide_mods=0&hide_avatars=0&avatar_size=32"></script>
             * </div><a href="http://disqus.com/">Powered by Disqus</a>
             */

            var sb = new StringBuilder(20);

            AppendToStringBuilder(ref sb, "<script language=\"javascript\" type=\"text/javascript\"> ");
            if (DisqusDeveloperMode == true)
            {
                AppendToStringBuilder(ref sb, "var disqus_developer = 1; ");
            }
            else
            {
                AppendToStringBuilder(ref sb, "var disqus_developer = 0; ");
            }
            AppendToStringBuilder(ref sb, "</script> ");

            this.AppendToStringBuilder(ref sb, "<h2 class=\"dsq-widget-title\">Top Commenters</h2>");
            string strModerators = (this.ShowModerators) ? strModerators = "0" : strModerators = "1";
            string strAvatar = (this.ShowAvatar) ? strAvatar = "1" : strAvatar="0";
            var strUrl = string.Format("<script type=\"text/javascript\" src=\"//{0}.disqus.com/top_commenters_widget.js?num_items={1}&hide_mods={2}&hide_avatars={3}&avatar_size={4}\"></script>", DisqusApplicationName, this.DisplayItems, strModerators, strAvatar, this.AvatarSize);
            var strPoweredBy = string.Format("</div><a href=\"http://disqus.com/\">{0}</a>", this.GetLocalizedString("PoweredBy.Text"));
            this.AppendToStringBuilder(ref sb, strPoweredBy);

            this.phScript.Controls.Add(new LiteralControl(sb.ToString()));
        }

        private void AttachModuleScript()
        {
            StringBuilder sb = new StringBuilder();

            AppendToStringBuilder(ref sb, "<script type=\"text/javascript\" language=\"javascript\">");
            AppendToStringBuilder(ref sb, "(function ($, Sys) { ");
            AppendToStringBuilder(ref sb, "function setupDnnSiteSettings() { ");
            string strNewModule = string.Format("var newModule = jQuery('.DnnModule-{0}'); ", this.AttachedModule);
            AppendToStringBuilder(ref sb, strNewModule);
            AppendToStringBuilder(ref sb, "if (newModule.hasClass('DnnModule-Blog')) { ");
            AppendToStringBuilder(ref sb, "jQuery('div.BlogFooterRight a[id$=\\'_lnkComments\\']').remove();");
            AppendToStringBuilder(ref sb, "jQuery('div[id$=\\'_MainView_ViewEntry_pnlComments\\']').remove();");
            AppendToStringBuilder(ref sb, "jQuery('div[id$=\\'_MainView_ViewEntry_pnlAddComment\\']').remove();");
            AppendToStringBuilder(ref sb, "}");
            string strInsert = string.Format("jQuery('.DnnModule-{0}').insertAfter('.DnnModule-{1}'); ", this.ModuleId, this.AttachedModule);
            AppendToStringBuilder(ref sb, strInsert);
            AppendToStringBuilder(ref sb, "} ");
            AppendToStringBuilder(ref sb, "jQuery(document).ready(function (){ ");
            AppendToStringBuilder(ref sb, "setupDnnSiteSettings(); ");
            AppendToStringBuilder(ref sb, "Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () { ");
            AppendToStringBuilder(ref sb, "setupDnnSiteSettings(); ");
            AppendToStringBuilder(ref sb, "}); ");
            AppendToStringBuilder(ref sb, "}); ");
            AppendToStringBuilder(ref sb, "} (jQuery, window.Sys)); ");
            AppendToStringBuilder(ref sb, "</script>");

            this.Controls.Add(new LiteralControl(sb.ToString()));
        }

        #endregion

        #region SEO

        private string GetCommentsForSearchEngines(DisqusInfo oComment)
        {
            if (oComment == null) return string.Empty;

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < oComment.Response.Length - 1; i++)
            {

                sb.Append("<div class=\"hreview\">");
                
                sb.Append("<span class=\"item\">");
                sb.AppendFormat("<span class=\"fn\">{0}</span>", oComment.Response[i].Thread.Title);
                sb.Append("</span>");

                sb.AppendFormat("<span class=\"reviewer\">{0}</span>", oComment.Response[i].Author.Name);

                sb.Append("<span class=\"dtreviewed\">");
                sb.Append(oComment.Response[i].CreatedAt.ToString("MMM dd, yyyy"));
                sb.AppendFormat("<span class=\"value-title\" title=\"{0}\"></span>", oComment.Response[i].CreatedAt.ToString("yyyy-MM-dd"));
                sb.Append("</span>");

                sb.AppendFormat("<span class=\"summary\">{0}</span>", oComment.Response[i].RawMessage.Substring(0, 50));

                sb.AppendFormat("<span class=\"description\">{0}</span>", oComment.Response[i].Message);

                // no way to really implement this since we aren't really posting reviews
                //sb.AppendFormat("<span class=\"rating\">{0}</span>", );

                sb.Append("</div>");

            }

            return sb.ToString();
        }

        #endregion

        #region Single Sign-On Helpers

        private string GetSsoMessage()
        {
            string strUserPrefix = PortalSettings.PortalAlias.HTTPAlias.Replace(".", string.Empty);
            strUserPrefix = strUserPrefix.Replace("/", string.Empty);
            string strUser = string.Concat(strUserPrefix, UserId);

            StringBuilder sb = new StringBuilder();

            sb.Append("{ ");
            sb.AppendFormat("\"id\": \"{0}\", ", strUser);
            sb.AppendFormat("\"username\": \"{0}\", ", UserInfo.DisplayName);
            sb.AppendFormat("\"email\": \"{0}\"", UserInfo.Email);

            //OAuthHelper auth = new OAuthHelper();

            if (!string.IsNullOrEmpty(UserInfo.Profile.Photo))
            {
                string avatar = GetFileUrl(UserInfo.Profile.Photo);
                sb.AppendFormat(", \"avatar\": \"{0}\"", ResolveUrl(avatar));
            }

            if (!string.IsNullOrEmpty(UserInfo.Profile.Website))
            {
                sb.AppendFormat(", \"url\": \"{0}\"", UserInfo.Profile.Website);
            }

            sb.Append(" }");

            return Encode(sb.ToString());
        }

        private string GetSsoHmac(string message, string timestamp)
        {
            byte[] byteKey = Encoding.ASCII.GetBytes(DisqusApiSecret);
            byte[] byteMessage = Encoding.ASCII.GetBytes(string.Concat(message, " ", timestamp));

            HMACSHA1 hmacsha1 = new HMACSHA1(byteKey);
            MemoryStream stream = new MemoryStream(byteMessage);

            return hmacsha1.ComputeHash(stream).Aggregate(string.Empty, (s, e) => s + String.Format("{0:x2}", e), s => s);
        }

        private string Encode(string message)
        {
            byte[] encbuff = Encoding.UTF8.GetBytes(message);
            return Convert.ToBase64String(encbuff);
        }

        private string GetFileUrl(string fileUrl)
        {
            if (Regex.IsMatch(fileUrl, "^\\d+$"))
            {
                IFileInfo file = FileManager.Instance.GetFile(int.Parse(fileUrl, NumberStyles.Integer));
                FolderMappingInfo mapFolder = FolderMappingController.Instance.GetFolderMapping(file.FolderMappingID);
                return FolderProvider.Instance(mapFolder.FolderProviderType).GetFileUrl(file);
            }

            return fileUrl;
        }

        private string ResolveUrl(string path)
        {
            return string.Format("://{0}{1}", PortalSettings.PortalAlias.HTTPAlias, path);
        }

        #endregion

        private void AppendToStringBuilder(ref StringBuilder sb, string strValue)
        {
#if DEBUG
            sb.Append(Environment.NewLine);
            sb.Append(strValue);
#else
            sb.Append(strValue);
#endif
        }

        #endregion

        #region IActionable Implementation

        /// <summary>
        /// ModuleActions - loads the actions menu with a custom menu item
        /// </summary>
        public ModuleActionCollection ModuleActions
        {
            get
            {
                ModuleActionCollection Actions = new ModuleActionCollection();

                Actions.Add(GetNextActionID(), Localization.GetString(c_AboutMe, LocalResourceFile), string.Empty, string.Empty, string.Empty, EditUrl(string.Empty, string.Empty, c_AboutMe), false, SecurityAccessLevel.Edit, true, false);

                Actions.Add(GetNextActionID(), Localization.GetString(c_EditModule, LocalResourceFile), string.Empty, string.Empty, string.Empty, EditUrl(), false, SecurityAccessLevel.Edit, true, false);
                return Actions;
            }
        }

        #endregion

    }

}
