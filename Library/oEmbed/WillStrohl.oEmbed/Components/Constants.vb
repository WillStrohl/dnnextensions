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

Namespace WillStrohl.API.oEmbed

    <Serializable()> _
    Public NotInheritable Class Constants

        Public Const FORMAT_JSON As String = "json"

        Public Const NULL_STRING As String = ""
        Public Const NULL_INTEGER As Integer = -1
        Public Const NULL_DECIMAL As Decimal = CType(0.0, Decimal)

        ''' <summary>
        ''' DEFAULT_VIDEO_WIDTH - This value is used for videos if a width is not specified
        ''' </summary>
        ''' <remarks></remarks>
        Public Const DEFAULT_VIDEO_WIDTH As Integer = 425

        ''' <summary>
        ''' DEFAULT_VIDEO_HEIGHT - This value is used for videos if a height is not specified
        ''' </summary>
        ''' <remarks></remarks>
        Public Const DEFAULT_VIDEO_HEIGHT As Integer = 344

        Public Const SSL_PATTERN As String = "^ht{2}ps"

        '
        ' VIDEO
        '

        ''' <summary>
        ''' URL EndPoint format for the YouTube oEmbed implementation
        ''' </summary>
        ''' <remarks></remarks>
        Public Const YOUTUBE_FORMAT As String = "http://www.youtube.com/oembed?url={0}&format=json&maxwidth={1}&maxheight={2}"

        ''' <summary>
        ''' URL EndPoint format for the Viddler oEmbed implementation
        ''' </summary>
        ''' <remarks></remarks>
        Public Const VIDDLER_FORMAT As String = "http://lab.viddler.com/services/oembed/?url={0}&format=json&maxwidth={1}&maxheight={2}"

        ''' <summary>
        ''' URL EndPoint format for the Qik oEmbed implementation
        ''' </summary>
        ''' <remarks></remarks>
        Public Const QIK_FORMAT As String = "http://qik.com/api/oembed.json?url={0}&maxwidth={1}&maxheight={2}"

        ''' <summary>
        ''' URL EndPoint format for the Revision3 oEmbed implementation
        ''' </summary>
        ''' <remarks></remarks>
        Public Const REVISION3_FORMAT As String = "http://revision3.com/api/oembed/?url={0}&format=json&maxwidth={1}&maxheight={2}"

        ''' <summary>
        ''' URL EndPoint format for the Hulu oEmbed implementation
        ''' </summary>
        ''' <remarks></remarks>
        Public Const HULU_FORMAT As String = "http://www.hulu.com/api/oembed.json?url={0}&maxwidth={1}&maxheight={2}"

        ''' <summary>
        ''' URL EndPoint format for the Vimeo oEmbed implementation
        ''' </summary>
        ''' <remarks></remarks>
        Public Const VIMEO_FORMAT As String = "http://www.vimeo.com/api/oembed.json?url={0}&maxwidth={1}&maxheight={2}"

        '
        ' PHOTO
        '

        ''' <summary>
        ''' URL EndPoint format for the Flickr oEmbed implementation
        ''' </summary>
        ''' <remarks></remarks>
        Public Const FLICKR_FORMAT As String = "http://flickr.com/services/oembed?url={0}&format=json&maxwidth={1}&maxheight={2}"

        '
        ' RICH
        '

        ''' <summary>
        ''' URL EndPoint format for the Poll Everywhere oEmbed implementation
        ''' </summary>
        ''' <remarks></remarks>
        Public Const POLLEVERYWHERE_FORMAT As String = "http://www.polleverywhere.com/services/oembed/?url={0}&format=json"

        ''' <summary>
        ''' URL EndPoint format for the oohEmbed oEmbed implementation
        ''' </summary>
        ''' <remarks></remarks>
        Public Const OOHEMBED_FORMAT As String = "http://oohembed.com/oohembed/?url={0}&format=json"

        ''' <summary>
        ''' URL EndPoint format for the MyOpera oEmbed implementation
        ''' </summary>
        ''' <remarks></remarks>
        Public Const MYOPERA_FORMAT As String = "http://my.opera.com/service/oembed?url={0}&format=json"

        ''' <summary>
        ''' URL EndPoint format for the Clearspring Widgets oEmbed implementation
        ''' </summary>
        ''' <remarks></remarks>
        Public Const CLEARSPRINGWIDGETS_FORMAT As String = "http://widgets.clearspring.com/widget/v1/oembed/?url={0}&format=json"

        ''' <summary>
        ''' URL EndPoint format for the Embed.ly oEmbed implementation
        ''' </summary>
        ''' <remarks></remarks>
        Public Const EMBEDLY_FORMAT As String = "http://api.embed.ly/1/oembed?url={0}&format=json"

        '
        ' PROVIDERS
        '

        Public Const PROVIDER_FLICKR As String = "Flickr"
        Public Const PROVIDER_CLEARSPRINGWIDGETS As String = "ClearspringWidgets"
        Public Const PROVIDER_EMBEDLY As String = "Embedly"
        Public Const PROVIDER_MYOPERA As String = "MyOpera"
        Public Const PROVIDER_OOHEMBED As String = "OohEmbed"
        Public Const PROVIDER_POLLEVERYWHERE As String = "PollEverywhere"
        Public Const PROVIDER_HULU As String = "Hulu"
        Public Const PROVIDER_QIK As String = "Qik"
        Public Const PROVIDER_REVISION3 As String = "Revision3"
        Public Const PROVIDER_VIDDLER As String = "Viddler"
        Public Const PROVIDER_VIMEO As String = "Vimeo"
        Public Const PROVIDER_YOUTUBE As String = "YouTube"

    End Class

End Namespace