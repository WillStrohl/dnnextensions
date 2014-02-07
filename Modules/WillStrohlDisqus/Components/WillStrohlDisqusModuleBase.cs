/*
 * Copyright (c) 2011-2012, Will Strohl
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
using DotNetNuke.Entities.Controllers;
using DotNetNuke.Entities.Portals;
using DotNetNuke.Modules.WillStrohlDisqus.Components;
using DotNetNuke.Services.Localization;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Scheduling;

namespace DotNetNuke.Modules.WillStrohlDisqus
{

    /// <summary>
    /// A base class for all module views to use
    /// </summary>
    public class WillStrohlDisqusModuleBase : Entities.Modules.PortalModuleBase
    {

        #region Private Members

        private string p_DisqusApplicationName = string.Empty;
        private string p_DisqusApiSecret = string.Empty;
        private bool p_RequireDnnLogin = false;
        private bool p_ScheduleEnabled = false;
        private bool p_DisqusDeveloperMode = false;
        private bool p_DisqusSsoEnabled = false;
        private string p_DisqusSsoApiKey = string.Empty;

        #endregion

        #region Properties

        /// <summary>
        /// DisqusApplicationName
        /// </summary>
        /// <returns>The name of the Disqus Application that was created on Disqus.com</returns>
        protected string DisqusApplicationName
        {
            get
            {
                // check to see if there is a module setting first
                if (this.Settings["ApplicationName"] != null)
                {
                    // converting from using module settings to site settings

                    // get the module setting value
                    p_DisqusApplicationName = this.Settings["ApplicationName"].ToString();
                    
                    // copy the module setting to the site settings
                    PortalController.UpdatePortalSetting(PortalId, "wnsDisqusApplicationName", p_DisqusApplicationName, true, PortalSettings.CultureCode);

                    // delete the module setting
                    var ctlModule = new Entities.Modules.ModuleController();
                    ctlModule.DeleteModuleSetting(ModuleId, "ApplicationName");
                }
                else
                {
                    string strAppName = PortalController.GetPortalSetting("ApplicationName", PortalId, string.Empty);
                    if (string.IsNullOrEmpty(strAppName) == false)
                    {
                        PortalController.UpdatePortalSetting(PortalId, "wnsDisqusApplicationName", strAppName, true, PortalSettings.CultureCode);
                        PortalController.DeletePortalSetting(PortalId, "ApplicationName");
                    }
                    p_DisqusApplicationName = PortalController.GetPortalSetting("wnsDisqusApplicationName", PortalId, string.Empty);
                }

                return p_DisqusApplicationName;
            }
            set { p_DisqusApplicationName = value; }
        }

        /// <summary>
        /// DisqusApiSecret
        /// </summary>
        /// <returns>The API secret for the Disqus Application that was created on Disqus.com</returns>
        protected string DisqusApiSecret
        {
            get
            {
                // check to see if there is a module setting first
                if (this.Settings["ApiSecret"] != null)
                {
                    // converting from using module settings to site settings

                    // get the module setting value
                    p_DisqusApiSecret = this.Settings["ApiSecret"].ToString();

                    // copy the module setting to the site settings
                    PortalController.UpdatePortalSetting(PortalId, "wnsDisqusApiSecret", p_DisqusApiSecret, true, PortalSettings.CultureCode);

                    // delete the module setting
                    var ctlModule = new Entities.Modules.ModuleController();
                    ctlModule.DeleteModuleSetting(ModuleId, "ApiSecret");
                }
                else
                {
                    string strApiKey = PortalController.GetPortalSetting("ApiSecret", PortalId, string.Empty);
                    if (string.IsNullOrEmpty(strApiKey) == false)
                    {
                        PortalController.UpdatePortalSetting(PortalId, "wnsDisqusApiSecret", strApiKey, true, PortalSettings.CultureCode);
                        PortalController.DeletePortalSetting(PortalId, "ApiSecret");
                    }
                    p_DisqusApiSecret = PortalController.GetPortalSetting("wnsDisqusApiSecret", PortalId, string.Empty);
                }

                return p_DisqusApiSecret;
            }
            set { p_DisqusApiSecret = value; }
        }

        /// <summary>
        /// RequireDnnLogin
        /// </summary>
        protected bool RequireDnnLogin
        {
            get
            {
                try
                {
                    string strDnnLogin = PortalController.GetPortalSetting("RequireDnnLogin", PortalId, string.Empty);
                    if (string.IsNullOrEmpty(strDnnLogin) == false)
                    {
                        PortalController.UpdatePortalSetting(PortalId, "wnsDisqusRequireDnnLogin", strDnnLogin, true, PortalSettings.CultureCode);
                        PortalController.DeletePortalSetting(PortalId, "RequireDnnLogin");
                    }
                    p_RequireDnnLogin = bool.Parse(PortalController.GetPortalSetting("wnsDisqusRequireDnnLogin", PortalId, string.Empty));
                }
                catch
                {
                    // the setting doesn't exist yet
                    p_RequireDnnLogin = false;
                }

                return p_RequireDnnLogin;
            }
            set { p_RequireDnnLogin = value; }
        }

        /// <summary>
        /// ScheduleEnabled
        /// </summary>
        protected bool ScheduleEnabled
        {
            get
            {
                try
                {
                    p_ScheduleEnabled = Convert.ToBoolean(HostController.Instance.GetString(FeatureController.HOST_SETTING_COMMENT_SCHEDULE, "false"));
                    ScheduleItem oSchedule = SchedulingProvider.Instance().GetSchedule(FeatureController.DISQUS_COMMENT_SCHEDULE_NAME, string.Empty);
                    if (oSchedule != null)
                    {
                        p_ScheduleEnabled = oSchedule.Enabled;
                    }
                    else
                    {
                        p_ScheduleEnabled = false;
                    }
                }
                catch(Exception ex)
                {
                    Exceptions.LogException(ex);
                    // the setting doesn't exist yet
                    p_ScheduleEnabled = false;
                }

                return p_ScheduleEnabled;
            }
            set { p_ScheduleEnabled = value; }
        }

        /// <summary>
        /// DisqusDeveloperMode
        /// </summary>
        protected bool DisqusDeveloperMode
        {
            get
            {
                try
                {
                    string strDeveloperMode = PortalController.GetPortalSetting("wnsDisqusDeveloperMode", PortalId, "false");
                    p_DisqusDeveloperMode = bool.Parse(strDeveloperMode);
                }
                catch
                {
                    // the setting doesn't exist yet
                    p_DisqusDeveloperMode = false;
                }

                return p_DisqusDeveloperMode;
            }
            set { p_DisqusDeveloperMode = value; }
        }

        /// <summary>
        /// DisqusSsoEnabled
        /// </summary>
        protected bool DisqusSsoEnabled
        {
            get
            {
                try
                {
                    string strSsoEnabled = PortalController.GetPortalSetting("wnsDisqusSsoEnabled", PortalId, "false");
                    p_DisqusSsoEnabled = bool.Parse(strSsoEnabled);
                }
                catch
                {
                    // the setting doesn't exist yet
                    p_DisqusSsoEnabled = false;
                }

                return p_DisqusSsoEnabled;
            }
            set { p_DisqusSsoEnabled = value; }
        }

        /// <summary>
        /// DisqusSsoApiKey
        /// </summary>
        protected string DisqusSsoApiKey
        {
            get
            {
                try
                {
                    p_DisqusSsoApiKey = PortalController.GetPortalSetting("wnsDisqusSsoApiKey", PortalId, string.Empty);
                }
                catch
                {
                    // the setting doesn't exist yet
                    p_DisqusSsoApiKey = string.Empty;
                }

                return p_DisqusSsoApiKey;
            }
            set { p_DisqusSsoApiKey = value; }
        }

        #endregion

        #region Localization

        /// <summary>
        /// GetLocalizedString - A shortcut to localizing a string object
        /// </summary>
        /// <param name="localizationKey">a unique string key representing the localization value</param>
        /// <returns></returns>
        protected string GetLocalizedString(string localizationKey)
        {
            if (!string.IsNullOrEmpty(localizationKey))
            {
                return Localization.GetString(localizationKey, this.LocalResourceFile);
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// GetLocalizedString - A shortcut to localizing a string object
        /// </summary>
        /// <param name="localizationKey">a unique string key representing the localization value</param>
        /// <param name="localResourceFilePath">the path to the localization file</param>
        /// <returns></returns>
        protected string GetLocalizedString(string localizationKey, string localResourceFilePath)
        {
            if (!string.IsNullOrEmpty(localizationKey))
            {
                return Localization.GetString(localizationKey, localResourceFilePath);
            }
            else
            {
                return string.Empty;
            }
        }

        #endregion

        #region Event Handlers

        private void InitializeComponent()
        {
            this.Load += new System.EventHandler(this.PageLoad);
        }

        private void PageLoad(object sender, System.EventArgs e)
        {
            // request that the DNN framework load the jQuery script into the markup
            DotNetNuke.Framework.jQuery.RequestDnnPluginsRegistration();
        }

        #endregion

    }

}
