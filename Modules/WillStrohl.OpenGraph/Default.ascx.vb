'
' Will Strohl (will.strohl@gmail.com)
' http://www.willstrohl.com
'
'Copyright (c) 2011-2013, Will Strohl
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

Imports DotNetNuke.UI.Skins.Controls
Imports DotNetNuke.Services.Exceptions
Imports DotNetNuke.Entities.Modules
Imports System.IO
Imports System.Linq
Imports System.Xml.Linq
Imports WillStrohl.Modules.OpenGraph.OpenGraphController

Namespace WillStrohl.Modules.OpenGraph

    Partial Public MustInherit Class DefaultView
        Inherits WnsPortalModuleBase
        Implements IActionable

#Region " Private Members "

        Private Const EDIT_SETTINGS_KEY As String = "EditSettings.Text"
        Private Const EDIT_PAGE_KEY As String = "Edit"
        Private Const SETTINGS_MISSING_ERROR As String = "SettingsMissing.ErrorMessage"

#End Region

#Region " Private Properties "



#End Region

#Region " Event Handlers "

        Private Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
            Try
                If Not Page.IsPostBack Then
                    BindData()
                End If

                If Not (IsEditable AndAlso PortalSettings.UserMode = Entities.Portals.PortalSettings.Mode.Edit) Then
                    ' hide the module container (and the rest of the module as well)
                    ContainerControl.Visible = False
                End If

            Catch exc As Exception ' Module failed to load
                ProcessModuleLoadException(Me, exc, IsEditable)
            End Try
        End Sub

#End Region

