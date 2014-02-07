<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="Settings.ascx.vb" Inherits="WillStrohl.Modules.ContactCollector.Settings" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register TagPrefix="dnn" TagName="TextBox" Src="~/controls/TextEditor.ascx" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.UI.WebControls" Assembly="DotNetNuke.Web" %>
<%@ Import Namespace="DotNetNuke.Services.Localization" %>
<div id="dnnContactSettings" class="dnnForm dnnEditEntry dnnClear">
    <h2 id="dnnPanel-h2Basic" class="dnnFormSectionHead"><a href="" class="dnnLabelExpanded"><%= Localization.GetString("BasicSettings.Text", Me.LocalResourceFile)%></a></h2>
    <fieldset id="fsBasicSettings">
        <div class="dnnFormItem">
            <dnn:Label ID="lblRequiredFields" runat="server" ResourceKey="lblRequiredFields" Suffix=":" />
            <div id="divRequiredFields" class="dnnLeft">
                <asp:CheckBoxList ID="chkRequiredFields" runat="server" RepeatColumns="1" RepeatDirection="Vertical" RepeatLayout="Flow" />
            </div>
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="lblComment" runat="server" ResourceKey="lblComment" ControlName="chkComment" Suffix=":" />
            <asp:CheckBox ID="chkComment" runat="server" CssClass="Normal" />
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="lblUseCaptcha" runat="server" ResourceKey="lblUseCaptcha" ControlName="chkUseCaptcha" Suffix=":" />
            <asp:CheckBox ID="chkUseCaptcha" runat="server" CssClass="Normal" />
        </div>
    </fieldset>
    <h2 id="dnnPanel-h2Email" class="dnnFormSectionHead"><a href="" class=""><%= Localization.GetString("EmailSettings.Text", Me.LocalResourceFile)%></a></h2>
    <fieldset id="fsEmailSettings">
        <div class="dnnFormItem">
            <dnn:Label ID="lblSendEmailToContact" runat="server" ResourceKey="lblSendEmailToContact" ControlName="chkSendEmailToContact" Suffix=":" />
            <asp:CheckBox ID="chkSendEmailToContact" runat="server" CssClass="Normal" />
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="lblEmailSubjectToContact" runat="server" ResourceKey="lblEmailSubjectToContact" ControlName="txtEmailSubjectToContact" Suffix=":" />
            <asp:TextBox ID="txtEmailSubjectToContact" runat="server" MaxLength="50" />
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="lblEmailMessageToContact" runat="server" ResourceKey="lblEmailMessageToContact" ControlName="txtEmailMessageToContact" Suffix=":" />
            <div class="dnnLeft">
                <dnn:TextBox ID="txtEmailMessageToContact" runat="server" ChooseMode="true" ChooseRender="true" Height="300px" HtmlEncode="true" Width="500px" />
            </div>
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="lblSendEmailToAdmin" runat="server" ResourceKey="lblSendEmailToAdmin" ControlName="chkSendEmailToAdmin" Suffix=":" />
            <asp:CheckBox ID="chkSendEmailToAdmin" runat="server" CssClass="Normal" />
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="lblEmailSubjectToAdmin" runat="server" ResourceKey="lblEmailSubjectToAdmin" ControlName="txtEmailSubjectToAdmin" Suffix=":" />
            <asp:TextBox ID="txtEmailSubjectToAdmin" runat="server" MaxLength="50" Width="400" />
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="lblEmailMessageToAdmin" runat="server" ResourceKey="lblEmailMessageToAdmin" ControlName="txtEmailMessageToAdmin" Suffix=":" />
            <div class="dnnLeft">
                <dnn:TextBox ID="txtEmailMessageToAdmin" runat="server" ChooseMode="true" ChooseRender="true" Height="300px" HtmlEncode="true" Width="500px" />
                <p class="Normal"><%= Localization.GetString("Token.Text", Me.LocalResourceFile)%></p>
            </div>
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="lblAdminEmail" runat="server" ResourceKey="lblAdminEmail" ControlName="txtAdminEmail" Suffix=":" />
            <asp:TextBox ID="txtAdminEmail" runat="server" MaxLength="255" CssClass="NormalTextBox" />
        </div>
    </fieldset>
</div>