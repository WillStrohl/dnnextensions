'
' Lightbox Gallery Module for DotNetNuke
' Project Contributors - Will Strohl (http://www.WillStrohl.com), Armand Datema (http://www.schwingsoft.com)
'
'Copyright (c) 2009-2012, Will Strohl
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
'Neither the name of Will Strohl, Armand Datema, Lightbox Gallery, nor the names of its contributors may be 
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

Imports DotNetNuke.Services.Exceptions
Imports System.Text.RegularExpressions
Imports System.Web
Imports System.Web.SessionState

Namespace WillStrohl.Modules.Lightbox

    Partial Public Class OrderHandler
        Implements IHttpHandler, IRequiresSessionState

#Region " Private Members "

        Private p_ModuleId As Integer = Null.NullInteger
        Private p_Order As String = Null.NullString

#End Region

#Region " Properties "

        Private ReadOnly Property ModuleId() As Integer
            Get
                Return Me.p_ModuleId
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

        Public Sub ProcessRequest(ByVal context As System.Web.HttpContext) Implements System.Web.IHttpHandler.ProcessRequest

            Try

                context.Response.Cache.SetCacheability(HttpCacheability.NoCache)
                context.Response.ContentType = "text/plain"

                If Not context.Request.QueryString("mid") Is Nothing Then
                    Try
                        Me.p_ModuleId = Integer.Parse(context.Request.QueryString("mid").ToString, Globalization.NumberStyles.Integer)
                    Catch
                        Me.HandleError(context, "The mid parameter was not in the correct format.")
                    End Try
                Else
                    Me.HandleError(context, "The mid parameter is missing.")
                End If

                If Not context.Request.QueryString("order") Is Nothing Then
                    Me.p_Order = context.Request.QueryString("order").ToString
                    If Not Regex.IsMatch(Me.p_Order, "^[\d,]+$") Then
                        Me.HandleError(context, "The order parameter was not in the correct format.")
                    End If
                Else
                    Me.HandleError(context, "The order parameter is missing.")
                End If

                Me.SaveOrder()

                context.Response.Write("true")

            Catch ex As Exception
                LogException(ex)
                If IsEditMode() Then
                    Me.HandleError(context, String.Concat(ex.Message, Environment.NewLine, ex.StackTrace))
                End If
            End Try

        End Sub

        Private Sub SaveOrder()

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
                arrInt(intCount) = Integer.Parse(oStr, Globalization.NumberStyles.Integer)
                intCount += 1
            Next

            Return arrInt

        End Function

        Private Sub HandleError(ByVal context As System.Web.HttpContext, ByVal ErrorText As String)

            context.Response.Write(ErrorText)
            context.Response.End()

        End Sub

#Region " IHttpHandler Implementation "

        Public ReadOnly Property IsReusable() As Boolean Implements System.Web.IHttpHandler.IsReusable
            Get
                Return True
            End Get
        End Property

#End Region

    End Class

End Namespace