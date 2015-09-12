'
' Will Strohl (will.strohl@gmail.com)
' http://www.willstrohl.com
'
'Copyright (c) 2015, Will Strohl
'All rights reserved.
'
'Redistribution and use in source and binary forms, with or without modification, are 
'permitted provided that the following conditions are met:
'
'Redistributions of source code must retain the above copyright notice, this list of 
'conditions and the following disclaimer.
'
'Redistributions in binary form must reproduce the above copyright notice, this list 
'of conditions and the following disclaimer in the documentation and/or other 
'materials provided with the distribution.
'
'Neither the name of Will Strohl, Contact Collector, nor the names of its contributors may be 
'used to endorse or promote products derived from this software without specific prior 
'written permission.
'
'THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY 
'EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES 
'OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT 
'SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, 
'INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED 
'TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR 
'BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN 
'CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN 
'ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH 
'DAMAGE.
'

Imports DotNetNuke
Imports DotNetNuke.Common
Imports System
Imports System.Data
Imports Microsoft.ApplicationBlocks.Data

Namespace WillStrohl.Modules.ContactCollector

    Public Class SqlDataProvider
        Inherits DataProvider

#Region " Private Members "

        Private Const ProviderType As String = "data"

        Private p_providerConfiguration As Framework.Providers.ProviderConfiguration = Framework.Providers.ProviderConfiguration.GetProviderConfiguration(ProviderType)
        Private p_connectionString As String = String.Empty
        Private p_providerPath As String = String.Empty
        Private p_objectQualifier As String = String.Empty
        Private p_databaseOwner As String = String.Empty

        ' local string values
        Private Const c_ConnectionStringName As String = "connectionStringName"
        Private Const c_ConnectionString As String = "connectionString"
        Private Const c_ProviderPath As String = "providerPath"
        Private Const c_ObjectQualifier As String = "objectQualifier"
        Private Const c_Underscore As String = "_"
        Private Const c_DatabaseOwner As String = "databaseOwner"
        Private Const c_Period As String = "."
        Private Const c_SProc_Prefix As String = "wns_cc_"

        ' sproc names
        Private Const c_GetContact As String = "GetContact"
        Private Const c_GetContacts As String = "GetContacts"
        Private Const c_AddContact As String = "AddContact"
        Private Const c_UpdateContact As String = "UpdateContact"
        Private Const c_DeleteContact As String = "DeleteContact"
        Private Const c_GetEmailTemplate As String = "GetEmailTemplate"
        Private Const c_UpdateEmailTemplate As String = "UpdateEmailTemplate"

#End Region

#Region " Constructors "

        Public Sub New()

            ' Read the configuration specific information for this provider
            Dim objProvider As Framework.Providers.Provider = CType(p_providerConfiguration.Providers(p_providerConfiguration.DefaultProvider), Framework.Providers.Provider)

            ' Read the attributes for this provider
            If Not String.IsNullOrEmpty(objProvider.Attributes(c_ConnectionStringName)) Then
                p_connectionString = Utilities.Config.GetConnectionString
            Else
                p_connectionString = objProvider.Attributes(c_ConnectionString)
            End If

            p_providerPath = objProvider.Attributes(c_ProviderPath)

            p_objectQualifier = objProvider.Attributes(c_ObjectQualifier)
            If Not String.IsNullOrEmpty(p_objectQualifier) And p_objectQualifier.EndsWith(c_Underscore) = False Then
                p_objectQualifier = String.Concat(p_objectQualifier, c_Underscore)
            End If

            ' Add willstrohl_ to the beginning of the sprocs
            p_objectQualifier = String.Concat(p_objectQualifier, c_SProc_Prefix)

            p_databaseOwner = objProvider.Attributes(c_DatabaseOwner)
            If Not String.IsNullOrEmpty(p_databaseOwner) And p_databaseOwner.EndsWith(c_Period) = False Then
                p_databaseOwner = String.Concat(p_databaseOwner, c_Period)
            End If

        End Sub

#End Region

#Region " Properties "

        Public ReadOnly Property ConnectionString() As String
            Get
                Return p_connectionString
            End Get
        End Property

        Public ReadOnly Property ProviderPath() As String
            Get
                Return p_providerPath
            End Get
        End Property

        Public ReadOnly Property ObjectQualifier() As String
            Get
                Return p_objectQualifier
            End Get
        End Property

        Public ReadOnly Property DatabaseOwner() As String
            Get
                Return p_databaseOwner
            End Get
        End Property

#End Region

#Region " Data Provider Implementation "

        ' contacts
        Public Overrides Function AddContact(ByVal ModuleId As Integer, ByVal FirstName As String, ByVal LastName As String, ByVal EmailAddress As String, ByVal IsActive As Boolean, ByVal Comment As String) As Integer
            Return CType(SqlHelper.ExecuteScalar(ConnectionString, String.Concat(DatabaseOwner, ObjectQualifier, c_AddContact), ModuleId, GetNull(FirstName), GetNull(LastName), GetNull(EmailAddress), IsActive, GetNull(Comment)), Integer)
        End Function
        Public Overrides Sub DeleteContact(ByVal ContactId As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, String.Concat(DatabaseOwner, ObjectQualifier, c_DeleteContact), ContactId)
        End Sub
        Public Overrides Function GetContact(ByVal ContactId As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, String.Concat(DatabaseOwner, ObjectQualifier, c_GetContact), ContactId), IDataReader)
        End Function
        Public Overrides Function GetContacts(ByVal ModuleId As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, String.Concat(DatabaseOwner, ObjectQualifier, c_GetContacts), ModuleId), IDataReader)
        End Function
        Public Overrides Sub UpdateContact(ByVal ContactId As Integer, ByVal ModuleId As Integer, ByVal FirstName As String, ByVal LastName As String, ByVal EmailAddress As String, ByVal IsActive As Boolean, ByVal Comment As String)
            SqlHelper.ExecuteNonQuery(ConnectionString, String.Concat(DatabaseOwner, ObjectQualifier, c_UpdateContact), ContactId, ModuleId, GetNull(FirstName), GetNull(LastName), GetNull(EmailAddress), IsActive, Comment)
        End Sub

        ' email template
        Public Overrides Function GetEmailTemplate(ByVal ModuleId As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, String.Concat(DatabaseOwner, ObjectQualifier, c_GetEmailTemplate), ModuleId), IDataReader)
        End Function
        Public Overrides Sub UpdateEmailTemplate(ByVal EmailId As Integer, ByVal ModuleId As Integer, ByVal ContactSubject As String, ByVal ContactTemplate As String, ByVal AdminSubject As String, ByVal AdminTemplate As String)
            SqlHelper.ExecuteNonQuery(ConnectionString, String.Concat(DatabaseOwner, ObjectQualifier, c_UpdateEmailTemplate), EmailId, ModuleId, ContactSubject, ContactTemplate, AdminSubject, AdminTemplate)
        End Sub

#End Region

#Region " Private Helper Methods "

        Private Function GetNull(ByVal Field As Object) As Object
            Return DotNetNuke.Common.Utilities.Null.GetNull(Field, DBNull.Value)
        End Function

#End Region

    End Class

End Namespace