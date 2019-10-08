/*
  * Copyright (c) 2011-2019, Will Strohl
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
using DotNetNuke.Services.Localization;

namespace DotNetNuke.Modules.WillStrohlDisqus
{

    /// <summary>
    /// A base class for all module views to use
    /// </summary>
    public class WillStrohlDisqusModuleSettingsBase : Entities.Modules.ModuleSettingsBase
    {

        #region Private Members

        

        #endregion

        #region Properties

        

        #endregion

        #region Localization

        /// <summary>
        /// GetLocalizedString - A shortcut to localizing a string object
        /// </summary>
        /// <param name="localizationKey">a unique string key representing the localization value</param>
        /// <returns></returns>
        protected string GetLocalizedString(string localizationKey)
        {
            if (!string.IsNullOrEmpty(localizationKey))
            {
                return Localization.GetString(localizationKey, this.LocalResourceFile);
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// GetLocalizedString - A shortcut to localizing a string object
        /// </summary>
        /// <param name="localizationKey">a unique string key representing the localization value</param>
        /// <param name="localResourceFilePath">the path to the localization file</param>
        /// <returns></returns>
        protected string GetLocalizedString(string localizationKey, string localResourceFilePath)
        {
            if (!string.IsNullOrEmpty(localizationKey))
            {
                return Localization.GetString(localizationKey, localResourceFilePath);
            }
            else
            {
                return string.Empty;
            }
        }

        #endregion

        #region Event Handlers

        private void InitializeComponent()
        {
            this.Load += new System.EventHandler(this.PageLoad);
        }

        private void PageLoad(object sender, EventArgs e)
        {
            // request that the DNN framework load the jQuery script into the markup
            DotNetNuke.Framework.jQuery.RequestDnnPluginsRegistration();
        }

        #endregion

    }

}
