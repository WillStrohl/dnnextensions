'
' Copyright Upendo Ventures, LLC
' https://upendoventures.com
' 
' Permission is hereby granted, free of charge, to any person obtaining a copy of this software
' and associated documentation files (the "Software"), to deal in the Software without restriction,
' including without limitation the rights to use, copy, modify, merge, publish, distribute,
' sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is
' furnished to do so, subject to the following conditions:
' 
' The above copyright notice and this permission notice shall be included in all copies or
' substantial portions of the Software.
' 
' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
' NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
' OUT OF Or IN CONNECTION WITH THE SOFTWARE Or THE USE Or OTHER DEALINGS IN THE SOFTWARE.
'

Imports DotNetNuke.Entities.Modules

Imports WillStrohl.Modules.Lightbox.ImageInfoMembers

Namespace WillStrohl.Modules.Lightbox

    <Serializable> _
    Public NotInheritable Class ImageInfo
        Implements IImageInfo, IHydratable
        
#Region " Private Members "

        Private p_ImageId As Integer = Null.NullInteger
        Private p_LightboxId As Integer = Null.NullInteger
        Private p_FileName As String = Null.NullString
        Private p_Title As String = Null.NullString
        Private p_Description As String = Null.NullString
        Private p_DisplayOrder As Integer = Null.NullInteger
        Private p_LastUpdatedBy As Integer = Null.NullInteger
        Private p_LastUpdatedDate As DateTime = Null.NullDate

#End Region

#Region " Public Properties "

        Public Property Description As String Implements IImageInfo.Description
            Get
                Return Me.p_Description
            End Get
            Set(ByVal value As String)
                Me.p_Description = value
            End Set
        End Property

        Public Property DisplayOrder As Integer Implements IImageInfo.DisplayOrder
            Get
                Return Me.p_DisplayOrder
            End Get
            Set(ByVal value As Integer)
                Me.p_DisplayOrder = value
            End Set
        End Property

        Public Property FileName As String Implements IImageInfo.FileName
            Get
                Return Me.p_FileName
            End Get
            Set(ByVal value As String)
                Me.p_FileName = value
            End Set
        End Property

        Public Property ImageId As Integer Implements IImageInfo.ImageId
            Get
                Return Me.p_ImageId
            End Get
            Set(ByVal value As Integer)
                Me.p_ImageId = value
            End Set
        End Property

        Public Property LightboxId As Integer Implements IImageInfo.LightboxId
            Get
                Return Me.p_LightboxId
            End Get
            Set(ByVal value As Integer)
                Me.p_LightboxId = value
            End Set
        End Property

        Public Property Title As String Implements IImageInfo.Title
            Get
                Return Me.p_Title
            End Get
            Set(ByVal value As String)
                Me.p_Title = value
            End Set
        End Property

        Public Property LastUpdatedBy As Integer Implements IImageInfo.LastUpdatedBy
            Get
                Return p_LastUpdatedBy
            End Get
            Set(value As Integer)
                p_LastUpdatedBy = value
            End Set
        End Property

        Public Property LastUpdatedDate() As DateTime Implements IImageInfo.LastUpdatedDate
            Get
                Return p_LastUpdatedDate
            End Get
            Set(value As DateTime)
                p_LastUpdatedDate = value
            End Set
        End Property

#End Region

#Region " Constructors "

        Public Sub New()
            ' do nothing
        End Sub

        Public Sub New(ByVal ImageId As Integer, ByVal LightboxId As Integer, ByVal FileName As String, ByVal Title As String, ByVal Description As String, ByVal DisplayOrder As Integer, ByVal LastUpdatedBy As Integer, ByVal LastUpdatedDate As DateTime)
            Me.p_ImageId = ImageId
            Me.p_LightboxId = LightboxId
            Me.p_FileName = FileName
            Me.p_Title = Title
            Me.p_Description = Description
            Me.p_DisplayOrder = DisplayOrder
            p_LastUpdatedBy = LastUpdatedBy
            p_LastUpdatedDate = LastUpdatedDate
        End Sub

        Public Sub New(ByVal LightboxId As Integer, ByVal FileName As String, ByVal Title As String, ByVal Description As String, ByVal DisplayOrder As Integer, ByVal LastUpdatedBy As Integer, ByVal LastUpdatedDate As DateTime)
            Me.p_LightboxId = LightboxId
            Me.p_FileName = FileName
            Me.p_Title = Title
            Me.p_Description = Description
            Me.p_DisplayOrder = DisplayOrder
            p_LastUpdatedBy = LastUpdatedBy
            p_LastUpdatedDate = LastUpdatedDate
        End Sub

#End Region

#Region " IHydratable Implementation "

        Public Property KeyID() As Integer Implements DotNetNuke.Entities.Modules.IHydratable.KeyID
            Get
                Return Me.p_ImageId
            End Get
            Set(ByVal value As Integer)
                Me.p_ImageId = value
            End Set
        End Property

        Public Sub Fill(ByVal dr As System.Data.IDataReader) Implements DotNetNuke.Entities.Modules.IHydratable.Fill
            Try
                While dr.Read
                    If Not dr.Item(ImageIdField) Is Nothing Then
                        Me.p_ImageId = Integer.Parse(dr.Item(ImageIdField).ToString, Globalization.NumberStyles.Integer)
                    End If
                    If Not dr.Item(LightboxIdField) Is Nothing Then
                        Me.p_LightboxId = Integer.Parse(dr.Item(LightboxIdField).ToString, Globalization.NumberStyles.Integer)
                    End If
                    If Not dr.Item(FileNameField) Is Nothing Then
                        Me.p_FileName = dr.Item(FileNameField).ToString
                    End If
                    If Not dr.Item(TitleField) Is Nothing Then
                        Me.p_Title = dr.Item(TitleField).ToString
                    End If
                    If Not dr.Item(DescriptionField) Is Nothing Then
                        Me.p_Description = dr.Item(DescriptionField).ToString
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