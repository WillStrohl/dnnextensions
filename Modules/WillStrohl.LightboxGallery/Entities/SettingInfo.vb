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

Imports WillStrohl.Modules.Lightbox.SettingInfoMembers

Namespace WillStrohl.Modules.Lightbox

    <Serializable> _
    Public NotInheritable Class SettingInfo
        Implements ISettingInfo, IHydratable

#Region " Private Members "

        Private p_SettingId As Integer = Null.NullInteger
        Private p_LightboxId As Integer = Null.NullInteger
        Private p_ChangeSpeed As Integer = 300
        Private p_Cyclic As Boolean = False
        Private p_EnableEscapeButton As Boolean = True
        Private p_Margin As Integer = 20
        Private p_Modal As Boolean = False
        Private p_OnCancel As String = String.Empty
        Private p_OnCleanup As String = String.Empty
        Private p_OnClosed As String = String.Empty
        Private p_OnComplete As String = String.Empty
        Private p_OnStart As String = String.Empty
        Private p_Opacity As Boolean = False
        Private p_OverlayColor As String = "#666"
        Private p_OverlayOpacity As Decimal = CType(0.3, Decimal)
        Private p_OverlayShow As Boolean = True
        Private p_Padding As Integer = 10
        Private p_ShowCloseButton As Boolean = True
        Private p_ShowNavArrows As Boolean = True
        Private p_Speed As Integer = 300
        Private p_TitlePosition As String = "over"
        Private p_TitleShow As Boolean = True
        Private p_Transition As String = "fade"

#End Region

