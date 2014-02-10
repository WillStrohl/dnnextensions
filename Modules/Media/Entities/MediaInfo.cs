//
// DNN Corp - http://www.dnnsoftware.com
// Copyright (c) 2002-2014
// by DNN Corp
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated 
// documentation files (the "Software"), to deal in the Software without restriction, including without limitation 
// the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and 
// to permit persons to whom the Software is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or substantial portions 
// of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
// TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
// DEALINGS IN THE SOFTWARE.
//

//ORIGINAL LINE: Imports DotNetNuke.Modules.Media.MediaInfoMembers
//INSTANT C# NOTE: The following line has been modified since C# non-aliased 'using' statements only operate on namespaces:
//INSTANT C# NOTE: Formerly VB project-level imports:
using DotNetNuke.Common.Utilities;
using DotNetNuke.Services.Exceptions;
using System;
using System.Text.RegularExpressions;
using DotNetNuke.Services.FileSystem;

namespace DotNetNuke.Modules.Media
{

    /// <summary>
    /// Represents a piece of Media.
    /// </summary>
    [Serializable()]
    public class MediaInfo : Entities.Modules.IHydratable, IMediaInfo
    {

        #region Constants

        private const string FILE_PATH_PATTERN = @"^([\w-\s]+/)*([\w-\s]+\.\w{2,})$";

        #endregion

        #region  Fields

        private int p_ModuleID = Null.NullInteger;
        private string p_Src = Null.NullString;
        private string p_Alt = Null.NullString;
        private int p_Width = Null.NullInteger;
        private int p_Height = Null.NullInteger;
        private string p_NavigateUrl = Null.NullString;
        private bool p_NewWindow = Null.NullBoolean;
        private bool p_TrackClicks = Null.NullBoolean;
        // 03.02.03
        private int p_MediaAlignment = Null.NullInteger;
        // 03.03.00
        private bool p_MediaLoop = Null.NullBoolean;
        private bool p_AutoStart = Null.NullBoolean;
        // 04.00.00
        private int p_MediaType = Null.NullInteger;
        // 04.01.00
        private string p_MediaMessage = Null.NullString;
        private int p_LastUpdatedBy = Null.NullInteger;
        private DateTime p_LastUpdatedDate = Null.NullDate;

        #endregion

        #region  Initialization

        /// <summary>
        /// Instantiates a new instance of the <c>ImageInfo</c> class.
        /// </summary>
        public MediaInfo()
        {
            this.p_ModuleID = -1;
            this.p_Src = string.Empty;
            this.p_Alt = string.Empty;
            this.p_Width = -1;
            this.p_Height = -1;
            this.p_NavigateUrl = string.Empty;
            this.p_NewWindow = false;
            this.p_TrackClicks = false;
            this.p_MediaAlignment = 0;
            this.p_MediaLoop = false;
            this.p_AutoStart = false;
            this.p_MediaType = 0;
            this.p_MediaMessage = string.Empty;
            this.p_LastUpdatedBy = -1;
            this.p_LastUpdatedDate = DateTime.MinValue;
        }

        #endregion

        #region  Properties

        /// <summary>
        /// Gets or sets the unique module identifier.
        /// </summary>
        public int ModuleId
        {
            get
            {
                return this.ModuleID;
            }
            set
            {
                this.ModuleID = value;
            }
        }
        
        /// <summary>
        /// ModuleId - backwards compatibility following the VB to C# conversion
        /// </summary>
        public int ModuleID
        {
            get
            {
                return this.p_ModuleID;
            }
            set
            {
                this.p_ModuleID = value;
            }
        }

        /// <summary>
        /// Gets or sets the URL of the Media to display.
        /// </summary>
        public string Src
        {
            get
            {
                return this.p_Src;
            }
            set
            {
                this.p_Src = value;
            }
        }

        /// <summary>
        /// Gets or sets the text to display if the Media cannot be displayed.
        /// </summary>
        public string Alt
        {
            get
            {
                return this.p_Alt;
            }
            set
            {
                this.p_Alt = value;
            }
        }

        /// <summary>
        /// Gets or sets the display width of the Media.
        /// </summary>
        public int Width
        {
            get
            {
                return this.p_Width;
            }
            set
            {
                this.p_Width = value;
            }
        }

