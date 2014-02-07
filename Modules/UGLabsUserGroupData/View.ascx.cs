/*
' Copyright (c) 2013  DNN Corp.
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
using System.Web.UI.WebControls;
using DNNCommunity.Modules.UGLabsUserGroupData.Components;
using DotNetNuke.Common;
using DotNetNuke.Common.Lists;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Security;
using DotNetNuke.Security.Roles;
using DotNetNuke.Security.Roles.Internal;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Modules.Actions;
using DotNetNuke.Services.Localization;
using DotNetNuke.UI.Skins.Controls;

/*
 * TODO: Fix UI issues with require fields
 * TODO: Add logging to tell who and when meta data was last updated
 * TODO: Add ability to delegate management of meta data to security roles
 * TODO: Gamify the user group updates
 */

namespace DNNCommunity.Modules.UGLabsUserGroupData
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

        private const string GROUPID_FORMAT = "GroupId={0}";
        private int p_GroupId = Null.NullInteger;
        
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

        protected RoleInfo Group
        {
            get
            {
                var ctlRole = new RoleController();
                var currentRole = ctlRole.GetRole(GroupId, PortalId);

                if (currentRole != null && currentRole.RoleID > Null.NullInteger)
                {
                    return currentRole;
                }
                else
                {
                    return null;
                }
            }
        }

        private string CurrentUrl
        {
            get
            {
                if (GroupId > Null.NullInteger)
                {
                    return Globals.NavigateURL(TabId, string.Empty, string.Format(GROUPID_FORMAT, GroupId));
                }
                else
                {
                    return Globals.NavigateURL();
                }
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

        protected void cboCountry_SelectionChanged(Object o, EventArgs e)
        {
            LoadRegions(cboCountry.SelectedValue);
        }

        protected void lnkSave_Click(Object o, EventArgs e)
        {
            if (Page.IsValid)
            {
                SaveSettings();
                Response.Redirect(CurrentUrl);
            }
        }

        protected void lnkReset_Click(Object o, EventArgs e)
        {
            Response.Redirect(CurrentUrl);
        }

        #endregion

        #region Helper Methods

        private void BindData()
        {
            // check to see if this module is in the right place
            if (GroupId == Null.NullInteger)
            {
                // only show the placement error to admins
                if (UserInfo.IsInRole(PortalSettings.AdministratorRoleName) || UserInfo.IsSuperUser)
                {
                    DotNetNuke.UI.Skins.Skin.AddModuleMessage(this, GetLocalizedString("GroupID.ErrorMessage"), ModuleMessage.ModuleMessageType.RedError);
                    pnlUserGroupData.Visible = false;
                    return;
                }

                // hide the module entirely from everyone else
                ContainerControl.Visible = false;
                return;
            }

            // check to make sure we're showing it to the right people
            if (Group != null && Group.RoleID > Null.NullInteger)
            {
                // query to see if the current user is the owner of the current social group
                var userRoles = from r in UserInfo.Social.Roles where r.RoleID == GroupId && r.IsOwner select r;

                // show to admins or group owners (user group leaders)
                if (!userRoles.Any() && !UserInfo.IsInRole(PortalSettings.AdministratorRoleName) && !UserInfo.IsSuperUser)
                {
                    // hide the module and stop processing
                    ContainerControl.Visible = false;
                    return;
                }
            }
            else
            {
                // hide the module and stop processing
                ContainerControl.Visible = false;
                return;
            }

            LocalizeModule();
            LoadLocales();
            LoadCountries();
            LoadSettings();
        }

        private void LocalizeModule()
        {
            rfvGroupName.ErrorMessage = GetLocalizedString("rfvGroupName.ErrorMessage");
            revGroupName.ErrorMessage = GetLocalizedString("revGroupName.ErrorMessage");
            rfvCountry.ErrorMessage = GetLocalizedString("rfvCountry.ErrorMessage");
            rfvRegion.ErrorMessage = GetLocalizedString("rfvRegion.ErrorMessage");
            rfvCity.ErrorMessage = GetLocalizedString("rfvCity.ErrorMessage");
            rfvDefaultLanguage.ErrorMessage = GetLocalizedString("rfvDefaultLanguage.ErrorMessage");
            revWebsiteUrl.ErrorMessage = GetLocalizedString("revWebsiteUrl.ErrorMessage");
            revWebsiteUrl.ValidationExpression = FeatureController.PATTERN_WEBSITE_URL;
            revFacebookUrl.ErrorMessage = GetLocalizedString("revFacebookUrl.ErrorMessage");
            revFacebookUrl.ValidationExpression = FeatureController.PATTERN_FACEBOOK_URL;
            revTwitterUrl.ErrorMessage = GetLocalizedString("revTwitterUrl.ErrorMessage");
            revTwitterUrl.ValidationExpression = FeatureController.PATTERN_TWITTER_URL;
            revLinkedInUrl.ErrorMessage = GetLocalizedString("revLinkedInUrl.ErrorMessage");
            revLinkedInUrl.ValidationExpression = FeatureController.PATTERN_LINKEDIN_URL;
            revGooglePlusUrl.ErrorMessage = GetLocalizedString("revGooglePlusUrl.ErrorMessage");
            revGooglePlusUrl.ValidationExpression = FeatureController.PATTERN_GOOGLEPLUS_URL;
            revMeetUpUrl.ErrorMessage = GetLocalizedString("revMeetUpUrl.ErrorMessage");
            revMeetUpUrl.ValidationExpression = FeatureController.PATTERN_MEETUP_URL;
            revYouTubeUrl.ErrorMessage = GetLocalizedString("revYouTubeUrl.ErrorMessage");
            revYouTubeUrl.ValidationExpression = FeatureController.PATTERN_YOUTUBE_URL;

            lnkSave.Text = GetLocalizedString("lnkSave.Text");
            lnkReset.Text = GetLocalizedString("lnkReset.Text");
        }

        private void LoadLocales()
        {
            cboDefaultLanguage.DataSource = LocaleController.Instance.GetLocales(PortalId).Keys;
            cboDefaultLanguage.DataBind();
            
            foreach (ListItem oItem in cboDefaultLanguage.Items)
            {
                oItem.Value = Regex.Replace(oItem.Value, "-", "_");
            }

            cboDefaultLanguage.Items.Insert(0, "---");
        }

        private void LoadCountries()
        {
            var ctlList = new ListController();
            cboCountry.DataSource = ctlList.GetListEntryInfoItems("Country", string.Empty, PortalId); 
            cboCountry.DataTextField = "Text";
            cboCountry.DataValueField = "Value";
            cboCountry.DataBind();

            cboCountry.Items.Insert(0,new ListItem("---"));
        }

        private void LoadRegions(string CountryId)
        {
            var ctlList = new ListController();
            IEnumerable<ListEntryInfo> regions = null;

            regions = ctlList.GetListEntryInfoItems("Region", string.Concat("Country.", CountryId), PortalId);

            if (regions != null && regions.Any())
            {
                cboRegion.DataSource = regions;
                cboRegion.DataTextField = "Text";
                cboRegion.DataValueField = "Value";
                cboRegion.DataBind();

                cboRegion.Items.Insert(0, new ListItem("---"));

                ToggleRegion(RegionState.DropDownList);
            }
            else
            {
                ToggleRegion(RegionState.TextBox);
            }
        }

        private void ToggleRegion(RegionState state)
        {
            if (state == RegionState.DropDownList)
            {
                lblRegion.CssClass = "dnnFormRequired";
                txtRegion.Visible = false;
                cboRegion.Visible = true;
                rfvRegion.Enabled = true;
            }
            else
            {
                lblRegion.CssClass = string.Empty;
                txtRegion.Visible = true;
                cboRegion.Visible = false;
                rfvRegion.Enabled = false;
            }
        }

        private void LoadSettings()
        {
            if (Group != null && Group.RoleID > Null.NullInteger)
            {
                txtGroupName.Text = Group.RoleName;

                if (string.IsNullOrEmpty(GetSetting(FeatureController.KEY_COUNTRY)) == false) 
                    cboCountry.Items.FindByValue(GetSetting(FeatureController.KEY_COUNTRY)).Selected = true;

                ParseRegionLoadSetting();

                if (string.IsNullOrEmpty(GetSetting(FeatureController.KEY_CITY)) == false)
                    txtCity.Text = GetSetting(FeatureController.KEY_CITY);

                if (string.IsNullOrEmpty(GetSetting(FeatureController.KEY_DEFAULTLANGUAGE)) == false)
                    cboDefaultLanguage.Items.FindByValue(GetSetting(FeatureController.KEY_DEFAULTLANGUAGE)).Selected = true;

                // set-up a default locale
                if (cboDefaultLanguage.Items.Count == 2 && cboDefaultLanguage.SelectedIndex == 0)
                    cboDefaultLanguage.SelectedIndex = 1;

                if (string.IsNullOrEmpty(GetSetting(FeatureController.KEY_WEBSITEURL)) == false)
                    txtWebsiteUrl.Text = GetSetting(FeatureController.KEY_WEBSITEURL);

                if (string.IsNullOrEmpty(GetSetting(FeatureController.KEY_FACEBOOKURL)) == false)
                    txtFacebookUrl.Text = GetSetting(FeatureController.KEY_FACEBOOKURL);

                if (string.IsNullOrEmpty(GetSetting(FeatureController.KEY_TWITTERURL)) == false)
                    txtTwitterUrl.Text = GetSetting(FeatureController.KEY_TWITTERURL);

                if (string.IsNullOrEmpty(GetSetting(FeatureController.KEY_LINKEDINURL)) == false)
                    txtLinkedInUrl.Text = GetSetting(FeatureController.KEY_LINKEDINURL);

                if (string.IsNullOrEmpty(GetSetting(FeatureController.KEY_GOOGLEPLUSURL)) == false)
                    txtGooglePlusUrl.Text = GetSetting(FeatureController.KEY_GOOGLEPLUSURL);

                if (string.IsNullOrEmpty(GetSetting(FeatureController.KEY_MEETUPURL)) == false)
                    txtMeetUpUrl.Text = GetSetting(FeatureController.KEY_MEETUPURL);

                if (string.IsNullOrEmpty(GetSetting(FeatureController.KEY_YOUTUBEURL)) == false)
                    txtYouTubeUrl.Text = GetSetting(FeatureController.KEY_YOUTUBEURL);
            }
        }

        private void ParseRegionLoadSetting()
        {
            if (string.IsNullOrEmpty(GetSetting(FeatureController.KEY_REGION)) == false)
            {
                LoadRegions(cboCountry.SelectedValue);

                var ctlList = new ListController();
                var value = GetSetting(FeatureController.KEY_REGION);

                var listItem =
                    from i in ctlList.GetListEntryInfoItems("Region", string.Concat("Country.", GetSetting(FeatureController.KEY_COUNTRY)), PortalId)
                    select i;

                if (listItem.Any())
                {
                    cboRegion.Items.FindByValue(value).Selected = true;
                    ToggleRegion(RegionState.DropDownList);
                }
                else
                {
                    txtRegion.Text = value;
                    ToggleRegion(RegionState.TextBox);
                }
            }
            else
            {
                // default state
                ToggleRegion(RegionState.TextBox);
            }
        }

        private string GetSetting(string Key)
        {
            string value = string.Empty;

            if (Group != null && Group.Settings.Keys.Contains(Key))
            {
                value = Group.Settings[Key].ToString(CultureInfo.InvariantCulture);
            }

            return value;
        }

        private void SaveSettings()
        {
            var ctlRole = new RoleController();
            RoleInfo role = ctlRole.GetRole(GroupId, PortalId);
            var sec = new PortalSecurity();

            role.RoleName = sec.InputFilter(txtGroupName.Text.Trim(), PortalSecurity.FilterFlag.NoMarkup);

            SaveSetting(ref role, FeatureController.KEY_COUNTRY, cboCountry.SelectedValue);
            SaveSetting(ref role, FeatureController.KEY_COUNTRYFULL, cboCountry.SelectedItem.Text);

            SaveSetting(ref role, FeatureController.KEY_REGION, sec.InputFilter(ParseRegionSaveSetting(), PortalSecurity.FilterFlag.NoMarkup));
            if (role.Settings[FeatureController.KEY_REGION] == cboRegion.SelectedValue)
            {
                SaveSetting(ref role, FeatureController.KEY_REGIONFULL, cboRegion.SelectedItem.Text);
            }
            else
            {
                SaveSetting(ref role, FeatureController.KEY_REGIONFULL, sec.InputFilter(txtRegion.Text.Trim(), PortalSecurity.FilterFlag.NoMarkup));
            }

            SaveSetting(ref role, FeatureController.KEY_CITY, sec.InputFilter(txtCity.Text.Trim(), PortalSecurity.FilterFlag.NoMarkup));

            SaveSetting(ref role, FeatureController.KEY_DEFAULTLANGUAGE, cboDefaultLanguage.SelectedValue);

            SaveSetting(ref role, FeatureController.KEY_WEBSITEURL, sec.InputFilter(txtWebsiteUrl.Text.Trim(), PortalSecurity.FilterFlag.NoMarkup));

            SaveSetting(ref role, FeatureController.KEY_FACEBOOKURL, sec.InputFilter(txtFacebookUrl.Text.Trim(), PortalSecurity.FilterFlag.NoMarkup));

            SaveSetting(ref role, FeatureController.KEY_TWITTERURL, sec.InputFilter(txtTwitterUrl.Text.Trim(), PortalSecurity.FilterFlag.NoMarkup));

            SaveSetting(ref role, FeatureController.KEY_LINKEDINURL, sec.InputFilter(txtLinkedInUrl.Text.Trim(), PortalSecurity.FilterFlag.NoMarkup));

            SaveSetting(ref role, FeatureController.KEY_GOOGLEPLUSURL, sec.InputFilter(txtGooglePlusUrl.Text.Trim(), PortalSecurity.FilterFlag.NoMarkup));

            SaveSetting(ref role, FeatureController.KEY_MEETUPURL, sec.InputFilter(txtMeetUpUrl.Text.Trim(), PortalSecurity.FilterFlag.NoMarkup));

            SaveSetting(ref role, FeatureController.KEY_YOUTUBEURL, sec.InputFilter(txtYouTubeUrl.Text.Trim(), PortalSecurity.FilterFlag.NoMarkup));

            // update the role to save the name change
            ctlRole.UpdateRole(role);

            // save the settings
            TestableRoleController.Instance.UpdateRoleSettings(role, true);
        }

        private void SaveSetting(ref RoleInfo Role, string Key, string Value)
        {
            if (Role.Settings.ContainsKey(Key))
            {
                // update the existing key
                if (string.IsNullOrEmpty(Value))
                {
                    // remove the key to keep things tidy if the user removed it too
                    Role.Settings.Remove(Key);
                }
                else
                {
                    // update
                    Role.Settings[Key] = Value;
                }
            }
            else
            {
                // add a new key/value pair
                if (!string.IsNullOrEmpty(Value)) Role.Settings.Add(Key, Value);
            }
        }

        private string ParseRegionSaveSetting()
        {
            var ctlList = new ListController();
            var value = string.Empty;

            var listItem =
                from i in ctlList.GetListEntryInfoItems("Region", string.Concat("Country.", cboCountry.SelectedValue), PortalId)
                select i;

            value = listItem.Any() ? cboRegion.SelectedValue : txtRegion.Text.Trim();

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

        private enum RegionState
        {
            DropDownList = 0, 
            TextBox = 1
        }

    }
}