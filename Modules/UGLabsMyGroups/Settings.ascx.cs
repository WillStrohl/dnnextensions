/*
' Copyright (c) 2012-2014  Will Strohl
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
using System.Web.UI.WebControls;
using DNNCommunity.Modules.MyGroups.Components;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Modules.Definitions;
using DotNetNuke.Entities.Tabs;
using DotNetNuke.Services.Exceptions;

namespace DNNCommunity.Modules.MyGroups
{
    /// -----------------------------------------------------------------------------
    /// <summary>
    /// The Settings class manages Module Settings
    /// 
    /// Typically your settings control would be used to manage settings for your module.
    /// There are two types of settings, ModuleSettings, and TabModuleSettings.
    /// 
    /// ModuleSettings apply to all "copies" of a module on a site, no matter which page the module is on. 
    /// 
    /// TabModuleSettings apply only to the current module on the current page, if you copy that module to
    /// another page the settings are not transferred.
    /// 
    /// If you happen to save both TabModuleSettings and ModuleSettings, TabModuleSettings overrides ModuleSettings.
    /// 
    /// Below we have some examples of how to access these settings but you will need to uncomment to use.
    /// 
    /// Because the control inherits from MyGroupsSettingsBase you have access to any custom properties
    /// defined there, as well as properties from DNN such as PortalId, ModuleId, TabId, UserId and many more.
    /// </summary>
    /// -----------------------------------------------------------------------------
    public partial class Settings : MyGroupsModuleSettingsBase
    {

        #region Event Handlers

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                // do nothing
            }
            catch (Exception exc) //Module failed to load
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        #endregion

        #region Helper Methods

        private void BindData()
        {
            LocalizeModule();
            
            // load the pages that have social group modules on them
            var mc = new ModuleController();
            var tc = new TabController();
            TabInfo tabInfo;

            foreach (ModuleInfo moduleInfo in mc.GetModules(PortalId))
            {

                if (moduleInfo.DesktopModule.ModuleName.Contains("Social Groups") && moduleInfo.IsDeleted == false)
                {
                    tabInfo = tc.GetTab(moduleInfo.TabID, PortalId, false);
                    if (tabInfo != null)
                    {
                        if (tabInfo.IsDeleted == false)
                        {

                            foreach (KeyValuePair<string, ModuleDefinitionInfo> def in moduleInfo.DesktopModule.ModuleDefinitions)
                            {
                                if (moduleInfo.ModuleDefinition.FriendlyName == def.Key)
                                {

                                    if (ddlGroupViewPage.Items.FindByValue(tabInfo.TabID.ToString()) == null)
                                    {
                                        ddlGroupViewPage.Items.Add(new ListItem(tabInfo.TabName + " - " + def.Key, tabInfo.TabID.ToString()));
                                    }
                                        
                                }

                            }
                        }
                        
                    }
                }
            }

            // insert a default choice for usability
            ddlGroupViewPage.Items.Insert(0, new ListItem(GetLocalizedString("ddlGroupViewPage.Items.Default")));

            // if there is more than one item in the dropdown list, enable it
            ToggleGroupDetailsList((ddlGroupViewPage.Items.Count == 1));
        }

        private void LocalizeModule()
        {
            rfvGroupViewPage.ErrorMessage = GetLocalizedString("rfvGroupViewPage.ErrorMessage");
            rfvGroupViewPage.InitialValue = GetLocalizedString("ddlGroupViewPage.Items.Default");
        }

        private void ToggleGroupDetailsList(bool isDisabled)
        {
            if (isDisabled)
            {
                // there are no group modules on the site
                ddlGroupViewPage.Enabled = false;
                ddlGroupViewPage.CssClass = "NormalDisabled dnnDisabled";
                rfvGroupViewPage.Enabled = false;

                divMessageWrapper.Visible = true;
                divMessage.InnerText = GetLocalizedString("NoGroupModules.ErrorMessage");
            }
            else
            {
                // there are no group modules on the site
                ddlGroupViewPage.Enabled = true;
                ddlGroupViewPage.CssClass = "NormaTextbox";
                rfvGroupViewPage.Enabled = true;

                divMessageWrapper.Visible = false;
            }
        }

        #endregion

        #region Base Method Implementations

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// LoadSettings loads the settings from the Database and displays them
        /// </summary>
        /// -----------------------------------------------------------------------------
        public override void LoadSettings()
        {
            try
            {
                if (Page.IsPostBack == false)
                {
                    // ensure the setting form is set-up before setting default values
                    BindData();

                    if (Settings.Contains(FeatureController.SETTINGKEY_PROFILETABID))
                    {
                        try
                        {
                            ddlGroupViewPage.Items.FindByValue(
                                Settings[FeatureController.SETTINGKEY_PROFILETABID].ToString()).Selected = true;
                        }
                        catch
                        {
                            ddlGroupViewPage.SelectedIndex = 0;
                        }
                    }
                }
            }
            catch (Exception exc) //Module failed to load
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// UpdateSettings saves the modified settings to the Database
        /// </summary>
        /// -----------------------------------------------------------------------------
        public override void UpdateSettings()
        {
            try
            {
                var modules = new ModuleController();

                modules.UpdateModuleSetting(ModuleId, FeatureController.SETTINGKEY_PROFILETABID, ddlGroupViewPage.SelectedValue);

                DotNetNuke.Entities.Modules.ModuleController.SynchronizeModule(ModuleId);
            }
            catch (Exception exc) //Module failed to load
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        #endregion
    }
}