        /// <summary>
        /// Gets or sets the display height of the Media.
        /// </summary>
        public int Height
        {
            get
            {
                return this.p_Height;
            }
            set
            {
                this.p_Height = value;
            }
        }

        /// <summary>
        /// Gets or sets the URL to navigate to when the Media is clicked.
        /// </summary>
        public string NavigateUrl
        {
            get
            {
                return this.p_NavigateUrl;
            }
            set
            {
                this.p_NavigateUrl = value;
            }
        }

        /// <summary>
        /// Gets or sets the target in which the <see cref="NavigateUrl"/> will be opened in.
        /// </summary>
        /// <value></value>
        /// <remarks></remarks>
        /// <history>
        /// 	[Leigh] 	26/02/2006	Created
        /// </history>
        public bool NewWindow
        {
            get
            {
                return this.p_NewWindow;
            }
            set
            {
                this.p_NewWindow = value;
            }
        }

        /// <summary>
        /// Gets or sets the TrackClicks in which the <see cref="NavigateUrl"/> will be opened in.
        /// </summary>
        /// <value></value>
        /// <remarks></remarks>
        /// <history>
        /// 	[Leigh] 	26/02/2006	Created
        /// </history>
        public bool TrackClicks
        {
            get
            {
                return this.p_TrackClicks;
            }
            set
            {
                this.p_TrackClicks = value;
            }
        }

        /// <summary>
        /// Gets or sets the MediaAlignment in which the media will use.
        /// </summary>
        /// <value></value>
        /// <remarks></remarks>
        /// <history>
        /// 	[Leigh] 	25/10/2006	Created
        /// </history>
        public int MediaAlignment
        {
            get
            {
                return this.p_MediaAlignment;
            }
            set
            {
                this.p_MediaAlignment = value;
            }
        }

        /// <summary>
        /// Gets or sets the MediaLoop in which the media will use.
        /// </summary>
        /// <value></value>
        /// <remarks></remarks>
        /// <history>
        /// [wstrohl] - 01/31/2010 - Created
        /// </history>
        public bool MediaLoop
        {
            get
            {
                return this.p_MediaLoop;
            }
            set
            {
                this.p_MediaLoop = value;
            }
        }

        /// <summary>
        /// Gets or sets the AutoStart in which the media will use.
        /// </summary>
        /// <value></value>
        /// <remarks></remarks>
        /// <history>
        /// [wstrohl] - 01/31/2010 - Created
        /// </history>
        public bool AutoStart
        {
            get
            {
                return this.p_AutoStart;
            }
            set
            {
                this.p_AutoStart = value;
            }
        }

        /// <summary>
        /// Gets or sets the MediaType in which the media will use.
        /// </summary>
        /// <value></value>
        /// <remarks></remarks>
        /// <history>
        /// [wstrohl] - 20110319 - Created
        /// </history>
        public int MediaType
        {
            get
            {
                return this.p_MediaType;
            }
            set
            {
                this.p_MediaType = value;
            }
        }

        /// <summary>
        /// Gets or sets the MediaMessage in which will be displayed with the media.
        /// </summary>
        /// <value></value>
        /// <remarks></remarks>
        /// <history>
        /// [wstrohl] - 20110708 - Created
        /// </history>
        public string MediaMessage
        {
            get
            {
                return this.p_MediaMessage;
            }
            set
            {
                this.p_MediaMessage = value;
            }
        }

        /// <summary>
        /// Gets or sets the LastUpdatedBy to keep track of who updates the content.
        /// </summary>
        /// <value></value>
        /// <remarks></remarks>
        /// <history>
        /// [wstrohl] - 20110708 - Created
        /// </history>
        public int LastUpdatedBy
        {
            get
            {
                return this.p_LastUpdatedBy;
            }
            set
            {
                this.p_LastUpdatedBy = value;
            }
        }

        /// <summary>
        /// Gets or sets the LastUpdatedDate to keep track of when the content was last updated.
        /// </summary>
        /// <value></value>
        /// <remarks></remarks>
        /// <history>
        /// [wstrohl] - 20110708 - Created
        /// </history>
        public DateTime LastUpdatedDate
        {
            get
            {
                return this.p_LastUpdatedDate;
            }
            set
            {
                this.p_LastUpdatedDate = value;
            }
        }

