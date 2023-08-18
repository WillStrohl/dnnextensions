/*
Copyright © Upendo Ventures, LLC

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