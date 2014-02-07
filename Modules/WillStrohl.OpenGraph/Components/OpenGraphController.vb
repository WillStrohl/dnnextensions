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
Imports DotNetNuke.Application
Imports System.IO
Imports System.Xml
Imports System.Xml.Linq
Imports DotNetNuke.Entities.Modules
Imports DotNetNuke.Entities.Portals

Namespace WillStrohl.Modules.OpenGraph

    Public NotInheritable Class OpenGraphController

#Region " Constants "

        ' configuration paths
        Private Const XMLMERGE_ADD_OPG As String = "~/DesktopModules/WillStrohl.OpenGraph/Config/HanderMerge-Add.xml.resources"
        Private Const XMLMERGE_REMOVE_OPG As String = "~/DesktopModules/WillStrohl.OpenGraph/Config/HanderMerge-Remove.xml.resources"
        Public Const HOST_CONFIGURATION_PATH As String = "/DesktopModules/WillStrohl.OpenGraph/Config/ogConfig.xml.resources"
        Private Const PAGE_CONFIGURATION_PATH As String = "/DesktopModules/WillStrohl.OpenGraph/Config/ogSettings-{0}-{1}.xml.resources"
        Private Const TILDE As String = "~"

        ' cache keys
        Public Const HOST_CONFIGURATION_CACHE_KEY As String = "WillStrohl.OpenGraph.Host.Config"
        Public Const SITE_CONFIGURATION_CACHE_KEY As String = "WillStrohl.OpenGraph.Site.Config"
        Private Const PAGE_CONFIGURATION_CACHE_KEY_FORMAT As String = "WillStrohl.OpenGraph.Page.Config.{0}.{1}"
        Private Const PAGE_MARKUP_CACHE_KEY_FORMAT As String = "WillStrohl.OpenGraph.Page.MarkUp.{0}.{1}"

        ' Module-Level Settings
        Public Const OG_TITLE_SETTING As String = "og-title"
        Public Const USE_TABTITLE_SETTING As String = "og-usetabtitle"
        Public Const OG_DESCRIPTION_SETTING As String = "og-description"
        Public Const USE_TABDESCRIPTION_SETTING As String = "og-usetabdescription"
        Public Const OG_URL_SETTING As String = "og-url"
        Public Const USE_TABURL_SETTING As String = "og-usetaburl"
        Public Const OG_IMAGE_SETTING As String = "og-image"
        Public Const OG_LOCALE_SETTING As String = "og-locale"
        Public Const OVERRIDE_OPENGRAPH_SETTING As String = "og-override-og-tags"

        ' Site-Level Settings
        Public Const OG_SITENAME_SETTING As String = "wnsogp-sitename"
        Public Const USE_PORTALTITLE_SETTING As String = "wnsogp-useportaltitle"
        Public Const OG_SITETYPE_SETTING As String = "wnsogp-sitetype"
        Public Const OG_FACEBOOKADMINS_SETTING As String = "wnsogp-facebookadmins"
        Public Const OG_FACEBOOKAPPID_SETTING As String = "wnsogp-facebookappid"
        Public Const USE_GLOBALSETTINGS_SETTING As String = "wnsogp-useglobalsettings"
        Public Const DEBUG_SETTING As String = "wnsogp-debug"
        
        ' Host-Level Settings
        Public Const HANDLER_MERGED_SETTING As String = "wnsogp-handler-merged"
        Public Const IS_FIRST_TIME_SETTING As String = "wnsogp-first-time-setting"
        
#Region " Numbers "

        Public Const NUMBER_0 As String = "0"
        Public Const NUMBER_1 As String = "1"
        Public Const NUMBER_2 As String = "2"
        Public Const NUMBER_3 As String = "3"
        Public Const NUMBER_4 As String = "4"
        Public Const NUMBER_5 As String = "5"
        Public Const NUMBER_6 As String = "6"
        Public Const NUMBER_7 As String = "7"
        Public Const NUMBER_8 As String = "8"
        Public Const NUMBER_9 As String = "9"
        Public Const NUMBER_10 As String = "10"
        Public Const NUMBER_11 As String = "11"
        Public Const NUMBER_12 As String = "12"
        Public Const NUMBER_13 As String = "13"
        Public Const NUMBER_14 As String = "14"
        Public Const NUMBER_15 As String = "15"
        Public Const NUMBER_16 As String = "16"
        Public Const NUMBER_17 As String = "17"
        Public Const NUMBER_18 As String = "18"
        Public Const NUMBER_19 As String = "19"
        Public Const NUMBER_20 As String = "20"
        Public Const NUMBER_21 As String = "21"
        Public Const NUMBER_22 As String = "22"
        Public Const NUMBER_23 As String = "23"
        Public Const NUMBER_24 As String = "24"
        Public Const NUMBER_25 As String = "25"
        Public Const NUMBER_26 As String = "26"
        Public Const NUMBER_27 As String = "27"
        Public Const NUMBER_28 As String = "28"
        Public Const NUMBER_29 As String = "29"
        Public Const NUMBER_30 As String = "30"
        Public Const NUMBER_31 As String = "31"
        Public Const NUMBER_32 As String = "32"
        Public Const NUMBER_33 As String = "33"
        Public Const NUMBER_34 As String = "34"
        Public Const NUMBER_35 As String = "35"
        Public Const NUMBER_36 As String = "36"
        Public Const NUMBER_37 As String = "37"

