<%@ Control language="C#" Inherits="DotNetNuke.Modules.WillStrohlDisqus.Edit" AutoEventWireup="false" Codebehind="Edit.ascx.cs" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register TagPrefix="dnnweb" Assembly="DotNetNuke.Web" Namespace="DotNetNuke.Web.UI.WebControls" %>
<%@ Import Namespace="DotNetNuke.Services.Localization" %>
<div id="dnnDisqusSettings" class="dnnForm dnnEditEntry dnnClear">
    <h2 id="dnnPanel-h2Settings" class="dnnFormSectionHead"><a href="" class="dnnLabelExpanded"><%= this.GetLocalizedString("Settings.Text")%></a></h2>
    <fieldset id="fsSettings" class="wns-fieldset">
        <ol id="olAlbum" class="wns-list">
            <li class="wns-listitem">
                <div class="dnnFormItem">
                    <dnn:label id="lblModuleView" runat="server" ResourceKey="lblModuleView" ControlName="cboModuleView" Suffix=":" />
                    <asp:DropDownList ID="cboModuleView" runat="server" CssClass="NormalTextBox dnnFormRequired" AutoPostBack="true" OnSelectedIndexChanged="CboModuleViewSelectedIndexChanged" ValidationGroup="disqus" /> 
                </div>
            </li>
            <li id="liModuleList" class="wns-listitem disqus-comments" runat="server">
                <div class="dnnFormItem">
                    <dnn:label id="lblModuleList" runat="server" ResourceKey="lblModuleList" ControlName="cboModuleList" Suffix=":" />
                    <asp:DropDownList ID="cboModuleList" runat="server" CssClass="NormalTextBox dnnFormRequired" ValidationGroup="disqus" /> 
                    <asp:RequiredFieldValidator ID="rfvModuleList" runat="server" InitialValue="---" ControlToValidate="cboModuleList" Display="Dynamic" CssClass="dnnFormMessage dnnFormError" ValidationGroup="disqus" />
                </div>
            </li>
            <li id="liDisplayItems" class="wns-listitem" runat="server">
                <div class="dnnFormItem">
                    <dnn:label id="lblDisplayItems" runat="server" ResourceKey="lblDisplayItems" ControlName="cboDisplayItems" Suffix=":" />
                    <asp:DropDownList ID="cboDisplayItems" runat="server" CssClass="NormalTextBox dnnFormRequired" ValidationGroup="disqus" /> 
                    <asp:CustomValidator ID="cvDisplayItems" runat="server" ControlToValidate="cboDisplayItems" Display="Dynamic" CssClass="dnnFormMessage dnnFormError" ValidationGroup="disqus" />
                </div>
            </li>
            <li id="liShowModerators" class="wns-listitem" runat="server">
                <div class="dnnFormItem">
                    <dnn:label id="lblShowModerators" runat="server" ResourceKey="lblShowModerators" ControlName="chkShowModerators" Suffix=":" />
                    <asp:CheckBox ID="chkShowModerators" runat="server" /> 
                </div>
            </li>
            <li id="liColorTheme" class="wns-listitem" runat="server">
                <div class="dnnFormItem">
                    <dnn:label id="lblColorTheme" runat="server" ResourceKey="lblColorTheme" ControlName="cboColorTheme" Suffix=":" />
                    <asp:DropDownList ID="cboColorTheme" runat="server" CssClass="NormalTextBox dnnFormRequired" ValidationGroup="disqus" /> 
                    <asp:CustomValidator ID="cvColorTheme" runat="server" ControlToValidate="cboColorTheme" Display="Dynamic" CssClass="dnnFormMessage dnnFormError" ValidationGroup="disqus" />
                </div>
            </li>
            <li id="liDefaultTab" class="wns-listitem" runat="server">
                <div class="dnnFormItem">
                    <dnn:label id="lblDefaultTab" runat="server" ResourceKey="lblDefaultTab" ControlName="cboDefaultTab" Suffix=":" />
                    <asp:DropDownList ID="cboDefaultTab" runat="server" CssClass="NormalTextBox dnnFormRequired" ValidationGroup="disqus" /> 
                    <asp:CustomValidator ID="cvDefaultTab" runat="server" ControlToValidate="cboDefaultTab" Display="Dynamic" CssClass="dnnFormMessage dnnFormError" ValidationGroup="disqus" />
                </div>
            </li>
            <li id="liCommentLength" class="wns-listitem" runat="server">
                <div class="dnnFormItem">
                    <dnn:label id="lblCommentLength" runat="server" ResourceKey="lblCommentLength" ControlName="txtCommentLength" Suffix=":" />
                    <asp:TextBox ID="txtCommentLength" runat="server" MaxLength="4" CssClass="NormalTextBox dnnFormRequired" ValidationGroup="disqus" /> 
                    <asp:RegularExpressionValidator ID="revCommentLength" runat="server" ControlToValidate="txtCommentLength" Display="Dynamic" CssClass="dnnFormMessage dnnFormError" ValidationExpression="^\d+$" ValidationGroup="disqus" />
                </div>
            </li>
            <li id="liShowAvatar" class="wns-listitem" runat="server">
                <div class="dnnFormItem">
                    <dnn:label id="lblShowAvatar" runat="server" ResourceKey="lblShowAvatar" ControlName="chkShowAvatar" Suffix=":" />
                    <asp:CheckBox ID="chkShowAvatar" runat="server" CssClass="NormalTextBox dnnFormRequired" ValidationGroup="disqus" /> 
                </div>
            </li>
            <li id="liAvatarSize" class="wns-listitem" runat="server">
                <div class="dnnFormItem">
                    <dnn:label id="lblAvatarSize" runat="server" ResourceKey="lblAvatarSize" ControlName="cboAvatarSize" Suffix=":" />
                    <asp:DropDownList ID="cboAvatarSize" runat="server" CssClass="NormalTextBox dnnFormRequired" ValidationGroup="disqus" /> 
                </div>
            </li>
        </ol>
    </fieldset>
    <h2 id="dnnPanel-h2SiteSettings" class="dnnFormSectionHead"><a href="" class="dnnLabelExpanded"><%= this.GetLocalizedString("SiteSettings.Text")%></a></h2>
    <fieldset id="fsSiteSettings" class="wns-fieldset">
        <ol id="olSiteSettings" class="wns-list">
            <li class="wns-listitem">
                <div class="dnnFormItem">
                    <div class="dnnFormMessage dnnFormWarning">
                        <%= this.GetLocalizedString("Disqus.AppName.Message") %>
                    </div>
                </div>
            </li>
            <li class="wns-listitem">
                <div class="dnnFormItem">
                    <dnn:label id="lblAppName" runat="server" ResourceKey="lblAppName" ControlName="txtAppName" Suffix=":" />
                    <asp:TextBox ID="txtAppName" runat="server" MaxLength="150" CssClass="NormalTextBox dnnFormRequired" ValidationGroup="disqus" /> 
                    <asp:RequiredFieldValidator ID="rfvAppName" runat="server" ControlToValidate="txtAppName" Display="Dynamic" CssClass="dnnFormMessage dnnFormError" ValidationGroup="disqus" />
                </div>
            </li>
            <li class="wns-listitem">
                <div class="dnnFormItem">
                    <dnn:label id="lblApiSecret" runat="server" ResourceKey="lblApiSecret" ControlName="txtApiSecret" Suffix=":" />
                    <asp:TextBox ID="txtApiSecret" runat="server" MaxLength="250" CssClass="NormalTextBox dnnFormRequired" ValidationGroup="disqus" /> 
                    <asp:RequiredFieldValidator ID="rfvApiSecret" runat="server" ControlToValidate="txtApiSecret" Display="Dynamic" CssClass="dnnFormMessage dnnFormError" ValidationGroup="disqus" />
                </div>
            </li>
            <li class="wns-listitem">
                <div class="dnnFormItem">
                    <dnn:Label ID="lblRequireDnnLogin" runat="server" ResourceKey="lblRequireDnnLogin" ControlName="chkRequireDnnLogin" Suffix=":" />
                    <asp:checkbox ID="chkRequireDnnLogin" runat="server" />
                </div>
            </li>
            <li class="wns-listitem">
                <div class="dnnFormItem">
                    <dnn:Label ID="lblSsoEnabled" runat="server" ResourceKey="lblSsoEnabled" ControlName="chkSsoEnabled" Suffix=":" />
                    <asp:checkbox ID="chkSsoEnabled" runat="server" OnCheckedChanged="ChkSsoEnabledClick" AutoPostBack="True" CausesValidation="False" />
                </div>
            </li>
            <li class="wns-listitem">
                <div class="dnnFormItem">
                    <dnn:label id="lblSsoApiKey" runat="server" ResourceKey="lblSsoApiKey" ControlName="txtSsoApiKey" Suffix=":" />
                    <asp:TextBox ID="txtSsoApiKey" runat="server" MaxLength="250" CssClass="NormalTextBox" ValidationGroup="disqus" /> 
                </div>
            </li>
            <li class="wns-listitem">
                <div class="dnnFormItem">
                    <dnn:Label ID="lblDeveloperMode" runat="server" ResourceKey="lblDeveloperMode" ControlName="chkDeveloperMode" Suffix=":" />
                    <asp:checkbox ID="chkDeveloperMode" runat="server" />
                </div>
            </li>
        </ol>
    </fieldset>
    <asp:Panel ID="pnlReceiverFile" runat="server" Visible="False">
        <h2 id="dnnPanel-h2ReceiverFile" class="dnnFormSectionHead"><a href="" class=""><%= this.GetLocalizedString("ReceiverFile.Text")%></a></h2>
        <fieldset id="fsReceiverFile" class="wns-fieldset">
            <ol id="olReceiverFile" class="wns-list">
                <li class="wns-listitem">
                    <div class="dnnFormItem">
                        <dnn:Label ID="lblDisqusReceiver" runat="server" ResourceKey="lblDisqusReceiver" ControlName="lblReceiverFile" Suffix=":" />
                        <asp:Label ID="lblReceiverFile" runat="server" CssClass="Normal" />
                    </div>
                </li>
            </ol>
        </fieldset>
    </asp:Panel>
    <asp:Panel ID="pnlHost" runat="server" Visible="False">
        <h2 id="dnnPanel-h2HostSettings" class="dnnFormSectionHead"><a href="" class=""><%= this.GetLocalizedString("HostSettings.Text")%></a></h2>
        <fieldset id="fsHostSettings" class="wns-fieldset">
            <ol id="olHostSettings" class="wns-list">
                <li class="wns-listitem">
                    <div class="dnnFormItem">
                        <div class="dnnFormMessage dnnFormInfo"><%=GetLocalizedString("HostScheduler.Text") %></div>
                    </div>
                </li>
                <li class="wns-listitem">
                    <div class="dnnFormItem">
                        <dnn:Label ID="lblSchedule" runat="server" ResourceKey="lblSchedule" ControlName="chkSchedule" Suffix=":" />
                        <asp:CheckBox ID="chkSchedule" runat="server"/>
                    </div>
                </li>
            </ol>
        </fieldset>
    </asp:Panel>
</div>
<div id="dnnDisqusCommands" class="dnnClear">
    <asp:LinkButton ID="cmdSave" runat="server" CssClass="dnnPrimaryAction" ValidationGroup="disqus" /> &nbsp; 
    <asp:LinkButton ID="cmdReturn" runat="server" CssClass="dnnSecondaryAction" CausesValidation="false" /> &nbsp; 
    <asp:LinkButton ID="cmdReceiverFile" runat="server" CssClass="dnnSecondaryAction" CausesValidation="false" />
</div>
<script language="javascript" type="text/javascript">/*<![CDATA[*/
    (function ($, Sys) {
        function setupDnnSiteSettings() {
            $('#dnnDisqusSettings').dnnPanels();
        }

        $(document).ready(function () {
            setupDnnSiteSettings();
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
                setupDnnSiteSettings();
            });
        });

    } (jQuery, window.Sys));
/*]]>*/</script>