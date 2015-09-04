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

using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Services.Exceptions;
using System;
using System.Globalization;
using DotNetNuke.Security;
using WillStrohl.Modules.Injection.Entities;
using WillStrohl.Modules.Injection.Components;

namespace WillStrohl.Modules.Injection
{
    public abstract partial class EditInjections : WNSPortalModuleBase
    {
        #region Private Members

        private InjectionInfoCollection p_Injections = null;
        private const string c_Command_Edit = "Edit";
        private const string c_Command_MoveUp = "MoveUp";
        private const string c_Command_MoveDown = "MoveDown";
        private const string c_Command_Delete = "Delete";

        private const string c_Command_Insert = "Insert";

        private const string c_True = "True";

        private int p_SearchParam = Null.NullInteger;
        private string p_EnabledImage = string.Empty;
        private string p_DisabledImage = string.Empty;
        private string p_EnabledAltText = string.Empty;

        private string p_DisabledAltText = string.Empty;

        #endregion

        #region Private Properties

        private InjectionInfoCollection Injections
        {
            get
            {
                if (p_Injections == null)
                {
                    InjectionController ctlModule = new InjectionController();
                    p_Injections = ctlModule.GetInjectionContents(ModuleId);
                }
                return p_Injections;
            }
        }

        private string EnabledImage
        {
            get
            {
                if (!string.IsNullOrEmpty(p_EnabledImage))
                {
                    return p_EnabledImage;
                }

                p_EnabledImage = string.Concat(DotNetNuke.Common.Globals.ApplicationPath, DotNetNuke.Entities.Icons.IconController.IconURL("Checked", "16x16"));

                return p_EnabledImage;
            }
        }

        private string DisabledImage
        {
            get
            {
                if (!string.IsNullOrEmpty(p_DisabledImage))
                {
                    return p_DisabledImage;
                }

                p_DisabledImage = string.Concat(DotNetNuke.Common.Globals.ApplicationPath, DotNetNuke.Entities.Icons.IconController.IconURL("Unchecked", "16x16"));

                return p_DisabledImage;
            }
        }

        private string EnabledAltText
        {
            get
            {
                if (string.IsNullOrEmpty(p_EnabledAltText))
                {
                    p_EnabledAltText = GetLocalizedString("Enabled.Alt");
                }

                return p_EnabledAltText;
            }
        }

        private string DisabledAltText
        {
            get
            {
                if (string.IsNullOrEmpty(p_DisabledAltText))
                {
                    p_DisabledAltText = GetLocalizedString("Disabled.Alt");
                }

                return p_DisabledAltText;
            }
        }

        #endregion

        #region Event Handlers

