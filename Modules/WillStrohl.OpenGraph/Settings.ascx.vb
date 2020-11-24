'
' Upendo Ventures, LLC (solutions@upendoventures.com)
' https://upendoventures.com
'
'Copyright (c) Upendo Ventures, LLC 
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
'Neither the name of Will Strohl, Open Graph for DotNetNuke, nor the names of its contributors may be 
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

Imports DotNetNuke.Security
Imports System.IO
Imports System.Text.RegularExpressions
Imports System.Web.UI.WebControls
Imports System.Xml.Linq
Imports System.Linq
Imports WillStrohl.Modules.OpenGraph.OpenGraphController

Namespace WillStrohl.Modules.OpenGraph

    Partial Public Class Settings
        Inherits WnsPortalModuleBase

#Region " Private Members "

        Private Const ENABLED_TEXTBOX_CSS_SELECTOR As String = "NormalTextBox dnnFormRequired"
        Private Const DISABLED_TEXTBOX_CSS_SELECTOR As String = "NormalTextBox dnnFormRequired NormalDisabled aspNetDisabled"
        'Private Const ENABLED_CHECKBOX_CSS_SELECTOR As String = "wnsFormCheckbox"
        Private Const DISABLED_CHECKBOX_CSS_SELECTOR As String = "wnsFormCheckboxDisabled NormalDisabled aspNetDisabled"
        Private Const SITE_TYPE_TEXT_PATTERN As String = "cboSiteType.Item."
        Private Const DATE_FORMAT As String = "MM/dd/yyyy hh:mm:ss tt"

        Public Const MERGE_CONFIGURATION_PATH As String = "/DesktopModules/WillStrohl.OpenGraph/Config/HandlerMerge.xml.resources"

#End Region

#Region " Private Properties "

        Private ReadOnly Property IsOpenGraphProtocolEnabled As Boolean
            Get

                Dim ctlOpenGraph As New OpenGraphController
                Return ctlOpenGraph.DoesHttpModuleExist()

            End Get
        End Property

#End Region

#Region " Event Handlers "

        Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            Try
                If Not Page.IsPostBack Then
                    BindData()
                End If
            Catch exc As Exception ' Module failed to load
                ProcessModuleLoadException(Me, exc, Me.IsEditable)
            End Try
        End Sub

        Private Sub lnkSubmit_Click(sender As Object, e As System.EventArgs) Handles lnkSubmit.Click
            If Me.Page.IsValid Then
                Me.UpdateSettings()
            End If

            Me.SendBackToModule()
        End Sub

        Private Sub lnkCancel_Click(sender As Object, e As System.EventArgs) Handles lnkCancel.Click
            Me.SendBackToModule()
        End Sub

        Private Sub chkTitle_CheckedChanged(sender As Object, e As System.EventArgs) Handles chkTitle.CheckedChanged

            If chkTitle.Checked Then
                txtTitle.Text = PortalSettings.ActiveTab.Title
                txtTitle.Enabled = False
                txtTitle.CssClass = DISABLED_TEXTBOX_CSS_SELECTOR
            Else
                txtTitle.Enabled = True
                txtTitle.CssClass = ENABLED_TEXTBOX_CSS_SELECTOR
            End If

        End Sub

        Private Sub chkDescription_CheckedChanged(sender As Object, e As System.EventArgs) Handles chkDescription.CheckedChanged

            If chkDescription.Checked Then
                txtDescription.Text = PortalSettings.ActiveTab.Description
                txtDescription.Enabled = False
                txtDescription.CssClass = DISABLED_TEXTBOX_CSS_SELECTOR
            Else
                txtDescription.Enabled = True
                txtDescription.CssClass = ENABLED_TEXTBOX_CSS_SELECTOR
            End If

        End Sub

        Private Sub chkUrl_CheckedChanged(sender As Object, e As System.EventArgs) Handles chkUrl.CheckedChanged

            If chkUrl.Checked Then
                txtUrl.Text = PortalSettings.ActiveTab.FullUrl
                txtUrl.Enabled = False
                txtUrl.CssClass = DISABLED_TEXTBOX_CSS_SELECTOR
            Else
                txtUrl.Enabled = True
                txtUrl.CssClass = ENABLED_TEXTBOX_CSS_SELECTOR
            End If

        End Sub

        Private Sub chkSiteName_CheckedChanged(sender As Object, e As System.EventArgs) Handles chkSiteName.CheckedChanged

            If chkSiteName.Checked Then
                txtSiteName.Text = PortalSettings.PortalName
                txtSiteName.Enabled = False
                txtSiteName.CssClass = DISABLED_TEXTBOX_CSS_SELECTOR
            Else
                txtSiteName.Enabled = True
                txtSiteName.CssClass = ENABLED_TEXTBOX_CSS_SELECTOR
            End If

        End Sub

        Private Sub lnkDisable_Click(sender As Object, e As System.EventArgs) Handles lnkDisable.Click

            CoordinateMergeAction()
            SendBackToModule()

        End Sub

        Private Sub lnkClearSettings_Click(sender As Object, e As EventArgs) Handles lnkClearSettings.Click
            ClearOgpHostSettingsCache()
            ClearOgpPageMarkUpCache()
            ClearOgpPageSettingsCache()
            OpenGraphController.NukeSettings(PortalId, ModuleId, TabModuleId)
            SendBackToModule()
        End Sub

