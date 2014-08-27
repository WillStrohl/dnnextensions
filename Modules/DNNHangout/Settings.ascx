<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Settings.ascx.cs" Inherits="WillStrohl.Modules.DNNHangout.Settings" %>
<%@ Register TagName="label" TagPrefix="dnn" Src="~/controls/labelcontrol.ascx" %>
<%@ Import namespace="DotNetNuke.Services.Localization" %>
<h2 id="dnnSitePanel-BasicSettings" class="dnnFormSectionHead"><a href="" class="dnnSectionExpanded"><%=LocalizeString("BasicSettings")%></a></h2>
<fieldset>
    <div class="dnnFormItem">
        <dnn:Label ID="lblTemplate" runat="server" /> 
        <asp:TextBox ID="txtTemplate" runat="server" TextMode="MultiLine" Rows="12" />
    </div>
    <div class="dnnFormItem">
        <div class="dnnRight">
            <asp:LinkButton runat="server" ID="lnkRestore" ResourceKey="lnkRestore" CssClass="dnnTertiaryAction" OnClick="lnkRestore_OnClick" CausesValidation="False" />
        </div>
    </div>
    <div class="dnnFormItem">
        <div class="dnnFormMessage dnnFormInfo"><%=Localization.GetString("TokenKey.Text", LocalResourceFile) %></div>
    </div>
    <div class="dnnFormItem">
        <dnn:Label ID="lblTemplateScope" runat="server" /> 
        <asp:CheckBox ID="chkTemplateScope" runat="server" />
    </div>
</fieldset>