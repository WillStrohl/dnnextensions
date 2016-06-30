<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="EditLightbox.ascx.vb" Inherits="WillStrohl.Modules.Lightbox.EditLightbox" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.UI.WebControls" Assembly="DotNetNuke.Web.Deprecated" %>
<%@ Import Namespace="DotNetNuke.Services.Localization" %>
<div class="dnnClear">
    <asp:ValidationSummary ID="vsError" runat="server" CssClass="dnnFormMessage dnnFormValidationSummary" DisplayMode="List" ValidationGroup="lightbox" />
</div>
<div id="dnnEditEntry" class="dnnForm dnnEditEntry dnnClear">
    <h2 id="dnnPanel-h2Album" class="dnnFormSectionHead"><a href="" class="dnnLabelExpanded"><%= Me.GetLocalizedString("AlbumSettings.Text")%></a></h2>
    <fieldset id="fsAlbum" class="wns-fieldset">
        <ol id="olAlbum" class="wns-list">
            <li class="wns-listitem">
                <div class="dnnFormItem">
                    <dnn:label id="lblGalleryName" runat="server" ResourceKey="lblGalleryName" ControlName="txtGalleryName" Suffix=":" />
                    <asp:TextBox ID="txtGalleryName" runat="server" MaxLength="50" CssClass="NormalTextBox dnnFormRequired" ValidationGroup="lightbox" /> 
                    <asp:RequiredFieldValidator ID="rfvGalleryName" runat="server" ControlToValidate="txtGalleryName" Display="Dynamic" CssClass="dnnFormMessage dnnFormError" ValidationGroup="lightbox" />
                </div>
            </li>
            <li class="wns-listitem">
                <div class="dnnFormItem">
                    <dnn:label id="lblGalleryDescription" runat="server" ResourceKey="lblGalleryDescription" ControlName="txtGalleryDescription" Suffix=":" />
                    <asp:TextBox ID="txtGalleryDescription" runat="server" MaxLength="500" Rows="5" TextMode="MultiLine" CssClass="NormalTextBox dnnFormRequired" ValidationGroup="lightbox" /> 
                    <asp:RequiredFieldValidator ID="rfvGalleryDescription" runat="server" ControlToValidate="txtGalleryDescription" Display="Dynamic" CssClass="dnnFormMessage dnnFormError" ValidationGroup="lightbox" />
                </div>
            </li>
            <li class="wns-listitem">
                <div class="dnnFormItem">
                    <dnn:label id="lblGalleryFolder" runat="server" ResourceKey="lblGalleryFolder" ControlName="cboGalleryFolder" Suffix=":" />
                    <dnn:DnnComboBox ID="cboGalleryFolder" runat="server" MaxHeight="300px" CssClass="wns_RadCombo NormalTextBox dnnFormRequired" DropDownCssClass="NormalTextBox dnnFormRequired" InputCssClass="NormalTextBox dnnFormRequired" /> 
                    <asp:RequiredFieldValidator ID="rfvGalleryFolder" runat="server" ControlToValidate="cboGalleryFolder" Display="Dynamic" CssClass="dnnFormMessage dnnFormError" ValidationGroup="lightbox" />
                </div>
            </li>
            <li class="wns-listitem">
                <div class="dnnFormItem">
                    <dnn:label id="lblHideTitleDescription" runat="server" ResourceKey="lblHideTitleDescription" ControlName="chkHideTitleDescription" Suffix=":" />
                    <asp:Checkbox ID="chkHideTitleDescription" runat="server" />
                </div>
            </li>
        </ol>
    </fieldset>
    <h2 id="dnnPanel-h2Lightbox" class="dnnFormSectionHead"><a href="" class=""><%= Me.GetLocalizedString("LightboxSettings.Text")%></a></h2>
    <fieldset id="fsLightbox" class="wns-fieldset">
        <ol id="olLightbox" class="wns-list">
            <li class="wns-listitem">
                <div class="dnnFormItem">
                    <dnn:label id="lblPadding" runat="server" ResourceKey="lblPadding" ControlName="txtPadding" Suffix=":" />
                    <asp:TextBox ID="txtPadding" runat="server" CssClass="NormalTextBox dnnFormRequired" MaxLength="3" ValidationGroup="lightbox" /> 
                    <asp:RequiredFieldValidator ID="rfvPadding" runat="server" ControlToValidate="txtPadding" Display="Dynamic" CssClass="dnnFormMessage dnnFormError" ValidationGroup="lightbox" />
                    <asp:RegularExpressionValidator ID="revPadding" runat="server" ControlToValidate="txtPadding" Display="Dynamic" ValidationExpression="^\d{1,3}$" CssClass="dnnFormMessage dnnFormError" ValidationGroup="lightbox" />
                </div>
            </li>
            <li class="wns-listitem">
                <div class="dnnFormItem">
                    <dnn:label id="lblMargin" runat="server" ResourceKey="lblMargin" ControlName="txtMargin" Suffix=":" />
                    <asp:TextBox ID="txtMargin" runat="server" CssClass="NormalTextBox dnnFormRequired" MaxLength="3" ValidationGroup="lightbox" /> 
                    <asp:RequiredFieldValidator ID="rfvMargin" runat="server" ControlToValidate="txtMargin" Display="Dynamic" CssClass="dnnFormMessage dnnFormError" ValidationGroup="lightbox" />
                    <asp:RegularExpressionValidator ID="revMargin" runat="server" ControlToValidate="txtMargin" Display="Dynamic" ValidationExpression="^\d{1,3}$" CssClass="dnnFormMessage dnnFormError" ValidationGroup="lightbox" />
                </div>
            </li>
            <li class="wns-listitem">
                <div class="dnnFormItem">
                    <dnn:label id="lblOpacity" runat="server" ResourceKey="lblOpacity" ControlName="chkOpacity" Suffix=":" />
                    <asp:Checkbox ID="chkOpacity" runat="server" />
                </div>
            </li>
            <li class="wns-listitem">
                <div class="dnnFormItem">
                    <dnn:label id="lblModal" runat="server" ResourceKey="lblModal" ControlName="chkModal" Suffix=":" />
                    <asp:Checkbox ID="chkModal" runat="server" />
                </div>
            </li>
            <li class="wns-listitem">
                <div class="dnnFormItem">
                    <dnn:label id="lblCyclic" runat="server" ResourceKey="lblCyclic" ControlName="chkCyclic" Suffix=":" />
                    <asp:Checkbox ID="chkCyclic" runat="server" />
                </div>
            </li>
            <li class="wns-listitem">
                <div class="dnnFormItem">
                    <dnn:label id="lblOverlayShow" runat="server" ResourceKey="lblOverlayShow" ControlName="chkOverlayShow" Suffix=":" />
                    <asp:Checkbox ID="chkOverlayShow" runat="server" />
                </div>
            </li>
            <li class="wns-listitem">
                <div class="dnnFormItem">
                    <dnn:label id="lblOverlayOpacity" runat="server" ResourceKey="lblOverlayOpacity" ControlName="txtOverlayOpacity" Suffix=":" />
                    <asp:TextBox ID="txtOverlayOpacity" runat="server" CssClass="NormalTextBox dnnFormRequired" MaxLength="3" ValidationGroup="lightbox" /> 
                    <asp:RequiredFieldValidator ID="rfvOverlayOpacity" runat="server" ControlToValidate="txtOverlayOpacity" Display="Dynamic" CssClass="dnnFormMessage dnnFormError" ValidationGroup="lightbox" />
                    <asp:RegularExpressionValidator ID="revOverlayOpacity" runat="server" ControlToValidate="txtOverlayOpacity" Display="Dynamic" ValidationExpression="^[0-1][,.][0-9]$" CssClass="dnnFormMessage dnnFormError" ValidationGroup="lightbox" />
                </div>
            </li>
            <li class="wns-listitem">
                <div class="dnnFormItem">
                    <dnn:label id="lblOverlayColor" runat="server" ResourceKey="lblOverlayColor" ControlName="txtOverlayColor" Suffix=":" />
                    <asp:TextBox ID="txtOverlayColor" runat="server" CssClass="NormalTextBox dnnFormRequired" MaxLength="50" ValidationGroup="lightbox" /> 
                    <asp:RequiredFieldValidator ID="rfvOverlayColor" runat="server" ControlToValidate="txtOverlayColor" Display="Dynamic" CssClass="dnnFormMessage dnnFormError" ValidationGroup="lightbox" />
                </div>
            </li>
            <li class="wns-listitem">
                <div class="dnnFormItem">
                    <dnn:label id="lblTitleShow" runat="server" ResourceKey="lblTitleShow" ControlName="chkTitleShow" Suffix=":" />
                    <asp:Checkbox ID="chkTitleShow" runat="server" />
                </div>
            </li>
            <li class="wns-listitem">
                <div class="dnnFormItem">
                    <dnn:label id="lblTitlePosition" runat="server" ResourceKey="lblTitlePosition" ControlName="lstTitlePosition" Suffix=":" />
                    <asp:ListBox ID="lstTitlePosition" runat="server" SelectionMode="Single" Rows="1" /> 
                    <asp:RequiredFieldValidator ID="rfvTitlePosition" runat="server" ControlToValidate="lstTitlePosition" Display="Dynamic" InitialValue="---" CssClass="dnnFormMessage dnnFormError" ValidationGroup="lightbox" />
                </div>
            </li>
            <li class="wns-listitem">
                <div class="dnnFormItem">
                    <dnn:label id="lblTransition" runat="server" ResourceKey="lblTransition" ControlName="lstTransition" Suffix=":" />
                    <asp:ListBox ID="lstTransition" runat="server" SelectionMode="Single" Rows="1" CssClass="NormalTextBox dnnFormRequired" /> 
                    <asp:RequiredFieldValidator ID="rfvTransition" runat="server" ControlToValidate="lstTransition" Display="Dynamic" InitialValue="---" CssClass="dnnFormMessage dnnFormError" ValidationGroup="lightbox" />
                </div>
            </li>
            <li class="wns-listitem">
                <div class="dnnFormItem">
                    <dnn:label id="lblSpeed" runat="server" ResourceKey="lblSpeed" ControlName="txtSpeed" Suffix=":" />
                    <asp:TextBox ID="txtSpeed" runat="server" CssClass="NormalTextBox dnnFormRequired" MaxLength="4" ValidationGroup="lightbox" /> 
                    <asp:RequiredFieldValidator ID="rfvSpeed" runat="server" ControlToValidate="txtSpeed" Display="Dynamic" CssClass="dnnFormMessage dnnFormError" ValidationGroup="lightbox" />
                    <asp:RegularExpressionValidator ID="revSpeed" runat="server" ControlToValidate="txtSpeed" Display="Dynamic" ValidationExpression="^\d+$" CssClass="dnnFormMessage dnnFormError" ValidationGroup="lightbox" />
                </div>
            </li>
            <li class="wns-listitem">
                <div class="dnnFormItem">
                    <dnn:label id="lblChangeSpeed" runat="server" ResourceKey="lblChangeSpeed" ControlName="txtChangeSpeed" Suffix=":" />
                    <asp:TextBox ID="txtChangeSpeed" runat="server" CssClass="NormalTextBox dnnFormRequired" MaxLength="4" ValidationGroup="lightbox" /> 
                    <asp:RequiredFieldValidator ID="rfvChangeSpeed" runat="server" ControlToValidate="txtChangeSpeed" Display="Dynamic" CssClass="dnnFormMessage dnnFormError" ValidationGroup="lightbox" />
                    <asp:RegularExpressionValidator ID="revChangeSpeed" runat="server" ControlToValidate="txtChangeSpeed" Display="Dynamic" ValidationExpression="^\d+$" CssClass="dnnFormMessage dnnFormError" ValidationGroup="lightbox" />
                </div>
            </li>
            <li class="wns-listitem">
                <div class="dnnFormItem">
                    <dnn:label id="lblShowCloseButton" runat="server" ResourceKey="lblShowCloseButton" ControlName="chkShowCloseButton" Suffix=":" />
                    <asp:Checkbox ID="chkShowCloseButton" runat="server" />
                </div>
            </li>
            <li class="wns-listitem">
                <div class="dnnFormItem">
                    <dnn:label id="lblShowNavArrows" runat="server" ResourceKey="lblShowNavArrows" ControlName="chkShowNavArrows" Suffix=":" />
                    <asp:Checkbox ID="chkShowNavArrows" runat="server" />
                </div>
            </li>
            <li class="wns-listitem">
                <div class="dnnFormItem">
                    <dnn:label id="lblEnableEscapeButton" runat="server" ResourceKey="lblEnableEscapeButton" ControlName="chkEnableEscapeButton" Suffix=":" />
                    <asp:Checkbox ID="chkEnableEscapeButton" runat="server" />
                </div>
            </li>
            <li class="wns-listitem">
                <div class="dnnFormItem">
                    <dnn:label id="lblOnStart" runat="server" ResourceKey="lblOnStart" ControlName="txtOnStart" Suffix=":" />
                    <asp:TextBox ID="txtOnStart" runat="server" CssClass="NormalTextBox" MaxLength="100" ValidationGroup="lightbox" />
                </div>
            </li>
            <li class="wns-listitem">
                <div class="dnnFormItem">
                    <dnn:label id="lblOnCancel" runat="server" ResourceKey="lblOnCancel" ControlName="txtOnCancel" Suffix=":" />
                    <asp:TextBox ID="txtOnCancel" runat="server" CssClass="NormalTextBox" MaxLength="100" ValidationGroup="lightbox" />
                </div>
            </li>
            <li class="wns-listitem">
                <div class="dnnFormItem">
                    <dnn:label id="lblOnComplete" runat="server" ResourceKey="lblOnComplete" ControlName="txtOnComplete" Suffix=":" />
                    <asp:TextBox ID="txtOnComplete" runat="server" CssClass="NormalTextBox" MaxLength="100" ValidationGroup="lightbox" />
                </div>
            </li>
            <li class="wns-listitem">
                <div class="dnnFormItem">
                    <dnn:label id="lblOnCleanup" runat="server" ResourceKey="lblOnCleanup" ControlName="txtOnCleanup" Suffix=":" />
                    <asp:TextBox ID="txtOnCleanup" runat="server" CssClass="NormalTextBox" MaxLength="100" ValidationGroup="lightbox" />
                </div>
            </li>
            <li class="wns-listitem">
                <div class="dnnFormItem">
                    <dnn:label id="lblOnClosed" runat="server" ResourceKey="lblOnClosed" ControlName="txtOnClosed" Suffix=":" />
                    <asp:TextBox ID="txtOnClosed" runat="server" CssClass="NormalTextBox" MaxLength="100" ValidationGroup="lightbox" />
                </div>
            </li>
        </ol>
    </fieldset>
    <div class="dnnClear">
        <asp:LinkButton ID="cmdUpdate" runat="server" CssClass="dnnPrimaryAction" CausesValidation="true" ValidationGroup="lightbox" />&nbsp; 
        <asp:LinkButton ID="cmdDelete" runat="server" CssClass="dnnSecondaryAction" CausesValidation="false" />&nbsp; 
        <asp:LinkButton ID="cmdDeleteThumbnails" runat="server" CssClass="dnnSecondaryAction" CausesValidation="false" />&nbsp; 
        <asp:LinkButton ID="cmdCancel" runat="server" CssClass="dnnSecondaryAction" CausesValidation="false" />
    </div>
