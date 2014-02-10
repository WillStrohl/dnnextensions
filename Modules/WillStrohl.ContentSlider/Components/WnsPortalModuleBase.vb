'
' Will Strohl (will.strohl@gmail.com)
' http://www.willstrohl.com
'
'Copyright (c) 2011-2013, Will Strohl
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

Imports DotNetNuke.Entities.Modules
Imports DotNetNuke.Services.Localization
Imports DotNetNuke.Framework.JavaScriptLibraries
Imports WillStrohl.Modules.ContentSlider.SliderController

Namespace WillStrohl.Modules.ContentSlider

    Public MustInherit Class WnsPortalModuleBase
        Inherits PortalModuleBase

#Region " Private Members "

        Private p_ActivePagerClass As String = Null.NullString
        Private p_After As String = Null.NullString
        Private p_AllowPagerClickBubble As Nullable(Of Boolean)
        Private p_AnimationIn As String = Null.NullString
        Private p_AnimationOut As String = Null.NullString
        Private p_AutoStop As Nullable(Of Integer)
        Private p_AutoStopCount As Nullable(Of Integer)
        Private p_Backwards As Nullable(Of Boolean)
        Private p_Before As String = Null.NullString
        Private p_ContainerResize As Nullable(Of Integer)
        Private p_Continuous As Nullable(Of Integer)
        Private p_CssAfter As String = Null.NullString
        Private p_CssBefore As String = Null.NullString
        Private p_Delay As Nullable(Of Integer)
        Private p_EaseIn As String = Null.NullString
        Private p_EaseOut As String = Null.NullString
        Private p_Easing As String = Null.NullString
        Private p_End As String = Null.NullString
        Private p_FastOnEvent As Nullable(Of Integer)
        Private p_Fit As Nullable(Of Integer)
        Private p_Fx As String = Null.NullString
        Private p_FxFn As String = Null.NullString
        Private p_Height As String = Null.NullString
        Private p_ManualTrump As Nullable(Of Boolean)
        Private p_MetaAttr As String = Null.NullString
        Private p_Next As String = Null.NullString
        Private p_NoWrap As Nullable(Of Integer)
        Private p_OnPagerEvent As String = Null.NullString
        Private p_OnPreviousNextEvent As String = Null.NullString
        Private p_Pager As Nullable(Of Boolean)
        Private p_PagerAnchorBuilder As String = Null.NullString
        Private p_PagerEvent As String = Null.NullString
        Private p_Pause As Nullable(Of Integer)
        Private p_PauseOnPagerHover As Nullable(Of Integer)
        Private p_Prev As String = Null.NullString
        Private p_PrevNextEvent As String = Null.NullString
        Private p_Random As Nullable(Of Integer)
        Private p_RandomizeEffects As Nullable(Of Integer)
        Private p_RequeueOnImageNotLoaded As Nullable(Of Boolean)
        Private p_RequeueTimeout As Nullable(Of Integer)
        Private p_Rev As Nullable(Of Integer)
        Private p_Shuffle As String = Null.NullString
        Private p_SlideExpr As String = Null.NullString
        Private p_SlideResize As Nullable(Of Integer)
        Private p_Speed As Nullable(Of Integer)
        Private p_SpeedIn As Nullable(Of Integer)
        Private p_SpeedOut As Nullable(Of Integer)
        Private p_StartingSlide As Nullable(Of Integer)
        Private p_Sync As Nullable(Of Integer)
        Private p_Timeout As Nullable(Of Integer)
        Private p_TimeoutFn As String = Null.NullString
        Private p_UpdateActivePagerLink As String = Null.NullString
        Private p_Width As String = Null.NullString

        Private p_ExcludeCycle As Nullable(Of Boolean)
        Private p_ExcludeEasing As Nullable(Of Boolean)

        Private p_CacheDuration As Nullable(Of Integer)

#End Region

