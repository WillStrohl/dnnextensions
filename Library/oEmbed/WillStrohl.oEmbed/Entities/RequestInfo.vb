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

Imports System.Runtime.Serialization
Imports WillStrohl.API.oEmbed.Constants

Namespace WillStrohl.API.oEmbed

    ''' <summary>
    ''' This is the request object used to generate generic requests to oEmbed providers that are not included in this API.
    ''' </summary>
    ''' <remarks></remarks>
    <DataContract()> _
    Public NotInheritable Class RequestInfo
        Implements IRequest

#Region " Private Members "

        Private p_MaxHeight As Integer = NULL_INTEGER
        Private p_MaxWidth As Integer = NULL_INTEGER
        Private p_Url As String = NULL_STRING

#End Region

        Public Sub New()
            ' do nothing
        End Sub

        Public Sub New(ByVal URL As String)
            Me.URL = URL
        End Sub

        Public Sub New(ByVal URL As String, ByVal Width As Integer, ByVal Height As Integer)
            Me.URL = URL
            Me.MaxHeight = Height
            Me.MaxWidth = Width
        End Sub

#Region " Properties "

        ''' <summary>
        ''' The required response format. When not specified, the provider can return any valid response format. When specified, the provider must return data in the request format, else return an error.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks>
        ''' ReadOnly. This API only supports JSON requests and responses at this time.
        ''' 
        ''' If there was an error, instead of the specified formatted content, oEmbed providers may return the following errors: 404 Not Found, 501 Not Implemented, 401 Not Authorized
        ''' </remarks>
        <DataMember()> _
        Public ReadOnly Property Format() As String Implements IRequest.Format
            Get
                Return FORMAT_JSON
            End Get
        End Property

        ''' <summary>
        ''' The maximum height of the embedded resource. For supported resource types, this parameter must be respected by providers.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks>
        ''' Required to be sent for video and photo oEmbed types.
        ''' </remarks>
        <DataMember()> _
        Public Property MaxHeight() As Integer Implements IRequest.MaxHeight
            Get
                Return Me.p_MaxHeight
            End Get
            Set(ByVal value As Integer)
                Me.p_MaxHeight = value
            End Set
        End Property

        ''' <summary>
        ''' The maximum width of the embedded resource. For supported resource types, this parameter must be respected by providers.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks>
        ''' Required to be sent for video and photo oEmbed types.
        ''' </remarks>
        <DataMember()> _
        Public Property MaxWidth() As Integer Implements IRequest.MaxWidth
            Get
                Return Me.p_MaxWidth
            End Get
            Set(ByVal value As Integer)
                Me.p_MaxWidth = value
            End Set
        End Property

        ''' <summary>
        ''' The URL to retrieve embedding information for.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <DataMember()> _
        Public Property URL() As String Implements IRequest.URL
            Get
                Return Me.p_Url
            End Get
            Set(ByVal value As String)
                Me.p_Url = value
            End Set
        End Property

#End Region

    End Class

End Namespace