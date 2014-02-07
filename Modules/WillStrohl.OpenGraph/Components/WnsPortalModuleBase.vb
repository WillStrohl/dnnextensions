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
Imports DotNetNuke.Entities.Controllers
Imports DotNetNuke.Entities.Modules
Imports DotNetNuke.Services.Localization
Imports DotNetNuke.Entities.Portals
Imports WillStrohl.Modules.OpenGraph.OpenGraphController

Namespace WillStrohl.Modules.OpenGraph

    Public MustInherit Class WnsPortalModuleBase
        Inherits PortalModuleBase

#Region " Private Members "

        Private p_SiteName As String = Null.NullString
        Private p_UsePortalTitle As Nullable(Of Boolean)
        Private p_Title As String = Null.NullString
        Private p_UseTabTitle As Nullable(Of Boolean)
        Private p_Description As String = Null.NullString
        Private p_UseTabDescription As Nullable(Of Boolean)
        Private p_SiteType As String = Null.NullString
        Private p_Url As String = Null.NullString
        Private p_UseTabUrl As Nullable(Of Boolean)
        Private p_Image As String = Null.NullString
        Private p_FacebookAdmins As String = Null.NullString
        Private p_FacebookAppId As String = Null.NullString
        Private p_SiteLocale As String = Null.NullString
        Private p_HandlerMerged As Nullable(Of Boolean) = Nothing
        Private p_IsFirstTimeSettings As Nullable(Of Boolean) = Nothing
        Private p_Debug As Nullable(Of Boolean) = Nothing

#End Region

