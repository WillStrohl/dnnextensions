<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Settings.ascx.cs" Inherits="WillStrohl.Modules.CodeCamp.Settings" %>
<%@ Register TagName="label" TagPrefix="dnn" Src="~/controls/labelcontrol.ascx" %>
<h2 id="dnnSitePanel-BasicSettings" class="dnnFormSectionHead"><a href="" class="dnnSectionExpanded"><%=GetLocalizedString("ViewSettings")%></a></h2>
<fieldset>
    <div class="dnnFormItem">
        <dnn:label id="lblView" runat="server" resourcekey="lblView"/>
        <asp:DropDownList runat="server" ID="ddlView"/>
    </div>
    <div class="dnnFormItem">
        <dnn:label id="lblIncludeBootstrap" runat="server" resourcekey="lblIncludeBootstrap"/>
        <asp:CheckBox runat="server" ID="chkIncludeBootstrap"/>
    </div>
</fieldset>