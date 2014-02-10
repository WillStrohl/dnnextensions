//
// DNN Corp - http://www.dnnsoftware.com
// Copyright (c) 2002-2014
// by DNN Corp
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

using DotNetNuke.Services.Exceptions;
using System;
using System.Collections.Generic;
using DNNSkins = DotNetNuke.UI.Skins;
using DotNetNuke.UI.Skins.Controls;
using System.Web;

namespace DotNetNuke.Modules.Media
{

    /// <summary>
    /// The MediaModule Class provides the UI for displaying the Media
    /// </summary>
    /// <remarks>
    /// </remarks>
    public partial class MediaModule : MediaModuleBase, DotNetNuke.Entities.Modules.IActionable
    {

        #region  Constants

        private const string MEDIA_WRAPPER_TAG = "<div class=\"dnnMedia-Wrapper\">{0}</div>";
        private const string NOMEDIA_TAG = "<div class=\"dnnFormMessage dnnFormWarning\">{0}</div>";
        private const string MESSAGE_TAG = "<div id=\"divMediaFooter-{0}\" class=\"dnnmedia-message dnnClear\">{1}</div>";

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
            this.Load += new System.EventHandler(Page_Load);
        }

        /// <summary>
        /// Page_Load runs when the control is loaded
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <history>
        /// 	[lpointer]	31.10.2005  Read from Business Layer
        /// 	[lpointer]	17.10.2006  Updated MediaSrc variable so it reads as lower case variable for Case check.
        /// </history>
        private void Page_Load(object sender, EventArgs e)
        {
            try
            {
                DotNetNuke.Framework.jQuery.RequestRegistration();

                BindData();
            }
            catch (Exception exc) //Module failed to load
            {
                Exceptions.ProcessModuleLoadException(this, exc, UserInfo.IsSuperUser);
            }

        }

        #endregion

        #region Private Methods

        private void BindData()
        {
            MediaController ctlMedia = new MediaController();
            List<string> lstMedia = ctlMedia.DisplayMedia(ModuleId, TabId, IsEditable, ModuleConfiguration, PortalSettings);

            if (string.IsNullOrEmpty(lstMedia[0]))
            {
                if (IsEditable)
                {
                    // there is no media yet
                    DNNSkins.Skin.AddModuleMessage(this, GetLocalizedString("NoMediaMessage.Text"),
                                                   ModuleMessage.ModuleMessageType.BlueInfo);
                }
                else
                {
                    // hide the module
                    ContainerControl.Visible = false;
                }
                return;
            }

            if (!string.IsNullOrEmpty(lstMedia[2]))
            {
                // there's an error returned
                DNNSkins.Skin.AddModuleMessage(this, GetLocalizedString(lstMedia[1]), ModuleMessage.ModuleMessageType.YellowWarning);
            }
            else
            {
                // there's media to display
                MediaLiteral.Text = string.Format(MEDIA_WRAPPER_TAG, lstMedia[0]);

                if (!string.IsNullOrEmpty(lstMedia[1]))
                {
                    MessageLiteral.Text = string.Format(MESSAGE_TAG, TabModuleId, HttpUtility.HtmlDecode(lstMedia[1]));
                }
            }
        }

        #endregion

        #region  Optional Interfaces

        public DotNetNuke.Entities.Modules.Actions.ModuleActionCollection ModuleActions
        {
            get
            {
                DotNetNuke.Entities.Modules.Actions.ModuleActionCollection Actions = new DotNetNuke.Entities.Modules.Actions.ModuleActionCollection();
                Actions.Add(GetNextActionID(), this.GetLocalizedString(DotNetNuke.Entities.Modules.Actions.ModuleActionType.AddContent), DotNetNuke.Entities.Modules.Actions.ModuleActionType.AddContent, string.Empty, string.Empty, EditUrl(), false, DotNetNuke.Security.SecurityAccessLevel.Edit, true, false);
                return Actions;
            }
        }

        #endregion

    }

}