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
Imports DotNetNuke.Services.Exceptions
Imports DotNetNuke.Services.FileSystem
Imports DotNetNuke.Security.PortalSecurity
Imports DotNetNuke.UI.Skins.Skin
Imports DotNetNuke.UI.Skins.Controls.ModuleMessage
Imports System.Collections.Generic
Imports System.Text.RegularExpressions
Imports System.Web.UI.WebControls
Imports WillStrohl.Modules.Lightbox.LightboxController

Namespace WillStrohl.Modules.Lightbox

    Partial Public MustInherit Class EditLightbox
        Inherits WNSPortalModuleBase

#Region " Private Members "

        Private p_AlbumId As Integer = Null.NullInteger
        Private p_FolderHandler As String = String.Empty

        Private c_AutoCompleteKey As String = "jquery.autocomplete"

#End Region

#Region " Public Properties "

        Protected ReadOnly Property AlbumId() As Integer
            Get
                If Not Me.p_AlbumId > Null.NullInteger Then
                    Dim objGallery As Object = Request.Params("AlbumId")
                    If Not objGallery Is Nothing Then
                        Me.p_AlbumId = Integer.Parse(objGallery.ToString, Globalization.NumberStyles.Integer)
                    Else
                        Me.p_AlbumId = Null.NullInteger
                    End If
                End If

                Return Me.p_AlbumId
            End Get
        End Property

        Protected ReadOnly Property FolderHandler() As String
            Get
                If Not String.IsNullOrEmpty(Me.p_FolderHandler) Then
                    Return Me.p_FolderHandler
                End If

                Me.p_FolderHandler = String.Concat(Me.ControlPath, "folders.ashx")

                Return Me.p_FolderHandler
            End Get
        End Property

#End Region

#Region " Event Handlers "

        Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            Try
                If Not Me.Page.IsPostBack Then
                    Me.BindData()
                End If
            Catch exc As Exception ' Module failed to load
                ProcessModuleLoadException(Me, exc, Me.IsEditable)
            End Try
        End Sub

        Private Sub cmdUpdate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdUpdate.Click
            If Page.IsValid Then
                Me.SaveGallery()
            End If
        End Sub

        Private Sub cmdDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdDelete.Click
            Dim ctlGallery As New LightboxController
            ctlGallery.DeleteLightbox(AlbumId)

            Me.SendToDefaultView()
        End Sub

        Private Sub cmdCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdCancel.Click
            Me.SendToDefaultView()
        End Sub

        Private Sub cmdDeleteThumbnails_Click(sender As Object, e As System.EventArgs) Handles cmdDeleteThumbnails.Click
            Me.DeleteThumbnails()
            Me.SendToDefaultView()
        End Sub

#End Region

