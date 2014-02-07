<%@ Control language="C#" Inherits="DotNetNuke.Modules.WillStrohlDisqus.View" AutoEventWireup="false"  Codebehind="View.ascx.cs" %>
<asp:PlaceHolder ID="phScript" runat="server" />
<div id="divNoLogin" class="wns-clear dnnClear" runat="server" Visible="False">
    <p class="wns-login SubHead"><%=LoginMessage()%></p>
</div>
<div ID="divComments" runat="server" class="wns-disqus-comments wns-clear dnnClear" Visible="False"></div>