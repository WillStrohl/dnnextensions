/*
 * Copyright (c) 2016, Will Strohl
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

namespace WillStrohl.Modules.CodeCamp.Components
{
    public class Globals
    {
        public const string LOCALIZATION_FILE_PATH = "/DesktopModules/CodeCamp/App_LocalResources/SharedResources.resx";

        public const string VIEW_PATH = "Views/";
        public const string VIEW_EXTENSION = ".ascx";

        public const string SETTINGS_VIEW = "View";
        public const string SETTINGS_BOOTSTRAP = "Bootstrap";
        public const string SETTINGS_USECDN = "UseCdn";

        public const string VIEW_SETTINGS = "SettingsView";
        public const string VIEW_CODECAMP = "CodeCampView";

        public const string RESPONSE_SUCCESS = "Success";
        public const string RESPONSE_FAILURE = "Failure";

        public const string TASKSTATE_OPEN = "open";
        public const string TASKSTATE_CLOSED = "closed";
        public const string TASKSTATE_OVERDUE = "overdue";

        public const string SPACE = " ";

        public static DateTime NULL_DATE => DateTime.Parse("1/1/1753 12:00:00 AM");
    }
}