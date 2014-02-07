<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ViewLightbox.ascx.vb" Inherits="WillStrohl.Modules.Lightbox.ViewLightbox" %>
<div id="wns_Module_AlbumWrapper" class="dnnClear">
    <asp:Repeater ID="rptGallery" runat="server">
        <ItemTemplate>
            <div class="wns_lightbox_wrapper wns-showlater">
                <a class="EditAlbumLink" href='<%#GetEditUrl(DataBinder.Eval(Container.DataItem, "LightboxId"))%>'><img src="" alt='<%=Me.GetLocalizedString("EditImage.Alt")%>' title='<%=Me.GetLocalizedString("EditImage.Alt")%>' class="wns-image EditImage" /></a>
                <a class="ReorderLink" href='<%#GetReorderUrl(DataBinder.Eval(Container.DataItem, "LightboxId"))%>'><img src="" alt='<%=Me.GetLocalizedString("OrderImage.Alt")%>' title='<%=Me.GetLocalizedString("OrderImage.Alt")%>' class="wns-image OrderImage" /></a>
                <h3 id="<%# String.Format("h3-{0}-{1}", TabModuleId, DataBinder.Eval(Container.DataItem, "LightboxId")) %>" class="wns_lightbox_head"><%#Server.HtmlDecode(DataBinder.Eval(Container.DataItem, "GalleryName"))%></h3>
                <p id="<%# String.Format("p-{0}-{1}", TabModuleId, DataBinder.Eval(Container.DataItem, "LightboxId")) %>" class="Normal wns_lightbox_description"><%#Server.HtmlDecode(DataBinder.Eval(Container.DataItem, "GalleryDescription").ToString)%></p>
                <div class="Normal"><%#Me.GetImageFiles(DataBinder.Eval(Container.DataItem, "LightboxId"), DataBinder.Eval(Container.DataItem, "GalleryFolder"), DataBinder.Eval(Container.DataItem, "GalleryName"))%></div>
            </div>
        </ItemTemplate>
    </asp:Repeater>
</div>
<script language="javascript" type="text/javascript">/*<![CDATA[*/
    var IsEditable = <%=Me.IsEditable.ToString.ToLower%>;

    jQuery(document).ready( function() {
        jQuery('.EditImage')
            .attr('src', '<%=Me.EditImage%>')
            .attr('align', 'left');
        jQuery('.OrderImage')
            .attr('src', '<%=Me.OrderImage%>')
            .attr('align', 'left');
        if(!IsEditable) { jQuery('.EditImage').parent().remove().end().remove(); jQuery('.OrderImage').parent().remove().end().remove(); }

        setTimeout(function (){ jQuery('.wns-showlater').show(); }, 250);
    } );
/*]]>*/</script>
<asp:PlaceHolder ID="phScript" runat="server" />