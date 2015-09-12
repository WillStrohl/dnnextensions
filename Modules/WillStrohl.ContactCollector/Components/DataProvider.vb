'
' Will Strohl (will.strohl@gmail.com)
' http://www.willstrohl.com
'
'Copyright (c) 2013, Will Strohl
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

Imports DotNetNuke.Common.Utilities
Imports System
Imports System.Data

Namespace WillStrohl.Modules.ContactCollector

    Public MustInherit Class DataProvider

#Region " Private Members "

        Private Const c_AssemblyName As String = "WillStrohl.Modules.ContactCollector.SqlDataProvider, WillStrohl.Modules.ContactCollector"

#End Region

#Region " Shared/Static Methods "

        ' singleton reference to the instantiated object 
        Private Shared objProvider As DataProvider = Nothing

        ' constructor
        Shared Sub New()
            CreateProvider()
        End Sub

        ' dynamically create provider
        Private Shared Sub CreateProvider()
            If objProvider IsNot Nothing Then
                Return
            End If

            Dim objectType As Type = Type.GetType(c_AssemblyName)

            objProvider = CType(Activator.CreateInstance(objectType), DataProvider)

            DataCache.SetCache(objectType.FullName, objProvider)
        End Sub

        ' return the provider
        Public Shared Shadows Function Instance() As DataProvider
            If objProvider Is Nothing Then
                CreateProvider()
            End If

            Return objProvider
        End Function

#End Region

#Region " Abstract Methods "

        ' Contacts
        Public MustOverride Function GetContact(ByVal ContactId As Integer) As IDataReader
        Public MustOverride Function GetContacts(ByVal ModuleId As Integer) As IDataReader
        Public MustOverride Function AddContact(ByVal ModuleId As Integer, ByVal FirstName As String, ByVal LastName As String, ByVal EmailAddress As String, ByVal IsActive As Boolean, ByVal Comment As String) As Integer
        Public MustOverride Sub UpdateContact(ByVal ContactId As Integer, ByVal ModuleId As Integer, ByVal FirstName As String, ByVal LastName As String, ByVal EmailAddress As String, ByVal IsActive As Boolean, ByVal Comment As String)
        Public MustOverride Sub DeleteContact(ByVal ContactId As Integer)

        ' Email Template
        Public MustOverride Function GetEmailTemplate(ByVal ModuleId As Integer) As IDataReader
        Public MustOverride Sub UpdateEmailTemplate(ByVal EmailId As Integer, ByVal ModuleId As Integer, ByVal ContactSubject As String, ByVal ContactTemplate As String, ByVal AdminSubject As String, ByVal AdminTemplate As String)

#End Region

    End Class

End Namespace