        protected EditInjections()
        {
            Load += Page_Load;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    pnlAddNew.Visible = false;
                    pnlManage.Visible = true;

                    BindData();
                }
                // Module failed to load
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc, IsEditable);
            }
        }
        
        protected void lnkAddNewInjection_Click(object sender, EventArgs e)
        {
            ClearForm();
            TogglePanels();

            cmdDelete.Visible = !string.IsNullOrEmpty(hidInjectionId.Value);

        }

        protected void cmdReturn_Click(object sender, EventArgs e)
        {
            Response.Redirect(DotNetNuke.Common.Globals.NavigateURL());
        }
        
        protected void cmdAdd_Click(object sender, EventArgs e)
        {
            if (Page.IsValid && !(string.IsNullOrEmpty(txtName.Text) | string.IsNullOrEmpty(txtContent.Text)))
            {
                SaveInjection();
                ClearForm();
                TogglePanels();
                BindData();
            }

        }

        protected void cvName_ServerValidate(object source, ServerValidateEventArgs args)
        {
            InjectionController ctlModule = new InjectionController();
            args.IsValid = (!ctlModule.DoesInjectionNameExist(txtName.Text, ModuleId));
        }

        protected void dlInjection_ItemCommand(object source, DataListCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case c_Command_MoveUp:
                    SwapOrder(Convert.ToInt32(e.CommandArgument), c_Command_MoveUp);
                    BindData();
                    break;
                case c_Command_MoveDown:
                    SwapOrder(Convert.ToInt32(e.CommandArgument), c_Command_MoveDown);
                    BindData();
                    break;
                case c_Command_Edit:
                    BindForm(Convert.ToInt32(e.CommandArgument));
                    TogglePanels();
                    break;
                case c_Command_Insert:
                    ClearForm();
                    TogglePanels();
                    break;
                case c_Command_Delete:
                    InjectionController ctlModule = new InjectionController();
                    ctlModule.DeleteInjectionContent(Convert.ToInt32(e.CommandArgument));
                    BindData();
                    break;
                default:
                    return;

                    break;
            }
        }
        
        protected void cmdDelete_Click(object sender, EventArgs e)
        {
            if (Regex.IsMatch(hidInjectionId.Value, "^\\d+$"))
            {
                InjectionController ctlModule = new InjectionController();
                ctlModule.DeleteInjectionContent(int.Parse(hidInjectionId.Value, System.Globalization.NumberStyles.Integer));
            }

            ClearForm();
            TogglePanels();
            BindData();

        }

        #endregion

        #region Private Helper Functions

        private void BindData()
        {
            LocalizeModule();

            // bind data to controls

            if (Injections.Count > 0)
            {
                var _with1 = dlInjection;
                _with1.DataSource = Injections;
                _with1.DataBind();
                dlInjection.Visible = true;
                lblNoRecords.Visible = false;
            }
            else
            {
                dlInjection.Visible = false;
                lblNoRecords.Visible = true;
            }

            txtName.Attributes.Add("onfocus", string.Concat("if (value == '", GetLocalizedString("txtName.Text"), "') { value = ''; }"));

            if (radInject.Items.Count == 0)
            {
                radInject.Items.Add(new ListItem(GetLocalizedString("radInject.0.Text")));
                radInject.Items.Add(new ListItem(GetLocalizedString("radInject.1.Text")));
            }

        }

        private void LocalizeModule()
        {
            txtName.Text = GetLocalizedString("txtName.Text");
            lnkAddNewInjection.Text = GetLocalizedString("lnkAdd.Text");
            cmdAdd.Text = GetLocalizedString("cmdAdd.Text");
            cmdDelete.Text = GetLocalizedString("cmdDelete.Text");
            rfvName.ErrorMessage = GetLocalizedString("rfvName.ErrorMessage");
            rfvName.InitialValue = GetLocalizedString("txtName.Text");
            rfvContent.ErrorMessage = GetLocalizedString("rfvContent.ErrorMessage");
            cmdCancel.Text = GetLocalizedString("cmdCancel.Text");
            cmdReturn.Text = GetLocalizedString("cmdReturn.Text");
            cvName.ErrorMessage = GetLocalizedString("cvName.ErrorMessage");

        }

        private void ClearForm()
        {
            hidInjectionId.Value = string.Empty;
            txtName.Text = GetLocalizedString("txtName.Text");
            txtContent.Text = string.Empty;
            chkEnabled.Checked = true;
            radInject.Items.FindByText(GetLocalizedString("radInject.0.Text")).Selected = true;
            cvName.Enabled = true;
        }

        private void BindForm(int ItemId)
        {
            InjectionController ctlModule = new InjectionController();
            InjectionInfo objInfo = new InjectionInfo();
            objInfo = ctlModule.GetInjectionContent(ItemId);

            txtName.Text = objInfo.InjectName;
            txtContent.Text = Server.HtmlDecode(objInfo.InjectContent);
            radInject.ClearSelection();
            if (objInfo.InjectTop)
            {
                radInject.Items.FindByText(GetLocalizedString("radInject.0.Text")).Selected = true;
            }
            else
            {
                radInject.Items.FindByText(GetLocalizedString("radInject.1.Text")).Selected = true;
            }
            chkEnabled.Checked = objInfo.IsEnabled;
            hidInjectionId.Value = objInfo.InjectionId.ToString();
            cvName.Enabled = false;

            cmdDelete.Visible = !string.IsNullOrEmpty(hidInjectionId.Value);

        }
        
        private void SwapOrder(int ItemId, string UpDown)
        {
            // set the global id to match the one we're looking for
            p_SearchParam = ItemId;

            // change the order
            InjectionController ctlModule = new InjectionController();
            ctlModule.ChangeOrder(ItemId, UpDown);

        }

        private bool FindInjectionById(InjectionInfo item)
        {
            return item.InjectionId == p_SearchParam;
        }

        private void TogglePanels()
        {
            pnlAddNew.Visible = (!pnlAddNew.Visible);
            pnlManage.Visible = (!pnlManage.Visible);
        }

        private void SaveInjection()
		{
			try {
				DotNetNuke.Security.PortalSecurity sec = new DotNetNuke.Security.PortalSecurity();
				InjectionController ctlModule = new InjectionController();
				InjectionInfo objInj = new InjectionInfo();

				if (!string.IsNullOrEmpty(hidInjectionId.Value)) {
					objInj = ctlModule.GetInjectionContent(int.Parse(hidInjectionId.Value));
					var _with2 = objInj;
					_with2.InjectContent = Server.HtmlEncode(txtContent.Text);
					_with2.InjectName = sec.InputFilter(txtName.Text, PortalSecurity.FilterFlag.NoMarkup);
					_with2.InjectTop = radInject.Items.FindByText(GetLocalizedString("radInject.0.Text")).Selected;
					_with2.IsEnabled = chkEnabled.Checked;
					_with2.ModuleId = ModuleId;
                    //TODO: FINISH THIS
                    //if (HasAuditingField(ref ref _with2.CustomProperties)) {
                    //    _with2.CustomProperties.Find(From);
                    //}
					ctlModule.UpdateInjectionContent(objInj);
				} else {
					var _with3 = objInj;
					_with3.InjectContent = Server.HtmlEncode(txtContent.Text);
					_with3.InjectName = sec.InputFilter(txtName.Text, PortalSecurity.FilterFlag.NoMarkup);
					_with3.InjectTop = radInject.Items.FindByText(GetLocalizedString("radInject.0.Text")).Selected;
					_with3.IsEnabled = chkEnabled.Checked;
					_with3.ModuleId = ModuleId;
					_with3.OrderShown = ctlModule.GetNextOrderNumber(ModuleId);
					ctlModule.AddInjectionContent(objInj);
				}
			} catch (Exception ex) {
				HandleException(ex);
			}
		}

        protected bool CommandUpVisible(object InjectionId)
        {
            if (InjectionId == null) return false;

            var ctlModule = new InjectionController();
            var oInject = ctlModule.GetInjectionContent(int.Parse(InjectionId.ToString(), NumberStyles.Integer));

            return (oInject.OrderShown != 1);
        }

        protected bool CommandDownVisible(object InjectionId)
        {
            if (InjectionId == null) return false;

            var ctlModule = new InjectionController();
            var oInject = ctlModule.GetInjectionContent(int.Parse(InjectionId.ToString(), NumberStyles.Integer));
            var collInject = ctlModule.GetInjectionContents(ModuleId);

            return (oInject.OrderShown != collInject.Count);
        }

        protected string GetEnabledImage(object EnabledText)
        {
            if (EnabledText != null && string.Equals(EnabledText.ToString(), c_True))
            {
                return EnabledImage;
            }
            else
            {
                return DisabledImage;
            }
        }

        protected string GetEnabledImageAltText(object EnabledText)
        {
            if (EnabledText != null && string.Equals(EnabledText, c_True))
            {
                return EnabledAltText;
            }
            else
            {
                return DisabledAltText;
            }
        }

        private void HandleException(Exception exc)
        {
            Exceptions.LogException(exc);
            if (UserInfo.IsSuperUser | UserInfo.UserID == PortalSettings.AdministratorId)
            {
                DotNetNuke.UI.Skins.Skin.AddModuleMessage(this, string.Concat(exc.Message, "<br />", exc.StackTrace), DotNetNuke.UI.Skins.Controls.ModuleMessage.ModuleMessageType.RedError);
            }
            else
            {
                DotNetNuke.UI.Skins.Skin.AddModuleMessage(this, exc.Message, DotNetNuke.UI.Skins.Controls.ModuleMessage.ModuleMessageType.RedError);
            }
        }

        #endregion
    }
}