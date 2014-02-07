<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="View.ascx.cs" Inherits="DNNCommunity.Modules.UGLabsUserGroupData.View" %>
<%@ Register TagName="label" TagPrefix="dnn" Src="~/controls/labelcontrol.ascx" %>
<asp:Panel ID="pnlUserGroupData" runat="server" Visible="True">
    <div class="dnnFormMessage dnnFormInfo uglabLeaderMessage"><%=GetLocalizedString("LeaderMessage.Text") %></div>
    <div class="uglabDataForm dnnClear">
        <div class="dnnFormItem dnnFormHelp dnnClear">
            <p class="dnnFormRequired"><span><%=GetLocalizedString("lblRequired.Text") %></span></p>
        </div>
        <h2 id="dnnSitePanel-BasicSettings" class="dnnFormSectionHead"><a href="" class="dnnSectionExpanded"><%=LocalizeString("BasicSettings.Text")%></a></h2>
        <fieldset>
            <div class="dnnFormItem">
                <dnn:label id="lblGroupName" runat="server" ResourceKey="lblGroupName" ControlName="txtGroupName" Suffix=":" CssClass="dnnFormRequired" />
                <asp:TextBox ID="txtGroupName" runat="server" MaxLength="50" CssClass="dnnFormRequired" ValidationGroup="UserGroupData" />
                <asp:RequiredFieldValidator ID="rfvGroupName" ControlToValidate="txtGroupName" runat="server" CssClass="dnnFormMessage dnnFormError" Display="Dynamic" ValidationGroup="UserGroupData" />
                <asp:RegularExpressionValidator ID="revGroupName" CssClass="dnnFormMessage dnnFormError" runat="server" ControlToValidate="txtGroupName" Display="Dynamic" ValidationGroup="UserGroupData" ValidationExpression="[A-Za-z0-9\.\s_-]*" />
            </div>
            <div class="dnnFormItem">
                <dnn:label id="lblCountry" runat="server" ResourceKey="lblCountry" ControlName="cboCountry" Suffix=":" CssClass="dnnFormRequired" />
                <asp:DropDownList ID="cboCountry" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cboCountry_SelectionChanged" ValidationGroup="UserGroupData" />
                <asp:RequiredFieldValidator ID="rfvCountry" ControlToValidate="cboCountry" runat="server" InitialValue="---" Display="Dynamic" CssClass="dnnFormMessage dnnFormError" ValidationGroup="UserGroupData" />
            </div>
            <div class="dnnFormItem">
                <dnn:label id="lblRegion" runat="server" ResourceKey="lblRegion" ControlName="cboRegion" Suffix=":" />
                <asp:DropDownList ID="cboRegion" runat="server" ValidationGroup="UserGroupData" />
                <asp:TextBox ID="txtRegion" runat="server" MaxLength="50" ValidationGroup="UserGroupData" />
                <asp:RequiredFieldValidator ID="rfvRegion" ControlToValidate="cboRegion" runat="server" InitialValue="---" Display="Dynamic" CssClass="dnnFormMessage dnnFormError" ValidationGroup="UserGroupData" />
            </div>
            <div class="dnnFormItem">
                <dnn:label id="lblCity" runat="server" ResourceKey="lblCity" ControlName="txtCity" Suffix=":" CssClass="dnnFormRequired" />
                <asp:TextBox ID="txtCity" runat="server" MaxLength="50" CssClass="dnnFormRequired" ValidationGroup="UserGroupData" />
                <asp:RequiredFieldValidator ID="rfvCity" ControlToValidate="txtCity" runat="server" Display="Dynamic" CssClass="dnnFormMessage dnnFormError" ValidationGroup="UserGroupData" />
            </div>
            <div class="dnnFormItem">
                <dnn:label id="lblDefaultLanguage" runat="server" ResourceKey="lblDefaultLanguage" ControlName="cboDefaultLanguage" Suffix=":" CssClass="dnnFormRequired" />
                <asp:DropDownList ID="cboDefaultLanguage" runat="server" CssClass="dnnFormRequired" ValidationGroup="UserGroupData" />
                <asp:RequiredFieldValidator ID="rfvDefaultLanguage" ControlToValidate="cboDefaultLanguage" runat="server" InitialValue="---" Display="Dynamic" CssClass="dnnFormMessage dnnFormError" ValidationGroup="UserGroupData" />
            </div>
            <div class="dnnFormItem">
                <dnn:label id="lblWebsiteUrl" runat="server" ResourceKey="lblWebsiteUrl" ControlName="txtWebsiteUrl" Suffix=":" />
                <asp:TextBox ID="txtWebsiteUrl" runat="server" MaxLength="255" ValidationGroup="UserGroupData" />
                <asp:RegularExpressionValidator ID="revWebsiteUrl" ControlToValidate="txtWebsiteUrl" runat="server" Display="Dynamic" CssClass="dnnFormMessage dnnFormError" ValidationGroup="UserGroupData" />
            </div>
        </fieldset>
        <h2 id="dnnSitePanel-AdvancedSettings" class="dnnFormSectionHead"><a href="" class="dnnSectionExpanded"><%=LocalizeString("AdvancedSettings.Text")%></a></h2>
        <fieldset>
            <div class="dnnFormItem">
                <dnn:label id="lblFacebookUrl" runat="server" ResourceKey="lblFacebookUrl" ControlName="txtFacebookUrl" Suffix=":" />
                <asp:TextBox ID="txtFacebookUrl" runat="server" MaxLength="255" ValidationGroup="UserGroupData" />
                <asp:RegularExpressionValidator ID="revFacebookUrl" ControlToValidate="txtFacebookUrl" runat="server" Display="Dynamic" CssClass="dnnFormMessage dnnFormError" ValidationGroup="UserGroupData" />
            </div>
            <div class="dnnFormItem">
                <dnn:label id="lblTwitterUrl" runat="server" ResourceKey="lblTwitterUrl" ControlName="txtTwitterUrl" Suffix=":" />
                <asp:TextBox ID="txtTwitterUrl" runat="server" MaxLength="255" ValidationGroup="UserGroupData" />
                <asp:RegularExpressionValidator ID="revTwitterUrl" ControlToValidate="txtTwitterUrl" runat="server" Display="Dynamic" CssClass="dnnFormMessage dnnFormError" ValidationGroup="UserGroupData" />
            </div>
            <div class="dnnFormItem">
                <dnn:label id="lblLinkedInUrl" runat="server" ResourceKey="lblLinkedInUrl" ControlName="txtLinkedInUrl" Suffix=":" />
                <asp:TextBox ID="txtLinkedInUrl" runat="server" MaxLength="255" ValidationGroup="UserGroupData" />
                <asp:RegularExpressionValidator ID="revLinkedInUrl" ControlToValidate="txtLinkedInUrl" runat="server" Display="Dynamic" CssClass="dnnFormMessage dnnFormError" ValidationGroup="UserGroupData" />
            </div>
            <div class="dnnFormItem">
                <dnn:label id="lblGooglePlusUrl" runat="server" ResourceKey="lblGooglePlusUrl" ControlName="txtGooglePlusUrl" Suffix=":" />
                <asp:TextBox ID="txtGooglePlusUrl" runat="server" MaxLength="255" ValidationGroup="UserGroupData" />
                <asp:RegularExpressionValidator ID="revGooglePlusUrl" ControlToValidate="txtGooglePlusUrl" runat="server" Display="Dynamic" CssClass="dnnFormMessage dnnFormError" ValidationGroup="UserGroupData" />
            </div>
            <div class="dnnFormItem">
                <dnn:label id="lblMeetUpUrl" runat="server" ResourceKey="lblMeetUpUrl" ControlName="txtMeetUpUrl" Suffix=":" />
                <asp:TextBox ID="txtMeetUpUrl" runat="server" MaxLength="255" ValidationGroup="UserGroupData" />
                <asp:RegularExpressionValidator ID="revMeetUpUrl" ControlToValidate="txtMeetUpUrl" runat="server" Display="Dynamic" CssClass="dnnFormMessage dnnFormError" ValidationGroup="UserGroupData" />
            </div>
            <div class="dnnFormItem">
                <dnn:label id="lblYouTubeUrl" runat="server" ResourceKey="lblYouTubeUrl" ControlName="txtYouTubeUrl" Suffix=":" />
                <asp:TextBox ID="txtYouTubeUrl" runat="server" MaxLength="255" ValidationGroup="UserGroupData" />
                <asp:RegularExpressionValidator ID="revYouTubeUrl" ControlToValidate="txtYouTubeUrl" runat="server" Display="Dynamic" CssClass="dnnFormMessage dnnFormError" ValidationGroup="UserGroupData" />
            </div>
        </fieldset>
        <div class="dnnFormItem">
            <asp:LinkButton ID="lnkSave" runat="server" OnClick="lnkSave_Click" CssClass="dnnPrimaryAction" ValidationGroup="UserGroupData" />&nbsp; 
            <asp:LinkButton ID="lnkReset" runat="server" OnClick="lnkReset_Click" CssClass="dnnSecondaryAction" CausesValidation="False" ValidationGroup="UserGroupData" />
        </div>
    </div>
    <script language="javascript" type="text/javascript">/*<![CDATA[*/
        (function ($, Sys) {
            function setupDnnSiteSettings() {
                $('.uglabDataForm').dnnPanels();
            }

            $(document).ready(function () {
                setupDnnSiteSettings();
                Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
                    setupDnnSiteSettings();
                });
            });

        }(jQuery, window.Sys));
    /*]]>*/</script>
</asp:Panel>