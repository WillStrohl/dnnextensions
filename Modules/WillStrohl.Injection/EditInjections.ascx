<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="EditInjections.ascx.vb" Inherits="WillStrohl.Modules.Injection.EditInjections" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register TagPrefix="dnnweb" Assembly="DotNetNuke.Web" Namespace="DotNetNuke.Web.UI.WebControls" %>
<asp:Panel ID="pnlAddNew" runat="server" CssClass="dnnClear">
    <asp:HiddenField ID="hidInjectionId" runat="server" />
    <div class="dnnClear">
        <asp:ValidationSummary ID="vsAddNew" runat="server" DisplayMode="BulletList" ValidationGroup="AddNew" CssClass="dnnFormMessage dnnFormValidationSummary" />
    </div>
    <div id="dnnEditEntry" class="dnnEditEntry dnnForm dnnClear">
        <h2 id="dnnPanel-h2Injection" class="dnnFormSectionHead"><a href="" class="dnnLabelExpanded"><%= Me.GetLocalizedString("Injection.Text")%></a></h2>
        <fieldset id="fsInjection">
            <div class="dnnFormItem">
                <dnn:Label ID="lblName" runat="server" ResourceKey="lblName" ControlName="txtName" Suffix=":" />
                <asp:TextBox ID="txtName" runat="server" MaxLength="50" CssClass="NormalTextBox dnnFormRequired" ValidationGroup="AddNew" /> 
                    <asp:RequiredFieldValidator ID="rfvName" runat="server" ControlToValidate="txtName" Display="Dynamic" CssClass="dnnFormMessage dnnFormError" ValidationGroup="AddNew" /> 
                    <asp:CustomValidator ID="cvName" runat="server" ControlToValidate="txtName" Display="Dynamic" CssClass="dnnFormMessage dnnFormError" ValidationGroup="AddNew" />
            </div>
            <div class="dnnFormItem">
                <dnn:Label ID="lblInject" runat="server" ResourceKey="lblInject" ControlName="radInject" Suffix=":" />
                <div class="dnnLeft dnnFormRadioButtons">
                    <asp:RadioButtonList ID="radInject" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow" ValidationGroup="AddNew" />
                </div>
            </div>
            <div class="dnnFormItem">
                <dnn:Label ID="lblContent" runat="server" ResourceKey="lblContent" ControlName="txtContent" Suffix=":" />
                <asp:TextBox ID="txtContent" runat="server" TextMode="MultiLine" Rows="6" Columns="10" CssClass="NormalTextBox dnnFormRequired wnsInjectionContent" ValidationGroup="AddNew" /> 
                    <asp:RequiredFieldValidator ID="rfvContent" runat="server" ControlToValidate="txtContent" Display="Dynamic" CssClass="dnnFormMessage dnnFormError" ValidationGroup="AddNew" />
            </div>
            <div class="dnnFormItem">
                <dnn:Label ID="lblEnabled" runat="server" ResourceKey="lblEnabled" ControlName="chkEnabled" Suffix=":" />
                <div class="dnnLeft">
                    <asp:Checkbox ID="chkEnabled" runat="server" ValidationGroup="AddNew" />
                </div>
            </div>
        </fieldset>
        <div class="dnnFormItem">
            <asp:LinkButton ID="cmdAdd" runat="server" CssClass="dnnPrimaryAction" ValidationGroup="AddNew" />&nbsp; 
            <asp:LinkButton ID="cmdDelete" runat="server" CausesValidation="false" CssClass="dnnSecondaryAction wns-DeleteLink" />&nbsp; 
            <asp:LinkButton ID="cmdCancel" runat="server" CausesValidation="false" CssClass="dnnSecondaryAction" />
        </div>
    </div>
