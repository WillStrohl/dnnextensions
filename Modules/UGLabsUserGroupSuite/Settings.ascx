<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Settings.ascx.cs" Inherits="DNNCommunity.Modules.UserGroupSuite.Settings" %>
<%@ Register TagName="label" TagPrefix="dnn" Src="~/controls/labelcontrol.ascx" %>
<h2 id="dnnSitePanel-BasicSettings" class="dnnFormSectionHead"><a href="" class="dnnSectionExpanded"><%=GetLocalizedString("ViewSettings")%></a></h2>
<fieldset>
    <div class="dnnFormItem">
        <dnn:Label ID="lblAddLanguages" runat="server" /> 
        <asp:Button ID="btnAddLanguages" runat="server" OnClick="btnAddLanguages_OnClick"  />
    </div>
</fieldset>