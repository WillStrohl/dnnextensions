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

Imports DotNetNuke.Entities.Modules
Imports DotNetNuke.UI.Skins.Skin
Imports DotNetNuke.UI.Skins.Controls.ModuleMessage
Imports DotNetNuke.Services.Exceptions
Imports DotNetNuke.Services.FileSystem
Imports System.Collections.Generic
Imports System.Drawing
Imports System.IO
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Web.UI
Imports WillStrohl.Modules.Lightbox.LightboxController

Namespace WillStrohl.Modules.Lightbox

    Partial Public MustInherit Class ViewLightbox
        Inherits WNSPortalModuleBase
        Implements IActionable

#Region " Private Members "

        Private Const EDIT_KEY As String = "Edit"
        Private Const MENU_ITEM_TITLE_KEY As String = "EditLightbox.MenuItem.Title"
        Private Const MENU_ITEM_CHANGEORDER_KEY As String = "EditLightbox.MenuItem.ChangeOrder"
        Private Const CHANGE_ORDER As String = "ChangeOrder"
        Private Const LIGHTBOX_KEY As String = "lightbox_clientscript"
        Private Const ERROR_WRAP As String = "<span class=""NormalRed"">{0}</span>"
        Private Const EDIT_IMAGE_PATH As String = "/images/edit.gif"
        Private Const ORDER_IMAGE_PATH As String = "/images/copy.gif"
        Private Const IMAGE_TEMPLATE As String = "<li class=""listitem""><span class=""wns_lightbox_span""><a class=""wns_lightbox_link"" rel=""{0}"" href=""{1}"" title=""{3}""><img src=""{2}"" alt=""{3}"" title=""{3}"" class=""wns_lightbox_image"" /></a></span></li> "
        Private Const IMAGE_EDIT_TEMPLATE As String = "<li class=""listitem editview""><span class=""wns_lightbox_span""><a class=""wns_lightbox_link"" rel=""{0}"" href=""{1}"" title=""{3}""><img src=""{2}"" alt=""{3}"" title=""{3}"" class=""wns_lightbox_image"" /></a></span><span class=""wns_lightbox_image_edit_wrap""><a href=""{4}"" class=""wns_lightbox_image_edit"">{5}</a></span></li> "
        Private Const MAX_IMAGE_HEIGHT As Integer = 100

        Private p_Albums As LightboxInfoCollection = Nothing
        Private p_AlbumIds As List(Of Integer) = Nothing
        Private p_EditImage As String = Null.NullString
        Private p_OrderImage As String = Null.NullString

#End Region

#Region " Private Properties "

        Private ReadOnly Property Albums() As LightboxInfoCollection
            Get
                If Me.p_Albums Is Nothing Then
                    Me.p_Albums = New LightboxInfoCollection

                    Dim ctlLightbox As New LightboxController

                    Dim blnCount As Boolean = ctlLightbox.DoesDisplayOrderNeedUpdate
                    If blnCount Then
                        ctlLightbox.UpdateDisplayOrder()
                    End If

                    Me.p_Albums = ctlLightbox.GetLightboxes(Me.ModuleId)
                End If

                Return Me.p_Albums
            End Get
        End Property

        Private ReadOnly Property AlbumIds() As List(Of Integer)
            Get
                If Me.p_AlbumIds Is Nothing Then
                    Dim ctlModule As New LightboxController
                    Me.p_AlbumIds = New List(Of Integer)
                    Me.p_AlbumIds = ctlModule.GetLightboxIds(Me.ModuleId)
                End If

                Return Me.p_AlbumIds
            End Get
        End Property

        Protected ReadOnly Property EditImage() As String
            Get
                If Not String.IsNullOrEmpty(p_EditImage) Then
                    Return p_EditImage
                End If

                p_EditImage = String.Concat(DotNetNuke.Common.Globals.ApplicationPath, Entities.Icons.IconController.IconURL("Edit", "16x16"))

                Return p_EditImage
            End Get
        End Property

        Protected ReadOnly Property OrderImage() As String
            Get
                If Not String.IsNullOrEmpty(Me.p_OrderImage) Then
                    Return Me.p_OrderImage
                End If

                p_OrderImage = String.Concat(DotNetNuke.Common.Globals.ApplicationPath, Entities.Icons.IconController.IconURL("Synchronize", "16x16"))

                Return p_OrderImage
            End Get
        End Property