#End Region

#Region " Settings "

        ''' <summary>
        ''' This is the inherited method that DNN uses to update the settings from the UI
        ''' </summary>
        ''' <remarks></remarks>
        ''' <history>
        ''' </history>
        Private Sub UpdateSettings()
            Try

                ' save settings from the UI to DNN settings
                SaveUISettings()

                ' save xml-based settings to file for OGP handler
                SaveConfigurationToFile() ' host-level settings
                SaveSettingsToFile() ' page-level settings

                ' check to see if the httpmodule needs to be enabled
                MergeHandler()

                ' clear the OGP cache
                ClearOgpPageMarkUpCache()
                ClearOgpPageSettingsCache()
                ClearOgpHostSettingsCache()

                ' save a global setting to allow the module to know 
                ' if this setting has been set in the past
                Entities.Controllers.HostController.Instance().Update(IS_FIRST_TIME_SETTING, "False")

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
        Private Sub LoadSettings()

            Try

                '******************************************************************************'
                '
                ' Page-Level Settings
                '
                '******************************************************************************'

                If Not String.IsNullOrEmpty(Title) Then
                    txtTitle.Text = Title
                Else
                    If Not String.IsNullOrEmpty(PortalSettings.ActiveTab.Title) Then
                        txtTitle.Text = PortalSettings.ActiveTab.Title
                    End If
                End If
                chkTitle.Checked = (String.Equals(txtTitle.Text, PortalSettings.ActiveTab.Title) AndAlso Not String.IsNullOrEmpty(PortalSettings.ActiveTab.Title))

                If Not String.IsNullOrEmpty(Description) Then
                    txtDescription.Text = Description
                Else
                    txtDescription.Text = PortalSettings.ActiveTab.Description
                End If
                chkDescription.Checked = (String.Equals(txtDescription.Text, PortalSettings.ActiveTab.Description) AndAlso Not String.IsNullOrEmpty(txtDescription.Text))

                If String.IsNullOrEmpty(Url) Then
                    txtUrl.Text = PortalSettings.ActiveTab.FullUrl
                Else
                    txtUrl.Text = Url
                End If
                chkUrl.Checked = String.Equals(txtUrl.Text, PortalSettings.ActiveTab.FullUrl)

                ctlImage.Url = Image

                If Not String.IsNullOrEmpty(SiteLocale) Then
                    cboLocale.Items.FindByValue(SiteLocale).Selected = True
                End If

                '******************************************************************************'
                '
                ' Site-Level Settings
                '
                '******************************************************************************'

                If String.IsNullOrEmpty(SiteName) Then
                    txtSiteName.Text = PortalSettings.PortalName
                Else
                    txtSiteName.Text = SiteName
                End If
                chkSiteName.Checked = String.Equals(txtSiteName.Text, PortalSettings.PortalName)

                If Not String.IsNullOrEmpty(SiteType) Then
                    cboType.Items.FindByValue(SiteType).Selected = True
                End If

                txtFacebookAdmins.Text = FacebookAdmins
                txtFacebookAppId.Text = FacebookAppId

                chkDebug.Checked = IsDebugEnabled

            Catch exc As Exception ' Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub SaveConfigurationToFile()

            Dim strSavePath As String = String.Format("{0}config/ogConfig.xml.resources", ControlPath)
            Dim strServerPath As String = Server.MapPath(strSavePath)

            If Not File.Exists(strServerPath) Then

                ' create a new configuration document
                Dim xConfig As XDocument =
                    <?xml version="1.0"?>
                    <ogConfig version=<%= ModuleConfiguration.DesktopModule.Version %>>
                        <Sites>
                            <Site portalId=<%= PortalId %> httpAlias=<%= PortalSettings.PortalAlias.HTTPAlias %>
                                lastModified=<%= DateTime.Now.ToString(DATE_FORMAT) %>
                                lastModifiedUtc=<%= DateTime.Now.ToUniversalTime() %>
                                lastModifiedBy=<%= UserId %>
                                lastModifiedUser=<%= UserInfo.Username %>
                                homeUrl=<%= NavigateURL(PortalSettings.HomeTabId) %>
                                debug=<%= chkDebug.Checked.ToString().ToLower() %>/>
                        </Sites>
                    </ogConfig>

                xConfig.Save(strServerPath)

            Else

                Dim xDoc As XDocument

                Using sReader As StreamReader = File.OpenText(strServerPath)

                    xDoc = XDocument.Load(sReader)

                End Using

                If Not xDoc.Element("ogConfig").Attribute("version") Is Nothing Then
                    xDoc.Element("ogConfig").SetAttributeValue("version", ModuleConfiguration.DesktopModule.Version)
                Else
                    Dim attr As New XAttribute("version", ModuleConfiguration.DesktopModule.Version)
                    xDoc.Element("ogConfig").Add(attr)
                End If

                Dim oElement As XElement = (From xconfig In xDoc.Descendants("Site")
                        Where xconfig.Attribute("portalId").Value = PortalId.ToString()
                        Select xconfig).FirstOrDefault()

                If Not oElement Is Nothing Then
                    ' update existing config
                    oElement.Remove()
                End If

                ' add new config node
                Dim newElement As New XElement("Site")
                newElement.SetAttributeValue("portalId", PortalId.ToString())
                newElement.SetAttributeValue("httpAlias", PortalSettings.PortalAlias.HTTPAlias)
                newElement.SetAttributeValue("lastModified", DateTime.Now.ToString(DATE_FORMAT))
                newElement.SetAttributeValue("lastModifiedUtc", DateTime.Now.ToUniversalTime().ToString())
                newElement.SetAttributeValue("lastModifiedBy", UserId.ToString())
                newElement.SetAttributeValue("lastModifiedUser", UserInfo.Username)
                newElement.SetAttributeValue("homeUrl", NavigateURL(PortalSettings.HomeTabId))
                newElement.SetAttributeValue("debug", chkDebug.Checked.ToString().ToLower())

                Dim refElement As XElement = (From xconfig In xDoc.Descendants("Sites")
                    Select xconfig).FirstOrDefault

                refElement.Add(newElement)

                xDoc.Save(strServerPath)

                End If

        End Sub

        Private Sub SaveSettingsToFile()

            Dim xSettings As XDocument =
                <?xml version="1.0"?>
                <ogSettings>
                    <SiteName><%= SiteName %></SiteName>
                    <UsePortalTitle><%= UsePortalTitle %></UsePortalTitle>
                    <Title><%= Title %></Title>
                    <UseTabTitle><%= UseTabTitle %></UseTabTitle>
                    <Description><%= Description %></Description>
                    <UseTabDescription><%= UseTabDescription %></UseTabDescription>
                    <Locale><%= SiteLocale %></Locale>
                    <SiteType><%= SiteType %></SiteType>
                    <Url><%= Url %></Url>
                    <UseTabUrl><%= UseTabUrl %></UseTabUrl>
                    <Image><%= Image %></Image>
                    <FacebookAdmins><%= FacebookAdmins %></FacebookAdmins>
                    <FacebookAppId><%= FacebookAppId %></FacebookAppId>
                    <SiteLocale><%= SiteLocale %></SiteLocale>
                    <PortalId><%= PortalId %></PortalId>
                    <ModuleId><%= ModuleId %></ModuleId>
                    <TabModuleId><%= TabModuleId %></TabModuleId>
                    <TabId><%= TabId %></TabId>
                    <HttpAlias><%= PortalSettings.PortalAlias.HTTPAlias %></HttpAlias>
                    <LastModified><%= DateTime.Now.ToString(DATE_FORMAT) %></LastModified>
                    <LastModifiedUtc><%= DateTime.Now.ToUniversalTime() %></LastModifiedUtc>
                    <LastModifiedBy><%= UserId %></LastModifiedBy>
                    <LastModifiedUser><%= UserInfo.Username %></LastModifiedUser>
                </ogSettings>

            Dim strSavePath As String = String.Format("{0}config/ogSettings-{1}-{2}.xml.resources", ControlPath, PortalId, TabId)
            Dim strServerPath As String = Server.MapPath(strSavePath)

            If File.Exists(strServerPath) Then
                File.Delete(strServerPath)
            End If

            xSettings.Save(strServerPath)
            
        End Sub

        Private Sub SaveUISettings()

            Dim ctlModule As New Entities.Modules.ModuleController
            Dim sec As New DotNetNuke.Security.PortalSecurity

            '******************************************************************************'
            '
            ' Page-Level Settings
            '
            '******************************************************************************'

            If Not String.IsNullOrEmpty(txtTitle.Text) Then
                ctlModule.UpdateTabModuleSetting(TabModuleId, OG_TITLE_SETTING, sec.InputFilter(txtTitle.Text, PortalSecurity.FilterFlag.NoMarkup))
            End If

            ctlModule.UpdateTabModuleSetting(TabModuleId, USE_TABTITLE_SETTING, chkTitle.Checked.ToString())

            If Not String.IsNullOrEmpty(txtDescription.Text) Then
                ctlModule.UpdateTabModuleSetting(TabModuleId, OG_DESCRIPTION_SETTING, sec.InputFilter(txtDescription.Text, PortalSecurity.FilterFlag.NoMarkup))
            End If

            ctlModule.UpdateTabModuleSetting(TabModuleId, USE_TABDESCRIPTION_SETTING, chkDescription.Checked.ToString())

            If Not String.IsNullOrEmpty(txtUrl.Text) Then
                ctlModule.UpdateTabModuleSetting(TabModuleId, OG_URL_SETTING, sec.InputFilter(txtUrl.Text, PortalSecurity.FilterFlag.NoMarkup))
            End If

            ctlModule.UpdateTabModuleSetting(TabModuleId, USE_TABURL_SETTING, chkUrl.Checked.ToString())

            If Not String.IsNullOrEmpty(ctlImage.Url) Then
                ctlModule.UpdateTabModuleSetting(TabModuleId, OG_IMAGE_SETTING, sec.InputFilter(ctlImage.Url, PortalSecurity.FilterFlag.NoMarkup))
            End If

            If Not String.IsNullOrEmpty(cboLocale.SelectedValue) Then
                ctlModule.UpdateTabModuleSetting(TabModuleId, OG_LOCALE_SETTING, sec.InputFilter(cboLocale.SelectedValue, PortalSecurity.FilterFlag.NoMarkup))
            End If

            '******************************************************************************'
            '
            ' Site-Level Settings
            '
            '******************************************************************************'

            If Not String.IsNullOrEmpty(txtSiteName.Text) Then
                ' *** IMPORTANT! ***
                '
                ' Remove this after 2-3 releases
                '
                ' *** .......... ***
                If Not Settings(OG_SITENAME_SETTING) Is Nothing Then
                    ctlModule.DeleteModuleSetting(ModuleId, OG_SITENAME_SETTING)
                End If

                Entities.Portals.PortalController.UpdatePortalSetting(PortalId, OG_SITENAME_SETTING, sec.InputFilter(txtSiteName.Text, PortalSecurity.FilterFlag.NoMarkup), True, cboLocale.SelectedItem.Text.ToLower())
            End If

            ' *** IMPORTANT! ***
            '
            ' Remove this after 2-3 releases
            '
            ' *** .......... ***
            If Not Settings(USE_PORTALTITLE_SETTING) Is Nothing Then
                ctlModule.DeleteModuleSetting(ModuleId, USE_PORTALTITLE_SETTING)
            End If

            Entities.Portals.PortalController.UpdatePortalSetting(PortalId, USE_PORTALTITLE_SETTING, chkSiteName.Checked.ToString(), True, cboLocale.SelectedItem.Text.ToLower())

            If Not String.IsNullOrEmpty(cboType.SelectedValue) Then
                ' *** IMPORTANT! ***
                '
                ' Remove this after 2-3 releases
                '
                ' *** .......... ***
                If Not Settings(OG_SITETYPE_SETTING) Is Nothing Then
                    ctlModule.DeleteModuleSetting(ModuleId, OG_SITETYPE_SETTING)
                End If

                Entities.Portals.PortalController.UpdatePortalSetting(PortalId, OG_SITETYPE_SETTING, sec.InputFilter(cboType.SelectedValue, PortalSecurity.FilterFlag.NoMarkup), True, cboLocale.SelectedItem.Text.ToLower())
            End If

            If Not String.IsNullOrEmpty(txtFacebookAdmins.Text) Then
                ' *** IMPORTANT! ***
                '
                ' Remove this after 2-3 releases
                '
                ' *** .......... ***
                If Not Settings(OG_FACEBOOKADMINS_SETTING) Is Nothing Then
                    ctlModule.DeleteModuleSetting(ModuleId, OG_FACEBOOKADMINS_SETTING)
                End If
                Entities.Portals.PortalController.UpdatePortalSetting(PortalId, OG_FACEBOOKADMINS_SETTING, sec.InputFilter(txtFacebookAdmins.Text, PortalSecurity.FilterFlag.NoMarkup), True, cboLocale.SelectedItem.Text.ToLower())

            Else

                If Not String.IsNullOrEmpty(Entities.Portals.PortalController.GetPortalSetting(OG_FACEBOOKADMINS_SETTING, PortalId, String.Empty)) Then
                    Entities.Portals.PortalController.DeletePortalSetting(PortalId, OG_FACEBOOKADMINS_SETTING, cboLocale.SelectedItem.Text.ToLower())
                End If

            End If

            If Not String.IsNullOrEmpty(txtFacebookAppId.Text) Then
                ' *** IMPORTANT! ***
                '
                ' Remove this after 2-3 releases
                '
                ' *** .......... ***
                If Not Settings(OG_FACEBOOKAPPID_SETTING) Is Nothing Then
                    ctlModule.DeleteModuleSetting(ModuleId, OG_FACEBOOKAPPID_SETTING)
                End If
                Entities.Portals.PortalController.UpdatePortalSetting(PortalId, OG_FACEBOOKAPPID_SETTING, sec.InputFilter(txtFacebookAppId.Text, PortalSecurity.FilterFlag.NoMarkup), True, cboLocale.SelectedItem.Text.ToLower())
            Else

                If Not String.IsNullOrEmpty(Entities.Portals.PortalController.GetPortalSetting(OG_FACEBOOKAPPID_SETTING, PortalId, String.Empty)) Then
                    Entities.Portals.PortalController.DeletePortalSetting(PortalId, OG_FACEBOOKAPPID_SETTING, cboLocale.SelectedItem.Text.ToLower())
                End If

            End If

            Entities.Portals.PortalController.UpdatePortalSetting(PortalId, DEBUG_SETTING, chkDebug.Checked.ToString())

            ' clear module settings cache to load those we just saved
            Entities.Modules.ModuleController.SynchronizeModule(Me.ModuleId)

        End Sub