#Region " Properties "

        Protected ReadOnly Property SiteName() As String
            Get
                If String.IsNullOrEmpty(p_SiteName) Then
                    If Not String.IsNullOrEmpty(PortalController.GetPortalSetting(OG_SITENAME_SETTING, PortalId, String.Empty)) Then
                        p_SiteName = PortalController.GetPortalSetting(OG_SITENAME_SETTING, PortalId, String.Empty)
                    End If
                End If

                Return p_SiteName
            End Get
        End Property

        Protected ReadOnly Property Title() As String
            Get
                If String.IsNullOrEmpty(p_Title) Then
                    If Not Settings(OG_TITLE_SETTING) Is Nothing Then
                        p_Title = Settings(OG_TITLE_SETTING).ToString()
                    End If
                End If

                Return p_Title
            End Get
        End Property

        Protected ReadOnly Property Description() As String
            Get
                If String.IsNullOrEmpty(p_Description) Then
                    If Not Settings(OG_DESCRIPTION_SETTING) Is Nothing Then
                        p_Description = Settings(OG_DESCRIPTION_SETTING).ToString()
                    End If
                End If

                Return p_Description
            End Get
        End Property

        Protected ReadOnly Property SiteType() As String
            Get
                If String.IsNullOrEmpty(p_SiteType) Then
                    If Not String.IsNullOrEmpty(PortalController.GetPortalSetting(OG_SITETYPE_SETTING, PortalId, String.Empty)) Then
                        p_SiteType = PortalController.GetPortalSetting(OG_SITETYPE_SETTING, PortalId, String.Empty)
                    End If
                End If

                Return p_SiteType
            End Get
        End Property

        Protected ReadOnly Property Url() As String
            Get
                If String.IsNullOrEmpty(p_Url) Then
                    If Not Settings(OG_URL_SETTING) Is Nothing Then
                        p_Url = Settings(OG_URL_SETTING).ToString()
                    Else
                        p_Url = PortalSettings.ActiveTab.FullUrl
                    End If
                End If

                Return p_Url
            End Get
        End Property

        Protected ReadOnly Property Image() As String
            Get
                If String.IsNullOrEmpty(p_Image) Then
                    If Not Settings(OG_IMAGE_SETTING) Is Nothing Then
                        p_Image = Settings(OG_IMAGE_SETTING).ToString()
                    End If
                End If

                Return p_Image
            End Get
        End Property

        Protected ReadOnly Property FacebookAdmins() As String
            Get
                If String.IsNullOrEmpty(p_FacebookAdmins) Then
                    If Not String.IsNullOrEmpty(PortalController.GetPortalSetting(OG_FACEBOOKADMINS_SETTING, PortalId, String.Empty)) Then
                        p_FacebookAdmins = PortalController.GetPortalSetting(OG_FACEBOOKADMINS_SETTING, PortalId, String.Empty)
                    End If
                End If

                Return p_FacebookAdmins
            End Get
        End Property

        Protected ReadOnly Property FacebookAppId() As String
            Get
                If String.IsNullOrEmpty(p_FacebookAppId) Then
                    If Not String.IsNullOrEmpty(PortalController.GetPortalSetting(OG_FACEBOOKAPPID_SETTING, PortalId, String.Empty)) Then
                        p_FacebookAppId = PortalController.GetPortalSetting(OG_FACEBOOKAPPID_SETTING, PortalId, String.Empty)
                    End If
                End If

                Return p_FacebookAppId
            End Get
        End Property

        Protected ReadOnly Property UsePortalTitle() As Boolean
            Get
                If Not p_UsePortalTitle.HasValue Then
                    If Not String.IsNullOrEmpty(PortalController.GetPortalSetting(USE_PORTALTITLE_SETTING, PortalId, String.Empty)) Then
                        p_UsePortalTitle = Boolean.Parse(PortalController.GetPortalSetting(USE_PORTALTITLE_SETTING, PortalId, "true"))
                    Else
                        p_UsePortalTitle = False
                    End If
                End If

                Return p_UsePortalTitle.Value
            End Get
        End Property

        Protected ReadOnly Property UseTabTitle() As Boolean
            Get
                If Not p_UseTabTitle.HasValue Then
                    If Not Settings(USE_TABTITLE_SETTING) Is Nothing Then
                        p_UseTabTitle = Boolean.Parse(Settings(USE_TABTITLE_SETTING).ToString())
                    Else
                        p_UseTabTitle = False
                    End If
                End If

                Return p_UseTabTitle.Value
            End Get
        End Property

        Protected ReadOnly Property UseTabDescription() As Boolean
            Get
                If Not p_UseTabDescription.HasValue Then
                    If Not Settings(USE_TABDESCRIPTION_SETTING) Is Nothing Then
                        p_UseTabDescription = Boolean.Parse(Settings(USE_TABDESCRIPTION_SETTING).ToString())
                    Else
                        p_UseTabDescription = False
                    End If
                End If

                Return p_UseTabDescription.Value
            End Get
        End Property

        Protected ReadOnly Property UseTabUrl() As Boolean
            Get
                If Not p_UseTabUrl.HasValue Then
                    If Not Settings(USE_TABURL_SETTING) Is Nothing Then
                        p_UseTabUrl = Boolean.Parse(Settings(USE_TABURL_SETTING).ToString())
                    Else
                        p_UseTabUrl = False
                    End If
                End If

                Return p_UseTabUrl.Value
            End Get
        End Property
        
        Protected ReadOnly Property SiteLocale() As String
            Get
                If String.IsNullOrEmpty(p_SiteLocale) Then
                    If Not Settings(OG_LOCALE_SETTING) Is Nothing Then
                        p_SiteLocale = Settings(OG_LOCALE_SETTING).ToString()
                    End If
                End If

                Return p_SiteLocale
            End Get
        End Property

        Protected ReadOnly Property HandlerMerged() As Boolean
            Get
                If Not p_HandlerMerged.HasValue Then
                    If Not String.IsNullOrEmpty(HostController.Instance().GetString(HANDLER_MERGED_SETTING, String.Empty)) Then
                        p_HandlerMerged = Boolean.Parse(HostController.Instance().GetString(HANDLER_MERGED_SETTING, "False"))
                    Else
                        p_HandlerMerged = False
                    End If
                End If

                Return p_HandlerMerged.Value
            End Get
        End Property

        Protected ReadOnly Property IsFirstTimeSettings() As Boolean
            Get
                Try
                    If Not p_IsFirstTimeSettings.HasValue Then
                        If Not String.IsNullOrEmpty(HostController.Instance().GetString(IS_FIRST_TIME_SETTING, String.Empty)) Then
                            p_IsFirstTimeSettings = Boolean.Parse(HostController.Instance().GetString(IS_FIRST_TIME_SETTING, "True"))
                        Else
                            p_IsFirstTimeSettings = True
                        End If
                    End If

                    Return p_IsFirstTimeSettings.Value
                Catch ex As Exception
                    LogException(ex)
                    p_IsFirstTimeSettings = True
                End Try
            End Get
        End Property

        Protected ReadOnly Property IsDebugEnabled() As Boolean
            Get
                If Not p_Debug.HasValue Then
                    If Not String.IsNullOrEmpty(PortalController.GetPortalSetting(DEBUG_SETTING, PortalId, String.Empty)) Then
                        p_Debug = Boolean.Parse(PortalController.GetPortalSetting(DEBUG_SETTING, PortalId, String.Empty))
                    Else
                        p_Debug = False
                    End If
                End If

                Return p_Debug.Value
            End Get
        End Property

#End Region

#Region " Localization "

        Protected Overloads Function GetLocalizedString(ByVal LocalizationKey As String) As String
            If Not String.IsNullOrEmpty(LocalizationKey) Then
                Return Localization.GetString(LocalizationKey, Me.LocalResourceFile)
            Else
                Return String.Empty
            End If
        End Function

        Protected Overloads Function GetLocalizedString(ByVal LocalizationKey As String, ByVal LocalResourceFilePath As String) As String
            If Not String.IsNullOrEmpty(LocalizationKey) Then
                Return Localization.GetString(LocalizationKey, LocalResourceFilePath)
            Else
                Return String.Empty
            End If
        End Function

#End Region

#Region " Event Handlers "

        Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            ' request that the DNN framework load the jQuery script into the markup
            jQuery.RequestDnnPluginsRegistration()

        End Sub

#End Region

    End Class

End Namespace