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
using WillStrohl.Modules.Injection.Components;
using WillStrohl.Modules.Injection.Entities;

namespace WillStrohl.Modules.Injection
{
	public abstract partial class ViewInjection : WNSPortalModuleBase, IActionable
	{
		#region " Private Members "

		private string p_Header = string.Empty;
		private string p_Footer = string.Empty;

		private string p_EditInjectionUrl = string.Empty;

		#endregion

		#region " Properties "

		private string HeaderInjection {
			get { return this.p_Header; }
		}

		private string FooterInjection {
			get { return this.p_Footer; }
		}

		private string EditInjectionUrl {
			get {
				if (!string.IsNullOrEmpty(this.p_EditInjectionUrl)) {
					return this.p_EditInjectionUrl;
				}

				this.p_EditInjectionUrl = EditUrl(string.Empty, string.Empty, "Edit");

				return this.p_EditInjectionUrl;
			}
		}

		#endregion

        #region " Event Handlers "

        protected ViewInjection()
        {
            Load += Page_Load;
        }

		protected void Page_Load(object sender, EventArgs e)
		{
			try {
				if (this.IsEditable && this.PortalSettings.UserMode == DotNetNuke.Entities.Portals.PortalSettings.Mode.Edit) {
					// If IsEditable, then the visitor has edit permissions to the module, is 
					// currently logged in, and the portal is in edit mode.
					DotNetNuke.UI.Skins.Skin.AddModuleMessage(this, this.GetLocalizedString("InjectionInfo.Text"), ModuleMessage.ModuleMessageType.BlueInfo);
				} else {
					// hide the module container (and the rest of the module as well)
					this.ContainerControl.Visible = false;
				}

				// inject any strings insto the page
				this.ExecutePageInjection();

			// Module failed to load
			} catch (Exception exc) {
				Exceptions.ProcessModuleLoadException(this, exc, this.IsEditable);
			}
		}

		public void InjectIntoFooter(object sender, EventArgs e)
		{
			if (!string.IsNullOrEmpty(FooterInjection)) {
				this.Page.Form.Controls.Add(new LiteralControl(FooterInjection));
			}
		}

		#endregion

		#region " Private Helper Methods "


		private void ExecutePageInjection()
		{
			InjectionController ctlModule = new InjectionController();
			InjectionInfoCollection collInj = new InjectionInfoCollection();
			collInj = ctlModule.GetActiveInjectionContents(this.ModuleId);


			if (collInj.Count > 0) {
				foreach (InjectionInfo objInj in collInj) {
					if (objInj.InjectTop) {
						this.p_Header = string.Concat(this.p_Header, Server.HtmlDecode(objInj.InjectContent));
					} else {
						this.p_Footer = string.Concat(this.p_Footer, Server.HtmlDecode(objInj.InjectContent));
					}
				}

				// add the injection content to the header
				if (!string.IsNullOrEmpty(HeaderInjection)) {
					this.Parent.Page.Header.Controls.Add(new LiteralControl(HeaderInjection));
				}

				// add the injection content to the footer
				if (!string.IsNullOrEmpty(FooterInjection)) {
					this.Page.LoadComplete += new EventHandler(InjectIntoFooter);
				}

			}

		}

		#endregion

		#region " IActionable Implementation "

		public DotNetNuke.Entities.Modules.Actions.ModuleActionCollection ModuleActions {
			get {
				DotNetNuke.Entities.Modules.Actions.ModuleActionCollection Actions = new DotNetNuke.Entities.Modules.Actions.ModuleActionCollection();
				Actions.Add(GetNextActionID(), this.GetLocalizedString("EditInjection.MenuItem.Title"), string.Empty, string.Empty, string.Empty, this.EditInjectionUrl, false, DotNetNuke.Security.SecurityAccessLevel.Edit, true, false);
				return Actions;
			}
		}

		#endregion
	}
}