#Region " Private Helper Functions "

        Private Sub BindData()

            Me.LocalizeModule()

            ' hide the delete button when adding a new album to the module
            Me.cmdDelete.Visible = (Me.AlbumId > Null.NullInteger)

            Me.cmdDeleteThumbnails.Visible = (Me.AlbumId > Null.NullInteger)

            If Not Page.IsPostBack Then

                If Me.lstTitlePosition.Items.Count = 0 Then
                    Me.lstTitlePosition.Items.Add(New ListItem(Me.GetLocalizedString("lstTitlePosition.Item.Outside"), "outside"))
                    Me.lstTitlePosition.Items.Add(New ListItem(Me.GetLocalizedString("lstTitlePosition.Item.Inside"), "inside"))
                    Me.lstTitlePosition.Items.Add(New ListItem(Me.GetLocalizedString("lstTitlePosition.Item.Over"), "over"))
                    Me.lstTitlePosition.Items.Insert(0, New ListItem("---"))
                End If

                If Me.lstTransition.Items.Count = 0 Then
                    Me.lstTransition.Items.Add(New ListItem(Me.GetLocalizedString("lstTransition.Item.Elastic"), "elastic"))
                    Me.lstTransition.Items.Add(New ListItem(Me.GetLocalizedString("lstTransition.Item.Fade"), "fade"))
                    Me.lstTransition.Items.Add(New ListItem(Me.GetLocalizedString("lstTransition.Item.None"), "none"))
                    Me.lstTransition.Items.Insert(0, New ListItem("---"))
                End If

                If Me.AlbumId > Null.NullInteger Then

                    ' Load the lightbox album information into the form
                    Dim ctlLightbox As New LightboxController
                    Dim objAlbum As New LightboxInfo
                    objAlbum = ctlLightbox.GetLightbox(Me.AlbumId)

                    Me.txtGalleryName.Text = Server.HtmlDecode(objAlbum.GalleryName)
                    Me.txtGalleryDescription.Text = Server.HtmlDecode(objAlbum.GalleryDescription)
                    Me.cboGalleryFolder.Text = Server.HtmlDecode(objAlbum.GalleryFolder)
                    Me.chkHideTitleDescription.Checked = objAlbum.HideTitleDescription

                    Dim objSetting As New SettingInfo
                    objSetting = ctlLightbox.GetSettings(objAlbum.LightboxId)

                    Me.txtPadding.Text = objSetting.Padding.ToString
                    Me.txtMargin.Text = objSetting.Margin.ToString
                    Me.chkOpacity.Checked = objSetting.Opacity
                    Me.chkModal.Checked = objSetting.Modal
                    Me.chkCyclic.Checked = objSetting.Cyclic
                    Me.chkOverlayShow.Checked = objSetting.OverlayShow
                    Me.txtOverlayOpacity.Text = objSetting.OverlayOpacity.ToString
                    Me.txtOverlayColor.Text = objSetting.OverlayColor
                    Me.chkTitleShow.Checked = objSetting.TitleShow
                    Me.lstTitlePosition.Items.FindByValue(objSetting.TitlePosition).Selected = True
                    Me.lstTransition.Items.FindByValue(objSetting.Transition).Selected = True
                    Me.txtSpeed.Text = objSetting.Speed.ToString
                    Me.txtChangeSpeed.Text = objSetting.ChangeSpeed.ToString
                    Me.chkShowCloseButton.Checked = objSetting.ShowCloseButton
                    Me.chkShowNavArrows.Checked = objSetting.ShowNavArrows
                    Me.chkEnableEscapeButton.Checked = objSetting.EnableEscapeButton
                    Me.txtOnStart.Text = objSetting.OnStart
                    Me.txtOnCancel.Text = objSetting.OnCancel
                    Me.txtOnComplete.Text = objSetting.OnComplete
                    Me.txtOnCleanup.Text = objSetting.OnCleanup
                    Me.txtOnClosed.Text = objSetting.OnClosed

                Else

                    ' Set Default Values:
                    ' The empty settings object will have the default values already assigned
                    Dim objSetting As New SettingInfo

                    Me.txtPadding.Text = objSetting.Padding.ToString
                    Me.txtMargin.Text = objSetting.Margin.ToString
                    Me.chkOpacity.Checked = objSetting.Opacity
                    Me.chkModal.Checked = objSetting.Modal
                    Me.chkCyclic.Checked = objSetting.Cyclic
                    Me.chkOverlayShow.Checked = objSetting.OverlayShow
                    Me.txtOverlayOpacity.Text = objSetting.OverlayOpacity.ToString
                    Me.txtOverlayColor.Text = objSetting.OverlayColor
                    Me.chkTitleShow.Checked = objSetting.TitleShow
                    Me.lstTitlePosition.Items.FindByValue(objSetting.TitlePosition).Selected = True
                    Me.lstTransition.Items.FindByValue(objSetting.Transition).Selected = True
                    Me.txtSpeed.Text = objSetting.Speed.ToString
                    Me.txtChangeSpeed.Text = objSetting.ChangeSpeed.ToString
                    Me.chkShowCloseButton.Checked = objSetting.ShowCloseButton
                    Me.chkShowNavArrows.Checked = objSetting.ShowNavArrows
                    Me.chkEnableEscapeButton.Checked = objSetting.EnableEscapeButton
                    Me.txtOnStart.Text = objSetting.OnStart
                    Me.txtOnCancel.Text = objSetting.OnCancel
                    Me.txtOnComplete.Text = objSetting.OnComplete
                    Me.txtOnCleanup.Text = objSetting.OnCleanup
                    Me.txtOnClosed.Text = objSetting.OnClosed

                End If

                Dim ctlFolder As New FolderController
                Dim collFolder As IEnumerable(Of IFolderInfo) = FolderManager.Instance.GetFolders(Me.PortalId)

                Dim lstFolder As New List(Of IFolderInfo)

                For Each oFolder As IFolderInfo In collFolder
                    If Not String.IsNullOrEmpty(oFolder.FolderPath) Then
                        lstFolder.Add(oFolder)
                    End If
                Next

                Me.cboGalleryFolder.AllowCustomText = True
                Me.cboGalleryFolder.MarkFirstMatch = True
                Me.cboGalleryFolder.ClearSelection()
                Me.cboGalleryFolder.Items.Clear()
                Me.cboGalleryFolder.DataTextField = "FolderPath"
                Me.cboGalleryFolder.DataValueField = "FolderPath"
                Me.cboGalleryFolder.DataSource = lstFolder
                Me.cboGalleryFolder.DataBind()

            End If

        End Sub

        Private Sub LocalizeModule()

            ' localize the text
            Me.cmdCancel.Text = Me.GetLocalizedString("cmdCancel.Text")
            Me.cmdDelete.Text = Me.GetLocalizedString("cmdDelete.Text")
            Me.cmdDeleteThumbnails.Text = Me.GetLocalizedString("cmdDeleteThumbnails.Text")
            Me.cmdUpdate.Text = Me.GetLocalizedString("cmdUpdate.Text")
            Me.vsError.HeaderText = String.Format("<span class=""NormalBold"">{0}</span>", Me.GetLocalizedString("vsError.HeaderText"))
            Me.rfvGalleryName.ErrorMessage = Me.GetLocalizedString("rfvGalleryName.ErrorMessage")
            Me.rfvGalleryFolder.ErrorMessage = Me.GetLocalizedString("rfvGalleryFolder.ErrorMessage")
            Me.rfvGalleryDescription.ErrorMessage = Me.GetLocalizedString("rfvGalleryDescription.ErrorMessage")

            AddModuleMessage(Me, Me.GetLocalizedString("lblFormMessage.Text"), ModuleMessageType.BlueInfo)

            Me.rfvPadding.ErrorMessage = Me.GetLocalizedString("rfvPadding.ErrorMessage")
            Me.revPadding.ErrorMessage = Me.GetLocalizedString("revPadding.ErrorMessage")
            Me.rfvMargin.ErrorMessage = Me.GetLocalizedString("rfvMargin.ErrorMessage")
            Me.revMargin.ErrorMessage = Me.GetLocalizedString("revMargin.ErrorMessage")
            Me.rfvOverlayOpacity.ErrorMessage = Me.GetLocalizedString("rfvOverlayOpacity.ErrorMessage")
            Me.revOverlayOpacity.ErrorMessage = Me.GetLocalizedString("revOverlayOpacity.ErrorMessage")
            Me.rfvOverlayColor.ErrorMessage = Me.GetLocalizedString("rfvOverlayColor.ErrorMessage")
            Me.rfvTitlePosition.ErrorMessage = Me.GetLocalizedString("rfvTitlePosition.ErrorMessage")
            Me.rfvTransition.ErrorMessage = Me.GetLocalizedString("rfvTransition.ErrorMessage")
            Me.rfvSpeed.ErrorMessage = Me.GetLocalizedString("rfvSpeed.ErrorMessage")
            Me.revSpeed.ErrorMessage = Me.GetLocalizedString("revSpeed.ErrorMessage")
            Me.rfvChangeSpeed.ErrorMessage = Me.GetLocalizedString("rfvChangeSpeed.ErrorMessage")
            Me.revChangeSpeed.ErrorMessage = Me.GetLocalizedString("revChangeSpeed.ErrorMessage")

        End Sub

        Private Sub SaveGallery()

            Dim ctlGallery As New LightboxController
            Dim ctlSecurity As New Security.PortalSecurity
            Dim oGallery As New LightboxInfo

            If Me.AlbumId > Null.NullInteger Then
                oGallery = ctlGallery.GetLightbox(Me.AlbumId)
            End If

            With oGallery
                .GalleryDescription = ctlSecurity.InputFilter(Me.txtGalleryDescription.Text, FilterFlag.NoScripting)
                .GalleryFolder = ctlSecurity.InputFilter(Me.cboGalleryFolder.Text, FilterFlag.NoScripting)
                .GalleryName = ctlSecurity.InputFilter(Me.txtGalleryName.Text, FilterFlag.NoScripting)
                .HideTitleDescription = Me.chkHideTitleDescription.Checked
                .ModuleId = Me.ModuleId
                .LastUpdatedBy = UserId
            End With

            Dim oSettings As SettingInfo = ctlGallery.GetSettings(Me.AlbumId)

            If oSettings Is Nothing Then
                oSettings = New SettingInfo
            End If

            With oSettings
                .ChangeSpeed = Integer.Parse(Me.txtChangeSpeed.Text, Globalization.NumberStyles.Integer)
                .Cyclic = Me.chkCyclic.Checked
                .EnableEscapeButton = Me.chkEnableEscapeButton.Checked
                .Margin = Integer.Parse(Me.txtMargin.Text, Globalization.NumberStyles.Integer)
                .Modal = Me.chkModal.Checked
                .OnCancel = Me.txtOnCancel.Text
                .OnCleanup = Me.txtOnCleanup.Text
                .OnClosed = Me.txtOnClosed.Text
                .OnComplete = Me.txtOnComplete.Text
                .OnStart = Me.txtOnStart.Text
                .Opacity = Me.chkOpacity.Checked
                .OverlayColor = Me.txtOverlayColor.Text
                .OverlayOpacity = Decimal.Parse(Me.txtOverlayOpacity.Text, Globalization.NumberStyles.Float)
                .OverlayShow = Me.chkOverlayShow.Checked
                .Padding = Integer.Parse(Me.txtPadding.Text, Globalization.NumberStyles.Integer)
                .ShowCloseButton = Me.chkShowCloseButton.Checked
                .ShowNavArrows = Me.chkShowNavArrows.Checked
                .Speed = Integer.Parse(Me.txtSpeed.Text, Globalization.NumberStyles.Integer)
                .TitlePosition = Me.lstTitlePosition.SelectedValue
                .TitleShow = Me.chkTitleShow.Checked
                .Transition = Me.lstTransition.SelectedValue
            End With

            If Me.AlbumId > Null.NullInteger Then
                oGallery.LightboxId = AlbumId
                ctlGallery.UpdateLightbox(oGallery)
                ctlGallery.UpdateSetting(oSettings)
            Else
                oSettings.LightboxId = ctlGallery.AddLightbox(oGallery)
                ctlGallery.AddSetting(oSettings)
            End If

            Me.SendToDefaultView()

        End Sub

        Private Sub DeleteThumbnails()

            Dim ctlLightbox As New LightboxController
            Dim oLightbox As LightboxInfo = ctlLightbox.GetLightbox(Me.AlbumId)
            Dim strFolderName As String = oLightbox.GalleryFolder

            ' get a list of the files in the lightbox directory
            If Regex.IsMatch(strFolderName, FOLDER_NAME_MATCH_PATTERN) Then
                strFolderName = Regex.Replace(strFolderName, FOLDER_NAME_REPLACE_PATTERN, FOLDER_NAME_REPLACEMENT_PATTERN)
            End If

            If FolderManager.Instance().FolderExists(Me.PortalId, strFolderName) Then

                Dim oFolder As IFolderInfo = FolderManager.Instance.GetFolder(Me.PortalId, strFolderName)
                Dim arrImages As ArrayList

                ' parse for each thumbnail
                arrImages = VerifiedFileList(FolderManager.Instance().GetFiles(oFolder))

                ' delete the thumbnail
                For Each oImage As IFileInfo In arrImages
                    Dim strThumbnail As String = GetThumbnailImageName(oImage.FileName, Me.PortalSettings, False)
                    If FileManager.Instance().FileExists(oFolder, strThumbnail) Then
                        Try
                            Dim oThumbnail As IFileInfo = FileManager.Instance().GetFile(oFolder, strThumbnail)
                            FileManager.Instance().DeleteFile(oThumbnail)
                        Catch ex As Exception
                            LogException(ex)
                        End Try
                    End If
                Next

            End If

        End Sub

        Private Sub SendToDefaultView()
            Response.Redirect(NavigateURL)
        End Sub

        Private Sub HandleException(ByVal exc As Exception)
            LogException(exc)
            If Me.UserInfo.IsSuperUser Or Me.UserInfo.UserID = PortalSettings.AdministratorId Then
                AddModuleMessage(Me, _
                    String.Concat(exc.Message, "<br />", exc.StackTrace), _
                    Skins.Controls.ModuleMessage.ModuleMessageType.RedError)
            Else
                AddModuleMessage(Me, exc.Message, _
                    Skins.Controls.ModuleMessage.ModuleMessageType.RedError)
            End If
        End Sub

#End Region

    End Class

End Namespace