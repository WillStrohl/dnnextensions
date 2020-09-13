﻿'
' Copyright Upendo Ventures, LLC
' https://upendoventures.com
' 
' Permission is hereby granted, free of charge, to any person obtaining a copy of this software
' and associated documentation files (the "Software"), to deal in the Software without restriction,
' including without limitation the rights to use, copy, modify, merge, publish, distribute,
' sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is
' furnished to do so, subject to the following conditions:
' 
' The above copyright notice and this permission notice shall be included in all copies or
' substantial portions of the Software.
' 
' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
' BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
' NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
' DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
' OUT OF Or IN CONNECTION WITH THE SOFTWARE Or THE USE Or OTHER DEALINGS IN THE SOFTWARE.
'

Imports System.Runtime.Serialization
Imports Newtonsoft.Json

Namespace WillStrohl.Modules.Lightbox.Models

    <DataContract>
    <Serializable>
    Public Class ImageReorderModel

        <DataMember(Name := "Album")>
        <JsonProperty(PropertyName := "Album")>
        Public Album As Integer

        <DataMember(Name := "Order")>
        <JsonProperty(PropertyName := "Order")>
        Public Order As String

        Public Sub New()
            Album = -1
            Order = string.Empty
        End Sub
        
        Public Sub New(Album As Integer, Order As String)
            Me.Album = Album
            Me.Order = Order
        End Sub

    End Class

End Namespace