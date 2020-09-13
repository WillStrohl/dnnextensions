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

Imports DotNetNuke

Namespace WillStrohl.Modules.Lightbox

    Public MustInherit Class DataProvider

#Region " Private Members "

        Private Const c_AssemblyName As String = "WillStrohl.Modules.Lightbox.SqlDataProvider, WillStrohl.Modules.Lightbox"

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
            'objProvider = CType(Framework.Reflection.CreateObject(c_Object, c_ObjectType, c_AssemblyName), DataProvider)

            If objProvider Is Nothing Then
                Dim objectType As Type = Type.GetType(c_AssemblyName)

                objProvider = DirectCast(Activator.CreateInstance(objectType), DataProvider)
                DataCache.SetCache(objectType.FullName, objProvider)
            End If
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