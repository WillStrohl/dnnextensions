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

Imports DotNetNuke.Services.FileSystem
Imports System.Globalization
Imports System.IO
Imports System.Linq
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Xml.Linq
Imports System.Web
Imports DotNetNuke.Entities.Portals

Namespace WillStrohl.Modules.OpenGraph

    Public NotInheritable Class OpenGraphWriter
        Implements IHttpModule

#Region " Private Members "

        Private mApplication As HttpApplication
        Private Const INVALID_EXTENSION_MATCH_PATTERN As String = "\.(swf|jpg|jpeg|jpe|gif|bmp|png|doc|docx|xls|xlsx|ppt|pptx|pdf|txt|xml|xsl|xsd|css|zip|template|htmtemplate|ico|avi|mpg|mpeg|mp3|wmv|mov|wav|js|axd|htm|html|ashx)$"
        Private Const INVALID_REQUEST_PATCH_MATCH_PATTERN As String = "(ctl=Edit|mid=\d+|popUp=true|ctl/Edit|mid/\d+|/Admin/|/Host/|/DesktopModules/|/Providers/|/Resources/|\.ashx/)"
        'Private Const REQUEST_POST As String = "POST"
        'Private Const REQUEST_CONTENTTYPE_AJAX As String = "application/json"
        Private Const HEADER_AJAX_KEY As String = "X-MicrosoftAjax"
        Private Const HEADER_AJAX_VALUE As String = "Delta=true"

#End Region

#Region " Constructors "

        Public Sub New()
        End Sub

#End Region

#Region " Event Handlers "

        Public Sub Application_BeginRequest(ByVal sender As [Object], ByVal e As System.EventArgs)

            ' only proceed if parsing for applicable web pages (ignore postbacks, reloads, editing pages, admin, host, and AJAX)
            If ProceedWithProcessing(mApplication.Context) Then

                ' Create a new filter
                Dim mStreamFilter As New OpenGraphStream(mApplication.Response.Filter, mApplication)

                ' Filter the content and insert it onto the page
                mApplication.Response.Filter = mStreamFilter

            End If

        End Sub

#End Region

#Region " Helper Methods "

        Private Function ProceedWithProcessing(ByRef oContext As HttpContext) As Boolean

            If Regex.IsMatch(oContext.Request.Url.LocalPath, INVALID_EXTENSION_MATCH_PATTERN, RegexOptions.IgnoreCase) Then
                Return False
            End If

            If Regex.IsMatch(oContext.Request.Url.OriginalString, INVALID_REQUEST_PATCH_MATCH_PATTERN, RegexOptions.IgnoreCase) Then
                Return False
            End If

            ' get a reference to detect if the request is from DNN AJAX
            Dim strAjaxHeader As String = String.Empty
            If Not mApplication.Request.Headers(HEADER_AJAX_KEY) Is Nothing Then
                strAjaxHeader = mApplication.Request.Headers(HEADER_AJAX_KEY).ToString()
            End If

            If String.Equals(strAjaxHeader, HEADER_AJAX_VALUE) Then
                Return False
            End If

            Return True

        End Function

#End Region

#Region " Implementations "

        Public Sub Init(application As HttpApplication) Implements IHttpModule.Init

            ' Wire up beginrequest
            AddHandler application.BeginRequest, AddressOf Me.Application_BeginRequest

            ' Save the application
            mApplication = application

        End Sub

        Public Sub Dispose() Implements IHttpModule.Dispose

        End Sub

#End Region

        Public NotInheritable Class OpenGraphStream
            Inherits Stream

