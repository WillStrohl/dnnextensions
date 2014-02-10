<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EditMedia.ascx.cs" Inherits="DotNetNuke.Modules.Media.EditMedia" %>
<%@ Register TagPrefix="dnn" TagName="Tracking" Src="~/controls/URLTrackingControl.ascx" %>
<%@ Register TagPrefix="dnn" TagName="Url" Src="~/controls/URLControl.ascx" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register TagPrefix="dnn" TagName="Text" Src="~/controls/TextEditor.ascx" %>
<div id="dnnEditEntry" class="dnnForm dnnEditEntry dnnClear">
    <div class="dnnFormExpandContent"><a href=""><%= Localization.GetString("ExpandAll", Localization.SharedResourceFile) %></a></div>
    <h2 id="dnnPanel-h2FileUpload" class="dnnFormSectionHead"><a href="" class="dnnLabelExpanded"><%= GetLocalizedString("lblFileUpload.Text") %></a></h2>
    <fieldset id="fsFileUpload" class="dnnmedia-fieldset">
        <ol id="olFileUpload" class="dnnmedia-list">
            <li class="dnnmedia-listitem dnnmedia-localfiles">
                <div class="dnnFormItem">
                    <dnn:label id="lblMediaType" runat="server" ResourceKey="plMediaType" ControlName="radMediaType" Suffix=":" />
                    <asp:RadioButtonList ID="radMediaType" runat="server" CssClass="Normal" RepeatColumns="3" RepeatDirection="Horizontal" RepeatLayout="Flow" AutoPostBack="true" /> 
                    <asp:CustomValidator id="cvMediaType" resourcekey="valMediaType.ErrorMessage" runat="server" controltovalidate="radMediaType"
                                         display="Dynamic" cssclass="dnnFormMessage dnnFormError" errormessage="<br />Media Is Required" />
                </div>
            </li>
            <li id="liFileSystem" class="dnnmedia-listitem dnnmedia-localfiles" runat="server">
                <div class="dnnmedia-fieldwrapper">
                    <div class="dnnFormMessage dnnFormWarning"><%= GetLocalizedString("lblUploadWarning.Text") %></div>
                </div>
                <div class="dnnFormItem">
                    <dnn:label id="plURL" runat="server" ResourceKey="plURL" ControlName="ctlUrl" Suffix=":" />
                    <div class="dnnLeft">
                        <dnn:Url id="ctlURL" runat="server" width="300" showtabs="False" showfiles="True" 
                                 showUrls="True" urltype="F" showlog="False" shownewwindow="False" showtrack="False" />
                    </div>
                </div>
                <div class="dnnmedia-fieldwrapper dnnClear">
                    <div id="dnnmedia-filesupport-wrapper">
                        <p class="SubHead"><%= GetLocalizedString("HostFileTypes.Text") %>:<br /> [<a id="lnkViewFileTypes" href="#"><%= GetLocalizedString("lnkViewFileTypes.Text") %></a>]</p>
                        <div id="divSupportedFileTypes" class="dnnmedia-hidden">
                            <p class="Normal"><%= GetLocalizedString("lblSupportedFileTypes.Text") %></p>
                            <asp:Repeater ID="rptMediaFileTypes" runat="server">
                                <HeaderTemplate>
                                    <table id="tblMediaFileTypes" cellpadding="0" cellspacing="0" summary="<%= GetLocalizedString("lblSupportedFileTypes.Text") %>">
                                    <tr class="dnnmedia-filetype-headrow">
                                        <td><%= GetLocalizedString("tblMediaFileTypes.Header.FileType") %></td>
                                        <td><%= GetLocalizedString("tblMediaFileTypes.Header.ModuleSupport") %></td>
                                        <td><%= GetLocalizedString("tblMediaFileTypes.Header.HostSupport") %></td>
                                    </tr>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr class="dnnmedia-filetype-row">
                                        <td class="dnnmedia-filetype-filetype"><%#DataBinder.Eval(Container.DataItem, "FileType") %></td>
                                        <td class="dnnmedia-filetype-modulesupport"><%#GetSupportedImage(DataBinder.Eval(Container.DataItem, "ModuleSupport")) %></td>
                                        <td class="dnnmedia-filetype-hostsupport"><%#GetSupportedImage(DataBinder.Eval(Container.DataItem, "HostSupport")) %></td>
                                    </tr>
                                </ItemTemplate>
                                <AlternatingItemTemplate>
                                    <tr class="dnnmedia-filetype-altrow">
                                        <td class="dnnmedia-filetype-filetype"><%#DataBinder.Eval(Container.DataItem, "FileType") %></td>
                                        <td class="dnnmedia-filetype-modulesupport"><%#GetSupportedImage(DataBinder.Eval(Container.DataItem, "ModuleSupport")) %></td>
                                        <td class="dnnmedia-filetype-hostsupport"><%#GetSupportedImage(DataBinder.Eval(Container.DataItem, "HostSupport")) %></td>
                                    </tr>
                                </AlternatingItemTemplate>
                                <FooterTemplate>
                                </table>
                                </FooterTemplate>
                            </asp:Repeater>
                            <p id="pSupportedFileTypesLegend" class="Normal"><%= SupportedImage %> = <%= GetLocalizedString("SupportedImage.Text") %>&nbsp; <%= UnsupportedImage %> = <%= GetLocalizedString("UnsupportedImage.Text") %></p>
                        </div>
                    </div>
                </div>
            </li>
            <li id="liEmbed" class="dnnmedia-listitem dnnmedia-embed" runat="server">
                <div class="dnnFormItem">
                    <dnn:label id="lblEmbed" runat="server" ResourceKey="plEmbed" ControlName="txtEmbed" suffix=":" />
                    <asp:TextBox ID="txtEmbed" runat="server" CssClass="NormalTextBox dnnmedia_textbox dnnFormRequired" MaxLength="1000" Rows="5" TextMode="MultiLine" />
                </div>
                <div class="dnnmedia-fieldwrapper">
                    <div class="dnnFormMessage dnnFormInfo"><%= GetLocalizedString("lblEmbedSupport.Text") %></div>
                </div>
            </li>
            <li id="liOEmbed" class="dnnmedia-listitem dnnmedia-oembed" runat="server">
                <div class="dnnmedia-fieldwrapper">
                    <p class="Normal"><%= GetLocalizedString("lblOEmbedDesc.Text") %></p>
                </div>
                <div class="dnnFormItem">
                    <dnn:label id="lblOEmbed" runat="server" ResourceKey="plOEmbed" ControlName="txtOEmbed" Suffix=":" />
                    <asp:TextBox ID="txtOEmbed" runat="server" CssClass="NormalTextBox dnnmedia_textbox dnnFormRequired" MaxLength="255" /> 
                    <asp:LinkButton ID="lnkOEmbed" runat="server" CausesValidation="false" CssClass="CommandButton" />
                </div>
                <div class="dnnmedia-fieldwrapper dnnmedia-message">
                    <asp:Label ID="lblOEmbedCheck" runat="server" CssClass="Normal" />
                </div>
                <div class="dnnmedia-fieldwrapper">
                    <div class="dnnFormMessage dnnFormInfo"><%= GetLocalizedString("lblOEmbedSupport.Text") %></div>
                </div>
            </li>
            <li class="dnnmedia-listitem">
                <div class="dnnFormItem">
                    <dnn:label id="plAlt" runat="server" ResourceKey="plAlt" ControlName="txtAlt" Suffix=":" />
                    <asp:TextBox id="txtAlt" CssClass="NormalTextBox dnnmedia_textbox dnnFormRequired" runat="server" MaxLength="100" />
                    <asp:RequiredFieldValidator id="valAltText" resourcekey="valAltText.ErrorMessage" runat="server" controltovalidate="txtAlt"
                                                display="Dynamic" cssclass="dnnFormMessage dnnFormError" errormessage="<br />Alternate Text Is Required" />
                </div>
            </li>
        </ol>
    </fieldset>
    <h2 id="dnnPanel-h2Settings" class="dnnFormSectionHead"><a href="" class=""><%= GetLocalizedString("BasicSettings.Text") %></a></h2>
    <fieldset id="fsSettings" class="dnnmedia-fieldset">
        <ol id="olSettings" class="dnnmedia-list">
            <li class="dnnmedia-listitem">
                <div class="dnnFormItem">
                    <dnn:label id="plWidth" runat="server" ResourceKey="plWidth" ControlName="txtWidth" Suffix=":" />
                    <asp:TextBox id="txtWidth" CssClass="NormalTextBox dnnmedia_textbox" runat="server" MaxLength="10" /> 
                    <asp:RegularExpressionValidator id="valWidth" resourcekey="valWidth.ErrorMessage" controltovalidate="txtWidth" validationexpression="^[1-9]+[0-9]*$"
                                                    display="Dynamic" cssclass="dnnFormMessage dnnFormError" errormessage="<br />Width Must Be A Valid Integer" runat="server" />
                </div>
            </li>
            <li class="dnnmedia-listitem">
                <p class="Normal dnnmedia-form-message"><%= GetLocalizedString("VideoDimsRequired.Text") %></p>
            </li>
            <li class="dnnmedia-listitem">
                <div class="dnnFormItem">
                    <dnn:label id="plHeight" runat="server" ResourceKey="plHeight" ControlName="txtHeight" Suffix=":" />
                    <asp:TextBox id="txtHeight" CssClass="NormalTextBox dnnmedia_textbox" runat="server" MaxLength="10" />
                    <asp:RegularExpressionValidator id="valHeight" resourcekey="valHeight.ErrorMessage" controltovalidate="txtHeight"
                                                    validationexpression="^[1-9]+[0-9]*$" display="Dynamic" cssclass="dnnFormMessage dnnFormError" errormessage="<br />Height Must Be A Valid Integer"
                                                    runat="server" />
                </div>
            </li>
            <li class="dnnmedia-listitem">
                <p class="Normal dnnmedia-form-message"><%= GetLocalizedString("VideoDimsRequired.Text") %></p>
            </li>
            <li class="dnnmedia-listitem">
                <div class="dnnFormItem">
                    <dnn:label id="plMessage" runat="server" ResourceKey="plMessage" ControlName="txtMessage" Suffix=":" />
                    <div id="divMessageWrapper" class="dnnRight">
                        <dnn:Text ID="txtMessage" runat="server" ChooseMode="true" ChooseRender="true" Height="300px" HtmlEncode="true" Width="570px" />
                    </div>
                </div>
            </li>
            <li class="dnnmedia-listitem">
                <div class="dnnFormItem">
                    <dnn:label id="plAlignment" runat="server" ResourceKey="plAlignment" ControlName="ddlImageAlignment" Suffix=":" />
                    <asp:DropDownList ID="ddlImageAlignment" Runat="server" Width="200" CssClass="dnnmedia_dropdownlist" />
                </div>
            </li>
        </ol>
    </fieldset>
    <h2 id="dnnPanel-h2VideoSettings" class="dnnFormSectionHead"><a href="" class=""><%= GetLocalizedString("lblVideosOnly.Text") %></a></h2>
    <fieldset id="fsVideoSettings" class="dnnmedia-fieldset">
        <ol id="olVideoSettings" class="dnnmedia-list">
            <li class="dnnmedia-listitem">
                <div class="dnnFormMessage dnnFormWarning"><%= GetLocalizedString("lblVideoMessage.Text") %></div>
            </li>
            <li class="dnnmedia-listitem">
                <div class="dnnFormItem">
                    <dnn:label id="lblAutoStart" runat="server" ResourceKey="lblAutoStart" ControlName="chkAutoStart" Suffix=":" />
                    <asp:CheckBox ID="chkAutoStart" runat="server" CssClass="dnnmedia_checkbox" />
                </div>
            </li>
            <li class="dnnmedia-listitem">
                <div class="dnnFormItem">
                    <dnn:label id="lblLoop" runat="server" ResourceKey="lblLoop" ControlName="chkLoop" Suffix=":" />
                    <asp:CheckBox ID="chkLoop" runat="server" CssClass="dnnmedia_checkbox" />
                </div>
            </li>
        </ol>
    </fieldset>
    <h2 id="dnnPanel-h2ImageSettings" class="dnnFormSectionHead"><a href="" class=""><%= GetLocalizedString("lblImagesOnly.Text") %></a></h2>
    <fieldset id="fsImageSettings" class="dnnmedia-fieldset">
        <ol id="olImageSettings" class="dnnmedia-list">
            <li class="dnnmedia-listitem">
                <div class="dnnFormItem">
                    <dnn:label id="plNavigateUrl" runat="server" ResourceKey="plNavigateUrl" Suffix=":" />
                    <div class="dnnLeft">
                        <dnn:Url id="ctlNavigateUrl" runat="server" width="300" required="False" showtabs="False"
                                 showfiles="True" showUrls="True" showlog="False" shownewwindow="False" showtrack="False" />
                    </div>
                </div>
            </li>
        </ol>
    </fieldset>
    <h2 id="H1" class="dnnFormSectionHead"><a href="" class=""><%= GetLocalizedString("lblJournalIntegration.Text") %></a></h2>
    <fieldset id="fsJournalIntegration" class="dnnmedia-fieldset">
        <ol id="olJournalIntegration" class="dnnmedia-list">
            <li id="liPostToJournal" runat="server" class="dnnmedia-listitem">
                <div class="dnnFormItem">
                    <dnn:label id="lblPostToJournal" runat="server" ResourceKey="lblPostToJournal" Suffix=":" />
                    <asp:CheckBox ID="chkPostToJournal" runat="server" CssClass="dnnmedia_checkbox" />
                </div>
            </li>
            <li id="liSideWideJournalSetting" runat="server" class="dnnmedia-listitem">
                <div class="dnnFormItem">
                    <dnn:label id="lblPostToJournalSiteWide" runat="server" ResourceKey="lblPostToJournalSiteWide" Suffix=":" />
                    <asp:CheckBox ID="chkPostToJournalSiteWide" runat="server" CssClass="dnnmedia_checkbox" />
                </div>
            </li>
            <li class="dnnmedia-listitem">
                <div class="dnnFormItem">
                    <dnn:label id="lblOverrideJournalSetting" runat="server" ResourceKey="lblOverrideJournalSetting" Suffix=":" />
                    <asp:CheckBox ID="chkOverrideJournalSetting" runat="server" CssClass="dnnmedia_checkbox" AutoPostBack="true" />
                </div>
            </li>
            <li id="liNotifyOnUpdate" runat="server" class="dnnmedia-listitem">
                <div class="dnnFormItem">
                    <dnn:label id="lblNotifyOnUpdate" runat="server" ResourceKey="lblNotifyOnUpdate" Suffix=":" />
                    <asp:CheckBox ID="chkNotifyOnUpdate" runat="server" CssClass="dnnmedia_checkbox" />
                </div>
            </li>
        </ol>
    </fieldset>
    <div id="divMediaCommands">
        <ul id="ulMediaCommands">
            <li class="dnnmedia-cmd-listitem">
                <asp:LinkButton class="dnnPrimaryAction" id="cmdUpdate" resourcekey="cmdUpdate" runat="server" borderstyle="none" text="Update" />
            </li>
            <li class="dnnmedia-cmd-listitem">
                <asp:LinkButton class="dnnSecondaryAction" id="cmdCancel" resourcekey="cmdCancel" runat="server" borderstyle="none" text="Cancel" causesvalidation="False" CommandName="cmdCancel_Click" />
            </li>
        </ul>
    </div>
</div>
<div id="divDnnAudit" class="dnnssStat dnnClear">
    <div id="divTracking" class="dnnmedia-audit dnnClear">
        <dnn:Tracking id="ctlTracking" runat="server" />
    </div>
    <div id="divLastUpdated" class="dnnmedia-audit dnnClear">
        <span class="SubHead"><%= LastUpdated %></span>
    </div>
</div>
<script language="javascript" type="text/javascript">
    (function($, Sys) {

        function setupDnnSiteSettings() {
            $('#dnnEditEntry').dnnPanels();
            $('#dnnEditEntry .dnnFormExpandContent a').dnnExpandAll({
                expandText: '<%= Localization.GetString("ExpandAll", Localization.SharedResourceFile) %>',
                collapseText: '<%= Localization.GetString("CollapseAll", Localization.SharedResourceFile) %>',
                targetArea: '#dnnEditEntry'
            });

            $('#divSupportedFileTypes').hide();
            $('#lnkViewFileTypes').live('click', function() {
                $('#divSupportedFileTypes').toggle();
                return false;
            });
        }

        $(document).ready(function() {
            setupDnnSiteSettings();
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function() {
                setupDnnSiteSettings();
            });
        });

    }(jQuery, window.Sys));
</script>