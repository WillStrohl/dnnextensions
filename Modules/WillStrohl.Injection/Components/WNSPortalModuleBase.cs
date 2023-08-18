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

using DotNetNuke.Services.Localization;
using System;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Framework.JavaScriptLibraries;

namespace WillStrohl.Modules.Injection.Components
{
    public abstract class WNSPortalModuleBase : PortalModuleBase
    {
        #region Localization
        protected string GetLocalizedString(string LocalizationKey)
        {
            if (!string.IsNullOrEmpty(LocalizationKey))
            {
                return Localization.GetString(LocalizationKey, this.LocalResourceFile);
            }
            else
            {
                return string.Empty;
            }
        }

        protected string GetLocalizedString(string LocalizationKey, string LocalResourceFilePath)
        {
            if (!string.IsNullOrEmpty(LocalizationKey))
            {
                return Localization.GetString(LocalizationKey, LocalResourceFilePath);
            }
            else
            {
                return string.Empty;
            }
        }
        #endregion

        #region Script Blocks

        protected string GetClientScriptBlock(string Script)
        {
            var strScriptBlock = "<script language=\"javascript\" type=\"text/javascript\">/*<![CDATA[*/ {0} /*]]*/></script>";

            if (!string.IsNullOrEmpty(Script))
            {
                return string.Format(strScriptBlock, Script);
            }
            else
            {
                return string.Empty;
            }
        }

        protected string GetClientScript(string ScriptPath)
        {
            var strScript = "<script language=\"javascript\" type=\"text/javascript\" src=\"{0}\"></script>";

            if (!string.IsNullOrEmpty(ScriptPath))
            {
                return string.Format(strScript, ScriptPath);
            }
            else
            {
                return string.Empty;
            }
        }

        #endregion

        #region Event Handlers
        protected WNSPortalModuleBase()
        {
            Load += Page_Load;
        }

        private void Page_Load(object sender, EventArgs e)
        {
            // request that the DNN framework load the jQuery script into the markup
            JavaScript.RequestRegistration(CommonJs.DnnPlugins);

        }
        #endregion

        #region Security

        protected bool CurrentUserCanEdit
        {
            get
            {
                return (IsEditable && PortalSettings.UserMode == DotNetNuke.Entities.Portals.PortalSettings.Mode.Edit);
            }
        }

        #endregion
    }
}