#Region " Constants "

            Private Const HEAD_END_TAG As String = "</head>"
            'Private Const MATCH_TAG_PATTERN As String = "<meta\sproperty=""{0}""\scontent=""[\w\.\s:|]+""\s?/>"
            Private Const ERROR_HEAD_TAG As String = "<!-- The Open Graph Protocol tag module threw an error --></head>"
            Private Const IMAGE_MATCH As String = "^FileID=\d+$"
            Private Const IMAGE_ID_MATCH As String = "\d+"
            Private Const HTTP_PREFIX As String = "http://"
            Private Const TABID_MATCH As String = "^\?TabID=(\d+).*"

            Private Const TITLE_ELEMENT As String = "Title"
            Private Const TITLE_OG_TAG_FORMAT As String = "<meta property=""og:title"" content=""{0}""/>"
            Private Const URL_ELEMENT As String = "Url"
            Private Const URL_OG_TAG_FORMAT As String = "<meta property=""og:url"" content=""{0}""/>"
            Private Const IMAGE_ELEMENT As String = "Image"
            Private Const IMAGE_OG_TAG_FORMAT As String = "<meta property=""og:image"" content=""{0}""/>"
            Private Const DESCRIPTION_ELEMENT As String = "Description"
            Private Const DESCRIPTION_OG_TAG_FORMAT As String = "<meta property=""og:description"" content=""{0}""/>"
            Private Const LOCALE_ELEMENT As String = "Locale"
            Private Const LOCALE_OG_TAG_FORMAT As String = "<meta property=""og:locale"" content=""{0}""/>"
            Private Const SITENAME_ELEMENT As String = "SiteName"
            Private Const SITENAME_OG_TAG_FORMAT As String = "<meta property=""og:site_name"" content=""{0}""/>"
            Private Const TYPE_ELEMENT As String = "Type"
            Private Const TYPE_OG_TAG_FORMAT As String = "<meta property=""og:type"" content=""{0}""/>"
            Private Const FACEBOOKADMINS_ELEMENT As String = "FacebookAdmins"
            Private Const FACEBOOKADMINS_OG_TAG_FORMAT As String = "<meta property=""fb:admins"" content=""{0}""/>"
            Private Const FACEBOOKAPPID_ELEMENT As String = "FacebookAppId"
            Private Const FACEBOOKAPPID_OG_TAG_FORMAT As String = "<meta property=""fb:app_id"" content=""{0}""/>"

            Private Const DEBUG_TITLE_FORMAT As String = "<div class=""dnnClear"" style=""background-color:#000000;color:#ffffff;width:inherit;font-weight:bold;"">{0}</div>"
            Private Const DEBUG_SUCCESS_FORMAT As String = "<div class=""dnnClear"" style=""background-color:#000000;color:#00ff00;width:inherit;"">{0}</div>"
            Private Const DEBUG_STATUS_FORMAT As String = "<div class=""dnnClear"" style=""background-color:#000000;color:#FF8C00;width:inherit;"">{0}</div>"
            Private Const DEBUG_ERROR_FORMAT As String = "<div class=""dnnClear"" style=""background-color:#000000;color:#ff0000;width:inherit;"">{0}</div>"

            Private Const ATTRIBUTE_DEBUG As String = "debug"
            Private Const ATTRIBUTE_PORTALID As String = "portalId"
            Private Const ATTRIBUTE_HTTPALIAS As String = "httpAlias"
            Private Const ELEMENT_HOMEURL As String = "homeUrl"
            Private Const ELEMENT_OGCONFIG As String = "ogConfig"
            Private Const ELEMENT_OGSETTINGS As String = "ogSettings"
            Private Const ELEMENT_SITE As String = "Site"
            Private Const ELEMENT_TABID As String = "TabId"
            Private Const ELEMENT_HTTPALIAS As String = "HttpAlias"

#End Region

#Region " Private Members "

            Private ReadOnly currentApp As HttpApplication

            '  The stream to the client
            Private moStream As Stream
            ' Used to track properties not supported by the client stream
            Private mlLength As Long
            Private mlPosition As Long
            ' An easy way to write a stream to the client
            Private mSR As StreamWriter

            Private p_DebugText As String = String.Empty

#End Region

#Region " Constructors "

            Public Sub New(stream As System.IO.Stream, ByVal CurrentApplication As HttpApplication)

                ' Save of the stream back to the client
                moStream = stream

                ' Create a streamwriter for later use
                mSR = New StreamWriter(moStream)

                currentApp = CurrentApplication

            End Sub

#End Region

