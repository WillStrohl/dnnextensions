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

using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Services.Exceptions;
using System;
using System.Globalization;
using DotNetNuke.Entities.Users;
using DotNetNuke.Security;
using WillStrohl.Modules.Injection.Entities;
using WillStrohl.Modules.Injection.Components;

namespace WillStrohl.Modules.Injection
{
    public partial class EditInjections : WNSPortalModuleBase
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
                    ToggleType();
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

        protected void cmdCancel_Click(object sender, EventArgs e)
        {
            ClearForm();
            TogglePanels();
            BindData();
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

        protected void radType_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            ToggleType();
        }

        protected void cvContent_OnServerValidate(object source, ServerValidateEventArgs args)
        {
            if (radType.SelectedIndex == 0)
            {
                var isValidPathFormat = false;
                var isValidFilePath = false;
                var pathToTest = txtContent.Text.Trim();

                isValidPathFormat = (InjectionController.IsValidCssInjectionType(pathToTest) ||
                                     InjectionController.IsValidJavaScriptInjectionType(pathToTest));

                if (pathToTest.StartsWith("http"))
                {
                    // external file path
                    isValidFilePath = InjectionController.IsValidFilePath(pathToTest);
                }
                else
                {
                    // local file path
                    var fullFilePath = Server.MapPath(pathToTest);
                    isValidFilePath = File.Exists(fullFilePath);
                }

                args.IsValid = (isValidPathFormat && isValidFilePath);
            }
            else
            {
                args.IsValid = true;
            }
        }

        #endregion

        #region Private Helper Functions

