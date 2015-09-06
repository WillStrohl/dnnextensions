<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EditInjections.ascx.cs" Inherits="WillStrohl.Modules.Injection.EditInjections" %>
<%@ Import Namespace="DotNetNuke.Services.Localization" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<asp:Panel ID="pnlAddNew" runat="server" CssClass="dnnClear">
    <asp:HiddenField ID="hidInjectionId" runat="server" />
    <div class="dnnClear">
        <asp:ValidationSummary ID="vsAddNew" runat="server" DisplayMode="BulletList" ValidationGroup="AddNew" CssClass="dnnFormMessage dnnFormValidationSummary" />
    </div>
    <div id="dnnEditEntry" class="dnnEditEntry dnnForm dnnClear">
        <h2 id="dnnPanel-h2Injection" class="dnnFormSectionHead"><a href="" class="dnnLabelExpanded"><%= GetLocalizedString("Injection.Text")%></a></h2>
        <fieldset id="fsInjection">
            <div class="dnnFormItem">
                <dnn:Label ID="lblName" runat="server" ResourceKey="lblName" ControlName="txtName" Suffix=":" />
                <asp:TextBox ID="txtName" runat="server" MaxLength="50" CssClass="NormalTextBox dnnFormRequired" ValidationGroup="AddNew" /> 
                    <asp:RequiredFieldValidator ID="rfvName" runat="server" ControlToValidate="txtName" Display="Dynamic" CssClass="dnnFormMessage dnnFormError" ValidationGroup="AddNew" /> 
                    <asp:CustomValidator ID="cvName" runat="server" ControlToValidate="txtName" Display="Dynamic" CssClass="dnnFormMessage dnnFormError" ValidationGroup="AddNew" />
            </div>
            <div class="dnnFormItem">
                <dnn:Label id="lblType" runat="server" ResourceKey="lblType" controlName="radType" suffix=":"/>
                <div class="dnnLeft dnnFormRadioButtons">
                    <asp:RadioButtonList ID="radType" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow" ValidationGroup="AddNew" OnSelectedIndexChanged="radType_OnSelectedIndexChanged" AutoPostBack="True" CausesValidation="False" />
                </div>
            </div>
            <div class="dnnFormItem">
                <dnn:Label ID="lblEnabled" runat="server" ResourceKey="lblEnabled" ControlName="chkEnabled" Suffix=":" />
                <div class="dnnLeft">
                    <asp:Checkbox ID="chkEnabled" runat="server" ValidationGroup="AddNew" />
                </div>
            </div>
            <div class="dnnFormItem">
                <dnn:Label ID="lblContent" runat="server" ResourceKey="lblContent" ControlName="txtContent" Suffix=":" />
                <asp:TextBox ID="txtContent" runat="server" TextMode="MultiLine" Rows="6" Columns="10" CssClass="NormalTextBox dnnFormRequired wnsInjectionContent" ValidationGroup="AddNew" /> 
                <asp:RequiredFieldValidator ID="rfvContent" runat="server" ControlToValidate="txtContent" Display="Dynamic" CssClass="dnnFormMessage dnnFormError" ValidationGroup="AddNew" />
                <asp:CustomValidator ID="cvContent" runat="server" ControlToValidate="txtContent" Display="Dynamic" CssClass="dnnFormMessage dnnFormError" ValidationGroup="AddNew" OnServerValidate="cvContent_OnServerValidate"/>
            </div>
            <div class="dnnFormItem" runat="server" id="divInject">
                <dnn:Label ID="lblInject" runat="server" ResourceKey="lblInject" ControlName="radInject" Suffix=":" />
                <div class="dnnLeft dnnFormRadioButtons">
                    <asp:RadioButtonList ID="radInject" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow" ValidationGroup="AddNew" />
                </div>
            </div>
        </fieldset>
    </div>
    <div id="divAdvanced" class="dnnEditEntry dnnForm dnnClear" runat="server">
        <h2 id="dnnPanel-h2Advanced" class="dnnFormSectionHead"><a href="" class=""><%= GetLocalizedString("Advanced")%></a></h2>
        <fieldset id="fsAdvanced">
            <div class="dnnFormItem">
                <dnn:Label id="lblCrmPriority" runat="server" ResourceKey="lblCrmPriority" ControlName="txtCrmPriority" Suffix=":"/>
                <asp:textbox id="txtCrmPriority" runat="server" ValidationGroup="AddNew"/>
                <asp:RangeValidator ID="rvCrmPriority" runat="server" Type="Integer" MinimumValue="0" MaximumValue="1000" ControlToValidate="txtCrmPriority" Display="Dynamic" CssClass="dnnFormMessage dnnFormError" ValidationGroup="AddNew" />
            </div>
            <div class="dnnFormItem">
                <div class="dnnRight wnsFormInfoMessage">
                    <div class="dnnFormMessage dnnFormInfo"><%=GetLocalizedString("PriorityInfo") %></div>
                </div>
            </div>
            <div class="dnnFormItem">
                <dnn:Label id="lblCrmProvider" runat="server" ResourceKey="lblCrmProvider" ControlName="ddlCrmProvider" Suffix=":"/>
                <asp:dropdownlist ID="ddlCrmProvider" runat="server"/>
            </div>
        </fieldset>
    </div>
    <ul class="wnsActions">
        <li><asp:LinkButton ID="cmdAdd" runat="server" CssClass="dnnPrimaryAction" ValidationGroup="AddNew" OnClick="cmdAdd_Click" /></li>
        <li><asp:LinkButton ID="cmdDelete" runat="server" CausesValidation="false" CssClass="dnnSecondaryAction wns-DeleteLink" OnClick="cmdDelete_Click" /></li>
        <li><asp:LinkButton ID="cmdCancel" runat="server" CausesValidation="false" CssClass="dnnSecondaryAction" OnClick="cmdCancel_Click" /></li>
    </ul>