#End Region

#Region " Event Handlers "

        Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            Try

                ' only load the scripts if we need them (when there is 1 or more albums)
                If Not Me.Albums Is Nothing AndAlso Me.Albums.Count > 0 Then

                    ' Load the Lightbox plugin client script on every page load
                    If Not Page.ClientScript.IsClientScriptBlockRegistered(LIGHTBOX_KEY) Then
                        Page.ClientScript.RegisterClientScriptBlock( _
                            Me.GetType, _
                            LIGHTBOX_KEY, _
                            String.Format(SCRIPT_TAG_FORMAT, String.Concat(Me.ControlPath, "js/fancybox/jquery.fancybox-1.3.4.pack.js")), _
                            False)
                    End If

                    Me.Page.Header.Controls.Add(New LiteralControl(String.Format(STYLESHEET_TAG_FORMAT, String.Concat(Me.ControlPath, "js/fancybox/jquery.fancybox-1.3.4.css"))))

                End If

                If Not Me.Page.IsPostBack Then
                    Me.BindData()
                End If

            Catch exc As Exception ' Module failed to load
                ProcessModuleLoadException(Me, exc, Me.IsEditable)
            End Try
        End Sub

#End Region

#Region " Helper Methods "

        Private Sub BindData()
            Me.rptGallery.Visible = False

            If Not Me.Albums Is Nothing AndAlso Me.Albums.Count > 0 Then
                Me.rptGallery.DataSource = Me.Albums
                Me.rptGallery.DataBind()
                Me.rptGallery.Visible = True

                Me.WriteOutLightboxConstructors()
            Else
                AddModuleMessage(Me, Me.GetLocalizedString("Error.NoAlbums"), ModuleMessageType.BlueInfo)
            End If

        End Sub

        Protected Function GetImageFiles(ByVal LightboxId As String, ByVal FolderName As String, ByVal GalleryName As String) As String

            Dim strReturn As String = String.Empty

            If Regex.IsMatch(FolderName, FOLDER_NAME_MATCH_PATTERN) Then
                FolderName = Regex.Replace(FolderName, FOLDER_NAME_REPLACE_PATTERN, FOLDER_NAME_REPLACEMENT_PATTERN)
            End If

            If FolderManager.Instance().FolderExists(Me.PortalId, FolderName) Then

                Dim intLightboxId As Integer = Integer.Parse(LightboxId, Globalization.NumberStyles.Integer)
                Dim arrImages As ArrayList

                ' grab a list of only images from the directory (folder)
                Dim oFolder = FolderManager.Instance.GetFolder(PortalId, FolderName)
                arrImages = VerifiedFileList(FolderManager.Instance.GetFiles(oFolder, True))

                ' create a thumbnail for each image
                CreateThumbnails(arrImages)

                ' delete any references to missing/deleted files
                SyncImagesWithDatabase(intLightboxId, arrImages)

                If arrImages.Count > 0 Then

                    Dim ctlModule As New LightboxController
                    Dim oLightbox As LightboxInfo = ctlModule.GetLightbox(intLightboxId)
                    Dim sb As New StringBuilder(50)

                    sb.AppendFormat("<ul id=""{0}"" class=""wns_lightbox"">", GetGalleryListId(GalleryName))

                    ' create the image tiles
                    For Each oImage As IFileInfo In arrImages

                        If Not String.IsNullOrEmpty(oImage.FileName) Then

                            Dim imgFileName As String = GetFileName(oImage.FileName)

                            ' attempt to get a reference to the image
                            Dim img As ImageInfo = ctlModule.GetImageByFileName(intLightboxId, imgFileName)

                            If img Is Nothing Then
                                ' create a reference to the image
                                ctlModule.AddImage(intLightboxId, imgFileName, imgFileName, String.Empty, 0, UserId)
                                ' reload the reference
                                img = ctlModule.GetImageByFileName(intLightboxId, imgFileName)
                            End If

                        End If
                    Next

                    ' get a collection of the images to display
                    Dim collImage As ImageInfoCollection = ctlModule.GetImages(intLightboxId)
                    Dim oLightboxFolder As IFolderInfo = FolderManager.Instance().GetFolder(PortalId, oLightbox.GalleryFolder)

                    For Each objImage As ImageInfo In collImage

                        Dim oImage As IFileInfo = ctlModule.GetImageFromProvider(PortalId, oLightboxFolder, objImage.FileName)
                        Dim oImageThumbnail As IFileInfo = ctlModule.GetImageFromProvider(PortalId, oLightboxFolder, GetThumbnailImageName(objImage.FileName, Me.PortalSettings, True))

                        If IsEditable AndAlso PortalSettings.UserMode = Entities.Portals.PortalSettings.Mode.Edit Then
                            sb.AppendFormat( _
                                IMAGE_EDIT_TEMPLATE, _
                                CleanNameForRel(GalleryName), _
                                GetImageFileUrl(oImage), _
                                GetImageFileUrl(oImageThumbnail), _
                                objImage.Title, _
                                GetImageEditUrl(objImage), _
                                Me.GetLocalizedString("ImageEdit.Text") _
                            )
                        Else
                            sb.AppendFormat( _
                                IMAGE_TEMPLATE, _
                                CleanNameForRel(GalleryName), _
                                GetImageFileUrl(oImage), _
                                GetImageFileUrl(oImageThumbnail), _
                                objImage.Title _
                            )
                        End If

                    Next

                    sb.Append("</ul>")

                    strReturn = sb.ToString
                Else
                    strReturn = String.Format(ERROR_WRAP, Me.GetLocalizedString("Error.NoImagesFound"))
                End If
            Else
                If Me.IsEditable Then
                    strReturn = String.Format(ERROR_WRAP, Me.GetLocalizedString("Error.PathNotFound"))
                    strReturn = String.Format(strReturn, FolderName)
                Else
                    strReturn = String.Format(ERROR_WRAP, Me.GetLocalizedString("Error.PathNotFound.Anonymous"))
                End If
            End If

            Return strReturn
        End Function

        Private Sub SyncImagesWithDatabase(ByVal LightboxId As Integer, ByVal FilesOnServer As ArrayList)

            Dim arrFilesOnServer As New ArrayList

            ' prepare a list of the valid files in the filesystem
            For Each strFile As IFileInfo In FilesOnServer
                If Not String.IsNullOrEmpty(strFile.FileName) Then
                    arrFilesOnServer.Add(GetFileName(strFile.FileName))
                End If
            Next

            Dim ctlLightbox As New LightboxController
            Dim collImagesInDb As ImageInfoCollection = ctlLightbox.GetImages(LightboxId)

            If Not collImagesInDb Is Nothing AndAlso collImagesInDb.Count > 0 Then

                ' iterate through the DB image list
                Dim arrFilesInDatabase As New ArrayList
                For Each oImageInDb As ImageInfo In collImagesInDb

                    arrFilesInDatabase.Add(oImageInDb.FileName)

                Next ' For Each oImageInDb As ImageInfo In collImagesInDb

                ' determine which files are orphaned
                Dim arrDifference As ArrayList = GetOrphanedFiles(arrFilesInDatabase, arrFilesOnServer)

                If Not arrDifference Is Nothing Then

                    For Each strFile As String In arrDifference

                        Dim oLightbox As LightboxInfo = ctlLightbox.GetLightbox(LightboxId)
                        Dim oImage As ImageInfo = ctlLightbox.GetImageByFileName(LightboxId, strFile)

                        ' need to get the thumbnail file name
                        Dim strThumbnail As String = GetThumbnailImageName(oImage.FileName, Me.PortalSettings, False)
                        
                        ' need to map to the file
                        If Not Regex.IsMatch(strThumbnail, "[/\\]+", RegexOptions.IgnoreCase) Then
                            strThumbnail = String.Concat(Me.PortalSettings.HomeDirectoryMapPath, oLightbox.GalleryFolder, strThumbnail)
                            strThumbnail = ReformatPathForLocal(strThumbnail)
                        End If

                        Dim oFolder As IFolderInfo = FolderManager.Instance().GetFolder(Me.PortalId, oLightbox.GalleryFolder)

                        If FileManager.Instance().FileExists(oFolder, strThumbnail) Then
                            Try
                                ' delete the thumbnail
                                Dim oFile As IFileInfo = ctlLightbox.GetImageFromProvider(PortalId, oFolder, strThumbnail)
                                FileManager.Instance().DeleteFile(oFile)
                            Catch ex As Exception
                                LogException(ex)
                            End Try
                        End If

                        ' drop from the database
                        ctlLightbox.DeleteImageById(oImage.ImageId)

                    Next

                End If

            End If ' If Not collImagesInDb Is Nothing AndAlso collImagesInDb.Count > 0 Then

        End Sub

        Private Function GetOrphanedFiles(ByVal FilesOnDatabase As ArrayList, ByRef FilesOnServer As ArrayList) As ArrayList

            Dim arrDifference As New ArrayList

            For Each item As String In FilesOnDatabase
                If Not FilesOnServer.Contains(item) Then
                    arrDifference.Add(item)
                End If
            Next

            Return arrDifference

        End Function

        Private Sub CreateThumbnails(ByVal Files As ArrayList)

            Dim oImage As Image = Nothing
            Dim oThumbnail As Image = Nothing

            For Each oFile As IFileInfo In Files

                ' only proceed if this is a real file name, and the file name doesn't already exist
                If Not String.IsNullOrEmpty(oFile.FileName) AndAlso Not Regex.IsMatch(oFile.FileName, THUMBNAIL_MATCH_PATTERN, RegexOptions.IgnoreCase) Then

                    ' only create a thumbnail if it doesn't exist
                    Dim strThumbnailFileName As String = GetThumbnailImageName(oFile.FileName, Me.PortalSettings, False)
                    Dim oFolder As IFolderInfo = FolderManager.Instance().GetFolder(Me.PortalId, oFile.Folder)

                    If Not FileManager.Instance().FileExists(oFolder, strThumbnailFileName) Then

                        Try
                            ' get an instance of the image
                            oImage = Image.FromStream(FileManager.Instance().GetFileContent(oFile))
                        Catch ex As Exception
                            LogException(ex)
                        End Try

                        If Not oImage Is Nothing Then
                            Dim intNewWidth As Integer = 100

                            If oImage.Width <= intNewWidth Then
                                intNewWidth = oImage.Width
                            End If

                            Dim intNewHeight As Integer = CType(oImage.Height * intNewWidth / oImage.Width, Integer)

                            If intNewHeight > MAX_IMAGE_HEIGHT Then
                                intNewWidth = CType(oImage.Width * MAX_IMAGE_HEIGHT / oImage.Height, Integer)
                                intNewHeight = MAX_IMAGE_HEIGHT
                            End If

                            ' create and save a thumbnail
                            oThumbnail = oImage.GetThumbnailImage(intNewWidth, intNewHeight, Nothing, Nothing)

                            ' save the image in memory
                            Dim ms As New MemoryStream

                            ' branch the save statement to avoid encoder errors
                            Select Case oFile.Extension.ToLower()
                                Case "jpg"
                                    oThumbnail.Save(ms, Imaging.ImageFormat.Jpeg)
                                Case "gif"
                                    oThumbnail.Save(ms, Imaging.ImageFormat.Gif)
                                Case "png"
                                    oThumbnail.Save(ms, Imaging.ImageFormat.Png)
                            End Select

                            ' save the image in the DNN file system
                            FileManager.Instance().AddFile(oFolder, strThumbnailFileName, ms)
                        End If

                    End If ' If Not File.Exists(ThumbnailImageName(oFile)) Then

                End If ' If Not String.IsNullOrEmpty(oFile) Then

            Next

        End Sub

        ''' <summary>
        ''' WriteOutLightboxConstructors - builds the JS constructors to add to the page for the fancybox plugin
        ''' </summary>
        ''' <remarks></remarks>
        ''' <history>
        ''' 20101013 - wstrohl - Added newlines to prevent javascript errors
        ''' </history>
        Private Sub WriteOutLightboxConstructors()

            If Not Me.AlbumIds Is Nothing Then

                Dim ctlLightbox As New LightboxController
                Dim objAlbum As New LightboxInfo
                Dim objSetting As New SettingInfo
                Dim sb As New StringBuilder(500)
                Dim strUlId As String = Null.NullString

                sb.Append("<script type=""text/javascript"" language=""javascript"">")
                sb.Append(Environment.NewLine)
                sb.Append("jQuery(document).ready(function (){ ")
                sb.Append(Environment.NewLine)

                For Each intI As Integer In AlbumIds
                    objAlbum = New LightboxInfo
                    objAlbum = ctlLightbox.GetLightbox(intI)
                    objSetting = ctlLightbox.GetSettings(objAlbum.LightboxId)

                    strUlId = GetGalleryListId(objAlbum.GalleryName)

                    With sb

                        .AppendFormat("jQuery('ul#{0} li span a.wns_lightbox_link').fancybox(", strUlId)
                        .Append(" { ")

                        .AppendFormat("'changeSpeed': {0}, ", objSetting.ChangeSpeed)
                        .AppendFormat("'cyclic': {0}, ", objSetting.Cyclic.ToString.ToLower)
                        .AppendFormat("'enableEscapeButton': {0}, ", objSetting.EnableEscapeButton.ToString.ToLower)
                        .AppendFormat("'margin': {0}, ", objSetting.Margin)
                        .AppendFormat("'modal': {0}, ", objSetting.Modal.ToString.ToLower)
                        If Not String.IsNullOrEmpty(objSetting.OnCancel) Then
                            .AppendFormat("'onCancel': {0}, ", objSetting.OnCancel)
                        End If
                        If Not String.IsNullOrEmpty(objSetting.OnCancel) Then
                            .AppendFormat("'onCancel': {0}, ", objSetting.OnCleanup)
                        End If
                        If Not String.IsNullOrEmpty(objSetting.OnClosed) Then
                            .AppendFormat("'onClosed': {0}, ", objSetting.OnClosed)
                        End If
                        If Not String.IsNullOrEmpty(objSetting.OnComplete) Then
                            .AppendFormat("'onComplete': {0}, ", objSetting.OnComplete)
                        End If
                        If Not String.IsNullOrEmpty(objSetting.OnStart) Then
                            .AppendFormat("'onState': {0}, ", objSetting.OnStart)
                        End If
                        .AppendFormat("'opacity': {0}, ", objSetting.Opacity.ToString.ToLower)
                        .AppendFormat("'overlayColor': '{0}', ", objSetting.OverlayColor)
                        '
                        ' This update has been made per 'codedude'
                        ' Issue: http://wnslightbox.codeplex.com/workitem/4605
                        ' Ref: http://wnslightbox.codeplex.com/Thread/View.aspx?ThreadId=208034
                        '.AppendFormat("'overlayOpacity': {0}, ", objSetting.OverlayOpacity)
                        .AppendFormat("'overlayOpacity': {0}, ", objSetting.OverlayOpacity.ToString.Replace(",", "."))
                        '
                        .AppendFormat("'overlayShow': {0}, ", objSetting.OverlayShow.ToString.ToLower)
                        .AppendFormat("'padding': {0}, ", objSetting.Padding)
                        .AppendFormat("'showCloseButton': {0}, ", objSetting.ShowCloseButton.ToString.ToLower)
                        .AppendFormat("'showNavArrows': {0}, ", objSetting.ShowNavArrows.ToString.ToLower)
                        .AppendFormat("'speedIn': {0}, ", objSetting.Speed.ToString.ToLower)
                        .AppendFormat("'speedOut': {0}, ", objSetting.Speed.ToString.ToLower)
                        .AppendFormat("'titlePosition': '{0}', ", objSetting.TitlePosition.ToString.ToLower)
                        .AppendFormat("'titleShow': {0}, ", objSetting.TitleShow.ToString.ToLower)
                        .AppendFormat("'transitionIn': '{0}', ", objSetting.Transition)
                        .AppendFormat("'transitionOut': '{0}' ", objSetting.Transition)

                        .Append(" } );")
                        sb.Append(Environment.NewLine)

                        If objAlbum.HideTitleDescription Then
                            .AppendFormat("jQuery('#h3-{0}-{1}, #p-{0}-{1}').remove();", TabModuleId, objAlbum.LightboxId)
                            sb.Append(Environment.NewLine)
                        End If

                    End With ' With sb
                Next

                sb.Append(" } );")
                sb.Append(Environment.NewLine)
                sb.Append("</script>")
                sb.Append(Environment.NewLine)

                If Not sb Is Nothing Then
                    Me.phScript.Controls.Add(New LiteralControl(sb.ToString))
                End If

            End If ' If Not Me.p_AlbumIds Is Nothing Then

        End Sub

        Private Function GetGalleryListId(ByVal GalleryName As String) As String
            Return String.Format("ul{0}_{1}", CleanNameForRel(GalleryName), Me.ModuleId)
        End Function

        Private Function GetFileName(ByVal FilePath As String) As String

            Return Path.GetFileName(FilePath)

        End Function

#End Region

#Region " URL Helper Methods "

        Protected Function GetEditUrl(ByVal GalleryId As Object) As String
            If Not GalleryId Is Nothing Then
                Return EditUrl("AlbumId", GalleryId.ToString, "Edit")
            Else
                Return "#"
            End If
        End Function

        Protected Function GetReorderUrl(ByVal GalleryId As Object) As String
            If Not GalleryId Is Nothing Then
                Return EditUrl("Album", GalleryId.ToString, "ChangeImageOrder")
            Else
                Return "#"
            End If
        End Function

        Private Function GetImageEditUrl(ByVal oImage As ImageInfo) As String

            Return EditUrl("Image", oImage.ImageId.ToString, "EditImage", String.Format("&Album={0}", oImage.LightboxId))

        End Function

        Private Function GetImageFileUrl(ByVal Image As IFileInfo) As String

            Try
                Dim ctlModule As New LightboxController
                Return ctlModule.GetImageFileUrl(Image)
            Catch ex As Exception
                If IsEditable AndAlso PortalSettings.UserMode = Entities.Portals.PortalSettings.Mode.Edit Then
                    ProcessModuleLoadException(Me, ex, True)
                Else
                    LogException(ex)
                End If
                Return String.Empty
            End Try

        End Function

#End Region

#Region " Clean Helper Methods "

        Protected Function CleanNameForRel(ByVal AlbumName As Object) As String
            If Not AlbumName Is Nothing Then
                Return CleanNameForRel(AlbumName.ToString)
            Else
                Return String.Empty
            End If
        End Function

        Protected Function CleanNameForRel(ByVal AlbumName As String) As String
            If Not String.IsNullOrEmpty(AlbumName) Then
                ' remove any non-alphanumeric characters from the GalleryName, and append to create a stand-alone lightbox album
                Return String.Concat("lightbox-", Regex.Replace(AlbumName, "\W", String.Empty))
            Else
                Return String.Empty
            End If
        End Function

        Private Function CleanForClientScript(ByVal Text As String) As String
            Return Regex.Replace(Text, "'", "\'")
        End Function

#End Region

#Region " IActionable Implementation "

        Public ReadOnly Property ModuleActions() As DotNetNuke.Entities.Modules.Actions.ModuleActionCollection Implements IActionable.ModuleActions
            Get
                Dim Actions As New Actions.ModuleActionCollection
                Actions.Add(GetNextActionID, Me.GetLocalizedString(MENU_ITEM_TITLE_KEY), _
                    String.Empty, String.Empty, String.Empty, _
                    EditUrl(String.Empty, String.Empty, EDIT_KEY), _
                    False, DotNetNuke.Security.SecurityAccessLevel.Edit, True, False)

                ' if there is more than one album, allow the change order view to be displayed

                Dim ctlModule As New LightboxController
                Dim intCount As Integer = ctlModule.GetLightboxCount(Me.ModuleId)

                If intCount > 1 Then

                    Actions.Add(GetNextActionID, Me.GetLocalizedString(MENU_ITEM_CHANGEORDER_KEY), _
                        String.Empty, String.Empty, String.Empty, _
                        EditUrl(String.Empty, String.Empty, CHANGE_ORDER), _
                        False, DotNetNuke.Security.SecurityAccessLevel.Edit, True, False)

                End If

                Return Actions
            End Get
        End Property

#End Region

    End Class

End Namespace