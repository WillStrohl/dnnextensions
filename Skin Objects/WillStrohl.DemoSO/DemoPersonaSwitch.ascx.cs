/*
 * Copyright (c) 2013, Will Strohl
 * All rights reserved.
 * 
 * Redistribution and use in source and binary forms, with or without modification, 
 * are permitted provided that the following conditions are met:
 * 
 * - Redistributions of source code must retain the above copyright notice, this 
 *   list of conditions and the following disclaimer.
 * - Redistributions in binary form must reproduce the above copyright notice, 
 *   this list of conditions and the following disclaimer in the documentation 
 *   and/or other materials provided with the distribution.
 * 
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND 
 * ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED 
 * WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. 
 * IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, 
 * INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, 
 * BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, 
 * DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF 
 * LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE 
 * OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED 
 * OF THE POSSIBILITY OF SUCH DAMAGE.
*/

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using DotNetNuke.Common;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Users;
using DotNetNuke.Security;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Log.EventLog;
using DotNetNuke.Services.Tokens;
using DotNetNuke.Web.Client.ClientResourceManagement;

namespace WillStrohl.SkinObjects.DemoSO
{
    /// -----------------------------------------------------------------------------
    /// <summary>
    /// 
    /// </summary>
    /// -----------------------------------------------------------------------------
    public partial class DemoPersonaSwitch : DemoSkinObjectModuleBase
    {

        #region Private Members

        private UserInfo _User = null;
        private bool _SecurityRoleSet = false;
        private bool _IncludeSuperusers = false;

        #endregion

        #region Properties

        protected UserInfo User {
            get
            {
                if (_User == null)
                {
                    var user = UserController.GetCurrentUserInfo();
                    if (user != null && user.UserID > -1) _User = user;
                }

                return _User;
            }
        }

        public string SecurityRoles { get; set; }

        public bool IncludeSuperusers {
            get 
            { 
                return _IncludeSuperusers;
            }
            set
            {
                _IncludeSuperusers = value;
            }
        }

        private int UserId { get; set; }

        #endregion

        #region Event Handlers

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                //
                // handle the login/logoff procedure
                //
                UserSwitch();

                // generate the demo persona
                if (!Page.IsPostBack && User != null) BindData();