#End Region

#Region " Private Helper Methods "

        ''' <summary>
        ''' This method centralizes the binding and object assignment for all settings
        ''' </summary>
        ''' <remarks></remarks>
        ''' <history>
        ''' </history>
        Private Sub BindData()

            Try

                If UserInfo.IsSuperUser AndAlso (Not IsFirstTimeSettings) Then
                    lnkDisable.Visible = True
                Else
                    lnkDisable.Visible = False
                End If

                If  Not IsFirstTimeSettings then
                    lnkClearSettings.Visible = UserInfo.IsSuperUser
                Else
                    lnkClearSettings.Visible = False
                End If

                LocalizeModule()
                LoadSiteTypes()
                LoadLocales()
                LoadSettings()

                txtTitle.Enabled = Not chkTitle.Checked
                txtDescription.Enabled = Not chkDescription.Checked
                txtUrl.Enabled = Not chkUrl.Checked
                txtSiteName.Enabled = Not chkSiteName.Checked

                If Not txtTitle.Enabled Then
                    txtTitle.CssClass = DISABLED_TEXTBOX_CSS_SELECTOR
                End If

                If Not txtDescription.Enabled Then
                    txtDescription.CssClass = DISABLED_TEXTBOX_CSS_SELECTOR
                End If

                If Not txtUrl.Enabled Then
                    txtUrl.CssClass = DISABLED_TEXTBOX_CSS_SELECTOR
                End If

                If Not txtSiteName.Enabled Then
                    txtSiteName.CssClass = DISABLED_TEXTBOX_CSS_SELECTOR
                End If

                If String.IsNullOrEmpty(PortalSettings.ActiveTab.Title) Then
                    chkTitle.Enabled = False
                    chkTitle.CssClass = DISABLED_CHECKBOX_CSS_SELECTOR
                End If

                If String.IsNullOrEmpty(PortalSettings.ActiveTab.Description) Then
                    chkDescription.Enabled = False
                    chkDescription.CssClass = DISABLED_CHECKBOX_CSS_SELECTOR
                End If

            Catch ex As Exception
                ProcessModuleLoadException(Me, ex, True)
            End Try

        End Sub

        Private Sub LocalizeModule()

            chkTitle.Text = GetLocalizedString("chkTitle.Text")
            chkDescription.Text = GetLocalizedString("chkDescription.Text")
            chkUrl.Text = GetLocalizedString("chkUrl.Text")
            chkSiteName.Text = GetLocalizedString("chkSiteName.Text")

            rfvSiteName.ErrorMessage = GetLocalizedString("rfvSiteName.ErrorMessage")
            rfvTitle.ErrorMessage = GetLocalizedString("rfvTitle.ErrorMessage")
            rfvDescription.ErrorMessage = GetLocalizedString("rfvDescription.ErrorMessage")
            rfvType.ErrorMessage = GetLocalizedString("rfvType.ErrorMessage")
            rfvUrl.ErrorMessage = GetLocalizedString("rfvUrl.ErrorMessage")
            revFacebookAdmins.ErrorMessage = GetLocalizedString("revFacebookAdmins.ErrorMessage")
            revFacebookAppId.ErrorMessage = GetLocalizedString("revFacebookAppId.ErrorMessage")
            revUrl.ErrorMessage = GetLocalizedString("revUrl.ErrorMessage")
            rfvLocale.ErrorMessage = GetLocalizedString("rfvLocale.ErrorMessage")

            lnkSubmit.Text = GetLocalizedString("lnkSubmit.Text")
            lnkCancel.Text = GetLocalizedString("lnkCancel.Text")

            If lnkDisable.Visible Then
                
                If Not IsOpenGraphProtocolEnabled Then
                    lnkDisable.Text = GetLocalizedString("lnkDisable.1.Text")
                Else
                    lnkDisable.Text = GetLocalizedString("lnkDisable.0.Text")
                End If

            End If

            If lnkClearSettings.Visible Then
                lnkClearSettings.Text = GetLocalizedString("lnkClearSettings.Text")
            End If

        End Sub

        Private Sub SendBackToModule()
            Response.Redirect(NavigateURL())
        End Sub

        Private Sub LoadSiteTypes()

            cboType.Items.Add(New ListItem(GetLocalizedString(String.Concat(SITE_TYPE_TEXT_PATTERN, NUMBER_0)), "activity"))
            cboType.Items.Add(New ListItem(GetLocalizedString(String.Concat(SITE_TYPE_TEXT_PATTERN, NUMBER_1)), "sport"))
            cboType.Items.Add(New ListItem(GetLocalizedString(String.Concat(SITE_TYPE_TEXT_PATTERN, NUMBER_2)), "bar"))
            cboType.Items.Add(New ListItem(GetLocalizedString(String.Concat(SITE_TYPE_TEXT_PATTERN, NUMBER_3)), "company"))
            cboType.Items.Add(New ListItem(GetLocalizedString(String.Concat(SITE_TYPE_TEXT_PATTERN, NUMBER_4)), "cafe"))
            cboType.Items.Add(New ListItem(GetLocalizedString(String.Concat(SITE_TYPE_TEXT_PATTERN, NUMBER_5)), "hotel"))
            cboType.Items.Add(New ListItem(GetLocalizedString(String.Concat(SITE_TYPE_TEXT_PATTERN, NUMBER_6)), "restaurant"))
            cboType.Items.Add(New ListItem(GetLocalizedString(String.Concat(SITE_TYPE_TEXT_PATTERN, NUMBER_7)), "cause"))
            cboType.Items.Add(New ListItem(GetLocalizedString(String.Concat(SITE_TYPE_TEXT_PATTERN, NUMBER_8)), "sports_league"))
            cboType.Items.Add(New ListItem(GetLocalizedString(String.Concat(SITE_TYPE_TEXT_PATTERN, NUMBER_9)), "sports_team"))
            cboType.Items.Add(New ListItem(GetLocalizedString(String.Concat(SITE_TYPE_TEXT_PATTERN, NUMBER_10)), "band"))
            cboType.Items.Add(New ListItem(GetLocalizedString(String.Concat(SITE_TYPE_TEXT_PATTERN, NUMBER_11)), "government"))
            cboType.Items.Add(New ListItem(GetLocalizedString(String.Concat(SITE_TYPE_TEXT_PATTERN, NUMBER_12)), "non_profit"))
            cboType.Items.Add(New ListItem(GetLocalizedString(String.Concat(SITE_TYPE_TEXT_PATTERN, NUMBER_13)), "school"))
            cboType.Items.Add(New ListItem(GetLocalizedString(String.Concat(SITE_TYPE_TEXT_PATTERN, NUMBER_14)), "university"))
            cboType.Items.Add(New ListItem(GetLocalizedString(String.Concat(SITE_TYPE_TEXT_PATTERN, NUMBER_15)), "actor"))
            cboType.Items.Add(New ListItem(GetLocalizedString(String.Concat(SITE_TYPE_TEXT_PATTERN, NUMBER_16)), "athlete"))
            cboType.Items.Add(New ListItem(GetLocalizedString(String.Concat(SITE_TYPE_TEXT_PATTERN, NUMBER_17)), "author"))
            cboType.Items.Add(New ListItem(GetLocalizedString(String.Concat(SITE_TYPE_TEXT_PATTERN, NUMBER_18)), "director"))
            cboType.Items.Add(New ListItem(GetLocalizedString(String.Concat(SITE_TYPE_TEXT_PATTERN, NUMBER_19)), "musician"))
            cboType.Items.Add(New ListItem(GetLocalizedString(String.Concat(SITE_TYPE_TEXT_PATTERN, NUMBER_20)), "politician"))
            cboType.Items.Add(New ListItem(GetLocalizedString(String.Concat(SITE_TYPE_TEXT_PATTERN, NUMBER_21)), "public_figure"))
            cboType.Items.Add(New ListItem(GetLocalizedString(String.Concat(SITE_TYPE_TEXT_PATTERN, NUMBER_22)), "city"))
            cboType.Items.Add(New ListItem(GetLocalizedString(String.Concat(SITE_TYPE_TEXT_PATTERN, NUMBER_23)), "country"))
            cboType.Items.Add(New ListItem(GetLocalizedString(String.Concat(SITE_TYPE_TEXT_PATTERN, NUMBER_24)), "landmark"))
            cboType.Items.Add(New ListItem(GetLocalizedString(String.Concat(SITE_TYPE_TEXT_PATTERN, NUMBER_25)), "state_province"))
            cboType.Items.Add(New ListItem(GetLocalizedString(String.Concat(SITE_TYPE_TEXT_PATTERN, NUMBER_26)), "album"))
            cboType.Items.Add(New ListItem(GetLocalizedString(String.Concat(SITE_TYPE_TEXT_PATTERN, NUMBER_27)), "book"))
            cboType.Items.Add(New ListItem(GetLocalizedString(String.Concat(SITE_TYPE_TEXT_PATTERN, NUMBER_28)), "drink"))
            cboType.Items.Add(New ListItem(GetLocalizedString(String.Concat(SITE_TYPE_TEXT_PATTERN, NUMBER_29)), "food"))
            cboType.Items.Add(New ListItem(GetLocalizedString(String.Concat(SITE_TYPE_TEXT_PATTERN, NUMBER_30)), "game"))
            cboType.Items.Add(New ListItem(GetLocalizedString(String.Concat(SITE_TYPE_TEXT_PATTERN, NUMBER_31)), "product"))
            cboType.Items.Add(New ListItem(GetLocalizedString(String.Concat(SITE_TYPE_TEXT_PATTERN, NUMBER_32)), "song"))
            cboType.Items.Add(New ListItem(GetLocalizedString(String.Concat(SITE_TYPE_TEXT_PATTERN, NUMBER_33)), "movie"))
            cboType.Items.Add(New ListItem(GetLocalizedString(String.Concat(SITE_TYPE_TEXT_PATTERN, NUMBER_34)), "tv_show"))
            cboType.Items.Add(New ListItem(GetLocalizedString(String.Concat(SITE_TYPE_TEXT_PATTERN, NUMBER_35)), "blog"))
            cboType.Items.Add(New ListItem(GetLocalizedString(String.Concat(SITE_TYPE_TEXT_PATTERN, NUMBER_36)), "website"))
            cboType.Items.Add(New ListItem(GetLocalizedString(String.Concat(SITE_TYPE_TEXT_PATTERN, NUMBER_37)), "article"))
            cboType.Items.Insert(0, "---")

        End Sub

        Private Sub LoadLocales()

            cboLocale.DataSource = LocaleController.Instance().GetLocales(PortalId).Keys
            cboLocale.DataBind()

            For Each oItem As ListItem In cboLocale.Items

                oItem.Value = Regex.Replace(oItem.Value, "-", "_")

            Next

            cboLocale.Items.Insert(0, "---")

        End Sub

        Private Sub ClearOgpHostSettingsCache()

            ' clear the ogp host-level cache for the next page load
            DataCache.RemoveCache(OpenGraphController.HOST_CONFIGURATION_CACHE_KEY)

        End Sub

        Private Sub ClearOgpPageMarkUpCache()

            ' clear the ogp page-level cache for the next page load
            Dim strCacheKey As String = GetOgpPageMarkUpCacheKey(PortalId, TabId)
            DataCache.RemoveCache(strCacheKey)

        End Sub

        Private Sub ClearOgpPageSettingsCache()

            ' clear the ogp page-level cache for the next page load
            Dim strCacheKey As String = GetOgpPageCacheKey(PortalId, TabId)
            DataCache.RemoveCache(strCacheKey)

        End Sub

