/*
Include jQuery UI Widget
Widget Suite for DotNetNuke
Author - Will Strohl (http://www.WillStrohl.com)
Project Contributors - Will Strohl (http://www.WillStrohl.com), Mark Allan (http://www.dnngarden.com), Armand Datema (http://www.schwingsoft.com)
Support - http://dnnwidgets.com
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
''' This widget script allows you to include jQuery UI in to your site.
''' not enabled.
''' </summary>
''' <version>01.00.00</version>
''' <remarks>
''' </remarks>
''' <history>
''' [wstrohl] - 20110217 - Created
''' </history>
''' -----------------------------------------------------------------------------

EXAMPLE:
<object id="ANY-UNIQUE-ID-YOU-WANT" codetype="dotnetnuke/client" codebase="WillStrohl.Widgets.IncludejQueryUI" declare="declare"> 
    <param name="version" value="1.8.10" /> 
    <param name="theme" value="vader" /> 
    <param name="debug" value="true" /> 
</object>
*/

////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// BEGIN: IncludejQueryUI class                                                                              //
////////////////////////////////////////////////////////////////////////////////////////////////////////////////

// BEGIN: Namespace management
Type.registerNamespace("WillStrohl.Widgets");
// END: Namespace management

WillStrohl.Widgets.IncludejQueryUI = function(widget)
{
	WillStrohl.Widgets.IncludejQueryUI.initializeBase(this, [widget]);
}

WillStrohl.Widgets.IncludejQueryUI.prototype =
{
    // BEGIN: render
    render: function () {
        var widget = this._widget;

        (function ($) {
            // Default parameters
            var theme = 'ui-lightness';
            var version = '1.8.10';
            var debugEnabled = 'false';

            // Parse parameters
            $(widget).children().each(function () {
                if (this.name && this.value) {
                    var paramName = this.name.toLowerCase();
                    var paramValue = this.value;

                    switch (paramName) {
                        case 'theme': theme = paramValue; break;
                        case 'version': version = paramValue; break;
                        case 'debug': debugEnabled = paramValue; break;
                    }
                }
            });

            /* INITIATE THE DEBUGGER */
            var runDebug = false;
            if (debugEnabled == 'true') runDebug = true;

            if (runDebug) {
                if ($('#DebugConsole').length == 0) $('body').append('<div id="DebugConsole" class="DebugConsole"></div>');
                $DEBUGLINE('<span class="Head">Widget Suite: IncludejQueryUI Debug Report</span><br />');
                $DEBUGLINE('<span class="SubHead">Parameters Values:</span>');
                $DEBUGLINE('version = ' + version);
                $DEBUGLINE('theme = ' + theme);
                $DEBUGLINE('debug = ' + debugEnabled);
                $DEBUGLINE('<br /><span class="SubHead">Activity Log:</span>');
            }

            if (runDebug) $DEBUGLINE('Including Script');

            /* reset the defaults as necessary */
            if (version == '') version = '1.8.10';

            try {

                if (theme != '' && theme != 'none') {
                    /*
                    BASIC FILE LOCATION FORMAT:  
                    http://ajax.googleapis.com/ajax/libs/jqueryui/{version-number}/themes/{theme-name}/jquery-ui.css

                    EXAMPLE CSS FILE LOCATIONS:
                    http://ajax.googleapis.com/ajax/libs/jqueryui/1.8.10/themes/base/jquery-ui.css
                    http://ajax.googleapis.com/ajax/libs/jqueryui/1.8.10/themes/black-tie/jquery-ui.css
                    http://ajax.googleapis.com/ajax/libs/jqueryui/1.8.10/themes/blitzer/jquery-ui.css
                    http://ajax.googleapis.com/ajax/libs/jqueryui/1.8.10/themes/cupertino/jquery-ui.css
                    http://ajax.googleapis.com/ajax/libs/jqueryui/1.8.10/themes/dot-luv/jquery-ui.css
                    http://ajax.googleapis.com/ajax/libs/jqueryui/1.8.10/themes/excite-bike/jquery-ui.css
                    http://ajax.googleapis.com/ajax/libs/jqueryui/1.8.10/themes/hot-sneaks/jquery-ui.css
                    http://ajax.googleapis.com/ajax/libs/jqueryui/1.8.10/themes/humanity/jquery-ui.css
                    http://ajax.googleapis.com/ajax/libs/jqueryui/1.8.10/themes/mint-choc/jquery-ui.css
                    http://ajax.googleapis.com/ajax/libs/jqueryui/1.8.10/themes/redmond/jquery-ui.css
                    http://ajax.googleapis.com/ajax/libs/jqueryui/1.8.10/themes/smoothness/jquery-ui.css
                    http://ajax.googleapis.com/ajax/libs/jqueryui/1.8.10/themes/south-street/jquery-ui.css
                    http://ajax.googleapis.com/ajax/libs/jqueryui/1.8.10/themes/start/jquery-ui.css
                    http://ajax.googleapis.com/ajax/libs/jqueryui/1.8.10/themes/swanky-purse/jquery-ui.css
                    http://ajax.googleapis.com/ajax/libs/jqueryui/1.8.10/themes/trontastic/jquery-ui.css
                    http://ajax.googleapis.com/ajax/libs/jqueryui/1.8.10/themes/ui-darkness/jquery-ui.css
                    http://ajax.googleapis.com/ajax/libs/jqueryui/1.8.10/themes/ui-lightness/jquery-ui.css
                    http://ajax.googleapis.com/ajax/libs/jqueryui/1.8.10/themes/vader/jquery-ui.css
                    */
                    if (runDebug) $DEBUGLINE('Loading jQueryUI CSS File');
                    $DEBUGLINE('File to Load: http://ajax.googleapis.com/ajax/libs/jqueryui/' + version + '/themes/' + theme + '/jquery-ui.css');
                    jQuery('head').append('<link id="lnkJQueryUI" />');
                    jQuery('#lnkJQueryUI').attr({ rel: "stylesheet", type: "text/css", href: 'http://ajax.googleapis.com/ajax/libs/jqueryui/' + version + '/themes/' + theme + '/jquery-ui.css' });
                }

                if (runDebug) {
                    $DEBUGLINE('Loading jQueryUI Script');
                    $DEBUGLINE('File to Load: http://ajax.googleapis.com/ajax/libs/jqueryui/' + version + '/jquery-ui.min.js');
                }
                /*
                BASIC FILE LOCATION FORMAT:
                https://ajax.googleapis.com/ajax/libs/jqueryui/{version-number}/jquery-ui.min.js
                EXAMPLE SCRIPT FILE LOCATION:
                https://ajax.googleapis.com/ajax/libs/jqueryui/1.8.10/jquery-ui.min.js
                */
                jQuery.getScript('http://ajax.googleapis.com/ajax/libs/jqueryui/' + version + '/jquery-ui.min.js');
            } catch (e) {
                alert(e.Message);
            }

            $(widget).remove();

            if (runDebug) $DEBUGLINE('<br /><span class="NormalRed">Widget Suite: IncludejQueryUI Debug Report Complete</span>');

        })(jQuery);
    }
    // END: render
}

WillStrohl.Widgets.IncludejQueryUI.inheritsFrom(DotNetNuke.UI.WebControls.Widgets.BaseWidget);
WillStrohl.Widgets.IncludejQueryUI.registerClass("WillStrohl.Widgets.IncludejQueryUI", DotNetNuke.UI.WebControls.Widgets.BaseWidget);
DotNetNuke.UI.WebControls.Widgets.renderWidgetType("WillStrohl.Widgets.IncludejQueryUI");
// END: IncludejQueryUI class
