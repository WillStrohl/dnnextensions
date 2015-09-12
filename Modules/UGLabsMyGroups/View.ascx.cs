/*
' Copyright (c) 2012-2014  Will Strohl
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
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using DNNCommunity.Modules.MyGroups.Components;
using DotNetNuke.Common;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Users;
using DotNetNuke.Security.Roles;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.FileSystem;

namespace DNNCommunity.Modules.MyGroups
{
    /// -----------------------------------------------------------------------------
    /// <summary>
    /// The View class displays the content
    /// 
    /// Typically your view control would be used to display content or functionality in your module.
    /// 
    /// View may be the only control you have in your project depending on the complexity of your module
    /// 
    /// Because the control inherits from MyGroupsModuleBase you have access to any custom properties
    /// defined there, as well as properties from DNN such as PortalId, ModuleId, TabId, UserId and many more.
    /// 
    /// </summary>
    /// -----------------------------------------------------------------------------
    public partial class View : MyGroupsModuleBase
    {

        #region Private Members

        private const string MESSAGE_FORMAT = "<div class=\"dnnFormMessage dnnFormInfo\">{0}</div>";
        private const string MESSAGE_ERROR_FORMAT = "<div class=\"dnnFormMessage dnnFormValidationSummary\">{0}</div>";
        private const int MAX_IMAGE_HEIGHT = 80;
        private const string IMAGE_THUMBNAIL_PATTERN = @"-thumb$0";
        private const string IMAGE_FILE_PATTERN = @"\.(png|gif|jpg)$";

        private int p_ProfileId = Null.NullInteger;
        private int p_ProfileTabId = Null.NullInteger;

        #endregion

        #region Properties

        private int ProfileId
        {
            get
            {
                if (p_ProfileId == Null.NullInteger)
                {
                    var oUserId = Request.QueryString["UserId"] ?? UserId.ToString();
                    if (!string.IsNullOrEmpty(oUserId) && !string.Equals(oUserId, "-1")) p_ProfileId = int.Parse(oUserId.ToString(), System.Globalization.NumberStyles.Integer);
                }

                return p_ProfileId;
            }
        }

        private int ProfileTabId
        {
            get
            {
                if (p_ProfileTabId == Null.NullInteger)
                {
                    var oTabId = Settings[FeatureController.SETTINGKEY_PROFILETABID];
                    if (oTabId != null) p_ProfileTabId = int.Parse(oTabId.ToString(), System.Globalization.NumberStyles.Integer);
                }

                return p_ProfileTabId;
            }
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
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

        #endregion

        #region Helper Methods

        private void BindData()
        {
            LocalizeModule();

            if (ProfileTabId == Null.NullInteger)
            {
                rptGroupList.Visible = false;
                litMessage.Text = string.Format(MESSAGE_ERROR_FORMAT, GetLocalizedString("SettingsEmpty.ErrorMessage"));
                return;
            }

            if (ProfileId > Null.NullInteger)
            {
                // only show groups if this module is on a profile page

                var ctlUser = new UserController();
                var ctlRole = new RoleController();
                var oUser = ctlUser.GetUser(PortalId, ProfileId);

                // get a list of the groups that this site member is a part of
                // This does not return the groups that only belong to this user like you expect. It returns all roles.
                //var roles = ctlRole.GetUserRoles(oUser, false);
                // This overload does.
                //var roles = ctlRole.GetUserRoles(PortalId, oUser.Username, string.Empty);
                var bindableRoles = ctlRole.GetUserRoles(PortalId, oUser.Username, string.Empty)
                    .Where(r => r.RoleType != 0 && r.Status == RoleStatus.Approved && r.SecurityMode == SecurityMode.SocialGroup || r.SecurityMode == SecurityMode.Both);

                if (bindableRoles.Any())
                {

                    /*
                    // some of the possibly helpful role properties for this module
                    roles[0].CreatedByUser(PortalId);
                    roles[0].CreatedByUserID;
                    roles[0].CreatedOnDate;
                    roles[0].Description;
                    roles[0].FullName;
                    roles[0].IconFile;
                    roles[0].IsOwner;
                    roles[0].IsPublic;
                    roles[0].PhotoURL;
                    roles[0].RoleName;
                    */

                    // assign the roles to the repeater to be displayed
                    rptGroupList.DataSource = bindableRoles;
                    rptGroupList.DataBind();

                    // hide the message placeholder
                    litMessage.Visible = false;
                }
                else
                {
                    // hide the repeater
                    rptGroupList.Visible = false;

                    // there are no roles for this user
                    litMessage.Text = string.Format(MESSAGE_FORMAT, GetLocalizedString("NoRoles.ErrorMessage"));
                }
            }
            else
            {
                // send back a user-friendly message that this module cannot be used on this page
                // hide the repeater
                rptGroupList.Visible = false;

                // there are no roles for this user
                litMessage.Text = string.Format(MESSAGE_FORMAT, GetLocalizedString("WrongPage.ErrorMessage"));
            }
        }

        private void LocalizeModule()
        {

        }

        /// <summary>
        /// Parses the group icon file.
        /// </summary>
        /// <param name="GroupId">The group identifier.</param>
        /// <param name="iconFile">The icon file.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">
        /// GroupId cannot be null
        /// or
        /// The image object is null.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">The File ID is not in a valid format.</exception>
        protected string ParseGroupIconFile(object GroupId, object iconFile)
        {
            if (GroupId == null) throw new ArgumentNullException("GroupId cannot be null");

            // return a default icon
            if (iconFile == null) return Page.ResolveClientUrl("/DesktopModules/UGLabsMyGroups/Images/ugdefault.jpg");

            var icon = iconFile.ToString();

            // parse the value to determine if it's a URL or a DNN file
            if (Regex.IsMatch(icon, FeatureController.FILEID_MATCH_PATTERN, RegexOptions.IgnoreCase))
            {
                // the value is FileID=[number]
                var fileIdValue = Regex.Match(icon, FeatureController.FILEID_MATCH_PATTERN, RegexOptions.IgnoreCase).Groups[1].Value;
                
                if (!Regex.IsMatch(fileIdValue, @"^\d+$")) throw new ArgumentOutOfRangeException("The File ID is not in a valid format.");

                var fileId = int.Parse(fileIdValue);

                var ctlModule = new FeatureController();

                // grab a reference to the image
                var image = FileManager.Instance.GetFile(fileId);

                if (image == null) throw new ArgumentNullException("The image object is null.");

                if (image.Width > 124)
                {
                    // resize and save a new image
                    var thumbnail = CreateGroupIconImage(image);

                    // update the role with the new file id
                    var ctlRole = new RoleController();
                    var role = ctlRole.GetRole(int.Parse(GroupId.ToString()), PortalId);
                    role.IconFile = string.Format("FileID={0}", thumbnail.FileId);
                    ctlRole.UpdateRole(role);

                    return ctlModule.GetImageFileUrl(thumbnail);
                }

                return ctlModule.GetImageFileUrl(image);
            }
            else
            {
                // this is an actual URL
                return icon;
            }
        }

        /// <summary>
        /// Gets the group page URL.
        /// </summary>
        /// <param name="GroupId">The group identifier.</param>
        /// <returns></returns>
        protected string GetGroupPageUrl(object GroupId)
        {
            if (GroupId == null)
            {
                return "#";
            }
            else
            {
                var groupUrl = Globals.NavigateURL(ProfileTabId, string.Empty, string.Format("GroupId={0}", GroupId));
                return groupUrl;
            }
        }

        #endregion

        #region Thumbnail Methods

        private IFileInfo CreateGroupIconImage(IFileInfo groupIcon)
        {

            // determine the new width
            var intNewWidth = 124;
            if (groupIcon.Width <= intNewWidth)
            {
                intNewWidth = groupIcon.Width;
            }

            // determine the new height (aspect ratio from width)
            var intNewHeight = (int) (groupIcon.Height*intNewWidth/groupIcon.Width);
            if (intNewHeight > MAX_IMAGE_HEIGHT)
            {
                intNewWidth = (int) (groupIcon.Width*MAX_IMAGE_HEIGHT/groupIcon.Height);
                intNewHeight = MAX_IMAGE_HEIGHT;
            }

            Image oImage = null;
            try
            {
                // get an instance of the image from the file system
                oImage = Image.FromStream(FileManager.Instance.GetFileContent(groupIcon));
            }
            catch (Exception ex)
            {
                Exceptions.LogException(ex);
            }

            if (oImage == null) throw new NullReferenceException("The image could not be loaded from the file system.");

            // create and save a thumbnail
            Image oThumbnail;
            // use the built-in thumbnail generation method
            //oThumbnail = oImage.GetThumbnailImage(intNewWidth, intNewHeight, ThumbnailCallback, IntPtr.Zero);
            oThumbnail = CreateThumbnailImage(oImage, intNewWidth, intNewHeight);

            // save the image in memory
            var ms = new MemoryStream();

            // branch the save statement to avoid encoder errors
            switch(groupIcon.Extension.ToLower())
            {
                case "jpg":
                    oThumbnail.Save(ms, ImageFormat.Jpeg);
                    break;
                case "gif":
                    oThumbnail.Save(ms, ImageFormat.Gif);
                    break;
                case "png":
                    oThumbnail.Save(ms, ImageFormat.Png);
                    break;
                default:
                    throw new InvalidCastException("The image file is not in a supported format.");
            }

            // get an instance of the DNN folder
            IFolderInfo folder = FolderManager.Instance.GetFolder(PortalId, groupIcon.Folder);

            // generate a thumbnail name
            var thumbnailName = GetThumbnailName(groupIcon.FileName, false);

            // save the image in the DNN file system
            var newFile = FileManager.Instance.AddFile(folder, thumbnailName, ms);

            // return the new thumbnail image to the calling method
            return newFile;
        }

        private string GetThumbnailName(string originalFileName, bool replaceSlash)
        {
            if (replaceSlash)
            {
                return
                    ReformatUrlForWeb(
                        Regex.Replace(originalFileName, IMAGE_FILE_PATTERN, IMAGE_THUMBNAIL_PATTERN,
                                      RegexOptions.IgnoreCase).
                            Replace(PortalSettings.HomeDirectoryMapPath, string.Empty));
            }
            else
            {
                return Regex.Replace(originalFileName, IMAGE_FILE_PATTERN, IMAGE_THUMBNAIL_PATTERN, RegexOptions.IgnoreCase);
            }
        }

        private string ReformatUrlForWeb(string path)
        {
            return path.Replace("\\", "/");
        }

        private bool ThumbnailCallback()
        {
            return false;
        }

        private Image CreateThumbnailImage(Image file, int newWidth, int newHeight)
        {
            var newImage = new Bitmap(newWidth, newHeight);
            using (var gr = Graphics.FromImage(newImage))
            {
                gr.SmoothingMode = SmoothingMode.HighQuality;
                gr.InterpolationMode = InterpolationMode.HighQualityBicubic;
                gr.PixelOffsetMode = PixelOffsetMode.HighQuality;
                gr.DrawImage(file, new Rectangle(0, 0, newWidth, newHeight));
            }

            return newImage;
        }

        #endregion

    }
}