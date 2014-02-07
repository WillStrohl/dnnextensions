/*
' Copyright (c) 2012  DotNetNuke Corporation
'  All rights reserved.
' 
' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED
' TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
' THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF
' CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
' DEALINGS IN THE SOFTWARE.
' 
*/


using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using DotNetNuke.Common;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Security;
using DotNetNuke.Security.Roles.Internal;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Security.Roles;
using DotNetNuke.UI.Skins.Controls;

namespace DNNCommunity.Modules.UGLabsMetaData
{
    /// -----------------------------------------------------------------------------
    /// <summary>
    /// The EditUGLabsMetaData class is used to manage content
    /// 
    /// Typically your edit control would be used to create new content, or edit existing content within your module.
    /// The ControlKey for this control is "Edit", and is defined in the manifest (.dnn) file.
    /// 
    /// Because the control inherits from UGLabsMetaDataModuleBase you have access to any custom properties
    /// defined there, as well as properties from DNN such as PortalId, ModuleId, TabId, UserId and many more.
    /// 
    /// </summary>
    /// -----------------------------------------------------------------------------
    public partial class Edit : UGLabsMetaDataModuleBase
    {

        #region Private Members

        private const string GROUPID_FORMAT = "GroupId={0}";

        private int p_GroupId = Null.NullInteger;
        private string p_SettingKey = Null.NullString;
        private string p_SettingValue = Null.NullString;
        private string p_Action = Null.NullString;

        #endregion

        #region Properties

        protected int GroupId
        {
            get
            {
                if (p_GroupId != Null.NullInteger) return p_GroupId;

                var groupId = Request.QueryString["GroupId"];

                if (groupId != null && Regex.IsMatch(groupId.ToString(CultureInfo.InvariantCulture), @"^\d+$", RegexOptions.IgnoreCase))
                {
                    p_GroupId = int.Parse(groupId.ToString(CultureInfo.InvariantCulture));
                }

                return p_GroupId;
            }
        }

        protected string SettingKey
        {
            get
            {
                if (!string.IsNullOrEmpty(p_SettingKey)) return p_SettingKey;

                var settingKeyValue = Request.QueryString["Key"];

                if (settingKeyValue != null)
                {
                    p_SettingKey = settingKeyValue.ToString(CultureInfo.InvariantCulture);
                }

                return p_SettingKey;
            }
        }

        protected string SettingValue
        {
            get
            {
                if (!string.IsNullOrEmpty(p_SettingValue)) return p_SettingValue;

                if (!string.IsNullOrEmpty(SettingKey))
                {
                    p_SettingValue = (from s in RoleProvider.Instance().GetRoleSettings(GroupId)
                        where s.Key == SettingKey
                        select s.Value).First();
                }

                return p_SettingValue;
            }
        }

        protected string Action
        {
            get
            {
                if (p_Action != Null.NullString) return p_Action;

                var value = Request.QueryString["Action"];

                if (value != null)
                {
                    p_Action = value;
                }

                return p_Action;
            }
        }

        #endregion

        #region Event Handlers

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack) BindData();
            }
            catch (Exception exc) //Module failed to load
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        protected void lnkReturn_Click(object o, EventArgs e)
        {
            SendBackToPage();
        }

        protected void lnkDelete_Click(object o, EventArgs e)
        {
            DeleteMetadata();
        }

        protected void lnkUpdate_Click(object o, EventArgs e)
        {
            if (Page.IsValid)
            {
                SaveMetadata();
                SendBackToPage();
            }
        }

        #endregion

        #region Helper Methods

        private void BindData()
        {
            LocalizeModule();

            lnkDelete.Visible = (GroupId != Null.NullInteger);

            if (GroupId > Null.NullInteger && !string.IsNullOrEmpty(SettingKey))
            {
                // this is an edit view
                var setting = from s in RoleProvider.Instance().GetRoleSettings(GroupId)
                    where s.Key == SettingKey
                    select s;

                txtSettingKey.Text = setting.First().Key;
                txtSettingValue.Text = setting.First().Value;
            }
            else
            {
                // this is a new item view
                txtSettingKey.Focus();
            }
        }

        private void LocalizeModule()
        {
            rfvSettingKey.ErrorMessage = GetLocalizedString("rfvSettingKey.ErrorMessage");
            rfvSettingValue.ErrorMessage = GetLocalizedString("rfvSettingValue.ErrorMessage");

            lnkDelete.Text = GetLocalizedString("lnkDelete.Text");
            lnkReturn.Text = GetLocalizedString("lnkReturn.Text");
            lnkUpdate.Text = GetLocalizedString("lnkUpdate.Text");
        }

        private void SendBackToPage()
        {
            Response.Redirect(Globals.NavigateURL(TabId, string.Empty, string.Format(GROUPID_FORMAT, GroupId)));
        }

        #endregion

        #region Data Access

        private void SaveMetadata()
        {
            var security = new DotNetNuke.Security.PortalSecurity();
            var ctlRole = new RoleController();
            var role = ctlRole.GetRole(GroupId, PortalId);

            var settingKey = security.InputFilter(txtSettingKey.Text.Trim(), PortalSecurity.FilterFlag.NoMarkup);
            var settingValue = security.InputFilter(txtSettingValue.Text.Trim(), PortalSecurity.FilterFlag.NoScripting);

            if (role.Settings.ContainsKey(settingKey))
            {
                // update the existing key
                role.Settings[settingKey] = settingValue;
            }
            else
            {
                // add a new key
                role.Settings.Add(settingKey, settingValue);
            }

            TestableRoleController.Instance.UpdateRoleSettings(role, true);
        }

        private void DeleteMetadata()
        {
            if (ExecuteDeleteCommand())
            {
                SendBackToPage();
            }
            else
            {
                DotNetNuke.UI.Skins.Skin.AddModuleMessage(this, 
                    GetLocalizedString("DeleteMetadata.ErrorMessage"),
                    ModuleMessage.ModuleMessageType.RedError);
            }
        }

        private bool ExecuteDeleteCommand()
        {
            try
            {
                var ctlRole = new RoleController();
                var role = ctlRole.GetRole(GroupId, PortalId);

                var result = role.Settings.Remove(SettingKey);

                RoleProvider.Instance().UpdateRoleSettings(role);

                return result;
            }
            catch(Exception ex)
            {
                Exceptions.LogException(ex);
                return false;
            }
        }

        #endregion

    }
}