        /// <summary>
        /// WebFriendlyUrl - returns a URL that can immediately be used to display the media source path, regardless of folder provider
        /// </summary>
        public string WebFriendlyUrl
        {
            get
            {
                if (Regex.IsMatch(Src, @"^FileID=\d+$"))
                {
                    int fileId = int.Parse(Regex.Replace(Src, @"FileID=", string.Empty));
                    IFileInfo oFile = FileManager.Instance.GetFile(fileId);
                    MediaController ctlMedia = new MediaController();

                    return ctlMedia.GetFileUrl(oFile);
                }
                else if (Regex.IsMatch(Src, FILE_PATH_PATTERN))
                {
                    // IMPORTANT NOTE!!
                    // This code is not thread safe... The portal settings require an active web session
                    Entities.Portals.PortalSettings settings = Entities.Portals.PortalController.GetCurrentPortalSettings();
                    //string folderPath = Regex.Match(Src, FILE_PATH_PATTERN).Groups[1].Value;
                    //string fileName = Regex.Match(Src, FILE_PATH_PATTERN).Groups[2].Value;
                    // Fix suggested from Issue 23595
                    string fileName = Regex.Match(Src, FILE_PATH_PATTERN).Groups[2].Value;
                    string folderPath = Src.Substring(0, Src.Length - fileName.Length); 

                    IFolderInfo oFolder = FolderManager.Instance.GetFolder(settings.PortalId, folderPath);
                    MediaController ctlMedia = new MediaController();
                    IFileInfo oFile = ctlMedia.GetFileFromProvider(settings.PortalId, oFolder, fileName);

                    return ctlMedia.GetFileUrl(oFile);
                }
                else
                {
                    return Src;
                }
            }
            set { }
        }

        /// <summary>
        /// FileExtension - returns the extension for the file
        /// </summary>
        public string FileExtension
        {
            get
            {
                if (Regex.IsMatch(Src, @"^FileID=\d+$"))
                {
                    int fileId = int.Parse(Regex.Replace(Src, @"FileID=", string.Empty));
                    IFileInfo oFile = FileManager.Instance.GetFile(fileId);

                    return oFile.Extension;
                }
                else if (Regex.IsMatch(Src, FILE_PATH_PATTERN))
                {
                    return Regex.Match(Src, @"\.(\w{2,})$").Groups[1].Value;
                }
                else
                {
                    return string.Empty;
                }
            }
            set { }
        }

        /// <summary>
        /// ContentType - returns the content type of the file
        /// </summary>
        public string ContentType
        {
            get
            {
                if (Regex.IsMatch(Src, @"^FileID=\d+$"))
                {
                    int fileId = int.Parse(Regex.Replace(Src, @"FileID=", string.Empty));
                    IFileInfo oFile = FileManager.Instance.GetFile(fileId);

                    return oFile.ContentType;
                }
                else if (Regex.IsMatch(Src, FILE_PATH_PATTERN))
                {
                    // IMPORTANT NOTE!!
                    // This code is not thread safe... The portal settings require an active web session
                    Entities.Portals.PortalSettings settings = Entities.Portals.PortalController.GetCurrentPortalSettings();
                    string folderPath = Regex.Match(Src, FILE_PATH_PATTERN).Groups[1].Value;
                    string fileName = Regex.Match(Src, FILE_PATH_PATTERN).Groups[2].Value;

                    IFolderInfo oFolder = FolderManager.Instance.GetFolder(settings.PortalId, folderPath);
                    MediaController ctlMedia = new MediaController();
                    IFileInfo oFile = ctlMedia.GetFileFromProvider(settings.PortalId, oFolder, fileName);

                    return oFile.ContentType;
                }
                else
                {
                    return string.Empty;
                }
            }
            set { }
        }

        #endregion

        #region  IHydratable Implementation

