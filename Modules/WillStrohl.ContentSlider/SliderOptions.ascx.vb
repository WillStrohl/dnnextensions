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
Imports DotNetNuke.UI.Skins.Skin
Imports DotNetNuke.UI.Skins.Controls.ModuleMessage.ModuleMessageType
Imports System.Web.UI.WebControls
Imports WillStrohl.Modules.ContentSlider.SliderController

Namespace WillStrohl.Modules.ContentSlider

    Public Class SliderOptions
        Inherits WnsPortalModuleBase

#Region " Private Members "

        Private Const c_True As String = "true"
        Private Const c_False As String = "false"
        Private Const DEFAULT_ANCHOR_BUILDER As String = "function (idx, slide) { return '<li class=""wnsSliderPagerItem""><a class=""wnsSliderPagerItemLink"" href=""#"">' + slide.title + '</a></li>'; }"

#End Region

#Region " Properties "

        Private ReadOnly Property SliderCacheName() As String
            Get
                Return String.Format(SLIDER_CACHE_KEY_FORMAT, TabId, TabModuleId)
            End Get
        End Property

        Private ReadOnly Property ScriptCacheName() As String
            Get
                Return String.Format(SCRIPT_CACHE_KEY_FORMAT, TabId, TabModuleId)
            End Get
        End Property

#End Region

#Region " Event Handlers "

        Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            Try
                AddModuleMessage(Me, GetLocalizedString("Message.Warning"), YellowWarning)

                If Not Page.IsPostBack Then
                    Me.BindData()
                End If
            Catch exc As Exception ' Module failed to load
                ProcessModuleLoadException(Me, exc, Me.IsEditable)
            End Try
        End Sub

        Private Sub lnkSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkSave.Click
            If Page.IsValid Then
                Me.UpdateSettings()
                Me.SendBackToModule()
            End If
        End Sub

        Private Sub lnkCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkCancel.Click
            Me.SendBackToModule()
        End Sub

        Private Sub lnkReset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkReset.Click
            Me.ResetToDefaulValues()
        End Sub

        Private Sub chkPager_CheckedChanged(sender As Object, e As System.EventArgs) Handles chkPager.CheckedChanged

            If chkPager.Checked Then

                If String.IsNullOrEmpty(txtPagerAnchorBuilder.Text) Then
                    txtPagerAnchorBuilder.Text = DEFAULT_ANCHOR_BUILDER
                End If

            Else

                If Not String.IsNullOrEmpty(txtPagerAnchorBuilder.Text) Then
                    txtPagerAnchorBuilder.Text = String.Empty
                End If

            End If

            ToggleAnchorBuilderLink()
            TogglePagerSettings()

        End Sub

        Private Sub lnkDefaultPagerAnchorBuilder_Click(sender As Object, e As System.EventArgs) Handles lnkDefaultPagerAnchorBuilder.Click

            txtPagerAnchorBuilder.Text = DEFAULT_ANCHOR_BUILDER

        End Sub

#End Region