#Region " Public Properties "

        Public Property SettingId() As Integer Implements ISettingInfo.SettingId
            Get
                Return Me.p_SettingId
            End Get
            Set(ByVal value As Integer)
                Me.p_SettingId = value
            End Set
        End Property

        Public Property LightboxId() As Integer Implements ISettingInfo.LightboxId
            Get
                Return Me.p_LightboxId
            End Get
            Set(ByVal value As Integer)
                Me.p_LightboxId = value
            End Set
        End Property

        Public Property ChangeSpeed() As Integer Implements ISettingInfo.ChangeSpeed
            Get
                Return Me.p_ChangeSpeed
            End Get
            Set(ByVal value As Integer)
                Me.p_ChangeSpeed = value
            End Set
        End Property

        Public Property Cyclic() As Boolean Implements ISettingInfo.Cyclic
            Get
                Return Me.p_Cyclic
            End Get
            Set(ByVal value As Boolean)
                Me.p_Cyclic = value
            End Set
        End Property

        Public Property EnableEscapeButton() As Boolean Implements ISettingInfo.EnableEscapeButton
            Get
                Return Me.p_EnableEscapeButton
            End Get
            Set(ByVal value As Boolean)
                Me.p_EnableEscapeButton = value
            End Set
        End Property

        Public Property Margin() As Integer Implements ISettingInfo.Margin
            Get
                Return Me.p_Margin
            End Get
            Set(ByVal value As Integer)
                Me.p_Margin = value
            End Set
        End Property

        Public Property Modal() As Boolean Implements ISettingInfo.Modal
            Get
                Return Me.p_Modal
            End Get
            Set(ByVal value As Boolean)
                Me.p_Modal = value
            End Set
        End Property

        Public Property OnCancel() As String Implements ISettingInfo.OnCancel
            Get
                Return Me.p_OnCancel
            End Get
            Set(ByVal value As String)
                Me.p_OnCancel = value
            End Set
        End Property

        Public Property OnCleanup() As String Implements ISettingInfo.OnCleanup
            Get
                Return Me.p_OnCleanup
            End Get
            Set(ByVal value As String)
                Me.p_OnCleanup = value
            End Set
        End Property

        Public Property OnClosed() As String Implements ISettingInfo.OnClosed
            Get
                Return Me.p_OnClosed
            End Get
            Set(ByVal value As String)
                Me.p_OnClosed = value
            End Set
        End Property

        Public Property OnComplete() As String Implements ISettingInfo.OnComplete
            Get
                Return Me.p_OnComplete
            End Get
            Set(ByVal value As String)
                Me.p_OnComplete = value
            End Set
        End Property

        Public Property OnStart() As String Implements ISettingInfo.OnStart
            Get
                Return Me.p_OnStart
            End Get
            Set(ByVal value As String)
                Me.p_OnStart = value
            End Set
        End Property

        Public Property Opacity() As Boolean Implements ISettingInfo.Opacity
            Get
                Return Me.p_Opacity
            End Get
            Set(ByVal value As Boolean)
                Me.p_Opacity = value
            End Set
        End Property

        Public Property OverlayColor() As String Implements ISettingInfo.OverlayColor
            Get
                Return Me.p_OverlayColor
            End Get
            Set(ByVal value As String)
                Me.p_OverlayColor = value
            End Set
        End Property

        Public Property OverlayOpacity() As Decimal Implements ISettingInfo.OverlayOpacity
            Get
                Return Me.p_OverlayOpacity
            End Get
            Set(ByVal value As Decimal)
                Me.p_OverlayOpacity = value
            End Set
        End Property

        Public Property OverlayShow() As Boolean Implements ISettingInfo.OverlayShow
            Get
                Return Me.p_OverlayShow
            End Get
            Set(ByVal value As Boolean)
                Me.p_OverlayShow = value
            End Set
        End Property

        Public Property Padding() As Integer Implements ISettingInfo.Padding
            Get
                Return Me.p_Padding
            End Get
            Set(ByVal value As Integer)
                Me.p_Padding = value
            End Set
        End Property

        Public Property ShowCloseButton() As Boolean Implements ISettingInfo.ShowCloseButton
            Get
                Return Me.p_ShowCloseButton
            End Get
            Set(ByVal value As Boolean)
                Me.p_ShowCloseButton = value
            End Set
        End Property

        Public Property ShowNavArrows() As Boolean Implements ISettingInfo.ShowNavArrows
            Get
                Return Me.p_ShowNavArrows
            End Get
            Set(ByVal value As Boolean)
                Me.p_ShowNavArrows = value
            End Set
        End Property

        Public Property Speed() As Integer Implements ISettingInfo.Speed
            Get
                Return Me.p_Speed
            End Get
            Set(ByVal value As Integer)
                Me.p_Speed = value
            End Set
        End Property

        Public Property TitlePosition() As String Implements ISettingInfo.TitlePosition
            Get
                Return Me.p_TitlePosition
            End Get
            Set(ByVal value As String)
                Me.p_TitlePosition = value
            End Set
        End Property

        Public Property TitleShow() As Boolean Implements ISettingInfo.TitleShow
            Get
                Return Me.p_TitleShow
            End Get
            Set(ByVal value As Boolean)
                Me.p_TitleShow = value
            End Set
        End Property

        Public Property Transition() As String Implements ISettingInfo.Transition
            Get
                Return Me.p_Transition
            End Get
            Set(ByVal value As String)
                Me.p_Transition = value
            End Set
        End Property

#End Region

        Public Sub New()
            ' do nothing
        End Sub

