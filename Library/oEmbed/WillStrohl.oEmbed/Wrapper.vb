'
' Will Strohl (will.strohl@gmail.com)
' http://www.willstrohl.com
'
'Copyright (c) 2009-2014, Will Strohl
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
'Neither the name of Will Strohl nor the names of its contributors may be 
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

Imports Newtonsoft.Json
Imports System.Text.RegularExpressions
Imports System.Web
Imports WillStrohl.API.oEmbed.Constants
Imports WillStrohl.API.oEmbed.Providers

Namespace WillStrohl.API.oEmbed

    ''' <summary>
    ''' Wrapper - this is the main class to use for any provider that is not provided in this API.
    ''' </summary>
    ''' <remarks></remarks>
    Public NotInheritable Class Wrapper
        Inherits Providers.ProviderBase

#Region " Private Members "

        Private Const URL_FORMAT As String = "?url={0}&format={1}"
        Private Const FORMAT_PATTERN As String = "\{format\}"

#End Region

        Public Function GetContent(ByVal Request As RequestInfo) As String

            Dim ctlProvider As New ProviderFormat

            If ctlProvider.IsUrlSupported(Request.URL) Then

                Dim strProvider As String = ctlProvider.ProviderToUse(Request.URL)
                Console.WriteLine(String.Concat("PROVIDER: ", strProvider))

                Select Case (strProvider)
                    Case PROVIDER_CLEARSPRINGWIDGETS
                        Dim ctlClearspring As New ClearspringWidgets
                        Return ctlClearspring.GetRichContent(Request.URL)
                    Case PROVIDER_EMBEDLY
                        Dim ctlEmbedly As New Embedly
                        Return ctlEmbedly.GetRichContent(Request.URL)
                    Case PROVIDER_FLICKR
                        Dim ctlFlickr As New Flickr
                        Return ctlFlickr.GetPhoto(Request.URL, Request.MaxWidth, Request.MaxHeight)
                    Case PROVIDER_HULU
                        Dim ctlHulu As New Hulu
                        Return ctlHulu.GetVideo(Request.URL)
                    Case PROVIDER_MYOPERA
                        Dim ctlMyOpera As New MyOpera
                        Return ctlMyOpera.GetRichContent(Request.URL)
                    Case PROVIDER_OOHEMBED
                        Dim ctlOohEmbed As New oohEmbed
                        Return ctlOohEmbed.GetRichContent(Request.URL)
                    Case PROVIDER_POLLEVERYWHERE
                        Dim ctlPollEverywhere As New PollEverywhere
                        Return ctlPollEverywhere.GetRichContent(Request.URL)
                    Case PROVIDER_QIK
                        Dim ctlQik As New Qik
                        Return ctlQik.GetVideo(Request.URL)
                    Case PROVIDER_REVISION3
                        Dim ctlRevision3 As New Revision3
                        Return ctlRevision3.GetVideo(Request.URL)
                    Case PROVIDER_VIDDLER
                        Dim ctlViddler As New Viddler
                        Return ctlViddler.GetVideo(Request.URL, Request.MaxWidth, Request.MaxHeight)
                    Case PROVIDER_VIMEO
                        Dim ctlVimeo As New Vimeo
                        Return ctlVimeo.GetVideo(Request.URL)
                    Case PROVIDER_YOUTUBE
                        Dim ctlYouTube As New YouTube
                        Return ctlYouTube.GetVideo(Request.URL)
                    Case Else
                        Throw New ArgumentOutOfRangeException("URL", "The specified URL is not supported by any existing OEmbed provider")
                End Select

            Else

                Throw New ArgumentOutOfRangeException("URL", "The specified URL is not supported by any existing OEmbed provider")

            End If

        End Function

        ''' <summary>
        ''' GetPhotoContent - this method makes a call to a photo oEmbed provider, and returns the image URL that you request
        ''' </summary>
        ''' <param name="EndPoint">String - the endpoint as specified by the oEmbed provider</param>
        ''' <param name="Request">RequestInfo - the request object required to pass information into the EndPoint URL</param>
        ''' <returns>String - the image URL to embed into your content</returns>
        ''' <remarks></remarks>
        Public Overloads Function GetPhotoContent(ByVal EndPoint As String, ByVal Request As RequestInfo) As String
            Return GetPhotoContent(EndPoint, Request, Nothing)
        End Function

        ''' <summary>
        ''' GetPhotoContent - this method makes a call to a photo oEmbed provider, and returns the image URL that you request
        ''' </summary>
        ''' <param name="EndPoint">String - the endpoint as specified by the oEmbed provider</param>
        ''' <param name="Request">RequestInfo - the request object required to pass information into the EndPoint URL</param>
        ''' <param name="Args">String() - optional arguments that can be appended to the end of the oEmbed GET request URL</param>
        ''' <returns>String - the image URL to embed into your content</returns>
        ''' <remarks></remarks>
        Public Overloads Function GetPhotoContent(ByVal EndPoint As String, ByVal Request As RequestInfo, ByVal ParamArray Args() As String) As String

            If String.IsNullOrEmpty(EndPoint) Then
                Throw New ArgumentNullException("EndPoint")
            End If

            If Request Is Nothing Then
                Throw New ArgumentNullException("Request")
            Else
                If String.IsNullOrEmpty(Request.URL) Then
                    Throw New ArgumentNullException("Request.URL")
                End If
                If Not Request.MaxHeight > NULL_INTEGER Then
                    Throw New ArgumentNullException("Request.MaxHeight")
                End If
                If Not Request.MaxWidth > NULL_INTEGER Then
                    Throw New ArgumentNullException("Request.MaxWidth")
                End If
            End If

            Dim strUrl As String = GetParsedURL(EndPoint, Request, Args)

            ' make the request
            Dim ctlRequest As New RequestController
            'Dim strResponse As String = ctlRequest.GetRemoteWebContent(strUrl, UseSSL(strUrl))
            Dim strResponse As String = ctlRequest.GetOEmbedContent(strUrl)

            ' parse the response
            Dim objPhoto As New PhotoInfo
            objPhoto = CType(JsonConvert.DeserializeObject(strResponse, objPhoto.GetType), PhotoInfo)

            ' return the content
            Return objPhoto.Url

        End Function


        ''' <summary>
        ''' GetVideoContent - this method makes a call to a video oEmbed provider, and returns the video markup that you request
        ''' </summary>
        ''' <param name="EndPoint">String - the endpoint as specified by the oEmbed provider</param>
        ''' <param name="Request">RequestInfo - the request object required to pass information into the EndPoint URL</param>
        ''' <returns>String - the video markup to embed into your content</returns>
        ''' <remarks></remarks>
        Public Overloads Function GetVideoContent(ByVal EndPoint As String, ByVal Request As RequestInfo) As String
            Return GetVideoContent(EndPoint, Request, Nothing)
        End Function

        ''' <summary>
        ''' GetVideoContent - this method makes a call to a video oEmbed provider, and returns the video markup that you request
        ''' </summary>
        ''' <param name="EndPoint">String - the endpoint as specified by the oEmbed provider</param>
        ''' <param name="Request">RequestInfo - the request object required to pass information into the EndPoint URL</param>
        ''' <param name="Args">String() - optional arguments that can be appended to the end of the oEmbed GET request URL</param>
        ''' <returns>String - the video markup to embed into your content</returns>
        ''' <remarks></remarks>
        Public Overloads Function GetVideoContent(ByVal EndPoint As String, ByVal Request As RequestInfo, ByVal ParamArray Args() As String) As String

            If String.IsNullOrEmpty(EndPoint) Then
                Throw New ArgumentNullException("EndPoint")
            End If

            If Request Is Nothing Then
                Throw New ArgumentNullException("Request")
            Else
                If String.IsNullOrEmpty(Request.URL) Then
                    Throw New ArgumentNullException("Request.URL")
                End If
                If Not Request.MaxHeight > NULL_INTEGER Then
                    Throw New ArgumentNullException("Request.MaxHeight")
                End If
                If Not Request.MaxWidth > NULL_INTEGER Then
                    Throw New ArgumentNullException("Request.MaxWidth")
                End If
            End If

            Dim strUrl As String = GetParsedURL(EndPoint, Request, Args)

            ' make the request
            Dim ctlRequest As New RequestController
            Dim strResponse As String = ctlRequest.GetOEmbedContent(strUrl)

            ' parse the response
            Dim objVideo As New VideoInfo
            objVideo = CType(JsonConvert.DeserializeObject(strResponse, objVideo.GetType), VideoInfo)

            ' return the content
            Return objVideo.Html

        End Function


        ''' <summary>
        ''' GetRichContent - this method makes a call to a rich content oEmbed provider, and returns the rich content markup that you request
        ''' </summary>
        ''' <param name="EndPoint">String - the endpoint as specified by the oEmbed provider</param>
        ''' <param name="Request">RequestInfo - the request object required to pass information into the EndPoint URL</param>
        ''' <returns>String - the rich content markup to embed into your content</returns>
        ''' <remarks></remarks>
        Public Overloads Function GetRichContent(ByVal EndPoint As String, ByVal Request As RequestInfo) As String
            Return GetRichContent(EndPoint, Request, Nothing)
        End Function

        ''' <summary>
        ''' GetRichContent - this method makes a call to a rich content oEmbed provider, and returns the rich content markup that you request
        ''' </summary>
        ''' <param name="EndPoint">String - the endpoint as specified by the oEmbed provider</param>
        ''' <param name="Request">RequestInfo - the request object required to pass information into the EndPoint URL</param>
        ''' <param name="Args">String() - optional arguments that can be appended to the end of the oEmbed GET request URL</param>
        ''' <returns>String - the rich content markup to embed into your content</returns>
        ''' <remarks></remarks>
        Public Overloads Function GetRichContent(ByVal EndPoint As String, ByVal Request As RequestInfo, ByVal ParamArray Args() As String) As String

            If String.IsNullOrEmpty(EndPoint) Then
                Throw New ArgumentNullException("EndPoint")
            End If

            If Request Is Nothing Then
                Throw New ArgumentNullException("Request")
            Else
                If String.IsNullOrEmpty(Request.URL) Then
                    Throw New ArgumentNullException("Request.URL")
                End If
            End If

            Dim strUrl As String = GetParsedURL(EndPoint, Request, Args)

            ' make the request
            Dim ctlRequest As New RequestController
            Dim strResponse As String = ctlRequest.GetOEmbedContent(strUrl)

            ' parse the response
            Dim objRich As New RichInfo
            objRich = CType(JsonConvert.DeserializeObject(strResponse, objRich.GetType), RichInfo)

            ' return the content
            Return objRich.Html

        End Function


#Region " Private Helper Methods "

        ''' <summary>
        ''' GetParsedURL - this method parses the URL and makes the necessary updates to the URL to make it oEmbed compatible.
        ''' </summary>
        ''' <param name="EndPoint">String - the endpoint as specified by the oEmbed provider</param>
        ''' <param name="Request">RequestInfo - the request object required to pass information into the EndPoint URL</param>
        ''' <param name="Args">String() - optional arguments that can be appended to the end of the oEmbed GET request URL</param>
        ''' <returns></returns>
        ''' <remarks>
        ''' If you use the Args() argument, you need to specify it in the following format: 
        ''' Args = {"&amp;arg1=value1&amp;arg2=value2&amp;arg3=value3"}
        ''' </remarks>
        Private Function GetParsedURL(ByVal EndPoint As String, ByVal Request As RequestInfo, ByVal ParamArray Args() As String) As String

            Dim strUrl As String = NULL_STRING

            ' check to see if the url has the format as part of the url itself
            If Regex.IsMatch(EndPoint, FORMAT_PATTERN) Then

                strUrl = Regex.Replace(EndPoint, FORMAT_PATTERN, Request.Format, RegexOptions.IgnoreCase)

                If strUrl.Contains("?") Then
                    strUrl = String.Concat(strUrl, String.Concat("&url=", HttpUtility.UrlEncode(Request.URL)))
                Else
                    strUrl = String.Concat(strUrl, String.Concat("?url=", HttpUtility.UrlEncode(Request.URL)))
                End If

            Else
                ' append the format and target url
                strUrl = String.Concat(EndPoint, String.Format(URL_FORMAT, HttpUtility.UrlEncode(Request.URL), Request.Format))
            End If

            ' iterate through the optional arguments and add them
            If Not Args Is Nothing AndAlso Args.Length > 0 Then
                For Each strObj As String In Args
                    strUrl = String.Concat(strUrl, strObj)
                Next
            End If

            Return strUrl

        End Function

#End Region

    End Class

End Namespace