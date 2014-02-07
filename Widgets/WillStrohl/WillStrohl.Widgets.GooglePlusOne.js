/*
Google Plus One Widget
Widget Suite for DotNetNuke
Author - Will Strohl (http://www.WillStrohl.com)
Project Contributors - Will Strohl (http://www.WillStrohl.com), Mark Allan (http://www.dnngarden.com), Armand Datema (http://www.schwingsoft.com)
Support - http://dnnwidgets.codeplex.com
Copyright (c) 2010-2011, Will Strohl
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
''' This widget script allows you to embed a +1 button your site for Google+
''' not enabled.
''' </summary>
''' <version>01.00.00</version>
''' <remarks>
''' </remarks>
''' <history>
''' [wstrohl] - 20111028 - Created
''' </history>
''' -----------------------------------------------------------------------------

EXAMPLE:
<object id="ANY-UNIQUE-ID-YOU-WANT" codetype="dotnetnuke/client" codebase="WillStrohl.Widgets.GooglePlusOne" declare="declare"> 
    <param name="kloutName" value="willstrohl" />
	<param name="size" value="small" />
	<param name="textColor" value="#000000" />
	<param name="debug" value="true" /> 
</object>
*/

////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// BEGIN: GooglePlusOne class                                                                              //
////////////////////////////////////////////////////////////////////////////////////////////////////////////////

// BEGIN: Namespace management
Type.registerNamespace("WillStrohl.Widgets");
// END: Namespace management

WillStrohl.Widgets.GooglePlusOne = function (widget)
{
    WillStrohl.Widgets.GooglePlusOne.initializeBase(this, [widget]);
}

WillStrohl.Widgets.GooglePlusOne.prototype =
{
    // BEGIN: render
    render: function () {
        var widget = this._widget;

        (function ($, Sys) {
            // Default parameters
            var debugEnabled = 'false';
            var url = document.location.href;
            var ogTitle = document.title;
            var ogImage = '';
            var ogDescription = jQuery('meta[name=\'description\']').attr('content');
            var language = 'en-US';
            var size = 'standard';
            var annotation = 'bubble';
            var width = 0;
            var expandTo = '';

            // Parse parameters
            $(widget).children().each(function () {
                if (this.name && this.value) {
                    var paramName = this.name.toLowerCase();
                    var paramValue = this.value;

                    switch (paramName) {
                        case 'debug': debugEnabled = paramValue; break;
                        case 'url': url = paramValue; break;
                        case 'title': ogTitle = paramValue; break;
                        case 'image': ogImage = paramValue; break;
                        case 'description': ogDescription = paramValue; break;
                        case 'language': language = paramValue; break;
                        case 'size': size = paramValue; break;
                        case 'annotation': annotation = paramValue; break;
                        case 'width': width = paramValue; break;
                        case 'expandTo': expandTo = paramValue; break;
                    }
                }
            });

            /* INITIATE THE DEBUGGER */
            var runDebug = false;
            if (debugEnabled == 'true') runDebug = true;

            if (runDebug) {
                if ($('#DebugConsole').length == 0) $('body').append('<div id="DebugConsole" class="DebugConsole dnnClear"></div>');
                $DEBUGLINE('<span class="Head">Widget Suite: GooglePlusOne Debug Report</span><br />');
                $DEBUGLINE('<span class="SubHead">Parameters Values:</span>');
                $DEBUGLINE('debug = ' + debugEnabled);
                $DEBUGLINE('url = ' + url);
                $DEBUGLINE('title = ' + ogTitle);
                $DEBUGLINE('image = ' + ogImage);
                $DEBUGLINE('description = ' + ogDescription);
                $DEBUGLINE('language = ' + language);
                $DEBUGLINE('size = ' + size);
                $DEBUGLINE('annotation = ' + annotation);
                $DEBUGLINE('width = ' + width);
                $DEBUGLINE('expandTo = ' + expandTo);
                $DEBUGLINE('<br /><span class="SubHead">Activity Log:</span>');
            }

            if (runDebug) $DEBUGLINE('Initiating the Google+ button');

            if (runDebug) $DEBUGLINE('Adding the Open Graph elements');
            if (ogTitle != '' && jQuery('meta[property=\'og:title\']').length == 0) jQuery('head').append('<meta property="og:title" content="' + ogTitle + '"/>');
            if (ogImage != '' && jQuery('meta[property=\'og:image\']').length == 0) jQuery('head').append('<meta property="og:image" content="' + ogImage + '"/>');
            if (ogDescription != '' && jQuery('meta[property=\'og:description\']').length == 0) jQuery('head').append('<meta property="og:description" content="' + ogDescription + '"/>');

            if (runDebug) $DEBUGLINE('Adding the Google+ button');
            var gTag = '<g:plusone';

            if (url != '') gTag = gTag + ' href="' + url + '"';
            if (size != '') gTag = gTag + ' size="' + size + '"';
            if (annotation != '') gTag = gTag + ' annotation="' + annotation + '"';
            if (width != '') gTag = gTag + ' width="' + width + '"';
            if (expandTo != '') gTag = gTag + ' expandTo="' + expandTo + '"';

            gTag = gTag + '></g:plusone>';
            $(widget).replaceWith(gTag);

            if (runDebug) {
                gTag = gTag.replace('<', '&lt;');
                gTag = gTag.replace('>', '&gt;');
                gTag = gTag.replace('<', '&lt;');
                gTag = gTag.replace('>', '&gt;');
                $DEBUGLINE(gTag);
            }

            if (runDebug) $DEBUGLINE('Set the Google+ language');
            window.___gcfg = { lang: language };

            if (runDebug) $DEBUGLINE('Calling the Google+ script');
            loadPlusOneButton();

            if (runDebug) $DEBUGLINE('<br /><span class="NormalRed">Widget Suite: GooglePlusOne Debug Report Complete</span>');

        })(jQuery, window.Sys);
    }
    // END: render
}

WillStrohl.Widgets.GooglePlusOne.inheritsFrom(DotNetNuke.UI.WebControls.Widgets.BaseWidget);
WillStrohl.Widgets.GooglePlusOne.registerClass("WillStrohl.Widgets.GooglePlusOne", DotNetNuke.UI.WebControls.Widgets.BaseWidget);
DotNetNuke.UI.WebControls.Widgets.renderWidgetType("WillStrohl.Widgets.GooglePlusOne");
// END: GooglePlusOne class

function loadPlusOneButton() {
    var scriptPath = 'https://apis.google.com/js/plusone.js';
    if (jQuery('script[src=\'' + scriptPath + '\']').length == 0) jQuery.getScript(scriptPath);
    /*
    (function () {
        var po = document.createElement('script'); po.type = 'text/javascript'; po.async = true;
        po.src = 'https://apis.google.com/js/plusone.js';
        var s = document.getElementsByTagName('script')[0]; s.parentNode.insertBefore(po, s);
    })();
    */
}
