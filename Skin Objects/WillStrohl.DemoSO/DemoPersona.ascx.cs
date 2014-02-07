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
using System.Web.UI;
using DotNetNuke.Entities.Users;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Tokens;
using DotNetNuke.Web.Client.ClientResourceManagement;

namespace WillStrohl.SkinObjects.DemoSO
{
    /// -----------------------------------------------------------------------------
    /// <summary>
    /// 
    /// </summary>
    /// -----------------------------------------------------------------------------
    public partial class DemoPersona : DemoSkinObjectModuleBase
    {

        #region Private Members

        private UserInfo _User = null;

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

        #endregion

        #region Event Handlers

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack && User != null) BindData();

                // user the API to properly include the stylesheet
                ClientResourceManager.RegisterStyleSheet(Page, string.Concat(ControlPath, "Styles/DemoPersona.css"));
            }
            catch (Exception exc) //Module failed to load
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        #endregion

        #region Helper Methods

        private void BindData()
        {
            var user = UserController.GetCurrentUserInfo();
            var tok = new TokenReplace(Scope.DefaultSettings, user.Profile.PreferredLocale, PortalSettings, user);

            var template = tok.ReplaceEnvironmentTokens(GetLocalizedString("Persona.Template", FeatureController.RESOURCEFILE_PERSONA));

            phTemplate.Controls.Add(new LiteralControl(template));
        }

        #endregion

    }
}