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

Imports System.Text.RegularExpressions
Imports System.Web.UI.WebControls
Imports DotNetNuke
Imports DotNetNuke.Services.Exceptions
Imports DotNetNuke.Security.PortalSecurity
Imports DotNetNuke.UI.Skins.Skin

Namespace WillStrohl.Modules.Injection

    Partial Public MustInherit Class EditInjections
        Inherits WNSPortalModuleBase

#Region " Private Members "

        Private p_Injections As InjectionInfoCollection = Nothing

        Private Const c_Command_Edit As String = "Edit"
        Private Const c_Command_MoveUp As String = "MoveUp"
        Private Const c_Command_MoveDown As String = "MoveDown"
        Private Const c_Command_Delete As String = "Delete"
        Private Const c_Command_Insert As String = "Insert"

        Private Const c_True As String = "True"

        Private p_SearchParam As Integer = Null.NullInteger

        Private p_EnabledImage As String = String.Empty
        Private p_DisabledImage As String = String.Empty
        Private p_EnabledAltText As String = String.Empty
        Private p_DisabledAltText As String = String.Empty

#End Region

#Region " Private Properties "

        Private ReadOnly Property Injections() As InjectionInfoCollection
            Get
                If Me.p_Injections Is Nothing Then
                    Dim ctlModule As New InjectionController
                    Me.p_Injections = ctlModule.GetInjectionContents(Me.ModuleId)
                End If
                Return Me.p_Injections
            End Get
        End Property

        Private ReadOnly Property EnabledImage() As String
            Get
                If Not String.IsNullOrEmpty(p_EnabledImage) Then
                    Return p_EnabledImage
                End If

                p_EnabledImage = String.Concat(Common.Globals.ApplicationPath, Entities.Icons.IconController.IconURL("Checked", "16x16"))

                Return p_EnabledImage
            End Get
        End Property

        Private ReadOnly Property DisabledImage() As String
            Get
                If Not String.IsNullOrEmpty(p_DisabledImage) Then
                    Return p_DisabledImage
                End If

                p_DisabledImage = String.Concat(Common.Globals.ApplicationPath, Entities.Icons.IconController.IconURL("Unchecked", "16x16"))

                Return p_DisabledImage
            End Get
        End Property

        Private ReadOnly Property EnabledAltText() As String
            Get
                If String.IsNullOrEmpty(Me.p_EnabledAltText) Then
                    Me.p_EnabledAltText = Me.GetLocalizedString("Enabled.Alt")
                End If

                Return Me.p_EnabledAltText
            End Get
        End Property

        Private ReadOnly Property DisabledAltText() As String
            Get
                If String.IsNullOrEmpty(Me.p_DisabledAltText) Then
                    Me.p_DisabledAltText = Me.GetLocalizedString("Disabled.Alt")
                End If

                Return Me.p_DisabledAltText
            End Get
        End Property
        
#End Region
        