#End Region

#Region " Xml Merge Methods "
        
        Private Sub CoordinateMergeAction()

            If IsOpenGraphProtocolEnabled Then

                DisableMergeHandler()

            Else

                EnableMergeHandler()

            End If

        End Sub

        Private Sub MergeHandler()

            If IsFirstTimeSettings AndAlso (Not HandlerMerged) Then

                Try

                    EnableMergeHandler()
                    Entities.Controllers.HostController.Instance().Update(HANDLER_MERGED_SETTING, "True")

                Catch ex As Exception

                    LogException(ex)
                    Entities.Controllers.HostController.Instance().Update(HANDLER_MERGED_SETTING, "False")

                End Try

            End If

        End Sub

        Private Sub EnableMergeHandler()

            Try

                Dim ctlOpg As New OpenGraphController
                ctlOpg.EnableOpenGraphProtocolInjection()

            Catch ex As Exception
                Entities.Controllers.HostController.Instance().Update(HANDLER_MERGED_SETTING, "False")
                LogException(ex)
            End Try

        End Sub

        Private Sub DisableMergeHandler()

            Try

                Dim ctlOpg As New OpenGraphController
                ctlOpg.DisableOpenGraphProtocolInjection()

            Catch ex As Exception
                Entities.Controllers.HostController.Instance().Update(HANDLER_MERGED_SETTING, "False")
                LogException(ex)
            End Try

        End Sub

#End Region

    End Class

End Namespace