<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ChangeOrder.ascx.vb" Inherits="WillStrohl.Modules.Lightbox.ChangeOrderView" %>
<div id="dnnEditEntry" class="dnnForm dnnEditEntry dnnClear">
    <asp:PlaceHolder ID="phOrder" runat="server" />
    <div id="divActions" class="dnnFormItem">
        <a id="lnkSave" href="#" class="dnnPrimaryAction" title="<%=Me.GetLocalizedString("lnkSave.Text")%>"><%=Me.GetLocalizedString("lnkSave.Text")%></a>&nbsp; 
        <a id="lnkCancel" href="#" class="dnnSecondaryAction" title="<%=Me.GetLocalizedString("lnkCancel.Text")%>"><%=Me.GetLocalizedString("lnkCancel.Text")%></a>
    </div>
    <div class="Hidden">
        <div id="dlgAjaxError" title="<%=Me.GetLocalizedString("dlgAjaxError.Title")%>"><%=Me.GetLocalizedString("dlgAjaxError.Text")%></div>
        <div id="dlgSaving" title="<%=Me.GetLocalizedString("dlgSaving.Title")%>"><%=Me.GetLocalizedString("dlgSaving.Text")%></div>
    </div>
</div>
<script language="javascript" type="text/javascript">/*<![CDATA[*/
    var returnUrl = '<%=NavigateUrl%>';
    var orderUrl = '<%=Me.OrderHandler%>';
    var order = '';
    var dlgOptions = {autoOpen: false, draggable: false, modal: true, 'buttons': { '<%=Me.GetLocalizedString("Message.Button.Ok")%>': function() { jQuery(this).dialog('close'); } }};

    jQuery(document).ready(function() {
    
        jQuery('#dlgAjaxError').dialog(dlgOptions);
        jQuery('#dlgSaving').dialog(dlgOptions);

        jQuery('#ulOrder').sortable({ revert: true });

        jQuery('#lnkSave').live('click', function() {
            jQuery('#dlgSaving').dialog('open');
        
            order = '';
            jQuery('#ulOrder li').each(function() {
                order = order + jQuery(this).attr('id') + ',';
            });

            jQuery.ajax({ url: orderUrl + '?mid=' + encodeURIComponent(<%=Me.ModuleId.ToString%>) + '&order=' + encodeURIComponent(order), cache: false, type: 'POST', dataType: 'text', async: false, success: onSuccessCallback, error: onErrorCallback });
            
            return false;
        });
        
        jQuery('#lnkCancel').live('click', function(){ document.location = returnUrl; });

    });
    
    function onSuccessCallback(data, textStatus, XMLHttpRequest){ document.location = returnUrl; }
    
    function onErrorCallback(XMLHttpRequest, textStatus, errorThrown) { jQuery('#dlgSaving').dialog('close'); jQuery('#dlgAjaxError').dialog('open'); }
/*]]>*/</script>