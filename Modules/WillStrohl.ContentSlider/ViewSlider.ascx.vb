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

Imports System.Resources
Imports DotNetNuke.Services.Cache
Imports DotNetNuke.Common
Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Entities.Modules
Imports DotNetNuke.UI.Skins.Skin
Imports DotNetNuke.UI.Skins.Controls.ModuleMessage
Imports DotNetNuke.Services.Exceptions
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Web.Caching
Imports System.Web.UI
Imports DotNetNuke.Framework.JavaScriptLibraries
Imports DotNetNuke.Web.Client
Imports DotNetNuke.Web.Client.ClientResourceManagement
Imports DotNetNuke.Web.Client.Providers
Imports WillStrohl.Modules.ContentSlider.SliderController

Namespace WillStrohl.Modules.ContentSlider

    Partial Public MustInherit Class ViewSlider
        Inherits WnsPortalModuleBase
        Implements IActionable

#Region " Private Members "

        Private Const c_Edit As String = "Edit"
        Private Const c_Options As String = "SliderOptions"
        Private Const c_Sliders As String = "Sliders"
        Private Const c_MenuItem_Title As String = "Slider.MenuItem.Edit"
        Private Const c_MenuItem_Options As String = "Slider.MenuItem.Options"
        Private Const c_MenuItem_Sliders As String = "Slider.MenuItem.Sliders"
        Private Const CYCLE_KEY As String = "jQuery.Plugin.Cycle"
        Private Const EASING_KEY As String = "jQuery.Plugin.Easing"
        'Private Const SCRIPT_TAG_FORMAT As String = "<script language=""javascript"" type=""text/javascript"" src=""{0}""></script>"

        Private p_Sliders As SliderInfoCollection = Nothing

#End Region

#Region " Private Properties "

        Private ReadOnly Property Sliders() As SliderInfoCollection
            Get
                If Me.p_Sliders Is Nothing Then
                    Dim ctlSlider As New SliderController
                    Me.p_Sliders = ctlSlider.GetSliders(Me.ModuleId)
                End If

                Return Me.p_Sliders
            End Get
        End Property

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
                ' Cannot ignore on postbacks, because the content will disappear
                'If Not Page.IsPostBack Then
                Me.BindData()
                'End If

                If Not Me.Setting_ExcludeCycle And Not Page.ClientScript.IsClientScriptBlockRegistered(CYCLE_KEY) Then

                    ClientResourceManager.RegisterScript(Page, String.Concat(Me.ControlPath, "js/jquery.cycle.min.js"), FileOrder.Js.jQuery + 1, DnnPageHeaderProvider.DefaultName, CYCLE_KEY, "2.9995")

                    'Page.ClientScript.RegisterClientScriptBlock( _
                    '    Me.GetType, _
                    '    CYCLE_KEY, _
                    '    String.Format(SCRIPT_TAG_FORMAT, String.Concat(Me.ControlPath, "js/jquery.cycle.min.js")), _
                    '    False)
                End If

                If Not Me.Setting_ExcludeEasing And Not Page.ClientScript.IsClientScriptBlockRegistered(EASING_KEY) Then

                    ClientResourceManager.RegisterScript(Page, String.Concat(Me.ControlPath, "js/jquery.easing.compatibility.js"), FileOrder.Js.jQuery + 2, DnnPageHeaderProvider.DefaultName, EASING_KEY, "1")

                    'Page.ClientScript.RegisterClientScriptBlock( _
                    '    Me.GetType, _
                    '    EASING_KEY, _
                    '    String.Format(SCRIPT_TAG_FORMAT, String.Concat(Me.ControlPath, "js/jquery.easing.compatibility.js")), _
                    '    False)
                End If
            Catch exc As Exception ' Module failed to load
                ProcessModuleLoadException(Me, exc, Me.IsEditable)
            End Try
        End Sub

#End Region

