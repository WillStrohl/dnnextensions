'
' Will Strohl (will.strohl@gmail.com)
' http://www.willstrohl.com
'
'Copyright (c) 2013, Will Strohl
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
'Neither the name of Will Strohl, Contact Collector, nor the names of its contributors may be 
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

Imports System.Web.UI.WebControls
Imports WillStrohl.Modules.ContactCollector.ContactCollectorController

Namespace WillStrohl.Modules.ContactCollector

    Partial Public Class ViewContacts
        Inherits WNSPortalModuleBase

#Region " Event Handlers "

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        ''' <history>
        ''' </history>
        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            Try
                If Not Page.IsPostBack Then
                    Me.BindData()
                End If
            Catch exc As Exception ' Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub

        Private Sub grdContact_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles grdContact.PageIndexChanging
            If Me.grdContact.EditIndex <> -1 Then
                e.Cancel = True
            Else
                Me.grdContact.PageIndex = e.NewPageIndex
                Me.BindData()
            End If
        End Sub

        Private Sub grdContact_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles grdContact.RowCommand
            Select Case e.CommandName
                Case "Delete"
                    Try
                        Dim ctlContact As New ContactCollectorController
                        Dim intContact As Integer = Integer.Parse(Me.grdContact.DataKeys(Integer.Parse(e.CommandArgument.ToString, Globalization.NumberStyles.Integer)).Value.ToString, Globalization.NumberStyles.Integer)
                        ctlContact.DeleteContact(intContact)
                        Me.ReloadPage()
                    Catch ex As Exception
                        Services.Exceptions.LogException(ex)
                        If Me.IsEditable Then
                            UI.Skins.Skin.AddModuleMessage(Me, _
                                String.Concat(ex.Message, "<br />", ex.StackTrace), _
                                Skins.Controls.ModuleMessage.ModuleMessageType.RedError)
                        Else
                            UI.Skins.Skin.AddModuleMessage(Me, _
                                Me.GetLocalizedString("Message.Failure"), _
                                Skins.Controls.ModuleMessage.ModuleMessageType.RedError)
                        End If
                    End Try
            End Select
        End Sub

        Private Sub grdContact_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles grdContact.RowDeleting
            e.Cancel = False
        End Sub

        Private Sub cmdReturn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdReturn.Click
            Response.Redirect(NavigateURL)
        End Sub

        Private Sub lnkExport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkExport.Click

            If Me.Page.IsValid Then
                Dim ctlModule As New ContactCollectorController
                Select Case Me.cboExport.SelectedValue
                    Case "CSV"
                        ctlModule.ExportData(Me.ModuleId, Me.ModuleConfiguration.DesktopModule.ModuleName, ExportType.CSV)
                    Case "Excel"
                        ctlModule.ExportData(Me.ModuleId, Me.ModuleConfiguration.DesktopModule.ModuleName, ExportType.Excel)
                    Case "XML"
                        ctlModule.ExportData(Me.ModuleId, Me.ModuleConfiguration.DesktopModule.ModuleName, ExportType.XML)
                End Select
            End If

        End Sub

#End Region

#Region " Private Methods "

        Private Sub BindData()
            Me.LocalizeModule()

            Dim ctlContact As New ContactCollectorController
            Me.grdContact.DataSource = ctlContact.GetContacts(Me.ModuleId)
            Me.grdContact.DataBind()
        End Sub

        Private Sub LocalizeModule()
            Localization.LocalizeGridView(Me.grdContact, Me.LocalResourceFile)
            Me.grdContact.EmptyDataText = Me.GetLocalizedString("grdContact.EmptyDataText")
            Me.cmdReturn.Text = Me.GetLocalizedString("cmdReturn.Text")

            Me.cboExport.Items.Add(New ListItem("---", "---"))
            Me.cboExport.Items.Add(New ListItem(Me.GetLocalizedString("cboExport.Items.CSV"), "CSV"))
            Me.cboExport.Items.Add(New ListItem(Me.GetLocalizedString("cboExport.Items.Excel"), "Excel"))
            Me.cboExport.Items.Add(New ListItem(Me.GetLocalizedString("cboExport.Items.XML"), "XML"))
            Me.lnkExport.Text = Me.GetLocalizedString("lnkExport.Text")
            Me.rfvExport.ErrorMessage = Me.GetLocalizedString("rfvExport.ErrorMessage")
        End Sub

        Private Sub ReloadPage()
            Response.Redirect(EditUrl(String.Empty, String.Empty, "ViewContacts"))
        End Sub

#End Region

    End Class

End Namespace