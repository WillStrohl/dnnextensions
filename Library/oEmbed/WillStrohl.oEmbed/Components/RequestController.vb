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

Imports System.IO
Imports System.Net
Imports WillStrohl.API.oEmbed.Constants

Namespace WillStrohl.API.oEmbed

    ''' <summary>
    ''' RequestController - this class handles all of the remote GET requests centrally for all oEmbed types.
    ''' </summary>
    ''' <remarks>
    ''' This class is not inheritable.
    ''' </remarks>
    Public NotInheritable Class RequestController

        ''' <summary>
        ''' GetOEmbedContent - this method makes a GET request to the oEmbed provider, and returns their response.
        ''' </summary>
        ''' <param name="RequestUrl">String - this is the oEmbed formatted URL to call</param>
        ''' <returns></returns>
        ''' <remarks>
        ''' If an error occurs, the error message will be returned in the "title" parameter of the oEmbed object.
        ''' </remarks>
        Public Overloads Function GetOEmbedContent(ByVal RequestUrl As String) As String
            Return GetOEmbedContent(RequestUrl, NULL_STRING, NULL_INTEGER)
        End Function

        ''' <summary>
        ''' GetOEmbedContent - this method makes a GET request to the oEmbed provider, and returns their response.
        ''' </summary>
        ''' <param name="RequestUrl">String - this is the oEmbed formatted URL to call</param>
        ''' <param name="ProxyAddress">String - this is the URL for the internet proxy to use for the oEmbed request</param>
        ''' <param name="ProxyPort">Integer - this is the http port for the internet proxy to use for the oEmbed request</param>
        ''' <returns></returns>
        ''' <remarks>
        ''' If an error occurs, the error message will be returned in the "title" parameter of the oEmbed object.
        ''' </remarks>
        Public Overloads Function GetOEmbedContent(ByVal RequestUrl As String, ByVal ProxyAddress As String, ByVal ProxyPort As Integer) As String

            Dim strReturn As String = NULL_STRING
            Dim request As WebRequest = Nothing
            Dim response As HttpWebResponse = Nothing
            Dim dataStream As Stream = Nothing
            Dim reader As StreamReader = Nothing

            Try

                ' Create a request for the URL.         
                request = WebRequest.Create(RequestUrl)
                If Not String.IsNullOrEmpty(ProxyAddress) And ProxyPort > NULL_INTEGER Then
                    request.Proxy = New WebProxy(String.Concat(ProxyAddress, ":", ProxyPort.ToString), True)
                Else
                    request.Proxy = Nothing
                End If

                ' If required by the server, set the credentials.
                request.Credentials = CredentialCache.DefaultCredentials

                ' Get the response.
                response = CType(request.GetResponse, HttpWebResponse)

                ' Display the status.
                ' Console.WriteLine (response.StatusDescription);

                ' Get the stream containing content returned by the server.
                dataStream = response.GetResponseStream

                ' Open the stream using a StreamReader for easy access.
                reader = New StreamReader(dataStream)

                strReturn = reader.ReadToEnd

            Catch ex As Exception

                Return ApplicationError.GetAppliationErrorObject(ex.Message)

            Finally

                ' Cleanup the streams and the response.
                reader.Close()
                dataStream.Close()
                response.Close()

            End Try

            Return strReturn

        End Function

    End Class

End Namespace