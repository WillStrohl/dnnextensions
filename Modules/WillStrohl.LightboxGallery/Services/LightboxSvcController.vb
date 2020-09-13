
Imports System.Globalization
Imports System.Net
Imports System.Net.Http
Imports System.Text.RegularExpressions
Imports System.Web.Http
Imports DotNetNuke.Instrumentation
Imports DotNetNuke.Security
Imports DotNetNuke.Web.Api
Imports WillStrohl.Modules.Lightbox.Models

Namespace WillStrohl.Modules.Lightbox.Services

    Public Class LightboxSvcController
        Inherits DnnApiController

        Private Shared ReadOnly Logger As ILog = LoggerSource.Instance.GetLogger(GetType(LightboxSvcController))

#Region " Private Members "

        Private p_LightboxId As Integer = Null.NullInteger
        Private p_Order As String = Null.NullString

#End Region

#Region " Properties "

        Private ReadOnly Property LightboxId() As Integer
            Get
                Return Me.p_LightboxId
            End Get
        End Property

        Private ReadOnly Property Order() As String
            Get
                If Me.p_Order.EndsWith(",") Then
                    Me.p_Order = Me.p_Order.Substring(0, Me.p_Order.Length - 1)
                End If
                Return Me.p_Order
            End Get
        End Property

#End Region

        <AllowAnonymous>
        <HttpGet>
        Public Function HelloWorld() As HttpResponseMessage
            Return Request.CreateResponse(HttpStatusCode.OK, true)
        End Function

        <DnnModuleAuthorize(AccessLevel:=SecurityAccessLevel.Edit)>
        <ValidateAntiForgeryToken>
        <HttpPost>
        Public Function ImageReorder(Model As ImageReorderModel) As HttpResponseMessage
            Try
                If Model Is Nothing Then
                    Throw New NullReferenceException("Model")
                Else 
                    If Not Model.Album > -1 Then
                        Throw New NullReferenceException("Model.Album")
                    End If
                    If String.IsNullOrEmpty(Model.Order) Then
                        Throw New NullReferenceException("Model.Order")
                    End If
                End If

                p_LightboxId = Model.Album
                p_Order = Model.Order

                If Not Regex.IsMatch(Me.p_Order, "^[\d,]+$") Then
                    Throw New ArgumentOutOfRangeException("The order parameter was not in the correct format.")
                End If

                SaveImageOrder()

                Return Request.CreateResponse(HttpStatusCode.OK, true)
            Catch ex As Exception
                LogError(ex)
                Return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "false")
            End Try
        End Function

        <DnnModuleAuthorize(AccessLevel:=SecurityAccessLevel.Edit)>
        <ValidateAntiForgeryToken>
        <HttpPost>
        Public Function AlbumReorder(<FromBody> Order As String) As HttpResponseMessage
            Try
                If String.IsNullOrEmpty(Order) Then
                    Throw New NullReferenceException("Order")
                End If

                p_Order = Order

                If Not Regex.IsMatch(Me.p_Order, "^[\d,]+$") Then
                    Throw New ArgumentOutOfRangeException("The order parameter was not in the correct format.")
                End If

                SaveAlbumOrder()

                Return Request.CreateResponse(HttpStatusCode.OK, true)
            Catch ex As Exception
                LogError(ex)
                Return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "false")
            End Try
        End Function

        #Region "Helper Methods"

        Private Sub SaveImageOrder()

            Dim arrId() As Integer = Me.GetIdArray
            Dim intCount As Integer = 1
            Dim ctlModule As New LightboxController
            Dim oImage As ImageInfo = Nothing

            For Each oId As Integer In arrId
                If oId > 0 Then
                    oImage = ctlModule.GetImageById(oId)
                    oImage.DisplayOrder = intCount
                    ctlModule.UpdateImage(oImage)
                End If

                intCount += 1
            Next

        End Sub

        Private Sub SaveAlbumOrder()

            Dim arrId() As Integer = Me.GetIdArray
            Dim intCount As Integer = 1
            Dim ctlModule As New LightboxController
            Dim album As LightboxInfo = Nothing

            For Each oId As Integer In arrId
                If oId > 0 Then
                    album = ctlModule.GetLightbox(oId)
                    album.DisplayOrder = intCount
                    ctlModule.UpdateLightbox(album)
                End If

                intCount += 1
            Next

        End Sub

        Private Function GetIdArray() As Integer()

            Dim arrStr() As String = Split(Me.Order, ",")
            Dim arrInt(arrStr.Length) As Integer
            Dim intCount As Integer = 0

            For Each oStr As String In arrStr
                arrInt(intCount) = Integer.Parse(oStr, NumberStyles.Integer)
                intCount += 1
            Next

            Return arrInt

        End Function

        Private Sub LogError(ex As Exception)
            If Not ex Is Nothing Then 
                Logger.Error(ex.Message, ex)
                If Not ex.InnerException Is Nothing Then 
                    Logger.Error(ex.InnerException.Message, ex.InnerException)
                End If
            End If
        End Sub

        #End Region

    End Class

End Namespace