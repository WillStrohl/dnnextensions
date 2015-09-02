/*
' Copyright (c) 2015 Will Strohl
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
using DotNetNuke.Framework.JavaScriptLibraries;
using DotNetNuke.Services.Localization;
using DotNetNuke.Services.Exceptions;
using WillStrohl.Modules.DNNHangout.Components;
using WillStrohl.Modules.DNNHangout.Entities;

namespace WillStrohl.Modules.DNNHangout
{
    public class DNNHangoutModuleBase : PortalModuleBase
    {

        #region Private Members

        private int p_HangoutId = Null.NullInteger;

        private HangoutInfo p_Hangout = null;

        #endregion

        #region Properties

        protected int HangoutId
        {
            get
            {
                if (Request.QueryString[DNNHangoutController.SETTINGS_HANGOUT_ID] != null)
                {
                    try
                    {
                        p_HangoutId = int.Parse(Request.QueryString[DNNHangoutController.SETTINGS_HANGOUT_ID]);
                    }
                    catch (Exception ex)
                    {
                        Exceptions.LogException(ex);
                    }
                }
                else if (Settings.ContainsKey(DNNHangoutController.SETTINGS_HANGOUT_ID))
                {
                    try
                    {
                        p_HangoutId = int.Parse(Settings[DNNHangoutController.SETTINGS_HANGOUT_ID].ToString());
                    }
                    catch (Exception ex)
                    {
                        Exceptions.LogException(ex);
                    }
                }

                return p_HangoutId;
            }
        }

        protected HangoutInfo Hangout
        {
            get
            {
                if (p_Hangout == null)
                {
                    if (HangoutId > Null.NullInteger)
                    {
                        var ctlHangout = new DNNHangoutController();
                        
                        p_Hangout = ctlHangout.GetContentItem(HangoutId);
                    }
                }

                return p_Hangout;
            }
        }

        #endregion

        #region Event Handlers

        private void InitializeComponent()
        {
            this.Load += new EventHandler(this.PageLoad);
        }

        private void PageLoad(object sender, System.EventArgs e)
        {
            // request that the DNN framework load the jQuery script into the markup
            JavaScript.RequestRegistration(CommonJs.DnnPlugins);
        }

        #endregion

        #region Localization

        protected string GetLocalizedString(string Key)
        {
            return GetLocalizedString(Key, this.LocalResourceFile);
        }

        protected string GetLocalizedString(string Key, string LocalizationFilePath)
        {
            return Localization.GetString(Key, LocalizationFilePath);
        }

        #endregion

    }
}