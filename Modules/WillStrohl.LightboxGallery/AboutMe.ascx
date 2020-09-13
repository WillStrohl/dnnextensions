<%@ Control Language="VB" AutoEventWireup="true" CodeBehind="AboutMe.ascx.vb" Inherits="WillStrohl.Modules.Lightbox.AboutMe" %>

<div class="dnnClear" style="margin: 0 auto; width: 800px; max-width: 800px;">
    <h1><%=GetLocalizedString("About") %></h1>
    <p><%=GetLocalizedString("ThankYou") %></p>
    <div class="dnnClear" style="width: 50%; margin: 0 auto;">
        <div class="dnnLeft">
            <p class="DNNAligncenter">
                <a href="https://github.com/hismightiness/dnnextensions/wiki" class="dnnSecondaryAction" target="_blank"><%=GetLocalizedString("Help") %></a>
            </p>
        </div>
        <div class="dnnRight">
            <p class="DNNAligncenter">
                <a href="https://github.com/sponsors/hismightiness" target="_blank" class="dnnPrimaryAction">
                    <%=GetLocalizedString("SponsorMe") %>
                </a>
            </p>
        </div>
    </div>
</div>