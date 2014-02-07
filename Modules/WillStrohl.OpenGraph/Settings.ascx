<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="Settings.ascx.vb" Inherits="WillStrohl.Modules.OpenGraph.Settings" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register TagPrefix="dnn" TagName="Url" Src="~/controls/UrlControl.ascx" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.UI.WebControls" Assembly="DotNetNuke.Web" %>
<%@ Import Namespace="DotNetNuke.Services.Localization" %>
<div id="dnnOpenGraphSettings" class="dnnForm dnnEditEntry dnnClear">
    <h2 id="dnnPanel-h2OpenGraph" class="dnnFormSectionHead"><%= GetLocalizedString("OpenGraphSettings.Text")%></h2>
    <fieldset id="fsOpenGraphSettings">
        <div class="dnnFormItem">
            <div class="dnnFormMessage dnnFormInfo"><%= GetLocalizedString("SettingsOverview.Text")%></div>
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="lblTitle" runat="server" ResourceKey="lblTitle" ControlName="txtTitle" Suffix=":" />
            <asp:TextBox ID="txtTitle" runat="server" MaxLength="150" CssClass="NormalTextBox dnnFormRequired" ValidationGroup="OpenGraph" /> 
            <asp:RequiredFieldValidator ID="rfvTitle" runat="server" ControlToValidate="txtTitle" Display="Dynamic" CssClass="dnnFormMessage dnnFormError" ValidationGroup="OpenGraph" />
            <asp:CheckBox runat="server" ID="chkTitle" CssClass="wnsFormCheckbox" AutoPostBack="True" CausesValidation="false" /> 
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="lblDescription" runat="server" ResourceKey="lblDescription" ControlName="txtDescription" Suffix=":" />
            <asp:TextBox ID="txtDescription" runat="server" MaxLength="500" CssClass="NormalTextBox dnnFormRequired wnsTallTextBox" TextMode="MultiLine" ValidationGroup="OpenGraph" /> 
            <asp:RequiredFieldValidator ID="rfvDescription" runat="server" ControlToValidate="txtDescription" Display="Dynamic" CssClass="dnnFormMessage dnnFormError" ValidationGroup="OpenGraph" />
            <asp:CheckBox runat="server" ID="chkDescription" CssClass="wnsFormCheckbox" AutoPostBack="True" CausesValidation="false" /> 
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="lblUrl" runat="server" ResourceKey="lblUrl" ControlName="txtUrl" Suffix=":" />
            <asp:TextBox ID="txtUrl" runat="server" MaxLength="255" CssClass="NormalTextBox dnnFormRequired" ValidationGroup="OpenGraph" /> 
            <asp:RequiredFieldValidator ID="rfvUrl" runat="server" ControlToValidate="txtUrl" Display="Dynamic" CssClass="dnnFormMessage dnnFormError" ValidationGroup="OpenGraph" /> 
            <asp:RegularExpressionValidator ID="revUrl" runat="server" ControlToValidate="txtUrl" Display="Dynamic" CssClass="dnnFormMessage dnnFormError" ValidationGroup="OpenGraph" ValidationExpression="http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&amp;=]*)?" /> 
            <asp:CheckBox runat="server" ID="chkUrl" CssClass="wnsFormCheckbox" AutoPostBack="True" CausesValidation="false" /> 
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="lblLocale" runat="server" ResourceKey="lblLocale" ControlName="cboLocale" Suffix=":" />
            <asp:DropDownList ID="cboLocale" runat="server" CssClass="NormalTextBox dnnFormRequired" ValidationGroup="OpenGraph" /> 
            <asp:RequiredFieldValidator ID="rfvLocale" runat="server" ControlToValidate="cboLocale" Display="Dynamic" CssClass="dnnFormMessage dnnFormError" InitialValue="---" ValidationGroup="OpenGraph" />
        </div>
        <div class="dnnFormItem">
            <dnn:label id="lblImage" runat="server" ResourceKey="lblImage" ControlName="ctlImage" Suffix=":" />
            <div class="dnnLeft">
                <dnn:Url ID="ctlImage" runat="server" FileFilter="png,jpg,jpeg,gif" Required="false" ShowDatabase="false" ShowFiles="true" 
                    ShowImages="false" ShowLog="false" ShowNewWindow="false" ShowNone="false" ShowSecure="false" ShowTabs="false" ShowTrack="false" 
                    ShowUpLoad="true" ShowUrls="true" ShowUsers="false" />
            </div>
        </div>
    </fieldset>
    <h2 id="dnnPanel-h2GlobalSettings" class="dnnFormSectionHead"><%= Localization.GetString("GlobalSettings.Text", Me.LocalResourceFile)%></h2>
    <fieldset id="fsGlobalSettings">
        <div class="dnnFormItem">
            <div class="dnnFormMessage dnnFormInfo"><%= Me.GetLocalizedString("GlobalSettingsOverview.Text")%></div>
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="lblSiteName" runat="server" ResourceKey="lblSiteName" ControlName="txtSiteName" Suffix=":" />
            <asp:TextBox ID="txtSiteName" runat="server" MaxLength="255" CssClass="NormalTextBox dnnFormRequired" ValidationGroup="OpenGraph" /> 
            <asp:RequiredFieldValidator ID="rfvSiteName" runat="server" ControlToValidate="txtSiteName" Display="Dynamic" CssClass="dnnFormMessage dnnFormError" ValidationGroup="OpenGraph" /> 
            <asp:CheckBox runat="server" ID="chkSiteName" CssClass="wnsFormCheckbox" AutoPostBack="True" CausesValidation="false" /> 
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="lblType" runat="server" ResourceKey="lblType" ControlName="cboType" Suffix=":" />
            <asp:DropDownList ID="cboType" runat="server" CssClass="NormalTextBox dnnFormRequired" ValidationGroup="OpenGraph" /> 
            <asp:RequiredFieldValidator ID="rfvType" runat="server" ControlToValidate="cboType" Display="Dynamic" CssClass="dnnFormMessage dnnFormError" InitialValue="---" ValidationGroup="OpenGraph" />
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="lblFacebookAdmins" runat="server" ResourceKey="lblFacebookAdmins" ControlName="txtFacebookAdmins" Suffix=":" />
            <asp:TextBox ID="txtFacebookAdmins" runat="server" MaxLength="255" CssClass="NormalTextBox" ValidationGroup="OpenGraph" /> 
            <asp:RegularExpressionValidator ID="revFacebookAdmins" runat="server" ControlToValidate="txtFacebookAdmins" Display="Dynamic" CssClass="dnnFormMessage dnnFormError" ValidationExpression="^(\d+,?)*$" ValidationGroup="OpenGraph" />
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="lblFacebookAppId" runat="server" ResourceKey="lblFacebookAppId" ControlName="txtFacebookAppId" Suffix=":" />
            <asp:TextBox ID="txtFacebookAppId" runat="server" MaxLength="150" CssClass="NormalTextBox" ValidationGroup="OpenGraph" /> 
            <asp:RegularExpressionValidator ID="revFacebookAppId" runat="server" ControlToValidate="txtFacebookAppId" Display="Dynamic" CssClass="dnnFormMessage dnnFormError" ValidationExpression="\d+" ValidationGroup="OpenGraph" />
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="lblDebug" runat="server" ResourceKey="lblDebug" ControlName="chkDebug" Suffix=":" />
            <asp:CheckBox runat="server" ID="chkDebug" AutoPostBack="False" ValidationGroup="OpenGraph" /> 
        </div>
    </fieldset>
    <div class="dnnClear">
        <asp:LinkButton ID="lnkSubmit" runat="server" CssClass="dnnPrimaryAction" CausesValidation="true" ValidationGroup="OpenGraph" />&nbsp; 
        <asp:LinkButton ID="lnkCancel" runat="server" CssClass="dnnSecondaryAction" CausesValidation="false" />&nbsp; 
        <asp:LinkButton ID="lnkDisable" runat="server" CssClass="dnnSecondaryAction" CausesValidation="false" />&nbsp; 
        <asp:LinkButton ID="lnkClearSettings" runat="server" CssClass="dnnSecondaryAction" CausesValidation="false" />
    </div>
</div>
<script language="javascript" type="text/javascript">/*<![CDATA[*/
    (function ($, Sys) {
        function setupDnnSiteSettings() {
            $('#dnnOpenGraphSettings').dnnPanels();
            
            $('<%=lnkDisable.ClientID %>').dnnConfirm({
                text: '<%= GetLocalizedString("lblDisableWarning.Text") %>',
                yesText: '<%= Localization.GetString("Yes.Text", Localization.SharedResourceFile) %>',
                noText: '<%= Localization.GetString("No.Text", Localization.SharedResourceFile) %>',
                title: '<%= Localization.GetString("Confirm.Text", Localization.SharedResourceFile) %>'
            });

            $('<%=lnkClearSettings.ClientID %>').dnnConfirm({
                text: '<%= GetLocalizedString("lblClearSettingsWarning.Text") %>',
                yesText: '<%= Localization.GetString("Yes.Text", Localization.SharedResourceFile) %>',
                noText: '<%= Localization.GetString("No.Text", Localization.SharedResourceFile) %>',
                title: '<%= Localization.GetString("Confirm.Text", Localization.SharedResourceFile) %>'
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