</asp:Panel>
<asp:Panel ID="pnlManage" runat="server" CssClass="dnnClear">
    <div runat="server" ID="lblNoRecords" runat="server" CssClass="dnnFormMessage dnnFormInfo">
        <%=GetLocalizedString("lblNoRecords.Text")%>
    </div>
    <asp:DataList ID="dlInjection" runat="server" DataKeyField="InjectionId" CssClass="wns_inj_fullwidth" OnItemCommand="dlInjection_ItemCommand">
        <HeaderTemplate>
            <table cellspacing="0" cellpadding="0" class="dnnGrid wns_inj_injectiontable wns_inj_border" summary="WillStrohl Content Injection Module Management Table Header">
                <tr class="dnnGridHeader wns_inj_header">
                    <td class="wns_inj_col_edit wns_inj_borderbottom"><%=GetLocalizedString("dlInjection.Header.Edit")%></td>
                    <td class="wns_inj_borderbottom"><%=GetLocalizedString("dlInjection.Header.Name")%></td>
                    <td class="wns_inj_col_editlarger wns_inj_borderbottom"><%=GetLocalizedString("dlInjection.Header.InjectType")%></td>
                    <td class="wns_inj_col_editlarge wns_inj_borderbottom"><%=GetLocalizedString("dlInjection.Header.Enabled")%></td>
                    <td class="wns_inj_col_editmove wns_inj_borderbottom"><%=GetLocalizedString("dlInjection.Header.Move")%></td>
                </tr>
        </HeaderTemplate>
        <ItemTemplate>
            <tr class="dnnGridItem">
                <td class="wns_inj_col_edit">
                    <asp:ImageButton ID="imgEdit" runat="server" ImageUrl="~/Icons/Sigma/Edit_16X16_Standard.png" CommandName="Edit" CommandArgument='<%#Eval("InjectionId")%>' CausesValidation="false" />
                </td>
                <td><asp:label ID="lblName" runat="server" Text='<%#Eval("InjectName")%>' /></td>
                <td class="wns_inj_col_editlarger"><asp:label ID="lblTop" runat="server" Text='<%#GetInjectionTypeForDisplay(Eval("InjectionId"))%>' /></td>
                <td class="wns_inj_col_editlarge"><img src='<%#GetEnabledImage(Eval("IsEnabled"))%>' alt='<%#GetEnabledImageAltText(Eval("IsEnabled"))%>' /></td>
                <td class="wns_inj_col_editmove">
                    <div class="dnnLeft wns_inj_left wns_inj_col_edit"><asp:ImageButton ID="imgMoveUp" runat="server" ImageUrl="~/Icons/Sigma/Up_16X16_Standard.png" CommandName="MoveUp" CommandArgument='<%#Eval("InjectionId")%>' CausesValidation="false" Visible='<%#CommandUpVisible(Eval("InjectionId"))%>' /></div> 
                    <div class="dnnLeft wns_inj_right wns_inj_col_edit"><asp:ImageButton ID="imgMoveDown" runat="server" ImageUrl="~/Icons/Sigma/Dn_16X16_Standard.png" CommandName="MoveDown" CommandArgument='<%#Eval("InjectionId")%>' CausesValidation="false" Visible='<%#CommandDownVisible(Eval("InjectionId"))%>' /></div>
                </td>
            </tr>
        </ItemTemplate>
        <AlternatingItemTemplate>
            <tr class="dnnGridAltItem">
                <td class="wns_inj_col_edit">
                    <asp:ImageButton ID="imgEdit" runat="server" ImageUrl="~/Icons/Sigma/Edit_16X16_Standard.png" CommandName="Edit" CommandArgument='<%#Eval("InjectionId")%>' CausesValidation="false" />
                </td>
                <td><asp:label ID="lblName" runat="server" Text='<%#Eval("InjectName")%>' /></td>
                <td class="wns_inj_col_editlarger"><asp:label ID="lblTop" runat="server" Text='<%#GetInjectionTypeForDisplay(Eval("InjectionId"))%>' /></td>
                <td class="wns_inj_col_editlarge"><img src='<%#GetEnabledImage(Eval("IsEnabled"))%>' alt='<%#GetEnabledImageAltText(Eval("IsEnabled"))%>' /></td>
                <td class="wns_inj_col_editmove">
                    <div class="dnnLeft wns_inj_left wns_inj_col_edit"><asp:ImageButton ID="imgMoveUp" runat="server" ImageUrl="~/Icons/Sigma/Up_16X16_Standard.png" CommandName="MoveUp" CommandArgument='<%#Eval("InjectionId")%>' CausesValidation="false" Visible='<%#CommandUpVisible(Eval("InjectionId"))%>' /></div> 
                    <div class="dnnLeft wns_inj_right wns_inj_col_edit"><asp:ImageButton ID="imgMoveDown" runat="server" ImageUrl="~/Icons/Sigma/Dn_16X16_Standard.png" CommandName="MoveDown" CommandArgument='<%#Eval("InjectionId")%>' CausesValidation="false" Visible='<%#CommandDownVisible(Eval("InjectionId"))%>' /></div>
                </td>
            </tr>
        </AlternatingItemTemplate>
        <FooterTemplate>
            </table>
        </FooterTemplate>
    </asp:DataList>
    <div id="divCommand" class="dnnClear">
        <asp:LinkButton ID="lnkAddNewInjection" runat="server" CommandName="AddNew" CssClass="dnnPrimaryAction" CausesValidation="false" OnClick="lnkAddNewInjection_Click" />&nbsp; 
        <asp:LinkButton ID="cmdReturn" runat="server" CausesValidation="false" CssClass="dnnSecondaryAction" OnClick="cmdReturn_Click" />
    </div>
</asp:Panel>
<script language="javascript" type="text/javascript">/*<![CDATA[*/
    (function ($, Sys) {
        function setupDnnSiteSettings() {
            $(".dnnEditEntry").dnnPanels();

            $(".wns-DeleteLink").click(function () {
                $(this).dnnConfirm({
                    text: "<%= GetLocalizedString("Delete.Confirm")%>",
                    yesText: "<%= Localization.GetString("Yes.Text", Localization.SharedResourceFile) %>",
                    noText: "<%= Localization.GetString("No.Text", Localization.SharedResourceFile) %>",
                    title: "<%= Localization.GetString("Confirm.Text", Localization.SharedResourceFile) %>",
                    isButton: true
                });
            });
        }

        $(document).ready(function () {
            setupDnnSiteSettings();
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
                setupDnnSiteSettings();
            });
        });
    } (jQuery, window.Sys));
/*]]>*/</script>