#Region " IHydratable Implementation "

        Public Property KeyID() As Integer Implements DotNetNuke.Entities.Modules.IHydratable.KeyID
            Get
                Return Me.p_SettingId
            End Get
            Set(ByVal value As Integer)
                Me.p_SettingId = value
            End Set
        End Property

        Public Sub Fill(ByVal dr As System.Data.IDataReader) Implements DotNetNuke.Entities.Modules.IHydratable.Fill
            Try
                While dr.Read
                    If Not dr.Item(SettingIdField) Is Nothing Then
                        Me.p_SettingId = Integer.Parse(dr.Item(SettingIdField).ToString, Globalization.NumberStyles.Integer)
                    End If
                    If Not dr.Item(LightboxIdField) Is Nothing Then
                        Me.p_LightboxId = Integer.Parse(dr.Item(LightboxIdField).ToString, Globalization.NumberStyles.Integer)
                    End If
                    If Not dr.Item(PaddingField) Is Nothing Then
                        Me.p_Padding = Integer.Parse(dr.Item(PaddingField).ToString, Globalization.NumberStyles.Integer)
                    End If
                    If Not dr.Item(MarginField) Is Nothing Then
                        Me.p_Margin = Integer.Parse(dr.Item(MarginField).ToString, Globalization.NumberStyles.Integer)
                    End If
                    If Not dr.Item(OpacityField) Is Nothing Then
                        Me.p_Opacity = Boolean.Parse(dr.Item(OpacityField).ToString)
                    End If
                    If Not dr.Item(ModalField) Is Nothing Then
                        Me.p_Modal = Boolean.Parse(dr.Item(ModalField).ToString)
                    End If
                    If Not dr.Item(CyclicField) Is Nothing Then
                        Me.p_Cyclic = Boolean.Parse(dr.Item(CyclicField).ToString)
                    End If
                    If Not dr.Item(OverlayShowField) Is Nothing Then
                        Me.p_OverlayShow = Boolean.Parse(dr.Item(OverlayShowField).ToString)
                    End If
                    If Not dr.Item(OverlayOpacityField) Is Nothing Then
                        Me.p_OverlayOpacity = Decimal.Parse(dr.Item(OverlayOpacityField).ToString, Globalization.NumberStyles.Float)
                    End If
                    If Not dr.Item(OverlayColorField) Is Nothing Then
                        Me.p_OverlayColor = dr.Item(OverlayColorField).ToString
                    End If
                    If Not dr.Item(TitleShowField) Is Nothing Then
                        Me.p_TitleShow = Boolean.Parse(dr.Item(TitleShowField).ToString)
                    End If
                    If Not dr.Item(TitlePositionField) Is Nothing Then
                        Me.p_TitlePosition = dr.Item(TitlePositionField).ToString
                    End If
                    If Not dr.Item(TransitionField) Is Nothing Then
                        Me.p_Transition = dr.Item(TransitionField).ToString
                    End If
                    If Not dr.Item(SpeedField) Is Nothing Then
                        Me.p_Speed = Integer.Parse(dr.Item(SpeedField).ToString, Globalization.NumberStyles.Integer)
                    End If
                    If Not dr.Item(ChangeSpeedField) Is Nothing Then
                        Me.p_ChangeSpeed = Integer.Parse(dr.Item(ChangeSpeedField).ToString, Globalization.NumberStyles.Integer)
                    End If
                    If Not dr.Item(ShowCloseButtonField) Is Nothing Then
                        Me.p_ShowCloseButton = Boolean.Parse(dr.Item(ShowCloseButtonField).ToString)
                    End If
                    If Not dr.Item(ShowNavArrowsField) Is Nothing Then
                        Me.p_ShowNavArrows = Boolean.Parse(dr.Item(ShowNavArrowsField).ToString)
                    End If
                    If Not dr.Item(EnableEscapeButtonField) Is Nothing Then
                        Me.p_EnableEscapeButton = Boolean.Parse(dr.Item(EnableEscapeButtonField).ToString)
                    End If
                    If Not dr.Item(OnStartField) Is Nothing Then
                        Me.p_OnStart = dr.Item(OnStartField).ToString
                    End If
                    If Not dr.Item(OnCancelField) Is Nothing Then
                        Me.p_OnCancel = dr.Item(OnCancelField).ToString
                    End If
                    If Not dr.Item(OnCompleteField) Is Nothing Then
                        Me.p_OnComplete = dr.Item(OnCompleteField).ToString
                    End If
                    If Not dr.Item(OnCleanupField) Is Nothing Then
                        Me.p_OnCleanup = dr.Item(OnCleanupField).ToString
                    End If
                    If Not dr.Item(OnClosedField) Is Nothing Then
                        Me.p_OnClosed = dr.Item(OnClosedField).ToString
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