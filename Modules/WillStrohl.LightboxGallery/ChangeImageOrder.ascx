<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ChangeImageOrder.ascx.vb" Inherits="WillStrohl.Modules.Lightbox.ChangeImageOrderView" %>
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
    var dlgOptions = { autoOpen: false, draggable: false, modal: true, "buttons": { "<%=Me.GetLocalizedString("Message.Button.Ok")%>": function () { $(this).dialog("close"); } }};

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

        $("#ulOrderImages").sortable({ revert: true });

        $("#lnkSave").on("click", function() {
            $("#dlgSaving").dialog("open");
        
            order = "";
            jQuery("#ulOrderImages li").each(function() {
                order = order + jQuery(this).attr("id") + ",";
            });

            $app.Model = {
                Album: <%=Me.LightboxId.ToString%>,
                Order: order
            };

            $.ajax({
                url: $app.ServicePath + "ImageReorder",
                type: "POST",
                contentType: "application/json",
                dataType: "json",
                beforeSend: _sf.setModuleHeaders,
                data: JSON.stringify($app.Model),
                success: function (data, textStatus, jqXHR) {
                    onSuccessCallback(data, textStatus, jqXHR);
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    onErrorCallback(jqXHR, textStatus, errorThrown);
                }
            });
        });
        
        $("#lnkCancel").on("click", function(){ document.location = returnUrl; });

    });
    
    function onSuccessCallback(data, textStatus, jqXHR){ document.location = returnUrl; }
    
    function onErrorCallback(jqXHR, textStatus, errorThrown) { $("#dlgSaving").dialog("close"); $("#dlgAjaxError").dialog("open"); }
/*]]>*/</script>