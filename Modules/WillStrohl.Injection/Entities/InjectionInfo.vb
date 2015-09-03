'
' Will Strohl (will.strohl@gmail.com)
' http://www.willstrohl.com
'
'Copyright (c) 2009-2015, Will Strohl
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
'Neither the name of Will Strohl, Content Injection, nor the names of its contributors may be 
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
Imports DotNetNuke.Entities.Modules
Imports DotNetNuke.Services.Exceptions

Imports WillStrohl.Modules.Injection.InjectionInfoMembers

Namespace WillStrohl.Modules.Injection

    <Serializable()> _
    Public NotInheritable Class InjectionInfo
        Implements IInjectionInfo, IHydratable

#Region " Private Members "

        Private p_InjectName As String = String.Empty
        Private p_InjectContent As String = String.Empty
        Private p_InjectionId As Integer = Null.NullInteger
        Private p_InjectTop As Boolean = Null.NullBoolean
        Private p_IsEnabled As Boolean = Null.NullBoolean
        Private p_ModuleId As Integer = Null.NullInteger
        Private p_OrderShown As Integer = Null.NullInteger

#End Region

#Region " Public Properties "

        Public Property InjectName() As String Implements IInjectionInfo.InjectName
            Get
                Return Me.p_InjectName
            End Get
            Set(ByVal value As String)
                Me.p_InjectName = value
            End Set
        End Property

        Public Property InjectContent() As String Implements IInjectionInfo.InjectContent
            Get
                Return Me.p_InjectContent
            End Get
            Set(ByVal value As String)
                Me.p_InjectContent = value
            End Set
        End Property

        Public Property InjectionId() As Integer Implements IInjectionInfo.InjectionId
            Get
                Return Me.p_InjectionId
            End Get
            Set(ByVal value As Integer)
                Me.p_InjectionId = value
            End Set
        End Property

        Public Property InjectTop() As Boolean Implements IInjectionInfo.InjectTop
            Get
                Return Me.p_InjectTop
            End Get
            Set(ByVal value As Boolean)
                Me.p_InjectTop = value
            End Set
        End Property

        Public Property IsEnabled() As Boolean Implements IInjectionInfo.IsEnabled
            Get
                Return Me.p_IsEnabled
            End Get
            Set(ByVal value As Boolean)
                Me.p_IsEnabled = value
            End Set
        End Property

        Public Property ModuleId() As Integer Implements IInjectionInfo.ModuleId
            Get
                Return Me.p_ModuleId
            End Get
            Set(ByVal value As Integer)
                Me.p_ModuleId = value
            End Set
        End Property

        Public Property OrderShown() As Integer Implements IInjectionInfo.OrderShown
            Get
                Return Me.p_OrderShown
            End Get
            Set(ByVal value As Integer)
                Me.p_OrderShown = value
            End Set
        End Property

#End Region

        Public Sub New()
            ' do nothing
        End Sub

#Region " IHydratable Implementation "

        Public Property KeyID() As Integer Implements DotNetNuke.Entities.Modules.IHydratable.KeyID
            Get
                Return Me.p_InjectionId
            End Get
            Set(ByVal value As Integer)
                Me.p_InjectionId = value
            End Set
        End Property

        Public Sub Fill(ByVal dr As System.Data.IDataReader) Implements DotNetNuke.Entities.Modules.IHydratable.Fill
            Try
                While dr.Read
                    If Not dr.Item(InjectionIdField) Is Nothing Then
                        Me.p_InjectionId = Integer.Parse(dr.Item(InjectionIdField).ToString, Globalization.NumberStyles.Integer)
                    End If
                    If Not dr.Item(InjectNameField) Is Nothing Then
                        Me.p_InjectName = dr.Item(InjectNameField).ToString
                    End If
                    If Not dr.Item(InjectContentField) Is Nothing Then
                        Me.p_InjectContent = dr.Item(InjectContentField).ToString
                    End If
                    If Not dr.Item(InjectTopField) Is Nothing Then
                        Me.p_InjectTop = Boolean.Parse(dr.Item(InjectTopField).ToString)
                    End If
                    If Not dr.Item(IsEnabledField) Is Nothing Then
                        Me.p_IsEnabled = Boolean.Parse(dr.Item(IsEnabledField).ToString)
                    End If
                    If Not dr.Item(ModuleIdField) Is Nothing Then
                        Me.p_ModuleId = Integer.Parse(dr.Item(ModuleIdField).ToString, Globalization.NumberStyles.Integer)
                    End If
                    If Not dr.Item(OrderShownField) Is Nothing Then
                        Me.p_OrderShown = Integer.Parse(dr.Item(OrderShownField).ToString, Globalization.NumberStyles.Integer)
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

#End Region

    End Class

End Namespace