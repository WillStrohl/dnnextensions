//
// Will Strohl (will.strohl@gmail.com)
// http://www.willstrohl.com
//
//Copyright (c) 2009-2016, Will Strohl
//All rights reserved.
//
//Redistribution and use in source and binary forms, with or without modification, are 
//permitted provided that the following conditions are met:
//
//Redistributions of source code must retain the above copyright notice, this list of 
//conditions and the following disclaimer.
//
//Redistributions in binary form must reproduce the above copyright notice, this list 
//of conditions and the following disclaimer in the documentation and/or other 
//materials provided with the distribution.
//
//Neither the name of Will Strohl, Content Injection, nor the names of its contributors may be 
//used to endorse or promote products derived from this software without specific prior 
//written permission.
//
//THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY 
//EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES 
//OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT 
//SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, 
//INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED 
//TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR 
//BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN 
//CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN 
//ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH 
//DAMAGE.
//

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