#Region " Helper Methods "

        Private Sub BindData()

            If Not Me.Sliders Is Nothing AndAlso Me.Sliders.Count > 0 Then

                Me.AppendSliders()

                Me.AppendScript()

            Else
                AddModuleMessage(Me, Me.GetLocalizedString("Error.NoSliders"), ModuleMessageType.BlueInfo)

                Me.phScript.Visible = False
                Me.phSlider.Visible = False
            End If

        End Sub

        Private Sub AppendSliders()

            If Not DataCache.GetCache(SliderCacheName) Is Nothing Then
                phSlider.Controls.Add(New LiteralControl(DataCache.GetCache(SliderCacheName).ToString()))
                Exit Sub
            End If

            Dim ctlSlider As New SliderController
            Dim sbSlider As New StringBuilder(20)

            With sbSlider
                .AppendFormat("<div id=""divWnsSlideShow_{0}"" class=""wns-slideshow-wrapper wns-slideshow-{0} dnnClear"">", TabModuleId)

                For Each oSlider As SliderInfo In Me.Sliders

                    ' use this to get the image paths 
                    ' FormatURL(Media.NavigateUrl, Media.TrackClicks, ModuleConfig.TabID, ModuleConfig.ModuleID)

                    Dim strFile As String = ctlSlider.GetFileById(oSlider.SliderContent, ModuleId, Me.PortalSettings)

                    If String.IsNullOrEmpty(strFile) Then
                        ' file doesn't exist
                        ctlSlider.DeleteSlider(oSlider.SliderId)
                        Exit For
                    End If

                    If Not String.IsNullOrEmpty(oSlider.Link) And oSlider.NewWindow Then
                        .AppendFormat("<a title=""{3}"" href=""{0}"" class=""wns-slider-item-{1} wns-slider-item-link-{2}"" target=""_blank"">", ParseLink(oSlider.Link), TabModuleId, oSlider.DisplayOrder, oSlider.AlternateText)
                    ElseIf Not String.IsNullOrEmpty(oSlider.Link) Then
                        .AppendFormat("<a title=""{3}"" href=""{0}"" class=""wns-slider-item-{1} wns-slider-item-link-{2}"">", ParseLink(oSlider.Link), TabModuleId, oSlider.DisplayOrder, oSlider.AlternateText)
                    End If

                    .AppendFormat("<img src=""{3}"" alt=""{0}"" title=""{0}"" class=""wns-slideshow-image wns-slider-item-{1} wns-slideshow-image-{2}"" />", oSlider.AlternateText, TabModuleId, oSlider.DisplayOrder, strFile)

                    If Not String.IsNullOrEmpty(oSlider.Link) Then
                        .Append("</a>")
                    End If

                Next

                .Append("</div>")

                If Setting_Pager Then
                    .AppendFormat("<ol class=""wnsSliderPager wnsSliderPager{0}"">", TabModuleId)
                End If
            End With

            Dim dep As DNNCacheDependency = Nothing
            DataCache.SetCache(SliderCacheName, sbSlider.ToString(), dep, DateTime.UtcNow.AddMinutes(Setting_CacheDuration), Cache.NoSlidingExpiration, CacheItemPriority.Default, Nothing)

            Me.phSlider.Controls.Add(New LiteralControl(sbSlider.ToString))

        End Sub

        Private Sub AppendScript()

            If Not DataCache.GetCache(ScriptCacheName) Is Nothing Then
                phScript.Controls.Add(New LiteralControl(DataCache.GetCache(ScriptCacheName).ToString()))
                Exit Sub
            End If

            Dim sbScript As New StringBuilder(50)
            GetScriptLine(sbScript, "<script language=""javascript"" type=""text/javascript"">/*<![CDATA[*/")

            'GetScriptLine(sbScript, "(function ($, Sys) {")
            'GetScriptLine(sbScript, "   function setupDnnSiteSettings() {")

            Dim strScriptName As String = String.Concat("   function initWnsContentSlider", TabModuleId, "() {")
            GetScriptLine(sbScript, strScriptName)

            'If Setting_Pager Then
            '    Dim strPager As String = String.Format("            jQuery('#divWnsSlideShow_{0}').after('<ol class=""wnsSliderPager wnsSliderPager{0}"">');", TabModuleId)

            '    GetScriptLine(sbScript, String.Concat("        if (jQuery('.wnsSliderPager.wnsSliderPager", TabModuleId, "').length == 0) {"))
            '    GetScriptLine(sbScript, strPager)
            '    GetScriptLine(sbScript, "        }")
            'End If

            Dim strCycle As String = String.Format("        jQuery('#divWnsSlideShow_{0}').cycle({1});", TabModuleId, GetSettingsForScript)
            GetScriptLine(sbScript, strCycle)

            'GetScriptLine(sbScript, "   }")

            'GetScriptLine(sbScript, "   $(document).ready(function () {")
            'GetScriptLine(sbScript, "       setupDnnSiteSettings();")
            'GetScriptLine(sbScript, "       Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {")
            'GetScriptLine(sbScript, "           setupDnnSiteSettings();")
            'GetScriptLine(sbScript, "       });")
            'GetScriptLine(sbScript, "   });")
            'GetScriptLine(sbScript, "} (jQuery, window.Sys));")

            GetScriptLine(sbScript, "   }")

            GetScriptLine(sbScript, "/*]]>*/</script>")

            Dim dep As DNNCacheDependency = Nothing
            DataCache.SetCache(ScriptCacheName, sbScript.ToString(), dep, DateTime.UtcNow.AddMinutes(Setting_CacheDuration), Cache.NoSlidingExpiration, CacheItemPriority.Default, Nothing)

            Me.phScript.Controls.Add(New LiteralControl(sbScript.ToString))

        End Sub

        Private Function GetSettingsForScript() As String

            Dim sb As New StringBuilder(20)

            If Not String.IsNullOrEmpty(Me.Setting_ActivePagerClass) And Not String.Equals(Me.Setting_ActivePagerClass, "activeSlide", StringComparison.InvariantCultureIgnoreCase) Then
                sb.AppendFormat("activePagerClass: '{0}',", Me.Setting_ActivePagerClass)
            End If

            If Not String.IsNullOrEmpty(Me.Setting_After) Then
                sb.AppendFormat("after: {0},", Me.Setting_After)
            End If

            If Me.Setting_AllowPagerClickBubble Then
                sb.Append("allowPagerClickBubble: true,")
            End If

            If Not String.IsNullOrEmpty(Me.Setting_AnimationIn) Then
                sb.AppendFormat("animIn: '{0}',", Me.Setting_AnimationIn)
            End If

            If Not String.IsNullOrEmpty(Me.Setting_AnimationOut) Then
                sb.AppendFormat("animOut: '{0}',", Me.Setting_AnimationOut)
            End If

            If Me.Setting_AutoStop = 1 Then
                sb.Append("autostop: 1,")
            End If

            If Me.Setting_AutoStopCount > 0 Then
                sb.AppendFormat("autostopCount: {0},", Me.Setting_AutoStopCount)
            End If

            If Me.Setting_Backwards Then
                sb.Append("backwards: true,")
            End If

            If Not String.IsNullOrEmpty(Me.Setting_Before) Then
                sb.AppendFormat("before: {0},", Me.Setting_Before)
            End If

            If Me.Setting_ContainerResize = 0 Then
                sb.Append("containerResize: 0,")
            End If

            If Me.Setting_Continuous = 1 Then
                sb.Append("continuous: 1,")
            End If

            If Not String.IsNullOrEmpty(Me.Setting_CssAfter) Then
                sb.AppendFormat("cssAfter: '{0}',", Me.Setting_CssAfter)
            End If

            If Not String.IsNullOrEmpty(Me.Setting_CssBefore) Then
                sb.AppendFormat("cssBefore: '{0}',", Me.Setting_CssBefore)
            End If

            If Me.Setting_Delay <> 0 Then
                sb.AppendFormat("delay: {0},", Me.Setting_Delay)
            End If

            If Not String.IsNullOrEmpty(Me.Setting_EaseIn) Then
                sb.AppendFormat("easeIn: '{0}',", Me.Setting_EaseIn)
            End If

            If Not String.IsNullOrEmpty(Me.Setting_EaseOut) Then
                sb.AppendFormat("easeOut: '{0}',", Me.Setting_EaseOut)
            End If

            If Not String.IsNullOrEmpty(Me.Setting_Easing) Then
                sb.AppendFormat("easing: '{0}',", Me.Setting_Easing)
            End If

            If Not String.IsNullOrEmpty(Me.Setting_End) Then
                sb.AppendFormat("end: {0},", Me.Setting_End)
            End If

            If Me.Setting_FastOnEvent > 0 Then
                sb.AppendFormat("fastOnEvent: {0},", Me.Setting_FastOnEvent)
            End If

            If Me.Setting_Fit = 1 Then
                sb.Append("fit: 1,")
            End If

            If Not String.Equals(Me.Setting_Fx, "fade", StringComparison.InvariantCultureIgnoreCase) Then
                sb.AppendFormat("fx: '{0}',", Me.Setting_Fx)
            End If

            If Not String.IsNullOrEmpty(Me.Setting_FxFn) Then
                sb.AppendFormat("fxFn: {0},", Me.Setting_FxFn)
            End If

            If Not String.Equals(Me.Setting_Height, "auto", StringComparison.InvariantCultureIgnoreCase) Then
                sb.AppendFormat("height: '{0}',", Me.Setting_Height)
            End If

            If Not Me.Setting_ManualTrump Then
                sb.Append("manualTrump: false,")
            End If

            If Not String.Equals(Me.Setting_MetaAttr, "cycle", StringComparison.InvariantCultureIgnoreCase) Then
                sb.AppendFormat("metaAttr: '{0}',", Me.Setting_MetaAttr)
            End If

            If Not String.IsNullOrEmpty(Me.Setting_Next) Then
                sb.AppendFormat("next: {0},", Me.Setting_Next)
            End If

            If Me.Setting_NoWrap = 1 Then
                sb.Append("nowrap: 1,")
            End If

            If Not String.IsNullOrEmpty(Me.Setting_OnPagerEvent) Then
                sb.AppendFormat("onPagerEvent: {0},", Me.Setting_OnPagerEvent)
            End If

            If Not String.IsNullOrEmpty(Me.Setting_OnPreviousNextEvent) Then
                sb.AppendFormat("onPrevNextEvent: {0},", Me.Setting_OnPreviousNextEvent)
            End If

            If Setting_Pager Then
                sb.AppendFormat("pager: '.wnsSliderPager{0}',", TabModuleId)
            End If

            If Not String.IsNullOrEmpty(Me.Setting_PagerAnchorBuilder) Then
                sb.AppendFormat("pagerAnchorBuilder: {0},", Me.Setting_PagerAnchorBuilder)
            End If

            If Not String.Equals(Me.Setting_PagerEvent, "click.cycle", StringComparison.InvariantCultureIgnoreCase) Then
                sb.AppendFormat("pagerEvent: '{0}',", Me.Setting_PagerEvent)
            End If

            If Me.Setting_Pause = 1 Then
                sb.Append("pause: 1,")
            End If

            If Me.Setting_PauseOnPagerHover = 1 Then
                sb.Append("pauseOnPagerHover: 1,")
            End If

            If Not String.IsNullOrEmpty(Me.Setting_Prev) Then
                sb.AppendFormat("prev: {0},", Me.Setting_Prev)
            End If

            If Not String.Equals(Me.Setting_PrevNextEvent, "click.cycle", StringComparison.InvariantCultureIgnoreCase) Then
                sb.AppendFormat("prevNextEvent: '{0}',", Me.Setting_PrevNextEvent)
            End If

            If Me.Setting_Random = 1 Then
                sb.Append("random: 1,")
            End If

            If Me.Setting_RandomizeEffects = 0 Then
                sb.Append("randomizeEffects: 0,")
            End If

            If Not Me.Setting_RequeueOnImageNotLoaded Then
                sb.Append("requequeOnImageNotLoaded: false,")
            End If

            If Me.Setting_RequeueTimeout <> 250 Then
                sb.AppendFormat("requeueTimeout: {0},", Me.Setting_RequeueTimeout)
            End If

            If Me.Setting_Rev = 1 Then
                sb.Append("rev: 1,")
            End If

            If Not String.IsNullOrEmpty(Me.Setting_Shuffle) Then
                sb.AppendFormat("shuffle: {0},", Me.Setting_Shuffle)
            End If

            If Not String.IsNullOrEmpty(Me.Setting_SlideExpr) Then
                sb.AppendFormat("slideExpr: {0},", Me.Setting_SlideExpr)
            End If

            If Me.Setting_SlideResize = 0 Then
                sb.Append("slideResize: 0,")
            End If

            If Me.Setting_Speed <> 1000 Then
                sb.AppendFormat("speed: {0},", Me.Setting_Speed)
            End If

            If Me.Setting_SpeedIn > Null.NullInteger Then
                sb.AppendFormat("speedIn: {0},", Me.Setting_SpeedIn)
            End If

            If Me.Setting_SpeedOut > Null.NullInteger Then
                sb.AppendFormat("speedOut: {0},", Me.Setting_SpeedOut)
            End If

            If Me.Setting_StartingSlide > 0 Then
                sb.AppendFormat("startingSlide: {0},", Me.Setting_StartingSlide)
            End If

            If Me.Setting_Sync = 0 Then
                sb.Append("sync: 0,")
            End If

            If Me.Setting_Timeout <> 4000 Then
                sb.AppendFormat("timeout: {0},", Me.Setting_Timeout)
            End If

            If Not String.IsNullOrEmpty(Me.Setting_TimeoutFn) Then
                sb.AppendFormat("timeoutFn: {0},", Me.Setting_TimeoutFn)
            End If

            If Not String.IsNullOrEmpty(Me.Setting_UpdateActivePagerLink) Then
                sb.AppendFormat("updateActivePagerLink: {0},", Me.Setting_UpdateActivePagerLink)
            End If

            If Not String.IsNullOrEmpty(Me.Setting_Width) Then
                sb.AppendFormat("width: '{0}',", Me.Setting_Width)
            End If

            ' generate and return the script settings

            Dim strReturn As String = sb.ToString

            If Not String.IsNullOrEmpty(strReturn) Then
                strReturn = Regex.Replace(strReturn, ",$", "")
                strReturn = String.Concat("{", strReturn, "}")
            End If

            Return strReturn

        End Function

        Private Sub GetScriptLine(ByRef sb As StringBuilder, ByVal Value As String)