#End Region

#End Region

#Region " Properties "

        ''' <summary>
        ''' Host configuration file path with a preceding tilde
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared ReadOnly Property HostConfigurationPath() As String
            Get

                Return String.Concat(TILDE, HOST_CONFIGURATION_PATH)

            End Get
        End Property

        ''' <summary>
        ''' Page configuration file path with a preceding tilde
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared ReadOnly Property PageConfigurationPath() As String
            Get

                Return String.Concat(TILDE, PAGE_CONFIGURATION_PATH)

            End Get
        End Property

#End Region

#Region " Web.Config "

        ''' <summary>
        ''' Enables the OGP process for the DNN site
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub EnableOpenGraphProtocolInjection()

            RunXmlMerge(XMLMERGE_ADD_OPG)

        End Sub

        ''' <summary>
        ''' Disables the OGP process for the DNN site
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub DisableOpenGraphProtocolInjection()

            RunXmlMerge(XMLMERGE_REMOVE_OPG)

        End Sub

        ''' <summary>
        ''' RunXmlMerge - uses the XmlMerge feature to enable/disable OGP in the web.config
        ''' </summary>
        ''' <param name="XmlMergeFilePath">Path where the merge file is found</param>
        ''' <remarks></remarks>
        Private Sub RunXmlMerge(ByVal XmlMergeFilePath As String)

            ' get a fully qualified path
            Dim strPath As String = System.Web.HttpContext.Current.Server.MapPath(XmlMergeFilePath)
            Dim doc As XmlDocument = New XmlDocument

            ' load the merge file into memory
            Using sReader As StreamReader = File.OpenText(strPath)
                doc.Load(sReader)
            End Using

            ' get a reference to the merge
            Dim app As Application = DotNetNukeContext.Current.Application
            Dim merge As Services.Installer.XmlMerge = New Services.Installer.XmlMerge(doc, DotNetNuke.Common.Globals.FormatVersion(app.Version), app.Description)

            ' execute the XML merge
            merge.UpdateConfigs()

        End Sub

        ''' <summary>
        ''' Checks to see if the OGP HttpModule is still specified in teh web.config
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function DoesHttpModuleExist() As Boolean

            Dim strConfig As String = System.Web.HttpContext.Current.Server.MapPath("~/web.config")

            ' Load the config file into an xml document.
            Dim xmlDoc As New XmlDocument
            xmlDoc.Load(strConfig)

            ' Load up the httpModules node.
            Dim nodModules As XmlNode
            nodModules = xmlDoc.SelectSingleNode("/configuration/system.webServer/modules/add[@name = 'willStrohlOpenGraphRewriter']")

            If nodModules Is Nothing Then
                Return False
            Else
                Return True
            End If

        End Function

#End Region

#Region " Configuration File "

        ''' <summary>
        ''' Returns the XDocument representation of the OGP configuration files
        ''' </summary>
        ''' <param name="CacheKey">Cache key to check for and cache the config for performance</param>
        ''' <param name="ConfigurationPath">The path where the configuration file is found</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetConfigurationFile(ByRef CacheKey As String, ByRef ConfigurationPath As String) As XDocument

            Dim xConfigDoc As XDocument

            ' attempt to load the configuration from cache
            Dim oConfig As Object = DataCache.GetCache(CacheKey)

            ' if the cache is empty, grab the config from the file system
            If oConfig Is Nothing Then

                If File.Exists(ConfigurationPath) Then

                    ' load the configuration
                    Using sReader As StreamReader = File.OpenText(ConfigurationPath)
                        xConfigDoc = XDocument.Load(sReader)
                        DataCache.SetCache(CacheKey, xConfigDoc, DateTime.Now.AddHours(1))
                    End Using

                Else

                    xConfigDoc = Nothing

                End If

            Else
                ' load the config from cache
                xConfigDoc = CType(oConfig, XDocument)
            End If

            Return xConfigDoc

        End Function

        ''' <summary>
        ''' This method returns a properly formatted path to use to access the page-level OGP configuration file
        ''' </summary>
        ''' <param name="PortalId">ID of the site</param>
        ''' <param name="TabId">ID of the page</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overloads Shared Function GetOgpPageConfigurationPath(ByVal PortalId As Integer, ByVal TabId As Integer) As String

            Return GetOgpPageConfigurationPath(PortalId.ToString(), TabId.ToString())

        End Function

        ''' <summary>
        ''' This method returns a properly formatted path to use to access the page-level OGP configuration file
        ''' </summary>
        ''' <param name="PortalId">ID of the site</param>
        ''' <param name="TabId">ID of the page</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overloads Shared Function GetOgpPageConfigurationPath(ByVal PortalId As String, ByVal TabId As String) As String

            If Not IsNumeric(PortalId) Or Not IsNumeric(TabId) Then
                Throw New ArgumentOutOfRangeException("GetOgpPageConfigurationPath() requires all parameters to be in numeric format.")
            End If

            Return String.Format(PAGE_CONFIGURATION_PATH, PortalId, TabId)

        End Function

