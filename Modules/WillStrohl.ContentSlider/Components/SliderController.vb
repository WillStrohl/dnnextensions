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

Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Entities.Modules
Imports DotNetNuke.Services.FileSystem
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Xml

Namespace WillStrohl.Modules.ContentSlider

    Public NotInheritable Class SliderController
        Implements IPortable

#Region " Constants "

        Public Const SETTING_ACTIVEPAGERCLASS_KEY As String = "activePagerClass"
        Public Const SETTING_AFTER_KEY As String = "after"
        Public Const SETTING_ALLOWPAGERCLICKBUBBLE_KEY As String = "allowPagerClickBubble"
        Public Const SETTING_ANIMIN_KEY As String = "animIn"
        Public Const SETTING_ANIMOUT_KEY As String = "animOut"
        Public Const SETTING_AUTOSTOP_KEY As String = "autostop"
        Public Const SETTING_AUTOSTOPCOUNT_KEY As String = "autostopCount"
        Public Const SETTING_BACKWARDS_KEY As String = "backwards"
        Public Const SETTING_BEFORE_KEY As String = "before"
        Public Const SETTING_CONTAINERRESIZE_KEY As String = "containerResize"
        Public Const SETTING_CONTINUOUS_KEY As String = "continuous"
        Public Const SETTING_CSSAFTER_KEY As String = "cssAfter"
        Public Const SETTING_CSSBEFORE_KEY As String = "cssBefore"
        Public Const SETTING_DELAY_KEY As String = "delay"
        Public Const SETTING_EASEIN_KEY As String = "easeIn"
        Public Const SETTING_EASEOUT_KEY As String = "easeOut"
        Public Const SETTING_EASING_KEY As String = "easing"
        Public Const SETTING_END_KEY As String = "end"
        Public Const SETTING_FASTONEVENT_KEY As String = "fastOnEvent"
        Public Const SETTING_FIT_KEY As String = "fit"
        Public Const SETTING_FX_KEY As String = "fx"
        Public Const SETTING_FXFN_KEY As String = "fxFn"
        Public Const SETTING_HEIGHT_KEY As String = "height"
        Public Const SETTING_MANUALTRUMP_KEY As String = "manualTrump"
        Public Const SETTING_METAATTR_KEY As String = "metaAttr"
        Public Const SETTING_NEXT_KEY As String = "next"
        Public Const SETTING_NOWRAP_KEY As String = "nowrap"
        Public Const SETTING_ONPAGEREVENT_KEY As String = "onPagerEvent"
        Public Const SETTING_ONPREVIOUSNEXTEVENT_KEY As String = "onPrevNextEvent"
        Public Const SETTING_PAGER_KEY As String = "pager"
        Public Const SETTING_PAGERANCHORBUILDER_KEY As String = "pagerAnchorBuilder"
        Public Const SETTING_PAGEREVENT_KEY As String = "pagerEvent"
        Public Const SETTING_PAUSE_KEY As String = "pause"
        Public Const SETTING_PAUSEONPAGERHOVER_KEY As String = "pauseOnPagerHover"
        Public Const SETTING_PREV_KEY As String = "prev"
        Public Const SETTING_PREVNEXTEVENT_KEY As String = "prevNextEvent"
        Public Const SETTING_RANDOM_KEY As String = "random"
        Public Const SETTING_RANDOMIZEEFFECTS_KEY As String = "randomizeEffects"
        Public Const SETTING_REQUEUEONIMAGENOTLOADED_KEY As String = "requeueOnImageNotLoaded"
        Public Const SETTING_REQUEUETIMEOUT_KEY As String = "requeuetimeout"
        Public Const SETTING_REV_KEY As String = "rev"
        Public Const SETTING_SHUFFLE_KEY As String = "shuffle"
        Public Const SETTING_SLIDEEXPR_KEY As String = "slideExpr"
        Public Const SETTING_SLIDERESIZE_KEY As String = "slideResize"
        Public Const SETTING_SPEED_KEY As String = "speed"
        Public Const SETTING_SPEEDIN_KEY As String = "speedIn"
        Public Const SETTING_SPEEDOUT_KEY As String = "speedOut"
        Public Const SETTING_STARTINGSLIDE_KEY As String = "startingSlide"
        Public Const SETTING_SYNC_KEY As String = "sync"
        Public Const SETTING_TIMEOUT_KEY As String = "timeout"
        Public Const SETTING_TIMEOUTFN_KEY As String = "timeoutFn"
        Public Const SETTING_UPDATEACTIVEPAGERLINK_KEY As String = "updateActivePagerLink"
        Public Const SETTING_WIDTH_KEY As String = "width"
        Public Const SETTING_INCLUDEPAGER_KEY As String = "includePager"
        Public Const SETTING_EXCLUDECYCLE_KEY As String = "excludeCycle"
        Public Const SETTING_EXCLUDEEASING_KEY As String = "excludeEasing"
        Public Const SETTING_CACHEDURATION_KEY As String = "cacheDuration"

        Public Const SLIDER_CACHE_KEY_FORMAT As String = "WnsContentSlider-Sliders-{0}-{1}"
        Public Const SCRIPT_CACHE_KEY_FORMAT As String = "WnsContentSlider-Script-{0}-{1}"

        Private Const FILEID_MATCH_PATTERN As String = "^FileID=\d+$"

#End Region

