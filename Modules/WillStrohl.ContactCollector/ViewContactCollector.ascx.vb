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

Imports DotNetNuke
Imports DotNetNuke.Security
Imports DotNetNuke.UI.Skins.Skin
Imports System.Text.RegularExpressions
Imports WillStrohl.Modules.ContactCollector.ContactCollectorController

Namespace WillStrohl.Modules.ContactCollector

    ''' <summary>
    ''' This is the view control for the CookieChecker module.
    ''' </summary>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' </history>
    Partial Class ViewContactCollector
        Inherits WNSPortalModuleBase
        Implements Entities.Modules.IActionable

#Region " Private Members "

        Private p_ContactId As Integer = Null.NullInteger
        Private Const c_DefaultRequired As Boolean = True
        Private p_SettingKeys As String()
        Private p_SettingValues As Boolean()
        Private p_AdminEmail As String = Null.NullString

#End Region

#Region " Private Properties "

        ''' <summary>
        ''' The ContactID is grabbed from the URL to be used in the work flow in this module control
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks>
        ''' This property is not yet used in this module control
        ''' </remarks>
        ''' <history>
        ''' </history>
        Private ReadOnly Property ContactId() As Integer
            Get
                If Me.p_ContactId = Null.NullInteger Then
                    Dim obj As Object = Request.Params("contactid")
                    If Not obj Is Nothing Then
                        If Regex.IsMatch(obj.ToString, "\d+") Then
                            Me.p_ContactId = Integer.Parse(obj.ToString, Globalization.NumberStyles.Integer)
                        End If
                    End If
                End If

                Return Me.p_ContactId
            End Get
        End Property

        ''' <summary>
        ''' These are the Required Field settings used for the UI, to toggle the required field controls from being enabled
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
                If Me.p_SettingKeys Is Nothing Then
                    Me.p_SettingKeys = New String(3) {}
                    Me.p_SettingKeys(0) = Localization.GetString("RequiredFields.Item0", Me.LocalResourceFile)
                    Me.p_SettingKeys(1) = Localization.GetString("RequiredFields.Item1", Me.LocalResourceFile)
                    Me.p_SettingKeys(2) = Localization.GetString("RequiredFields.Item2", Me.LocalResourceFile)
                End If
                Return Me.p_SettingKeys
            End Get
        End Property

        ''' <summary>
        ''' This object returns the Required Field setting values, used to assign the values to the UI elements
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks>
        ''' The settings are done this way for programming simplicity. Typically, settings are handled individually.
        ''' </remarks>
        ''' <history>
        ''' </history>
        Private ReadOnly Property SettingValues() As Boolean()
            Get
                If Me.p_SettingValues Is Nothing Then
                    Dim obj As Object = Nothing
                    Me.p_SettingValues = New Boolean(3) {}

                    obj = Me.Settings(Me.GetLocalizedString("RequiredFields.Item0"))
                    If obj Is Nothing Then
                        Me.p_SettingValues(0) = c_DefaultRequired
                    Else
                        Me.p_SettingValues(0) = Boolean.Parse(obj.ToString)
                    End If

                    obj = Me.Settings(Me.GetLocalizedString("RequiredFields.Item1"))
                    If obj Is Nothing Then
                        Me.p_SettingValues(1) = c_DefaultRequired
                    Else
                        Me.p_SettingValues(1) = Boolean.Parse(obj.ToString)
                    End If

                    obj = Me.Settings(Me.GetLocalizedString("RequiredFields.Item2"))
                    If obj Is Nothing Then
                        Me.p_SettingValues(2) = c_DefaultRequired
                    Else
                        Me.p_SettingValues(2) = Boolean.Parse(obj.ToString)
                    End If
                End If

                Return Me.p_SettingValues
            End Get
        End Property

        ''' <summary>
        ''' UseCaptcha - this property reads the module settings, and dictates whether we use the CAPTCHA control.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        ''' <history>
        ''' </history>
        Private ReadOnly Property UseCaptcha() As Boolean
            Get
                If Not Me.Settings(SETTING_USECAPTCHA) Is Nothing Then
                    Return Boolean.Parse(Me.Settings(SETTING_USECAPTCHA).ToString)
                Else
                    Return False
                End If
            End Get
        End Property

        ''' <summary>
        ''' NotifyContact - this property tells us if the contact is supposed to be notified
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        ''' <history>
        ''' </history>
        Private ReadOnly Property NotifyContact() As Boolean
            Get
                If Not Me.Settings(SETTING_SENDEMAILTOCONTACT) Is Nothing Then
                    Return Boolean.Parse(Me.Settings(SETTING_SENDEMAILTOCONTACT).ToString)
                Else
                    Return False
                End If
            End Get
        End Property

        ''' <summary>
        ''' NotifyAdmin - this property tells us if the admin is supposed to be notified
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        ''' <history>
        ''' </history>
        Private ReadOnly Property NotifyAdmin() As Boolean
            Get
                If Not Me.Settings(SETTING_SENDEMAILTOADMIN) Is Nothing Then
                    Return Boolean.Parse(Me.Settings(SETTING_SENDEMAILTOADMIN).ToString)
                Else
                    Return False
                End If
            End Get
        End Property

        ''' <summary>
        ''' AdminEmail - this property tells us the email address that the contact email is supposed to be from
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        ''' <history>
        ''' </history>
        Private ReadOnly Property AdminEmail() As String
            Get
                If Not String.IsNullOrEmpty(Me.p_AdminEmail) Then
                    Return Me.p_AdminEmail
                End If

                If Not Me.Settings(SETTING_ADMINEMAIL) Is Nothing Then
                    Me.p_AdminEmail = Me.Settings(SETTING_ADMINEMAIL).ToString
                Else
                    Dim ctlUser As New UserController
                    Dim usrAdmin As New UserInfo
                    usrAdmin = ctlUser.GetUser(Me.PortalId, Me.PortalSettings.AdministratorId)
                    Me.p_AdminEmail = usrAdmin.Email
                End If

                Return Me.p_AdminEmail
            End Get
        End Property

        ''' <summary>
        ''' IncludeComment - this property reads the module settings, and dictates is visitors can include comments
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        ''' <history>
        ''' </history>
        Private ReadOnly Property IncludeComment() As Boolean
            Get
                If Not Me.Settings(SETTING_INCLUDE_COMMENT) Is Nothing Then
                    Return Boolean.Parse(Me.Settings(SETTING_INCLUDE_COMMENT).ToString)
                Else
                    Return False
                End If
            End Get
        End Property

#End Region

#Region " Event Handlers "

        ''' <summary>
        ''' Page_Load runs when the control is loaded
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' </history>
        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            Try
                If Not Page.IsPostBack Then
                    Me.BindData()
                End If

                Me.LocalizeModule()
            Catch exc As Exception ' Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub

        Private Sub cmdSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSubmit.Click
            
            If Page.IsValid And ((Me.UseCaptcha And Me.ctlCaptcha.IsValid) Or Not Me.UseCaptcha) Then

                Dim objContact As New ContactInfo

                objContact = Me.SaveContact()

                Dim ctlContact As New ContactCollectorController
                Dim objEmail As New EmailTemplateInfo
                objEmail = ctlContact.GetEmailTemplate(Me.ModuleId)

                Me.SendMailToContact(Me.AdminEmail, objContact, objEmail)
                Me.SendMailToAdmin(Me.AdminEmail, Me.AdminEmail, objContact, objEmail)

            End If

        End Sub

#End Region

#Region " Private Methods "

        ''' <summary>
        ''' BindData - this method performs the binding steps needed for the module
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' </history>
        Private Sub BindData()

            Me.rfvFirstName.Enabled = Me.SettingValues(0)
            If Me.rfvFirstName.Enabled Then
                Me.txtFirstName.CssClass = "NormalTextBox dnnFormRequired"
            Else
                Me.txtFirstName.CssClass = "NormalTextBox"
            End If

            Me.rfvLastName.Enabled = Me.SettingValues(1)
            If Me.rfvLastName.Enabled Then
                Me.txtLastName.CssClass = "NormalTextBox dnnFormRequired"
            Else
                Me.txtLastName.CssClass = "NormalTextBox"
            End If

            Me.rfvEmailAddress.Enabled = Me.SettingValues(2)
            If Me.rfvEmailAddress.Enabled Then
                Me.txtEmailAddress.CssClass = "NormalTextBox dnnFormRequired"
            Else
                Me.txtEmailAddress.CssClass = "NormalTextBox"
            End If

            Me.ctlCaptcha.Visible = Me.UseCaptcha
            Me.ctlCaptcha.Enabled = Me.UseCaptcha

            Me.lblComment.Visible = Me.IncludeComment
            Me.txtComment.Visible = Me.IncludeComment
            Me.rfvComment.Visible = Me.IncludeComment
            Me.rfvComment.Enabled = Me.IncludeComment

            If Me.ContactId > Null.NullInteger Then
                ' the module control is updating an existing contact record
                Dim ctlContact As New ContactCollectorController
                Dim cInfo As New ContactInfo
                cInfo = ctlContact.GetContact(ContactId)
                Me.txtFirstName.Text = cInfo.FirstName
                Me.txtLastName.Text = cInfo.LastName
                Me.txtEmailAddress.Text = cInfo.EmailAddress
                Me.txtComment.Text = cInfo.Comment
            ElseIf Me.UserId > Null.NullInteger Then
                ' prefill the contact fields for the visitor
                Me.txtEmailAddress.Text = Me.UserInfo.Email
                Me.txtFirstName.Text = Me.UserInfo.FirstName
                Me.txtLastName.Text = Me.UserInfo.LastName
            End If

        End Sub

        Private Sub LocalizeModule()
            Me.rfvFirstName.ErrorMessage = Me.GetLocalizedString("rfvFirstName.ErrorMessage")
            Me.rfvLastName.ErrorMessage = Me.GetLocalizedString("rfvLastName.ErrorMessage")
            Me.rfvEmailAddress.ErrorMessage = Me.GetLocalizedString("rfvEmailAddress.ErrorMessage")
            Me.revEmailAddress.ErrorMessage = Me.GetLocalizedString("revEmailAddress.ErrorMessage")
            Me.rfvComment.ErrorMessage = Me.GetLocalizedString("rfvComment.ErrorMessage")
            Me.cmdSubmit.Text = Me.GetLocalizedString("cmdSubmit.Text")
            Me.ctlCaptcha.ErrorMessage = Me.GetLocalizedString("ctlCaptcha.ErrorMessage")
        End Sub

        Private Sub ClearForm()
            Me.txtFirstName.Text = Null.NullString
            Me.txtLastName.Text = Null.NullString
            Me.txtEmailAddress.Text = Null.NullString
            Me.txtComment.Text = Null.NullString
        End Sub

        Private Function SaveContact() As ContactInfo

            Try
                Dim intContact As Integer = Null.NullInteger
                Dim sec As New PortalSecurity
                Dim ctlContact As New ContactCollectorController

                Dim strFirstName As String = sec.InputFilter(Me.txtFirstName.Text.Trim, PortalSecurity.FilterFlag.NoMarkup)
                Dim strLastName As String = sec.InputFilter(Me.txtLastName.Text.Trim, PortalSecurity.FilterFlag.NoMarkup)
                Dim strEmailAddress As String = sec.InputFilter(Me.txtEmailAddress.Text.Trim, PortalSecurity.FilterFlag.NoMarkup)
                Dim strComment As String = sec.InputFilter(Me.txtComment.Text.Trim, PortalSecurity.FilterFlag.NoMarkup)

                If Me.ContactId > Null.NullInteger Then
                    ' update
                    ctlContact.UpdateContact(Me.ContactId, Me.ModuleId, strFirstName, strLastName, strEmailAddress, True, strComment)
                    intContact = Me.ContactId
                Else
                    ' insert
                    intContact = ctlContact.AddContact(Me.ModuleId, strFirstName, strLastName, strEmailAddress, True, strComment)
                End If

                AddModuleMessage(Me, _
                    Me.GetLocalizedString("Message.Success"), _
                    Skins.Controls.ModuleMessage.ModuleMessageType.GreenSuccess)

                Me.ClearForm()

                Dim contact As New ContactInfo
                contact = ctlContact.GetContact(intContact)

                Return contact

            Catch ex As Exception
                DotNetNuke.Services.Exceptions.LogException(ex)

                If Me.IsEditable AndAlso PortalSettings.UserMode = DotNetNuke.Entities.Portals.PortalSettings.Mode.Edit Then
                    AddModuleMessage(Me, _
                        String.Concat(ex.Message, "<br />", ex.StackTrace), _
                        Skins.Controls.ModuleMessage.ModuleMessageType.GreenSuccess)
                Else
                    AddModuleMessage(Me, _
                        Me.GetLocalizedString("Message.Failure"), _
                        Skins.Controls.ModuleMessage.ModuleMessageType.RedError)
                End If

                Return Nothing
            End Try

        End Function

        Private Sub SendMailToContact(ByVal FromEmail As String, ByVal Contact As ContactInfo, ByVal EmailInfo As EmailTemplateInfo)

            If Me.NotifyContact AndAlso Not String.IsNullOrEmpty(Contact.EmailAddress) Then
                Dim ctlContact As New ContactCollectorController
                Dim strBody As String = Me.FormatEmailBody(EmailInfo.ContactTemplate, Contact)
                ctlContact.SendMail(FromEmail, Contact.EmailAddress, EmailInfo.ContactSubject, strBody)
            End If

        End Sub

        Private Sub SendMailToAdmin(ByVal FromEmail As String, ByVal ToEmail As String, ByVal Contact As ContactInfo, ByVal EmailInfo As EmailTemplateInfo)

            If Me.NotifyAdmin Then
                Dim ctlContact As New ContactCollectorController
                Dim strBody As String = Me.FormatEmailBody(EmailInfo.AdminTemplate, Contact)
                ctlContact.SendMail(FromEmail, ToEmail, EmailInfo.AdminSubject, strBody)
            End If

        End Sub

        Private Function FormatEmailBody(ByVal Body As String, ByVal Contact As ContactInfo) As String

            Dim strReturn As String = Regex.Replace(Body, "\[CONTACT:FIRSTNAME\]", Contact.FirstName, RegexOptions.IgnoreCase)
            strReturn = Regex.Replace(strReturn, "\[CONTACT:LASTNAME\]", Contact.LastName, RegexOptions.IgnoreCase)
            strReturn = Regex.Replace(strReturn, "\[CONTACT:EMAIL\]", Contact.EmailAddress, RegexOptions.IgnoreCase)
            strReturn = Regex.Replace(strReturn, "\[PORTAL:PORTALNAME\]", Me.PortalSettings.PortalName, RegexOptions.IgnoreCase)

            Return Server.HtmlDecode(strReturn)

        End Function

#End Region

#Region " IActionable Implementation "

        Public ReadOnly Property ModuleActions() As DotNetNuke.Entities.Modules.Actions.ModuleActionCollection Implements DotNetNuke.Entities.Modules.IActionable.ModuleActions
            Get
                Dim Actions As New Entities.Modules.Actions.ModuleActionCollection
                Actions.Add(GetNextActionID, Me.GetLocalizedString("ViewContacts.MenuItem.Title"), _
                    String.Empty, String.Empty, String.Empty, _
                    Me.EditUrl("ViewContacts"), _
                    False, DotNetNuke.Security.SecurityAccessLevel.Edit, True, False)
                Return Actions
            End Get
        End Property

#End Region

    End Class

End Namespace