#Region " Properties "

            Public Overrides ReadOnly Property CanRead() As Boolean
                Get
                    Return False
                End Get
            End Property

            Public Overrides ReadOnly Property CanSeek() As Boolean
                Get
                    Return True
                End Get
            End Property

            Public Overrides ReadOnly Property CanWrite() As Boolean
                Get
                    Return True
                End Get
            End Property

            Public Overrides ReadOnly Property Length() As Long
                Get
                    Return mlLength
                End Get
            End Property

            Public Overrides Property Position() As Long
                Get
                    Return mlPosition
                End Get
                Set(value As Long)
                    mlPosition = value
                End Set
            End Property

#End Region

#Region " Helper Methods "

            Public Overrides Function Read(ByVal buffer As [Byte](), ByVal offset As Integer, ByVal count As Integer) As Integer
                Throw New NotSupportedException()
            End Function

            Public Overrides Function Seek(ByVal offset As Long, ByVal direction As System.IO.SeekOrigin) As Long
                Return moStream.Seek(offset, direction)
            End Function

            Public Overrides Sub SetLength(ByVal length As Long)
                mlLength = length
            End Sub

            Public Overrides Sub Close()
                moStream.Close()
            End Sub

            Public Overrides Sub Flush()
                moStream.Flush()
            End Sub