#If DEBUG Then
            sb.Append(Environment.NewLine)
            sb.Append(Value)
#Else
            sb.Append(Value.Trim)
#End If

        End Sub

        Protected Function GetEditUrl(ByVal GalleryId As Object) As String
            If Not GalleryId Is Nothing Then
                Return EditUrl("AlbumId", GalleryId.ToString, "Edit")
            Else
                Return "#"
            End If
        End Function

        ''' <summary>
        ''' EncodeUrl - this method switches any ampersands with the encoded versions of the ampersands for XHTML compliance
        ''' </summary>
        ''' <param name="URL">String - the URL to encode</param>
        ''' <returns></returns>
        ''' <remarks>This method does not check to see if the encoding has already been done.</remarks>
        ''' <history>
        ''' </history>
        Public Shared Function EncodeUrl(ByVal Url As String) As String
            Return Regex.Replace(Url, "&", "&amp;")
        End Function

        Private Function ParseLink(ByVal Link As String) As String
            If Regex.IsMatch(Link, "^\d+$", RegexOptions.IgnoreCase) Then
                Return EncodeUrl(Globals.LinkClick(Link, Me.TabId, Me.ModuleId, False))
            Else
                Return EncodeUrl(Link)
            End If
        End Function

#End Region

#Region " IActionable Implementation "

        Public ReadOnly Property ModuleActions() As DotNetNuke.Entities.Modules.Actions.ModuleActionCollection Implements IActionable.ModuleActions
            Get
                Dim Actions As New Actions.ModuleActionCollection

                Actions.Add(GetNextActionID, Me.GetLocalizedString(c_MenuItem_Title), _
                    String.Empty, String.Empty, String.Empty, _
                    EditUrl(String.Empty, String.Empty, c_Edit), _
                    False, DotNetNuke.Security.SecurityAccessLevel.Edit, True, False)

                If Me.Sliders.Count > 0 Then

                    Actions.Add(GetNextActionID, Me.GetLocalizedString(c_MenuItem_Sliders), _
                        String.Empty, String.Empty, String.Empty, _
                        EditUrl(String.Empty, String.Empty, c_Sliders), _
                        False, DotNetNuke.Security.SecurityAccessLevel.Edit, True, False)

                End If

                Actions.Add(GetNextActionID, Me.GetLocalizedString(c_MenuItem_Options), _
                    String.Empty, String.Empty, String.Empty, _
                    EditUrl(String.Empty, String.Empty, c_Options), _
                    False, DotNetNuke.Security.SecurityAccessLevel.Edit, True, False)

                Return Actions
            End Get
        End Property

#End Region

    End Class

End Namespace