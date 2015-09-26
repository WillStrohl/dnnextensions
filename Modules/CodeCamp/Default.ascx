<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Default.ascx.cs" Inherits="WillStrohl.Modules.CodeCamp.Default" %>
<script type="text/javascript">
    var moduleId = <%=ModuleId%>;
    var moduleName = "<%=ModuleConfiguration.DesktopModule.FolderName%>";
    var templatePath = "<%=ControlPath%>";
</script>
<asp:PlaceHolder runat="server" ID="plOutput"/>