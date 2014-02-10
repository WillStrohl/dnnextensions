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
    ''' oEmbedInfo - this is the base oEmbed response object that contains all of the universal oEmbed properties
    ''' </summary>
    ''' <remarks></remarks>
    <DataContract()> _
    Public Class oEmbedInfo
        Implements IBaseType

#Region " Private Members "

        Private p_AuthorName As String = NULL_STRING
        Private p_AuthorUrl As String = NULL_STRING
        Private p_CacheAge As Integer = NULL_INTEGER
        Private p_ProviderName As String = NULL_STRING
        Private p_ProviderUrl As String = NULL_STRING
        Private p_ThumbnailHeight As Integer = NULL_INTEGER
        Private p_ThumbnailUrl As String = NULL_STRING
        Private p_ThumbnailWidth As Integer = NULL_INTEGER
        Private p_Title As String = NULL_STRING
        Private p_Type As String = NULL_STRING
        Private p_Version As Decimal = NULL_DECIMAL

#End Region

        Public Sub New()
            ' do nothing
        End Sub

#Region " Public Properties "

        ''' <summary>
        ''' The name of the author/owner of the resource.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks>
        ''' Optionally returned by the oEmbed provider
        ''' </remarks>
        <DataMember(Name:="author_name")> _
        Public Property AuthorName() As String Implements IBaseType.AuthorName
            Get
                Return Me.p_AuthorName
            End Get
            Set(ByVal value As String)
                Me.p_AuthorName = value
            End Set
        End Property

        ''' <summary>
        ''' A URL for the author/owner of the resource.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks>
        ''' Optionally returned by the oEmbed provider
        ''' </remarks>
        <DataMember(Name:="author_url")> _
        Public Property AuthorUrl() As String Implements IBaseType.AuthorUrl
            Get
                Return Me.p_AuthorUrl
            End Get
            Set(ByVal value As String)
                Me.p_AuthorUrl = value
            End Set
        End Property

        ''' <summary>
        ''' The suggested cache lifetime for this resource, in seconds. Consumers may choose to use this value or not.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks>
        ''' Optionally returned by the oEmbed provider
        ''' </remarks>
        <DataMember(Name:="cache_age")> _
        Public Property CacheAge() As Integer Implements IBaseType.CacheAge
            Get
                Return Me.p_CacheAge
            End Get
            Set(ByVal value As Integer)
                Me.p_CacheAge = value
            End Set
        End Property

        ''' <summary>
        ''' The name of the resource provider.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks>
        ''' Optionally returned by the oEmbed provider
        ''' </remarks>
        <DataMember(Name:="provider_name")> _
        Public Property ProviderName() As String Implements IBaseType.ProviderName
            Get
                Return Me.p_ProviderName
            End Get
            Set(ByVal value As String)
                Me.p_ProviderName = value
            End Set
        End Property

        ''' <summary>
        ''' A URL for the author/owner of the resource.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks>
        ''' Optionally returned by the oEmbed provider
        ''' </remarks>
        <DataMember(Name:="provider_url")> _
        Public Property ProviderUrl() As String Implements IBaseType.ProviderUrl
            Get
                Return Me.p_ProviderUrl
            End Get
            Set(ByVal value As String)
                Me.p_ProviderUrl = value
            End Set
        End Property

        ''' <summary>
        ''' The height of the optional thumbnail. If this paramater is present, thumbnail_url and thumbnail_width must also be present.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks>
        ''' Optionally returned by the oEmbed provider
        ''' </remarks>
        <DataMember(Name:="thumbnail_height")> _
        Public Property ThumbnailHeight() As Integer Implements IBaseType.ThumbnailHeight
            Get
                Return Me.p_ThumbnailHeight
            End Get
            Set(ByVal value As Integer)
                Me.p_ThumbnailHeight = value
            End Set
        End Property

        ''' <summary>
        ''' A URL to a thumbnail image representing the resource. The thumbnail must respect any maxwidth and maxheight parameters. If this paramater is present, thumbnail_width and thumbnail_height must also be present.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks>
        ''' Optionally returned by the oEmbed provider
        ''' </remarks>
        <DataMember(Name:="thumbnail_url")> _
        Public Property ThumbnailUrl() As String Implements IBaseType.ThumbnailUrl
            Get
                Return Me.p_ThumbnailUrl
            End Get
            Set(ByVal value As String)
                Me.p_ThumbnailUrl = value
            End Set
        End Property

        ''' <summary>
        ''' The width of the optional thumbnail. If this paramater is present, thumbnail_url and thumbnail_height must also be present.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks>
        ''' Optionally returned by the oEmbed provider
        ''' </remarks>
        <DataMember(Name:="thumbnail_width")> _
        Public Property ThumbnailWidth() As Integer Implements IBaseType.ThumbnailWidth
            Get
                Return Me.p_ThumbnailWidth
            End Get
            Set(ByVal value As Integer)
                Me.p_ThumbnailWidth = value
            End Set
        End Property

        ''' <summary>
        ''' A text title, describing the resource.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks>
        ''' Optionally returned by the oEmbed provider
        ''' </remarks>
        <DataMember(Name:="title")> _
        Public Property Title() As String Implements IBaseType.Title
            Get
                Return Me.p_Title
            End Get
            Set(ByVal value As String)
                Me.p_Title = value
            End Set
        End Property

        ''' <summary>
        ''' The resource type.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks>
        ''' Valid values are photo, video, link, and rich
        ''' </remarks>
        <DataMember(Name:="type")> _
        Public Property Type() As String Implements IBaseType.Type
            Get
                Return Me.p_Type
            End Get
            Set(ByVal value As String)
                Me.p_Type = value
            End Set
        End Property

        ''' <summary>
        ''' The oEmbed version number. This must be 1.0.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <DataMember(Name:="version")> _
        Public Property Version() As Decimal Implements IBaseType.Version
            Get
                Return Me.p_Version
            End Get
            Set(ByVal value As Decimal)
                Me.p_Version = value
            End Set
        End Property

#End Region

    End Class

End Namespace