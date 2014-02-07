/*
 * Copyright (c) 2011, Will Strohl
 * All rights reserved.
 * 
 * Redistribution and use in source and binary forms, with or without modification, 
 * are permitted provided that the following conditions are met:
 * 
 * Redistributions of source code must retain the above copyright notice, this list 
 * of conditions and the following disclaimer.
 * 
 * Redistributions in binary form must reproduce the above copyright notice, this 
 * list of conditions and the following disclaimer in the documentation and/or 
 * other materials provided with the distribution.
 * 
 * Neither the name of WillStrohl.com nor the names of its contributors may be used 
 * to endorse or promote products derived from this software without specific prior 
 * written permission.
 * 
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND 
 * ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED 
 * WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. 
 * IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, 
 * INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, 
 * BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, 
 * DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF 
 * LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE 
 * OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF 
 * ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/

using System;
using System.Data;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Framework.Providers;
using Microsoft.ApplicationBlocks.Data;

namespace DotNetNuke.Modules.WillStrohlDisqus
{

    /// -----------------------------------------------------------------------------
    /// <summary>
    /// SQL Server implementation of the abstract DataProvider class
    /// </summary>
    /// -----------------------------------------------------------------------------
    public class SqlDataProvider : DataProvider
    {

        #region Private Members

        private const string ProviderType = "data";
        private const string ModuleQualifier = "wnsDisqus_";

        private readonly ProviderConfiguration _providerConfiguration = ProviderConfiguration.GetProviderConfiguration(ProviderType);
        private readonly string _connectionString;
        private readonly string _providerPath;
        private readonly string _objectQualifier;
        private readonly string _databaseOwner;

        #endregion

        #region Constructors

        public SqlDataProvider()
        {

            // Read the configuration specific information for this provider
            Provider objProvider = (Provider)(_providerConfiguration.Providers[_providerConfiguration.DefaultProvider]);

            // Read the attributes for this provider

            //Get Connection string from web.config
            _connectionString = Config.GetConnectionString();

            if (string.IsNullOrEmpty(_connectionString))
            {
                // Use connection string specified in provider
                _connectionString = objProvider.Attributes["connectionString"];
            }

            _providerPath = objProvider.Attributes["providerPath"];

            _objectQualifier = objProvider.Attributes["objectQualifier"];
            if (!string.IsNullOrEmpty(_objectQualifier) && _objectQualifier.EndsWith("_", StringComparison.Ordinal) == false)
            {
                _objectQualifier += "_";
            }

            _databaseOwner = objProvider.Attributes["databaseOwner"];
            if (!string.IsNullOrEmpty(_databaseOwner) && _databaseOwner.EndsWith(".", StringComparison.Ordinal) == false)
            {
                _databaseOwner += ".";
            }

        }

        #endregion

        #region Properties

        public string ConnectionString
        {
            get
            {
                return _connectionString;
            }
        }

        public string ProviderPath
        {
            get
            {
                return _providerPath;
            }
        }

        public string ObjectQualifier
        {
            get
            {
                return _objectQualifier;
            }
        }

        public string DatabaseOwner
        {
            get
            {
                return _databaseOwner;
            }
        }

        private string NamePrefix
        {
            get { return DatabaseOwner + ObjectQualifier + ModuleQualifier; }
        }

        #endregion

        #region Private Methods

        private static object GetNull(object Field)
        {
            return Null.GetNull(Field, DBNull.Value);
        }

        #endregion

        #region Public Methods

        public override int AddDisqus(int PortalId, int TabId, int TabModuleId, string CommentPath, string DisqusComment, string CreatedAt)
        {
            return Convert.ToInt32(SqlHelper.ExecuteScalar(ConnectionString, NamePrefix + "AddDisqus",PortalId, TabId, TabModuleId, CommentPath, DisqusComment, CreatedAt));
        }

        public override void UpdateDisqus(int LocalDbId, int PortalId, int TabId, int TabModuleId, string CommentPath, string DisqusComment, string CreatedAt)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, NamePrefix + "UpdateDisqus", LocalDbId, PortalId, TabId, TabModuleId, CommentPath, DisqusComment, CreatedAt);
        }

        public override void DeleteDisqus(int LocalDbId)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, NamePrefix + "DeleteDisqus", LocalDbId);
        }

        public override void DeleteDisqusbyPortal(int PortalId)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, NamePrefix + "DeleteDisqusByPortal", PortalId);
        }

        public override void DeleteDisqusByTab(int TabId)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, NamePrefix + "DeleteDisqusByTab", TabId);
        }

        public override IDataReader GetDisqus()
        {
            return SqlHelper.ExecuteReader(ConnectionString, NamePrefix + "GetDisqus");
        }

        public override IDataReader GetDisqusByPortal(int PortalId)
        {
            return SqlHelper.ExecuteReader(ConnectionString, NamePrefix + "GetDisqusByPortal", PortalId);
        }

        public override IDataReader GetDisqusByTab(int TabId)
        {
            return SqlHelper.ExecuteReader(ConnectionString, NamePrefix + "GetDisqusByTab", TabId);
        }

        public override IDataReader GetDisqusByModule(int TabModuleId)
        {
            return SqlHelper.ExecuteReader(ConnectionString, NamePrefix + "GetDisqusByModule", TabModuleId);
        }

        #endregion

    }

}