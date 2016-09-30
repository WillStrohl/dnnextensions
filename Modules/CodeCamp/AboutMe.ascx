<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AboutMe.ascx.cs" Inherits="WillStrohl.Modules.CodeCamp.AboutMe" %>

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
            <p class="DNNAligncenter"><a href="https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=3T7UPHNMW5AB8" target="_blank"><img alt="" src="https://www.paypalobjects.com/en_US/i/btn/btn_donateCC_LG.gif"/></a></p>
            <img alt="" border="0" src="https://www.paypalobjects.com/en_US/i/scr/pixel.gif" width="1" height="1">
        </div>
    </div>
</div>