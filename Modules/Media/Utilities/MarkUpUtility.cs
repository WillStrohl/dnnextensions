//
// DotNetNuke® - http://www.dotnetnuke.com
// Copyright (c) 2002-2013
// by DotNetNuke Corporation
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

//ORIGINAL LINE: Imports DotNetNuke.Modules.Media.MediaController
//INSTANT C# NOTE: The following line has been modified since C# non-aliased 'using' statements only operate on namespaces:
//INSTANT C# NOTE: Formerly VB project-level imports:
using DotNetNuke;
using DotNetNuke.Common;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Localization;
using System;
using System.Collections;
using System.Data;
using System.Diagnostics;

using DotNetNuke.Modules.Media;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace DotNetNuke.Modules.Media
{

	public sealed class MediaMarkUpUtility
	{

#region  Constants 

		//Private Const OPEN_BRACKET As String = "<"
		private const string CLOSE_BRACKET = ">";
		private const string SELF_CLOSE_BRACKET = "/>";
		private const string SPACE = " ";
		private const string OPEN_TAG_FORMAT = "<{0} ";
		private const string CLOSE_TAG_FORMAT = "</{0}>";
        private const string PARAM_TAG_FORMAT = "<param name=\"{0}\" value=\"{1}\" />";
        private const string SOURCE_TAG_FORMAT = "<source src=\"{0}\" type=\"{1}\" />";
        private const string SOURCE_TAG_FORMAT_ALT = "<source src=\"{0}\" />";

		private const string DIV_TAG = "div";
		private const string ANCHOR_TAG = "a";
		private const string IMAGE_TAG = "img";
		//Private Const PARAM_TAG As String = "param"
		private const string OBJECT_TAG = "object";
        //Private Const EMBED_TAG As String = "embed"
        private const string VIDEO_TAG = "video";
        private const string AUDIO_TAG = "audio";

		private const string ID_ATTRIBUTE = "id=\"{0}\" ";
		private const string CLASS_ATTRIBUTE = "class=\"{0}\" ";
		private const string DATA_ATTRIBUTE = "data=\"{0}\" ";
		private const string SRC_ATTRIBUTE = "src=\"{0}\" ";
		private const string ALT_ATTRIBUTE = "alt=\"{0}\" ";
		private const string TITLE_ATTRIBUTE = "title=\"{0}\" ";
		//Private Const NAME_ATTRIBUTE As String = "name=""{0}"" "
		//Private Const VALUE_ATTRIBUTE As String = "value=""{0}"" "
		private const string WIDTH_ATTRIBUTE = "width=\"{0}\" ";
		private const string HEIGHT_ATTRIBUTE = "height=\"{0}\" ";
		//Private Const LOOP_ATTRIBUTE As String = "loop=""{0}"" "
		private const string HREF_ATTRIBUTE = "href=\"{0}\" ";
		private const string TARGET_ATTRIBUTE = "target=\"{0}\" ";
		//Private Const AUTOSTART_ATTRIBUTE As String = "autostart=""{0}"" "
		//Private Const AUTOPLAY_ATTRIBUTE As String = "autoplay=""{0}"" "
        private const string STYLE_ATTRIBUTE = "style=\"width:{0}px;height:{1};\" ";
        private const string AUTOPLAY_ATTRIBUTE = "autoplay=\"autoplay\" ";
        private const string LOOP_ATTRIBUTE = "loop=\"loop\" ";
        private const string CONTROLS_ATTRIBUTE = "controls=\"controls\" ";

		private const string DIV_ID_PREFIX = "div_";
		private const string IMAGE_ID_PREFIX = "image_";
		private const string ANCHOR_ID_PREFIX = "a_";
		private const string FLASH_ID_PREFIX = "flash_";
		private const string WINDOWS_MEDIA_ID_PREFIX = "windowsmedia_";
		private const string REAL_PLAYER_ID_PREFIX = "realplayer_";
		private const string QUICKTIME_ID_PREFIX = "quicktime_";

		private const string AUTOSTART = "autostart";
		private const string SRC = "src";
		//Private Const URL As String = "url"
		private const string AUTOPLAY = "autoplay";
		private const string MEDIA_LOOP = "loop";
		private const string BLANK_ATTRIBUTE = "_blank";

		private const string DIV_CLASS = "dnnmedia_wrapper";
		private const string MEDIA_IMAGE_CLASS = "dnnmedia_image";
		private const string ALIGN_LEFT_CLASS = "dnnmedia_left";
		private const string ALIGN_RIGHT_CLASS = "dnnmedia_right";
		private const string ALIGN_CENTER_CLASS = "dnnmedia_center";

#endregion

		private string p_CurrentDomain = Null.NullString;
		private string CurrentDomain
		{
			get
			{
				if (! (string.IsNullOrEmpty(p_CurrentDomain)))
				{
					return p_CurrentDomain;
				}

				DotNetNuke.Entities.Portals.PortalSettings pSettings = null;
				pSettings = DotNetNuke.Entities.Portals.PortalController.GetCurrentPortalSettings();

				if (HttpContext.Current.Request.IsSecureConnection)
				{
					p_CurrentDomain = string.Concat("https://", pSettings.PortalAlias.HTTPAlias);
				}
				else
				{
					p_CurrentDomain = string.Concat("http://", pSettings.PortalAlias.HTTPAlias);
				}

				return p_CurrentDomain;
			}
		}

#region  RegEx Patterns 

		private const string IMAGE_PATTERN = "\\.(jpg|jpeg?|gif|bmp|png)";
		private const string FLASH_PATTERN = "\\.swf";
		private const string WINDOWS_MEDIA_PATTERN = "\\.(avi|wmv|midi|wav|asx|mp3|mpg|mpeg|asf|wma)";
		private const string REAL_PLAYER_PATTERN = "\\.(ram|rpm|rm)";
		private const string QUICKTIME_PATTERN = "\\.(mov|qt|mp4)";

		private const string NUMERIC_MATCH_PATTERN = "^\\d+$";

#endregion

#region  Get Media Type 

		public MediaType GetMediaType(MediaInfo Media)
		{
			return GetMediaType(Media.Src);
		}

		public MediaType GetMediaType(string MediaURL)
		{

			if (Regex.IsMatch(MediaURL, IMAGE_PATTERN, RegexOptions.IgnoreCase))
			{
				return MediaType.Image;
			}
			else if (Regex.IsMatch(MediaURL, FLASH_PATTERN, RegexOptions.IgnoreCase))
			{
				return MediaType.Flash;
			}
			else if (Regex.IsMatch(MediaURL, WINDOWS_MEDIA_PATTERN, RegexOptions.IgnoreCase))
			{
				return MediaType.WindowsMedia;
			}
			else if (Regex.IsMatch(MediaURL, QUICKTIME_PATTERN, RegexOptions.IgnoreCase))
			{
				return MediaType.Quicktime;
			}
			else if (Regex.IsMatch(MediaURL, REAL_PLAYER_PATTERN, RegexOptions.IgnoreCase))
			{
				return MediaType.RealPlayer;
			}
			else
			{
				return MediaType.Unknown;
			}

		}

#endregion

#region  Get Media Mark-Up 

		public string GetImageMarkUp(MediaInfo Media, DotNetNuke.Entities.Modules.ModuleInfo ModuleConfig)
		{

			string strTagId = string.Concat(IMAGE_ID_PREFIX, Media.ModuleID.ToString());
			string strAnchorId = string.Concat(ANCHOR_ID_PREFIX, strTagId);
			string strDivId = string.Concat(DIV_ID_PREFIX, strTagId);
			string strDivCssClass = string.Concat(DIV_CLASS, GetMediaAlignment(Media.MediaAlignment, ModuleConfig));

			//
			// BUILD:
			// <div id="" class="">
			// <img id="" class="" src="" alt="" title="" />
			// </div>
			//

			StringBuilder sb = new StringBuilder(10);

			// build open div
			sb.AppendFormat(OPEN_TAG_FORMAT, DIV_TAG);
			sb.AppendFormat(ID_ATTRIBUTE, strDivId);
			sb.AppendFormat(CLASS_ATTRIBUTE.Trim(), strDivCssClass);
			sb.Append(CLOSE_BRACKET);
			//

			// build open anchor tag
			if (! (string.IsNullOrEmpty(Media.NavigateUrl)))
			{
				sb.AppendFormat(OPEN_TAG_FORMAT, ANCHOR_TAG);
				sb.AppendFormat(ID_ATTRIBUTE, strAnchorId);

				if (Regex.IsMatch(Media.NavigateUrl, NUMERIC_MATCH_PATTERN))
				{
					Media.NavigateUrl = Globals.NavigateURL(int.Parse(Media.NavigateUrl, NumberStyles.Integer));
				}

				if (Media.TrackClicks)
				{
					// need to parse the local URL to get the human-friendly one
					sb.AppendFormat(HREF_ATTRIBUTE, FormatURL(Media.NavigateUrl, Media.TrackClicks, ModuleConfig.TabID, ModuleConfig.ModuleID));
				}
				else
				{
					// this must be a URL already
					sb.AppendFormat(HREF_ATTRIBUTE, FormatURL(Media.NavigateUrl));
				}

				if (Media.NewWindow)
				{
					sb.AppendFormat(TARGET_ATTRIBUTE, BLANK_ATTRIBUTE);
				}

				sb.Append(CLOSE_BRACKET);
			}
			//

			// begin building the image tag
			sb.AppendFormat(OPEN_TAG_FORMAT, IMAGE_TAG);
			sb.AppendFormat(ID_ATTRIBUTE, strTagId);
			sb.AppendFormat(CLASS_ATTRIBUTE, MEDIA_IMAGE_CLASS);
			sb.AppendFormat(SRC_ATTRIBUTE, Media.WebFriendlyUrl);

			if (! (string.IsNullOrEmpty(Media.Alt)))
			{
				sb.AppendFormat(ALT_ATTRIBUTE, Media.Alt);
				sb.AppendFormat(TITLE_ATTRIBUTE, Media.Alt);
			}
			else
			{
				// for XHTML compliance
				sb.AppendFormat(ALT_ATTRIBUTE, string.Empty);
			}

			if (Media.Width > Null.NullInteger & Media.Height > Null.NullInteger)
			{
				sb.AppendFormat(STYLE_ATTRIBUTE, Media.Width.ToString(), Media.Height.ToString());
			}

			sb.Append(SELF_CLOSE_BRACKET);
			//

			// build close anchor
			if (! (string.IsNullOrEmpty(Media.NavigateUrl)))
			{
				sb.AppendFormat(CLOSE_TAG_FORMAT, ANCHOR_TAG);
			}
			//

			// build close div
			sb.AppendFormat(CLOSE_TAG_FORMAT, DIV_TAG);
			//

			return sb.ToString();

		}

		public string GetFlashMarkUp(MediaInfo Media, DotNetNuke.Entities.Modules.ModuleInfo ModuleConfig)
		{

			string strFlashId = string.Concat(FLASH_ID_PREFIX, Media.ModuleID.ToString());
			string strDivId = string.Concat(DIV_ID_PREFIX, strFlashId);
			string strDivCssClass = string.Concat(DIV_CLASS, GetMediaAlignment(Media.MediaAlignment, ModuleConfig));

			//Media.Src = String.Concat(CurrentDomain, Media.Src)

			//
			// BUILD:
			// <div id="" class="">
			// <embed pluginspage="http://www.macromedia.com/go/getflashplayer" src="" width="" height="" type="application/x-shockwave-flash" bgcolor="" salign="LT" quality="high" menu="false" loop=""></embed>
			// </div>
			//

			StringBuilder sb = new StringBuilder(10);

			// build open div
			sb.AppendFormat(OPEN_TAG_FORMAT, DIV_TAG);
			sb.AppendFormat(ID_ATTRIBUTE, strDivId);
			sb.AppendFormat(CLASS_ATTRIBUTE.Trim(), strDivCssClass);
			sb.Append(CLOSE_BRACKET);
			//

			// build flash object
			sb.AppendFormat(OPEN_TAG_FORMAT, OBJECT_TAG);
			sb.AppendFormat(ID_ATTRIBUTE, strFlashId);
			if (Media.Width > Null.NullInteger)
			{
				sb.AppendFormat(WIDTH_ATTRIBUTE, Media.Width);
			}
			if (Media.Height > Null.NullInteger)
			{
				sb.AppendFormat(HEIGHT_ATTRIBUTE, Media.Height);
			}
			sb.Append("classid=\"clsid:D27CDB6E-AE6D-11cf-96B8-444553540000\" ");
			sb.AppendFormat("codebase=\"http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=6,0,40,0\"");
			sb.Append(CLOSE_BRACKET);
            sb.AppendFormat(PARAM_TAG_FORMAT, "movie", Media.WebFriendlyUrl);
			sb.AppendFormat(PARAM_TAG_FORMAT, "quality", "high");
			sb.AppendFormat(PARAM_TAG_FORMAT, "bgcolor", "#ffffff");
			sb.AppendFormat(PARAM_TAG_FORMAT, "wmode", "transparent");
			sb.Append("<!--[if !IE]> <-->");
			sb.AppendFormat(OPEN_TAG_FORMAT, OBJECT_TAG);
			sb.AppendFormat(DATA_ATTRIBUTE, Media.WebFriendlyUrl);
			if (Media.Width > Null.NullInteger)
			{
				sb.AppendFormat(WIDTH_ATTRIBUTE, Media.Width);
			}
			if (Media.Height > Null.NullInteger)
			{
				sb.AppendFormat(HEIGHT_ATTRIBUTE, Media.Height);
			}
			sb.Append("type=\"application/x-shockwave-flash\"");
			sb.Append(CLOSE_BRACKET);
			sb.AppendFormat(PARAM_TAG_FORMAT, "quality", "high");
			sb.AppendFormat(PARAM_TAG_FORMAT, "bgcolor", "#ffffff");
			sb.AppendFormat(PARAM_TAG_FORMAT, "wmode", "transparent");
			sb.AppendFormat(PARAM_TAG_FORMAT, "pluginurl", "http://www.macromedia.com/go/getflashplayer");
			sb.AppendFormat(PARAM_TAG_FORMAT, "wmode", "transparent");
			sb.AppendFormat(CLOSE_TAG_FORMAT, OBJECT_TAG);
			sb.Append("<!--> <![endif]-->");
			sb.AppendFormat(CLOSE_TAG_FORMAT, OBJECT_TAG);
			//

			// build close div
			sb.AppendFormat(CLOSE_TAG_FORMAT, DIV_TAG);
			//

			return sb.ToString();

		}

		public string GetWindowsMediaMarkUp(MediaInfo Media, DotNetNuke.Entities.Modules.ModuleInfo ModuleConfig)
		{

			string strWMediaId = string.Concat(WINDOWS_MEDIA_ID_PREFIX, Media.ModuleID.ToString());
			string strDivId = string.Concat(DIV_ID_PREFIX, strWMediaId);
			string strDivCssClass = string.Concat(DIV_CLASS, GetMediaAlignment(Media.MediaAlignment, ModuleConfig), " dnnmedia_wmp");

			//
			// BUILD:
			// <div id="" class="">
			// <embed src="" width="" height="" autostart="" loop="" pluginspage="http://download.microsoft.com/download/winmediaplayer/nsplugin/6.4/WIN98/EN-US/wmpplugin.exe" type="application/x-mplayer2"></embed>
			// </div>
			//

			StringBuilder sb = new StringBuilder(10);

			// build open div
			sb.AppendFormat(OPEN_TAG_FORMAT, DIV_TAG);
			sb.AppendFormat(ID_ATTRIBUTE, strDivId);
			sb.AppendFormat(CLASS_ATTRIBUTE.Trim(), strDivCssClass);
			sb.Append(CLOSE_BRACKET);
			//

			// build windows media object
			sb.AppendFormat(OPEN_TAG_FORMAT, OBJECT_TAG);
			sb.AppendFormat(ID_ATTRIBUTE, strWMediaId);
			if (Media.Width > Null.NullInteger)
			{
				sb.AppendFormat(WIDTH_ATTRIBUTE, Media.Width);
			}
			if (Media.Height > Null.NullInteger)
			{
				sb.AppendFormat(HEIGHT_ATTRIBUTE, Media.Height);
			}
			sb.Append("classid=\"clsid:6BF52A52-394A-11d3-B153-00C04F79FAA6\"");
			sb.Append(CLOSE_BRACKET);
            sb.AppendFormat(PARAM_TAG_FORMAT, "url", Media.WebFriendlyUrl);
            sb.AppendFormat(PARAM_TAG_FORMAT, "src", Media.WebFriendlyUrl);
			sb.AppendFormat(PARAM_TAG_FORMAT, "showcontrols", "true");
			sb.AppendFormat(PARAM_TAG_FORMAT, AUTOSTART, Media.AutoStart.ToString().ToLower());
			sb.AppendFormat(PARAM_TAG_FORMAT, MEDIA_LOOP, Media.MediaLoop.ToString().ToLower());
			sb.Append("<!--[if !IE]> <-->");
			sb.AppendFormat(OPEN_TAG_FORMAT, OBJECT_TAG);
            sb.AppendFormat(DATA_ATTRIBUTE, Media.WebFriendlyUrl);
			if (Media.Width > Null.NullInteger)
			{
				sb.AppendFormat(WIDTH_ATTRIBUTE, Media.Width);
			}
			if (Media.Height > Null.NullInteger)
			{
				sb.AppendFormat(HEIGHT_ATTRIBUTE, Media.Height);
			}
			sb.Append("type=\"video/x-ms-wmv\"");
			sb.Append(CLOSE_BRACKET);
            sb.AppendFormat(PARAM_TAG_FORMAT, "src", Media.WebFriendlyUrl);
			sb.AppendFormat(PARAM_TAG_FORMAT, "controller", "true");
			sb.AppendFormat(PARAM_TAG_FORMAT, AUTOSTART, Media.AutoStart.ToString().ToLower());
			sb.AppendFormat(PARAM_TAG_FORMAT, MEDIA_LOOP, Media.MediaLoop.ToString().ToLower());
			sb.AppendFormat(CLOSE_TAG_FORMAT, OBJECT_TAG);
			sb.Append("<!--> <![endif]-->");
			sb.AppendFormat(CLOSE_TAG_FORMAT, OBJECT_TAG);
			//

			// build close div
			sb.AppendFormat(CLOSE_TAG_FORMAT, DIV_TAG);
			//

			return sb.ToString();

		}

		public string GetRealPlayerMarkUp(MediaInfo Media, DotNetNuke.Entities.Modules.ModuleInfo ModuleConfig)
		{

			string strRMediaId = string.Concat(REAL_PLAYER_ID_PREFIX, Media.ModuleID.ToString());
			string strDivId = string.Concat(DIV_ID_PREFIX, strRMediaId);
			string strDivCssClass = string.Concat(DIV_CLASS, GetMediaAlignment(Media.MediaAlignment, ModuleConfig), " dnnmedia_wmp");

			//
			// BUILD:
			// <div id="" class="">
			// <object classid="clsid:CFCDAA03-8BE4-11cf-B84B-0020AFBBCCFA" width="" height="">
			// <param name="src" value="" />
			// <param name="controls" value="ImageWindow" />
			// <param name="console" value="one" />
			// <param name="autostart" value="true" />
			// <param name="wmode" value="transparent" />
			// <embed src="" width="" height="" nojava="true" controls="ImageWindow" console="one" autostart="" wmode="transparent"></embed>
			// </object>
			// </div>
			//

			StringBuilder sb = new StringBuilder(10);

			// build open div
			sb.AppendFormat(OPEN_TAG_FORMAT, DIV_TAG);
			sb.AppendFormat(ID_ATTRIBUTE, strDivId);
			sb.AppendFormat(CLASS_ATTRIBUTE.Trim(), strDivCssClass);
			sb.Append(CLOSE_BRACKET);
			//

			// build real media player object
			sb.AppendFormat(OPEN_TAG_FORMAT, OBJECT_TAG);
			sb.AppendFormat(ID_ATTRIBUTE, strRMediaId);
			if (Media.Width > Null.NullInteger)
			{
				sb.AppendFormat(WIDTH_ATTRIBUTE, Media.Width);
			}
			if (Media.Height > Null.NullInteger)
			{
				sb.AppendFormat(HEIGHT_ATTRIBUTE, Media.Height);
			}
			sb.Append("classid=\"clsid:CFCDAA03-8BE4-11cf-B84B-0020AFBBCCFA\"");
			sb.Append(CLOSE_BRACKET);
            sb.AppendFormat(PARAM_TAG_FORMAT, SRC, Media.WebFriendlyUrl);
			sb.AppendFormat(PARAM_TAG_FORMAT, MEDIA_LOOP, Media.MediaLoop);
			sb.AppendFormat(PARAM_TAG_FORMAT, "controls", "ImageWindow");
			sb.AppendFormat(PARAM_TAG_FORMAT, "maintainaspect", "true");
			//sb.AppendFormat(PARAM_TAG_FORMAT, "console", "_unique")
			sb.AppendFormat(PARAM_TAG_FORMAT, AUTOSTART, Media.AutoStart.ToString().ToLower());
			sb.Append("<!--[if !IE]> <-->");
			sb.AppendFormat(OPEN_TAG_FORMAT, OBJECT_TAG);
            sb.AppendFormat(DATA_ATTRIBUTE, Media.WebFriendlyUrl);
			if (Media.Width > Null.NullInteger)
			{
				sb.AppendFormat(WIDTH_ATTRIBUTE, Media.Width);
			}
			if (Media.Height > Null.NullInteger)
			{
				sb.AppendFormat(HEIGHT_ATTRIBUTE, Media.Height);
			}
			sb.Append("type=\"audio/x-pn-realaudio-plugin\"");
			sb.Append(CLOSE_BRACKET);
            sb.AppendFormat(PARAM_TAG_FORMAT, SRC, Media.WebFriendlyUrl);
			sb.AppendFormat(PARAM_TAG_FORMAT, MEDIA_LOOP, Media.MediaLoop);
			sb.AppendFormat(PARAM_TAG_FORMAT, AUTOSTART, Media.AutoStart.ToString().ToLower());
			sb.AppendFormat(PARAM_TAG_FORMAT, "nojava", "true");
			sb.AppendFormat(PARAM_TAG_FORMAT, "controls", "ImageWindow");
			sb.AppendFormat(PARAM_TAG_FORMAT, "maintainaspect", "true");
			//sb.AppendFormat(PARAM_TAG_FORMAT, "console", "_unique")
			sb.AppendFormat(CLOSE_TAG_FORMAT, OBJECT_TAG);
			sb.Append("<!--> <![endif]-->");
			sb.AppendFormat(CLOSE_TAG_FORMAT, OBJECT_TAG);
			//

			// build close div
			sb.AppendFormat(CLOSE_TAG_FORMAT, DIV_TAG);
			//

			return sb.ToString();

		}

		public string GetQuicktimeMarkUp(MediaInfo Media, DotNetNuke.Entities.Modules.ModuleInfo ModuleConfig)
		{

			string strQMediaId = string.Concat(QUICKTIME_ID_PREFIX, Media.ModuleID.ToString());
			string strDivId = string.Concat(DIV_ID_PREFIX, strQMediaId);
			string strDivCssClass = string.Concat(DIV_CLASS, GetMediaAlignment(Media.MediaAlignment, ModuleConfig), " dnnmedia_wmp");

			//
			// BUILD:
			// <div id="" class="">
			// <object classid="clsid:02BF25D5-8C17-4B23-BC80-D3488ABDDC6B" codebase="http://www.apple.com/qtactivex/qtplugin.cab" width="" height="">
			// <param name="src" value="" />
			// <param name="autoplay" value="true" />
			// <param name="controller" value="true" />
			// <param name="loop" value="false" />
			// <param name="type" value="video/quicktime" />
			// <param name="target" value="myself" />
			// <embed width="" height="" pluginspage="http://www.apple.com/quicktime/download/" src="" type="video/quicktime" autoplay="true" controller="true" loop="false" target="myself"></embed>
			// </object>
			// </div>
			//

			StringBuilder sb = new StringBuilder(10);

			// build open div
			sb.AppendFormat(OPEN_TAG_FORMAT, DIV_TAG);
			sb.AppendFormat(ID_ATTRIBUTE, strDivId);
			sb.AppendFormat(CLASS_ATTRIBUTE.Trim(), strDivCssClass);
			sb.Append(CLOSE_BRACKET);
			//

			// build quicktime object
			sb.AppendFormat(OPEN_TAG_FORMAT, OBJECT_TAG);
			sb.AppendFormat(ID_ATTRIBUTE, strQMediaId);
			if (Media.Width > Null.NullInteger)
			{
				sb.AppendFormat(WIDTH_ATTRIBUTE, Media.Width);
			}
			if (Media.Height > Null.NullInteger)
			{
				sb.AppendFormat(HEIGHT_ATTRIBUTE, Media.Height);
			}
			// ISSUE 18955 - http://dnnmedia.codeplex.com/workitem/18955
			// Changed the clsid to be the correct one
			sb.Append("classid=\"clsid:02BF25D5-8C17-4B23-BC80-D3488ABDDC6B\" ");
			sb.Append("codebase=\"http://www.apple.com/qtactivex/qtplugin.cab\"");
			sb.Append(CLOSE_BRACKET);
            sb.AppendFormat(PARAM_TAG_FORMAT, "src", Media.WebFriendlyUrl);
			sb.AppendFormat(PARAM_TAG_FORMAT, "controller", "true");
			sb.AppendFormat(PARAM_TAG_FORMAT, AUTOPLAY, Media.AutoStart);
			sb.AppendFormat(PARAM_TAG_FORMAT, MEDIA_LOOP, Media.MediaLoop);
			sb.Append("<!--[if !IE]> <-->");
			sb.AppendFormat(OPEN_TAG_FORMAT, OBJECT_TAG);
            sb.AppendFormat(DATA_ATTRIBUTE, Media.WebFriendlyUrl);
			if (Media.Width > Null.NullInteger)
			{
				sb.AppendFormat(WIDTH_ATTRIBUTE, Media.Width);
			}
			if (Media.Height > Null.NullInteger)
			{
				sb.AppendFormat(HEIGHT_ATTRIBUTE, Media.Height);
			}
			sb.Append("type=\"video/quicktime\"");
			sb.Append(CLOSE_BRACKET);
			sb.AppendFormat(PARAM_TAG_FORMAT, "controller", "true");
			sb.AppendFormat(PARAM_TAG_FORMAT, AUTOPLAY, Media.AutoStart);
			sb.AppendFormat(PARAM_TAG_FORMAT, MEDIA_LOOP, Media.MediaLoop);
			sb.AppendFormat(CLOSE_TAG_FORMAT, OBJECT_TAG);
			sb.Append("<!--> <![endif]-->");
			sb.AppendFormat(CLOSE_TAG_FORMAT, OBJECT_TAG);
			//

			// build close div
			sb.AppendFormat(CLOSE_TAG_FORMAT, DIV_TAG);
			//

			return sb.ToString();

		}

#endregion

#region  Private Methods 

		private string GetMediaAlignment(int Alignment, DotNetNuke.Entities.Modules.ModuleInfo ModuleConfig)
		{
			switch (Alignment)
			{
				case 0: // Use ModuleSettings values
					if (ModuleConfig.Alignment.Trim().Length > 0)
					{
						switch (ModuleConfig.Alignment.ToLower())
						{
							case "left":
								return string.Concat(SPACE, ALIGN_LEFT_CLASS);
							case "center":
								return string.Concat(SPACE, ALIGN_CENTER_CLASS);
							case "right":
								return string.Concat(SPACE, ALIGN_RIGHT_CLASS);
						}
					}

					return string.Empty;
				case 1: // None
					return string.Empty;
				case 2: // Left
					return string.Concat(SPACE, ALIGN_LEFT_CLASS);
				case 3: // Center
					return string.Concat(SPACE, ALIGN_CENTER_CLASS);
				case 4: // Right
					return string.Concat(SPACE, ALIGN_RIGHT_CLASS);
				default:
					return string.Empty;
			}
		}

		private string FormatURL(string Link)
		{
			return FormatURL(Link, false, -1, -1);
		}

		private string FormatURL(string Link, bool TrackClicks, int TabId, int ModuleId)
		{
			if (TrackClicks)
			{
				return MediaController.EncodeUrl(DotNetNuke.Common.Globals.LinkClick(Link, TabId, ModuleId, TrackClicks));
			}
			else
			{
				return MediaController.EncodeUrl(Link);
			}
		}

#endregion

	}

}