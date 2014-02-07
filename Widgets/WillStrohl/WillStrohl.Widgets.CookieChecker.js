/*
Cookie Checker Widget
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
''' This widget script allows you to check for support for cookies in a client 
''' web browser, and then display a useful message if the browsers cookies are 
''' not enabled.
''' </summary>
''' <version>01.00.00</version>
''' <remarks>
''' </remarks>
''' <history>
''' [wstrohl] - 20101218 - Created
''' </history>
''' -----------------------------------------------------------------------------

EXAMPLE:
<object id="ANY-UNIQUE-ID-YOU-WANT" codetype="dotnetnuke/client" codebase="WillStrohl.Widgets.CookieChecker" declare="declare"> 
    <param name="helpUrl" value="true" /> 
    <param name="headerMessage" value="true" /> 
    <param name="cookieMessage" value="true" /> 
    <param name="debug" value="true" /> 
</object>
*/

////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// BEGIN: CookieChecker class                                                                              //
////////////////////////////////////////////////////////////////////////////////////////////////////////////////

// BEGIN: Namespace management
Type.registerNamespace("WillStrohl.Widgets");
// END: Namespace management

WillStrohl.Widgets.CookieChecker = function(widget)
{
	WillStrohl.Widgets.CookieChecker.initializeBase(this, [widget]);
}

WillStrohl.Widgets.CookieChecker.prototype =
{
    // BEGIN: render
    render: function () {
        var widget = this._widget;

        (function ($) {
            // Default parameters
            var helpUrl = 'http://www.google.com/support/accounts/bin/answer.py?&amp;answer=61416';
            var headerMessage = 'Whoops! Cookies Are Disabled...';
            var cookieMessage = 'Cookies are used on this site to help enhance your experience and satisfaction with our features. Please <a href="[REPLACEME]" target="_blank">enable cookies in your web browser</a> before you continue.';
            var debugEnabled = 'false';

            // Parse parameters
            $(widget).children().each(function () {
                if (this.name && this.value) {
                    var paramName = this.name.toLowerCase();
                    var paramValue = this.value;

                    switch (paramName) {
                        case 'helpurl': helpUrl = paramValue; break;
                        case 'headerMessage': headerMessage = paramValue; break;
                        case 'cookieMessage': cookieMessage = paramValue; break;
                        case 'debug': debugEnabled = paramValue; break;
                    }
                }
            });

            var selector = '';

            /* INITIATE THE DEBUGGER */
            var runDebug = false;
            if (debugEnabled == 'true') runDebug = true;

            if (runDebug) {
                if ($('#DebugConsole').length == 0) $('body').append('<div id="DebugConsole" class="DebugConsole"></div>');
                $DEBUGLINE('<span class="Head">Widget Suite: CookieChecker Debug Report</span><br />');
                $DEBUGLINE('<span class="SubHead">Parameters Values:</span>');
                $DEBUGLINE('helpUrl = ' + helpUrl);
                $DEBUGLINE('headerMessage = ' + headerMessage);
                $DEBUGLINE('cookieMessage = ' + cookieMessage);
                $DEBUGLINE('debug = ' + debugEnabled);
                $DEBUGLINE('<br /><span class="SubHead">Activity Log:</span>');
            }

            if (runDebug) $DEBUGLINE('Attempt to create a client-side cookie');
            setWNSCookie('testCookie', 'successful', 7);

            var strCheck = getWNSCookie('testCookie');

            if (runDebug) $DEBUGLINE('Cookie Value = ' + strCheck);

            if (runDebug) $DEBUGLINE('Test the client-side cookie to see if it exists');
            if (strCheck != null && strCheck != '') {

                if (runDebug) $DEBUGLINE('The cookie EXISTS');

            } else {

                if (runDebug) $DEBUGLINE('The cookie DOES NOT EXIST');

                if (runDebug) $DEBUGLINE('BEGIN CookieChecker Logic');

                if (helpUrl != '' && helpUrl.indexOf('[REPLACEME]') > -1) {
                    if (runDebug) $DEBUGLINE('Replacing helpUrl in cookieMessage');
                    cookieMessage.replace('[REPLACEME]', helpUrl);
                }

                if (runDebug) $DEBUGLINE('Creating a new message container');
                $('#dnn_ContentPane').prepend('<div id="wgtCookieCheckerContainer" class="CookieChecker Normal"><span id="spnCookieCheckerWrap" class="CookieChecker"><h2 id="hdrCookieChecker" class="Head CookieChecker"></h2><p id="pCookieCheckerMessage" class="CookieChecker"></p></span></div>');

                $('#hdrCookieChecker').html(headerMessage);
                $('#pCookieCheckerMessage').html(cookieMessage);

                if (runDebug) $DEBUGLINE('END CookieChecker Logic');

            }

            $(widget).remove();

            if (runDebug) $DEBUGLINE('<br /><span class="NormalRed">Widget Suite: CookieChecker Debug Report Complete</span>');

        })(jQuery);
    }
    // END: render
}

function setWNSCookie(c_name, value, expiredays) { var exdate = new Date(); exdate.setDate(exdate.getDate() + expiredays); document.cookie = c_name + '=' + escape(value) + ((expiredays == null) ? '' : ';expires=' + exdate.toGMTString()); }
function getWNSCookie(c_name) { if (document.cookie.length > 0) { c_start = document.cookie.indexOf(c_name + '='); if (c_start != -1) { c_start = c_start + c_name.length + 1; c_end = document.cookie.indexOf(';', c_start); if (c_end == -1) c_end = document.cookie.length; return unescape(document.cookie.substring(c_start, c_end)); } } return ''; }

WillStrohl.Widgets.CookieChecker.inheritsFrom(DotNetNuke.UI.WebControls.Widgets.BaseWidget);
WillStrohl.Widgets.CookieChecker.registerClass("WillStrohl.Widgets.CookieChecker", DotNetNuke.UI.WebControls.Widgets.BaseWidget);
DotNetNuke.UI.WebControls.Widgets.renderWidgetType("WillStrohl.Widgets.CookieChecker");
// END: CookieChecker class
