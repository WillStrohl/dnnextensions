<%@ Control language="vb" Inherits="WillStrohl.Modules.ContactCollector.ViewContactCollector" CodeBehind="ViewContactCollector.ascx.vb" AutoEventWireup="false" Explicit="True" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.UI.WebControls" Assembly="DotNetNuke.Web" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.UI.WebControls" Assembly="DotNetNuke" %>
<div class="cc_wrapper">
    <asp:ValidationSummary ID="vsContact" runat="server" CssClass="dnnFormMessage dnnFormValidationSummary" DisplayMode="List" ValidationGroup="contact" />
</div>
<div id="dnnEditEntry" class="dnnForm dnnEditEntry dnnClear">
    <div class="dnnFormItem">
        <dnn:Label ID="lblFirstName" runat="server" ControlName="txtFirstName" ResourceKey="lblFirstName" Suffix=":" />
        <asp:TextBox ID="txtFirstName" runat="server" MaxLength="50" CssClass="NormalTextBox dnnFormRequired" ValidationGroup="contact" /> 
        <asp:RequiredFieldValidator ID="rfvFirstName" runat="server" ControlToValidate="txtFirstName" Display="Dynamic" CssClass="dnnFormMessage dnnFormError" ValidationGroup="contact" /> 
    </div>
    <div class="dnnFormItem">
        <dnn:Label ID="lblLastName" runat="server" ControlName="txtLastName" ResourceKey="lblLastName" Suffix=":" />
        <asp:TextBox ID="txtLastName" runat="server" MaxLength="50" CssClass="NormalTextBox dnnFormRequired" ValidationGroup="contact" /> 
        <asp:RequiredFieldValidator ID="rfvLastName" runat="server" ControlToValidate="txtLastName" Display="Dynamic" CssClass="dnnFormMessage dnnFormError" ValidationGroup="contact" /> 
    </div>
    <div class="dnnFormItem">
        <dnn:Label ID="lblEmailAddress" runat="server" ControlName="txtEmailAddress" ResourceKey="lblEmailAddress" Suffix=":" />
        <asp:TextBox ID="txtEmailAddress" runat="server" MaxLength="255" CssClass="NormalTextBox dnnFormRequired" ValidationGroup="contact" /> 
        <asp:RequiredFieldValidator ID="rfvEmailAddress" runat="server" ControlToValidate="txtEmailAddress" Display="Dynamic" CssClass="dnnFormMessage dnnFormError" ValidationGroup="contact" /> 
        <asp:RegularExpressionValidator ID="revEmailAddress" runat="server" ControlToValidate="txtEmailAddress" Display="Dynamic" 
            ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" CssClass="dnnFormMessage dnnFormError" ValidationGroup="contact" />
    </div>
    <div class="dnnFormItem">
        <dnn:Label ID="lblComment" runat="server" ControlName="txtComment" ResourceKey="lblComment" Suffix=":" />
        <asp:TextBox ID="txtComment" runat="server" CssClass="NormalTextBox dnnFormRequired" Columns="10" MaxLength="500" Rows="5" TextMode="MultiLine" ValidationGroup="contact" />
        <asp:RequiredFieldValidator ID="rfvComment" runat="server" ControlToValidate="txtComment" Display="Dynamic" CssClass="dnnFormMessage dnnFormError" ValidationGroup="contact" /> 
    </div>
    <div class="dnnFormItem">
        <div id="divCaptchaWrapper" class="dnnLeft">
            <dnn:CaptchaControl ID="ctlCaptcha" CaptchaHeight="40" CaptchaWidth="150" ErrorStyle-CssClass="NormalRed" cssclass="Normal" runat="server" />
        </div>
    </div>
    <div class="dnnFormItem">
        <asp:LinkButton ID="cmdSubmit" runat="server" CssClass="dnnPrimaryAction" CausesValidation="true" ValidationGroup="contact" />
    </div>
</div>