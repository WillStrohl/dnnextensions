//
// Will Strohl (will.strohl@gmail.com)
// http://www.willstrohl.com
//
//Copyright (c) 2009-2016, Will Strohl
//All rights reserved.
//
//Redistribution and use in source and binary forms, with or without modification, are 
//permitted provided that the following conditions are met:
//
//Redistributions of source code must retain the above copyright notice, this list of 
//conditions and the following disclaimer.
//
//Redistributions in binary form must reproduce the above copyright notice, this list 
//of conditions and the following disclaimer in the documentation and/or other 
//materials provided with the distribution.
//
//Neither the name of Will Strohl, Content Injection, nor the names of its contributors may be 
//used to endorse or promote products derived from this software without specific prior 
//written permission.
//
//THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY 
//EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES 
//OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT 
//SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, 
//INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED 
//TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR 
//BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN 
//CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN 
//ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH 
//DAMAGE.
//

using System;
using System.Data;
using DotNetNuke.Framework.Providers;
using Microsoft.ApplicationBlocks.Data;

namespace WillStrohl.Modules.Injection.Components
{
	public class SqlDataProvider : DataProvider
	{
		#region " Private Members "

		private const string ProviderType = "data";
		private DotNetNuke.Framework.Providers.ProviderConfiguration p_providerConfiguration = DotNetNuke.Framework.Providers.ProviderConfiguration.GetProviderConfiguration(ProviderType);
		private string p_connectionString = string.Empty;
		private string p_providerPath = string.Empty;
		private string p_objectQualifier = string.Empty;

		private string p_databaseOwner = string.Empty;
		// local string values
		private const string c_ConnectionStringName = "connectionStringName";
		private const string c_ConnectionString = "connectionString";
		private const string c_ProviderPath = "providerPath";
		private const string c_ObjectQualifier = "objectQualifier";
		private const string c_Underscore = "_";
		private const string c_DatabaseOwner = "databaseOwner";
		private const string c_Period = ".";

		private const string c_SProc_Prefix = "wns_inj_";
		// sproc names
		private const string c_AddInjectionContent = "AddInjectionContent";
		private const string c_UpdateInjectionContent = "UpdateInjectionContent";
		private const string c_DisableInjectionContent = "DisableInjectionContent";
		private const string c_EnableInjectionContent = "EnableInjectionContent";
		private const string c_DeleteInjectionContent = "DeleteInjectionContent";
		private const string c_GetInjectionContent = "GetInjectionContent";
		private const string c_GetActiveInjectionContents = "GetActiveInjectionContents";
		private const string c_GetInjectionContents = "GetInjectionContents";
		private const string c_GetNextOrderNumber = "GetNextOrderNumber";
		private const string c_ChangeOrder = "ChangeOrder";

		private const string c_DoesInjectionNameExist = "DoesInjectionNameExist";
		#endregion

		#region " Constructors "


		public SqlDataProvider()
		{
			// Read the configuration specific information for this provider
			var objProvider = (Provider)p_providerConfiguration.Providers[p_providerConfiguration.DefaultProvider];

			// Read the attributes for this provider
			if (!string.IsNullOrEmpty(objProvider.Attributes[c_ConnectionStringName])) {
				p_connectionString = DotNetNuke.Common.Utilities.Config.GetConnectionString();
			} else {
				p_connectionString = objProvider.Attributes[c_ConnectionString];
			}

			p_providerPath = objProvider.Attributes[c_ProviderPath];

			p_objectQualifier = objProvider.Attributes[c_ObjectQualifier];
			if (!string.IsNullOrEmpty(p_objectQualifier) & p_objectQualifier.EndsWith(c_Underscore) == false) {
				p_objectQualifier = string.Concat(p_objectQualifier, c_Underscore);
			}

			// Add willstrohl_ to the beginning of the sprocs
			p_objectQualifier = string.Concat(p_objectQualifier, c_SProc_Prefix);

			p_databaseOwner = objProvider.Attributes[c_DatabaseOwner];
			if (!string.IsNullOrEmpty(p_databaseOwner) & p_databaseOwner.EndsWith(c_Period) == false) {
				p_databaseOwner = string.Concat(p_databaseOwner, c_Period);
			}

		}

