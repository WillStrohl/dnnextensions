<%@ Control language="C#" Inherits="DotNetNuke.Modules.WillStrohlDisqus.Settings" AutoEventWireup="false" Codebehind="Settings.ascx.cs" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Import Namespace="DotNetNuke.Services.Localization" %>
<div id="dnnDisqusSettings" class="dnnForm dnnEditEntry dnnClear">
    <h2 id="dnnPanel-h2Settings" class="dnnFormSectionHead"><a href="" class="dnnLabelExpanded"><%= this.GetLocalizedString("Settings.Text")%></a></h2>
    <fieldset id="fsSettings">
        <div class="dnnFormItem">
            <dnn:label id="lblPortalGuidOverride" runat="server" ResourceKey="lblPortalGuidOverride" ControlName="txtPortalGuidOverride" Suffix=":" />
            <asp:TextBox ID="txtPortalGuidOverride" runat="server" MaxLength="255" CssClass="NormalTextBox" />
        </div>
        <div class="dnnFormItem">
            <dnn:label id="lblTabIdOverride" runat="server" ResourceKey="lblTabIdOverride" ControlName="txtTabIdOverride" Suffix=":" />
            <asp:TextBox ID="txtTabIdOverride" runat="server" MaxLength="10" CssClass="NormalTextBox" />
            <asp:RangeValidator runat="server" ID="rvTabIdOverride" Type="Integer" MinimumValue="0" MaximumValue="999999999" ControlToValidate="txtTabIdOverride" Display="Dynamic" CssClass="dnnFormMessage dnnFormError"  />
        </div>
        <div class="dnnFormItem">
            <dnn:label id="lblTabModuleIdOverride" runat="server" ResourceKey="lblTabModuleIdOverride" ControlName="txtTabModuleIdOverride" Suffix=":" />
            <asp:TextBox ID="txtTabModuleIdOverride" runat="server" MaxLength="10" CssClass="NormalTextBox" />
            <asp:RangeValidator runat="server" ID="rvTabModuleIdOverride" Type="Integer" MinimumValue="0" MaximumValue="999999999" ControlToValidate="txtTabModuleIdOverride" Display="Dynamic" CssClass="dnnFormMessage dnnFormError"  />
        </div>
    </fieldset>
</div>