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

Imports DotNetNuke
Imports DotNetNuke.Services.Exceptions
Imports DotNetNuke.Security.PortalSecurity
Imports DotNetNuke.UI.Skins.Skin
Imports WillStrohl.Modules.ContentSlider.SliderController

Namespace WillStrohl.Modules.ContentSlider

    Partial Public MustInherit Class EditSliderView
        Inherits WNSPortalModuleBase

#Region " Private Members "

        Private p_SliderId As Integer = Null.NullInteger

#End Region

#Region " Public Properties "

        Protected ReadOnly Property SliderId() As Integer
            Get
                If Not p_SliderId > Null.NullInteger Then
                    Dim objSlider As Object = Request.Params("SliderId")
                    If Not objSlider Is Nothing Then
                        p_SliderId = Integer.Parse(objSlider.ToString, Globalization.NumberStyles.Integer)
                    Else
                        p_SliderId = Null.NullInteger
                    End If
                End If

                Return p_SliderId
            End Get
        End Property

        Private ReadOnly Property SliderCacheName() As String
            Get
                Return String.Format(SLIDER_CACHE_KEY_FORMAT, TabId, TabModuleId)
            End Get
        End Property

#End Region

#Region " Event Handlers "

        Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            Try
                If Not Page.IsPostBack Then
                    BindData()
                End If
            Catch exc As Exception ' Module failed to load
                ProcessModuleLoadException(Me, exc, IsEditable)
            End Try
        End Sub

        Private Sub cmdUpdate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdUpdate.Click
            If Page.IsValid Then
                SaveSlider()
            End If
        End Sub

        Private Sub cmdDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdDelete.Click
            Dim ctlSlider As New SliderController
            ctlSlider.DeleteSlider(SliderId)

            SendToDefaultView()
        End Sub

        Private Sub cmdCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdCancel.Click
            SendToDefaultView()
        End Sub

#End Region

#Region " Private Helper Functions "

        Private Sub BindData()

            LocalizeModule()

            ' hide the delete button when adding a new album to the module
            cmdDelete.Visible = (SliderId > Null.NullInteger)

            If SliderId > Null.NullInteger Then

                ' Load the lightbox album information into the form
                Dim ctlSlider As New SliderController
                Dim objSlider As New SliderInfo
                objSlider = ctlSlider.GetSlider(SliderId)

                txtSliderName.Text = Server.HtmlDecode(objSlider.SliderName)
                ctlFile.Url = objSlider.SliderContent
                txtAlternateText.Text = Server.HtmlDecode(objSlider.AlternateText)
                ctlLink.Url = objSlider.Link
                'ctlLink.NewWindow = objSlider.NewWindow
                If Not objSlider.StartDate = Null.NullDate Then
                    txtStartDate.SelectedDate = objSlider.StartDate
                Else
                    txtStartDate.SelectedDate = DateTime.Now
                End If
                If Not objSlider.EndDate = Null.NullDate Then
                    txtEndDate.SelectedDate = objSlider.EndDate
                Else
                    ' do nothing
                End If

            Else

                ' assign default values
                txtStartDate.SelectedDate = DateTime.Now

            End If

        End Sub

        Private Sub LocalizeModule()

            ' localize the text
            cmdCancel.Text = GetLocalizedString("cmdCancel.Text")
            cmdDelete.Text = GetLocalizedString("cmdDelete.Text")
            cmdUpdate.Text = GetLocalizedString("cmdUpdate.Text")
            rfvSliderName.ErrorMessage = GetLocalizedString("rfvSliderNaErrorMessage")
            rfvAlternateText.ErrorMessage = GetLocalizedString("rfvAlternateText.ErrorMessage")
            rfvStartDate.ErrorMessage = GetLocalizedString("rfvStartDate.ErrorMessage")

        End Sub

        Private Sub SaveSlider()

            Dim ctlSlider As New SliderController
            Dim ctlSecurity As New Security.PortalSecurity
            Dim oSlider As New SliderInfo

            If SliderId > Null.NullInteger Then
                oSlider = ctlSlider.GetSlider(SliderId)
            End If

            With oSlider
                .SliderContent = ctlSecurity.InputFilter(ctlFile.Url, FilterFlag.NoScripting)
                .AlternateText = ctlSecurity.InputFilter(txtAlternateText.Text, FilterFlag.NoMarkup)
                .SliderName = ctlSecurity.InputFilter(txtSliderName.Text, FilterFlag.NoMarkup)
                .Link = ctlSecurity.InputFilter(ctlLink.Url, FilterFlag.NoMarkup)
                .NewWindow = ctlLink.NewWindow
                .StartDate = txtStartDate.SelectedDate.Value
                If txtEndDate.SelectedDate.HasValue Then
                    .EndDate = txtEndDate.SelectedDate.Value
                Else
                    .EndDate = Null.NullDate
                End If
                .LastUpdatedBy = UserId
                .ModuleId = ModuleId
            End With

            If SliderId > Null.NullInteger Then
                oSlider.SliderId = SliderId
                ctlSlider.UpdateSlider(oSlider)
            Else
                oSlider.SliderId = ctlSlider.AddSlider(oSlider)
            End If

            ' url tracking
            Dim objUrls As New UrlController
            objUrls.UpdateUrl(PortalId, ctlLink.Url, ctlLink.UrlType, ctlLink.Log, _
                ctlLink.Track, ModuleId, ctlLink.NewWindow)

            DataCache.RemoveCache(SliderCacheName)

            SendToDefaultView()

        End Sub

        Private Sub SendToDefaultView()
            Response.Redirect(NavigateURL)
        End Sub

        Private Sub HandleException(ByVal exc As Exception)
            LogException(exc)
            If UserInfo.IsSuperUser Or UserInfo.UserID = PortalSettings.AdministratorId Then
                AddModuleMessage(Me, _
                    String.Concat(exc.Message, "<br />", exc.StackTrace), _
                    Skins.Controls.ModuleMessage.ModuleMessageType.RedError)
            Else
                AddModuleMessage(Me, exc.Message, _
                    Skins.Controls.ModuleMessage.ModuleMessageType.RedError)
            End If
        End Sub

#End Region
        
    End Class

End Namespace