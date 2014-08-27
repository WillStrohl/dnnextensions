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
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.UI;
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
using WillStrohl.Modules.DNNHangout.Components;
using WillStrohl.Modules.DNNHangout.Entities;

namespace WillStrohl.Modules.DNNHangout
{
    /// -----------------------------------------------------------------------------
    /// <summary>
    /// The View class displays the content
    /// 
    /// Typically your view control would be used to display content or functionality in your module.
    /// 
    /// View may be the only control you have in your project depending on the complexity of your module
    /// 
    /// Because the control inherits from DNNHangoutModuleBase you have access to any custom properties
    /// defined there, as well as properties from DNN such as PortalId, ModuleId, TabId, UserId and many more.
    /// 
    /// </summary>
    /// -----------------------------------------------------------------------------
    public partial class View : DNNHangoutModuleBase, IActionable
    {

        #region Private Members

        

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

        #endregion

        #region Helper Methods

        private void BindData()
        {
            pnlNoConfig.Visible = (HangoutId == Null.NullInteger);
            pnlHangout.Visible = !pnlNoConfig.Visible;

            if (pnlHangout.Visible)
            {
                // show the hangout
                phHangout.Controls.Add(new LiteralControl(GetTemplate()));
            }
        }

        private string GetTemplate()
        {
            var template = string.Empty;

            if (Settings.ContainsKey(DNNHangoutController.SETTINGS_TEMPLATE))
            {
                // use a saved template from settings
                template = Settings[DNNHangoutController.SETTINGS_TEMPLATE].ToString();
            }
            else
            {
                // use a default template from RESX
                template = Localization.GetString("DefaultTemplate.Text", LocalResourceFile.Replace("View", "Settings"));
            }

            // replace tokens
            var ctlHangout = new DNNHangoutController();
            template = ctlHangout.ReplaceTokens(template, Hangout, PortalSettings, ModuleId, LocalResourceFile);

            return template;
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

                if (HangoutId > Null.NullInteger)
                {
                    Actions.Add(GetNextActionID(),
                        GetLocalizedString("EditModule.Text"),
                        string.Empty,
                        string.Empty,
                        string.Empty,
                        EditUrl(DNNHangoutController.SETTINGS_HANGOUT_ID, HangoutId.ToString(), "Edit"),
                        false,
                        SecurityAccessLevel.Edit,
                        true, false);
                }
                else
                {
                    Actions.Add(GetNextActionID(),
                    GetLocalizedString("EditModule.Text"),
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    EditUrl(string.Empty, string.Empty, "Edit"),
                    false,
                    SecurityAccessLevel.Edit,
                    true, false);
                }

                return Actions;
            }
        }

        #endregion

    }
}