#End Region

            ''' <summary>
            ''' Called by the parent OpenGraphWriter class to parse the HTML markup for the page being rendered
            ''' </summary>
            ''' <param name="buffer"></param>
            ''' <param name="offset"></param>
            ''' <param name="count"></param>
            ''' <remarks></remarks>
            Public Overrides Sub Write(ByVal buffer As Byte(), ByVal offset As Integer, ByVal count As Integer)

                Dim blnDebug As Boolean = False
                Dim utf8 As New UTF8Encoding()
                ' Get the string into a stringbuilder
                Dim sbBuffer As New StringBuilder(utf8.GetString(buffer))
                Dim ctlOpenGraph As New OpenGraphController

                Try

                    '-------------------------------------------------------------------------------'
                    '
                    ' HOST CONFIGURATION
                    ' get a reference to the host config
                    '
                    '-------------------------------------------------------------------------------'
                    Dim strHostConfigPath As String = Hosting.HostingEnvironment.MapPath(OpenGraphController.HOST_CONFIGURATION_PATH)

                    ' check to see if the configuration file exists
                    If Not File.Exists(strHostConfigPath) Then
                        'sbBuffer.Replace(HEAD_END_TAG, "<!-- Open Graph Protocol Module Error: Installation config file is missing -->")
                        FlushResults(sbBuffer, p_DebugText)
                        Exit Sub
                    End If

                    ' load the host configuration
                    Dim xConfigDoc As XDocument = ctlOpenGraph.GetConfigurationFile(OpenGraphController.HOST_CONFIGURATION_CACHE_KEY, strHostConfigPath)

                    ' get a reference to the current domain name
                    Dim strCurrentDomain As String = currentApp.Request.Url.Host

                    ' get the config for this module instance
                    Dim oModuleConfig As XElement = (From xconfig In xConfigDoc.Descendants(ELEMENT_OGCONFIG)
                            Select xconfig).FirstOrDefault

                    ' get the config for this website (portal)
                    Dim oSiteConfig As XElement = (From xconfig In xConfigDoc.Descendants(ELEMENT_SITE)
                            Where xconfig.Attribute(ATTRIBUTE_HTTPALIAS).Value = strCurrentDomain
                            Select xconfig).FirstOrDefault


                    ' get a reference to know if debugging will be done
                    If Not oSiteConfig.Attribute(ATTRIBUTE_DEBUG) Is Nothing Then
                        blnDebug = Boolean.Parse(oSiteConfig.Attribute(ATTRIBUTE_DEBUG).Value)
                    End If

                    If blnDebug Then
                        p_DebugText = String.Format(DEBUG_TITLE_FORMAT, "OPEN GRAPH MODULE DEBUG LOG")
                        p_DebugText = String.Concat(p_DebugText, String.Format(DEBUG_TITLE_FORMAT, "- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -"))
                        p_DebugText = String.Concat(p_DebugText, String.Format(DEBUG_STATUS_FORMAT, String.Concat("DotNetNuke Version: ", Application.DotNetNukeContext.Current.Application.Version)))
                        If Not oModuleConfig Is Nothing AndAlso Not oModuleConfig.Attribute("version") Is Nothing Then
                            p_DebugText = String.Concat(p_DebugText, String.Format(DEBUG_STATUS_FORMAT, String.Concat("OGP Module Version: ", oModuleConfig.Attribute("version").Value)))
                        Else
                            p_DebugText = String.Concat(p_DebugText, "OGP Module Version: Unable to determine the module version")
                        End If
                        p_DebugText = String.Concat(p_DebugText, String.Format(DEBUG_TITLE_FORMAT, "- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -"))
                        p_DebugText = String.Concat(p_DebugText, String.Format(DEBUG_SUCCESS_FORMAT, "Site Config Exists:  TRUE"))
                    End If


                    ' no need to continue if this site doesn't have a configuration saved
                    If oSiteConfig Is Nothing Then
                        If blnDebug Then
                            p_DebugText = String.Concat(p_DebugText, String.Format(DEBUG_ERROR_FORMAT, "Site Exists in Config:  FALSE"))
                        End If
                        FlushResults(sbBuffer, p_DebugText)
                        Exit Sub
                    Else
                        If blnDebug Then
                            p_DebugText = String.Concat(p_DebugText, String.Format(DEBUG_SUCCESS_FORMAT, "Site Exists in Config:  TRUE"))
                        End If
                    End If

                    '-------------------------------------------------------------------------------'
                    '
                    ' PAGE CONFIGURATION
                    ' get a reference to the page's Open Graph Protocol Config
                    '
                    '-------------------------------------------------------------------------------'
                    Dim strPortalId As String = oSiteConfig.Attribute(ATTRIBUTE_PORTALID).Value
                    Dim strTabId As String = Regex.Match(currentApp.Request.Url.Query, TABID_MATCH, RegexOptions.IgnoreCase).Groups(1).Value
                    
                    '
                    ' There is a known error here that will occur with the homepage when accessed using a root domain address
                    ' Example:  http://domain.com  vs.  http://domain.com/Home.aspx
                    ' This should be fixed in a future release - either through the new URL provider and/or a fallback site config for all pages.
                    '
                    ' Going to just assume any page without a tabid is the homepage for now and treat it as such.
                    ' This will have the added benefit of having a "default" set of OGP data for errored pages.
                    '
                    If String.IsNullOrEmpty(strTabId) Then
                        Dim oPortalSettings As PortalSettings = New PortalSettings(Integer.Parse(strPortalId, NumberStyles.Integer))
                        strTabId = oPortalSettings.HomeTabId.ToString()
                    End If

                    'If String.IsNullOrEmpty(strTabId) Then
                    '    ' there is likely a host config file present, but the module hasn't yet been set-up or...
                    '    If blnDebug Then
                    '        p_DebugText = String.Concat(p_DebugText, String.Format(DEBUG_ERROR_FORMAT, "ERROR: The TabId was not found!"))
                    '    End If
                    '    FlushResults(sbBuffer, p_DebugText)
                    '    Exit Sub
                    'End If

                    Dim strPageConfigPath As String = Hosting.HostingEnvironment.MapPath(OpenGraphController.GetOgpPageConfigurationPath(strPortalId, strTabId))
                    Dim strPageCacheKey As String = OpenGraphController.GetOgpPageCacheKey(strPortalId, strTabId)


                    If blnDebug Then
                        p_DebugText = String.Concat(p_DebugText, String.Format(DEBUG_STATUS_FORMAT, String.Concat("Site Config PortalId: ", strPortalId)))
                        p_DebugText = String.Concat(p_DebugText, String.Format(DEBUG_STATUS_FORMAT, String.Concat("Request TabId: ", strTabId)))
                        p_DebugText = String.Concat(p_DebugText, String.Format(DEBUG_STATUS_FORMAT, String.Concat("Page Configuration Path: ", strPageConfigPath)))
                    End If


                    ' check to see if the configuration file exists
                    If Not File.Exists(strPageConfigPath) Then
                        If blnDebug Then
                            p_DebugText = String.Concat(p_DebugText, String.Format(DEBUG_ERROR_FORMAT, "Page Config Path Exists:  FALSE"))
                        End If
                        ' no need to continue if the configuration file isn't found
                        FlushResults(sbBuffer, p_DebugText)
                        Exit Sub
                    Else
                        If blnDebug Then
                            p_DebugText = String.Concat(p_DebugText, String.Format(DEBUG_SUCCESS_FORMAT, "Page Config Path Exists:  TRUE"))
                        End If
                    End If


                    ' load the page configuration 
                    Dim xPage As XDocument = ctlOpenGraph.GetConfigurationFile(strPageCacheKey, strPageConfigPath)

                    ' get the current URL from the other config file
                    Dim oPageConfig As XElement = (From xconfig In xPage.Descendants(ELEMENT_OGSETTINGS)
                            Select xconfig).FirstOrDefault

                    Dim strUrl As String = oPageConfig.Element("Url").Value
                    Dim intPortalId As Integer = Integer.Parse(strPortalId, NumberStyles.Integer)
                    Dim intTabId As Integer = Integer.Parse(oPageConfig.Element(ELEMENT_TABID).Value, NumberStyles.Integer)
                    Dim strHomeUrl As String = String.Empty
                    Dim ctlPage As New Entities.Tabs.TabController
                    Dim oTab As Entities.Tabs.TabInfo = ctlPage.GetTab(intTabId, intPortalId, False)

                    ' get a localized version of the URL
                    Dim strMlUrl As String = strUrl.Replace(String.Concat(strCurrentDomain, "/"), String.Concat(strCurrentDomain, "/", Threading.Thread.CurrentThread.CurrentCulture.Name, "/"))

                    If Not oSiteConfig.Element(ELEMENT_HOMEURL) Is Nothing Then
                        strHomeUrl = oSiteConfig.Element(ELEMENT_HOMEURL).Value
                    End If


                    If blnDebug Then
                        p_DebugText = String.Concat(p_DebugText, String.Format(DEBUG_STATUS_FORMAT, String.Concat("Page Config TabId: ", intTabId.ToString())))
                        p_DebugText = String.Concat(p_DebugText, String.Format(DEBUG_STATUS_FORMAT, String.Concat("Page Config URL: ", strUrl)))
                        p_DebugText = String.Concat(p_DebugText, String.Format(DEBUG_STATUS_FORMAT, String.Concat("Homepage URL: ", strHomeUrl)))
                        p_DebugText = String.Concat(p_DebugText, String.Format(DEBUG_STATUS_FORMAT, String.Concat("Localized URL: ", strMlUrl)))
                    End If


                    ' only proceed if this is the page that the module is on
                    If IsInvalidPageRequest(strUrl, oTab.FullUrl, strHomeUrl, strMlUrl) Then
                        If blnDebug Then
                            p_DebugText = String.Concat(p_DebugText, String.Format(DEBUG_ERROR_FORMAT, "Page/Request URL Match: FALSE"))
                        End If
                        ' no need to continue if this page doesn't match the config
                        FlushResults(sbBuffer, p_DebugText)
                        Exit Sub
                    Else
                        If blnDebug Then
                            p_DebugText = String.Concat(p_DebugText, String.Format(DEBUG_SUCCESS_FORMAT, "Page/Request URL Match: TRUE"))
                        End If
                    End If

                    ' this may be implemented in a future release
                    'Dim blnReplace As Boolean = Boolean.Parse(oElement.Attribute("OverrideOpenGraph").Value)

                    '-------------------------------------------------------------------------------'
                    '
                    ' INJECT OPEN GRAPH PROTOCOL
                    ' add the open graph protocol tags to the page header
                    '
                    '-------------------------------------------------------------------------------'
                    If Not xPage Is Nothing Then

                        If blnDebug Then
                            p_DebugText = String.Concat(p_DebugText, String.Format(DEBUG_TITLE_FORMAT, "- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -"))
                            p_DebugText = String.Concat(p_DebugText, String.Format(DEBUG_TITLE_FORMAT, "BEGIN INJECTING OPEN GRAPH PROTOCOL"))
                        End If

                        Dim strMarkUpCacheKey As String = OpenGraphController.GetOgpPageMarkUpCacheKey(intPortalId, intTabId)

                        ' load an object with a cached reference to the OGP markup
                        Dim oContent As Object = DataCache.GetCache(strMarkUpCacheKey)

                        If Not oContent Is Nothing Then

                            ' use the cached OGP data
                            sbBuffer.Replace(HEAD_END_TAG, oContent.ToString())

                        Else

                            ' string object to send back to the page
                            Dim sbReplacement As New StringBuilder

                            ' create the Open Graph Protocol tags
                            Dim pageElement As XElement = (From xconfig In xPage.Descendants(ELEMENT_OGSETTINGS)
                                    Select xconfig).FirstOrDefault

                            ' this method will take care of the OGP injection
                            InjectOpenGraphProtocol(sbReplacement, pageElement, oPageConfig.Element(ELEMENT_HTTPALIAS).Value, blnDebug)


                            If blnDebug Then
                                p_DebugText = String.Concat(p_DebugText, String.Format(DEBUG_TITLE_FORMAT, "- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -"))
                            End If


                            ' append a </head> tag for the replacement below
                            sbReplacement.Append(HEAD_END_TAG)

                            ' insert the replacement markup into the cache
                            DataCache.SetCache(strMarkUpCacheKey, sbReplacement.ToString(), DateTime.Now.AddHours(1))

                            ' throw the new OGP header into the existing markup
                            sbBuffer.Replace(HEAD_END_TAG, sbReplacement.ToString)

                        End If

                        ' Write to the stream
                        FlushResults(sbBuffer, p_DebugText)

                    End If

                Catch ex As Exception
                    LogException(ex)

                    ' write the original content back to the stream
                    sbBuffer.Replace(HEAD_END_TAG, ERROR_HEAD_TAG)
                    ' clean-up
                    FlushResults(sbBuffer, String.Format(DEBUG_ERROR_FORMAT, ex.StackTrace))

                End Try

            End Sub

            ''' <summary>
            ''' Compartmentalizes the parsing to determine is the page URL is matched
            ''' </summary>
            ''' <param name="Url">Requested URL</param>
            ''' <param name="FullUrl">Full URL from the server</param>
            ''' <param name="HomeUrl">Home page URL</param>
            ''' <param name="MultiLingualUrl">Localized URL</param>
            ''' <returns></returns>
            ''' <remarks></remarks>
            Private Function IsInvalidPageRequest(ByRef Url As String, ByRef FullUrl As String, ByRef HomeUrl As String,
                                                ByRef MultiLingualUrl As String) As Boolean

                ' returns true if this is the page that the module is on
                Return (Not String.Equals(Url.ToLower(), FullUrl.ToLower()) And _
                        Not String.Equals(Url, HomeUrl) And _
                        Not String.Equals(MultiLingualUrl.ToLower(), FullUrl.ToLower()))

            End Function

            ''' <summary>
            ''' Returns the HTML markup to the parent class, and to the requesting web browser
            ''' </summary>
            ''' <param name="Buffer"></param>
            ''' <param name="DebugText"></param>
            ''' <remarks></remarks>
            Private Sub FlushResults(ByVal Buffer As StringBuilder, ByVal DebugText As String)

                If Not String.IsNullOrEmpty(DebugText) Then
                    DebugText = String.Concat(DebugText, String.Format(DEBUG_TITLE_FORMAT, "END OF LOG"))
                    DebugText = String.Concat("<div id=""wnsOpenGraphDebug"" class=""dnnClear"" style=""background-color:#000000;position:relative;z-index:899;width:100%;padding:10px;"">", DebugText, "</div>")
                    Buffer.Replace("</body>", String.Concat(DebugText, "</body>"))
                End If

                ' Write to the stream
                mSR.Write(Buffer.ToString())
                mSR.Flush()

            End Sub

            ''' <summary>
            ''' Parses the HTML markup, the OGP settings, and inject the OGP tags into the page header
            ''' </summary>
            ''' <param name="NewMarkUp">New markup pending insertion into the page</param>
            ''' <param name="SiteElement">XElement for the Site/Page config</param>
            ''' <param name="HttpAlias">The specified domain name/alias for the site</param>
            ''' <param name="Debug">If true, debugging is turned on</param>
            ''' <remarks></remarks>
            Private Sub InjectOpenGraphProtocol(ByRef NewMarkUp As StringBuilder, ByRef SiteElement As XElement,
                                                ByRef HttpAlias As String, ByRef Debug As Boolean)

                If Not SiteElement.Element(TITLE_ELEMENT) Is Nothing AndAlso Not String.IsNullOrEmpty(SiteElement.Element(TITLE_ELEMENT).Value) Then
                    NewMarkUp.AppendFormat(TITLE_OG_TAG_FORMAT, SiteElement.Element(TITLE_ELEMENT).Value)
                    If Debug Then
                        p_DebugText = String.Concat(p_DebugText, String.Format(DEBUG_SUCCESS_FORMAT, String.Concat(TITLE_ELEMENT, ": ", SiteElement.Element(TITLE_ELEMENT).Value)))
                    End If
                End If
                If Not SiteElement.Element(URL_ELEMENT) Is Nothing AndAlso Not String.IsNullOrEmpty(SiteElement.Element(URL_ELEMENT).Value) Then
                    NewMarkUp.AppendFormat(URL_OG_TAG_FORMAT, SiteElement.Element(URL_ELEMENT).Value)
                    If Debug Then
                        p_DebugText = String.Concat(p_DebugText, String.Format(DEBUG_SUCCESS_FORMAT, String.Concat(URL_ELEMENT, ": ", SiteElement.Element(URL_ELEMENT).Value)))
                    End If
                End If
                If Not SiteElement.Element(IMAGE_ELEMENT) Is Nothing AndAlso Not String.IsNullOrEmpty(SiteElement.Element(IMAGE_ELEMENT).Value) Then
                    If Regex.IsMatch(SiteElement.Element(IMAGE_ELEMENT).Value, IMAGE_MATCH, RegexOptions.IgnoreCase) Then

                        Dim strFileId As String = Regex.Match(SiteElement.Element(IMAGE_ELEMENT).Value, IMAGE_ID_MATCH, RegexOptions.IgnoreCase).Value
                        Dim oImage As IFileInfo = FileManager.Instance().GetFile(Integer.Parse(strFileId, NumberStyles.Integer))

                        If Not oImage Is Nothing Then
                            ' only append the file tag if the file still exists
                            Dim strImagePath As String = String.Concat(HTTP_PREFIX, HttpAlias, FileManager.Instance().GetUrl(oImage))
                            NewMarkUp.AppendFormat(IMAGE_OG_TAG_FORMAT, strImagePath)
                            If Debug Then
                                p_DebugText = String.Concat(p_DebugText, String.Format(DEBUG_SUCCESS_FORMAT, String.Concat(IMAGE_ELEMENT, ": ", strImagePath)))
                            End If
                        Else
                            ' file doesn't exist any longer 
                            ' Need to delete the reference
                        End If

                    Else
                        NewMarkUp.AppendFormat(IMAGE_OG_TAG_FORMAT, SiteElement.Element(IMAGE_ELEMENT).Value)
                        If Debug Then
                            p_DebugText = String.Concat(p_DebugText, String.Format(DEBUG_SUCCESS_FORMAT, String.Concat(IMAGE_ELEMENT, ": ", SiteElement.Element(IMAGE_ELEMENT).Value)))
                        End If
                    End If
                End If

                If Not SiteElement.Element(DESCRIPTION_ELEMENT) Is Nothing AndAlso Not String.IsNullOrEmpty(SiteElement.Element(DESCRIPTION_ELEMENT).Value) Then
                    NewMarkUp.AppendFormat(DESCRIPTION_OG_TAG_FORMAT, SiteElement.Element(DESCRIPTION_ELEMENT).Value)
                    If Debug Then
                        p_DebugText = String.Concat(p_DebugText, String.Format(DEBUG_SUCCESS_FORMAT, String.Concat(DESCRIPTION_ELEMENT, ": ", SiteElement.Element(DESCRIPTION_ELEMENT).Value)))
                    End If
                End If
                If Not SiteElement.Element(LOCALE_ELEMENT) Is Nothing AndAlso Not String.IsNullOrEmpty(SiteElement.Element(LOCALE_ELEMENT).Value) Then
                    NewMarkUp.AppendFormat(LOCALE_OG_TAG_FORMAT, SiteElement.Element(LOCALE_ELEMENT).Value)
                    If Debug Then
                        p_DebugText = String.Concat(p_DebugText, String.Format(DEBUG_SUCCESS_FORMAT, String.Concat(LOCALE_ELEMENT, ": ", SiteElement.Element(LOCALE_ELEMENT).Value)))
                    End If
                End If
                If Not SiteElement.Element(SITENAME_ELEMENT) Is Nothing AndAlso Not String.IsNullOrEmpty(SiteElement.Element(SITENAME_ELEMENT).Value) Then
                    NewMarkUp.AppendFormat(SITENAME_OG_TAG_FORMAT, SiteElement.Element(SITENAME_ELEMENT).Value)
                    If Debug Then
                        p_DebugText = String.Concat(p_DebugText, String.Format(DEBUG_SUCCESS_FORMAT, String.Concat(SITENAME_ELEMENT, ": ", SiteElement.Element(SITENAME_ELEMENT).Value)))
                    End If
                End If
                If Not SiteElement.Element(TYPE_ELEMENT) Is Nothing AndAlso Not String.IsNullOrEmpty(SiteElement.Element(TYPE_ELEMENT).Value) Then
                    NewMarkUp.AppendFormat(TYPE_OG_TAG_FORMAT, SiteElement.Element(TYPE_ELEMENT).Value)
                    If Debug Then
                        p_DebugText = String.Concat(p_DebugText, String.Format(DEBUG_SUCCESS_FORMAT, String.Concat(TYPE_ELEMENT, ": ", SiteElement.Element(TYPE_ELEMENT).Value)))
                    End If
                End If
                If Not SiteElement.Element(FACEBOOKADMINS_ELEMENT) Is Nothing AndAlso Not String.IsNullOrEmpty(SiteElement.Element(FACEBOOKADMINS_ELEMENT).Value) Then
                    NewMarkUp.AppendFormat(FACEBOOKADMINS_OG_TAG_FORMAT, SiteElement.Element(FACEBOOKADMINS_ELEMENT).Value)
                    If Debug Then
                        p_DebugText = String.Concat(p_DebugText, String.Format(DEBUG_SUCCESS_FORMAT, String.Concat(FACEBOOKADMINS_ELEMENT, ": ", SiteElement.Element(FACEBOOKADMINS_ELEMENT).Value)))
                    End If
                End If
                If Not SiteElement.Element(FACEBOOKAPPID_ELEMENT) Is Nothing AndAlso Not String.IsNullOrEmpty(SiteElement.Element(FACEBOOKAPPID_ELEMENT).Value) Then
                    NewMarkUp.AppendFormat(FACEBOOKAPPID_OG_TAG_FORMAT, SiteElement.Element(FACEBOOKAPPID_ELEMENT).Value)
                    If Debug Then
                        p_DebugText = String.Concat(p_DebugText, String.Format(DEBUG_SUCCESS_FORMAT, String.Concat(FACEBOOKAPPID_ELEMENT, ": ", SiteElement.Element(FACEBOOKAPPID_ELEMENT).Value)))
                    End If
                End If

            End Sub

        End Class

    End Class

End Namespace