<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="EditImage.ascx.vb" Inherits="WillStrohl.Modules.Lightbox.EditImage" %>
<%@ Import Namespace="DotNetNuke.Services.Localization" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register TagPrefix="dnnweb" Assembly="DotNetNuke.Web" Namespace="DotNetNuke.Web.UI.WebControls" %>
<div id="divImagePreview" class="dnnClear">
    <p class="wns_lightbox_wrapper">
        <asp:Literal ID="litImage" runat="server" />
    </p>
    <p class="wns_lightbox_wrapper">
        <asp:Label ID="lblFileName" runat="server" CssClass="SubHead" />
    </p>
</div>
<div class="dnnClear">
    <asp:ValidationSummary ID="vsError" runat="server" CssClass="NormalRed" DisplayMode="List" ValidationGroup="lightbox" />
</div>
<div id="dnnEditEntry" class="dnnForm dnnEditEntry dnnClear">
    <h2 id="dnnPanel-h2Image" class="dnnFormSectionHead"><a href="" class="dnnLabelExpanded"><%= Me.GetLocalizedString("EditImage.Text")%></a></h2>
    <fieldset id="fsImage" class="wns-fieldset">
        <ol id="olImage" class="wns-list">
            <li class="wns-listitem">
                <div class="dnnFormItem">
                    <dnn:label id="lblImageTitle" runat="server" ResourceKey="lblImageTitle" ControlName="txtImageTitle" Suffix=":" />
                    <asp:TextBox ID="txtImageTitle" runat="server" MaxLength="50" CssClass="NormalTextBox dnnFormRequired" ValidationGroup="lightbox" /> 
                    <asp:RequiredFieldValidator ID="rfvImageTitle" runat="server" ControlToValidate="txtImageTitle" Display="Dynamic" CssClass="dnnFormMessage dnnFormError" ValidationGroup="lightbox" />
                </div>
            </li>
            <li class="wns-listitem">
                <div class="dnnFormItem">
                    <dnn:label id="lblImageDescription" runat="server" ResourceKey="lblImageDescription" ControlName="txtImageDescription" Suffix=":" />
                    <asp:TextBox ID="txtImageDescription" runat="server" MaxLength="500" Rows="5" TextMode="MultiLine" CssClass="NormalTextBox" ValidationGroup="lightbox" /> 
                    <asp:RequiredFieldValidator ID="rfvImageDescription" runat="server" ControlToValidate="txtImageDescription" Display="Dynamic" CssClass="dnnFormMessage dnnFormError" ValidationGroup="lightbox" Enabled="false" />
                </div>
            </li>
        </ol>
    </fieldset>
    <h2 id="dnnPanel-h2ImageMetaData" class="dnnFormSectionHead"><a href="" class=""><%= Me.GetLocalizedString("ImageMetaData.Text")%></a></h2>
    <fieldset id="fsImageMetaData" class="wns-fieldset">
        <ol id="olImageMetaData" class="wns-list">
            <li class="wns-listitem">
                <div id="divImageMetaData" class="dnnFormItem" runat="server">
                </div>
            </li>
        </ol>
    </fieldset>
    <p id="pLightboxCommands">
        <asp:LinkButton ID="cmdUpdate" runat="server" CssClass="dnnPrimaryAction" CausesValidation="true" ValidationGroup="lightbox" />&nbsp; 
        <asp:LinkButton ID="cmdDelete" runat="server" CssClass="dnnSecondaryAction" CausesValidation="false" />&nbsp; 
        <asp:LinkButton ID="cmdCancel" runat="server" CssClass="dnnSecondaryAction" CausesValidation="false" />
    </p>
</div>
<script language="javascript" type="text/javascript">/*<![CDATA[*/
    (function ($, Sys) {
        function setupDnnSiteSettings() {
            $('#dnnEditEntry').dnnPanels();

            $('#<%=Me.txtImageTitle.ClientId%>').focus();
            
            $('#<%= Me.cmdDelete.ClientID %>').dnnConfirm({
                text: '<%= Me.GetLocalizedString("ConfirmDelete.Text") %>',
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