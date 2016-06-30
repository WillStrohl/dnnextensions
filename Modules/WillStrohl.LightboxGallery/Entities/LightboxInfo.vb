'
' Lightbox Gallery Module for DotNetNuke
' Project Contributors - Will Strohl (http://www.WillStrohl.com), Armand Datema (http://www.schwingsoft.com)
'
'Copyright (c) 2009-2016, Will Strohl
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
'Neither the name of Will Strohl, Armand Datema, Lightbox Gallery, nor the names of its contributors may be 
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

Imports WillStrohl.Modules.Lightbox.LightboxInfoMembers

Namespace WillStrohl.Modules.Lightbox

    <Serializable()> _
    Public NotInheritable Class LightboxInfo
        Implements ILightboxInfo, IHydratable

#Region " Private Members "

        Private p_GalleryDescription As String = Null.NullString
        Private p_GalleryFolder As String = Null.NullString
        Private p_GalleryName As String = Null.NullString
        Private p_LightboxId As Integer = Null.NullInteger
        Private p_ModuleId As Integer = Null.NullInteger
        Private p_DisplayOrder As Integer = Null.NullInteger
        Private p_HideTitleDescription As Boolean = False
        Private p_LastUpdatedBy As Integer = Null.NullInteger
        Private p_LastUpdatedDate As DateTime = Null.NullDate

#End Region

#Region " Public Properties "

        Public Property GalleryDescription() As String Implements ILightboxInfo.GalleryDescription
            Get
                Return Me.p_GalleryDescription
            End Get
            Set(ByVal value As String)
                Me.p_GalleryDescription = value
            End Set
        End Property

        Public Property GalleryFolder() As String Implements ILightboxInfo.GalleryFolder
            Get
                Return Me.p_GalleryFolder
            End Get
            Set(ByVal value As String)
                Me.p_GalleryFolder = value
            End Set
        End Property

        Public Property GalleryName() As String Implements ILightboxInfo.GalleryName
            Get
                Return Me.p_GalleryName
            End Get
            Set(ByVal value As String)
                Me.p_GalleryName = value
            End Set
        End Property

        Public Property LightboxId() As Integer Implements ILightboxInfo.LightboxId
            Get
                Return Me.p_LightboxId
            End Get
            Set(ByVal value As Integer)
                Me.p_LightboxId = value
            End Set
        End Property

        Public Property ModuleId() As Integer Implements ILightboxInfo.ModuleId
            Get
                Return Me.p_ModuleId
            End Get
            Set(ByVal value As Integer)
                Me.p_ModuleId = value
            End Set
        End Property

        Public Property DisplayOrder() As Integer Implements ILightboxInfo.DisplayOrder
            Get
                Return Me.p_DisplayOrder
            End Get
            Set(ByVal value As Integer)
                Me.p_DisplayOrder = value
            End Set
        End Property

        ''' <summary>
        ''' HideTitleDescription - a flag to determine if the title and description should be shown, or hidden
        ''' </summary>
        ''' <value>Boolean - if true, the title and description will not be visible</value>
        ''' <returns>Boolean - defaults to False for backwards compatibility</returns>
        ''' <remarks></remarks>
        ''' <history>
        ''' 20101013 - wstrohl - Added.
        ''' </history>
        Public Property HideTitleDescription As Boolean Implements ILightboxInfo.HideTitleDescription
            Get
                Return Me.p_HideTitleDescription
            End Get
            Set(ByVal value As Boolean)
                Me.p_HideTitleDescription = value
            End Set
        End Property

        Public Property LastUpdatedBy() As Integer Implements ILightboxInfo.LastUpdatedBy
            Get
                Return p_LastUpdatedBy
            End Get
            Set(ByVal value As Integer)
                p_LastUpdatedBy = value
            End Set
        End Property

        Public Property LastUpdatedDate() As DateTime Implements ILightboxInfo.LastUpdatedDate
            Get
                Return p_LastUpdatedDate
            End Get
            Set(ByVal value As DateTime)
                p_LastUpdatedDate = value
            End Set
        End Property

#End Region

        Public Sub New()
            ' do nothing
        End Sub

#Region " IHydratable Implementation "

        Public Property KeyID() As Integer Implements DotNetNuke.Entities.Modules.IHydratable.KeyID
            Get
                Return Me.p_LightboxId
            End Get
            Set(ByVal value As Integer)
                Me.p_LightboxId = value
            End Set
        End Property

        Public Sub Fill(ByVal dr As System.Data.IDataReader) Implements DotNetNuke.Entities.Modules.IHydratable.Fill
            Try
                While dr.Read
                    If Not dr.Item(LightboxIdField) Is Nothing Then
                        Me.p_LightboxId = Integer.Parse(dr.Item(LightboxIdField).ToString, Globalization.NumberStyles.Integer)
                    End If
                    If Not dr.Item(ModuleIdField) Is Nothing Then
                        Me.p_ModuleId = Integer.Parse(dr.Item(ModuleIdField).ToString, Globalization.NumberStyles.Integer)
                    End If
                    If Not dr.Item(GalleryNameField) Is Nothing Then
                        Me.p_GalleryName = dr.Item(GalleryNameField).ToString
                    End If
                    If Not dr.Item(GalleryDescriptionField) Is Nothing Then
                        Me.p_GalleryDescription = dr.Item(GalleryDescriptionField).ToString
                    End If
                    If Not dr.Item(GalleryFolderField) Is Nothing Then
                        Me.p_GalleryFolder = dr.Item(GalleryFolderField).ToString
                    End If
                    If Not dr.Item(DisplayOrderField) Is Nothing Then
                        Me.p_DisplayOrder = Integer.Parse(dr.Item(DisplayOrderField).ToString, Globalization.NumberStyles.Integer)
                    End If
                    If Not dr.Item(LastUpdatedByField) Is Nothing Then
                        Try
                            p_LastUpdatedBy = Integer.Parse(dr.Item(LastUpdatedByField).ToString, Globalization.NumberStyles.Integer)
                        Catch
                            ' do nothing
                        End Try
                    End If
                    If Not dr.Item(LastUpdatedDateField) Is Nothing Then
                        Try
                            p_LastUpdatedDate = DateTime.Parse(dr.Item(LastUpdatedDateField).ToString)
                        Catch
                            p_LastUpdatedDate = DateTime.Now
                        End Try
                    End If
                    Try
                        If Not dr.Item(HideTitleDescriptionField) Is Nothing Then
                            Me.p_HideTitleDescription = Boolean.Parse(dr.Item(HideTitleDescriptionField).ToString)
                        End If
                    Catch
                        ' do nothing
                    End Try
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