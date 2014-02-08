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

	/// -----------------------------------------------------------------------------
	/// <summary>
	/// The DataProvider Class Is an abstract class that provides the DataLayer
	/// for the Image Module.
	/// </summary>
	/// <remarks>
	/// </remarks>
	/// <history>
	///		[lpointer]	31.10.2005	documented
	/// </history>
	/// -----------------------------------------------------------------------------
	public abstract class DataProvider
	{

#region  Constants 

		private const string p_ObjectProviderType = "data";
		private const string p_ObjectNamespace = "DotNetNuke.Modules.Media";
		private const string p_ObjectAssemblyName = "DotNetNuke.Modules.Media";

#endregion

#region  Shared/Static Methods 

		// singleton reference to the instantiated object 
		private static DataProvider objProvider = null;

		// constructor
		static DataProvider()
		{
			CreateProvider();
		}

		// dynamically create provider
		private static void CreateProvider()
		{
			objProvider = (DataProvider)(DotNetNuke.Framework.Reflection.CreateObject(p_ObjectProviderType, p_ObjectNamespace, p_ObjectAssemblyName));
		}

		// return the provider
		public static new DataProvider Instance()
		{
			return objProvider;
		}

#endregion

#region  Abstract Methods 

		public abstract void AddMedia(int ModuleId, string Src, string Alt, int Width, int Height, string NavigateUrl, int MediaAlignment, bool AutoStart, bool MediaLoop, bool NewWindow, bool TrackClicks, int MediaType, string MediaMessage, int LastUpdatedBy);
		public abstract IDataReader GetMedia(int ModuleId);
		public abstract void UpdateMedia(int ModuleId, string src, string alt, int width, int height, string navigateUrl, int MediaAlignment, bool AutoStart, bool MediaLoop, bool NewWindow, bool TrackClicks, int MediaType, string MediaMessage, int LastUpdatedBy);
		public abstract void UpgradeMedia(int OldModuleDefID, int NewModuleDefID);
		public abstract void DeleteMedia(int ModuleId);

#endregion

	}

}