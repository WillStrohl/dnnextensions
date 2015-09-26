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
using DotNetNuke.Entities.Modules;
using DotNetNuke.Framework;
using DotNetNuke.Framework.JavaScriptLibraries;
using DotNetNuke.Services.Localization;
using DotNetNuke.Web.Client;
using DotNetNuke.Web.Client.ClientResourceManagement;
using DotNetNuke.Web.Client.Providers;

namespace WillStrohl.Modules.CodeCamp
{
    public class CodeCampModuleBase : PortalModuleBase
    {
        #region Properties

        protected string DefaultView
        {
            get
            {
                var view = Settings[Components.Globals.SETTINGS_VIEW];

                return view?.ToString() ?? string.Empty;
            }
        }

        protected internal string SelectedControl { get; set; }

        #endregion

        #region Event Handlers

        protected void Page_Init(object sender, EventArgs e)
        {
            ServicesFramework.Instance.RequestAjaxAntiForgerySupport();

            JavaScript.RequestRegistration(CommonJs.DnnPlugins);

#if DEBUG
            ClientResourceManager.RegisterScript(this.Page, "http://ajax.googleapis.com/ajax/libs/angularjs/1.4.6/angular.js", FileOrder.Js.DefaultPriority, DnnFormBottomProvider.DefaultName);
            ClientResourceManager.RegisterScript(this.Page, "https://ajax.googleapis.com/ajax/libs/angularjs/1.4.6/angular-route.js", FileOrder.Js.DefaultPriority + 1, DnnFormBottomProvider.DefaultName);
            ClientResourceManager.RegisterScript(this.Page, "https://ajax.googleapis.com/ajax/libs/angularjs/1.4.6/angular-resource.js", FileOrder.Js.DefaultPriority + 2, DnnFormBottomProvider.DefaultName);
#else
            ClientResourceManager.RegisterScript(this.Page, "http://ajax.googleapis.com/ajax/libs/angularjs/1.4.6/angular.min.js", FileOrder.Js.DefaultPriority, DnnFormBottomProvider.DefaultName);
            ClientResourceManager.RegisterScript(this.Page, "https://ajax.googleapis.com/ajax/libs/angularjs/1.4.6/angular-route.min.js", FileOrder.Js.DefaultPriority + 1, DnnFormBottomProvider.DefaultName);
            ClientResourceManager.RegisterScript(this.Page, "https://ajax.googleapis.com/ajax/libs/angularjs/1.4.6/angular-resource.min.js", FileOrder.Js.DefaultPriority + 2, DnnFormBottomProvider.DefaultName);
#endif

            ClientResourceManager.RegisterScript(this.Page, "~/DesktopModules/CodeCamp/Scripts/ng/app.js", FileOrder.Js.DefaultPriority + 3, DnnFormBottomProvider.DefaultName);
            ClientResourceManager.RegisterScript(this.Page, "~/DesktopModules/CodeCamp/Scripts/ng/factories/codeCampFactory.js", FileOrder.Js.DefaultPriority + 4, DnnFormBottomProvider.DefaultName);
            ClientResourceManager.RegisterScript(this.Page, "~/DesktopModules/CodeCamp/Scripts/ng/controllers/AboutController.js", FileOrder.Js.DefaultPriority + 5, DnnFormBottomProvider.DefaultName);
            ClientResourceManager.RegisterScript(this.Page, "~/DesktopModules/CodeCamp/Scripts/ng/controllers/AgendaController.js", FileOrder.Js.DefaultPriority + 6, DnnFormBottomProvider.DefaultName);
            ClientResourceManager.RegisterScript(this.Page, "~/DesktopModules/CodeCamp/Scripts/ng/controllers/CreateController.js", FileOrder.Js.DefaultPriority + 7, DnnFormBottomProvider.DefaultName);
            ClientResourceManager.RegisterScript(this.Page, "~/DesktopModules/CodeCamp/Scripts/ng/controllers/RegisterController.js", FileOrder.Js.DefaultPriority + 8, DnnFormBottomProvider.DefaultName);
            ClientResourceManager.RegisterScript(this.Page, "~/DesktopModules/CodeCamp/Scripts/ng/controllers/SessionController.js", FileOrder.Js.DefaultPriority + 9, DnnFormBottomProvider.DefaultName);
            ClientResourceManager.RegisterScript(this.Page, "~/DesktopModules/CodeCamp/Scripts/ng/controllers/SessionsController.js", FileOrder.Js.DefaultPriority + 10, DnnFormBottomProvider.DefaultName);
            ClientResourceManager.RegisterScript(this.Page, "~/DesktopModules/CodeCamp/Scripts/ng/controllers/SpeakerController.js", FileOrder.Js.DefaultPriority + 11, DnnFormBottomProvider.DefaultName);
            ClientResourceManager.RegisterScript(this.Page, "~/DesktopModules/CodeCamp/Scripts/ng/controllers/SpeakersController.js", FileOrder.Js.DefaultPriority + 12, DnnFormBottomProvider.DefaultName);
            ClientResourceManager.RegisterScript(this.Page, "~/DesktopModules/CodeCamp/Scripts/ng/controllers/SettingsController.js", FileOrder.Js.DefaultPriority + 13, DnnFormBottomProvider.DefaultName);
        }

        #endregion

        #region Localization

        protected string GetLocalizedString(string Key)
        {
            return GetLocalizedString(Key, Components.Globals.LOCALIZATION_FILE_PATH);
        }

        protected string GetLocalizedString(string Key, string LocalizationFilePath)
        {
            return Localization.GetString(Key, Components.Globals.LOCALIZATION_FILE_PATH);
        }

        #endregion
    }
}