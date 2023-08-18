/*
Copyright © Upendo Ventures, LLC

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and 
associated documentation files (the "Software"), to deal in the Software without restriction, 
including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, 
and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, 
subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial 
portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT 
NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND 
NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES 
OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN 
CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 */

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