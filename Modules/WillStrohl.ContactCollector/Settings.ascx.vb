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

Imports DotNetNuke.Services.Localization
Imports System.Web.UI.WebControls
Imports WillStrohl.Modules.ContactCollector.ContactCollectorController

Namespace WillStrohl.Modules.ContactCollector

    Partial Public Class Settings
        Inherits Entities.Modules.ModuleSettingsBase

#Region " Private Members "

        Private Const c_DefaultCheck As Boolean = True
        Private p_CheckboxSettingKeys As String()
        Private p_EmailTemplate As EmailTemplateInfo = Nothing

        ''' <summary>
        ''' These are the keys for the Required Field settings that will be used to determine which fields will be required.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks>
        ''' The settings are done this way for programming simplicity. Typically, settings are handled individually.
        ''' </remarks>
        ''' <history>
        ''' </history>
        Private ReadOnly Property CheckboxSettingKeys() As String()
            Get
                If Me.p_CheckboxSettingKeys Is Nothing Then
                    Me.p_CheckboxSettingKeys = New String(3) {}
                    Me.p_CheckboxSettingKeys(0) = Localization.GetString("chkRequiredFields.Item0", Me.LocalResourceFile)
                    Me.p_CheckboxSettingKeys(1) = Localization.GetString("chkRequiredFields.Item1", Me.LocalResourceFile)
                    Me.p_CheckboxSettingKeys(2) = Localization.GetString("chkRequiredFields.Item2", Me.LocalResourceFile)
                End If
                Return Me.p_CheckboxSettingKeys
            End Get
        End Property

        Private ReadOnly Property EmailTemplate() As EmailTemplateInfo
            Get
                If Not Me.p_EmailTemplate Is Nothing Then
                    Return Me.p_EmailTemplate
                End If

                Dim ctlContact As New ContactCollectorController
                Dim objTemplate As New EmailTemplateInfo
                objTemplate = ctlContact.GetEmailTemplate(Me.ModuleId)

                Return objTemplate
            End Get
        End Property

