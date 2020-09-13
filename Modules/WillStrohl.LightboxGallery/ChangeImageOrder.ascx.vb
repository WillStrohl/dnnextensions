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

Imports DotNetNuke.UI.Skins.Skin
Imports DotNetNuke.UI.Skins.Controls.ModuleMessage
Imports System.Text
Imports System.Web.UI
Imports DotNetNuke.Framework.JavaScriptLibraries
Imports DotNetNuke.Web.Client
Imports DotNetNuke.Web.Client.ClientResourceManagement
Imports DotNetNuke.Web.Client.Providers
Imports WillStrohl.Modules.Lightbox.LightboxController

Namespace WillStrohl.Modules.Lightbox

    Partial Public MustInherit Class ChangeImageOrderView
        Inherits WNSPortalModuleBase

#Region " Private Members "

        Private p_LightboxId As Integer = Null.NullInteger

#End Region

#Region " Properties "

        Protected ReadOnly Property LightboxId As Integer
            Get
                If Me.p_LightboxId > 0 Then
                    Return Me.p_LightboxId
                End If

                Integer.TryParse(Request.QueryString("album").ToString, Me.p_LightboxId)

                Return Me.p_LightboxId
            End Get
        End Property

#End Region

#Region " Event Handlers "

        Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            Try

                ' if there aren't enough albums to re-order, send the visitor back to the main view
                Me.CheckForImages()

                Me.BindData()

                JavaScript.RequestRegistration(CommonJs.DnnPlugins)

                ServicesFramework.Instance.RequestAjaxAntiForgerySupport()

                ClientResourceManager.RegisterScript(Me.Page, "https://cdnjs.cloudflare.com/ajax/libs/fancybox/3.5.7/jquery.fancybox.min.js", FileOrder.Js.DefaultPriority + 1, DnnBodyProvider.DefaultName)
                ClientResourceManager.RegisterStyleSheet(Me.Page, "https://cdnjs.cloudflare.com/ajax/libs/fancybox/3.5.7/jquery.fancybox.min.css", FileOrder.Css.DefaultPriority + 1, DnnPageHeaderProvider.DefaultName)
                
                ClientResourceManager.RegisterScript(Me.Page, "https://cdnjs.cloudflare.com/ajax/libs/jqueryui/1.12.1/jquery-ui.min.js", FileOrder.Js.DefaultPriority + 2, DnnBodyProvider.DefaultName)
                ClientResourceManager.RegisterStyleSheet(Me.Page, "https://cdnjs.cloudflare.com/ajax/libs/jqueryui/1.12.1/themes/base/jquery-ui.min.css", FileOrder.Css.DefaultPriority + 2, DnnPageHeaderProvider.DefaultName)
                ClientResourceManager.RegisterStyleSheet(Me.Page, "https://cdnjs.cloudflare.com/ajax/libs/jqueryui/1.12.1/themes/base/theme.min.css", FileOrder.Css.DefaultPriority + 3, DnnPageHeaderProvider.DefaultName)

            Catch exc As Exception ' Module failed to load
                ProcessModuleLoadException(Me, exc, Me.IsEditable)
            End Try
        End Sub

#End Region

#Region " Private Helper Functions "

        Private Sub BindData()

            Me.phOrder.Controls.Add(New LiteralControl(Me.BuildImageList))

            AddModuleMessage(Me, Me.GetLocalizedString("ChangeOrder.Text"), ModuleMessageType.BlueInfo)

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

        Private Sub CheckForImages()

            'Dim ctlModule As New LightboxController
            'Dim intCount As Integer = ctlModule.GetImageCount(Me.LightboxId)

            'If Not intCount > 1 Then

            '    Me.SendToDefaultView()

            'End If

        End Sub

        Private Function BuildImageList() As String

            Dim ctlModule As New LightboxController
            Dim oLightbox As LightboxInfo = ctlModule.GetLightbox(Me.LightboxId)
            Dim collImage As New ImageInfoCollection
            collImage = ctlModule.GetImages(Me.LightboxId)

            If Not collImage Is Nothing AndAlso collImage.Count > 0 Then

                Dim sb As New StringBuilder(10)

                With sb

                    .Append("<ul id=""ulOrderImages"" class=""ui-sortable"">")

                    For Each oImage As ImageInfo In collImage

                        sb.AppendFormat("<li id=""{0}"" title=""{1}"" class=""wns-sortable ui-state-default""><img src=""{2}"" alt=""{3}"" title=""{3}"" /></li>", _
                            oImage.ImageId, _
                            oImage.Title, _
                            GetThumbnailImageName(String.Concat(Me.PortalSettings.HomeDirectory, oLightbox.GalleryFolder, oImage.FileName), Me.PortalSettings, True), _
                            oImage.Title)

                    Next

                    .Append("</ul>")

                End With

                Return sb.ToString

            End If

            Return String.Format("<p class=""Normal"">{0}</p>", Me.GetLocalizedString("NoAlbums.Text"))

        End Function

#End Region

    End Class

End Namespace