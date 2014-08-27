<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="View.ascx.cs" Inherits="WillStrohl.Modules.DNNHangout.View" %>
<div class="gh-wrapper">
    <asp:Panel runat="server" ID="pnlHangout" CssClass="gh-hangout">
        <asp:placeholder id="phHangout" runat="server" />
    </asp:Panel>
    <asp:Panel runat="server" ID="pnlNoConfig" CssClass="gh-noconfig">
        <div class="dnnClear">
            <h2><%=GetLocalizedString("Title.Text") %></h2>
            <p><%=GetLocalizedString("Description.Text") %></p>
            <p><img src="/DesktopModules/DNNHangout/Images/Google-Hangouts-Logo.png" alt="<%=GetLocalizedString("Alt.Text") %>"/></p>
        </div>
    </asp:Panel>
</div>
<%-- 
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
--%>