#Region " Event Handlers "

        Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            Try
                If Not Me.Page.IsPostBack Then
                    Me.pnlAddNew.Visible = False
                    Me.pnlManage.Visible = True

                    Me.BindData()
                End If
            Catch exc As Exception ' Module failed to load
                ProcessModuleLoadException(Me, exc, Me.IsEditable)
            End Try
        End Sub

        Private Sub lnkAddNewInjection_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkAddNewInjection.Click, cmdCancel.Click

            Me.ClearForm()
            Me.TogglePanels()

            Me.cmdDelete.Visible = Not String.IsNullOrEmpty(Me.hidInjectionId.Value)

        End Sub

        Private Sub cmdReturn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdReturn.Click
            Response.Redirect(NavigateURL)
        End Sub

        Private Sub cmdAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdAdd.Click

            If Me.Page.IsValid AndAlso Not (String.IsNullOrEmpty(Me.txtName.Text) Or String.IsNullOrEmpty(Me.txtContent.Text)) Then
                Me.SaveInjection()
                Me.ClearForm()
                Me.TogglePanels()
                Me.BindData()
            End If

        End Sub

        Private Sub cvName_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles cvName.ServerValidate
            Dim ctlModule As New InjectionController
            args.IsValid = (Not ctlModule.DoesInjectionNameExist(Me.txtName.Text, Me.ModuleId))
        End Sub

        Protected Sub dlInjection_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataListCommandEventArgs) Handles dlInjection.ItemCommand
            Select Case e.CommandName
                Case c_Command_MoveUp
                    Me.SwapOrder(Convert.ToInt32(e.CommandArgument), c_Command_MoveUp)
                    Me.BindData()
                Case c_Command_MoveDown
                    Me.SwapOrder(Convert.ToInt32(e.CommandArgument), c_Command_MoveDown)
                    Me.BindData()
                Case c_Command_Edit
                    Me.BindForm(Convert.ToInt32(e.CommandArgument))
                    Me.TogglePanels()
                Case c_Command_Insert
                    Me.ClearForm()
                    Me.TogglePanels()
                Case c_Command_Delete
                    Dim ctlModule As New InjectionController
                    ctlModule.DeleteInjectionContent(Convert.ToInt32(e.CommandArgument))
                    Me.BindData()
                Case Else
                    Exit Sub
            End Select
        End Sub

        Private Sub cmdDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdDelete.Click

            If Regex.IsMatch(Me.hidInjectionId.Value, "^\d+$") Then
                Dim ctlModule As New InjectionController
                ctlModule.DeleteInjectionContent(Integer.Parse(Me.hidInjectionId.Value, Globalization.NumberStyles.Integer))
            End If

            Me.ClearForm()
            Me.TogglePanels()
            Me.BindData()

        End Sub

#End Region