#Region " Settings "

        Protected ReadOnly Property Setting_ActivePagerClass As String
            Get
                If Not String.IsNullOrEmpty(Me.p_ActivePagerClass) Then
                    Return Me.p_ActivePagerClass
                End If

                Dim obj As Object = Settings(SETTING_AFTER_KEY)
                If Not obj Is Nothing Then
                    Me.p_ActivePagerClass = obj.ToString
                Else
                    Me.p_ActivePagerClass = "activeSlide"
                End If

                Return Me.p_ActivePagerClass
            End Get
        End Property

        Protected ReadOnly Property Setting_After As String
            Get
                If Not String.IsNullOrEmpty(Me.p_After) Then
                    Return Me.p_After
                End If

                Dim obj As Object = Settings(SETTING_AFTER_KEY)
                If Not obj Is Nothing Then
                    Me.p_After = obj.ToString
                End If

                Return Me.p_After
            End Get
        End Property

        Protected ReadOnly Property Setting_AllowPagerClickBubble As Boolean
            Get
                If Me.p_AllowPagerClickBubble.HasValue Then
                    Return Me.p_AllowPagerClickBubble.Value
                End If

                Dim obj As Object = Settings(SETTING_ALLOWPAGERCLICKBUBBLE_KEY)
                If Not obj Is Nothing Then
                    Me.p_AllowPagerClickBubble = Boolean.Parse(obj.ToString)
                Else
                    Me.p_AllowPagerClickBubble = False
                End If

                Return Me.p_AllowPagerClickBubble.Value
            End Get
        End Property

        Protected ReadOnly Property Setting_AnimationIn As String
            Get
                If Not String.IsNullOrEmpty(Me.p_AnimationIn) Then
                    Return Me.p_AnimationIn
                End If

                Dim obj As Object = Settings(SETTING_ANIMIN_KEY)
                If Not obj Is Nothing Then
                    Me.p_AnimationIn = obj.ToString
                End If

                Return Me.p_AnimationIn
            End Get
        End Property

        Protected ReadOnly Property Setting_AnimationOut As String
            Get
                If Not String.IsNullOrEmpty(Me.p_AnimationOut) Then
                    Return Me.p_AnimationOut
                End If

                Dim obj As Object = Settings(SETTING_ANIMOUT_KEY)
                If Not obj Is Nothing Then
                    Me.p_AnimationOut = obj.ToString
                End If

                Return Me.p_AnimationOut
            End Get
        End Property

        Protected ReadOnly Property Setting_AutoStop As Integer
            Get
                If Me.p_AutoStop.HasValue Then
                    Return Me.p_AutoStop.Value
                End If

                Dim obj As Object = Settings(SETTING_AUTOSTOP_KEY)
                If Not obj Is Nothing Then
                    Me.p_AutoStop = CType(IIf(String.Equals(obj.ToString(), "true", StringComparison.InvariantCultureIgnoreCase), 1, 0), Integer)
                Else
                    Me.p_AutoStop = 0
                End If

                Return Me.p_AutoStop.Value
            End Get
        End Property

        Protected ReadOnly Property Setting_AutoStopCount As Integer
            Get
                If Me.p_AutoStopCount.HasValue Then
                    Return Me.p_AutoStopCount.Value
                End If

                Dim obj As Object = Settings(SETTING_AUTOSTOPCOUNT_KEY)
                If Not obj Is Nothing Then
                    Me.p_AutoStopCount = Integer.Parse(obj.ToString, Globalization.NumberStyles.Integer)
                Else
                    Me.p_AutoStopCount = 0
                End If

                Return Me.p_AutoStopCount.Value
            End Get
        End Property

        Protected ReadOnly Property Setting_Backwards As Boolean
            Get
                If Me.p_Backwards.HasValue Then
                    Return Me.p_Backwards.Value
                End If

                Dim obj As Object = Settings(SETTING_BACKWARDS_KEY)
                If Not obj Is Nothing Then
                    Me.p_Backwards = Boolean.Parse(obj.ToString)
                Else
                    Me.p_Backwards = False
                End If

                Return Me.p_Backwards.Value
            End Get
        End Property

        Protected ReadOnly Property Setting_Before As String
            Get
                If Not String.IsNullOrEmpty(Me.p_Before) Then
                    Return Me.p_Before
                End If

                Dim obj As Object = Settings(SETTING_BEFORE_KEY)
                If Not obj Is Nothing Then
                    Me.p_Before = obj.ToString
                End If

                Return Me.p_Before
            End Get
        End Property

        Protected ReadOnly Property Setting_ContainerResize As Integer
            Get
                If Me.p_ContainerResize.HasValue Then
                    Return Me.p_ContainerResize.Value
                End If

                Dim obj As Object = Settings(SETTING_CONTAINERRESIZE_KEY)
                If Not obj Is Nothing Then
                    Me.p_ContainerResize = Integer.Parse(obj.ToString, Globalization.NumberStyles.Integer)
                Else
                    Me.p_ContainerResize = 1
                End If

                Return Me.p_ContainerResize.Value
            End Get
        End Property

        Protected ReadOnly Property Setting_Continuous As Integer
            Get
                If Me.p_Continuous.HasValue Then
                    Return Me.p_Continuous.Value
                End If

                Dim obj As Object = Settings(SETTING_CONTINUOUS_KEY)
                If Not obj Is Nothing Then
                    Me.p_Continuous = CType(IIf(String.Equals(obj.ToString(), "true", StringComparison.InvariantCultureIgnoreCase), 1, 0), Integer)
                Else
                    Me.p_Continuous = 0
                End If

                Return Me.p_Continuous.Value
            End Get
        End Property

        Protected ReadOnly Property Setting_CssAfter As String
            Get
                If Not String.IsNullOrEmpty(Me.p_CssAfter) Then
                    Return Me.p_CssAfter
                End If

                Dim obj As Object = Settings(SETTING_CSSAFTER_KEY)
                If Not obj Is Nothing Then
                    Me.p_CssAfter = obj.ToString
                End If

                Return Me.p_CssAfter
            End Get
        End Property

        Protected ReadOnly Property Setting_CssBefore As String
            Get
                If Not String.IsNullOrEmpty(Me.p_CssBefore) Then
                    Return Me.p_CssBefore
                End If

                Dim obj As Object = Settings(SETTING_CSSBEFORE_KEY)
                If Not obj Is Nothing Then
                    Me.p_CssBefore = obj.ToString
                End If

                Return Me.p_CssBefore
            End Get
        End Property

        Protected ReadOnly Property Setting_Delay As Integer
            Get
                If Me.p_Delay.HasValue Then
                    Return Me.p_Delay.Value
                End If

                Dim obj As Object = Settings(SETTING_DELAY_KEY)
                If Not obj Is Nothing Then
                    Me.p_Delay = Integer.Parse(obj.ToString, Globalization.NumberStyles.Integer)
                Else
                    Me.p_Delay = 0
                End If

                Return Me.p_Delay.Value
            End Get
        End Property

        Protected ReadOnly Property Setting_EaseIn As String
            Get
                If Not String.IsNullOrEmpty(Me.p_EaseIn) Then
                    Return Me.p_EaseIn
                End If

                Dim obj As Object = Settings(SETTING_EASEIN_KEY)
                If Not obj Is Nothing Then
                    Me.p_EaseIn = obj.ToString
                End If

                Return Me.p_EaseIn
            End Get
        End Property

        Protected ReadOnly Property Setting_EaseOut As String
            Get
                If Not String.IsNullOrEmpty(Me.p_EaseOut) Then
                    Return Me.p_EaseOut
                End If

                Dim obj As Object = Settings(SETTING_EASEOUT_KEY)
                If Not obj Is Nothing Then
                    Me.p_EaseOut = obj.ToString
                End If

                Return Me.p_EaseOut
            End Get
        End Property

        Protected ReadOnly Property Setting_Easing As String
            Get
                If Not String.IsNullOrEmpty(Me.p_Easing) Then
                    Return Me.p_Easing
                End If

                Dim obj As Object = Settings(SETTING_EASING_KEY)
                If Not obj Is Nothing Then
                    Me.p_Easing = obj.ToString
                End If

                Return Me.p_Easing
            End Get
        End Property

        Protected ReadOnly Property Setting_End As String
            Get
                If Not String.IsNullOrEmpty(Me.p_End) Then
                    Return Me.p_End
                End If

                Dim obj As Object = Settings(SETTING_END_KEY)
                If Not obj Is Nothing Then
                    Me.p_End = obj.ToString
                End If

                Return Me.p_End
            End Get
        End Property

        Protected ReadOnly Property Setting_FastOnEvent As Integer
            Get
                If Me.p_FastOnEvent.HasValue Then
                    Return Me.p_FastOnEvent.Value
                End If

                Dim obj As Object = Settings(SETTING_FASTONEVENT_KEY)
                If Not obj Is Nothing Then
                    Me.p_FastOnEvent = Integer.Parse(obj.ToString, Globalization.NumberStyles.Integer)
                Else
                    Me.p_FastOnEvent = 0
                End If

                Return Me.p_FastOnEvent.Value
            End Get
        End Property

        Protected ReadOnly Property Setting_Fit As Integer
            Get
                If Me.p_Fit.HasValue Then
                    Return Me.p_Fit.Value
                End If

                Dim obj As Object = Settings(SETTING_FIT_KEY)
                If Not obj Is Nothing Then
                    Me.p_Fit = CType(IIf(String.Equals(obj.ToString(), "true", StringComparison.InvariantCultureIgnoreCase), 1, 0), Integer)
                Else
                    Me.p_Fit = 0
                End If

                Return Me.p_Fit.Value
            End Get
        End Property

        Protected ReadOnly Property Setting_Fx As String
            Get
                If Not String.IsNullOrEmpty(Me.p_Fx) Then
                    Return Me.p_Fx
                End If

                Dim obj As Object = Settings(SETTING_FX_KEY)
                If Not obj Is Nothing Then
                    Me.p_Fx = obj.ToString
                Else
                    Me.p_Fx = "fade"
                End If

                Return Me.p_Fx
            End Get
        End Property

        Protected ReadOnly Property Setting_FxFn As String
            Get
                If Not String.IsNullOrEmpty(Me.p_FxFn) Then
                    Return Me.p_FxFn
                End If

                Dim obj As Object = Settings(SETTING_FXFN_KEY)
                If Not obj Is Nothing Then
                    Me.p_FxFn = obj.ToString
                End If

                Return Me.p_FxFn
            End Get
        End Property

        Protected ReadOnly Property Setting_Height As String
            Get
                If Not String.IsNullOrEmpty(Me.p_Height) Then
                    Return Me.p_Height
                End If

                Dim obj As Object = Settings(SETTING_HEIGHT_KEY)
                If Not obj Is Nothing Then
                    Me.p_Height = obj.ToString
                Else
                    Me.p_Height = "auto"
                End If

                Return Me.p_Height
            End Get
        End Property

        Protected ReadOnly Property Setting_ManualTrump As Boolean
            Get
                If Me.p_ManualTrump.HasValue Then
                    Return Me.p_ManualTrump.Value
                End If

                Dim obj As Object = Settings(SETTING_MANUALTRUMP_KEY)
                If Not obj Is Nothing Then
                    Me.p_ManualTrump = Boolean.Parse(obj.ToString)
                Else
                    Me.p_ManualTrump = True
                End If

                Return Me.p_ManualTrump.Value
            End Get
        End Property

        Protected ReadOnly Property Setting_MetaAttr As String
            Get
                If Not String.IsNullOrEmpty(Me.p_MetaAttr) Then
                    Return Me.p_MetaAttr
                End If

                Dim obj As Object = Settings(SETTING_METAATTR_KEY)
                If Not obj Is Nothing Then
                    Me.p_MetaAttr = obj.ToString
                Else
                    Me.p_MetaAttr = "cycle"
                End If

                Return Me.p_MetaAttr
            End Get
        End Property

        Protected ReadOnly Property Setting_Next As String
            Get
                If Not String.IsNullOrEmpty(Me.p_Next) Then
                    Return Me.p_Next
                End If

                Dim obj As Object = Settings(SETTING_NEXT_KEY)
                If Not obj Is Nothing Then
                    Me.p_Next = obj.ToString
                End If

                Return Me.p_Next
            End Get
        End Property

        Protected ReadOnly Property Setting_NoWrap As Integer
            Get
                If Me.p_NoWrap.HasValue AndAlso (Me.p_NoWrap.Value = 0 Or Me.p_NoWrap = 1) Then
                    Return Me.p_NoWrap.Value
                End If

                Dim obj As Object = Settings(SETTING_NOWRAP_KEY)
                If Not obj Is Nothing Then
                    Me.p_NoWrap = CType(IIf(String.Equals(obj.ToString(), "true", StringComparison.InvariantCultureIgnoreCase), 1, 0), Integer)
                Else
                    Me.p_NoWrap = 0
                End If

                Return Me.p_NoWrap.Value
            End Get
        End Property

        Protected ReadOnly Property Setting_OnPagerEvent As String
            Get
                If Not String.IsNullOrEmpty(Me.p_OnPagerEvent) Then
                    Return Me.p_OnPagerEvent
                End If

                Dim obj As Object = Settings(SETTING_ONPAGEREVENT_KEY)
                If Not obj Is Nothing Then
                    Me.p_OnPagerEvent = obj.ToString
                End If

                Return Me.p_OnPagerEvent
            End Get
        End Property

        Protected ReadOnly Property Setting_OnPreviousNextEvent As String
            Get
                If Not String.IsNullOrEmpty(Me.p_OnPreviousNextEvent) Then
                    Return Me.p_OnPreviousNextEvent
                End If

                Dim obj As Object = Settings(SETTING_ONPREVIOUSNEXTEVENT_KEY)
                If Not obj Is Nothing Then
                    Me.p_OnPreviousNextEvent = obj.ToString
                End If

                Return Me.p_OnPreviousNextEvent
            End Get
        End Property

        Protected ReadOnly Property Setting_Pager As Boolean
            Get
                If Me.p_Pager.HasValue Then
                    Return Me.p_Pager.Value
                End If

                Dim obj As Object = Settings(SETTING_PAGER_KEY)
                If Not obj Is Nothing Then
                    Me.p_Pager = Boolean.Parse(obj.ToString)
                Else
                    Me.p_Pager = False
                End If

                Return Me.p_Pager.Value
            End Get
        End Property

        Protected ReadOnly Property Setting_PagerAnchorBuilder As String
            Get
                If Not String.IsNullOrEmpty(Me.p_PagerAnchorBuilder) Then
                    Return Me.p_PagerAnchorBuilder
                End If

                Dim obj As Object = Settings(SETTING_PAGERANCHORBUILDER_KEY)
                If Not obj Is Nothing Then
                    Me.p_PagerAnchorBuilder = obj.ToString
                End If

                Return Me.p_PagerAnchorBuilder
            End Get
        End Property

        Protected ReadOnly Property Setting_PagerEvent As String
            Get
                If Not String.IsNullOrEmpty(Me.p_PagerEvent) Then
                    Return Me.p_PagerEvent
                End If

                Dim obj As Object = Settings(SETTING_PAGEREVENT_KEY)
                If Not obj Is Nothing Then
                    Me.p_PagerEvent = obj.ToString
                Else
                    Me.p_PagerEvent = "click.cycle"
                End If

                Return Me.p_PagerEvent
            End Get
        End Property

        Protected ReadOnly Property Setting_Pause As Integer
            Get
                If Me.p_Pause.HasValue AndAlso (Me.p_Pause.Value = 0 Or Me.p_Pause = 1) Then
                    Return Me.p_Pause.Value
                End If

                Dim obj As Object = Settings(SETTING_PAUSE_KEY)
                If Not obj Is Nothing Then
                    Me.p_Pause = CType(IIf(String.Equals(obj.ToString(), "true", StringComparison.InvariantCultureIgnoreCase), 1, 0), Integer)
                Else
                    Me.p_Pause = 0
                End If

                Return Me.p_Pause.Value
            End Get
        End Property

        Protected ReadOnly Property Setting_PauseOnPagerHover As Integer
            Get
                If Me.p_PauseOnPagerHover.HasValue AndAlso (Me.p_PauseOnPagerHover.Value = 0 Or Me.p_PauseOnPagerHover = 1) Then
                    Return Me.p_PauseOnPagerHover.Value
                End If

                Dim obj As Object = Settings(SETTING_PAUSEONPAGERHOVER_KEY)
                If Not obj Is Nothing Then
                    Me.p_PauseOnPagerHover = CType(IIf(String.Equals(obj.ToString(), "true", StringComparison.InvariantCultureIgnoreCase), 1, 0), Integer)
                Else
                    Me.p_PauseOnPagerHover = 0
                End If

                Return Me.p_PauseOnPagerHover.Value
            End Get
        End Property

        Protected ReadOnly Property Setting_Prev As String
            Get
                If Not String.IsNullOrEmpty(Me.p_Prev) Then
                    Return Me.p_Prev
                End If

                Dim obj As Object = Settings(SETTING_PREV_KEY)
                If Not obj Is Nothing Then
                    Me.p_Prev = obj.ToString
                End If

                Return Me.p_Prev
            End Get
        End Property

        Protected ReadOnly Property Setting_PrevNextEvent As String
            Get
                If Not String.IsNullOrEmpty(Me.p_PrevNextEvent) Then
                    Return Me.p_PrevNextEvent
                End If

                Dim obj As Object = Settings(SETTING_PREVNEXTEVENT_KEY)
                If Not obj Is Nothing Then
                    Me.p_PrevNextEvent = obj.ToString
                Else
                    Me.p_PrevNextEvent = "click.cycle"
                End If

                Return Me.p_PrevNextEvent
            End Get
        End Property

        Protected ReadOnly Property Setting_Random As Integer
            Get
                If Me.p_Random.HasValue AndAlso (Me.p_Random.Value = 0 Or Me.p_Random = 1) Then
                    Return Me.p_Random.Value
                End If

                Dim obj As Object = Settings(SETTING_RANDOM_KEY)
                If Not obj Is Nothing Then
                    Me.p_Random = CType(IIf(String.Equals(obj.ToString(), "true", StringComparison.InvariantCultureIgnoreCase), 1, 0), Integer)
                Else
                    Me.p_Random = 0
                End If

                Return Me.p_Random.Value
            End Get
        End Property

        Protected ReadOnly Property Setting_RandomizeEffects As Integer
            Get
                If Me.p_RandomizeEffects.HasValue AndAlso (Me.p_RandomizeEffects.Value = 0 Or Me.p_RandomizeEffects = 1) Then
                    Return Me.p_RandomizeEffects.Value
                End If

                Dim obj As Object = Settings(SETTING_RANDOMIZEEFFECTS_KEY)
                If Not obj Is Nothing Then
                    Me.p_RandomizeEffects = CType(IIf(String.Equals(obj.ToString(), "true", StringComparison.InvariantCultureIgnoreCase), 1, 0), Integer)
                Else
                    Me.p_RandomizeEffects = 1
                End If

                Return Me.p_RandomizeEffects.Value
            End Get
        End Property

        Protected ReadOnly Property Setting_RequeueOnImageNotLoaded As Boolean
            Get
                If Me.p_RequeueOnImageNotLoaded.HasValue Then
                    Return Me.p_RequeueOnImageNotLoaded.Value
                End If

                Dim obj As Object = Settings(SETTING_REQUEUEONIMAGENOTLOADED_KEY)
                If Not obj Is Nothing Then
                    Me.p_RequeueOnImageNotLoaded = Boolean.Parse(obj.ToString)
                Else
                    Me.p_RequeueOnImageNotLoaded = True
                End If

                Return Me.p_RequeueOnImageNotLoaded.Value
            End Get
        End Property

        Protected ReadOnly Property Setting_RequeueTimeout As Integer
            Get
                If Me.p_RequeueTimeout.HasValue Then
                    Return Me.p_RequeueTimeout.Value
                End If

                Dim obj As Object = Settings(SETTING_REQUEUETIMEOUT_KEY)
                If Not obj Is Nothing Then
                    Me.p_RequeueTimeout = Integer.Parse(obj.ToString, Globalization.NumberStyles.Integer)
                Else
                    Me.p_RequeueTimeout = 250
                End If

                Return Me.p_RequeueTimeout.Value
            End Get
        End Property

        Protected ReadOnly Property Setting_Rev As Integer
            Get
                If Me.p_Rev.HasValue AndAlso (Me.p_Rev.Value = 0 Or Me.p_Rev = 1) Then
                    Return Me.p_Rev.Value
                End If

                Dim obj As Object = Settings(SETTING_REV_KEY)
                If Not obj Is Nothing Then
                    Me.p_Rev = CType(IIf(String.Equals(obj.ToString(), "true", StringComparison.InvariantCultureIgnoreCase), 1, 0), Integer)
                Else
                    Me.p_Rev = 0
                End If

                Return Me.p_Rev.Value
            End Get
        End Property

        Protected ReadOnly Property Setting_Shuffle As String
            Get
                If Not String.IsNullOrEmpty(Me.p_Shuffle) Then
                    Return Me.p_Shuffle
                End If

                Dim obj As Object = Settings(SETTING_SHUFFLE_KEY)
                If Not obj Is Nothing Then
                    Me.p_Shuffle = obj.ToString
                End If

                Return Me.p_Shuffle
            End Get
        End Property

        Protected ReadOnly Property Setting_SlideExpr As String
            Get
                If Not String.IsNullOrEmpty(Me.p_SlideExpr) Then
                    Return Me.p_SlideExpr
                End If

                Dim obj As Object = Settings(SETTING_SLIDEEXPR_KEY)
                If Not obj Is Nothing Then
                    Me.p_SlideExpr = obj.ToString
                End If

                Return Me.p_SlideExpr
            End Get
        End Property

        Protected ReadOnly Property Setting_SlideResize As Integer
            Get
                If Me.p_SlideResize.HasValue AndAlso (Me.p_SlideResize.Value = 0 Or Me.p_SlideResize = 1) Then
                    Return Me.p_SlideResize.Value
                End If

                Dim obj As Object = Settings(SETTING_SLIDERESIZE_KEY)
                If Not obj Is Nothing Then
                    Me.p_SlideResize = CType(IIf(String.Equals(obj.ToString(), "true", StringComparison.InvariantCultureIgnoreCase), 1, 0), Integer)
                Else
                    Me.p_SlideResize = 1
                End If

                Return Me.p_SlideResize.Value
            End Get
        End Property

        Protected ReadOnly Property Setting_Speed As Integer
            Get
                If Me.p_Speed.HasValue AndAlso (Me.p_Speed.Value = 0 Or Me.p_Speed = 1) Then
                    Return Me.p_Speed.Value
                End If

                Dim obj As Object = Settings(SETTING_SPEED_KEY)
                If Not obj Is Nothing Then
                    Me.p_Speed = Integer.Parse(obj.ToString, Globalization.NumberStyles.Integer)
                Else
                    Me.p_Speed = 1000
                End If

                Return Me.p_Speed.Value
            End Get
        End Property

        Protected ReadOnly Property Setting_SpeedIn As Integer
            Get
                If Me.p_SpeedIn.HasValue Then
                    Return Me.p_SpeedIn.Value
                End If

                Dim obj As Object = Settings(SETTING_SPEEDIN_KEY)
                If Not obj Is Nothing Then
                    Me.p_SpeedIn = Integer.Parse(obj.ToString, Globalization.NumberStyles.Integer)
                Else
                    Me.p_SpeedIn = Null.NullInteger
                End If

                Return Me.p_SpeedIn.Value
            End Get
        End Property

        Protected ReadOnly Property Setting_SpeedOut As Integer
            Get
                If Me.p_SpeedOut.HasValue Then
                    Return Me.p_SpeedOut.Value
                End If

                Dim obj As Object = Settings(SETTING_SPEEDOUT_KEY)
                If Not obj Is Nothing Then
                    Me.p_SpeedOut = Integer.Parse(obj.ToString, Globalization.NumberStyles.Integer)
                Else
                    Me.p_SpeedOut = Null.NullInteger
                End If

                Return Me.p_SpeedOut.Value
            End Get
        End Property

        Protected ReadOnly Property Setting_StartingSlide As Integer
            Get
                If Me.p_StartingSlide.HasValue AndAlso (Me.p_StartingSlide.Value = 0 Or Me.p_StartingSlide = 1) Then
                    Return Me.p_StartingSlide.Value
                End If

                Dim obj As Object = Settings(SETTING_STARTINGSLIDE_KEY)
                If Not obj Is Nothing Then
                    Me.p_StartingSlide = Integer.Parse(obj.ToString, Globalization.NumberStyles.Integer)
                Else
                    Me.p_StartingSlide = 0
                End If

                Return Me.p_StartingSlide.Value
            End Get
        End Property

        Protected ReadOnly Property Setting_Sync As Integer
            Get
                If Me.p_Sync.HasValue AndAlso (Me.p_Sync.Value = 0 Or Me.p_Sync = 1) Then
                    Return Me.p_Sync.Value
                End If

                Dim obj As Object = Settings(SETTING_SYNC_KEY)
                If Not obj Is Nothing Then
                    Dim blnValue As Boolean = Boolean.Parse(obj.ToString)
                    If blnValue Then
                        Me.p_Sync = 1
                    Else
                        Me.p_Sync = 0
                    End If
                Else
                    Me.p_Sync = 1
                End If

                Return Me.p_Sync.Value
            End Get
        End Property

        Protected ReadOnly Property Setting_Timeout As Integer
            Get
                If Me.p_Timeout.HasValue Then
                    Return Me.p_Timeout.Value
                End If

                Dim obj As Object = Settings(SETTING_TIMEOUT_KEY)
                If Not obj Is Nothing Then
                    Me.p_Timeout = Integer.Parse(obj.ToString, Globalization.NumberStyles.Integer)
                Else
                    Me.p_Timeout = 4000
                End If

                Return Me.p_Timeout.Value
            End Get
        End Property

        Protected ReadOnly Property Setting_TimeoutFn As String
            Get
                If Not String.IsNullOrEmpty(Me.p_TimeoutFn) Then
                    Return Me.p_TimeoutFn
                End If

                Dim obj As Object = Settings(SETTING_TIMEOUTFN_KEY)
                If Not obj Is Nothing Then
                    Me.p_TimeoutFn = obj.ToString
                End If

                Return Me.p_TimeoutFn
            End Get
        End Property

        Protected ReadOnly Property Setting_UpdateActivePagerLink As String
            Get
                If Not String.IsNullOrEmpty(Me.p_UpdateActivePagerLink) Then
                    Return Me.p_UpdateActivePagerLink
                End If

                Dim obj As Object = Settings(SETTING_UPDATEACTIVEPAGERLINK_KEY)
                If Not obj Is Nothing Then
                    Me.p_UpdateActivePagerLink = obj.ToString
                End If

                Return Me.p_UpdateActivePagerLink
            End Get
        End Property

        Protected ReadOnly Property Setting_Width As String
            Get
                If Not String.IsNullOrEmpty(Me.p_Width) Then
                    Return Me.p_Width
                End If

                Dim obj As Object = Settings(SETTING_WIDTH_KEY)
                If Not obj Is Nothing Then
                    Me.p_Width = obj.ToString
                End If

                Return Me.p_Width
            End Get
        End Property


        Protected ReadOnly Property Setting_ExcludeCycle As Boolean
            Get
                If Me.p_ExcludeCycle.HasValue Then
                    Return Me.p_ExcludeCycle.Value
                End If

                Dim obj As Object = Settings(SETTING_EXCLUDECYCLE_KEY)
                If Not obj Is Nothing Then
                    Me.p_ExcludeCycle = Boolean.Parse(obj.ToString)
                Else
                    Me.p_ExcludeCycle = False
                End If

                Return Me.p_ExcludeCycle.Value
            End Get
        End Property

        Protected ReadOnly Property Setting_ExcludeEasing As Boolean
            Get
                If Me.p_ExcludeEasing.HasValue Then
                    Return Me.p_ExcludeEasing.Value
                End If

                Dim obj As Object = Settings(SETTING_EXCLUDEEASING_KEY)
                If Not obj Is Nothing Then
                    Me.p_ExcludeEasing = Boolean.Parse(obj.ToString)
                Else
                    Me.p_ExcludeEasing = False
                End If

                Return Me.p_ExcludeEasing.Value
            End Get
        End Property

        Protected ReadOnly Property Setting_CacheDuration As Integer
            Get
                If p_CacheDuration.HasValue Then
                    Return p_CacheDuration.Value
                End If

                Dim obj As Object = Settings(SETTING_CACHEDURATION_KEY)
                If Not obj Is Nothing Then
                    p_CacheDuration = Integer.Parse(obj.ToString, Globalization.NumberStyles.Integer)
                Else
                    p_CacheDuration = 5
                End If

                Return p_CacheDuration.Value
            End Get
        End Property

#End Region

#Region " Localization "

        Protected Overloads Function GetLocalizedString(ByVal LocalizationKey As String) As String
            If Not String.IsNullOrEmpty(LocalizationKey) Then
                Return Localization.GetString(LocalizationKey, Me.LocalResourceFile)
            Else
                Return String.Empty
            End If
        End Function

        Protected Overloads Function GetLocalizedString(ByVal LocalizationKey As String, ByVal LocalResourceFilePath As String) As String
            If Not String.IsNullOrEmpty(LocalizationKey) Then
                Return Localization.GetString(LocalizationKey, LocalResourceFilePath)
            Else
                Return String.Empty
            End If
        End Function

#End Region

#Region " Event Handlers "

        Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            ' request that the DNN framework load the jQuery script into the markup
            JavaScript.RequestRegistration(CommonJs.DnnPlugins)

        End Sub

#End Region

    End Class

End Namespace