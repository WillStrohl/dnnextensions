<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ViewSlider.ascx.vb" Inherits="WillStrohl.Modules.ContentSlider.ViewSlider" %>
<asp:PlaceHolder ID="phSlider" runat="server" />
<asp:PlaceHolder ID="phScript" runat="server" />
<% If CanAppendSlidersAndScript() Then %>
<script language="javascript"type="text/javascript">/*<![CDATA[*/

    (function ($, Sys) {
        function setupDnnSiteSettings() {
            initWnsContentSlider<%=TabModuleId %>();
        }

        $(document).ready(function () {
            setupDnnSiteSettings();
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
                setupDnnSiteSettings();
            });
        });

    } (jQuery, window.Sys));

/*]]>*/</script>
<% End If %>