        private void BindData()
        {
            LocalizeModule();

            if (Injections.Count > 0)
            {
                dlInjection.DataSource = Injections;
                dlInjection.DataBind();
                dlInjection.Visible = true;
                lblNoRecords.Visible = false;
            }
            else
            {
                dlInjection.Visible = false;
                lblNoRecords.Visible = true;
            }

            if (radType.Items.Count == 0)
            {
                radType.Items.Add(new ListItem(GetLocalizedString("radType.0.Text")));
                radType.Items.Add(new ListItem(GetLocalizedString("radType.1.Text")));
                radType.ClearSelection();
                // default injection type for NEW injections is 0 because that's the most common use case
                radType.SelectedIndex = 0;
            }

            if (ddlCrmProvider.Items.Count == 0)
            {
                ddlCrmProvider.Items.Add(new ListItem(GetLocalizedString("ddlCrmProvider.0.Text")));
                ddlCrmProvider.Items.Add(new ListItem(GetLocalizedString("ddlCrmProvider.1.Text")));
                ddlCrmProvider.Items.Add(new ListItem(GetLocalizedString("ddlCrmProvider.2.Text")));
                ddlCrmProvider.Items.Add(new ListItem(GetLocalizedString("ddlCrmProvider.3.Text")));
                ddlCrmProvider.ClearSelection();
                ddlCrmProvider.SelectedIndex = 0;
            }

            if (radInject.Items.Count == 0)
            {
                radInject.Items.Add(new ListItem(GetLocalizedString("radInject.0.Text")));
                radInject.Items.Add(new ListItem(GetLocalizedString("radInject.1.Text")));
                radInject.ClearSelection();
                radInject.SelectedIndex = 0;
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
            cvContent.ErrorMessage = GetLocalizedString("cvContent.ErrorMessage");
            rvCrmPriority.ErrorMessage = GetLocalizedString("rvCrmPriority.ErrorMessage");
        }

        private void ClearForm()
        {
            hidInjectionId.Value = string.Empty;
            txtName.Text = GetLocalizedString("txtName.Text");
            txtContent.Text = string.Empty;
            chkEnabled.Checked = true;
            radType.ClearSelection();
            radType.Items.FindByText(GetLocalizedString("radType.0.Text")).Selected = true;
            radInject.ClearSelection();
            radInject.Items.FindByText(GetLocalizedString("radInject.0.Text")).Selected = true;
            cvName.Enabled = true;
            ddlCrmProvider.ClearSelection();
            ddlCrmProvider.SelectedIndex = 0;

            ToggleType();
        }

        private void BindForm(int ItemId)
        {
            var ctlModule = new InjectionController();
            var injection = new InjectionInfo();

            injection = ctlModule.GetInjectionContent(ItemId);

            txtName.Text = injection.InjectName;
            txtContent.Text = Server.HtmlDecode(injection.InjectContent);
            radInject.ClearSelection();

            if (injection.InjectTop)
            {
                radInject.Items.FindByText(GetLocalizedString("radInject.0.Text")).Selected = true;
            }
            else
            {
                radInject.Items.FindByText(GetLocalizedString("radInject.1.Text")).Selected = true;
            }

            chkEnabled.Checked = injection.IsEnabled;
            hidInjectionId.Value = injection.InjectionId.ToString();
            cvName.Enabled = false;

            cmdDelete.Visible = !string.IsNullOrEmpty(hidInjectionId.Value);

            if (injection.CustomProperties.Any(p => p.Name == InjectionInfoMembers.CrmPriorityField))
            {
                var priorityLevel = injection.CustomProperties.FirstOrDefault(p => p.Name == InjectionInfoMembers.CrmPriorityField).Value;
                txtCrmPriority.Text = (priorityLevel == "-1") ? string.Empty : priorityLevel;
            } 
            
            if (injection.CustomProperties.Any(p => p.Name == InjectionInfoMembers.CrmProviderField))
            {
                var provider = injection.CustomProperties.FirstOrDefault(p => p.Name == InjectionInfoMembers.CrmProviderField).Value;
                ddlCrmProvider.ClearSelection();
                ddlCrmProvider.SelectedIndex = int.Parse(provider);
            }

            if (injection.CustomProperties.Any(p => p.Name == InjectionInfoMembers.LastUpdatedByField))
            {
                var updatedBy = injection.CustomProperties.FirstOrDefault(p => p.Name == InjectionInfoMembers.LastUpdatedByField);
                var updatedDate = injection.CustomProperties.FirstOrDefault(p => p.Name == InjectionInfoMembers.LastUpdatedDateField);

                var user = UserController.GetUserById(PortalSettings.PortalId, int.Parse(updatedBy.Value, NumberStyles.Integer));

                lblAudit.Text = string.Format(GetLocalizedString("lblAudit"), user.DisplayName, updatedDate.Value);
            }

            if (injection.CustomProperties.Any(p => p.Name == InjectionInfoMembers.InjectionTypeField))
            {
                var intType =int.Parse(
                    injection.CustomProperties.FirstOrDefault(p => p.Name == InjectionInfoMembers.InjectionTypeField)
                        .Value);

                radType.SelectedIndex = intType;
            }
            else
            {
                // the default for existing injections is 1 because JS/CSS wasn't an option in the past
                radType.SelectedIndex = 1;
            }

            ToggleType();
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

        private void SwapOrder(int ItemId, string UpDown)
        {
            // set the global id to match the one we're looking for
            p_SearchParam = ItemId;

            // change the order
            InjectionController ctlModule = new InjectionController();
            ctlModule.ChangeOrder(ItemId, UpDown);
        }

        #endregion

        #region Data Access

        private void SaveInjection()
        {
            try
            {
                var ctlModule = new InjectionController();
                InjectionInfo objInj = null;

                objInj = !string.IsNullOrEmpty(hidInjectionId.Value) ? ctlModule.GetInjectionContent(int.Parse(hidInjectionId.Value)) : new InjectionInfo();

                PopulateInjectionForSave(ref objInj);

                if (!string.IsNullOrEmpty(hidInjectionId.Value))
                {
                    ctlModule.UpdateInjectionContent(objInj);
                }
                else
                {
                    ctlModule.AddInjectionContent(objInj);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void PopulateInjectionForSave(ref InjectionInfo injection)
        {
            var security = new PortalSecurity();

            injection.InjectContent = Server.HtmlEncode(txtContent.Text);
            injection.InjectName = security.InputFilter(txtName.Text, PortalSecurity.FilterFlag.NoMarkup);
            injection.InjectTop = radInject.Items.FindByText(GetLocalizedString("radInject.0.Text")).Selected;
            injection.IsEnabled = chkEnabled.Checked;
            injection.ModuleId = ModuleId;

            if (injection.CustomProperties.Any(p => p.Name == InjectionInfoMembers.InjectionTypeField))
            {
                // update the existing injection type
                injection.CustomProperties.FirstOrDefault(p => p.Name == InjectionInfoMembers.InjectionTypeField)
                    .Value = radType.SelectedIndex.ToString();
            }
            else
            {
                // create new injection type
                injection.CustomProperties.Add(new CustomPropertyInfo()
                {
                    Name = InjectionInfoMembers.InjectionTypeField,
                    Value = radType.SelectedIndex.ToString()
                });
            }

            // CRM PRIORITY LEVEL
            var priorityLevel = ParsePriotityLevel(security);
            if (injection.CustomProperties.Any(p => p.Name == InjectionInfoMembers.CrmPriorityField))
            {
                // update existing CRM/CDF priority level
                injection.CustomProperties.FirstOrDefault(p => p.Name == InjectionInfoMembers.CrmPriorityField)
                    .Value = priorityLevel.ToString();
            }
            else
            {
                // create new CRM/CDF priority level
                injection.CustomProperties.Add(new CustomPropertyInfo()
                {
                    Name = InjectionInfoMembers.CrmPriorityField,
                    Value = priorityLevel.ToString()
                });
            }

            // CRM PAGE PROVIDER
            var provider = ddlCrmProvider.SelectedIndex;
            if (injection.CustomProperties.Any(p => p.Name == InjectionInfoMembers.CrmProviderField))
            {
                // update existing CRM/CDF provider
                injection.CustomProperties.FirstOrDefault(p => p.Name == InjectionInfoMembers.CrmProviderField).Value = provider.ToString();
            }
            else
            {
                // create new CRM/CDF provider
                injection.CustomProperties.Add(new CustomPropertyInfo()
                {
                    Name = InjectionInfoMembers.CrmProviderField,
                    Value = provider.ToString()
                });
            }

            if (injection.CustomProperties.Any(p => p.Name == InjectionInfoMembers.CrmPriorityField))
            {
                injection.CustomProperties.FirstOrDefault(p => p.Name == InjectionInfoMembers.CrmPriorityField)
                    .Value = security.InputFilter(txtCrmPriority.Text.Trim(), PortalSecurity.FilterFlag.NoMarkup);
            }
            else
            {
                injection.CustomProperties.Add(new CustomPropertyInfo()
                {
                    Name = InjectionInfoMembers.CrmPriorityField,
                    Value = security.InputFilter(txtCrmPriority.Text.Trim(), PortalSecurity.FilterFlag.NoMarkup)
                });
            }

            if (injection.CustomProperties.Any(p => p.Name == InjectionInfoMembers.LastUpdatedByField))
            {
                // update the existing auditing fields
                injection.CustomProperties.FirstOrDefault(p => p.Name == InjectionInfoMembers.LastUpdatedByField)
                    .Value = UserInfo.UserID.ToString();
                injection.CustomProperties.FirstOrDefault(p => p.Name == InjectionInfoMembers.LastUpdatedDateField)
                    .Value = DateTime.UtcNow.ToString();
            }
            else
            {
                // adding new audting fields
                injection.CustomProperties.Add(new CustomPropertyInfo()
                {
                    Name = InjectionInfoMembers.LastUpdatedByField,
                    Value = UserInfo.UserID.ToString()
                });

                injection.CustomProperties.Add(new CustomPropertyInfo()
                {
                    Name = InjectionInfoMembers.LastUpdatedDateField,
                    Value = DateTime.UtcNow.ToString()
                });
            }
        }

        private int ParsePriotityLevel(PortalSecurity security)
        {
            var priorityInput = security.InputFilter(txtCrmPriority.Text.Trim(), PortalSecurity.FilterFlag.NoMarkup);
            var priorityLevel = InjectionController.GetCrmPriority(priorityInput);

            return (priorityLevel > Null.NullInteger) ? priorityLevel : Null.NullInteger;
        }

        #endregion

        #region User Interface

        private void ToggleType()
        {
            divInject.Visible = radType.SelectedIndex == 1;
            radInject.Enabled = radType.SelectedIndex == 1;
            cvContent.Enabled = radType.SelectedIndex == 0;
            divAdvanced.Visible = radType.SelectedIndex == 0;

            txtContent.Rows = radType.SelectedIndex == 1 ? 10 : 2;
        }

        private void TogglePanels()
        {
            pnlAddNew.Visible = (!pnlAddNew.Visible);
            pnlManage.Visible = (!pnlManage.Visible);
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

        protected string GetInjectionTypeForDisplay(object InjectionId)
        {
            if (InjectionId == null) return string.Empty;

            var ctl = new InjectionController();
            var injection = ctl.GetInjectionContent(int.Parse(InjectionId.ToString(), NumberStyles.Integer));
            var injectionType = InjectionController.GetInjectionType(injection);
            
            if (injectionType == InjectionType.HtmlBottom || injectionType == InjectionType.HtmlTop)
            {
                return GetLocalizedString(injectionType.ToString());
            }
            else
            {
                var injectionProvider = InjectionController.GetCrmProvider(injection);

                if (string.IsNullOrEmpty(injectionProvider))
                {
                    return string.Concat(GetLocalizedString(injectionType.ToString()),
                        GetLocalizedString(InjectionController.GetCrmProviderDefault(injectionType)));
                }
                else
                {
                    return string.Concat(GetLocalizedString(injectionType.ToString()),
                        GetLocalizedString(injectionProvider));
                }
            }
        }

        #endregion
    }
}