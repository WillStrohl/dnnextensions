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

Imports DotNetNuke.Services.Exceptions
Imports System.Collections.Generic

Imports WillStrohl.Modules.Lightbox.SettingInfoMembers

Namespace WillStrohl.Modules.Lightbox

    Public NotInheritable Class SettingInfoCollection
        Inherits List(Of SettingInfo)

#Region " Constructors "

        Public Sub New()
            MyBase.New()
        End Sub

        Public Sub New(ByVal capacity As Integer)
            MyBase.New(capacity)
        End Sub

        Public Sub New(ByVal collection As IEnumerable(Of SettingInfo))
            MyBase.New(collection)
        End Sub

#End Region

        Public Sub Fill(ByVal dr As System.Data.IDataReader)
            Try
                While dr.Read
                    Dim obj As New SettingInfo
                    If Not dr.Item(SettingIdField) Is Nothing Then
                        obj.SettingId = Integer.Parse(dr.Item(SettingIdField).ToString, Globalization.NumberStyles.Integer)
                    End If
                    If Not dr.Item(LightboxIdField) Is Nothing Then
                        obj.LightboxId = Integer.Parse(dr.Item(LightboxIdField).ToString, Globalization.NumberStyles.Integer)
                    End If
                    If Not dr.Item(PaddingField) Is Nothing Then
                        obj.Padding = Integer.Parse(dr.Item(PaddingField).ToString, Globalization.NumberStyles.Integer)
                    End If
                    If Not dr.Item(MarginField) Is Nothing Then
                        obj.Margin = Integer.Parse(dr.Item(MarginField).ToString, Globalization.NumberStyles.Integer)
                    End If
                    If Not dr.Item(OpacityField) Is Nothing Then
                        obj.Opacity = Boolean.Parse(dr.Item(OpacityField).ToString)
                    End If
                    If Not dr.Item(ModalField) Is Nothing Then
                        obj.Modal = Boolean.Parse(dr.Item(ModalField).ToString)
                    End If
                    If Not dr.Item(CyclicField) Is Nothing Then
                        obj.Cyclic = Boolean.Parse(dr.Item(CyclicField).ToString)
                    End If
                    If Not dr.Item(OverlayShowField) Is Nothing Then
                        obj.OverlayShow = Boolean.Parse(dr.Item(OverlayShowField).ToString)
                    End If
                    If Not dr.Item(OverlayOpacityField) Is Nothing Then
                        obj.OverlayOpacity = Decimal.Parse(dr.Item(OverlayOpacityField).ToString, Globalization.NumberStyles.Float)
                    End If
                    If Not dr.Item(OverlayColorField) Is Nothing Then
                        obj.OverlayColor = dr.Item(OverlayColorField).ToString
                    End If
                    If Not dr.Item(TitleShowField) Is Nothing Then
                        obj.TitleShow = Boolean.Parse(dr.Item(TitleShowField).ToString)
                    End If
                    If Not dr.Item(TitlePositionField) Is Nothing Then
                        obj.TitlePosition = dr.Item(TitlePositionField).ToString
                    End If
                    If Not dr.Item(TransitionField) Is Nothing Then
                        obj.Transition = dr.Item(TransitionField).ToString
                    End If
                    If Not dr.Item(SpeedField) Is Nothing Then
                        obj.Speed = Integer.Parse(dr.Item(SpeedField).ToString, Globalization.NumberStyles.Integer)
                    End If
                    If Not dr.Item(ChangeSpeedField) Is Nothing Then
                        obj.ChangeSpeed = Integer.Parse(dr.Item(ChangeSpeedField).ToString, Globalization.NumberStyles.Integer)
                    End If
                    If Not dr.Item(ShowCloseButtonField) Is Nothing Then
                        obj.ShowCloseButton = Boolean.Parse(dr.Item(ShowCloseButtonField).ToString)
                    End If
                    If Not dr.Item(ShowNavArrowsField) Is Nothing Then
                        obj.ShowNavArrows = Boolean.Parse(dr.Item(ShowNavArrowsField).ToString)
                    End If
                    If Not dr.Item(EnableEscapeButtonField) Is Nothing Then
                        obj.EnableEscapeButton = Boolean.Parse(dr.Item(EnableEscapeButtonField).ToString)
                    End If
                    If Not dr.Item(OnStartField) Is Nothing Then
                        obj.OnStart = dr.Item(OnStartField).ToString
                    End If
                    If Not dr.Item(OnCancelField) Is Nothing Then
                        obj.OnCancel = dr.Item(OnCancelField).ToString
                    End If
                    If Not dr.Item(OnCompleteField) Is Nothing Then
                        obj.OnComplete = dr.Item(OnCompleteField).ToString
                    End If
                    If Not dr.Item(OnCleanupField) Is Nothing Then
                        obj.OnCleanup = dr.Item(OnCleanupField).ToString
                    End If
                    If Not dr.Item(OnClosedField) Is Nothing Then
                        obj.OnClosed = dr.Item(OnClosedField).ToString
                    End If
                    Me.Add(obj)
                End While
            Catch ex As Exception
                LogException(ex)
            Finally
                If Not dr.IsClosed Then
                    dr.Close()
                End If
            End Try
        End Sub

    End Class

End Namespace