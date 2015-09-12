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

Imports System.Globalization
Imports System.Web.UI.WebControls
Imports DotNetNuke.UI.Skins
Imports DotNetNuke.UI.Skins.Controls
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
        Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
            Try
                If Not Page.IsPostBack Then
                    BindData()
                End If
            Catch exc As Exception ' Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub

        Private Sub grdContact_PageIndexChanging(ByVal sender As Object, ByVal e As GridViewPageEventArgs) Handles grdContact.PageIndexChanging
            If grdContact.EditIndex <> -1 Then
                e.Cancel = True
            Else
                grdContact.PageIndex = e.NewPageIndex
                BindData()
            End If
        End Sub

        Private Sub grdContact_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs) Handles grdContact.RowCommand
            Select Case e.CommandName
                Case "Delete"
                    Try

                        Dim ctlContact As New ContactCollectorController
                        Dim intContact As Integer = Integer.Parse(grdContact.DataKeys(Integer.Parse(e.CommandArgument.ToString, NumberStyles.Integer)).Value.ToString, NumberStyles.Integer)

                        ctlContact.DeleteContact(intContact)

                        ReloadPage()

                    Catch ex As Exception
                        LogException(ex)

                        If IsEditable Then
                            Skin.AddModuleMessage(Me, _
                                String.Concat(ex.Message, "<br />", ex.StackTrace), _
                                ModuleMessage.ModuleMessageType.RedError)
                        Else
                            Skin.AddModuleMessage(Me, _
                                GetLocalizedString("Message.Failure"), _
                                ModuleMessage.ModuleMessageType.RedError)
                        End If
                    End Try
            End Select
        End Sub

        Private Sub grdContact_RowDeleting(ByVal sender As Object, ByVal e As GridViewDeleteEventArgs) Handles grdContact.RowDeleting
            e.Cancel = False
        End Sub

        Private Sub cmdReturn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdReturn.Click
            Response.Redirect(NavigateURL)
        End Sub

        Private Sub lnkExport_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lnkExport.Click

            If Page.IsValid Then
                Dim ctlModule As New ContactCollectorController
                Select Case cboExport.SelectedValue
                    Case "CSV"
                        ctlModule.ExportData(ModuleId, ModuleConfiguration.DesktopModule.ModuleName, ExportType.CSV)
                    Case "Excel"
                        ctlModule.ExportData(ModuleId, ModuleConfiguration.DesktopModule.ModuleName, ExportType.Excel)
                    Case "XML"
                        ctlModule.ExportData(ModuleId, ModuleConfiguration.DesktopModule.ModuleName, ExportType.XML)
                End Select
            End If

        End Sub

#End Region

#Region " Private Methods "

        Private Sub BindData()
            LocalizeModule()

            Dim ctlContact As New ContactCollectorController
            Dim contacts As ContactInfoCollection = ctlContact.GetContacts(ModuleId)

            grdContact.DataSource = contacts
            grdContact.DataBind()

            divExport.Visible = (Not contacts Is Nothing) AndAlso (contacts.Count > 0)
        End Sub

        Private Sub LocalizeModule()
            Localization.LocalizeGridView(grdContact, LocalResourceFile)

            grdContact.EmptyDataText = GetLocalizedString("grdContact.EmptyDataText")

            cmdReturn.Text = GetLocalizedString("cmdReturn.Text")

            cboExport.Items.Add(New ListItem("---", "---"))
            cboExport.Items.Add(New ListItem(GetLocalizedString("cboExport.Items.CSV"), "CSV"))
            cboExport.Items.Add(New ListItem(GetLocalizedString("cboExport.Items.Excel"), "Excel"))
            cboExport.Items.Add(New ListItem(GetLocalizedString("cboExport.Items.XML"), "XML"))

            lnkExport.Text = GetLocalizedString("lnkExport.Text")

            rfvExport.ErrorMessage = GetLocalizedString("rfvExport.ErrorMessage")
        End Sub

        Private Sub ReloadPage()
            Response.Redirect(EditUrl(String.Empty, String.Empty, "ViewContacts"))
        End Sub

#End Region

    End Class

End Namespace