		#endregion

		#region " Properties "

		public string ConnectionString {
			get { return p_connectionString; }
		}

		public string ProviderPath {
			get { return p_providerPath; }
		}

		public string ObjectQualifier {
			get { return p_objectQualifier; }
		}

		public string DatabaseOwner {
			get { return p_databaseOwner; }
		}

		#endregion

		#region " Data Provider Implementation "

		public override int AddInjectionContent(int ModuleId, bool InjectTop, string InjectName, string InjectContent, bool IsEnabled, int OrderShown, string CustomProperties)
		{
			return SqlHelper.ExecuteNonQuery(ConnectionString, string.Concat(DatabaseOwner, ObjectQualifier, c_AddInjectionContent), ModuleId, InjectTop, GetNull(InjectName), GetNull(InjectContent), IsEnabled, OrderShown, CustomProperties);
		}

		public override void DeleteInjectionContent(int InjectionId)
		{
			SqlHelper.ExecuteNonQuery(ConnectionString, string.Concat(DatabaseOwner, ObjectQualifier, c_DeleteInjectionContent), InjectionId);
		}

		public override void DisableInjectionContent(int InjectionId)
		{
			SqlHelper.ExecuteNonQuery(ConnectionString, string.Concat(DatabaseOwner, ObjectQualifier, c_DisableInjectionContent), InjectionId);
		}

		public override void EnableInjectionContent(int InjectionId)
		{
			SqlHelper.ExecuteNonQuery(ConnectionString, string.Concat(DatabaseOwner, ObjectQualifier, c_EnableInjectionContent), InjectionId);
		}

		public override IDataReader GetActiveInjectionContents(int ModuleId)
		{
			return (IDataReader)SqlHelper.ExecuteReader(ConnectionString, string.Concat(DatabaseOwner, ObjectQualifier, c_GetActiveInjectionContents), ModuleId);
		}

		public override IDataReader GetInjectionContent(int InjectionId)
		{
			return (IDataReader)SqlHelper.ExecuteReader(ConnectionString, string.Concat(DatabaseOwner, ObjectQualifier, c_GetInjectionContent), InjectionId);
		}

		public override IDataReader GetInjectionContents(int ModuleId)
		{
			return (IDataReader)SqlHelper.ExecuteReader(ConnectionString, string.Concat(DatabaseOwner, ObjectQualifier, c_GetInjectionContents), ModuleId);
		}

		public override void UpdateInjectionContent(int InjectionId, int ModuleId, bool InjectTop, string InjectName, string InjectContent, bool IsEnabled, int OrderShown, string CustomProperties)
		{
			SqlHelper.ExecuteNonQuery(ConnectionString, string.Concat(DatabaseOwner, ObjectQualifier, c_UpdateInjectionContent), InjectionId, ModuleId, InjectTop, GetNull(InjectName), GetNull(InjectContent), IsEnabled, OrderShown, CustomProperties);
		}

		public override int GetNextOrderNumber(int ModuleId)
		{
			// WStrohl - 20090307 
			// The DNN Module Installer removes plus signs from the DB scripts. So the addition 
			// is done here as a workaround.
			return (Convert.ToInt32(SqlHelper.ExecuteScalar(ConnectionString, string.Concat(DatabaseOwner, ObjectQualifier, c_GetNextOrderNumber), ModuleId)) + 1);
		}

		public override void ChangeOrder(int InjectionId, string Direction)
		{
			SqlHelper.ExecuteNonQuery(ConnectionString, string.Concat(DatabaseOwner, ObjectQualifier, c_ChangeOrder), InjectionId, Direction);
		}

		public override bool DoesInjectionNameExist(string InjectionName, int ModuleId)
		{
			return Convert.ToBoolean(SqlHelper.ExecuteScalar(ConnectionString, string.Concat(DatabaseOwner, ObjectQualifier, c_DoesInjectionNameExist), InjectionName, ModuleId));
		}

		#endregion

		#region " Private Helper Methods "

		private object GetNull(object Field)
		{
			return DotNetNuke.Common.Utilities.Null.GetNull(Field, DBNull.Value);
		}

		#endregion
	}
}