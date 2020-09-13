'
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

Imports System.Collections.Generic
Imports System.Runtime.Serialization
Imports Newtonsoft.Json

Namespace WillStrohl.Modules.Lightbox.Models

    <Serializable>
    <DataContract>
    <JsonObject(MemberSerialization.OptIn)>
    Public Class DdlDataItems
        Implements IDdlDataItems

        Private queryName As String = "Unit"

        Public Sub New()
            Query = queryName
        End Sub

        Public Sub New(ByVal QueryName As String)
            Query = QueryName
        End Sub

        <DataMember(Name:="query")>
        <JsonProperty(PropertyName:="query")>
        Public Property Query As String Implements IDdlDataItems.Query

        <DataMember(Name:="suggestions")>
        <JsonProperty(PropertyName:="suggestions")>
        Public Property Suggestions As List(Of DdlDataItem) Implements IDdlDataItems.Suggestions

    End Class

    <Serializable>
    <DataContract>
    <JsonObject(MemberSerialization.OptIn)>
    Public Class DdlDataItem
        Implements IDdlDataItem

        Dim _itemName, _itemValue As String

        Public Sub New()
        End Sub

        <DataMember(Name:="value")>
        <JsonProperty(PropertyName:="value")>
        Public Property ItemName As String Implements IDdlDataItem.ItemName
            Get
                Return _itemName
            End Get
            Set(value As String)
                _itemName = value
            End Set
        End Property

        <DataMember(Name:="data")>
        <JsonProperty(PropertyName:="data")>
        Public Property ItemValue As String Implements IDdlDataItem.ItemValue
            Get
                Return _itemValue
            End Get
            Set(value As String)
                _itemValue = value
            End Set
        End Property

    End Class

End Namespace