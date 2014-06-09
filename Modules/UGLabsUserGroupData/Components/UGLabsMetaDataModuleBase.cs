/*
' Copyright (c) 2013  DNN Corp.
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
using DotNetNuke.Entities.Modules;
using DotNetNuke.Framework.JavaScriptLibraries;
using DotNetNuke.Services.Localization;

namespace DNNCommunity.Modules.UGLabsUserGroupData
{
    public class UGLabsMetaDataModuleBase : PortalModuleBase
    {

        #region Event Handlers

        private void InitializeComponent()
        {
            this.Load += new System.EventHandler(this.PageLoad);
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