#Region " Data Access "

        Public Function AddSlider(ByVal Slider As SliderInfo) As Integer
            Return DataProvider.Instance().AddSlider(Slider.ModuleId, Slider.SliderName, Slider.SliderContent, Slider.AlternateText, Slider.Link, Slider.NewWindow, Slider.DisplayOrder, Slider.LastUpdatedBy, Slider.StartDate, Slider.EndDate)
        End Function
        Public Sub UpdateSlider(ByVal Slider As SliderInfo)
            DataProvider.Instance().UpdateSlider(Slider.SliderId, Slider.ModuleId, Slider.SliderName, Slider.SliderContent, Slider.AlternateText, Slider.Link, Slider.NewWindow, Slider.DisplayOrder, Slider.LastUpdatedBy, Slider.StartDate, Slider.EndDate)
        End Sub
        Public Sub DeleteSlider(ByVal SliderId As Integer)
            DataProvider.Instance().DeleteSlider(SliderId)
        End Sub
        Public Function GetSlider(ByVal SliderId As Integer) As SliderInfo
            Dim objSlider As New SliderInfo
            objSlider.Fill(DataProvider.Instance().GetSlider(SliderId))
            Return objSlider
        End Function
        Public Function GetSliders(ByVal ModuleId As Integer) As SliderInfoCollection
            Dim collSlider As New SliderInfoCollection
            collSlider.Fill(DataProvider.Instance().GetSliders(ModuleId))
            Return collSlider
        End Function
        Public Function GetSlidersForEdit(ByVal ModuleId As Integer) As SliderInfoCollection
            Dim collSlider As New SliderInfoCollection
            collSlider.Fill(DataProvider.Instance().GetSlidersForEdit(ModuleId))
            Return collSlider
        End Function

#End Region

        Public Function GetFileById(ByVal File As String, ByVal ModuleId As Integer, ByVal pSettings As Entities.Portals.PortalSettings) As String

            If Not Regex.IsMatch(File, FILEID_MATCH_PATTERN, RegexOptions.IgnoreCase) Then
                Return File
            End If

            Dim intFile As Integer = Integer.Parse(Regex.Replace(File, "[^\d]", String.Empty, RegexOptions.IgnoreCase), Globalization.NumberStyles.Integer)
            Dim oFile As IFileInfo = FileManager.Instance().GetFile(intFile)
            Dim strPath As String = String.Empty

            If Not oFile Is Nothing Then
                Select Case oFile.StorageLocation
                    Case FolderController.StorageLocationTypes.DatabaseSecure, FolderController.StorageLocationTypes.SecureFileSystem
                        strPath = LinkClick(File, pSettings.ActiveTab.TabID, ModuleId, False)
                    Case Else
                        strPath = String.Concat(pSettings.HomeDirectory, oFile.RelativePath)
                End Select
            End If

            Return strPath

        End Function

#Region " IPortable Implementation "

        Public Function ExportModule(ByVal ModuleID As Integer) As String Implements DotNetNuke.Entities.Modules.IPortable.ExportModule
            Dim sb As New StringBuilder(150)
            Dim collSlider As New SliderInfoCollection
            collSlider = GetSliders(ModuleID)

            sb.Append("<WillStrohl><Sliders>")
            For Each obj As SliderInfo In collSlider
                sb.Append("<Slider>")

                sb.AppendFormat("<SliderId>{0}</SliderId>", obj.SliderId)
                sb.AppendFormat("<ModuleId>{0}</ModuleId>", obj.ModuleId)
                sb.AppendFormat("<SliderName><![CDATA[{0}]]></SliderName>", XmlUtils.XMLEncode(obj.SliderName))
                sb.AppendFormat("<SliderContent><![CDATA[{0}]]></SliderContent>", XmlUtils.XMLEncode(obj.SliderContent))
                sb.AppendFormat("<AlternateText><![CDATA[{0}]]></AlternateText>", XmlUtils.XMLEncode(obj.AlternateText))
                sb.AppendFormat("<DisplayOrder>{0}</DisplayOrder>", obj.DisplayOrder)
                sb.AppendFormat("<LastUpdatedBy>{0}</LastUpdatedBy>", obj.LastUpdatedBy)
                sb.AppendFormat("<LastUpdatedDate>{0}</LastUpdatedDate>", obj.LastUpdatedDate)

                sb.Append("</Slider>")
            Next
            sb.Append("</Sliders>")

            ' later on, will probably need to add module settings here

            sb.Append("</WillStrohl>")

            Return sb.ToString
        End Function

        Public Sub ImportModule(ByVal ModuleID As Integer, ByVal Content As String, ByVal Version As String, ByVal UserID As Integer) Implements DotNetNuke.Entities.Modules.IPortable.ImportModule
            Dim injContents As XmlNode = DotNetNuke.Common.Globals.GetContent(Content, "Sliders")

            For Each xmlSlider As XmlNode In injContents.SelectNodes("Slider")
                Dim objSlider As New SliderInfo
                objSlider.ModuleId = ModuleID
                objSlider.SliderName = xmlSlider.SelectSingleNode("SliderName").InnerText
                objSlider.SliderContent = xmlSlider.SelectSingleNode("SliderContent").InnerText
                objSlider.AlternateText = xmlSlider.SelectSingleNode("AlternateText").InnerText
                objSlider.DisplayOrder = Integer.Parse(xmlSlider.SelectSingleNode("DisplayOrder").InnerText, Globalization.NumberStyles.Integer)
                objSlider.LastUpdatedBy = UserID
                objSlider.SliderId = AddSlider(objSlider)
            Next

            ' will need to add settings here

        End Sub

#End Region

    End Class

End Namespace