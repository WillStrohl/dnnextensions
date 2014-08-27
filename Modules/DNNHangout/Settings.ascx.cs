/*
' Copyright (c) 2014 Will Strohl
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
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Portals;
using DotNetNuke.Security;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Localization;
using WillStrohl.Modules.DNNHangout.Components;

namespace WillStrohl.Modules.DNNHangout
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
    /// Because the control inherits from UGLabsMetaDataSettingsBase you have access to any custom properties
    /// defined there, as well as properties from DNN such as PortalId, ModuleId, TabId, UserId and many more.
    /// </summary>
    /// -----------------------------------------------------------------------------
    public partial class Settings : DNNHangoutModuleSettingsBase
    {
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
                LocalizeModule();

                if (!Page.IsPostBack)
                {

                    if (Settings.Contains(DNNHangoutController.SETTINGS_TEMPLATE))
                    {
                        txtTemplate.Text = Settings[DNNHangoutController.SETTINGS_TEMPLATE].ToString();
                    }
                    else
                    {
                        txtTemplate.Text = Localization.GetString("DefaultTemplate.Text", LocalResourceFile);
                    }

                    if (Settings.Contains(DNNHangoutController.SETTINGS_TEMPLATE_SCOPE))
                    {
                        chkTemplateScope.Checked = bool.Parse(Settings[DNNHangoutController.SETTINGS_TEMPLATE_SCOPE].ToString());
                    }
                    else
                    {
                        chkTemplateScope.Checked = true;
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
                var ctlModule = new ModuleController();
                var sec = new PortalSecurity();
                var template = sec.InputFilter(txtTemplate.Text.Trim(), PortalSecurity.FilterFlag.NoSQL);

                if (chkTemplateScope.Checked)
                {
                    ctlModule.UpdateTabModuleSetting(TabModuleId, DNNHangoutController.SETTINGS_TEMPLATE, template);
                }
                else
                {
                    ctlModule.UpdateModuleSetting(ModuleId, DNNHangoutController.SETTINGS_TEMPLATE, template);
                }

                // clear any cached hangouts
                DataCache.ClearCache("WillStrohl.DNNHangout");

                // synchronize the module settings
                ModuleController.SynchronizeModule(ModuleId);
            }
            catch (Exception exc) //Module failed to load
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        #endregion

        private void LocalizeModule()
        {

            Localization.GetString("lnkRestore.Text", LocalResourceFile);

        }

        protected void lnkRestore_OnClick(object sender, EventArgs e)
        {
            txtTemplate.Text = Localization.GetString("DefaultTemplate.Text", LocalResourceFile);
        }
    }
}