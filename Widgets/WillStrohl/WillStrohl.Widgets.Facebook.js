/*
Facebook Widget
Widget Suite for DotNetNuke
Author - Mark Allan (http://www.dnngarden.com)
Project Contributors - Will Strohl (http://www.WillStrohl.com), Mark Allan (http://www.dnngarden.com), Armand Datema (http://www.schwingsoft.com)
Support - http://dnnwidgets.codeplex.com
Copyright (c) 2011, Will Strohl
All rights reserved.

Redistribution and use in source and binary forms, with or without modification, are permitted 
provided that the following conditions are met:

* Redistributions of source code must retain the above copyright notice, this list of conditions 
and the following disclaimer.

* Redistributions in binary form must reproduce the above copyright notice, this list of 
conditions and the following disclaimer in the documentation and/or other materials provided 
with the distribution.

* Neither the name of Widget Suite for DotNetNuke nor the names of its contributors may be used 
to endorse or promote products derived from this software without specific prior written permission.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR 
IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY 
AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR 
CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL 
DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, 
DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER 
IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT 
OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

''' -----------------------------------------------------------------------------
''' <summary>
''' This widget script adds Facebook plugins to the page.
''' </summary>
''' <version>01.00.00</version>
''' <remarks>
''' </remarks>
''' <history>
''' [markxa]  - 20110314 - Created
''' </history>
''' -----------------------------------------------------------------------------

EXAMPLE:
<object id="wgtFacebook" codetype="dotnetnuke/client" codebase="WillStrohl.Widgets.Facebook" declare="declare">
</object>

<object id="wgtFacebook" codetype="dotnetnuke/client" codebase="WillStrohl.Widgets.Facebook" declare="declare">
	<param name="plugin" value="like" />
	<param name="show_faces" value="true" />
	<param name="width" value="200" />
</object>
*/

////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// BEGIN: Facebook class                                                                                 //
////////////////////////////////////////////////////////////////////////////////////////////////////////////////

// BEGIN: Namespace management
Type.registerNamespace("WillStrohl.Widgets");
// END: Namespace management

WillStrohl.Widgets.Facebook = function (widget) {
	WillStrohl.Widgets.Facebook.initializeBase(this, [widget]);
}

WillStrohl.Widgets.Facebook.prototype = {
    // BEGIN: render
    render: function () {
        var widget = this._widget;
        var widgetBase = $dnn.baseResourcesUrl + "Widgets/User/WillStrohl/";

        (function ($) {
            var $widget = $(widget);

            // Default parameters
            var plugin = "like";
            var ogTitle = document.title;
            var ogType = '';
            var ogUrl = document.location;
            var ogImage = '';
            var ogSiteName = '';
            var fbAdmins = '';
            var fbAppId = '';
            var ogDescription = jQuery('meta[name=\'description\']').attr('content');
            var params = {};

            if (ogDescription == undefined) ogDescription = '';

            // Parse parameters
            $widget.children("param").each(function () {
                if (this.name && this.value) {
                    var paramName = this.name.toLowerCase().trim();
                    var paramValue = this.value.trim();

                    switch (paramName) {
                        case "ogtitle": ogTitle = paramValue; break;
                        case "ogtype": ogType = paramValue; break;
                        case "ogurl": ogUrl = paramValue; break;
                        case "ogimage": ogImage = paramValue; break;
                        case "ogsitename": ogSiteName = paramValue; break;
                        case "fbadmins": fbAdmins = paramValue; break;
                        case "fbappid": fbAppId = paramValue; break;
                        case "ogdescription": ogDescription = paramValue; break;
                        case "plugin": plugin = paramValue; break;
                        default: params[paramName] = paramValue; break;
                    }
                }
            });

            if (ogTitle != '' && jQuery('meta[property=\'og:title\']').length == 0) jQuery('head').append('<meta property="og:title" content="' + ogTitle + '"/>');
            if (ogType != '' && jQuery('meta[property=\'og:type\']').length == 0) jQuery('head').append('<meta property="og:type" content="' + ogType + '"/>');
            if (ogUrl != '' && jQuery('meta[property=\'og:url\']').length == 0) jQuery('head').append('<meta property="og:url" content="' + ogUrl + '"/>');
            if (ogImage != '' && jQuery('meta[property=\'og:image\']').length == 0) jQuery('head').append('<meta property="og:image" content="' + ogImage + '"/>');
            if (ogSiteName != '' && jQuery('meta[property=\'og:site_name\']').length == 0) jQuery('head').append('<meta property="og:site_name" content="' + ogSiteName + '"/>');
            if (fbAdmins != '' && jQuery('meta[property=\'fb:admins\']').length == 0) jQuery('head').append('<meta property="fb:admins" content="' + fbAdmins + '"/>');
            if (fbAppId != '' && jQuery('meta[property=\'fb:app_id\']').length == 0) jQuery('head').append('<meta property="fb:app_id" content="' + fbAdmins + '"/>');
            if (ogDescription != '' && jQuery('meta[property=\'og:description\']').length == 0) jQuery('head').append('<meta property="og:description" content="' + ogDescription + '"/>');

            // Replace widget declaration with XFBML
            if (plugin == 'comments' || plugin == 'login-button' || plugin == 'live-stream' && $('#fb-root').length < 1) {
                $(widget).replaceWith($("<div id=\"fb-root\"></div><fb:" + plugin + " />").attr(params));
            } else {
                $(widget).replaceWith($("<fb:" + plugin + " />").attr(params));
            }
        })(jQuery);
    }
    // END: render
}

WillStrohl.Widgets.Facebook.inheritsFrom(DotNetNuke.UI.WebControls.Widgets.BaseWidget);
WillStrohl.Widgets.Facebook.registerClass("WillStrohl.Widgets.Facebook", DotNetNuke.UI.WebControls.Widgets.BaseWidget);
DotNetNuke.UI.WebControls.Widgets.renderWidgetType("WillStrohl.Widgets.Facebook");

// Load Facebook script once all XFBML is instantiated
jQuery.getScript("http://connect.facebook.net/en_US/all.js#xfbml=1", function() {
	FB.init({status: true, cookie: true, xfbml  : true});
});
// END: Facebook class
