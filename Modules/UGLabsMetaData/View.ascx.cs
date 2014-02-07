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
using System.Globalization;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using DotNetNuke.Common;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Users;
using DotNetNuke.Security;
using DotNetNuke.Security.Roles;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Entities.Icons;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Modules.Actions;
using DotNetNuke.Services.Localization;

namespace DNNCommunity.Modules.UGLabsMetaData
{
    /// -----------------------------------------------------------------------------
    /// <summary>
    /// The View class displays the content
    /// 
    /// Typically your view control would be used to display content or functionality in your module.
    /// 
    /// View may be the only control you have in your project depending on the complexity of your module
    /// 
    /// Because the control inherits from UGLabsMetaDataModuleBase you have access to any custom properties
    /// defined there, as well as properties from DNN such as PortalId, ModuleId, TabId, UserId and many more.
    /// 
    /// </summary>
    /// -----------------------------------------------------------------------------
    public partial class View : UGLabsMetaDataModuleBase, IActionable
    {

        #region Private Members

        private const string URL_MATCH_PATTERN = @"^http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&amp;=]*)?$";
        private const string DISPLAY_NAME_PATTERN = @"^\w+(\s*\w+)*$";
        private const string URL_FORMAT = "<a href=\"{0}\" target=\"_blank\">{0}</a>";
        private const string PROFILE_FORMAT = "<a href=\"{0}\" target=\"_blank\">{1}</a>";
        private const string USERID_FORMAT = "UserID={0}";
        private const string GROUPID_FORMAT = "GroupId={0}";

        private int p_GroupId = Null.NullInteger;
        private string p_EditImage = string.Empty;
        private string p_DeleteImage = string.Empty;

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

        protected string EditImage
        {
            get
            {
                if (string.IsNullOrEmpty(p_EditImage))
                {
                    p_EditImage = string.Concat(Globals.ApplicationPath, IconController.IconURL("Edit", "16x16"));
                }

                return p_EditImage;
            }
        }

        protected string DeleteImage
        {
            get
            {
                if (string.IsNullOrEmpty(p_DeleteImage))
                {
                    p_DeleteImage = string.Concat(Globals.ApplicationPath, IconController.IconURL("Delete", "16x16"));
                }

                return p_DeleteImage;
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

        protected void rptSettings_ItemCommand(object source, System.Web.UI.WebControls.RepeaterCommandEventArgs e)
        {
            if (e.CommandName.ToLower() == "delete")
            {
                var arg = e.CommandArgument;
            }
        }

        #endregion

        #region Helper Methods

        private void BindData()
        {
            var ctlRole = new RoleController();
            var currentRole = ctlRole.GetRole(GroupId, PortalId);

            if (currentRole != null)
            {

                rptSettings.DataSource = currentRole.Settings;
                rptSettings.DataBind();

            }
            else
            {
                lblMessage.Text = GetLocalizedString("NoRecords.Text");
            }

        }

        #endregion

        #region Grid Helpers

        protected string ReformatUrlForEdit(object Key)
        {
            if (Key == null) throw new ArgumentNullException("The group setting key cannot be null.");

            return EditUrl(string.Empty, string.Empty, "Edit", string.Concat("&GroupId=", GroupId, "&Action=edit&Key=", Key));
        }

        protected string ParseMetaDataValue(object MetaValue)
        {
            if (MetaValue == null) return string.Empty;

            var value = MetaValue.ToString().Trim();

            // check to see if this is a URL
            if (Regex.IsMatch(value, URL_MATCH_PATTERN))
            {
                return string.Format(URL_FORMAT, value);
            }

            // check to see if this looks like a profile display name
            if (Regex.IsMatch(value, DISPLAY_NAME_PATTERN))
            {
                // might be a display name for a user
                var users = from UserInfo u
                    in UserController.GetUsers(true, false, PortalId)
                            where u.DisplayName == value
                            select u;

                if (users.Count() == 1)
                {
                    var profileUrl = Globals.NavigateURL(PortalSettings.UserTabId, string.Empty,
                                                 string.Format(USERID_FORMAT, users.First().UserID));
                    return string.Format(PROFILE_FORMAT, profileUrl, value);
                }
            }

            return value;
        }

        #endregion

        #region Implementations

        /// <summary>
        /// ModuleActions - loads the actions menu with a custom menu item
        /// </summary>
        public ModuleActionCollection ModuleActions
        {
            get
            {
                var Actions = new ModuleActionCollection();
                Actions.Add(GetNextActionID(), 
                    GetLocalizedString("EditModule.Text"),
                    string.Empty, 
                    string.Empty, 
                    string.Empty, 
                    EditUrl(string.Empty, string.Empty, "Edit", string.Format(GROUPID_FORMAT, GroupId.ToString(CultureInfo.InvariantCulture))), 
                    false, 
                    SecurityAccessLevel.Edit, 
                    true, false);
                return Actions;
            }
        }

        #endregion

    }
}