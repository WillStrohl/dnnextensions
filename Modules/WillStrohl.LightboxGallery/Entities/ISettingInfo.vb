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

Namespace WillStrohl.Modules.Lightbox

    Public Interface ISettingInfo
        Property SettingId() As Integer
        Property LightboxId() As Integer
        Property Padding() As Integer
        Property Margin() As Integer
        Property Opacity() As Boolean
        Property Modal() As Boolean
        Property Cyclic() As Boolean
        Property OverlayShow() As Boolean
        Property OverlayOpacity() As Decimal
        Property OverlayColor() As String
        Property TitleShow() As Boolean
        Property TitlePosition() As String
        Property Transition() As String
        Property Speed() As Integer
        Property ChangeSpeed() As Integer
        Property ShowCloseButton() As Boolean
        Property ShowNavArrows() As Boolean
        Property EnableEscapeButton() As Boolean
        Property OnStart() As String
        Property OnCancel() As String
        Property OnComplete() As String
        Property OnCleanup() As String
        Property OnClosed() As String
    End Interface

End Namespace