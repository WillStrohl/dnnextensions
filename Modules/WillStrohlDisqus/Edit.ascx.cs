/*
  * Copyright (c) 2011-2016, Will Strohl
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
using System.Collections.Generic;
using System.IO;
using System.Globalization;
using System.Web.UI.WebControls;
using DotNetNuke.Common;
using DotNetNuke.Entities.Controllers;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Modules.WillStrohlDisqus.Components;
using DotNetNuke.Security;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Scheduling;

namespace DotNetNuke.Modules.WillStrohlDisqus
{

    /// -----------------------------------------------------------------------------
    /// <summary>
    /// The EditWillStrohlDisqus class is used to manage content
    /// </summary>
    /// -----------------------------------------------------------------------------
    public partial class Edit : WillStrohlDisqusModuleBase
    {

        #region Private Properties

        private const string RECEIVER_FILE_NAME = "disqus.htm";
        private const string URL_FORMAT = "<a href=\"{0}\" target=\"_blank\">{0}</a>";

        private int AttachedModule { get; set; }
        private string DisqusView { get; set; }
        private int DisplayItems { get; set; }
        private bool ShowModerators { get; set; }
        private string ColorTheme { get; set; }
        private string DefaultTab { get; set; }
        private int CommentLength { get; set; }
        private bool ShowAvatar { get; set; }
        private int AvatarSize { get; set; }

        #endregion

        #region Event Handlers

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
            this.Load += new System.EventHandler(this.PageLoad);
            this.cmdSave.Click += new System.EventHandler(this.CmdSaveClick);
            this.cmdReturn.Click += new System.EventHandler(this.CmdReturnClick);
            this.cboModuleView.SelectedIndexChanged += new System.EventHandler(this.CboModuleViewSelectedIndexChanged);
            this.cmdReceiverFile.Click += new EventHandler(this.CmdReceiverFileClick);
            chkSsoEnabled.CheckedChanged += new EventHandler(this.ChkSsoEnabledClick);
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Page_Load runs when the control is loaded
        /// </summary>
        /// -----------------------------------------------------------------------------
        private void PageLoad(object sender, System.EventArgs e)
        {
            try
            {
                if(!this.IsPostBack)
                {
                    this.BindData();
                }
            }
            catch (Exception exc) //Module failed to load
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        /// <summary>
        /// Runs when the Save linkbutton is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CmdSaveClick(object sender, System.EventArgs e)
        {
            if (this.Page.IsValid)
            {
                this.SaveSettings();
                this.SendBackToModule();
            }
        }

        /// <summary>
        /// Runs when the Return linkbutton is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CmdReturnClick(object sender, System.EventArgs e)
        {
            this.SendBackToModule();
        }

        /// <summary>
        /// Runs when the DisqusView widget selection is changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CboModuleViewSelectedIndexChanged(object sender, System.EventArgs e)
        {
            this.ChangeView(this.cboModuleView.SelectedValue);
        }

        /// <summary>
        /// Runs when the ReceiverFile linkbutton is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CmdReceiverFileClick(object sender, System.EventArgs e)
        {
            this.CreateReceiverFile();
            this.SendBackToModule();
        }

        /// <summary>
        /// Runs with the chkSsoEnabled checkbox is checked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ChkSsoEnabledClick(object sender, System.EventArgs e)
        {
            if (chkSsoEnabled.Checked)
            {
                chkRequireDnnLogin.Checked = true;
            }
        }

        #endregion

        #region Private Helper Methods

        private void BindData()
        {
            this.LocalizeModule();

            Dictionary<int, ModuleInfo>.ValueCollection collModule = new ModuleController().GetTabModules(this.TabId).Values;

            foreach (ModuleInfo objModule in collModule)
            {
                if (objModule.ModuleID == this.ModuleId && objModule.IsDeleted == false) continue;

                if (!string.IsNullOrEmpty(objModule.ModuleTitle.Trim()))
                {
                    this.cboModuleList.Items.Add(new ListItem(objModule.ModuleTitle, objModule.ModuleID.ToString()));
                }
                else
                {
                    this.cboModuleList.Items.Add(new ListItem(this.GetLocalizedString("NoName.Text"), objModule.ModuleID.ToString()));
                }
            }
            this.cboModuleList.Items.Insert(0, new ListItem("---"));

            this.cboModuleView.Items.Add(new ListItem(this.GetLocalizedString("cboModuleView.Items.Comments"), "comments"));
            this.cboModuleView.Items.Add(new ListItem(this.GetLocalizedString("cboModuleView.Items.Combination"), "combination"));
            this.cboModuleView.Items.Add(new ListItem(this.GetLocalizedString("cboModuleView.Items.RecentComments"), "recent-comments"));
            this.cboModuleView.Items.Add(new ListItem(this.GetLocalizedString("cboModuleView.Items.PopularThreads"), "popular-threads"));
            this.cboModuleView.Items.Add(new ListItem(this.GetLocalizedString("cboModuleView.Items.TopCommenters"), "top-commenters"));

            for(int i = 1; i < 21; i++)
            {
                this.cboDisplayItems.Items.Add(new ListItem(i.ToString()));
            }

            this.cboColorTheme.Items.Add(new ListItem(this.GetLocalizedString("cboColorTheme.Items.Blue"), "blue"));
            this.cboColorTheme.Items.Add(new ListItem(this.GetLocalizedString("cboColorTheme.Items.Grey"), "grey"));
            this.cboColorTheme.Items.Add(new ListItem(this.GetLocalizedString("cboColorTheme.Items.Green"), "green"));
            this.cboColorTheme.Items.Add(new ListItem(this.GetLocalizedString("cboColorTheme.Items.Red"), "red"));
            this.cboColorTheme.Items.Add(new ListItem(this.GetLocalizedString("cboColorTheme.Items.Orange"), "orange"));

            this.cboDefaultTab.Items.Add(new ListItem(this.GetLocalizedString("cboDefaultTab.Items.People"), "people"));
            this.cboDefaultTab.Items.Add(new ListItem(this.GetLocalizedString("cboDefaultTab.Items.Recent"), "recent"));
            this.cboDefaultTab.Items.Add(new ListItem(this.GetLocalizedString("cboDefaultTab.Items.Popular"), "popular"));

            this.cboAvatarSize.Items.Add(new ListItem(this.GetLocalizedString("cboAvatarSize.Items.Small"), "24"));
            this.cboAvatarSize.Items.Add(new ListItem(this.GetLocalizedString("cboAvatarSize.Items.Medium"), "32"));
            this.cboAvatarSize.Items.Add(new ListItem(this.GetLocalizedString("cboAvatarSize.Items.Large"), "48"));
            this.cboAvatarSize.Items.Add(new ListItem(this.GetLocalizedString("cboAvatarSize.Items.XLarge"), "92"));
            this.cboAvatarSize.Items.Add(new ListItem(this.GetLocalizedString("cboAvatarSize.Items.Ginormous"), "128"));

            this.cmdReceiverFile.Visible = (!this.DoesReceiverFileExist() || string.IsNullOrEmpty(DisqusApplicationName) == false);
            this.pnlReceiverFile.Visible = (this.DoesReceiverFileExist());

            if (this.pnlReceiverFile.Visible)
            {
                this.lblReceiverFile.Text = this.Page.ResolveUrl(this.CrossDomainReceiverUrlPath());
            }

            pnlHost.Visible = UserInfo.IsSuperUser;

            this.LoadSettings();

            this.LoadFormFields();
        }

        private void LocalizeModule()
        {
            this.rfvModuleList.ErrorMessage = this.GetLocalizedString("rfvModuleList.ErrorMessage");
            this.rfvAppName.ErrorMessage = this.GetLocalizedString("rfvAppName.ErrorMessage");
            this.cmdSave.Text = this.GetLocalizedString("cmdSave.Text");
            this.cmdReturn.Text = this.GetLocalizedString("cmdReturn.Text");
            this.cmdReceiverFile.Text = this.GetLocalizedString("cmdReceiverFile.Text");
            rfvApiSecret.ErrorMessage = GetLocalizedString("rfvApiSecret.ErrorMessage");
        }

        private void LoadSettings()
        {
            if (this.Settings["AttachedModuleId"] != null)
                this.AttachedModule = int.Parse(this.Settings["AttachedModuleId"].ToString(), NumberStyles.Integer);

            if (this.Settings["DisqusView"] != null)
                this.DisqusView = this.Settings["DisqusView"].ToString();

            if (this.Settings["DisplayItems"] != null)
                this.DisplayItems = int.Parse(this.Settings["DisplayItems"].ToString(), NumberStyles.Integer);

            if (this.Settings["ShowModerators"] != null)
                this.ShowModerators = bool.Parse(this.Settings["ShowModerators"].ToString());

            if (this.Settings["ColorTheme"] != null)
                this.ColorTheme = this.Settings["ColorTheme"].ToString();

            if (this.Settings["DefaultTab"] != null)
                this.DefaultTab = this.Settings["DefaultTab"].ToString();

            if (this.Settings["CommentLength"] != null)
                this.CommentLength = int.Parse(this.Settings["CommentLength"].ToString(), NumberStyles.Integer);

            if (this.Settings["ShowAvatar"] != null)
                this.ShowAvatar = bool.Parse(this.Settings["ShowAvatar"].ToString());

            if (this.Settings["AvatarSize"] != null)
                this.AvatarSize = int.Parse(this.Settings["AvatarSize"].ToString(), NumberStyles.Integer);
        }

        private void LoadFormFields()
        {
            try
            {
                if (this.AttachedModule > 0)
                    this.cboModuleList.Items.FindByValue(this.AttachedModule.ToString()).Selected = true;
            }
            catch
            {
                ModuleController ctlModule = new ModuleController();
                ctlModule.DeleteModuleSetting(this.ModuleId, "AttachedModuleId");
                ModuleController.SynchronizeModule(this.ModuleId);
                this.cboModuleList.SelectedIndex = 0;
            }

            this.txtAppName.Text = DisqusApplicationName;
            chkRequireDnnLogin.Checked = RequireDnnLogin;

            if (string.IsNullOrEmpty(this.DisqusView))
            {
                this.cboModuleView.Items.FindByValue("comments").Selected = true;
                this.DisqusView = "comments";
            }
            else
            {
                this.cboModuleView.Items.FindByValue(this.DisqusView).Selected = true;
            }

            if (this.DisplayItems > 0)
            {
                this.cboDisplayItems.Items.FindByValue(this.DisplayItems.ToString()).Selected = true;
            }
            else
            {
                this.cboDisplayItems.Items[0].Selected = true;
            }

            this.chkShowModerators.Checked = this.ShowModerators;

            if (string.IsNullOrEmpty(this.ColorTheme))
            {
                this.cboColorTheme.Items.FindByValue("blue").Selected = true;
            }
            else
            {
                this.cboColorTheme.Items.FindByValue(this.ColorTheme).Selected = true;
            }

            if (string.IsNullOrEmpty(this.DefaultTab))
            {
                this.cboDefaultTab.Items.FindByValue("people").Selected = true;
            }
            else
            {
                this.cboDefaultTab.Items.FindByValue(this.DefaultTab).Selected = true;
            }

            if (this.CommentLength > 0)
            {
                this.txtCommentLength.Text = this.CommentLength.ToString();
            }
            else
            {
                this.txtCommentLength.Text = "200";
            }

            this.chkShowAvatar.Checked = this.ShowAvatar;

            if (this.AvatarSize > 0)
            {
                this.cboAvatarSize.Items.FindByValue(this.AvatarSize.ToString()).Selected = true;
            }
            else
            {
                this.cboAvatarSize.Items[0].Selected = true;
            }

            /* SITE-LEVEL SETTINGS */

            if (string.IsNullOrEmpty(DisqusApiSecret) == false)
            {
                txtApiSecret.Text = DisqusApiSecret;
            }

            if (UserInfo.IsSuperUser)
            {
                chkSchedule.Checked = ScheduleEnabled;
            }

            chkDeveloperMode.Checked = DisqusDeveloperMode;

            chkSsoEnabled.Checked = DisqusSsoEnabled;

            txtSsoApiKey.Text = DisqusSsoApiKey;

            this.ChangeView(this.DisqusView);
        }

        private void SaveSettings()
        {
            ModuleController ctlModule = new ModuleController();
            var sec = new Security.PortalSecurity();
            ctlModule.DeleteModuleSettings(this.ModuleId);
            ctlModule.DeleteTabModuleSettings(this.TabModuleId);

            Entities.Portals.PortalController.UpdatePortalSetting(PortalId, "wnsDisqusApplicationName", sec.InputFilter(txtAppName.Text, PortalSecurity.FilterFlag.NoMarkup), true, PortalSettings.CultureCode);
            Entities.Portals.PortalController.UpdatePortalSetting(PortalId, "wnsDisqusApiSecret", sec.InputFilter(txtApiSecret.Text, PortalSecurity.FilterFlag.NoMarkup), true, PortalSettings.CultureCode);
            Entities.Portals.PortalController.UpdatePortalSetting(PortalId, "wnsDisqusRequireDnnLogin", chkRequireDnnLogin.Checked.ToString(), true, PortalSettings.CultureCode);
            Entities.Portals.PortalController.UpdatePortalSetting(PortalId, "wnsDisqusDeveloperMode", chkDeveloperMode.Checked.ToString(), true, PortalSettings.CultureCode);
            Entities.Portals.PortalController.UpdatePortalSetting(PortalId, "wnsDisqusSsoEnabled", chkSsoEnabled.Checked.ToString(), true);
            Entities.Portals.PortalController.UpdatePortalSetting(PortalId, "wnsDisqusSsoApiKey", sec.InputFilter(txtSsoApiKey.Text, PortalSecurity.FilterFlag.NoMarkup), true);

            if (UserInfo.IsSuperUser)
            {
                // save & apply schedule preferences
                ManageSchedulerItem();
            }

            ctlModule.UpdateTabModuleSetting(this.TabModuleId, "DisqusView", this.cboModuleView.SelectedValue);

            switch (this.cboModuleView.SelectedValue)
            {
                case "comments":
                    ctlModule.UpdateTabModuleSetting(this.TabModuleId, "AttachedModuleId", this.cboModuleList.SelectedValue);
                    break;
                case "combination":
                    ctlModule.UpdateTabModuleSetting(this.TabModuleId, "DisplayItems", this.cboDisplayItems.SelectedValue);
                    ctlModule.UpdateTabModuleSetting(this.TabModuleId, "ShowModerators", this.chkShowModerators.Checked.ToString());
                    ctlModule.UpdateTabModuleSetting(this.TabModuleId, "ColorTheme", this.cboColorTheme.SelectedValue);
                    ctlModule.UpdateTabModuleSetting(this.TabModuleId, "DefaultTab", this.cboDefaultTab.SelectedValue);
                    ctlModule.UpdateTabModuleSetting(this.TabModuleId, "CommentLength", sec.InputFilter(this.txtCommentLength.Text, PortalSecurity.FilterFlag.NoMarkup));
                    break;
                case "recent-comments":
                    ctlModule.UpdateTabModuleSetting(this.TabModuleId, "DisplayItems", this.cboDisplayItems.SelectedValue);
                    ctlModule.UpdateTabModuleSetting(this.TabModuleId, "ShowAvatar", this.chkShowAvatar.Checked.ToString());
                    ctlModule.UpdateTabModuleSetting(this.TabModuleId, "AvatarSize", this.cboAvatarSize.SelectedValue);
                    ctlModule.UpdateTabModuleSetting(this.TabModuleId, "CommentLength", sec.InputFilter(this.txtCommentLength.Text, PortalSecurity.FilterFlag.NoMarkup));
                    break;
                case "popular-threads":
                    ctlModule.UpdateTabModuleSetting(this.TabModuleId, "DisplayItems", this.cboDisplayItems.SelectedValue);
                    break;
                case "top-commenters":
                    ctlModule.UpdateTabModuleSetting(this.TabModuleId, "DisplayItems", this.cboDisplayItems.SelectedValue);
                    ctlModule.UpdateTabModuleSetting(this.TabModuleId, "ShowModerators", this.chkShowModerators.Checked.ToString());
                    ctlModule.UpdateTabModuleSetting(this.TabModuleId, "ShowAvatar", this.chkShowAvatar.Checked.ToString());
                    ctlModule.UpdateTabModuleSetting(this.TabModuleId, "AvatarSize", this.cboAvatarSize.SelectedValue);
                    break;
            }

            ModuleController.SynchronizeModule(this.ModuleId);
        }

        private void SendBackToModule()
        {
            Response.Redirect(Globals.NavigateURL());
        }

        private void ChangeView(string newView)
        {
            switch (newView)
            {
                case "comments":
                    this.liModuleList.Visible = true;
                    this.liDisplayItems.Visible = false;
                    this.liShowModerators.Visible = false;
                    this.liColorTheme.Visible = false;
                    this.liDefaultTab.Visible = false;
                    this.liCommentLength.Visible = false;
                    this.liShowAvatar.Visible = false;
                    this.liAvatarSize.Visible = false;
                    break;
                case "combination":
                    this.liModuleList.Visible = false;
                    this.liDisplayItems.Visible = true;
                    this.liShowModerators.Visible = true;
                    this.liColorTheme.Visible = true;
                    this.liDefaultTab.Visible = true;
                    this.liCommentLength.Visible = true;
                    this.liShowAvatar.Visible = false;
                    this.liAvatarSize.Visible = false;
                    break;
                case "recent-comments":
                    this.liModuleList.Visible = false;
                    this.liDisplayItems.Visible = true;
                    this.liShowModerators.Visible = false;
                    this.liColorTheme.Visible = false;
                    this.liDefaultTab.Visible = false;
                    this.liCommentLength.Visible = true;
                    this.liShowAvatar.Visible = true;
                    this.liAvatarSize.Visible = true;
                    break;
                case "popular-threads":
                    this.liModuleList.Visible = false;
                    this.liDisplayItems.Visible = true;
                    this.liShowModerators.Visible = false;
                    this.liColorTheme.Visible = false;
                    this.liDefaultTab.Visible = false;
                    this.liCommentLength.Visible = false;
                    this.liShowAvatar.Visible = false;
                    this.liAvatarSize.Visible = false;
                    break;
                case "top-commenters":
                    this.liModuleList.Visible = false;
                    this.liDisplayItems.Visible = true;
                    this.liShowModerators.Visible = true;
                    this.liColorTheme.Visible = false;
                    this.liDefaultTab.Visible = false;
                    this.liCommentLength.Visible = false;
                    this.liShowAvatar.Visible = true;
                    this.liAvatarSize.Visible = true;
                    break;
            }
        }

        #region Cross Domain Receiver File

        private void CreateReceiverFile()
        {
            if (this.DoesReceiverFileExist() == false)
            {
                var strPath = CrossDomainReceiverFilePath();
                File.Create(strPath);
                Common.Utilities.Config.Touch();
                this.BindData();
            }
        }

        private bool DoesReceiverFileExist()
        {
            var strPath = CrossDomainReceiverFilePath();
            return File.Exists(strPath);
        }

        private string CrossDomainReceiverFilePath()
        {
            var strPath = string.Concat(this.PortalSettings.HomeDirectoryMapPath, RECEIVER_FILE_NAME);
            return strPath.Replace("\"", string.Empty);
        }

        private string CrossDomainReceiverUrlPath()
        {
            var strPath = string.Concat("http://", this.PortalSettings.PortalAlias.HTTPAlias, this.PortalSettings.HomeDirectory, RECEIVER_FILE_NAME);
            return string.Format(URL_FORMAT, strPath);
        }

        #endregion

        private ScheduleItem CreateScheduleItem()
        {
            var scheduleItem = new ScheduleItem();
            scheduleItem.TypeFullName = FeatureController.DISQUS_COMMENT_SCHEDULE_TYPE;
            scheduleItem.FriendlyName = FeatureController.DISQUS_COMMENT_SCHEDULE_NAME;
            scheduleItem.TimeLapse = 1;
            scheduleItem.TimeLapseMeasurement = "d"; // d == days
            scheduleItem.RetryTimeLapse = 1;
            scheduleItem.RetryTimeLapseMeasurement = "h"; // h == hours
            scheduleItem.RetainHistoryNum = 10;
            scheduleItem.AttachToEvent = string.Empty;
            scheduleItem.CatchUpEnabled = false;
            scheduleItem.Enabled = true;
            scheduleItem.ObjectDependencies = string.Empty;
            scheduleItem.Servers = string.Empty;
            return scheduleItem;
        }

        private void ManageSchedulerItem()
        {
            try
            {

                // update the host setting
                HostController.Instance.Update(FeatureController.HOST_SETTING_COMMENT_SCHEDULE, chkSchedule.Checked.ToString());
            
                // get an instance of the scheduler item
                ScheduleItem oSchedule = SchedulingProvider.Instance().GetSchedule(FeatureController.DISQUS_COMMENT_SCHEDULE_NAME, string.Empty);

                if (chkSchedule.Checked)
                {
                    if (oSchedule == null)
                    {
                        // there isn't a scheduler item
                        // create and enable one
                        oSchedule = CreateScheduleItem();
                        SchedulingProvider.Instance().AddSchedule(oSchedule);
                    }
                    else
                    {
                        // there is a scheduler item
                        // enable it
                        oSchedule.Enabled = true;
                        SchedulingProvider.Instance().UpdateSchedule(oSchedule);
                    }
                }
                else
                {
                    if (oSchedule != null)
                    {
                        // be sure to disable the scheduler if it exists
                        oSchedule.Enabled = false;
                        SchedulingProvider.Instance().UpdateSchedule(oSchedule);
                    }
                }

                if (SchedulingProvider.SchedulerMode == SchedulerMode.TIMER_METHOD)
                {
                    SchedulingProvider.Instance().ReStart("Change made to schedule.");
                }

            }
            catch (Exception ex)
            {
                Exceptions.LogException(ex);
            }
        }

        /*
        private bool DoesBlogExist()
        {
            var oModule = DesktopModuleController.GetDesktopModuleByModuleName("Blog", this.PortalId);
            return (oModule != null);
        }
        */

        #endregion

    }

}