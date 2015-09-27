/*
 * Copyright (c) 2015, Will Strohl
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
 * Neither the name of Will Strohl, nor the names of its contributors may be used 
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
using System.Web.UI.WebControls;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Services.Exceptions;

namespace WillStrohl.Modules.CodeCamp
{
    public partial class Settings : CodeCampModuleSettingsBase
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
                if (!Page.IsPostBack) BindData();

                if (Settings[Components.Globals.SETTINGS_VIEW] != null)
                {
                    ddlView.ClearSelection();
                    ddlView.Items.FindByValue(Settings[Components.Globals.SETTINGS_VIEW].ToString());
                }
                else
                {
                    ddlView.SelectedIndex = 0;
                }

                if (Settings[Components.Globals.SETTINGS_BOOTSTRAP] != null)
                {
                    chkIncludeBootstrap.Checked = bool.Parse(Settings[Components.Globals.SETTINGS_BOOTSTRAP].ToString());
                }
            }
            catch (Exception exc) //Module failed to load
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        private void BindData()
        {
            //
            // populate the form and set the defaults
            //
            ddlView.Items.Add(new ListItem() {Text = GetLocalizedString("ViewDefault"), Value = Components.Globals.VIEW_CODECAMP});
            ddlView.Items.Insert(0, new ListItem(GetLocalizedString("ChooseOne")));

            chkIncludeBootstrap.Checked = true;
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
                var controller = new ModuleController();

                controller.UpdateModuleSetting(ModuleId, Components.Globals.SETTINGS_VIEW, ddlView.SelectedIndex == 0 ? string.Empty : ddlView.SelectedValue);

                controller.UpdateModuleSetting(ModuleId, Components.Globals.SETTINGS_BOOTSTRAP, chkIncludeBootstrap.Checked.ToString());

                ModuleController.SynchronizeModule(ModuleId);
            }
            catch (Exception exc) //Module failed to load
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        #endregion
    }
}