</asp:Panel>
<asp:Panel ID="pnlManage" runat="server" CssClass="dnnClear">
    <asp:Label ID="lblNoRecords" runat="server" CssClass="NormalRed" />
    <asp:DataList ID="dlInjection" runat="server" DataKeyField="InjectionId" CssClass="wns_inj_fullwidth">
        <HeaderTemplate>
            <table cellspacing="0" cellpadding="0" class="dnnGrid wns_inj_injectiontable wns_inj_border" summary="WillStrohl Content Injection Module Management Table Header">
                <tr class="wns_inj_header">
                    <td class="dnnGridHeader wns_inj_col_edit wns_inj_borderbottom"><%=Me.GetLocalizedString("dlInjection.Header.Edit")%></td>
                    <td class="dnnGridHeader wns_inj_col_editlarge wns_inj_borderbottom"><%=Me.GetLocalizedString("dlInjection.Header.InjectTop")%></td>
                    <td class="dnnGridHeader wns_inj_col_editlarge wns_inj_borderbottom"><%=Me.GetLocalizedString("dlInjection.Header.Enabled")%></td>
                    <td class="dnnGridHeader wns_inj_borderbottom"><%=Me.GetLocalizedString("dlInjection.Header.Name")%></td>
                    <td class="dnnGridHeader wns_inj_col_editmove wns_inj_borderbottom"><%=Me.GetLocalizedString("dlInjection.Header.Move")%></td>
                    <td class="dnnGridHeader wns_inj_col_editlarge wns_inj_borderbottom"><%=Me.GetLocalizedString("dlInjection.Header.Delete")%></td>
                </tr>
        </HeaderTemplate>
        <ItemTemplate>
            <tr>
                <td class="dnnGridItem wns_inj_col_edit">
                    <asp:ImageButton ID="imgEdit" runat="server" ImageUrl="~/Icons/Sigma/Edit_16X16_Standard.png" CommandName="Edit" CommandArgument='<%#Eval("InjectionId")%>' CausesValidation="false" />
                </td>
                <td class="dnnGridItem wns_inj_col_editlarge"><asp:label ID="lblTop" runat="server" Text='<%#Eval("InjectTop").ToString%>' /></td>
                <td class="dnnGridItem wns_inj_col_editlarge"><img src='<%#Me.GetEnabledImage(Eval("IsEnabled"))%>' alt='<%#Me.GetEnabledImageAltText(Eval("IsEnabled"))%>' /></td>
                <td class="dnnGridItem"><asp:label ID="lblName" runat="server" Text='<%#Eval("InjectName")%>' /></td>
                <td class="dnnGridItem wns_inj_col_editmove">
                    <div class="dnnLeft wns_inj_left wns_inj_col_edit"><asp:ImageButton ID="imgMoveUp" runat="server" ImageUrl="~/Icons/Sigma/Up_16X16_Standard.png" CommandName="MoveUp" CommandArgument='<%#Eval("InjectionId")%>' CausesValidation="false" Visible='<%#Me.CommandUpVisible(Eval("InjectionId"))%>' /></div> 
                    <div class="dnnLeft wns_inj_right wns_inj_col_edit"><asp:ImageButton ID="imgMoveDown" runat="server" ImageUrl="~/Icons/Sigma/Dn_16X16_Standard.png" CommandName="MoveDown" CommandArgument='<%#Eval("InjectionId")%>' CausesValidation="false" Visible='<%#Me.CommandDownVisible(Eval("InjectionId"))%>' /></div>
                </td>
                <td class="dnnGridItem wns_inj_col_editlarge">
                    <asp:ImageButton ID="imgDelete" runat="server" ImageUrl="~/Icons/Sigma/Delete_16X16_Standard.png" CommandName="Delete" CommandArgument='<%#Eval("InjectionId")%>' CssClass="wns-DeleteImage" CausesValidation="false" />
                </td>
            </tr>
        </ItemTemplate>
        <AlternatingItemTemplate>
            <tr>
                <td class="dnnGridAltItem wns_inj_col_edit">
                    <asp:ImageButton ID="imgEdit" runat="server" ImageUrl="~/Icons/Sigma/Edit_16X16_Standard.png" CommandName="Edit" CommandArgument='<%#Eval("InjectionId")%>' CausesValidation="false" />
                </td>
                <td class="dnnGridAltItem wns_inj_col_editlarge"><asp:label ID="lblTop" runat="server" Text='<%#Eval("InjectTop").ToString%>' /></td>
                <td class="dnnGridAltItem wns_inj_col_editlarge"><img src='<%#Me.GetEnabledImage(Eval("IsEnabled"))%>' alt='<%#Me.GetEnabledImageAltText(Eval("IsEnabled"))%>' /></td>
                <td class="dnnGridAltItem"><asp:label ID="lblName" runat="server" Text='<%#Eval("InjectName")%>' /></td>
                <td class="dnnGridAltItem wns_inj_col_editmove">
                    <div class="dnnLeft wns_inj_left wns_inj_col_edit"><asp:ImageButton ID="imgMoveUp" runat="server" ImageUrl="~/Icons/Sigma/Up_16X16_Standard.png" CommandName="MoveUp" CommandArgument='<%#Eval("InjectionId")%>' CausesValidation="false" Visible='<%#Me.CommandUpVisible(Eval("InjectionId"))%>' /></div> 
                    <div class="dnnLeft wns_inj_right wns_inj_col_edit"><asp:ImageButton ID="imgMoveDown" runat="server" ImageUrl="~/Icons/Sigma/Dn_16X16_Standard.png" CommandName="MoveDown" CommandArgument='<%#Eval("InjectionId")%>' CausesValidation="false" Visible='<%#Me.CommandDownVisible(Eval("InjectionId"))%>' /></div>
                </td>
                <td class="dnnGridAltItem wns_inj_col_editlarge">
                    <asp:ImageButton ID="imgDelete" runat="server" ImageUrl="~/Icons/Sigma/Delete_16X16_Standard.png" CommandName="Delete" CommandArgument='<%#Eval("InjectionId")%>' CausesValidation="false" />
                </td>
            </tr>
        </AlternatingItemTemplate>
        <FooterTemplate>
            </table>
        </FooterTemplate>
    </asp:DataList>
    <div id="divCommand" class="dnnClear">
        <asp:LinkButton ID="lnkAddNewInjection" runat="server" CommandName="AddNew" CssClass="dnnPrimaryAction" CausesValidation="false" />&nbsp; 
        <asp:LinkButton ID="cmdReturn" runat="server" CausesValidation="false" CssClass="dnnSecondaryAction" />
    </div>
</asp:Panel>
<script language="javascript" type="text/javascript">/*<![CDATA[*/
    var delText = '<%=Me.GetLocalizedString("Delete.Confirm")%>';

    function confirmDeleteInjection() { return confirm(delText); }

    (function ($, Sys) {
        function setupDnnSiteSettings() {
            $('#dnnEditEntry').dnnPanels();

            jQuery('input[src$=\'delete.gif\'], .wns-DeleteImage, .wns-DeleteLink').live('click', function() {
                return confirm('<%=Me.GetLocalizedString("Delete.Confirm")%>');
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