#Region " Helper Methods "

        Private Sub BindData()

            ' localize any static text
            LocalizeModule()

            ' check to see if the settings have ever been saved
            If Not SettingsAvailable() Then
                Skins.Skin.AddModuleMessage(Me, GetLocalizedString(SETTINGS_MISSING_ERROR), ModuleMessage.ModuleMessageType.RedError)
            Else
                DisplayUserMessages()
            End If

            ' if the user is allowed to edit the settings, perform a health check
            If IsEditable AndAlso PortalSettings.UserMode = Entities.Portals.PortalSettings.Mode.Edit Then
                CheckDependencies()
            End If

        End Sub

        Private Sub LocalizeModule()

            ' nothing to localize at this time

        End Sub

        ''' <summary>
        ''' These OGP tags are not required, but are highly recommended, so we let the user know
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub DisplayUserMessages()

            If String.IsNullOrEmpty(Image) Then
                ' warn the admin that the image really is strongly suggested
                If IsEditable AndAlso PortalSettings.UserMode = Entities.Portals.PortalSettings.Mode.Edit Then
                    Skins.Skin.AddModuleMessage(Me, GetLocalizedString("ImageMissing.Warning"), ModuleMessage.ModuleMessageType.YellowWarning)
                End If
            End If

            If String.IsNullOrEmpty(FacebookAdmins) Or String.IsNullOrEmpty(FacebookAppId) Then
                ' warn the admin that the Facebook settings are strongly suggested
                If IsEditable AndAlso PortalSettings.UserMode = Entities.Portals.PortalSettings.Mode.Edit Then
                    Skins.Skin.AddModuleMessage(Me, GetLocalizedString("FacebookMissing.Warning"), ModuleMessage.ModuleMessageType.YellowWarning)
                End If
            End If

        End Sub

        ''' <summary>
        ''' Look for required OGP settings
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function SettingsAvailable() As Boolean

            Return Not (String.IsNullOrEmpty(Title) Or String.IsNullOrEmpty(Description) Or String.IsNullOrEmpty(Url))

        End Function

        ''' <summary>
        ''' This method looks for dependencies to ensure things can load properly and returns information if they cannot.
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub CheckDependencies()

            Dim blnStatus As Boolean = True
            Dim ctlOpenGraph As New OpenGraphController

            ' check to see if OGP is enabled in the web.config
            If Not ctlOpenGraph.DoesHttpModuleExist() AndAlso Not IsFirstTimeSettings Then
                Skins.Skin.AddModuleMessage(Me, GetLocalizedString("OpenGraphDisabled.ErrorMessage"), ModuleMessage.ModuleMessageType.RedError)
                blnStatus = False
            End If

            ' look for the host settings configuration file
            Dim strHostConfigPath As String = Server.MapPath(HostConfigurationPath)
            If Not File.Exists(strHostConfigPath) And SettingsAvailable() Then

                Skins.Skin.AddModuleMessage(Me, GetLocalizedString("HostConfig.ErrorMessage"), ModuleMessage.ModuleMessageType.RedError)
                blnStatus = False

            ElseIf File.Exists(strHostConfigPath) Then

                Dim xConfigDoc As XDocument = ctlOpenGraph.GetConfigurationFile(HOST_CONFIGURATION_CACHE_KEY, strHostConfigPath)

                Dim oHostConfig As XElement = (From xconfig In xConfigDoc.Descendants("Site")
                    Where xconfig.Attribute("portalId").Value = PortalId.ToString()
                    Select xconfig).FirstOrDefault

                If Not oHostConfig Is Nothing AndAlso Not String.Equals(oHostConfig.Attribute("httpAlias").Value.ToLower(), PortalSettings.PortalAlias.HTTPAlias.ToLower()) Then
                    Skins.Skin.AddModuleMessage(Me, GetLocalizedString("HttpAlias.ErrorMessage"), ModuleMessage.ModuleMessageType.RedError)
                    blnStatus = False
                End If

            End If

            ' look for the page-level configuration file
            Dim strPageConfigPath As String = Server.MapPath(String.Format(PageConfigurationPath, PortalId, TabId))
            If Not File.Exists(strPageConfigPath) And SettingsAvailable() Then

                Skins.Skin.AddModuleMessage(Me, GetLocalizedString("PageConfig.ErrorMessage"), ModuleMessage.ModuleMessageType.RedError)
                blnStatus = False

            ElseIf File.Exists(strPageConfigPath) Then

                Dim strCacheKey As String = GetOgpPageCacheKey(PortalId, TabId)

                Dim xConfigDoc As XDocument = ctlOpenGraph.GetConfigurationFile(strCacheKey, strPageConfigPath)

                Dim oSiteConfig As XElement = (From xconfig In xConfigDoc.Descendants("ogSettings")
                    Select xconfig).FirstOrDefault

                If Not oSiteConfig Is Nothing AndAlso Not String.Equals(oSiteConfig.Element("HttpAlias").Value.ToLower(), PortalSettings.PortalAlias.HTTPAlias.ToLower()) Then
                    Skins.Skin.AddModuleMessage(Me, GetLocalizedString("HttpAlias.ErrorMessage"), ModuleMessage.ModuleMessageType.RedError)
                    blnStatus = False
                End If

            End If

            ' check cache dependencies to see if they load properly
            If SettingsAvailable() AndAlso Not CheckCacheDependencies() Then

                ' display a generic message to general content editors since they can't really fix the cache issue
                If Not UserInfo.IsSuperUser Then
                    Skins.Skin.AddModuleMessage(Me, GetLocalizedString("GenericCacheIssue.ErrorMessage"), ModuleMessage.ModuleMessageType.RedError)
                End If

                blnStatus = False

            End If

            ' notify the content editor of the success
            If blnStatus AndAlso SettingsAvailable() Then
                Skins.Skin.AddModuleMessage(Me, GetLocalizedString("OpenGraph.Success"), ModuleMessage.ModuleMessageType.GreenSuccess)
            End If

        End Sub

        ''' <summary>
        ''' Check to see if items can be loaded from cache to give more detailed results if the module is working properly
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function CheckCacheDependencies() As Boolean

            Dim blnStatus As Boolean = True
            Dim ctlModule As New OpenGraphController

            ' check to see if the host configuration can be loaded from cache
            Dim xHost As XDocument = ctlModule.GetConfigurationFile(HOST_CONFIGURATION_CACHE_KEY, OpenGraphController.HOST_CONFIGURATION_PATH)
            If xHost Is Nothing Then
                If UserInfo.IsSuperUser Then
                    Skins.Skin.AddModuleMessage(Me, GetLocalizedString("HostCache.ErrorMessage"), ModuleMessage.ModuleMessageType.RedError)
                End If
                blnStatus = False
            End If

            ' check to see if the page configuration can be loaded from cache
            Dim strPageCacheKey As String = OpenGraphController.GetOgpPageCacheKey(PortalId, TabId)
            Dim strPageConfigurationPath As String = OpenGraphController.GetOgpPageConfigurationPath(PortalId, TabId)
            Dim xPage As XDocument = ctlModule.GetConfigurationFile(strPageCacheKey, strPageConfigurationPath)
            If xPage Is Nothing Then
                If UserInfo.IsSuperUser Then
                    Skins.Skin.AddModuleMessage(Me, GetLocalizedString("PageCache.ErrorMessage"), ModuleMessage.ModuleMessageType.RedError)
                End If
                blnStatus = False
            End If

            ' check to see if there is cached OGP markup
            Dim strPageMarkupCacheKey As String = OpenGraphController.GetOgpPageMarkUpCacheKey(PortalId, TabId)
            Dim oPage As Object = DataCache.GetCache(strPageMarkupCacheKey)
            If oPage Is Nothing OrElse String.IsNullOrEmpty(oPage.ToString()) Then
                If UserInfo.IsSuperUser Then
                    Skins.Skin.AddModuleMessage(Me, GetLocalizedString("PageMarkupCache.ErrorMessage"), ModuleMessage.ModuleMessageType.RedError)
                End If
                blnStatus = False
            End If

            Return blnStatus

        End Function

#End Region

#Region " IActionable Implementation "

        Public ReadOnly Property ModuleActions() As Actions.ModuleActionCollection Implements IActionable.ModuleActions
            Get
                Dim oActions As New Actions.ModuleActionCollection
                oActions.Add(GetNextActionID, Me.GetLocalizedString(EDIT_SETTINGS_KEY), _
                    String.Empty, String.Empty, String.Empty, _
                    EditUrl(String.Empty, String.Empty, EDIT_PAGE_KEY), _
                    False, DotNetNuke.Security.SecurityAccessLevel.Edit, True, False)

                Return oActions
            End Get
        End Property

#End Region

    End Class

End Namespace