#End Region

        ''' <summary>
        ''' This method centralizes the binding and object assignment for all settings
        ''' </summary>
        ''' <remarks></remarks>
        ''' <history>
        ''' </history>
        Private Sub BindData()

            Try

                If Me.chkRequiredFields.Items.Count = 0 Then
                    With Me.chkRequiredFields.Items
                        .Add(New ListItem(Me.CheckboxSettingKeys(0)))
                        .Add(New ListItem(Me.CheckboxSettingKeys(1)))
                        .Add(New ListItem(Me.CheckboxSettingKeys(2)))
                    End With

                    For Each item As ListItem In Me.chkRequiredFields.Items
                        item.Selected = c_DefaultCheck
                    Next
                End If

                If Not Me.EmailTemplate Is Nothing AndAlso Me.EmailTemplate.EmailId > Null.NullInteger Then
                    Me.txtEmailSubjectToAdmin.Text = Me.EmailTemplate.AdminSubject
                    Me.txtEmailMessageToAdmin.Text = Me.EmailTemplate.AdminTemplate
                    Me.txtEmailSubjectToContact.Text = Me.EmailTemplate.ContactSubject
                    Me.txtEmailMessageToContact.Text = Me.EmailTemplate.ContactTemplate
                Else
                    Me.txtEmailSubjectToAdmin.Text = Localization.GetString("Email.Template.ToAdmin.Subject", Me.LocalResourceFile)
                    Me.txtEmailMessageToAdmin.Text = Localization.GetString("Email.Template.ToAdmin.Body", Me.LocalResourceFile)
                    Me.txtEmailSubjectToContact.Text = Localization.GetString("Email.Template.ToContact.Subject", Me.LocalResourceFile)
                    Me.txtEmailMessageToContact.Text = Localization.GetString("Email.Template.ToContact.Body", Me.LocalResourceFile)
                End If

            Catch ex As Exception
                ProcessModuleLoadException(Me, ex, True)
            End Try
        End Sub

        ''' <summary>
        ''' This is the inherited method that DNN uses to update the settings from the UI
        ''' </summary>
        ''' <remarks></remarks>
        ''' <history>
        ''' </history>
        Public Overrides Sub UpdateSettings()
            Try

                Dim objModules As New DotNetNuke.Entities.Modules.ModuleController
                objModules.UpdateModuleSetting(ModuleId, Me.CheckboxSettingKeys(0), Me.chkRequiredFields.Items(0).Selected.ToString)
                objModules.UpdateModuleSetting(ModuleId, Me.CheckboxSettingKeys(1), Me.chkRequiredFields.Items(1).Selected.ToString)
                objModules.UpdateModuleSetting(ModuleId, Me.CheckboxSettingKeys(2), Me.chkRequiredFields.Items(2).Selected.ToString)

                objModules.UpdateModuleSetting(Me.ModuleId, SETTING_USECAPTCHA, Me.chkUseCaptcha.Checked.ToString)

                objModules.UpdateModuleSetting(Me.ModuleId, SETTING_SENDEMAILTOCONTACT, Me.chkSendEmailToContact.Checked.ToString)
                objModules.UpdateModuleSetting(Me.ModuleId, SETTING_SENDEMAILTOADMIN, Me.chkSendEmailToAdmin.Checked.ToString)

                If Not String.IsNullOrEmpty(Me.txtAdminEmail.Text) Then
                    objModules.UpdateModuleSetting(Me.ModuleId, SETTING_ADMINEMAIL, Me.txtAdminEmail.Text)
                Else
                    Dim ctlUser As New UserController
                    Dim objAdmin As New UserInfo
                    objAdmin = ctlUser.GetUser(Me.PortalId, PortalSettings.AdministratorId)
                    objModules.UpdateModuleSetting(Me.ModuleId, SETTING_ADMINEMAIL, objAdmin.Email)
                End If

                objModules.UpdateModuleSetting(Me.ModuleId, SETTING_INCLUDE_COMMENT, Me.chkComment.Checked.ToString)

                Dim sec As New DotNetNuke.Security.PortalSecurity
                Dim ctlContact As New ContactCollectorController
                If Not Me.EmailTemplate Is Nothing AndAlso Me.EmailTemplate.EmailId > Null.NullInteger Then
                    ctlContact.UpdateEmailTemplate(Me.EmailTemplate.EmailId, Me.EmailTemplate.ModuleId, _
                        sec.InputFilter(Me.txtEmailSubjectToContact.Text, DotNetNuke.Security.PortalSecurity.FilterFlag.NoScripting), _
                        sec.InputFilter(Me.txtEmailMessageToContact.Text, DotNetNuke.Security.PortalSecurity.FilterFlag.NoScripting), _
                        sec.InputFilter(Me.txtEmailSubjectToAdmin.Text, DotNetNuke.Security.PortalSecurity.FilterFlag.NoScripting), _
                        sec.InputFilter(Me.txtEmailMessageToAdmin.Text, DotNetNuke.Security.PortalSecurity.FilterFlag.NoScripting))
                Else
                    ctlContact.UpdateEmailTemplate(Null.NullInteger, Me.ModuleId, _
                        sec.InputFilter(Me.txtEmailSubjectToContact.Text, DotNetNuke.Security.PortalSecurity.FilterFlag.NoScripting), _
                        sec.InputFilter(Me.txtEmailMessageToContact.Text, DotNetNuke.Security.PortalSecurity.FilterFlag.NoScripting), _
                        sec.InputFilter(Me.txtEmailSubjectToAdmin.Text, DotNetNuke.Security.PortalSecurity.FilterFlag.NoScripting), _
                        sec.InputFilter(Me.txtEmailMessageToAdmin.Text, DotNetNuke.Security.PortalSecurity.FilterFlag.NoScripting))
                End If

                Dim modCtl As New Entities.Modules.ModuleController
                Entities.Modules.ModuleController.SynchronizeModule(Me.ModuleId)

            Catch exc As Exception ' Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub

        ''' <summary>
        ''' This is the inherited DNN method that loads saved settings into the settings UI
        ''' </summary>
        ''' <remarks></remarks>
        ''' <history>
        ''' </history>
        Public Overrides Sub LoadSettings()

            Try
                Me.BindData()

                If Not Page.IsPostBack Then

                    Dim strSettingValue As String = Null.NullString

                    strSettingValue = CType(ModuleSettings(Me.CheckboxSettingKeys(0)), String)
                    If Not String.IsNullOrEmpty(strSettingValue) Then
                        Me.chkRequiredFields.Items.FindByText(Me.CheckboxSettingKeys(0)).Selected = Boolean.Parse(strSettingValue)
                    End If

                    strSettingValue = CType(ModuleSettings(Me.CheckboxSettingKeys(1)), String)
                    If Not String.IsNullOrEmpty(strSettingValue) Then
                        Me.chkRequiredFields.Items.FindByText(Me.CheckboxSettingKeys(1)).Selected = Boolean.Parse(strSettingValue)
                    End If

                    strSettingValue = CType(ModuleSettings(Me.CheckboxSettingKeys(2)), String)
                    If Not String.IsNullOrEmpty(strSettingValue) Then
                        Me.chkRequiredFields.Items.FindByText(Me.CheckboxSettingKeys(2)).Selected = Boolean.Parse(strSettingValue)
                    End If

                    strSettingValue = CType(ModuleSettings(SETTING_ADMINEMAIL), String)
                    If Not String.IsNullOrEmpty(strSettingValue) Then
                        Me.txtAdminEmail.Text = strSettingValue
                    Else
                        Dim ctlUser As New UserController
                        Dim objAdmin As New UserInfo
                        objAdmin = ctlUser.GetUser(Me.PortalId, PortalSettings.AdministratorId)
                        Me.txtAdminEmail.Text = objAdmin.Email
                    End If

                    If Not ModuleSettings(SETTING_USECAPTCHA) Is Nothing Then
                        Me.chkUseCaptcha.Checked = Boolean.Parse(ModuleSettings(SETTING_USECAPTCHA).ToString)
                    End If

                    If Not ModuleSettings(SETTING_SENDEMAILTOCONTACT) Is Nothing Then
                        Me.chkSendEmailToContact.Checked = Boolean.Parse(ModuleSettings(SETTING_SENDEMAILTOCONTACT).ToString)
                    End If

                    If Not ModuleSettings(SETTING_SENDEMAILTOADMIN) Is Nothing Then
                        Me.chkSendEmailToAdmin.Checked = Boolean.Parse(ModuleSettings(SETTING_SENDEMAILTOADMIN).ToString)
                    End If

                    If Not ModuleSettings(SETTING_INCLUDE_COMMENT) Is Nothing Then
                        Me.chkComment.Checked = Boolean.Parse(ModuleSettings(SETTING_INCLUDE_COMMENT).ToString)
                    End If

                End If

            Catch exc As Exception ' Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

    End Class

End Namespace