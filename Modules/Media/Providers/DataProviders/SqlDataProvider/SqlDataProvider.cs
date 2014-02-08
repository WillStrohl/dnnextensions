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
using System;
using System.Collections;
using System.Data;
using System.Diagnostics;

using Microsoft.ApplicationBlocks.Data;

namespace DotNetNuke.Modules.Media
{

	/// -----------------------------------------------------------------------------
	/// <summary>
	/// The SqlDataProvider Class is an SQL Server implementation of the DataProvider Abstract
	/// class that provides the DataLayer for the HTML Module.
	/// </summary>
	/// <remarks>
	/// </remarks>
	/// <history>
	/// 	[cnurse]	9/21/2004	Moved HTML to a separate Project
	/// </history>
	/// -----------------------------------------------------------------------------
	public class SqlDataProvider : DataProvider
	{

#region  Private Members 

		private const string ProviderType = "data";

		private Framework.Providers.ProviderConfiguration p_providerConfiguration = Framework.Providers.ProviderConfiguration.GetProviderConfiguration(ProviderType);
		private string p_connectionString = string.Empty;
		private string p_providerPath = string.Empty;
		private string p_objectQualifier = string.Empty;
		private string p_databaseOwner = string.Empty;

		private const string c_ConnectionStringName = "connectionStringName";
		private const string c_ConnectionString = "connectionString";

		private const string c_AddMedia = "AddMedia";
		private const string c_GetMedia = "GetMedia";
		private const string c_UpdateMedia = "UpdateMedia";
		private const string c_UpgradeMedia = "UpgradeMedia";
		private const string c_DeleteMedia = "DeleteMedia";

#endregion

#region  Constructors 

		public SqlDataProvider()
		{

			// Read the configuration specific information for this provider
			Framework.Providers.Provider objProvider = (Framework.Providers.Provider)(p_providerConfiguration.Providers[p_providerConfiguration.DefaultProvider]);

			// Read the attributes for this provider
			if (! (string.IsNullOrEmpty(objProvider.Attributes[c_ConnectionStringName])))
			{
				p_connectionString = DotNetNuke.Common.Utilities.Config.GetConnectionString();
			}
			else
			{
				p_connectionString = objProvider.Attributes[c_ConnectionString];
			}

			p_providerPath = objProvider.Attributes["providerPath"];

			p_objectQualifier = objProvider.Attributes["objectQualifier"];
			if (! (string.IsNullOrEmpty(p_objectQualifier)) && p_objectQualifier.EndsWith("_") == false)
			{
				p_objectQualifier = string.Concat(p_objectQualifier, "_");
			}

			p_databaseOwner = objProvider.Attributes["databaseOwner"];
			if (! (string.IsNullOrEmpty(p_databaseOwner)) && p_databaseOwner.EndsWith(".") == false)
			{
				p_databaseOwner = string.Concat(p_databaseOwner, ".");
			}

		}

#endregion

#region  Properties 

		public string ConnectionString
		{
			get
			{
				return p_connectionString;
			}
		}

		public string ProviderPath
		{
			get
			{
				return p_providerPath;
			}
		}

		public string ObjectQualifier
		{
			get
			{
				return p_objectQualifier;
			}
		}

		public string DatabaseOwner
		{
			get
			{
				return p_databaseOwner;
			}
		}

#endregion

#region  Public Methods 

		private object GetNull(object Field)
		{
			return Common.Utilities.Null.GetNull(Field, DBNull.Value);
		}

		public override void AddMedia(int ModuleId, string Src, string Alt, int Width, int Height, string NavigateUrl, int MediaAlignment, bool AutoStart, bool MediaLoop, bool NewWindow, bool TrackClicks, int MediaType, string MediaMessage, int LastUpdatedBy)
		{
			SqlHelper.ExecuteNonQuery(ConnectionString, string.Concat(DatabaseOwner, ObjectQualifier, c_AddMedia), ModuleId, Src, GetNull(Alt), GetNull(Width), GetNull(Height), GetNull(NavigateUrl), MediaAlignment, AutoStart, MediaLoop, NewWindow, TrackClicks, MediaType, MediaMessage, LastUpdatedBy);
		}

		public override IDataReader GetMedia(int moduleId)
		{
			return (IDataReader)(SqlHelper.ExecuteReader(ConnectionString, string.Concat(DatabaseOwner, ObjectQualifier, c_GetMedia), moduleId));
		}

		public override void UpdateMedia(int ModuleId, string Src, string Alt, int Width, int Height, string NavigateUrl, int MediaAlignment, bool AutoStart, bool MediaLoop, bool NewWindow, bool TrackClicks, int MediaType, string MediaMessage, int LastUpdatedBy)
		{
			SqlHelper.ExecuteNonQuery(ConnectionString, string.Concat(DatabaseOwner, ObjectQualifier, c_UpdateMedia), ModuleId, Src, GetNull(Alt), GetNull(Width), GetNull(Height), GetNull(NavigateUrl), MediaAlignment, AutoStart, MediaLoop, NewWindow, TrackClicks, MediaType, MediaMessage, LastUpdatedBy);
		}

		public override void UpgradeMedia(int OldModuleDefID, int NewModuleDefID)
		{
			SqlHelper.ExecuteNonQuery(ConnectionString, string.Concat(DatabaseOwner, ObjectQualifier, c_UpgradeMedia), OldModuleDefID, NewModuleDefID);
		}

		public override void DeleteMedia(int ModuleId)
		{
			SqlHelper.ExecuteNonQuery(ConnectionString, string.Concat(DatabaseOwner, ObjectQualifier, c_DeleteMedia), ModuleId);
		}

#endregion

	}

}