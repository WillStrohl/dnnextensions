
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