<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Edit.ascx.cs" Inherits="WillStrohl.Modules.DNNHangout.Edit" %>
<%@ Register TagName="label" TagPrefix="dnn" Src="~/controls/labelcontrol.ascx" %>
<%@ Register TagName="Editor" TagPrefix="dnn" Src="~/controls/texteditor.ascx" %>
<%@ Register TagPrefix="dnn" Assembly="DotNetNuke.Web" Namespace="DotNetNuke.Web.UI.WebControls" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.UI.WebControls" Assembly="DotNetNuke.Web" %>
<%@ Import namespace="DotNetNuke.Services.Localization" %>
<div id="dnnHangoutForm" class="dnnForm dnnHangoutForm">
    <h2 id="dnnSitePanel-BasicSettings" class="dnnFormSectionHead"><a href="" class="dnnSectionExpanded"><%=LocalizeString("BasicSettings.Text")%></a></h2>
    <fieldset id="fsSetting">
        <div class="dnnFormItem">
            <dnn:label id="lblTitle" resourcekey="lblTitle" ControlName="txtTitle" runat="server" />
            <asp:TextBox ID="txtTitle" runat="server" MaxLength="100" CssClass="NormalTextbox dnnFormRequired"/>
            <asp:RequiredFieldValidator ID="rfvTitle" runat="server" ControlToValidate="txtTitle" CssClass="dnnFormMessage dnnFormError" Display="Dynamic" ValidationGroup="GoogleHangout" />
        </div>
        <div class="dnnFormItem">
            <dnn:label id="lblDescription" resourcekey="lblDescription" ControlName="txtDescription" runat="server" />
            <div id="divDesccriptionWrapper" class="dnnRight">
                <dnn:Editor ID="txtDescription" runat="server" ChooseMode="true" ChooseRender="true" Height="300px" HtmlEncode="true" Width="570px" />
            </div>
        </div>
        <div class="dnnFormItem">
            <dnn:label id="lblHangoutAddress" resourcekey="lblHangoutAddress" ControlName="txtHangoutAddress" runat="server" />
            <asp:TextBox ID="txtHangoutAddress" runat="server" TextMode="MultiLine" Rows="4" MaxLength="250" CssClass="NormalTextbox dnnFormRequired"/>
            <asp:RequiredFieldValidator ID="rfvHangoutAddress" runat="server" ControlToValidate="txtHangoutAddress" CssClass="dnnFormMessage dnnFormError" Display="Dynamic" ValidationGroup="GoogleHangout" />
            <asp:CustomValidator ID="cvHangoutAddress" runat="server" ControlToValidate="txtHangoutAddress" CssClass="dnnFormMessage dnnFormError" Display="Dynamic" ValidationGroup="GoogleHangout" OnServerValidate="cvHangoutAddressValidate" />
        </div>
        <div class="dnnFormItem gh-hidden">
            <div class="dnnRight">
                <asp:LinkButton runat="server" CausesValidation="False" Text="Validate Hangout Address" OnClick="ValidateHangoutAddress" /><asp:label ID="lblValidation" runat="server" />
            </div>
        </div>
        <div class="dnnFormItem">
            <div class="dnnRight"><span class="dnnFormMessage dnnFormInfo">
                <%=GetLocalizedString("DNNHangoutHelp.Text") %>
            </span></div>
        </div>
        <div class="dnnFormItem">
            <dnn:label id="lblStartDate" resourcekey="lblStartDate" ControlName="txtStartDate" runat="server" />
            <dnn:dnndatetimepicker id="txtStartDate" runat="server" CssClass="NormalTextbox dnnFormRequired" />
            <asp:RequiredFieldValidator ID="rfvStartDate" runat="server" ControlToValidate="txtStartDate" CssClass="dnnFormMessage dnnFormError" Display="Dynamic" ValidationGroup="GoogleHangout" />
        </div>
        <div class="dnnFormItem">
            <dnn:label id="lblDuration" resourcekey="lblDuration" ControlName="txtDuration" runat="server" />
            <asp:TextBox ID="txtDuration" runat="server" MaxLength="2" CssClass="NormalTextbox dnnFormRequired"/>
            <asp:RequiredFieldValidator ID="rfvDuration" runat="server" ControlToValidate="txtDuration" CssClass="dnnFormMessage dnnFormError" Display="Dynamic" ValidationGroup="GoogleHangout" />
            <asp:RegularExpressionValidator ID="revDuration" runat="server" ControlToValidate="txtDuration" CssClass="dnnFormMessage dnnFormError" Display="Dynamic" ValidationGroup="GoogleHangout" ValidationExpression="^\d+$" />
        </div>
        <div class="dnnFormItem">
            <dnn:label id="lblDurationUnits" resourcekey="lblDurationUnits" ControlName="ddlDurationUnits" runat="server" />
            <asp:DropDownList ID="ddlDurationUnits" runat="server" CssClass="NormalTextbox dnnFormRequired"/>
            <asp:CustomValidator ID="cvDuration" runat="server" ControlToValidate="txtDuration" CssClass="dnnFormMessage dnnFormError" Display="Dynamic" ValidationGroup="GoogleHangout" OnServerValidate="cvDurationValidate" />
        </div>
    </fieldset>
</div>
<ul class="dnnActions dnnClear">
    <li><asp:LinkButton ID="lnkUpdate" runat="server" CssClass="dnnPrimaryAction" ValidationGroup="GoogleHangout" OnClick="lnkUpdate_Click" /></li>
    <li><asp:LinkButton ID="lnkReturn" runat="server" CssClass="dnnSecondaryAction" CausesValidation="False" OnClick="lnkReturn_Click" /></li>
    <li><asp:LinkButton ID="lnkDelete" runat="server" CssClass="dnnTertiaryAction" CausesValidation="False" OnClick="lnkDelete_Click" /></li>
</ul>
<script language="javascript" type="text/javascript">/*<![CDATA[*/
    (function ($, Sys) {
        function setupDnnSiteSettings() {
            $('#dnnHangoutForm').dnnPanels();

            $('#<%= lnkDelete.ClientID %>').dnnConfirm({
                text: '<%= GetLocalizedString("lnkDelete.Confirm.Text") %>',
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