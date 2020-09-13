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
    var moduleId = <%=ModuleId%>;
    var moduleName = "WillStrohl.LightboxGallery";
    var controller = "LightboxSvc";
    var _sf = {};
    var $app = {};

    var returnUrl = "<%=NavigateUrl%>";
    var order = "";
    var dlgOptions = {autoOpen: false, draggable: false, modal: true, "buttons": { "<%=Me.GetLocalizedString("Message.Button.Ok")%>": function() { $(this).dialog("close"); } }};

    $(document).ready(function () {
        if ($.ServicesFramework) {
            _sf = $.ServicesFramework(moduleId);
            $app.ServiceRoot = _sf.getServiceRoot(moduleName);
            $app.ServicePath = $app.ServiceRoot + controller + "/";
            $app.Headers = {
                "ModuleId": moduleId,
                "TabId": _sf.getTabId(),
                "RequestVerificationToken": _sf.getAntiForgeryValue()
            };
            $app.Model = {};
        }
    
        $("#dlgAjaxError").dialog(dlgOptions);
        $("#dlgSaving").dialog(dlgOptions);

        $("#ulOrder").sortable({ revert: true });

        $("#lnkSave").on("click", function() {
            $("#dlgSaving").dialog("open");
        
            order = "";
            $("#ulOrder li").each(function() {
                order = order + $(this).attr("id") + ",";
            });

            $app.Model = order;

            $.ajax({
                url: $app.ServicePath + "AlbumReorder",
                type: "POST",
                contentType: "application/json",
                dataType: "json",
                beforeSend: _sf.setModuleHeaders,
                data: JSON.stringify($app.Model),
                success: function (data, textStatus, jqXHR) {
                    console.log(textStatus);
                    onSuccessCallback(data, textStatus, jqXHR);
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    console.log(textStatus);
                    console.log($.parseJSON(jqXHR.responseText));
                    onErrorCallback(jqXHR, textStatus, errorThrown);
                }
            });
            
            return false;
        });
        
        $('#lnkCancel').on('click', function(){ document.location = returnUrl; });
    });
    
    function onSuccessCallback(data, textStatus, XMLHttpRequest){ document.location = returnUrl; }
    
    function onErrorCallback(XMLHttpRequest, textStatus, errorThrown) { $('#dlgSaving').dialog('close'); $('#dlgAjaxError').dialog('open'); }
/*]]>*/</script>