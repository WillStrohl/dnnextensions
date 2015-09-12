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

Imports WillStrohl.Modules.ContactCollector.EmailTemplateInfoMembers

Namespace WillStrohl.Modules.ContactCollector

    <Serializable()> _
    Public NotInheritable Class EmailTemplateInfo
        Implements IEmailTemplateInfo, Entities.Modules.IHydratable

#Region " Private Members "

        Private p_EmailId As Integer = Null.NullInteger
        Private p_ModuleId As Integer = Null.NullInteger
        Private p_ContactSubject As String = Null.NullString
        Private p_ContactTemplate As String = Null.NullString
        Private p_AdminSubject As String = Null.NullString
        Private p_AdminTemplate As String = Null.NullString

#End Region

#Region " Public Properties "

        Public Property AdminSubject() As String Implements IEmailTemplateInfo.AdminSubject
            Get
                Return Me.p_AdminSubject
            End Get
            Set(ByVal value As String)
                Me.p_AdminSubject = value
            End Set
        End Property

        Public Property AdminTemplate() As String Implements IEmailTemplateInfo.AdminTemplate
            Get
                Return Me.p_AdminTemplate
            End Get
            Set(ByVal value As String)
                Me.p_AdminTemplate = value
            End Set
        End Property

        Public Property ContactSubject() As String Implements IEmailTemplateInfo.ContactSubject
            Get
                Return Me.p_ContactSubject
            End Get
            Set(ByVal value As String)
                Me.p_ContactSubject = value
            End Set
        End Property

        Public Property ContactTemplate() As String Implements IEmailTemplateInfo.ContactTemplate
            Get
                Return Me.p_ContactTemplate
            End Get
            Set(ByVal value As String)
                Me.p_ContactTemplate = value
            End Set
        End Property

        Public Property EmailId() As Integer Implements IEmailTemplateInfo.EmailId
            Get
                Return Me.p_EmailId
            End Get
            Set(ByVal value As Integer)
                Me.p_EmailId = value
            End Set
        End Property

        Public Property ModuleId() As Integer Implements IEmailTemplateInfo.ModuleId
            Get
                Return Me.p_ModuleId
            End Get
            Set(ByVal value As Integer)
                Me.p_ModuleId = value
            End Set
        End Property

#End Region

#Region " IHydratable Implementation "

        Public Sub Fill(ByVal dr As System.Data.IDataReader) Implements DotNetNuke.Entities.Modules.IHydratable.Fill
            Try
                While dr.Read
                    If Not dr.Item(EmailIdField) Is Nothing Then
                        Me.p_EmailId = Integer.Parse(dr.Item(EmailIdField).ToString, Globalization.NumberStyles.Integer)
                    End If
                    If Not dr.Item(ModuleIdField) Is Nothing Then
                        Me.p_ModuleId = Integer.Parse(dr.Item(ModuleIdField).ToString, Globalization.NumberStyles.Integer)
                    End If
                    If Not dr.Item(ContactSubjectField) Is Nothing Then
                        Me.p_ContactSubject = dr.Item(ContactSubjectField).ToString
                    End If
                    If Not dr.Item(ContactTemplateField) Is Nothing Then
                        Me.p_ContactTemplate = dr.Item(ContactTemplateField).ToString
                    End If
                    If Not dr.Item(AdminSubjectField) Is Nothing Then
                        Me.p_AdminSubject = dr.Item(AdminSubjectField).ToString
                    End If
                    If Not dr.Item(AdminTemplateField) Is Nothing Then
                        Me.p_AdminTemplate = dr.Item(AdminTemplateField).ToString
                    End If
                End While
            Catch ex As Exception
                LogException(ex)
            End Try
        End Sub

        Public Property KeyID() As Integer Implements DotNetNuke.Entities.Modules.IHydratable.KeyID
            Get
                Return Me.EmailId
            End Get
            Set(ByVal value As Integer)
                Me.EmailId = value
            End Set
        End Property

#End Region

    End Class

End Namespace