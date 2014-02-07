<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Edit.ascx.cs" Inherits="DNNCommunity.Modules.UGLabsMetaData.Edit" %>
<%@ Import Namespace="DotNetNuke.Services.Localization" %>
<%@ Register TagName="label" TagPrefix="dnn" Src="~/controls/labelcontrol.ascx" %>
<div class="dnnForm dnnMetadataForm">
    <h2 id="dnnSitePanel-BasicSettings" class="dnnFormSectionHead"><a href="" class="dnnSectionExpanded"><%=LocalizeString("BasicSettings.Text")%></a></h2>
    <fieldset id="fsSetting">
        <div class="dnnFormItem">
            <dnn:label id="lblSettingKey" resourcekey="lblSettingKey" ControlName="txtSettingKey" runat="server" />
            <asp:TextBox ID="txtSettingKey" runat="server" CssClass="NormalTextbox dnnFormRequired"/>
            <asp:RequiredFieldValidator ID="rfvSettingKey" runat="server" ControlToValidate="txtSettingKey" CssClass="dnnFormMessage dnnFormError" Display="Dynamic" ValidationGroup="GroupSettings" />
        </div>
        <div class="dnnFormItem">
            <dnn:label id="lblSettingValue" resourcekey="lblSettingValue" ControlName="txtSettingValue" runat="server" />
            <asp:TextBox ID="txtSettingValue" runat="server" CssClass="NormalTextbox dnnFormRequired"/>
            <asp:RequiredFieldValidator ID="rfvSettingValue" runat="server" ControlToValidate="txtSettingValue" CssClass="dnnFormMessage dnnFormError" Display="Dynamic" ValidationGroup="GroupSettings" />
        </div>
        <div class="dnnFormItem">
            <asp:LinkButton ID="lnkUpdate" runat="server" CssClass="dnnPrimaryAction" ValidationGroup="GroupSettings" OnClick="lnkUpdate_Click" />
            <asp:LinkButton ID="lnkDelete" runat="server" CssClass="dnnSecondaryAction" CausesValidation="False" OnClick="lnkDelete_Click" />
            <asp:LinkButton ID="lnkReturn" runat="server" CssClass="dnnSecondaryAction" CausesValidation="False" OnClick="lnkReturn_Click" />
        </div>
    </fieldset>
</div>
<script language="javascript" type="text/javascript">/*<![CDATA[*/
    (function ($, Sys) {
        function setupDnnSiteSettings() {
            $('#<%=lnkDelete.ClientID%>').dnnConfirm({
                text: '<%= GetLocalizedString("Confirmation.Text") %>',
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

    }(jQuery, window.Sys));
    /*]]>*/</script>