#Region " Private Methods "

        Private Sub BindData()

            Me.LocalizeModule()

            Me.LoadSettings()

            ToggleAnchorBuilderLink()
            TogglePagerSettings()

        End Sub

        Private Sub LocalizeModule()

            rfvHeight.ErrorMessage = GetLocalizedString("rfvHeight.ErrorMessage")
            rfvSpeed.ErrorMessage = GetLocalizedString("rfvSpeed.ErrorMessage")
            revSpeed.ErrorMessage = GetLocalizedString("revSpeed.ErrorMessage")
            rfvFx.ErrorMessage = GetLocalizedString("rfvFx.ErrorMessage")
            rfvMetaAttr.ErrorMessage = GetLocalizedString("rfvMetaAttr.ErrorMessage")
            rfvRequeueTimeout.ErrorMessage = GetLocalizedString("rfvRequeueTimeout.ErrorMessage")
            rfvPagerEvent.ErrorMessage = GetLocalizedString("rfvPagerEvent.ErrorMessage")
            rfvPrevNextEvent.ErrorMessage = GetLocalizedString("rfvPrevNextEvent.ErrorMessage")
            rfvTimeout.ErrorMessage = GetLocalizedString("rfvTimeout.ErrorMessage")
            revRequeueTimeout.ErrorMessage = GetLocalizedString("revRequeueTimeout.ErrorMessage")
            rfvCacheDuration.ErrorMessage = GetLocalizedString("rfvCacheDuration.ErrorMessage")
            revCacheDuration.ErrorMessage = GetLocalizedString("revCacheDuration.ErrorMessage")
            lnkDefaultPagerAnchorBuilder.Text = GetLocalizedString("lnkDefaultPagerAnchorBuilder.Text")

            For intI As Integer = 0 To 27
                cboFx.Items.Add(New ListItem( _
                    GetLocalizedString(String.Format("cboFx.Items.{0}.Value", intI))) _
                )
            Next
            cboFx.Items.Insert(0, New ListItem("---"))

            lnkCancel.Text = GetLocalizedString("lnkCancel.Text")
            lnkSave.Text = GetLocalizedString("lnkSave.Text")
            lnkReset.Text = GetLocalizedString("lnkReset.Text")

        End Sub

        ''' <summary>
        ''' This is the inherited method that DNN uses to update the settings from the UI
        ''' </summary>
        ''' <remarks></remarks>
        ''' <history>
        ''' </history>
        Public Sub UpdateSettings()
            Try

                Dim objModules As New DotNetNuke.Entities.Modules.ModuleController
                'Dim sec As New DotNetNuke.Security.PortalSecurity

                objModules.DeleteModuleSettings(Me.ModuleId)
                objModules.DeleteTabModuleSettings(Me.TabModuleId)

                If Not String.IsNullOrEmpty(Me.txtWidth.Text) Then
                    objModules.UpdateTabModuleSetting(Me.TabModuleId, SETTING_WIDTH_KEY, Me.txtWidth.Text)
                End If

                If String.Equals(Me.txtHeight.Text, "auto", StringComparison.InvariantCultureIgnoreCase) Then
                    objModules.UpdateTabModuleSetting(Me.TabModuleId, SETTING_HEIGHT_KEY, Me.txtHeight.Text)
                End If

                If Me.chkFit.Checked Then
                    objModules.UpdateTabModuleSetting(Me.TabModuleId, SETTING_FIT_KEY, c_True)
                End If

                If Not Me.chkContainerResize.Checked Then
                    Me.Settings(SETTING_CONTAINERRESIZE_KEY) = c_False
                End If

                If Me.chkPause.Checked Then
                    objModules.UpdateTabModuleSetting(Me.TabModuleId, SETTING_PAUSE_KEY, c_True)
                End If

                If Me.chkExcludeCycle.Checked Then
                    objModules.UpdateTabModuleSetting(Me.TabModuleId, SETTING_EXCLUDECYCLE_KEY, c_True)
                End If

                If Me.chkExcludeEasing.Checked Then
                    objModules.UpdateTabModuleSetting(Me.TabModuleId, SETTING_EXCLUDEEASING_KEY, c_True)
                End If

                If Not String.Equals(Me.txtSpeed.Text, "1000", StringComparison.InvariantCultureIgnoreCase) Then
                    objModules.UpdateTabModuleSetting(Me.TabModuleId, SETTING_SPEED_KEY, Me.txtSpeed.Text)
                End If

                If Not String.IsNullOrEmpty(Me.txtCssAfter.Text) Then
                    objModules.UpdateTabModuleSetting(Me.TabModuleId, SETTING_CSSAFTER_KEY, Me.txtCssAfter.Text)
                End If

                If Not String.IsNullOrEmpty(Me.txtCssBefore.Text) Then
                    objModules.UpdateTabModuleSetting(Me.TabModuleId, SETTING_CSSBEFORE_KEY, Me.txtCssBefore.Text)
                End If

                If Not String.IsNullOrEmpty(Me.txtEasing.Text) Then
                    objModules.UpdateTabModuleSetting(Me.TabModuleId, SETTING_EASING_KEY, Me.txtEasing.Text)
                End If

                If Not String.IsNullOrEmpty(Me.txtEaseIn.Text) Then
                    objModules.UpdateTabModuleSetting(Me.TabModuleId, SETTING_EASEIN_KEY, Me.txtEaseIn.Text)
                End If

                If Not String.IsNullOrEmpty(Me.txtEaseOut.Text) Then
                    objModules.UpdateTabModuleSetting(Me.TabModuleId, SETTING_EASEOUT_KEY, Me.txtEaseOut.Text)
                End If

                If Me.chkAllowPagerClickBubble.Checked Then
                    objModules.UpdateTabModuleSetting(Me.TabModuleId, SETTING_ALLOWPAGERCLICKBUBBLE_KEY, c_True)
                End If

                objModules.UpdateTabModuleSetting(TabModuleId, SETTING_PAGER_KEY, chkPager.Checked.ToString())

                If Not String.Equals(Me.txtActivePagerClass.Text, "activeSlide", StringComparison.InvariantCultureIgnoreCase) Then
                    objModules.UpdateTabModuleSetting(Me.TabModuleId, SETTING_ACTIVEPAGERCLASS_KEY, Me.txtActivePagerClass.Text)
                End If

                If Not String.IsNullOrEmpty(Me.txtNext.Text) Then
                    objModules.UpdateTabModuleSetting(Me.TabModuleId, SETTING_NEXT_KEY, Me.txtNext.Text)
                End If

                If Not String.IsNullOrEmpty(Me.txtPrev.Text) Then
                    objModules.UpdateTabModuleSetting(Me.TabModuleId, SETTING_PREV_KEY, Me.txtPrev.Text)
                End If

                If Me.chkPauseOnPagerHover.Checked Then
                    objModules.UpdateTabModuleSetting(Me.TabModuleId, SETTING_PAUSEONPAGERHOVER_KEY, c_True)
                End If

                If Me.chkAutoStop.Checked Then
                    objModules.UpdateTabModuleSetting(Me.TabModuleId, SETTING_AUTOSTOP_KEY, c_True)
                End If

                If Not String.IsNullOrEmpty(Me.txtAutoStopCount.Text) Then
                    objModules.UpdateTabModuleSetting(Me.TabModuleId, SETTING_AUTOSTOPCOUNT_KEY, Me.txtAutoStopCount.Text)
                End If

                If Me.chkBackwards.Checked Then
                    objModules.UpdateTabModuleSetting(Me.TabModuleId, SETTING_BACKWARDS_KEY, c_True)
                End If

                If Not String.Equals(Me.txtMetaAttr.Text, "cycle", StringComparison.InvariantCultureIgnoreCase) Then
                    objModules.UpdateTabModuleSetting(Me.TabModuleId, SETTING_METAATTR_KEY, Me.txtMetaAttr.Text)
                End If

                If Me.chkNoWrap.Checked Then
                    objModules.UpdateTabModuleSetting(Me.TabModuleId, SETTING_NOWRAP_KEY, c_True)
                End If

                If Me.chkRandom.Checked Then
                    objModules.UpdateTabModuleSetting(Me.TabModuleId, SETTING_RANDOM_KEY, c_True)
                End If

                If Not Me.chkRequeueOnImageNotLoaded.Checked Then
                    objModules.UpdateTabModuleSetting(Me.TabModuleId, SETTING_REQUEUEONIMAGENOTLOADED_KEY, c_False)
                End If

                If Not String.Equals(Me.txtRequeueTimeout.Text, "250", StringComparison.InvariantCultureIgnoreCase) Then
                    objModules.UpdateTabModuleSetting(Me.TabModuleId, SETTING_REQUEUETIMEOUT_KEY, Me.txtRequeueTimeout.Text)
                End If

                If Not String.IsNullOrEmpty(Me.txtShuffle.Text) Then
                    objModules.UpdateTabModuleSetting(Me.TabModuleId, SETTING_SHUFFLE_KEY, Me.txtShuffle.Text)
                End If

                If Not Me.chkSlideResize.Checked Then
                    objModules.UpdateTabModuleSetting(Me.TabModuleId, SETTING_SLIDERESIZE_KEY, c_False)
                End If

                If Not String.Equals(Me.txtStartingSlide.Text, "0", StringComparison.InvariantCultureIgnoreCase) Then
                    objModules.UpdateTabModuleSetting(Me.TabModuleId, SETTING_STARTINGSLIDE_KEY, Me.txtStartingSlide.Text)
                End If

                If Not String.IsNullOrEmpty(Me.txtAnimIn.Text) Then
                    objModules.UpdateTabModuleSetting(Me.TabModuleId, SETTING_ANIMIN_KEY, Me.txtAnimIn.Text)
                End If

                If Not String.IsNullOrEmpty(Me.txtAnimOut.Text) Then
                    objModules.UpdateTabModuleSetting(Me.TabModuleId, SETTING_ANIMOUT_KEY, Me.txtAnimOut.Text)
                End If

                If Me.chkContinuous.Checked Then
                    objModules.UpdateTabModuleSetting(Me.TabModuleId, SETTING_CONTINUOUS_KEY, c_True)
                End If

                If Not String.Equals(Me.txtDelay.Text, "0", StringComparison.InvariantCultureIgnoreCase) Then
                    objModules.UpdateTabModuleSetting(Me.TabModuleId, SETTING_DELAY_KEY, Me.txtDelay.Text)
                End If

                If Not String.Equals(Me.txtFastOnEvent.Text, "0", StringComparison.InvariantCultureIgnoreCase) Then
                    objModules.UpdateTabModuleSetting(Me.TabModuleId, SETTING_FASTONEVENT_KEY, Me.txtFastOnEvent.Text)
                End If

                If Not String.Equals(cboFx.SelectedValue, "fade", StringComparison.InvariantCultureIgnoreCase) Then
                    objModules.UpdateTabModuleSetting(TabModuleId, SETTING_FX_KEY, cboFx.SelectedValue)
                End If

                If Not Me.chkManualTrump.Checked Then
                    objModules.UpdateTabModuleSetting(Me.TabModuleId, SETTING_MANUALTRUMP_KEY, c_False)
                End If

                If Not Me.chkRandomizeEffects.Checked Then
                    objModules.UpdateTabModuleSetting(Me.TabModuleId, SETTING_RANDOMIZEEFFECTS_KEY, c_False)
                End If

                If Me.chkRev.Checked Then
                    objModules.UpdateTabModuleSetting(Me.TabModuleId, SETTING_REV_KEY, c_True)
                End If

                If Not String.IsNullOrEmpty(Me.txtSpeedIn.Text) Then
                    objModules.UpdateTabModuleSetting(Me.TabModuleId, SETTING_SPEEDIN_KEY, Me.txtSpeedIn.Text)
                End If

                If Not String.IsNullOrEmpty(Me.txtSpeedOut.Text) Then
                    objModules.UpdateTabModuleSetting(Me.TabModuleId, SETTING_SPEEDOUT_KEY, Me.txtSpeedOut.Text)
                End If

                If Not String.Equals(Me.txtTimeout.Text, "4000", StringComparison.InvariantCultureIgnoreCase) Then
                    objModules.UpdateTabModuleSetting(Me.TabModuleId, SETTING_TIMEOUT_KEY, Me.txtTimeout.Text)
                End If

                If Not Me.chkSync.Checked Then
                    objModules.UpdateTabModuleSetting(Me.TabModuleId, SETTING_SYNC_KEY, c_False)
                End If

                If Not String.IsNullOrEmpty(Me.txtBefore.Text) Then
                    objModules.UpdateTabModuleSetting(Me.TabModuleId, SETTING_BEFORE_KEY, Me.txtBefore.Text)
                End If

                If Not String.IsNullOrEmpty(Me.txtEnd.Text) Then
                    objModules.UpdateTabModuleSetting(Me.TabModuleId, SETTING_END_KEY, Me.txtEnd.Text)
                End If

                If Not String.IsNullOrEmpty(Me.txtFxFn.Text) Then
                    objModules.UpdateTabModuleSetting(Me.TabModuleId, SETTING_FXFN_KEY, Me.txtFxFn.Text)
                End If

                If Not String.IsNullOrEmpty(Me.txtOnPagerEvent.Text) Then
                    objModules.UpdateTabModuleSetting(Me.TabModuleId, SETTING_ONPAGEREVENT_KEY, Me.txtOnPagerEvent.Text)
                End If

                If Not String.IsNullOrEmpty(Me.txtOnPrevNextEvent.Text) Then
                    objModules.UpdateTabModuleSetting(Me.TabModuleId, SETTING_ONPREVIOUSNEXTEVENT_KEY, Me.txtOnPrevNextEvent.Text)
                End If

                If Not String.IsNullOrEmpty(Me.txtPagerAnchorBuilder.Text) Then
                    objModules.UpdateTabModuleSetting(Me.TabModuleId, SETTING_PAGERANCHORBUILDER_KEY, Me.txtPagerAnchorBuilder.Text)
                End If

                If Not String.Equals(Me.txtPagerEvent.Text, "click.cycle", StringComparison.InvariantCultureIgnoreCase) Then
                    objModules.UpdateTabModuleSetting(Me.TabModuleId, SETTING_PAGEREVENT_KEY, Me.txtPagerEvent.Text)
                End If

                If Not String.Equals(Me.txtPrevNextEvent.Text, "click.cycle", StringComparison.InvariantCultureIgnoreCase) Then
                    objModules.UpdateTabModuleSetting(Me.TabModuleId, SETTING_PREVNEXTEVENT_KEY, Me.txtPrevNextEvent.Text)
                End If

                If Not String.IsNullOrEmpty(Me.txtSlideExpr.Text) Then
                    objModules.UpdateTabModuleSetting(Me.TabModuleId, SETTING_SLIDEEXPR_KEY, Me.txtSlideExpr.Text)
                End If

                If Not String.IsNullOrEmpty(Me.txtTimeoutFn.Text) Then
                    objModules.UpdateTabModuleSetting(Me.TabModuleId, SETTING_TIMEOUTFN_KEY, Me.txtTimeoutFn.Text)
                End If

                If Not String.IsNullOrEmpty(Me.txtUpdateActivePagerLink.Text) Then
                    objModules.UpdateTabModuleSetting(Me.TabModuleId, SETTING_UPDATEACTIVEPAGERLINK_KEY, Me.txtUpdateActivePagerLink.Text)
                End If

                If Not String.Equals(txtCacheDuration.Text, "5") Then
                    objModules.UpdateTabModuleSetting(TabModuleId, SETTING_CACHEDURATION_KEY, txtCacheDuration.Text)
                End If

                DataCache.RemoveCache(SliderCacheName)
                DataCache.RemoveCache(ScriptCacheName)

                Entities.Modules.ModuleController.SynchronizeModule(Me.ModuleId)

            Catch exc As Exception ' Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub

        ''' <summary>
        ''' This is the inherited DNN method that loads saved settings into the settings UI
        ''' </summary>
        ''' <remarks></remarks>
        ''' <history>
        ''' </history>
        Public Sub LoadSettings()

            Try
                If Me.Settings.Count > 0 Then

                    Me.txtWidth.Text = Me.Setting_Width
                    Me.txtHeight.Text = Me.Setting_Height
                    Me.chkFit.Checked = (Me.Setting_Fit = 1)
                    Me.chkContainerResize.Checked = (Me.Setting_ContainerResize = 1)
                    Me.chkPause.Checked = (Me.Setting_Pause = 1)
                    Me.chkExcludeCycle.Checked = Me.Setting_ExcludeCycle
                    Me.chkExcludeEasing.Checked = Me.Setting_ExcludeEasing
                    Me.txtSpeed.Text = Me.Setting_Speed.ToString
                    txtCacheDuration.Text = Setting_CacheDuration.ToString()

                    Me.txtCssAfter.Text = Me.Setting_CssAfter
                    Me.txtCssBefore.Text = Me.Setting_CssBefore
                    Me.txtEasing.Text = Me.Setting_Easing
                    Me.txtEaseIn.Text = Me.Setting_EaseIn
                    Me.txtEaseOut.Text = Me.Setting_EaseOut

                    Me.chkAllowPagerClickBubble.Checked = Boolean.Parse(Me.Setting_AllowPagerClickBubble.ToString)
                    Me.txtActivePagerClass.Text = Me.Setting_ActivePagerClass
                    chkPager.Checked = Setting_Pager
                    Me.txtNext.Text = Me.Setting_Next
                    Me.txtPrev.Text = Me.Setting_Prev
                    Me.chkPauseOnPagerHover.Checked = (Me.Setting_PauseOnPagerHover = 1)

                    Me.chkAutoStop.Checked = (Me.Setting_AutoStop = 1)
                    Me.txtAutoStopCount.Text = Me.Setting_AutoStopCount.ToString
                    Me.chkBackwards.Checked = Boolean.Parse(Me.Setting_Backwards.ToString)
                    Me.txtMetaAttr.Text = Me.Setting_MetaAttr
                    Me.chkNoWrap.Checked = (Me.Setting_NoWrap = 1)
                    Me.chkRandom.Checked = (Me.Setting_Random = 1)
                    Me.chkRequeueOnImageNotLoaded.Checked = Boolean.Parse(Me.Setting_RequeueOnImageNotLoaded.ToString)
                    Me.txtRequeueTimeout.Text = Me.Setting_RequeueTimeout.ToString
                    Me.txtShuffle.Text = Me.Setting_Shuffle
                    Me.chkSlideResize.Checked = (Me.Setting_SlideResize = 1)
                    Me.txtStartingSlide.Text = Me.Setting_StartingSlide.ToString

                    Me.txtAnimIn.Text = Me.Setting_AnimationIn
                    Me.txtAnimOut.Text = Me.Setting_AnimationOut
                    Me.chkContinuous.Checked = (Me.Setting_Continuous = 1)
                    Me.txtDelay.Text = Me.Setting_Delay.ToString
                    Me.txtFastOnEvent.Text = Me.Setting_FastOnEvent.ToString
                    Try
                        cboFx.Items.FindByValue(Setting_Fx).Selected = True
                    Catch
                        cboFx.Items.FindByValue(GetLocalizedString("cboFx.Items.6.Value")).Selected = True
                    End Try
                    Me.chkManualTrump.Checked = Boolean.Parse(Me.Setting_ManualTrump.ToString)
                    Me.chkRandomizeEffects.Checked = (Me.Setting_RandomizeEffects = 1)
                    Me.chkRev.Checked = (Me.Setting_Rev = 1)
                    Me.txtSpeedIn.Text = Me.Setting_SpeedIn.ToString
                    Me.txtSpeedOut.Text = Me.Setting_SpeedOut.ToString
                    Me.txtTimeout.Text = Me.Setting_Timeout.ToString
                    Me.chkSync.Checked = (Me.Setting_Sync = 1)

                    Me.txtAfter.Text = Me.Setting_After
                    Me.txtBefore.Text = Me.Setting_Before
                    Me.txtEnd.Text = Me.Setting_End
                    Me.txtFxFn.Text = Me.Setting_FxFn
                    Me.txtOnPagerEvent.Text = Me.Setting_OnPagerEvent
                    Me.txtOnPrevNextEvent.Text = Me.Setting_OnPreviousNextEvent
                    Me.txtPagerAnchorBuilder.Text = Me.Setting_PagerAnchorBuilder
                    Me.txtPagerEvent.Text = Me.Setting_PagerEvent
                    Me.txtPrevNextEvent.Text = Me.Setting_PrevNextEvent
                    Me.txtSlideExpr.Text = Me.Setting_SlideExpr
                    Me.txtTimeoutFn.Text = Me.Setting_TimeoutFn
                    Me.txtUpdateActivePagerLink.Text = Me.Setting_UpdateActivePagerLink

                Else
                    Me.ResetToDefaulValues()
                End If
            Catch exc As Exception ' Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub SendBackToModule()
            Response.Redirect(NavigateURL)
        End Sub

        Private Sub ResetToDefaulValues()

            Me.txtActivePagerClass.Text = "activeSlide"
            Me.txtAfter.Text = String.Empty
            Me.chkAllowPagerClickBubble.Checked = False
            Me.txtAnimIn.Text = String.Empty
            Me.txtAnimOut.Text = String.Empty
            Me.chkAutoStop.Checked = False
            Me.txtAutoStopCount.Text = "0"
            Me.chkBackwards.Checked = False
            Me.txtBefore.Text = String.Empty
            Me.chkContainerResize.Checked = True
            Me.chkContinuous.Checked = False
            Me.txtCssAfter.Text = String.Empty
            Me.txtCssBefore.Text = String.Empty
            Me.txtDelay.Text = "0"
            Me.txtEaseIn.Text = String.Empty
            Me.txtEaseOut.Text = String.Empty
            Me.txtEasing.Text = String.Empty
            Me.txtEnd.Text = String.Empty
            Me.txtFastOnEvent.Text = "0"
            Me.chkFit.Checked = False
            cboFx.Items.FindByValue(GetLocalizedString("cboFx.Items.6.Value")).Selected = True
            Me.txtFxFn.Text = String.Empty
            Me.txtHeight.Text = "auto"
            Me.chkManualTrump.Checked = True
            Me.txtMetaAttr.Text = "cycle"
            Me.txtNext.Text = String.Empty
            Me.chkNoWrap.Checked = False
            Me.txtOnPagerEvent.Text = String.Empty
            Me.txtOnPrevNextEvent.Text = String.Empty
            chkPager.Checked = False
            Me.txtPagerAnchorBuilder.Text = String.Empty
            Me.txtPagerEvent.Text = "click.cycle"
            Me.chkPause.Checked = False
            Me.chkPauseOnPagerHover.Checked = False
            Me.txtPrev.Text = String.Empty
            Me.txtPrevNextEvent.Text = "click.cycle"
            Me.chkRandom.Checked = False
            Me.chkRandomizeEffects.Checked = True
            Me.chkRequeueOnImageNotLoaded.Checked = True
            Me.txtRequeueTimeout.Text = "250"
            Me.chkRev.Checked = False
            Me.txtShuffle.Text = String.Empty
            Me.txtSlideExpr.Text = String.Empty
            Me.chkSlideResize.Checked = True
            Me.txtSpeed.Text = "1000"
            Me.txtSpeedIn.Text = String.Empty
            Me.txtSpeedOut.Text = String.Empty
            Me.txtStartingSlide.Text = "0"
            Me.chkSync.Checked = True
            Me.txtTimeout.Text = "4000"
            Me.txtTimeoutFn.Text = String.Empty
            Me.txtUpdateActivePagerLink.Text = String.Empty
            Me.txtWidth.Text = String.Empty

        End Sub

        Private Sub ToggleAnchorBuilderLink()

            lnkDefaultPagerAnchorBuilder.Visible = chkPager.Checked AndAlso Not String.Equals(txtPagerAnchorBuilder.Text, DEFAULT_ANCHOR_BUILDER)

        End Sub

        Private Sub TogglePagerSettings()

            txtActivePagerClass.Enabled = chkPager.Checked
            txtPagerAnchorBuilder.Enabled = chkPager.Checked
            txtNext.Enabled = chkPager.Checked
            txtPrev.Enabled = chkPager.Checked
            chkPauseOnPagerHover.Enabled = chkPager.Checked
            chkAllowPagerClickBubble.Enabled = chkPager.Checked

            If Not chkPager.Checked Then
                txtActivePagerClass.CssClass = "aspNetDisabled"
                txtPagerAnchorBuilder.CssClass = "aspNetDisabled"
                txtNext.CssClass = "aspNetDisabled"
                txtPrev.CssClass = "aspNetDisabled"
            Else
                txtActivePagerClass.CssClass = "NormalTextbox dnnFormRequired"
                txtPagerAnchorBuilder.CssClass = "NormalTextbox"
                txtNext.CssClass = "NormalTextbox"
                txtPrev.CssClass = "NormalTextbox"
            End If

        End Sub

#End Region

    End Class

End Namespace