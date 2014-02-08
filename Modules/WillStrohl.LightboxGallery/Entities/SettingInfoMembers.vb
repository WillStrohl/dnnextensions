'
' Lightbox Gallery Module for DotNetNuke
' Project Contributors - Will Strohl (http://www.WillStrohl.com), Armand Datema (http://www.schwingsoft.com)
'
'Copyright (c) 2009-2012, Will Strohl
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

Namespace WillStrohl.Modules.Lightbox

    Public NotInheritable Class SettingInfoMembers

        Public Const SettingIdField As String = "SettingId"
        Public Const LightboxIdField As String = "LightboxId"
        Public Const PaddingField As String = "Padding"
        Public Const MarginField As String = "Margin"
        Public Const OpacityField As String = "Opacity"
        Public Const ModalField As String = "Modal"
        Public Const CyclicField As String = "Cyclic"
        Public Const OverlayShowField As String = "OverlayShow"
        Public Const OverlayOpacityField As String = "OverlayOpacity"
        Public Const OverlayColorField As String = "OverlayColor"
        Public Const TitleShowField As String = "TitleShow"
        Public Const TitlePositionField As String = "TitlePosition"
        Public Const TransitionField As String = "Transition"
        Public Const SpeedField As String = "Speed"
        Public Const ChangeSpeedField As String = "ChangeSpeed"
        Public Const ShowCloseButtonField As String = "ShowCloseButton"
        Public Const ShowNavArrowsField As String = "ShowNavArrows"
        Public Const EnableEscapeButtonField As String = "EnableEscapeButton"
        Public Const OnStartField As String = "OnStart"
        Public Const OnCancelField As String = "OnCancel"
        Public Const OnCompleteField As String = "OnComplete"
        Public Const OnCleanupField As String = "OnCleanup"
        Public Const OnClosedField As String = "OnClosed"

    End Class

End Namespace