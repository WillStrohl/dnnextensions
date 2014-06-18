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

Imports DotNetNuke

Namespace WillStrohl.Modules.Lightbox

    Public MustInherit Class DataProvider

#Region " Private Members "

        Private Const c_Object As String = "data"
        Private Const c_ObjectType As String = "WillStrohl.Modules.Lightbox"

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
            objProvider = CType(Framework.Reflection.CreateObject(c_Object, c_ObjectType, c_ObjectType), DataProvider)
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

        Public MustOverride Function AddLightbox(ByVal ModuleId As Integer, ByVal GalleryName As String, ByVal GalleryDescription As String, ByVal GalleryFolder As String, ByVal DisplayOrder As Integer, ByVal HideTitleDescription As Boolean, ByVal LastUpdatedBy As Integer) As Integer
        Public MustOverride Sub UpdateLightbox(ByVal LightboxId As Integer, ByVal ModuleId As Integer, ByVal GalleryName As String, ByVal GalleryDescription As String, ByVal GalleryFolder As String, ByVal DisplayOrder As Integer, ByVal HideTitleDescription As Boolean, ByVal LastUpdatedBy As Integer)
        Public MustOverride Sub DeleteLightbox(ByVal LightboxId As Integer)
        Public MustOverride Function GetLightboxes(ByVal ModuleId As Integer) As IDataReader
        Public MustOverride Function GetLightbox(ByVal LightboxId As Integer) As IDataReader
        Public MustOverride Function GetLightboxIds(ByVal ModuleId As Integer) As IDataReader
        Public MustOverride Function GetLightboxCount(ByVal ModuleId As Integer) As Integer
        Public MustOverride Sub UpdateDisplayOrder()
        Public MustOverride Function DoesDisplayOrderNeedUpdate() As Boolean

        Public MustOverride Function AddSetting(ByVal LightboxId As Integer, ByVal Padding As Integer, ByVal Margin As Integer, ByVal Opacity As Boolean, ByVal Modal As Boolean, ByVal Cyclic As Boolean, ByVal OverlayShow As Boolean, ByVal OverlayOpacity As Decimal, ByVal OverlayColor As String, ByVal TitleShow As Boolean, ByVal TitlePosition As String, ByVal Transition As String, ByVal Speed As Integer, ByVal ChangeSpeed As Integer, ByVal ShowCloseButton As Boolean, ByVal ShowNavArrows As Boolean, ByVal EnableEscapeButton As Boolean, ByVal OnStart As String, ByVal OnCancel As String, ByVal OnComplete As String, ByVal OnCleanup As String, ByVal OnClosed As String) As Integer
        Public MustOverride Sub UpdateSetting(ByVal SettingId As Integer, ByVal LightboxId As Integer, ByVal Padding As Integer, ByVal Margin As Integer, ByVal Opacity As Boolean, ByVal Modal As Boolean, ByVal Cyclic As Boolean, ByVal OverlayShow As Boolean, ByVal OverlayOpacity As Decimal, ByVal OverlayColor As String, ByVal TitleShow As Boolean, ByVal TitlePosition As String, ByVal Transition As String, ByVal Speed As Integer, ByVal ChangeSpeed As Integer, ByVal ShowCloseButton As Boolean, ByVal ShowNavArrows As Boolean, ByVal EnableEscapeButton As Boolean, ByVal OnStart As String, ByVal OnCancel As String, ByVal OnComplete As String, ByVal OnCleanup As String, ByVal OnClosed As String)
        Public MustOverride Sub DeleteSetting(ByVal SettingId As Integer)
        Public MustOverride Function GetSettings(ByVal LightboxId As Integer) As IDataReader

        Public MustOverride Function GetImages(ByVal LightboxId As Integer) As IDataReader
        Public MustOverride Function GetImageById(ByVal ImageId As Integer) As IDataReader
        Public MustOverride Function GetImageByFileName(ByVal LightboxId As Integer, ByVal FileName As String) As IDataReader
        Public MustOverride Function AddImage(ByVal LightboxId As Integer, ByVal FileName As String, ByVal Title As String, ByVal Description As String, ByVal DisplayOrder As Integer, ByVal LastUpdatedBy As Integer) As Integer
        Public MustOverride Sub UpdateImage(ByVal ImageId As Integer, ByVal LightboxId As Integer, ByVal FileName As String, ByVal Title As String, ByVal Description As String, ByVal DisplayOrder As Integer, ByVal LastUpdatedBy As Integer)
        Public MustOverride Sub DeleteImageById(ByVal ImageId As Integer)
        Public MustOverride Sub DeleteImageByFileName(ByVal LightboxId As Integer, ByVal FileName As String)
        Public MustOverride Function GetImageCount(ByVal LightboxId As Integer) As Integer

#End Region

    End Class

End Namespace