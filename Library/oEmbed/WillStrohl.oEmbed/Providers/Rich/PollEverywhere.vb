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
Imports System.Web
Imports WillStrohl.API.oEmbed.Constants

Namespace WillStrohl.API.oEmbed.Providers

    ''' <summary>
    ''' This is the implementation wrapper for the Poll Everywhere oEmbed provider
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable()> _
    Public NotInheritable Class PollEverywhere
        Inherits ProviderBase
        Implements IRichProvider, IPollEverywhere, IEmbed

#Region " Instantiation "

        ''' <summary>
        ''' Instantiates the object (without using an internet proxy)
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New()
            MyBase.New()
        End Sub

        ''' <summary>
        ''' Instantiates the object (using an internet proxy)
        ''' </summary>
        ''' <param name="ProxyAddress">String - the URL of a proxy server to use for oEmbed requests</param>
        ''' <param name="ProxyPort">Integer - the http port to use for the proxy server</param>
        ''' <remarks></remarks>
        Public Sub New(ByVal ProxyAddress As String, ByVal ProxyPort As Integer)
            MyBase.New(ProxyAddress, ProxyPort)
        End Sub

#End Region

        ''' <summary>
        ''' GetRichContentObject - this method will return the rich content to be embedded into your content
        ''' </summary>
        ''' <param name="URL">The source URL of the image. Consumers should be able to insert this URL into an &lt;img&gt; element. Only HTTP and HTTPS URLs are valid.</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetRichContent(ByVal URL As String) As String Implements IRichProvider.GetRichContent
            Return GetRichContentObject(URL).Html
        End Function

        ''' <summary>
        ''' GetRichContentObject - this method will return the rich content oEmbed object that is returned from the provider
        ''' </summary>
        ''' <param name="URL">The source URL of the image. Consumers should be able to insert this URL into an &lt;img&gt; element. Only HTTP and HTTPS URLs are valid.</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetRichContentObject(ByVal URL As String) As RichInfo Implements IRichProvider.GetRichContentObject

            If String.IsNullOrEmpty(URL) Then
                Throw New ArgumentNullException("URL")
            End If

            Dim strUrl As String = String.Format(YOUTUBE_FORMAT, HttpUtility.UrlEncode(URL))

            Dim ctlRequest As New RequestController
            Dim strResponse As String = NULL_STRING
            If Me.HasProxy Then
                strResponse = ctlRequest.GetOEmbedContent(strUrl, Me.Proxy, Me.Port)
            Else
                strResponse = ctlRequest.GetOEmbedContent(strUrl)
            End If

            Dim objRich As New RichInfo
            objRich = CType(JsonConvert.DeserializeObject(strResponse, objRich.GetType), RichInfo)

            Return objRich

        End Function

#Region " IEmbed Implementation "

        Public Function IsProviderActive() As Boolean Implements IEmbed.IsProviderActive
            Return True
        End Function

        Public Function IsProviderProxy() As Boolean Implements IEmbed.IsProviderProxy
            Return False
        End Function

#End Region

    End Class

    Public Interface IPollEverywhere

    End Interface

End Namespace