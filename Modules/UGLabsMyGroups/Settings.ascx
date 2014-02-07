<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Settings.ascx.cs" Inherits="DNNCommunity.Modules.MyGroups.Settings" %>
<%@ Register TagName="label" TagPrefix="dnn" Src="~/controls/labelcontrol.ascx" %>
<h2 id="dnnSitePanel-BasicSettings" class="dnnFormSectionHead"><a href="" class="dnnSectionExpanded"><%=LocalizeString("BasicSettings")%></a></h2>
<fieldset id="fsMyGroupSettings">
    <div id="divMessageWrapper" class="dnnFormItem" runat="server">
        <div id="divMessage" class="dnnFormMessage dnnFormValidationSummary" runat="server"></div>
    </div>
    <div class="dnnFormItem">
        <dnn:label id="lblGroupViewPage" resourcekey="lblGroupViewPage" ControlName="ddlGroupViewPage" runat="server" />
        <asp:DropDownList ID="ddlGroupViewPage" runat="server" CssClass="NormalTextbox dnnFormRequired"/>
        <asp:RequiredFieldValidator ID="rfvGroupViewPage" runat="server" ControlToValidate="ddlGroupViewPage" CssClass="dnnFormMessage dnnFormError" Display="Dynamic" />
    </div>
</fieldset>