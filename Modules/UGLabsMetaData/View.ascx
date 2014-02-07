<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="View.ascx.cs" Inherits="DNNCommunity.Modules.UGLabsMetaData.View" %>
<%@ Import namespace="DotNetNuke.Services.Localization" %>
<asp:Repeater ID="rptSettings" runat="server" OnItemCommand="rptSettings_ItemCommand">
    <HeaderTemplate>
        <table id="tblRoleSettings" class="dnnGrid grpSettingsGrid">
            <tr class="dnnGridHeader">
                <td class="dnnGridHeader"><%= GetLocalizedString("Grid.Header.Edit") %></td>
                <td class="dnnGridHeader"><%= GetLocalizedString("Grid.Header.Key") %></td>
                <td class="dnnGridHeader"><%= GetLocalizedString("Grid.Header.Value") %></td>
            </tr>
    </HeaderTemplate>
    <ItemTemplate>
        <tr>
            <td class="dnnGridItem">
                <a href="<%#ReformatUrlForEdit(DataBinder.Eval(Container.DataItem, "Key")) %>"><img src="<%=EditImage %>" alt="<%=GetLocalizedString("Edit.Text") %>" title="<%=GetLocalizedString("Edit.Text") %>" /></a>
            </td>
            <td class="dnnGridItem"><%# DataBinder.Eval(Container.DataItem, "Key") %></td>
            <td class="dnnGridItem"><%# ParseMetaDataValue(DataBinder.Eval(Container.DataItem, "Value")) %></td>
        </tr>
    </ItemTemplate>
    <AlternatingItemTemplate>
        <tr>
            <td class="dnnGridAltItem">
                <a href="<%#ReformatUrlForEdit(DataBinder.Eval(Container.DataItem, "Key")) %>"><img src="<%=EditImage %>" alt="<%=GetLocalizedString("Edit.Text") %>" title="<%=GetLocalizedString("Edit.Text") %>" /></a>
            </td>
            <td class="dnnGridAltItem"><%# DataBinder.Eval(Container.DataItem, "Key") %></td>
            <td class="dnnGridAltItem"><%# ParseMetaDataValue(DataBinder.Eval(Container.DataItem, "Value")) %></td>
        </tr>
    </AlternatingItemTemplate>
    <FooterTemplate>
        </table>
    </FooterTemplate>
</asp:Repeater>
<asp:Label ID="lblMessage" runat="server" />
<script language="javascript" type="text/javascript">/*<![CDATA[*/
    (function ($, Sys) {
        function setupDnnSiteSettings() {
            
        }

        $(document).ready(function () {
            setupDnnSiteSettings();
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
                setupDnnSiteSettings();
            });
        });

    }(jQuery, window.Sys));
    /*]]>*/</script>