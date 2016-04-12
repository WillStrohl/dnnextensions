'
' Will Strohl (will.strohl@gmail.com)
' http://www.willstrohl.com
'
'Copyright (c) 2011-2016, Will Strohl
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
'Neither the name of Will Strohl, Content Slider, nor the names of its contributors may be 
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

Imports DotNetNuke.UI.Skins.Skin
Imports DotNetNuke.UI.Skins.Controls.ModuleMessage.ModuleMessageType
Imports System.Text
Imports System.Web.UI

Namespace WillStrohl.Modules.ContentSlider

    Public Class SlidersView
        Inherits WNSPortalModuleBase

#Region " Event Handlers "

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            Try

                If Not Page.IsPostBack Then
                    Me.BindData()
                End If

            Catch ex As Exception
                ProcessModuleLoadException(Me, ex, Me.IsEditable)
            End Try
        End Sub

#End Region

#Region " Private Helpers Methods "

        Private Sub BindData()

            Me.LocalizeModule()

            Me.AppendSliders()

        End Sub

        Private Sub LocalizeModule()

            AddModuleMessage(Me, Me.GetLocalizedString("Intro.Text"), BlueInfo)

        End Sub

        Private Sub AppendSliders()

            Dim ctlSlider As New SliderController
            Dim collSliders As SliderInfoCollection = ctlSlider.GetSlidersForEdit(Me.ModuleId)
            Dim sb As New StringBuilder(40)

            sb.Append("<ol class=""wns-slider-list"">")

            For Each oSlider As SliderInfo In collSliders

                Dim strFile As String = ctlSlider.GetFileById(oSlider.SliderContent, ModuleId, Me.PortalSettings)

                If String.IsNullOrEmpty(strFile) Then
                    ' file doesn't exist
                    ctlSlider.DeleteSlider(oSlider.SliderId)
                    Exit For
                End If

                sb.Append("<li class=""wns-slider-list-item"">")
                sb.AppendFormat("<a href=""{0}"">", EditUrl("SliderId", oSlider.SliderId.ToString, "Edit"))
                sb.AppendFormat("<img src=""{1}"" alt=""{0}"" title=""{0}"" class=""editSliderImage"" />", oSlider.AlternateText, strFile)
                sb.Append("</a>")
                sb.Append("</li>")

            Next

            sb.Append("</ol>")

            Me.phContent.Controls.Add(New LiteralControl(sb.ToString))

        End Sub

#End Region

    End Class

End Namespace