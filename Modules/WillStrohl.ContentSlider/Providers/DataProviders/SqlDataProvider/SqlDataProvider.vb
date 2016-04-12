'
' Will Strohl (will.strohl@gmail.com)
' http://www.willstrohl.com
'
'Copyright (c) 2011-2016, Will Strohl
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
'Neither the name of Will Strohl, Content Slider, nor the names of its contributors may be 
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

Imports Microsoft.ApplicationBlocks.Data

Namespace WillStrohl.Modules.ContentSlider

    Public Class SqlDataProvider
        Inherits DataProvider

#Region " Private Members "

        Private Const ProviderType As String = "data"

        Private p_providerConfiguration As DotNetNuke.Framework.Providers.ProviderConfiguration = DotNetNuke.Framework.Providers.ProviderConfiguration.GetProviderConfiguration(ProviderType)
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
        Private Const c_SProc_Prefix As String = "wns_contentslider_"

        ' sproc names
        Private Const c_AddSlider As String = "AddSlider"
        Private Const c_UpdateSlider As String = "UpdateSlider"
        Private Const c_DeleteSlider As String = "DeleteSlider"
        Private Const c_GetSlider As String = "GetSlider"
        Private Const c_GetSliders As String = "GetSliders"
        Private Const c_GetSlidersForEdit As String = "GetSlidersForEdit"

#End Region

#Region " Constructors "

        ''' <summary>
        ''' Constructor
        ''' </summary>
        ''' <remarks></remarks>
        ''' <history>
        ''' 20101022 - wstrohl - Changed the connection string to use a different method of retrieval
        ''' </history>
        Public Sub New()

	        ' Read the configuration specific information for this provider
	        Dim objProvider As DotNetNuke.Framework.Providers.Provider = DirectCast(p_providerConfiguration.Providers(p_providerConfiguration.DefaultProvider), DotNetNuke.Framework.Providers.Provider)

	        ' Read the attributes for this provider

	        'Get Connection string from web.config
	        p_connectionString = Config.GetConnectionString()

	        If String.IsNullOrEmpty(p_connectionString) Then
		        ' Use connection string specified in provider
		        p_connectionString = objProvider.Attributes("connectionString")
	        End If

	        p_providerPath = objProvider.Attributes("providerPath")

	        p_objectQualifier = objProvider.Attributes("objectQualifier")
	        If Not String.IsNullOrEmpty(p_objectQualifier) AndAlso p_objectQualifier.EndsWith("_", StringComparison.Ordinal) = False Then
		        p_objectQualifier += "_"
	        End If

	        p_databaseOwner = objProvider.Attributes("databaseOwner")
	        If Not String.IsNullOrEmpty(p_databaseOwner) AndAlso p_databaseOwner.EndsWith(".", StringComparison.Ordinal) = False Then
		        p_databaseOwner += "."
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

        Public Overrides Function AddSlider(ByVal ModuleId As Integer, ByVal SliderName As String, ByVal SliderContent As String, ByVal AlternateText As String, ByVal Link As String, ByVal NewWindow As Boolean, ByVal DisplayOrder As Integer, ByVal LastUpdatedBy As Integer, ByVal StartDate As DateTime, ByVal EndDate As DateTime) As Integer
            If EndDate = Null.NullDate Then
                EndDate = DateTime.Parse("1/1/1900")
            End If
            Return CType(SqlHelper.ExecuteScalar(ConnectionString, String.Concat(DatabaseOwner, ObjectQualifier, c_SProc_Prefix, c_AddSlider), ModuleId, GetNull(SliderName), GetNull(SliderContent), GetNull(AlternateText), GetNull(Link), NewWindow, DisplayOrder, LastUpdatedBy, StartDate, EndDate), Integer)
        End Function
        Public Overrides Sub DeleteSlider(ByVal SliderId As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, String.Concat(DatabaseOwner, ObjectQualifier, c_SProc_Prefix, c_DeleteSlider), SliderId)
        End Sub
        Public Overrides Function GetSliders(ByVal ModuleId As Integer) As System.Data.IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, String.Concat(DatabaseOwner, ObjectQualifier, c_SProc_Prefix, c_GetSliders), ModuleId), IDataReader)
        End Function
        Public Overrides Function GetSlidersForEdit(ByVal ModuleId As Integer) As System.Data.IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, String.Concat(DatabaseOwner, ObjectQualifier, c_SProc_Prefix, c_GetSlidersForEdit), ModuleId), IDataReader)
        End Function
        Public Overrides Function GetSlider(ByVal SliderId As Integer) As System.Data.IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, String.Concat(DatabaseOwner, ObjectQualifier, c_SProc_Prefix, c_GetSlider), SliderId), IDataReader)
        End Function
        Public Overrides Sub UpdateSlider(ByVal SliderId As Integer, ByVal ModuleId As Integer, ByVal SliderName As String, ByVal SliderContent As String, ByVal AlternateText As String, ByVal Link As String, ByVal NewWindow As Boolean, ByVal DisplayOrder As Integer, ByVal LastUpdatedBy As Integer, ByVal StartDate As DateTime, ByVal EndDate As DateTime)
            If EndDate = Null.NullDate Then
                EndDate = DateTime.Parse("1/1/1900")
            End If
            SqlHelper.ExecuteNonQuery(ConnectionString, String.Concat(DatabaseOwner, ObjectQualifier, c_SProc_Prefix, c_UpdateSlider), SliderId, ModuleId, GetNull(SliderName), GetNull(SliderContent), GetNull(AlternateText), GetNull(Link), NewWindow, DisplayOrder, LastUpdatedBy, StartDate, EndDate)
        End Sub

#End Region

#Region " Private Helper Methods "

        Private Function GetNull(ByVal Field As Object) As Object
            Return DotNetNuke.Common.Utilities.Null.GetNull(Field, DBNull.Value)
        End Function

#End Region

    End Class

End Namespace