#Region " Private Helper Functions "

        Private Sub BindData()

            Me.LocalizeModule()

            ' bind data to controls

            If Me.Injections.Count > 0 Then
                With Me.dlInjection
                    .DataSource = Injections
                    .DataBind()
                End With
                Me.dlInjection.Visible = True
                Me.lblNoRecords.Visible = False
            Else
                Me.dlInjection.Visible = False
                Me.lblNoRecords.Visible = True
            End If

            Me.txtName.Attributes.Add("onfocus", String.Concat("if (this.value == '", Me.GetLocalizedString("txtName.Text"), "') { this.value = ''; }"))

            If Me.radInject.Items.Count = 0 Then
                Me.radInject.Items.Add(New ListItem(Me.GetLocalizedString("radInject.0.Text")))
                Me.radInject.Items.Add(New ListItem(Me.GetLocalizedString("radInject.1.Text")))
            End If

        End Sub

        Private Sub LocalizeModule()

            Me.txtName.Text = Me.GetLocalizedString("txtName.Text")
            Me.lnkAddNewInjection.Text = Me.GetLocalizedString("lnkAdd.Text")
            Me.cmdAdd.Text = Me.GetLocalizedString("cmdAdd.Text")
            Me.cmdDelete.Text = Me.GetLocalizedString("cmdDelete.Text")
            Me.rfvName.ErrorMessage = Me.GetLocalizedString("rfvName.ErrorMessage")
            Me.rfvName.InitialValue = Me.GetLocalizedString("txtName.Text")
            Me.rfvContent.ErrorMessage = Me.GetLocalizedString("rfvContent.ErrorMessage")
            Me.cmdCancel.Text = Me.GetLocalizedString("cmdCancel.Text")
            Me.cmdReturn.Text = Me.GetLocalizedString("cmdReturn.Text")
            Me.cvName.ErrorMessage = Me.GetLocalizedString("cvName.ErrorMessage")

        End Sub

        Private Sub ClearForm()
            Me.hidInjectionId.Value = String.Empty
            Me.txtName.Text = Me.GetLocalizedString("txtName.Text")
            Me.txtContent.Text = String.Empty
            Me.chkEnabled.Checked = True
            Me.radInject.Items.FindByText(Me.GetLocalizedString("radInject.0.Text")).Selected = True
            Me.cvName.Enabled = True
        End Sub

        Private Sub BindForm(ByVal ItemId As Integer)

            Dim ctlModule As New InjectionController
            Dim objInfo As New InjectionInfo
            objInfo = ctlModule.GetInjectionContent(ItemId)

            Me.txtName.Text = objInfo.InjectName
            Me.txtContent.Text = Server.HtmlDecode(objInfo.InjectContent)
            Me.radInject.ClearSelection()
            If objInfo.InjectTop Then
                Me.radInject.Items.FindByText(Me.GetLocalizedString("radInject.0.Text")).Selected = True
            Else
                Me.radInject.Items.FindByText(Me.GetLocalizedString("radInject.1.Text")).Selected = True
            End If
            Me.chkEnabled.Checked = objInfo.IsEnabled
            Me.hidInjectionId.Value = objInfo.InjectionId.ToString
            Me.cvName.Enabled = False

            Me.cmdDelete.Visible = Not String.IsNullOrEmpty(Me.hidInjectionId.Value)

        End Sub

        Private Sub SwapOrder(ByVal ItemId As Integer, ByVal UpDown As String)
            
            ' set the global id to match the one we're looking for
            Me.p_SearchParam = ItemId

            ' change the order
            Dim ctlModule As New InjectionController
            ctlModule.ChangeOrder(ItemId, UpDown)

        End Sub

        Private Function FindInjectionById(ByVal item As InjectionInfo) As Boolean
            Return item.InjectionId = Me.p_SearchParam
        End Function

        Private Sub TogglePanels()
            Me.pnlAddNew.Visible = (Not Me.pnlAddNew.Visible)
            Me.pnlManage.Visible = (Not Me.pnlManage.Visible)
        End Sub

        Private Sub SaveInjection()
            Try
                Dim sec As New Security.PortalSecurity
                Dim ctlModule As New InjectionController
                Dim objInj As New InjectionInfo

                If Not String.IsNullOrEmpty(Me.hidInjectionId.Value) Then
                    objInj = ctlModule.GetInjectionContent(Integer.Parse(Me.hidInjectionId.Value))
                    With objInj
                        .InjectContent = Server.HtmlEncode(Me.txtContent.Text)
                        .InjectName = sec.InputFilter(Me.txtName.Text, FilterFlag.NoMarkup)
                        .InjectTop = Me.radInject.Items.FindByText(Me.GetLocalizedString("radInject.0.Text")).Selected
                        .IsEnabled = Me.chkEnabled.Checked
                        .ModuleId = Me.ModuleId
                    End With
                    ctlModule.UpdateInjectionContent(objInj)
                Else
                    With objInj
                        .InjectContent = Server.HtmlEncode(Me.txtContent.Text)
                        .InjectName = sec.InputFilter(Me.txtName.Text, FilterFlag.NoMarkup)
                        .InjectTop = Me.radInject.Items.FindByText(Me.GetLocalizedString("radInject.0.Text")).Selected
                        .IsEnabled = Me.chkEnabled.Checked
                        .ModuleId = Me.ModuleId
                        .OrderShown = ctlModule.GetNextOrderNumber(Me.ModuleId)
                    End With
                    ctlModule.AddInjectionContent(objInj)
                End If
            Catch ex As Exception
                HandleException(ex)
            End Try
        End Sub

        Public Function CommandUpVisible(ByVal InjectionId As String) As String

            Dim ctlModule As New InjectionController
            Dim oInject As InjectionInfo = ctlModule.GetInjectionContent(Integer.Parse(InjectionId, Globalization.NumberStyles.Integer))
            
            Return (Not oInject.OrderShown = 1).ToString

        End Function

        Public Function CommandDownVisible(ByVal InjectionId As String) As String

            Dim ctlModule As New InjectionController
            Dim oInject As InjectionInfo = ctlModule.GetInjectionContent(Integer.Parse(InjectionId, Globalization.NumberStyles.Integer))
            Dim collInject As InjectionInfoCollection = ctlModule.GetInjectionContents(Me.ModuleId)

            Return (Not oInject.OrderShown = collInject.Count).ToString

        End Function

        Public Function GetEnabledImage(ByVal EnabledText As String) As String
            If String.Equals(EnabledText, c_True) Then
                Return EnabledImage
            Else
                Return DisabledImage
            End If
        End Function

        Public Function GetEnabledImageAltText(ByVal EnabledText As String) As String
            If String.Equals(EnabledText, c_True) Then
                Return EnabledAltText
            Else
                Return DisabledAltText
            End If
        End Function

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