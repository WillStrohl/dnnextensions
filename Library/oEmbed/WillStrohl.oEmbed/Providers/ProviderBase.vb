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

Imports System.Text.RegularExpressions
Imports WillStrohl.API.oEmbed.Constants

Namespace WillStrohl.API.oEmbed.Providers

    ''' <summary>
    ''' ProviderBase - this is the base class for all oEmbed providers
    ''' </summary>
    ''' <remarks></remarks>
    Public MustInherit Class ProviderBase

#Region " Private Members "

        Private p_Proxy As String = NULL_STRING
        Private p_Port As Integer = NULL_INTEGER

#End Region

#Region " Properties "

        ''' <summary>
        ''' Proxy - the web address for an Internet proxy to use for oEmbed requests
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Protected ReadOnly Property Proxy() As String
            Get
                Return Me.p_Proxy
            End Get
        End Property

        ''' <summary>
        ''' Port - the http port number for an Internet proxy to use for oEmbed requests
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Protected ReadOnly Property Port() As Integer
            Get
                Return Me.p_Port
            End Get
        End Property

        ''' <summary>
        ''' HasProxy - this property specifies if the proxy information has been properly assigned
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Protected ReadOnly Property HasProxy() As Boolean
            Get
                Return (Not String.IsNullOrEmpty(Me.Proxy) And Me.Port > NULL_INTEGER)
            End Get
        End Property

#End Region

#Region " Instantiation "

        ''' <summary>
        ''' Instantiates the object (without using an internet proxy)
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New()
            ' do nothing
        End Sub

        ''' <summary>
        ''' Instantiates the object (using an internet proxy)
        ''' </summary>
        ''' <param name="ProxyAddress">String - the URL of a proxy server to use for oEmbed requests</param>
        ''' <param name="ProxyPort">Integer - the http port to use for the proxy server</param>
        ''' <remarks></remarks>
        Public Sub New(ByVal ProxyAddress As String, ByVal ProxyPort As Integer)
            If ProxyPort > NULL_INTEGER Then
                Me.p_Port = ProxyPort
            End If
            If Not String.IsNullOrEmpty(ProxyAddress) Then
                Me.p_Proxy = ProxyAddress
            End If
        End Sub

#End Region

        ''' <summary>
        ''' UseSSL - this method returns a boolean status telling if the URL is an SSL one or not
        ''' </summary>
        ''' <param name="URL"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Protected Function UseSSL(ByVal URL As String) As Boolean
            Return Regex.IsMatch(URL, SSL_PATTERN, RegexOptions.IgnoreCase)
        End Function

    End Class

End Namespace