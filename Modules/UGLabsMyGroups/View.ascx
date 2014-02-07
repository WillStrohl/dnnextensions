<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="View.ascx.cs" Inherits="DNNCommunity.Modules.MyGroups.View" %>
<div class="dnnForm dnnGroupDirectory">
    <div class="dgdMainContent">
        <asp:Repeater id="rptGroupList" runat="server">
            <ItemTemplate>
                <div class="divGroupAvatarWrap">
                    <div class="divAvatar"><a href="<%# GetGroupPageUrl(DataBinder.Eval(Container.DataItem, "RoleId")) %>"><img 
                        src="<%# ParseGroupIconFile(DataBinder.Eval(Container.DataItem, "RoleId"), DataBinder.Eval(Container.DataItem, "IconFile")) %>" 
                        alt="<%# DataBinder.Eval(Container.DataItem, "RoleName") %>" 
                        title="<%# DataBinder.Eval(Container.DataItem, "RoleName") %>" /></a></div>
                </div>
            </ItemTemplate>
        </asp:Repeater>
        <asp:Literal ID="litMessage" runat="server" />
    </div>
</div>