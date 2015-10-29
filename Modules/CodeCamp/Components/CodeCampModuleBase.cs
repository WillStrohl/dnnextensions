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

        protected bool IncludeBootstrap
        {
            get
            {
                var includeBootstrap = Settings[Components.Globals.SETTINGS_BOOTSTRAP];

                return includeBootstrap == null || bool.Parse(includeBootstrap.ToString());
            }
        }

        protected internal string SelectedControl { get; set; }

        #endregion

        #region Event Handlers

        protected void Page_Init(object sender, EventArgs e)
        {
            IncludeScriptStyleDependencies();
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

        private void IncludeScriptStyleDependencies()
        {
            ServicesFramework.Instance.RequestAjaxAntiForgerySupport();

            JavaScript.RequestRegistration(CommonJs.DnnPlugins);

            var prefix = (Request.IsSecureConnection) ? "https" : "http";

            var useCdn = false;
            if (Settings["UseCdn"] != null)
            {
                useCdn = bool.Parse(Settings["UseCdn"].ToString());
            }

            if (!useCdn)
            {
                IncludeLocalFiles();
            }
            else
            {
                IncludeCdnFiles(prefix);
            }

            ClientResourceManager.RegisterScript(this.Page, "~/DesktopModules/CodeCamp/Scripts/ng/app.js", FileOrder.Js.DefaultPriority + 12, DnnFormBottomProvider.DefaultName);
            ClientResourceManager.RegisterScript(this.Page, "~/DesktopModules/CodeCamp/Scripts/Common.js", FileOrder.Js.DefaultPriority + 14, DnnFormBottomProvider.DefaultName);
            ClientResourceManager.RegisterScript(this.Page, "~/DesktopModules/CodeCamp/Scripts/ng/factories/codeCampServiceFactory.js", FileOrder.Js.DefaultPriority + 16, DnnFormBottomProvider.DefaultName);
            ClientResourceManager.RegisterScript(this.Page, "~/DesktopModules/CodeCamp/Scripts/ng/directives/dateDirectives.js", FileOrder.Js.DefaultPriority + 20, DnnFormBottomProvider.DefaultName);
            ClientResourceManager.RegisterScript(this.Page, "~/DesktopModules/CodeCamp/Scripts/ng/directives/confirmDirectives.js", FileOrder.Js.DefaultPriority + 22, DnnFormBottomProvider.DefaultName);
            ClientResourceManager.RegisterScript(this.Page, "~/DesktopModules/CodeCamp/Scripts/ng/directives/sessionDirectives.js", FileOrder.Js.DefaultPriority + 24, DnnFormBottomProvider.DefaultName);
            ClientResourceManager.RegisterScript(this.Page, "~/DesktopModules/CodeCamp/Scripts/ng/controllers/AboutController.js", FileOrder.Js.DefaultPriority + 26, DnnFormBottomProvider.DefaultName);
            ClientResourceManager.RegisterScript(this.Page, "~/DesktopModules/CodeCamp/Scripts/ng/controllers/AgendaController.js", FileOrder.Js.DefaultPriority + 28, DnnFormBottomProvider.DefaultName);
            ClientResourceManager.RegisterScript(this.Page, "~/DesktopModules/CodeCamp/Scripts/ng/controllers/EventController.js", FileOrder.Js.DefaultPriority + 30, DnnFormBottomProvider.DefaultName);
            ClientResourceManager.RegisterScript(this.Page, "~/DesktopModules/CodeCamp/Scripts/ng/controllers/NavigationController.js", FileOrder.Js.DefaultPriority + 31, DnnFormBottomProvider.DefaultName);
            ClientResourceManager.RegisterScript(this.Page, "~/DesktopModules/CodeCamp/Scripts/ng/controllers/RegisterController.js", FileOrder.Js.DefaultPriority + 32, DnnFormBottomProvider.DefaultName);
            ClientResourceManager.RegisterScript(this.Page, "~/DesktopModules/CodeCamp/Scripts/ng/controllers/RoomsController.js", FileOrder.Js.DefaultPriority + 34, DnnFormBottomProvider.DefaultName);
            ClientResourceManager.RegisterScript(this.Page, "~/DesktopModules/CodeCamp/Scripts/ng/controllers/SessionController.js", FileOrder.Js.DefaultPriority + 36, DnnFormBottomProvider.DefaultName);
            ClientResourceManager.RegisterScript(this.Page, "~/DesktopModules/CodeCamp/Scripts/ng/controllers/SessionsController.js", FileOrder.Js.DefaultPriority + 38, DnnFormBottomProvider.DefaultName);
            ClientResourceManager.RegisterScript(this.Page, "~/DesktopModules/CodeCamp/Scripts/ng/controllers/SpeakerController.js", FileOrder.Js.DefaultPriority + 40, DnnFormBottomProvider.DefaultName);
            ClientResourceManager.RegisterScript(this.Page, "~/DesktopModules/CodeCamp/Scripts/ng/controllers/SpeakersController.js", FileOrder.Js.DefaultPriority + 42, DnnFormBottomProvider.DefaultName);
            ClientResourceManager.RegisterScript(this.Page, "~/DesktopModules/CodeCamp/Scripts/ng/controllers/SettingsController.js", FileOrder.Js.DefaultPriority + 44, DnnFormBottomProvider.DefaultName);
            ClientResourceManager.RegisterScript(this.Page, "~/DesktopModules/CodeCamp/Scripts/ng/controllers/TimeSlotsController.js", FileOrder.Js.DefaultPriority + 46, DnnFormBottomProvider.DefaultName);
            ClientResourceManager.RegisterScript(this.Page, "~/DesktopModules/CodeCamp/Scripts/ng/controllers/TrackController.js", FileOrder.Js.DefaultPriority + 48, DnnFormBottomProvider.DefaultName);
            ClientResourceManager.RegisterScript(this.Page, "~/DesktopModules/CodeCamp/Scripts/ng/controllers/TracksController.js", FileOrder.Js.DefaultPriority + 50, DnnFormBottomProvider.DefaultName);
            ClientResourceManager.RegisterScript(this.Page, "~/DesktopModules/CodeCamp/Scripts/ng/controllers/VolunteersController.js", FileOrder.Js.DefaultPriority + 52, DnnFormBottomProvider.DefaultName);
            ClientResourceManager.RegisterScript(this.Page, "~/DesktopModules/CodeCamp/Scripts/ng/controllers/TestController.js", FileOrder.Js.DefaultPriority + 54, DnnFormBottomProvider.DefaultName);
        }

        private void IncludeLocalFiles()
        {
#if DEBUG
            if (IncludeBootstrap)
            {
                ClientResourceManager.RegisterStyleSheet(this.Page, "~/DesktopModules/CodeCamp/Scripts/bootstrap/3.3.5/css/bootstrap.css", FileOrder.Css.DefaultPriority);
                ClientResourceManager.RegisterStyleSheet(this.Page, "~/DesktopModules/CodeCamp/Styles/bootstrap/flat/bootstrap-flat.css", FileOrder.Css.DefaultPriority + 1);
                ClientResourceManager.RegisterStyleSheet(this.Page, "~/DesktopModules/CodeCamp/Styles/bootstrap/flat/bootstrap-flat-extras.css", FileOrder.Css.DefaultPriority + 2);
            }

            ClientResourceManager.RegisterScript(this.Page, "~/DesktopModules/CodeCamp/Scripts/ng/angular/angular.js", FileOrder.Js.DefaultPriority, DnnPageHeaderProvider.DefaultName);
            ClientResourceManager.RegisterScript(this.Page, "~/DesktopModules/CodeCamp/Scripts/ng/angular/angular-route.js", FileOrder.Js.DefaultPriority + 1, DnnFormBottomProvider.DefaultName);
            ClientResourceManager.RegisterScript(this.Page, "~/DesktopModules/CodeCamp/Scripts/ng/angular/angular-resource.js", FileOrder.Js.DefaultPriority + 2, DnnFormBottomProvider.DefaultName);
            ClientResourceManager.RegisterScript(this.Page, "~/DesktopModules/CodeCamp/Scripts/ng/angular/angular-animate.js", FileOrder.Js.DefaultPriority + 3, DnnFormBottomProvider.DefaultName);
            ClientResourceManager.RegisterScript(this.Page, "~/DesktopModules/CodeCamp/Scripts/ng-bootstrap/ui-bootstrap-tpls-0.14.3.js", FileOrder.Js.DefaultPriority + 4, DnnFormBottomProvider.DefaultName);

            ClientResourceManager.RegisterScript(this.Page, "~/DesktopModules/CodeCamp/Scripts/moment/moment.js", FileOrder.Js.DefaultPriority + 5, DnnFormBottomProvider.DefaultName);
            ClientResourceManager.RegisterScript(this.Page, "~/DesktopModules/CodeCamp/Scripts/moment/angular-moment.js", FileOrder.Js.DefaultPriority + 6, DnnFormBottomProvider.DefaultName);
            // TODO: add internationalization support for moment.js

            if (IncludeBootstrap)
            {
                ClientResourceManager.RegisterScript(this.Page, "~/DesktopModules/CodeCamp/Scripts/bootstrap/3.3.5/js/bootstrap.js", FileOrder.Js.DefaultPriority + 8, DnnFormBottomProvider.DefaultName);
                ClientResourceManager.RegisterScript(this.Page, "~/DesktopModules/CodeCamp/Styles/bootstrap/bootflat/js/icheck.min.js", FileOrder.Js.jQueryUI + 1, DnnPageHeaderProvider.DefaultName);
                ClientResourceManager.RegisterScript(this.Page, "~/DesktopModules/CodeCamp/Styles/bootstrap/bootflat/js/jquery.fs.selecter.min.js", FileOrder.Js.jQueryUI + 2, DnnPageHeaderProvider.DefaultName);
                ClientResourceManager.RegisterScript(this.Page, "~/DesktopModules/CodeCamp/Styles/bootstrap/bootflat/js/jquery.fs.stepper.min.js", FileOrder.Js.jQueryUI + 3, DnnPageHeaderProvider.DefaultName);
            }

            if (Request.Browser.Type == "IE" && Request.Browser.MajorVersion < 9)
            {
                ClientResourceManager.RegisterScript(this.Page, "~/DesktopModules/CodeCamp/Scripts/IE-Support/html5shiv.js", FileOrder.Js.DefaultPriority + 9, DnnFormBottomProvider.DefaultName);
                ClientResourceManager.RegisterScript(this.Page, "~/DesktopModules/CodeCamp/Scripts/IE-Support/respond.js", FileOrder.Js.DefaultPriority + 10, DnnFormBottomProvider.DefaultName);
            }

            ClientResourceManager.RegisterScript(this.Page, "~/DesktopModules/CodeCamp/Scripts/StackBlur/StackBlur.js", FileOrder.Js.DefaultPriority + 11, DnnFormBottomProvider.DefaultName);
#else
            if (IncludeBootstrap)
            {
                ClientResourceManager.RegisterStyleSheet(this.Page, "~/DesktopModules/CodeCamp/Scripts/bootstrap/3.3.5/css/bootstrap.min.css", FileOrder.Css.DefaultPriority);
                ClientResourceManager.RegisterStyleSheet(this.Page, "~/DesktopModules/CodeCamp/Styles/bootstrap/bootflat/css/bootstrap-flat.min.css", FileOrder.Css.DefaultPriority + 1);
                ClientResourceManager.RegisterStyleSheet(this.Page, "~/DesktopModules/CodeCamp/Styles/bootstrap/bootflat/css/bootstrap-flat-extras.min.css", FileOrder.Css.DefaultPriority + 2);
            }

            ClientResourceManager.RegisterScript(this.Page, "~/DesktopModules/CodeCamp/Scripts/ng/angular/angular.min.js", FileOrder.Js.DefaultPriority, DnnPageHeaderProvider.DefaultName);
            ClientResourceManager.RegisterScript(this.Page, "~/DesktopModules/CodeCamp/Scripts/ng/angular/angular-route.min.js", FileOrder.Js.DefaultPriority + 1, DnnFormBottomProvider.DefaultName);
            ClientResourceManager.RegisterScript(this.Page, "~/DesktopModules/CodeCamp/Scripts/ng/angular/angular-resource.min.js", FileOrder.Js.DefaultPriority + 2, DnnFormBottomProvider.DefaultName);
            ClientResourceManager.RegisterScript(this.Page, "~/DesktopModules/CodeCamp/Scripts/ng/angular/angular-animate.js", FileOrder.Js.DefaultPriority + 3, DnnFormBottomProvider.DefaultName);
            ClientResourceManager.RegisterScript(this.Page, "~/DesktopModules/CodeCamp/Scripts/ng-bootstrap/ui-bootstrap-tpls.min.js", FileOrder.Js.DefaultPriority + 4, DnnFormBottomProvider.DefaultName);
            
            ClientResourceManager.RegisterScript(this.Page, "~/DesktopModules/CodeCamp/Scripts/moment/moment.min.js", FileOrder.Js.DefaultPriority + 5, DnnFormBottomProvider.DefaultName);
            ClientResourceManager.RegisterScript(this.Page, "~/DesktopModules/CodeCamp/Scripts/moment/angular-moment.min.js", FileOrder.Js.DefaultPriority + 6, DnnFormBottomProvider.DefaultName);
            // TODO: add internationalization support for moment.js
            
            if (IncludeBootstrap)
            {
                ClientResourceManager.RegisterScript(this.Page, "~/DesktopModules/CodeCamp/Scripts/bootstrap/3.3.5/js/bootstrap.min.js", FileOrder.Js.DefaultPriority + 8, DnnFormBottomProvider.DefaultName);
                ClientResourceManager.RegisterScript(this.Page, "~/DesktopModules/CodeCamp/Styles/bootstrap/bootflat/js/icheck.min.js", FileOrder.Js.jQueryUI + 1, DnnPageHeaderProvider.DefaultName);
                ClientResourceManager.RegisterScript(this.Page, "~/DesktopModules/CodeCamp/Styles/bootstrap/bootflat/js/jquery.fs.selecter.min.js", FileOrder.Js.jQueryUI + 2, DnnPageHeaderProvider.DefaultName);
                ClientResourceManager.RegisterScript(this.Page, "~/DesktopModules/CodeCamp/Styles/bootstrap/bootflat/js/jquery.fs.stepper.min.js", FileOrder.Js.jQueryUI + 3, DnnPageHeaderProvider.DefaultName);
            }

            if (Request.Browser.Type == "IE" && Request.Browser.MajorVersion < 9)
            {
                ClientResourceManager.RegisterScript(this.Page, "~/DesktopModules/CodeCamp/Scripts/IE-Support/html5shiv.min.js", FileOrder.Js.DefaultPriority + 9, DnnFormBottomProvider.DefaultName);
                ClientResourceManager.RegisterScript(this.Page, "~/DesktopModules/CodeCamp/Scripts/IE-Support/respond.min.js", FileOrder.Js.DefaultPriority + 10, DnnFormBottomProvider.DefaultName);
            }

            ClientResourceManager.RegisterScript(this.Page, "~/DesktopModules/CodeCamp/Scripts/StackBlur/StackBlur.min.js", FileOrder.Js.DefaultPriority + 11, DnnFormBottomProvider.DefaultName);
#endif
        }

        private void IncludeCdnFiles(string prefix)
        {
#if DEBUG
            if (IncludeBootstrap)
            {
                ClientResourceManager.RegisterStyleSheet(this.Page, prefix + "://maxcdn.bootstrapcdn.com/bootstrap/3.3.5/css/bootstrap.css", FileOrder.Css.DefaultPriority);
                ClientResourceManager.RegisterStyleSheet(this.Page, "~/DesktopModules/CodeCamp/Styles/bootstrap/flat/bootstrap-flat.css", FileOrder.Css.DefaultPriority + 1);
                ClientResourceManager.RegisterStyleSheet(this.Page, "~/DesktopModules/CodeCamp/Styles/bootstrap/flat/bootstrap-flat-extras.css", FileOrder.Css.DefaultPriority + 2);
            }

            ClientResourceManager.RegisterScript(this.Page, prefix + "://ajax.googleapis.com/ajax/libs/angularjs/1.4.7/angular.js", FileOrder.Js.DefaultPriority, DnnPageHeaderProvider.DefaultName);
            ClientResourceManager.RegisterScript(this.Page, prefix + "://ajax.googleapis.com/ajax/libs/angularjs/1.4.7/angular-route.js", FileOrder.Js.DefaultPriority + 1, DnnFormBottomProvider.DefaultName);
            ClientResourceManager.RegisterScript(this.Page, prefix + "://ajax.googleapis.com/ajax/libs/angularjs/1.4.7/angular-resource.js", FileOrder.Js.DefaultPriority + 2, DnnFormBottomProvider.DefaultName);
            ClientResourceManager.RegisterScript(this.Page, prefix + "://ajax.googleapis.com/ajax/libs/angularjs/1.4.7/angular-animate.js", FileOrder.Js.DefaultPriority + 3, DnnFormBottomProvider.DefaultName);
            ClientResourceManager.RegisterScript(this.Page, prefix + "://cdnjs.cloudflare.com/ajax/libs/angular-ui-bootstrap/0.14.3/ui-bootstrap-tpls.js", FileOrder.Js.DefaultPriority + 4, DnnFormBottomProvider.DefaultName);

            ClientResourceManager.RegisterScript(this.Page, prefix + "://cdnjs.cloudflare.com/ajax/libs/moment.js/2.10.6/moment.js", FileOrder.Js.DefaultPriority + 5, DnnFormBottomProvider.DefaultName);
            ClientResourceManager.RegisterScript(this.Page, prefix + "://cdnjs.cloudflare.com/ajax/libs/angular-moment/0.10.3/angular-moment.js", FileOrder.Js.DefaultPriority + 6, DnnFormBottomProvider.DefaultName);
            // TODO: add internationalization support for moment.js

            if (IncludeBootstrap)
            {
                ClientResourceManager.RegisterScript(this.Page, prefix + "://maxcdn.bootstrapcdn.com/bootstrap/3.3.5/js/bootstrap.js", FileOrder.Js.DefaultPriority + 8, DnnFormBottomProvider.DefaultName);
                ClientResourceManager.RegisterScript(this.Page, "~/DesktopModules/CodeCamp/Styles/bootstrap/bootflat/js/icheck.min.js", FileOrder.Js.jQueryUI + 1, DnnPageHeaderProvider.DefaultName);
                ClientResourceManager.RegisterScript(this.Page, "~/DesktopModules/CodeCamp/Styles/bootstrap/bootflat/js/jquery.fs.selecter.min.js", FileOrder.Js.jQueryUI + 2, DnnPageHeaderProvider.DefaultName);
                ClientResourceManager.RegisterScript(this.Page, "~/DesktopModules/CodeCamp/Styles/bootstrap/bootflat/js/jquery.fs.stepper.min.js", FileOrder.Js.jQueryUI + 3, DnnPageHeaderProvider.DefaultName);
            }

            if (Request.Browser.Type == "IE" && Request.Browser.MajorVersion < 9)
            {
                ClientResourceManager.RegisterScript(this.Page, prefix + "://oss.maxcdn.com/html5shiv/3.7.2/html5shiv.js", FileOrder.Js.DefaultPriority + 9, DnnFormBottomProvider.DefaultName);
                ClientResourceManager.RegisterScript(this.Page, prefix + "://oss.maxcdn.com/respond/1.4.2/respond.js", FileOrder.Js.DefaultPriority + 10, DnnFormBottomProvider.DefaultName);
            }

            ClientResourceManager.RegisterScript(this.Page, "~/DesktopModules/CodeCamp/Scripts/StackBlur/StackBlur.js", FileOrder.Js.DefaultPriority + 11, DnnFormBottomProvider.DefaultName);
#else
            if (IncludeBootstrap)
            {
                ClientResourceManager.RegisterStyleSheet(this.Page, prefix + "://maxcdn.bootstrapcdn.com/bootstrap/3.3.5/css/bootstrap.min.css", FileOrder.Css.DefaultPriority);
                ClientResourceManager.RegisterStyleSheet(this.Page, "~/DesktopModules/CodeCamp/Styles/bootstrap/bootflat/css/bootstrap-flat.min.css", FileOrder.Css.DefaultPriority + 1);
                ClientResourceManager.RegisterStyleSheet(this.Page, "~/DesktopModules/CodeCamp/Styles/bootstrap/bootflat/css/bootstrap-flat-extras.min.css", FileOrder.Css.DefaultPriority + 2);
            }

            ClientResourceManager.RegisterScript(this.Page, prefix + "://ajax.googleapis.com/ajax/libs/angularjs/1.4.7/angular.min.js", FileOrder.Js.DefaultPriority, DnnPageHeaderProvider.DefaultName);
            ClientResourceManager.RegisterScript(this.Page, prefix + "://ajax.googleapis.com/ajax/libs/angularjs/1.4.7/angular-route.min.js", FileOrder.Js.DefaultPriority + 1, DnnFormBottomProvider.DefaultName);
            ClientResourceManager.RegisterScript(this.Page, prefix + "://ajax.googleapis.com/ajax/libs/angularjs/1.4.7/angular-resource.min.js", FileOrder.Js.DefaultPriority + 2, DnnFormBottomProvider.DefaultName);
            ClientResourceManager.RegisterScript(this.Page, prefix + "://ajax.googleapis.com/ajax/libs/angularjs/1.4.7/angular-animate.js", FileOrder.Js.DefaultPriority + 3, DnnFormBottomProvider.DefaultName);
            ClientResourceManager.RegisterScript(this.Page, prefix + "://cdnjs.cloudflare.com/ajax/libs/angular-ui-bootstrap/0.14.3/ui-bootstrap-tpls.min.js", FileOrder.Js.DefaultPriority + 4, DnnFormBottomProvider.DefaultName);
            
            ClientResourceManager.RegisterScript(this.Page, prefix + "://cdnjs.cloudflare.com/ajax/libs/moment.js/2.10.6/moment.min.js", FileOrder.Js.DefaultPriority + 5, DnnFormBottomProvider.DefaultName);
            ClientResourceManager.RegisterScript(this.Page, prefix + "://cdnjs.cloudflare.com/ajax/libs/angular-moment/0.10.3/angular-moment.min.js", FileOrder.Js.DefaultPriority + 6, DnnFormBottomProvider.DefaultName);
            // TODO: add internationalization support for moment.js
            
            if (IncludeBootstrap)
            {
                ClientResourceManager.RegisterScript(this.Page, prefix + "://maxcdn.bootstrapcdn.com/bootstrap/3.3.5/js/bootstrap.min.js", FileOrder.Js.DefaultPriority + 8, DnnFormBottomProvider.DefaultName);
                ClientResourceManager.RegisterScript(this.Page, "~/DesktopModules/CodeCamp/Styles/bootstrap/bootflat/js/icheck.min.js", FileOrder.Js.jQueryUI + 1, DnnPageHeaderProvider.DefaultName);
                ClientResourceManager.RegisterScript(this.Page, "~/DesktopModules/CodeCamp/Styles/bootstrap/bootflat/js/jquery.fs.selecter.min.js", FileOrder.Js.jQueryUI + 2, DnnPageHeaderProvider.DefaultName);
                ClientResourceManager.RegisterScript(this.Page, "~/DesktopModules/CodeCamp/Styles/bootstrap/bootflat/js/jquery.fs.stepper.min.js", FileOrder.Js.jQueryUI + 3, DnnPageHeaderProvider.DefaultName);
            }

            if (Request.Browser.Type == "IE" && Request.Browser.MajorVersion < 9)
            {
                ClientResourceManager.RegisterScript(this.Page, prefix + "://oss.maxcdn.com/html5shiv/3.7.2/html5shiv.min.js", FileOrder.Js.DefaultPriority + 9, DnnFormBottomProvider.DefaultName);
                ClientResourceManager.RegisterScript(this.Page, prefix + "://oss.maxcdn.com/respond/1.4.2/respond.min.js", FileOrder.Js.DefaultPriority + 10, DnnFormBottomProvider.DefaultName);
            }

            ClientResourceManager.RegisterScript(this.Page, "~/DesktopModules/CodeCamp/Scripts/StackBlur/StackBlur.min.js", FileOrder.Js.DefaultPriority + 11, DnnFormBottomProvider.DefaultName);
#endif
        }
    }
}