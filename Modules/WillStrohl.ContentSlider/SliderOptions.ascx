<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="SliderOptions.ascx.vb" Inherits="WillStrohl.Modules.ContentSlider.SliderOptions" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/labelcontrol.ascx" %>
<%@ Register TagPrefix="dnn" TagName="TextBox" Src="~/controls/texteditor.ascx" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.UI.WebControls" Assembly="DotNetNuke.Web" %>
<%@ Import Namespace="DotNetNuke.Services.Localization" %>
<div id="dnnEditEntry" class="dnnForm dnnEditEntry dnnClear">
    <div class="dnnFormExpandContent dnnClear"><a href=""><%= Localization.GetString("ExpandAll", Localization.SharedResourceFile)%></a></div>
    <div class="dnnFormItem dnnFormHelp dnnClear">
        <p class="dnnFormRequired"><span><%= Me.GetLocalizedString("lblRequiredFields.Text") %></span></p>
    </div>
    <h2 id="dnnPanel-h2CommonSettings" class="dnnFormSectionHead"><a href="" class="dnnLabelExpanded"><%= Me.GetLocalizedString("CommonSettings.Text")%></a></h2>
    <fieldset id="fsCommonSettings">
        <div class="dnnFormItem">
            <dnn:Label ID="lblWidth" runat="server" ResourceKey="lblWidth" ControlName="txtWidth" Suffix=":" />
            <asp:TextBox ID="txtWidth" runat="server" ValidationGroup="slider" />
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="lblHeight" runat="server" ResourceKey="lblHeight" ControlName="txtHeight" Suffix=":" />
            <asp:TextBox ID="txtHeight" runat="server" Text="auto" ValidationGroup="slider" />
            <asp:RequiredFieldValidator ID="rfvHeight" runat="server" Display="Dynamic" ControlToValidate="txtHeight" CssClass="dnnFormMessage dnnFormError" ValidationGroup="slider" />
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="lblSpeed" runat="server" ResourceKey="lblSpeed" ControlName="txtSpeed" Suffix=":" />
            <asp:TextBox ID="txtSpeed" runat="server" Text="1000" ValidationGroup="slider" />
            <asp:RequiredFieldValidator ID="rfvSpeed" runat="server" Display="Dynamic" ControlToValidate="txtSpeed" CssClass="dnnFormMessage dnnFormError" ValidationGroup="slider" />
            <asp:RegularExpressionValidator ID="revSpeed" runat="server" Display="Dynamic" ControlToValidate="txtSpeed" CssClass="dnnFormMessage dnnFormError" ValidationGroup="slider" ValidationExpression="\d+" />
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="lblFit" runat="server" ResourceKey="lblFit" ControlName="chkFit" Suffix=":" />
            <asp:CheckBox ID="chkFit" runat="server" ValidationGroup="slider" />
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="lblContainerResize" runat="server" ResourceKey="lblContainerResize" ControlName="chkContainerResize" Suffix=":" />
            <asp:CheckBox ID="chkContainerResize" runat="server" ValidationGroup="slider" />
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="lblPause" runat="server" ResourceKey="lblPause" ControlName="chkPause" Suffix=":" />
            <asp:CheckBox ID="chkPause" runat="server" ValidationGroup="slider" />
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="lblExcludeCycle" runat="server" ResourceKey="lblExcludeCycle" ControlName="chkExcludeCycle" Suffix=":" />
            <asp:CheckBox ID="chkExcludeCycle" runat="server" ValidationGroup="slider" />
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="lblExcludeEasing" runat="server" ResourceKey="lblExcludeEasing" ControlName="chkExcludeEasing" Suffix=":" />
            <asp:CheckBox ID="chkExcludeEasing" runat="server" ValidationGroup="slider" />
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="lblCacheDuration" runat="server" ResourceKey="lblCacheDuration" ControlName="txtCacheDuration" Suffix=":" />
            <asp:TextBox ID="txtCacheDuration" runat="server" Text="5" ValidationGroup="slider" />
            <asp:RequiredFieldValidator ID="rfvCacheDuration" runat="server" Display="Dynamic" ControlToValidate="txtCacheDuration" CssClass="dnnFormMessage dnnFormError" ValidationGroup="slider" />
            <asp:RegularExpressionValidator ID="revCacheDuration" runat="server" Display="Dynamic" ControlToValidate="txtCacheDuration" CssClass="dnnFormMessage dnnFormError" ValidationGroup="slider" ValidationExpression="\d+" />
        </div>
    </fieldset>
    <h2 id="dnnPanel-h2Appearance" class="dnnFormSectionHead"><a href="" class=""><%= Me.GetLocalizedString("Appearance.Text")%></a></h2>
    <fieldset id="fsAppearance">
        <div class="dnnFormItem">
            <dnn:Label ID="lblCssAfter" runat="server" ResourceKey="lblCssAfter" ControlName="txtCssAfter" Suffix=":" />
            <asp:TextBox ID="txtCssAfter" runat="server" ValidationGroup="slider" />
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="lblCssBefore" runat="server" ResourceKey="lblCssBefore" ControlName="txtCssBefore" Suffix=":" />
            <asp:TextBox ID="txtCssBefore" runat="server" ValidationGroup="slider" />
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="lblEasing" runat="server" ResourceKey="lblEasing" ControlName="txtEasing" Suffix=":" />
            <asp:TextBox ID="txtEasing" runat="server" ValidationGroup="slider" />
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="lblEaseIn" runat="server" ResourceKey="lblEaseIn" ControlName="txtEaseIn" Suffix=":" />
            <asp:TextBox ID="txtEaseIn" runat="server" ValidationGroup="slider" />
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="lblEaseOut" runat="server" ResourceKey="lblEaseOut" ControlName="txtEaseOut" Suffix=":" />
            <asp:TextBox ID="txtEaseOut" runat="server" ValidationGroup="slider" />
        </div>
    </fieldset>
    <h2 id="dnnPanel-h2Pager" class="dnnFormSectionHead"><a href="" class=""><%= Me.GetLocalizedString("Pager.Text")%></a></h2>
    <fieldset id="fsPager">
        <div class="dnnFormItem">
            <dnn:Label ID="lblPager" runat="server" ResourceKey="lblPager" ControlName="chkPager" Suffix=":" />
            <asp:CheckBox ID="chkPager" runat="server" AutoPostBack="True" CausesValidation="False" ValidationGroup="slider" />
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="lblActivePagerClass" runat="server" ResourceKey="lblActivePagerClass" ControlName="txtActivePagerClass" Suffix=":" />
            <asp:TextBox ID="txtActivePagerClass" runat="server" Text="activeSlide" ValidationGroup="slider" />
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="lblPagerAnchorBuilder" runat="server" ResourceKey="lblPagerAnchorBuilder" ControlName="txtPagerAnchorBuilder" Suffix=":" />
            <asp:TextBox ID="txtPagerAnchorBuilder" runat="server" ValidationGroup="slider" />&nbsp; <asp:LinkButton ID="lnkDefaultPagerAnchorBuilder" runat="server" />
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="lblNext" runat="server" ResourceKey="lblNext" ControlName="txtNext" Suffix=":" />
            <asp:TextBox ID="txtNext" runat="server" ValidationGroup="slider" />
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="lblPrev" runat="server" ResourceKey="lblPrev" ControlName="txtPrev" Suffix=":" />
            <asp:TextBox ID="txtPrev" runat="server" ValidationGroup="slider" />
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="lblPauseOnPagerHover" runat="server" ResourceKey="lblPauseOnPagerHover" ControlName="chkPauseOnPagerHover" Suffix=":" />
            <asp:CheckBox ID="chkPauseOnPagerHover" runat="server" ValidationGroup="slider" />
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="lblAllowPagerClickBubble" runat="server" ResourceKey="lblAllowPagerClickBubble" ControlName="chkAllowPagerClickBubble" Suffix=":" />
            <asp:CheckBox ID="chkAllowPagerClickBubble" runat="server" ValidationGroup="slider" />
        </div>
    </fieldset>
    <h2 id="dnnPanel-h2Behavior" class="dnnFormSectionHead"><a href="" class=""><%= Me.GetLocalizedString("Behavior.Text")%></a></h2>
    <fieldset id="fsBehavior">
        <div class="dnnFormItem">
            <dnn:Label ID="lblAutoStop" runat="server" ResourceKey="lblAutoStop" ControlName="chkAutoStop" Suffix=":" />
            <asp:CheckBox ID="chkAutoStop" runat="server" ValidationGroup="slider" />
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="lblAutoStopCount" runat="server" ResourceKey="lblAutoStopCount" ControlName="txtAutoStopCount" Suffix=":" />
            <asp:TextBox ID="txtAutoStopCount" runat="server" ValidationGroup="slider" />
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="lblBackwards" runat="server" ResourceKey="lblBackwards" ControlName="chkBackwards" Suffix=":" />
            <asp:CheckBox ID="chkBackwards" runat="server" ValidationGroup="slider" />
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="lblMetaAttr" runat="server" ResourceKey="lblMetaAttr" ControlName="txtMetaAttr" Suffix=":" />
            <asp:TextBox ID="txtMetaAttr" runat="server" Text="cycle" ValidationGroup="slider" /> 
            <asp:RequiredFieldValidator ID="rfvMetaAttr" runat="server" Display="Dynamic" ControlToValidate="txtMetaAttr" CssClass="dnnFormMessage dnnFormError" ValidationGroup="slider" />
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="lblNoWrap" runat="server" ResourceKey="lblNoWrap" ControlName="chkNoWrap" Suffix=":" />
            <asp:CheckBox ID="chkNoWrap" runat="server" ValidationGroup="slider" />
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="lblRandom" runat="server" ResourceKey="lblRandom" ControlName="chkRandom" Suffix=":" />
            <asp:CheckBox ID="chkRandom" runat="server" ValidationGroup="slider" />
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="lblRequeueOnImageNotLoaded" runat="server" ResourceKey="lblRequeueOnImageNotLoaded" ControlName="chkRequeueOnImageNotLoaded" Suffix=":" />
            <asp:CheckBox ID="chkRequeueOnImageNotLoaded" runat="server" ValidationGroup="slider" />
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="lblRequeueTimeout" runat="server" ResourceKey="lblRequeueTimeout" ControlName="txtRequeueTimeout" Suffix=":" />
            <asp:TextBox ID="txtRequeueTimeout" runat="server" Text="250" ValidationGroup="slider" /> 
            <asp:RequiredFieldValidator ID="rfvRequeueTimeout" runat="server" Display="Dynamic" ControlToValidate="txtRequeueTimeout" CssClass="dnnFormMessage dnnFormError" ValidationGroup="slider" />
            <asp:RegularExpressionValidator ID="revRequeueTimeout" runat="server" Display="Dynamic" ControlToValidate="txtRequeueTimeout" CssClass="dnnFormMessage dnnFormError" ValidationGroup="slider" ValidationExpression="\d+" />
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="lblShuffle" runat="server" ResourceKey="lblShuffle" ControlName="txtShuffle" Suffix=":" />
            <asp:TextBox ID="txtShuffle" runat="server" ValidationGroup="slider" />
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="lblSlideResize" runat="server" ResourceKey="lblSlideResize" ControlName="chkSlideResize" Suffix=":" />
            <asp:CheckBox ID="chkSlideResize" runat="server" ValidationGroup="slider" />
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="lblStartingSlide" runat="server" ResourceKey="lblStartingSlide" ControlName="txtStartingSlide" Suffix=":" />
            <asp:TextBox ID="txtStartingSlide" runat="server" ValidationGroup="slider" />
        </div>
    </fieldset>
    <h2 id="dnnPanel-h2Transition" class="dnnFormSectionHead"><a href="" class=""><%= Me.GetLocalizedString("Transition.Text")%></a></h2>
    <fieldset id="fsTransition">
        <div class="dnnFormItem">
            <dnn:Label ID="lblAnimIn" runat="server" ResourceKey="lblAnimIn" ControlName="txtAnimIn" Suffix=":" />
            <asp:TextBox ID="txtAnimIn" runat="server" ValidationGroup="slider" />
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="lblAnimOut" runat="server" ResourceKey="lblAnimOut" ControlName="txtAnimOut" Suffix=":" />
            <asp:TextBox ID="txtAnimOut" runat="server" ValidationGroup="slider" />
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="lblContinuous" runat="server" ResourceKey="lblContinuous" ControlName="chkContinuous" Suffix=":" />
            <asp:CheckBox ID="chkContinuous" runat="server" ValidationGroup="slider" />
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="lblDelay" runat="server" ResourceKey="lblDelay" ControlName="txtDelay" Suffix=":" />
            <asp:TextBox ID="txtDelay" runat="server" ValidationGroup="slider" />
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="lblFastOnEvent" runat="server" ResourceKey="lblFastOnEvent" ControlName="txtFastOnEvent" Suffix=":" />
            <asp:TextBox ID="txtFastOnEvent" runat="server" ValidationGroup="slider" />
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="lblFx" runat="server" ResourceKey="lblFx" ControlName="cboFx" Suffix=":" />
            <asp:DropDownList ID="cboFx" runat="server" ValidationGroup="slider" />
            <asp:RequiredFieldValidator ID="rfvFx" runat="server" ControlToValidate="cboFx" CssClass="dnnFormMessage dnnFormError" Display="Dynamic" InitialValue="---" ValidationGroup="slider" />
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="lblManualTrump" runat="server" ResourceKey="lblManualTrump" ControlName="chkManualTrump" Suffix=":" />
            <asp:CheckBox ID="chkManualTrump" runat="server" ValidationGroup="slider" />
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="lblRandomizeEffects" runat="server" ResourceKey="lblRandomizeEffects" ControlName="chkRandomizeEffects" Suffix=":" />
            <asp:CheckBox ID="chkRandomizeEffects" runat="server" ValidationGroup="slider" />
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="lblRev" runat="server" ResourceKey="lblRev" ControlName="chkRev" Suffix=":" />
            <asp:CheckBox ID="chkRev" runat="server" ValidationGroup="slider" />
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="lblSpeedIn" runat="server" ResourceKey="lblSpeedIn" ControlName="txtSpeedIn" Suffix=":" />
            <asp:TextBox ID="txtSpeedIn" runat="server" ValidationGroup="slider" />
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="lblSpeedOut" runat="server" ResourceKey="lblSpeedOut" ControlName="txtSpeedOut" Suffix=":" />
            <asp:TextBox ID="txtSpeedOut" runat="server" ValidationGroup="slider" />
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="lblTimeout" runat="server" ResourceKey="lblTimeout" ControlName="txtTimeout" Suffix=":" />
            <asp:TextBox ID="txtTimeout" runat="server" ValidationGroup="slider" />
            <asp:RequiredFieldValidator ID="rfvTimeout" runat="server" ControlToValidate="txtTimeout" CssClass="dnnFormMessage dnnFormError" Display="Dynamic" ValidationGroup="slider" />
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="lblSync" runat="server" ResourceKey="lblSync" ControlName="chkSync" Suffix=":" />
            <asp:CheckBox ID="chkSync" runat="server" ValidationGroup="slider" />
        </div>
    </fieldset>
    <h2 id="dnnPanel-h2JavaScriptEvents" class="dnnFormSectionHead"><a href="" class=""><%= Me.GetLocalizedString("JavaScriptEvents.Text")%></a></h2>
    <fieldset id="fsJavaScriptEvents">
        <div class="dnnFormItem">
            <dnn:Label ID="lblAfter" runat="server" ResourceKey="lblAfter" ControlName="txtAfter" Suffix=":" />
            <asp:TextBox ID="txtAfter" runat="server" ValidationGroup="slider" />
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="lblBefore" runat="server" ResourceKey="lblBefore" ControlName="txtBefore" Suffix=":" />
            <asp:TextBox ID="txtBefore" runat="server" ValidationGroup="slider" />
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="lblEnd" runat="server" ResourceKey="lblEnd" ControlName="txtEnd" Suffix=":" />
            <asp:TextBox ID="txtEnd" runat="server" ValidationGroup="slider" />
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="lblFxFn" runat="server" ResourceKey="lblFxFn" ControlName="txtFxFn" Suffix=":" />
            <asp:TextBox ID="txtFxFn" runat="server" ValidationGroup="slider" />
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="lblOnPagerEvent" runat="server" ResourceKey="lblOnPagerEvent" ControlName="txtOnPagerEvent" Suffix=":" />
            <asp:TextBox ID="txtOnPagerEvent" runat="server" ValidationGroup="slider" />
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="lblOnPrevNextEvent" runat="server" ResourceKey="lblOnPrevNextEvent" ControlName="txtOnPrevNextEvent" Suffix=":" />
            <asp:TextBox ID="txtOnPrevNextEvent" runat="server" ValidationGroup="slider" />
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="lblPagerEvent" runat="server" ResourceKey="lblPagerEvent" ControlName="txtPagerEvent" Suffix=":" />
            <asp:TextBox ID="txtPagerEvent" runat="server" Text="click.cycle" ValidationGroup="slider" />
            <asp:RequiredFieldValidator ID="rfvPagerEvent" runat="server" CssClass="dnnFormMessage dnnFormError" Display="Dynamic" ControlToValidate="txtPagerEvent" ValidationGroup="slider" />
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="lblPrevNextEvent" runat="server" ResourceKey="lblPrevNextEvent" ControlName="txtPrevNextEvent" Suffix=":" />
            <asp:TextBox ID="txtPrevNextEvent" runat="server" Text="click.cycle" ValidationGroup="slider" />
            <asp:RequiredFieldValidator ID="rfvPrevNextEvent" runat="server" CssClass="dnnFormMessage dnnFormError" Display="Dynamic" ControlToValidate="txtPrevNextEvent" ValidationGroup="slider" />
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="lblSlideExpr" runat="server" ResourceKey="lblSlideExpr" ControlName="txtSlideExpr" Suffix=":" />
            <asp:TextBox ID="txtSlideExpr" runat="server" ValidationGroup="slider" />
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="lblTimeoutFn" runat="server" ResourceKey="lblTimeoutFn" ControlName="txtTimeoutFn" Suffix=":" />
            <asp:TextBox ID="txtTimeoutFn" runat="server" ValidationGroup="slider" />
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="lblUpdateActivePagerLink" runat="server" ResourceKey="lblUpdateActivePagerLink" ControlName="txtUpdateActivePagerLink" Suffix=":" />
            <asp:TextBox ID="txtUpdateActivePagerLink" runat="server" ValidationGroup="slider" />
        </div>
    </fieldset>
</div>
<div class="dnnClear">
    <asp:LinkButton ID="lnkSave" runat="server" CssClass="dnnPrimaryAction" CausesValidation="true" ValidationGroup="slider" />&nbsp; 
    <asp:LinkButton ID="lnkCancel" runat="server" CssClass="dnnSecondaryAction" CausesValidation="false" />&nbsp; 
    <asp:LinkButton ID="lnkReset" runat="server" CssClass="dnnSecondaryAction" CausesValidation="false" />
</div>
<script language="javascript" type="text/javascript">/*<![CDATA[*/
    (function ($, Sys) {
        function setupDnnSiteSettings() {
            $('#dnnEditEntry').dnnPanels();
            $('#dnnEditEntry .dnnFormExpandContent a').dnnExpandAll({
                expandText: '<%=Localization.GetString("ExpandAll", Localization.SharedResourceFile)%>',
                collapseText: '<%=Localization.GetString("CollapseAll", Localization.SharedResourceFile)%>',
                targetArea: '#dnnEditEntry'
            });
        }

        $(document).ready(function () {
            setupDnnSiteSettings();
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
                setupDnnSiteSettings();
            });
        });

    } (jQuery, window.Sys));
/*]]>*/</script>