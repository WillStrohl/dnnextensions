//
// Will Strohl (will.strohl@gmail.com)
// http://www.willstrohl.com
//
//Copyright (c) 2009-2015, Will Strohl
//All rights reserved.
//
//Redistribution and use in source and binary forms, with or without modification, are 
//permitted provided that the following conditions are met:
//
//Redistributions of source code must retain the above copyright notice, this list of 
//conditions and the following disclaimer.
//
//Redistributions in binary form must reproduce the above copyright notice, this list 
//of conditions and the following disclaimer in the documentation and/or other 
//materials provided with the distribution.
//
//Neither the name of Will Strohl, Content Injection, nor the names of its contributors may be 
//used to endorse or promote products derived from this software without specific prior 
//written permission.
//
//THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY 
//EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES 
//OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT 
//SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, 
//INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED 
//TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR 
//BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN 
//CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN 
//ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH 
//DAMAGE.
//

using DotNetNuke.Services.Exceptions;
using System;
using DotNetNuke.Entities.Modules;
using DotNetNuke.UI.Skins.Controls;
using System.Web.UI;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules.Actions;
using DotNetNuke.UI.Skins;
using DotNetNuke.Web.Client;
using DotNetNuke.Web.Client.ClientResourceManagement;
using DotNetNuke.Web.Client.Providers;
using WillStrohl.Modules.Injection.Components;
using WillStrohl.Modules.Injection.Entities;

namespace WillStrohl.Modules.Injection
{
    public partial class ViewInjection : WNSPortalModuleBase, IActionable
    {
        #region Private Members

        private string p_Header = string.Empty;
        private string p_Footer = string.Empty;
        private string p_EditInjectionUrl = string.Empty;

        #endregion

        #region Properties

        private string HeaderInjection
        {
            get { return p_Header; }
        }

        private string FooterInjection
        {
            get { return p_Footer; }
        }

        private string EditInjectionUrl
        {
            get
            {
                if (!string.IsNullOrEmpty(p_EditInjectionUrl))
                {
                    return p_EditInjectionUrl;
                }

                p_EditInjectionUrl = EditUrl(string.Empty, string.Empty, "Edit");

                return p_EditInjectionUrl;
            }
        }

        #endregion

        #region Event Handlers

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (CurrentUserCanEdit)
                {
                    // If IsEditable, then the visitor has edit permissions to the module, is 
                    // currently logged in, and the portal is in edit mode.
                    Skin.AddModuleMessage(this, GetLocalizedString("InjectionInfo.Text"), ModuleMessage.ModuleMessageType.BlueInfo);
                }
                else
                {
                    // hide the module container (and the rest of the module as well)
                    ContainerControl.Visible = false;
                }

                // inject any strings insto the page
                ExecutePageInjection();
            }
            catch (Exception exc)
            {
                // Module failed to load
                Exceptions.ProcessModuleLoadException(this, exc, IsEditable);
            }
        }

        public void InjectIntoFooter(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(FooterInjection))
            {
                Page.Form.Controls.Add(new LiteralControl(FooterInjection));
            }
        }

        #endregion

        #region Private Helper Methods

        private void ExecutePageInjection()
        {
            var ctlModule = new InjectionController();
            var collInj = new InjectionInfoCollection();

            collInj = ctlModule.GetActiveInjectionContents(ModuleId);

            if (collInj.Count <= 0) return;

            p_Header = string.Format("<!-- {0} -->", GetLocalizedString("HeaderHeader"));
            p_Footer = string.Format("<!-- {0} -->", GetLocalizedString("FooterHeader"));

            foreach (var injection in collInj)
            {
                var injectionType = InjectionController.GetInjectionType(injection);
                var priority = InjectionController.GetCrmPriority(injection);
                var provider = InjectionController.GetCrmProvider(injection);

                switch (injectionType)
                {
                    case InjectionType.CSS:
                        RegisterStyleSheet(injection.InjectContent, priority, provider);
                        break;
                    case InjectionType.JavaScript:
                        RegisterScript(injection.InjectContent, priority, provider);
                        break;
                    case InjectionType.HtmlBottom:
                        p_Footer = string.Concat(p_Footer, Server.HtmlDecode(injection.InjectContent));
                        break;
                    case InjectionType.HtmlTop:
                        p_Header = string.Concat(p_Header, Server.HtmlDecode(injection.InjectContent));
                        break;
                }
            }

            p_Header = string.Concat(p_Header, string.Format("<!-- {0} -->", GetLocalizedString("HeaderFooter")));
            p_Footer = string.Concat(p_Footer, string.Format("<!-- {0} -->", GetLocalizedString("FooterFooter")));

            // add the injection content to the header
            if (!string.IsNullOrEmpty(HeaderInjection))
            {
                Parent.Page.Header.Controls.Add(new LiteralControl(HeaderInjection));
            }

            // add the injection content to the footer
            if (!string.IsNullOrEmpty(FooterInjection))
            {
                Page.LoadComplete += new EventHandler(InjectIntoFooter);
            }
        }

        private void RegisterStyleSheet(string path, int priority, string provider)
        {
            if (priority == Null.NullInteger)
            {
                priority = (int)FileOrder.Css.DefaultPriority;
            }

            ClientResourceManager.RegisterStyleSheet(this.Page, path, priority, provider);
        }

        private void RegisterScript(string path, int priority, string provider)
        {
            if (priority == Null.NullInteger)
            {
                priority = (int)FileOrder.Js.DefaultPriority;
            }

            ClientResourceManager.RegisterScript(this.Page, path, priority, provider);
        }

        #endregion

        #region IActionable Implementation

        public ModuleActionCollection ModuleActions
        {
            get
            {
                var Actions = new ModuleActionCollection();
                Actions.Add(GetNextActionID(), GetLocalizedString("EditInjection.MenuItem.Title"), string.Empty, string.Empty, string.Empty, EditInjectionUrl, false, DotNetNuke.Security.SecurityAccessLevel.Edit, true, false);
                return Actions;
            }
        }

        #endregion
    }
}