</div>
<script language="javascript" type="text/javascript">/*<![CDATA[*/
    var delText = '<%=Me.GetLocalizedString("Delete.Confirm").Replace("'", "") %>';

    (function ($, Sys) {
        function setupDnnSiteSettings() {
            $('#dnnEditEntry').dnnPanels();

            /* add a friend confirmation message for when a deletion is requested */
            $('#<%= Me.cmdDelete.ClientId %>').dnnConfirm({
                text: delText,
                yesText: '<%= Localization.GetString("Yes.Text", Localization.SharedResourceFile) %>',
                noText: '<%= Localization.GetString("No.Text", Localization.SharedResourceFile) %>',
                title: '<%= Localization.GetString("Confirm.Text", Localization.SharedResourceFile) %>'
            });
            
            $('#<%=Me.cmdDeleteThumbnails.ClientId%>').dnnConfirm({
                text: '<%= Me.GetLocalizedString("DeleteThumbnails.Confirm").Replace("'", "") %>',
                yesText: '<%= Localization.GetString("Yes.Text", Localization.SharedResourceFile) %>',
                noText: '<%= Localization.GetString("No.Text", Localization.SharedResourceFile) %>',
                title: '<%= Localization.GetString("Confirm.Text", Localization.SharedResourceFile) %>'
            });

            /* focus the cursor on the first form field */
            jQuery('#<%=Me.txtGalleryName.ClientId%>').focus();
        }

        $(document).ready(function () {
            setupDnnSiteSettings();
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
                setupDnnSiteSettings();
            });
        });

    } (jQuery, window.Sys));
/*]]>*/</script>