                IncludeResources();
            }
            catch (Exception exc) //Module failed to load
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
            finally
            {
                ClearCookies();
            }
        }

        #endregion

        #region Helper Methods

        private void IncludeResources()
        {
            // use the API to properly include the stylesheet
            ClientResourceManager.RegisterStyleSheet(Page,
                                                     string.Concat(ControlPath, "Styles/DemoPersonaSwitch.css"));

            // work around for a HTTP 301 redirect issue on homepages in DNN 07.01.00
            // https://dnntracker.atlassian.net/browse/CONTENT-1561
            ClientResourceManager.RegisterScript(Page,
                                                 string.Concat(ControlPath, "Includes/jqueryCookie/jquery.cookie.js"));
        }

        private void BindData()
        {
            // we are expecting one or more security roles specified and delimited by commas, but...
            // set a default security role, if one or more aren't specified
            if (string.IsNullOrEmpty(SecurityRoles))
            {
                SecurityRoles = "Registered Users";
                _SecurityRoleSet = false;
            }
            else
            {
                _SecurityRoleSet = true;
            }

            // grab the users 
            var superusers = from u in UserController.GetUsers(false, true, -1).Cast<UserInfo>().ToList()
                             select u;
            var users = from u in UserController.GetUsers(PortalSettings.PortalId).Cast<UserInfo>().ToList()
                        where u.IsDeleted == false && u.Roles.Any(s => s == SecurityRoles)
                        select u;
            if (!_SecurityRoleSet || IncludeSuperusers) users = users.Concat(superusers);

            var user = UserController.GetCurrentUserInfo();
            UserId = user.UserID;
            var tok = new TokenReplace(Scope.DefaultSettings, user.Profile.PreferredLocale, PortalSettings, user);
            var template = GetLocalizedString("Persona.Template", FeatureController.RESOURCEFILE_PERSONA_SWITCH);

            // build the persona switch content to use for replacement
            var personaSwitchList = GeneratePersonaMarkup(users);

            // replace the switch token
            template = template.Replace(FeatureController.TOKEN_SWITCH, personaSwitchList);

            // perform core token replace
            template = tok.ReplaceEnvironmentTokens(template);
            
            // add the html to the view
            phTemplate.Controls.Add(new LiteralControl(template));

            // inject jquery for the background images
            litScript.Text = GenerateScriptMarkup(tok, users);
        }

        private string GeneratePersonaMarkup(IEnumerable<UserInfo> users)
        {
            var userTemplate = GetLocalizedString("Head.Template", FeatureController.RESOURCEFILE_PERSONA_SWITCH);
            var sb = new StringBuilder();

            foreach (UserInfo user in users.OrderBy(x => x.DisplayName))
            {
                // perform core token replace
                if (user.UserID != UserId)
                {
                    var userToken = new TokenReplace(Scope.DefaultSettings, user.Profile.PreferredLocale, PortalSettings, user);
                    sb.Append(userToken.ReplaceEnvironmentTokens(userTemplate));
                }
            }

            return sb.ToString();
        }
        
        private string GenerateScriptMarkup(TokenReplace tok, IEnumerable<UserInfo> users)
        {
            var scriptTemplate = GetLocalizedString("Script.Template", FeatureController.RESOURCEFILE_PERSONA_SWITCH);
            var userScriptTemplate = GetLocalizedString("ScriptItem.Template", FeatureController.RESOURCEFILE_PERSONA_SWITCH);
            var sbUserScript = new StringBuilder();
            var sec = new PortalSecurity();

            // create the user avatar listing
            foreach (UserInfo user in users)
            {
                /*
                 * $('.dpc[User:UserId]')
                 *      .css('background', 'url([Profile:Photo]) no-repeat')
                 *      .css('background-position', 'center center')
                 *      .attr('title', '[User:DisplayName]')
                 *      .click(function(){ window.location = '[DemoPersona:Login]'; })
                 *      .hover(function (){ $(this).css('opacity', '1.0'); }, function (){ $(this).css('opacity', '0.5'); }); 
                 */
                if (user.UserID != UserId)
                {
                    var userKeyForCookie = sec.EncryptString(user.UserID.ToString(), PortalSettings.GUID.ToString());
                    var userKeyForUrl = HttpUtility.UrlEncode(userKeyForCookie);
                    var newUrl = Globals.NavigateURL(PortalSettings.ActiveTab.TabID, 
                        string.Empty, 
                        string.Concat(FeatureController.QS_LOGINID, "=", userKeyForUrl));

                    // executing this line of code breaks the JS, removing the BG images
                    var alteredTemplate = userScriptTemplate.Replace(FeatureController.TOKEN_LOGIN, newUrl);

                    // work around for a HTTP 301 redirect issue on homepages in DNN 07.01.00
                    // https://dnntracker.atlassian.net/browse/CONTENT-1561
                    alteredTemplate = alteredTemplate.Replace(FeatureController.TOKEN_COOKIE_NAME, FeatureController.QS_LOGINID);
                    alteredTemplate = alteredTemplate.Replace(FeatureController.TOKEN_COOKIE_VALUE, userKeyForCookie);

                    var userToken = new TokenReplace(Scope.DefaultSettings, user.Profile.PreferredLocale, PortalSettings, user);
                    alteredTemplate = userToken.ReplaceEnvironmentTokens(alteredTemplate);
                    sbUserScript.Append(alteredTemplate);
                }
            }

            // insert the persona scripts
            scriptTemplate = scriptTemplate.Replace(FeatureController.TOKEN_SCRIPT, sbUserScript.ToString());

            // perform core token replace
            scriptTemplate = tok.ReplaceEnvironmentTokens(scriptTemplate);

            return scriptTemplate;
        }

        private void UserSwitch()
        {
            if (Request.QueryString[FeatureController.QS_LOGINID] != null ||
                    Request.Cookies[FeatureController.QS_LOGINID] != null)
            {
                var loginid = string.Empty;
                if (Request.QueryString[FeatureController.QS_LOGINID] != null &&
                    PortalSettings.ActiveTab.TabID != PortalSettings.HomeTabId)
                {
                    // run this if not on the homepage
                    var qsValue = Request.QueryString[FeatureController.QS_LOGINID];
                    loginid = HttpUtility.UrlDecode(qsValue);
                    // necessary because UrlDecode doesn't properly translate the decoded string
                    loginid = loginid.Replace(" ", "+");
                }
                else
                {
                    // work around for a HTTP 301 redirect issue on homepages in DNN 07.01.00
                    // https://dnntracker.atlassian.net/browse/CONTENT-1561
                    loginid = Request.Cookies[FeatureController.QS_LOGINID].Value;
                }

                if (string.IsNullOrEmpty(loginid) == false)
                {
                    var sec = new PortalSecurity();

                    // decrypt the id
                    var userId = sec.DecryptString(loginid, PortalSettings.GUID.ToString());
                    int newUserId = int.Parse(userId, NumberStyles.Integer);

                    ProcessLogin(newUserId);
                }
            }
        }

        private void ProcessLogin(int newUserId)
        {
            var currentUser = UserController.GetCurrentUserInfo();

            //Log event
            var objEventLog = new EventLogController();
            objEventLog.AddLog("Username", currentUser.Username, PortalSettings, currentUser.UserID, EventLogController.EventLogType.USER_IMPERSONATED);

            //Remove user from cache
            DataCache.ClearUserCache(PortalSettings.PortalId, currentUser.Username);

            var objPortalSecurity = new PortalSecurity();
            objPortalSecurity.SignOut();

            var ctlUser = new UserController();
            var newUser = ctlUser.GetUser(PortalSettings.PortalId, newUserId);

            UserController.UserLogin(newUser.PortalID, newUser, PortalSettings.PortalName, HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"], false);

            ClearCookies();

            Response.Redirect(DotNetNuke.Common.Globals.NavigateURL(PortalSettings.ActiveTab.TabID));
        }

        private void ClearCookies()
        {
            // work around for a HTTP 301 redirect issue on homepages in DNN 07.01.00
            // https://dnntracker.atlassian.net/browse/CONTENT-1561
            // clean-up by removing the cookie, if it exists
            if (Request.Cookies[FeatureController.QS_LOGINID] != null)
            {
                Response.Cookies[FeatureController.QS_LOGINID].Expires = DateTime.Now.AddDays(-1);
            }
        }
        
        #endregion

    }
}