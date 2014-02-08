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

Imports DotNetNuke.Entities.Modules
Imports WillStrohl.Modules.ContactCollector.ContactInfoMembers

Namespace WillStrohl.Modules.ContactCollector

    <Serializable()> _
    Public NotInheritable Class ContactInfo
        Implements IContactInfo, IHydratable

#Region " Private Members "

        Private p_ContactId As Integer = Null.NullInteger
        Private p_ModuleId As Integer = Null.NullInteger
        Private p_FirstName As String = Null.NullString
        Private p_LastName As String = Null.NullString
        Private p_EmailAddress As String = Null.NullString
        Private p_IsActive As Boolean = Null.NullBoolean
        Private p_Comment As String = Null.NullString

#End Region

#Region " Public Properties "

        Public Property ContactId() As Integer Implements IContactInfo.ContactId
            Get
                Return Me.p_ContactId
            End Get
            Set(ByVal value As Integer)
                Me.p_ContactId = value
            End Set
        End Property

        Public Property EmailAddress() As String Implements IContactInfo.EmailAddress
            Get
                Return Me.p_EmailAddress
            End Get
            Set(ByVal value As String)
                Me.p_EmailAddress = value
            End Set
        End Property

        Public Property FirstName() As String Implements IContactInfo.FirstName
            Get
                Return Me.p_FirstName
            End Get
            Set(ByVal value As String)
                Me.p_FirstName = value
            End Set
        End Property

        Public Property IsActive() As Boolean Implements IContactInfo.IsActive
            Get
                Return Me.p_IsActive
            End Get
            Set(ByVal value As Boolean)
                Me.p_IsActive = value
            End Set
        End Property

        Public Property LastName() As String Implements IContactInfo.LastName
            Get
                Return Me.p_LastName
            End Get
            Set(ByVal value As String)
                Me.p_LastName = value
            End Set
        End Property

        Public Property ModuleId() As Integer Implements IContactInfo.ModuleId
            Get
                Return Me.p_ModuleId
            End Get
            Set(ByVal value As Integer)
                Me.p_ModuleId = value
            End Set
        End Property

        Public Property Comment() As String Implements IContactInfo.Comment
            Get
                Return Me.p_Comment
            End Get
            Set(ByVal value As String)
                Me.p_Comment = value
            End Set
        End Property

#End Region

#Region " IHydratable Implementation "

        Public Sub Fill(ByVal dr As System.Data.IDataReader) Implements IHydratable.Fill
            Try
                While dr.Read
                    If Not dr.Item(ContactIdField) Is Nothing Then
                        Me.p_ContactId = Integer.Parse(dr.Item(ContactIdField).ToString, Globalization.NumberStyles.Integer)
                    End If
                    If Not dr.Item(ModuleIdField) Is Nothing Then
                        If RegExUtility.IsNumber(dr.Item(ModuleIdField)) Then
                            Me.p_ModuleId = Integer.Parse(dr.Item(ModuleIdField).ToString, Globalization.NumberStyles.Integer)
                        End If
                    End If
                    If Not dr.Item(FirstNameField) Is Nothing Then
                        Me.p_FirstName = dr.Item(FirstNameField).ToString
                    End If
                    If Not dr.Item(LastNameField) Is Nothing Then
                        Me.p_LastName = dr.Item(LastNameField).ToString
                    End If
                    If Not dr.Item(EmailAddressField) Is Nothing Then
                        Me.p_EmailAddress = dr.Item(EmailAddressField).ToString
                    End If
                    If Not dr.Item(IsActiveField) Is Nothing Then
                        If RegExUtility.IsBoolean(dr.Item(IsActiveField)) Then
                            Me.p_IsActive = Boolean.Parse(dr.Item(IsActiveField).ToString)
                        End If
                    End If
                    If Not dr.Item(CommentField) Is Nothing Then
                        Me.p_Comment = dr.Item(CommentField).ToString
                    End If
                End While
            Catch ex As Exception
                LogException(ex)
            Finally
                If Not dr.IsClosed Then
                    dr.Close()
                End If
            End Try
        End Sub

        Public Property KeyID() As Integer Implements IHydratable.KeyID
            Get
                Return Me.p_ModuleId
            End Get
            Set(ByVal value As Integer)
                Me.p_ModuleId = value
            End Set
        End Property

#End Region

    End Class

End Namespace