#End Region

#Region " Cache Key "

        ''' <summary>
        ''' This method returns a properly formatted key to use to save and read the page-level OGP settings from cache
        ''' </summary>
        ''' <param name="PortalId"></param>
        ''' <param name="TabId"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overloads Shared Function GetOgpPageCacheKey(ByVal PortalId As Integer, ByVal TabId As Integer) As String

            Return GetOgpPageCacheKey(PortalId.ToString(), TabId.ToString())

        End Function

        ''' <summary>
        ''' This method returns a properly formatted key to use to save and read the page-level OGP settings from cache
        ''' </summary>
        ''' <param name="PortalId"></param>
        ''' <param name="TabId"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overloads Shared Function GetOgpPageCacheKey(ByVal PortalId As String, ByVal TabId As String) As String

            If Not IsNumeric(PortalId) Or Not IsNumeric(TabId) Then
                Throw New ArgumentOutOfRangeException("GetOgpCacheKey() requires all parameters to be in numeric format.")
            End If

            Return String.Format(PAGE_CONFIGURATION_CACHE_KEY_FORMAT, PortalId, TabId)

        End Function

        ''' <summary>
        ''' This method returns a properly formatted key to use to save and read the page OGP markup from cache
        ''' </summary>
        ''' <param name="PortalId"></param>
        ''' <param name="TabId"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overloads Shared Function GetOgpPageMarkUpCacheKey(ByVal PortalId As Integer, ByVal TabId As Integer) As String

            Return GetOgpPageMarkUpCacheKey(PortalId.ToString(), TabId.ToString())

        End Function

        ''' <summary>
        ''' This method returns a properly formatted key to use to save and read the page OGP markup from cache
        ''' </summary>
        ''' <param name="PortalId"></param>
        ''' <param name="TabId"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overloads Shared Function GetOgpPageMarkUpCacheKey(ByVal PortalId As String, ByVal TabId As String) As String

            If Not IsNumeric(PortalId) Or Not IsNumeric(TabId) Then
                Throw New ArgumentOutOfRangeException("GetOgpPageMarkUpCacheKey() requires all parameters to be in numeric format.")
            End If

            Return String.Format(PAGE_MARKUP_CACHE_KEY_FORMAT, PortalId, TabId)

        End Function

#End Region

        Public Shared Sub NukeSettings(ByVal PortalId As Integer, ByVal ModuleId As Integer, ByVal TabModuleId As Integer)

            Dim ctlModule As New ModuleController
            ctlModule.DeleteModuleSettings(ModuleId)
            ctlModule.DeleteTabModuleSettings(TabModuleId)

            PortalController.DeletePortalSetting(PortalId, OG_SITENAME_SETTING)
            PortalController.DeletePortalSetting(PortalId, USE_PORTALTITLE_SETTING)
            PortalController.DeletePortalSetting(PortalId, OG_SITETYPE_SETTING)
            PortalController.DeletePortalSetting(PortalId, OG_FACEBOOKADMINS_SETTING)
            PortalController.DeletePortalSetting(PortalId, OG_FACEBOOKAPPID_SETTING)
            PortalController.DeletePortalSetting(PortalId, DEBUG_SETTING)

            HostController.Instance().Update(IS_FIRST_TIME_SETTING, String.Empty)

        End Sub

    End Class

End Namespace