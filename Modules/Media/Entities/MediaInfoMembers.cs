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

namespace DotNetNuke.Modules.Media
{

	public sealed class MediaInfoMembers
	{

		public const string ModuleIdField = "ModuleID";
		public const string SrcField = "Src";
		public const string AltField = "alt";
		public const string WidthField = "width";
		public const string HeightField = "height";
		public const string NavigateUrlField = "NavigateUrl";
		public const string MediaAlignmentField = "MediaAlignment";
		//
		public const string NewWindowField = "NewWindow";
		public const string TrackClicksField = "TrackClicks";
		//
		public const string MediaLoopField = "MediaLoop";
		public const string AutoStartField = "AutoStart";
		//
		public const string MediaTypeField = "MediaType";
		//
		public const string MediaMessageField = "MediaMessage";
		public const string LastUpdatedByField = "LastUpdatedBy";
		public const string LastUpdatedDateField = "LastUpdatedDate";

	}

}