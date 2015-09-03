'
' Will Strohl (will.strohl@gmail.com)
' http://www.willstrohl.com
'
'Copyright (c) 2009-2015, Will Strohl
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
'Neither the name of Will Strohl, Content Injection, nor the names of its contributors may be 
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
Imports DotNetNuke.Services.Exceptions
Imports DotNetNuke.UI.Skins.Skin
Imports DotNetNuke.UI.Skins.Controls.ModuleMessage
Imports System.Web.UI

Namespace WillStrohl.Modules.Injection

    Partial Public MustInherit Class ViewInjection
        Inherits WNSPortalModuleBase
        Implements IActionable

#Region " Private Members "

        Private p_Header As String = String.Empty
        Private p_Footer As String = String.Empty
        Private p_EditInjectionUrl As String = String.Empty

#End Region

#Region " Properties "

        Private ReadOnly Property HeaderInjection() As String
            Get
                Return Me.p_Header
            End Get
        End Property

        Private ReadOnly Property FooterInjection() As String
            Get
                Return Me.p_Footer
            End Get
        End Property

        Private ReadOnly Property EditInjectionUrl() As String
            Get
                If Not String.IsNullOrEmpty(Me.p_EditInjectionUrl) Then
                    Return Me.p_EditInjectionUrl
                End If

                Me.p_EditInjectionUrl = EditUrl(String.Empty, String.Empty, "Edit")

                Return Me.p_EditInjectionUrl
            End Get
        End Property

#End Region

#Region " Event Handlers "

        Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            Try
                If Me.IsEditable AndAlso Me.PortalSettings.UserMode = DotNetNuke.Entities.Portals.PortalSettings.Mode.Edit Then
                    ' If IsEditable, then the visitor has edit permissions to the module, is 
                    ' currently logged in, and the portal is in edit mode.
                    AddModuleMessage(Me, Me.GetLocalizedString("InjectionInfo.Text"), ModuleMessageType.BlueInfo)
                Else
                    ' hide the module container (and the rest of the module as well)
                    Me.ContainerControl.Visible = False
                End If

                ' inject any strings insto the page
                Me.ExecutePageInjection()

            Catch exc As Exception ' Module failed to load
                ProcessModuleLoadException(Me, exc, Me.IsEditable)
            End Try
        End Sub

        Public Sub InjectIntoFooter(ByVal sender As Object, ByVal e As EventArgs)
            If Not String.IsNullOrEmpty(FooterInjection) Then
                Me.Page.Form.Controls.Add(New LiteralControl(FooterInjection))
            End If
        End Sub

#End Region

#Region " Private Helper Methods "

        Private Sub ExecutePageInjection()

            Dim ctlModule As New InjectionController
            Dim collInj As New InjectionInfoCollection
            collInj = ctlModule.GetActiveInjectionContents(Me.ModuleId)

            If collInj.Count > 0 Then

                For Each objInj As InjectionInfo In collInj
                    If objInj.InjectTop Then
                        Me.p_Header = String.Concat(Me.p_Header, Server.HtmlDecode(objInj.InjectContent))
                    Else
                        Me.p_Footer = String.Concat(Me.p_Footer, Server.HtmlDecode(objInj.InjectContent))
                    End If
                Next

                ' add the injection content to the header
                If Not String.IsNullOrEmpty(HeaderInjection) Then
                    Me.Parent.Page.Header.Controls.Add(New LiteralControl(HeaderInjection))
                End If

                ' add the injection content to the footer
                If Not String.IsNullOrEmpty(FooterInjection) Then
                    AddHandler Me.Page.LoadComplete, New EventHandler(AddressOf InjectIntoFooter)
                End If

            End If

        End Sub

#End Region

#Region " IActionable Implementation "

        Public ReadOnly Property ModuleActions() As DotNetNuke.Entities.Modules.Actions.ModuleActionCollection Implements IActionable.ModuleActions
            Get
                Dim Actions As New Actions.ModuleActionCollection
                Actions.Add(GetNextActionID, Me.GetLocalizedString("EditInjection.MenuItem.Title"), _
                    String.Empty, String.Empty, String.Empty, _
                    Me.EditInjectionUrl, _
                    False, DotNetNuke.Security.SecurityAccessLevel.Edit, True, False)
                Return Actions
            End Get
        End Property

#End Region

    End Class

End Namespace