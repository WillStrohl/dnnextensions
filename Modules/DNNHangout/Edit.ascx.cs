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
using System.Globalization;
using System.Net;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;
using DotNetNuke.Common;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Security;
using DotNetNuke.Services.Exceptions;
using WillStrohl.Modules.DNNHangout.Components;
using WillStrohl.Modules.DNNHangout.Entities;

namespace WillStrohl.Modules.DNNHangout
{
    /// -----------------------------------------------------------------------------
    /// <summary>
    /// The EditUGLabsMetaData class is used to manage content
    /// 
    /// Typically your edit control would be used to create new content, or edit existing content within your module.
    /// The ControlKey for this control is "Edit", and is defined in the manifest (.dnn) file.
    /// 
    /// Because the control inherits from DNNHangoutModuleBase you have access to any custom properties
    /// defined there, as well as properties from DNN such as PortalId, ModuleId, TabId, UserId and many more.
    /// 
    /// </summary>
    /// -----------------------------------------------------------------------------
    public partial class Edit : DNNHangoutModuleBase
    {
        private const string DURATION_PATTERN = @"^\d+$";

        #region Event Handlers

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack) BindData();
            }
            catch (Exception exc) //Module failed to load
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        protected void lnkReturn_Click(object o, EventArgs e)
        {
            SendBackToPage();
        }

        protected void lnkUpdate_Click(object o, EventArgs e)
        {
            if (Page.IsValid)
            {
                SaveHangout();

                SendBackToPage();
            }
        }

        protected void cvDurationValidate(object source, ServerValidateEventArgs args)
        {
            var value = txtDuration.Text.Trim();

            if (!Regex.IsMatch(value, DURATION_PATTERN, RegexOptions.CultureInvariant))
            {
                args.IsValid = false;
            }

            var duration = Null.NullInteger;
            int.TryParse(value, out duration);

            if (duration == Null.NullInteger)
            {
                args.IsValid = false;
            }

            if (ddlDurationUnits.SelectedIndex == 0)
            {
                // minutes chosen
                if (duration < 1 || duration > 60)
                {
                    args.IsValid = false;
                }
            }
            else
            {
                // hours chosen
                if (duration < 1 || duration > 8)
                {
                    args.IsValid = false;
                }
            }
        }

        protected void ValidateHangoutAddress(object sender, EventArgs e)
        {
            if (IsValidHangoutAddress())
            {
                lblValidation.Text = "Yay... VALID!";
            }
            else
            {
                lblValidation.Text = "Whoops... Something is wrong!";
            }
        }

        protected void cvHangoutAddressValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = IsValidHangoutAddress();
        }

        protected void lnkDelete_Click(object sender, EventArgs e)
        {
            DeleteHangout();
            SendBackToPage();
        }

        #endregion

        #region Helper Methods

        private void BindData()
        {
            LocalizeModule();

            LoadDurationUnits();

            lnkDelete.Visible = (HangoutId > Null.NullInteger);

            if (HangoutId > Null.NullInteger)
            {
                txtDescription.Text = Hangout.Description;
                txtDuration.Text = Hangout.Duration.ToString();
                txtHangoutAddress.Text = Hangout.HangoutAddress;
                txtStartDate.SelectedDate = Hangout.StartDate;
                txtTitle.Text = Hangout.Title;

                if (Hangout.DurationUnits == DurationType.Minutes)
                {
                    ddlDurationUnits.SelectedIndex = 0;
                }
                else
                {
                    ddlDurationUnits.SelectedIndex = 1;
                }
            }
        }

        private void LoadDurationUnits()
        {
            if (ddlDurationUnits.Items.Count == 0)
            {

                ddlDurationUnits.Items.Add(new ListItem(GetLocalizedString("ddlDurationUnits.0.Text"), "0"));
                ddlDurationUnits.Items.Add(new ListItem(GetLocalizedString("ddlDurationUnits.1.Text"), "1"));

            }
        }

        private void LocalizeModule()
        {
            rfvTitle.ErrorMessage = GetLocalizedString("rfvTitle.ErrorMessage");
            rfvHangoutAddress.ErrorMessage = GetLocalizedString("rfvHangoutAddress.ErrorMessage");
            rfvStartDate.ErrorMessage = GetLocalizedString("rfvStartDate.ErrorMessage");
            rfvDuration.ErrorMessage = GetLocalizedString("rfvDuration.ErrorMessage");
            revDuration.ErrorMessage = GetLocalizedString("revDuration.ErrorMessage");
            cvDuration.ErrorMessage = GetLocalizedString("cvDuration.ErrorMessage");
            cvHangoutAddress.ErrorMessage = GetLocalizedString("cvHangoutAddress.ErrorMessage");

            lnkReturn.Text = GetLocalizedString("lnkReturn.Text");
            lnkUpdate.Text = GetLocalizedString("lnkUpdate.Text");
            lnkDelete.Text = GetLocalizedString("lnkDelete.Text");
        }

        private void SendBackToPage()
        {
            Response.Redirect(Globals.NavigateURL());
        }

        private bool IsValidHangoutAddress()
        {
            var value = txtHangoutAddress.Text.Trim();
            var result = false;

            /*
             * DNN Google Hangout URL patterns
             * 
             * BAD - https://plus.google.com/events/clhanescd7t9aibhmh236juii88
             * OK  - N6kZAEs7uQ4
             * OK  - http://www.youtube.com/watch?v=N6kZAEs7uQ4
             * OK  - http://www.youtube.com/watch?v=N6kZAEs7uQ4&feature=youtu.be
             * OK  - http://youtu.be/N6kZAEs7uQ4
             * OK  - <iframe width="420" height="315" src="http://www.youtube.com/embed/N6kZAEs7uQ4" frameborder="0" allowfullscreen></iframe>
             */

            // check youtube video id (and assign it)
            result = Regex.IsMatch(value, @"^\w{11,11}$", RegexOptions.IgnoreCase);

            // skip processing if we already have the ID
            if (result) return result;

            // clean the URL if it has additional querystring parameters
            if (Regex.IsMatch(value, @"^http(s)*://(youtu\.be|(www\.)*youtube\.com)/(watch\?v=|embed/)*(\w{11,11})&\w+", RegexOptions.IgnoreCase))
            {
                var index = value.IndexOf("&");
                value = value.Substring(0, index);
            }

            // check known URL patterns
            if (Regex.IsMatch(value, @"^http(s)*://(youtu\.be|(www\.)*youtube\.com)/(watch\?v=|embed/)*(\w{11,11})$", RegexOptions.IgnoreCase))
            {
                // matches youtube URL
                // extract video id
                txtHangoutAddress.Text = Regex.Match(value, @"^http(s)*://(youtu\.be|(www\.)*youtube\.com)/(watch\?v=|embed/)*(\w{11,11})$", RegexOptions.IgnoreCase).Groups[5].Value;
                result = true;
            }

            // iframe check
            if (Regex.IsMatch(value, @"^<iframe.+</iframe>$", RegexOptions.IgnoreCase))
            {
                // matches iframe
                if (Regex.IsMatch(value, @"http(s)*://(youtu\.be|(www\.)*youtube\.com)/(watch\?v=|embed/)*(\w{11,11})", RegexOptions.IgnoreCase))
                {
                    // extract video id
                    txtHangoutAddress.Text = Regex.Match(value, @"http(s)*://(youtu\.be|(www\.)*youtube\.com)/(watch\?v=|embed/)*(\w{11,11})", RegexOptions.IgnoreCase).Groups[5].Value;
                    result = true;
                }
            }

            //if (result)
            //{
            //    // temporarily disabled until a new way is found to validate the google hangout ID
            //    // all current methods found so far require OAuth 2.0 authenticate.
            //    // this won't be done for now, due to the added complexity required to have an editor authenticate
            //    result = IsValidVideoId(txtHangoutAddress.Text);
            //}

            return result;
        }

        private bool IsValidVideoId(string videoId)
        {
            /*
             * Verify video id using an empirical test:
             * http://gdata.youtube.com/feeds/api/videos/VIDEO_ID
             * http://gdata.youtube.com/feeds/api/videos/N6kZAEs7uQ4
             * 
             * Reference: http://webapps.stackexchange.com/questions/54443/format-for-id-of-youtube-video?newreg=54e9ac9353cd4970a968feddeaa7510a
             */

            var validationUrl = "http://gdata.youtube.com/feeds/api/videos/{0}";

            try
            {
                var request = (HttpWebRequest) WebRequest.Create(string.Format(validationUrl, videoId));

                request.Method = "HEAD";

                var response = (HttpWebResponse) request.GetResponse();

                return (response.StatusCode == HttpStatusCode.OK);
            }
            catch (WebException webEx)
            {
                return false;
            }
            catch (Exception ex)
            {
                Exceptions.LogException(ex);
                return false;
            }
        }

        #endregion

        #region Data Access

        private void SaveHangout()
        {
            var ctlHangout = new DNNHangoutController();
            var sec = new PortalSecurity();
            HangoutInfo hangout = null;

            // get an instance of the hangout (if necessary)
            hangout = Hangout ?? new HangoutInfo();

            // populate the hangout with the user field values
            hangout.Description = sec.InputFilter(txtDescription.Text.Trim(), PortalSecurity.FilterFlag.NoScripting);
            hangout.Duration = int.Parse(sec.InputFilter(txtDuration.Text.Trim(), PortalSecurity.FilterFlag.NoMarkup), NumberStyles.Integer);
            hangout.HangoutAddress = sec.InputFilter(txtHangoutAddress.Text.Trim(), PortalSecurity.FilterFlag.NoScripting);
            hangout.StartDate = txtStartDate.SelectedDate ?? DateTime.Now;
            hangout.Title = sec.InputFilter(txtTitle.Text.Trim(), PortalSecurity.FilterFlag.NoMarkup);

            // determine the units to use
            hangout.DurationUnits = ddlDurationUnits.SelectedIndex == 0 ? DurationType.Minutes : DurationType.Hours;

            var contentItemId = Null.NullInteger;

            // update or create the hangout
            contentItemId = HangoutId > Null.NullInteger ? 
                ctlHangout.UpdateContentItem(TabId, ModuleId, hangout.ContentItemId, hangout) : 
                ctlHangout.CreateContentItem(TabId, ModuleId, hangout);

            if (contentItemId <= Null.NullInteger) return;

            // update the module settings to set the default Google Hangout to show on the first page load
            var ctlModule = new ModuleController();

            ctlModule.UpdateTabModuleSetting(TabModuleId, DNNHangoutController.SETTINGS_HANGOUT_ID,contentItemId.ToString());

            ModuleController.SynchronizeModule(ModuleId);
        }

        private void DeleteHangout()
        {
            if (HangoutId > Null.NullInteger)
            {
                // delete the related content item
                var ctlHangout = new DNNHangoutController();
                ctlHangout.DeleteContentItem(HangoutId);

                // delete the module setting
                var ctlModule = new ModuleController();
                if (Settings.ContainsKey(DNNHangoutController.SETTINGS_HANGOUT_ID))
                {
                    ctlModule.DeleteTabModuleSetting(TabModuleId, DNNHangoutController.SETTINGS_HANGOUT_ID);
                }
                ModuleController.SynchronizeModule(ModuleId);
            }
        }

        #endregion
    }
}