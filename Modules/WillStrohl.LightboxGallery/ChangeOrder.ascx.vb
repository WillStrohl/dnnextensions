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

Imports DotNetNuke.Services.Exceptions
Imports DotNetNuke.UI.Skins.Skin
Imports DotNetNuke.UI.Skins.Controls.ModuleMessage
Imports System.Text
Imports System.Web.UI
Imports WillStrohl.Modules.Lightbox.LightboxController

Namespace WillStrohl.Modules.Lightbox

    Partial Public MustInherit Class ChangeOrderView
        Inherits WNSPortalModuleBase

#Region " Private Members "

        Private p_jQueryUIKey As String = "jQuery.ui"
        Private p_OrderHandler As String = String.Empty

#End Region

#Region " Public Properties "

        Protected ReadOnly Property OrderHandler() As String
            Get
                If Not String.IsNullOrEmpty(Me.p_OrderHandler) Then
                    Return Me.p_OrderHandler
                End If

                'If Request.IsSecureConnection Then
                '    Me.p_OrderHandler = String.Concat("https://", Request.Url.Host, Me.ControlPath, "order.ashx")
                'Else
                '    Me.p_OrderHandler = String.Concat("http://", Request.Url.Host, Me.ControlPath, "order.ashx")
                'End If
                Me.p_OrderHandler = String.Concat(Me.ControlPath, "order.ashx")

                Return Me.p_OrderHandler
            End Get
        End Property

#End Region

#Region " Event Handlers "

        Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            Try

                ' if there aren't enough albums to re-order, send the visitor back to the main view
                Me.CheckForAlbums()

                Me.BindData()

                Me.Page.Header.Controls.Add(New LiteralControl(String.Format(STYLESHEET_TAG_FORMAT, String.Concat(Me.ControlPath, "js/fancybox/jquery.fancybox-1.3.4.css"))))

                Me.Page.Header.Controls.Add(New LiteralControl(String.Format(STYLESHEET_TAG_FORMAT, String.Concat(Me.ControlPath, "js/jquery-ui-1.8.2.css"))))

            Catch exc As Exception ' Module failed to load
                ProcessModuleLoadException(Me, exc, Me.IsEditable)
            End Try
        End Sub

#End Region

#Region " Private Helper Functions "

        Private Sub BindData()

            Me.phOrder.Controls.Add(New LiteralControl(Me.BuildAlbumList))

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

        Private Sub CheckForAlbums()

            Dim ctlModule As New LightboxController
            Dim intCount As Integer = ctlModule.GetLightboxCount(Me.ModuleId)

            If Not intCount > 1 Then

                Me.SendToDefaultView()

            End If

        End Sub

        Private Function BuildAlbumList() As String

            Dim ctlModule As New LightboxController
            Dim collAlbum As New LightboxInfoCollection
            collAlbum = ctlModule.GetLightboxes(Me.ModuleId)

            If Not collAlbum Is Nothing AndAlso collAlbum.Count > 0 Then

                Dim sb As New StringBuilder(10)

                With sb

                    .Append("<ul id=""ulOrder"" class=""ui-sortable"">")

                    For Each oAlbum As LightboxInfo In collAlbum

                        sb.AppendFormat("<li id=""{0}"" title=""{1}"" class=""wns-sortable ui-state-default""><img src=""{2}"" alt=""{3}"" title=""{3}"" />{1}</li>", oAlbum.LightboxId, oAlbum.GalleryName, String.Concat(Me.TemplateSourceDirectory, "/images/arrow-move.png"), Me.GetLocalizedString("Image.Resort.AltText"))

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