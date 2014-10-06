/*
Copyright (c) 2014 Will Strohl

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated 
documentation files (the "Software"), to deal in the Software without restriction, including without limitation 
the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, 
and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions 
of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
DEALINGS IN THE SOFTWARE.
*/

using System;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Web.Client.ClientResourceManagement;

namespace WillStrohl.SkinObjects.PrismJS
{
    public partial class DemoPersona : SkinObjectModuleBase
    {

        #region Constants

        private const string SCRIPT_CORE = "Scripts/prism.core.js";
        private const string SCRIPT_DNN = "Scripts/prism.dnn.js";
        private const string SCRIPT_ALL = "Scripts/prism.all.js";

        private const string THEME_COY = "Styles/prism.coy.css";
        private const string THEME_DARK = "Styles/prism.dark.css";
        private const string THEME_DEFAULT = "Styles/prism.default.css";
        private const string THEME_FUNKY = "Styles/prism.funky.css";
        private const string THEME_OKAIDIA = "Styles/prism.okaidia.css";
        private const string THEME_TWILIGHT = "Styles/prism.twilight.css";

        #endregion

        public string Script { get; set; }
        public string Theme { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(Script))
                {
                    switch (Script.ToLower().Trim())
                    {
                        case "core":
                            ClientResourceManager.RegisterScript(Page, string.Concat(ControlPath, SCRIPT_CORE));
                            break;
                        case "all":
                            ClientResourceManager.RegisterScript(Page, string.Concat(ControlPath, SCRIPT_ALL));
                            break;
                        default:
                            ClientResourceManager.RegisterScript(Page, string.Concat(ControlPath, SCRIPT_DNN));
                            break;
                    }
                }
                else
                {
                    ClientResourceManager.RegisterScript(Page, string.Concat(ControlPath, SCRIPT_DNN));
                }

                if (!string.IsNullOrEmpty(Theme))
                {
                    switch (Theme.ToLower().Trim())
                    {
                        case "coy":
                            ClientResourceManager.RegisterStyleSheet(Page, string.Concat(ControlPath, THEME_COY));
                            break;
                        case "dark":
                            ClientResourceManager.RegisterStyleSheet(Page, string.Concat(ControlPath, THEME_DARK));
                            break;
                        case "funky":
                            ClientResourceManager.RegisterStyleSheet(Page, string.Concat(ControlPath, THEME_FUNKY));
                            break;
                        case "okaidia":
                            ClientResourceManager.RegisterStyleSheet(Page, string.Concat(ControlPath, THEME_OKAIDIA));
                            break;
                        case "twilight":
                            ClientResourceManager.RegisterStyleSheet(Page, string.Concat(ControlPath, THEME_TWILIGHT));
                            break;
                        default:
                            ClientResourceManager.RegisterStyleSheet(Page, string.Concat(ControlPath, THEME_DEFAULT));
                            break;
                    }
                }
                else
                {
                    ClientResourceManager.RegisterStyleSheet(Page, string.Concat(ControlPath, THEME_DEFAULT));
                }
            }
            catch (Exception exc) //Module failed to load
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

    }
}