        public void Fill(System.Data.IDataReader dr)
        {
            try
            {
                while (dr.Read())
                {
                    if (dr[MediaInfoMembers.AltField] != null)
                    {
                        this.p_Alt = dr[MediaInfoMembers.AltField].ToString();
                    }
                    if (dr[MediaInfoMembers.HeightField] != null)
                    {
                        if (RegExUtility.IsNumber(dr[MediaInfoMembers.HeightField]))
                        {
                            this.p_Height = int.Parse(dr[MediaInfoMembers.HeightField].ToString(), System.Globalization.NumberStyles.Integer);
                        }
                    }
                    if (dr[MediaInfoMembers.MediaAlignmentField] != null)
                    {
                        if (RegExUtility.IsNumber(dr[MediaInfoMembers.MediaAlignmentField]))
                        {
                            this.p_MediaAlignment = int.Parse(dr[MediaInfoMembers.MediaAlignmentField].ToString(), System.Globalization.NumberStyles.Integer);
                        }
                    }
                    if (dr[MediaInfoMembers.ModuleIdField] != null)
                    {
                        if (RegExUtility.IsNumber(dr[MediaInfoMembers.ModuleIdField]))
                        {
                            this.p_ModuleID = int.Parse(dr[MediaInfoMembers.ModuleIdField].ToString(), System.Globalization.NumberStyles.Integer);
                        }
                    }
                    if (dr[MediaInfoMembers.NavigateUrlField] != null)
                    {
                        this.p_NavigateUrl = dr[MediaInfoMembers.NavigateUrlField].ToString();
                    }
                    if (dr[MediaInfoMembers.NewWindowField] != null)
                    {
                        if (RegExUtility.IsBoolean(dr[MediaInfoMembers.NewWindowField]))
                        {
                            this.p_NewWindow = bool.Parse(dr[MediaInfoMembers.NewWindowField].ToString());
                        }
                    }
                    if (dr[MediaInfoMembers.SrcField] != null)
                    {
                        this.p_Src = dr[MediaInfoMembers.SrcField].ToString();
                    }
                    if (dr[MediaInfoMembers.TrackClicksField] != null)
                    {
                        if (RegExUtility.IsBoolean(dr[MediaInfoMembers.TrackClicksField]))
                        {
                            this.p_TrackClicks = bool.Parse(dr[MediaInfoMembers.TrackClicksField].ToString());
                        }
                    }
                    if (dr[MediaInfoMembers.WidthField] != null)
                    {
                        if (RegExUtility.IsNumber(dr[MediaInfoMembers.WidthField]))
                        {
                            this.p_Width = int.Parse(dr[MediaInfoMembers.WidthField].ToString(), System.Globalization.NumberStyles.Integer);
                        }
                    }
                    if (dr[MediaInfoMembers.MediaLoopField] != null)
                    {
                        if (RegExUtility.IsBoolean(dr[MediaInfoMembers.MediaLoopField]))
                        {
                            this.p_MediaLoop = bool.Parse(dr[MediaInfoMembers.MediaLoopField].ToString());
                        }
                    }
                    if (dr[MediaInfoMembers.AutoStartField] != null)
                    {
                        if (RegExUtility.IsBoolean(dr[MediaInfoMembers.AutoStartField]))
                        {
                            this.p_AutoStart = bool.Parse(dr[MediaInfoMembers.AutoStartField].ToString());
                        }
                    }
                    if (dr[MediaInfoMembers.MediaTypeField] != null)
                    {
                        if (RegExUtility.IsNumber(dr[MediaInfoMembers.MediaTypeField]))
                        {
                            this.p_MediaType = int.Parse(dr[MediaInfoMembers.MediaTypeField].ToString(), System.Globalization.NumberStyles.Integer);
                        }
                    }
                    if (dr[MediaInfoMembers.MediaMessageField] != null)
                    {
                        this.p_MediaMessage = dr[MediaInfoMembers.MediaMessageField].ToString();
                    }
                    if (dr[MediaInfoMembers.LastUpdatedByField] != null)
                    {
                        if (RegExUtility.IsNumber(dr[MediaInfoMembers.LastUpdatedByField]))
                        {
                            this.p_LastUpdatedBy = int.Parse(dr[MediaInfoMembers.LastUpdatedByField].ToString(), System.Globalization.NumberStyles.Integer);
                        }
                    }
                    if (dr[MediaInfoMembers.LastUpdatedDateField] != null)
                    {
                        if (SimulateIsDate.IsDate(dr[MediaInfoMembers.LastUpdatedDateField]))
                        {
                            DateTime.TryParse(dr[MediaInfoMembers.LastUpdatedDateField].ToString(), out this.p_LastUpdatedDate);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Exceptions.LogException(ex);
            }
            finally
            {
                if (!dr.IsClosed)
                {
                    dr.Close();
                }
            }
        }

        public int KeyID
        {
            get
            {
                return this.p_ModuleID;
            }
            set
            {
                this.p_ModuleID = value;
            }
        }

        #endregion

    }

}
