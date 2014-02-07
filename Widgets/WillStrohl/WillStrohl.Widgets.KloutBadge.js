/*
Klout Badge Widget
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
''' This widget script allows you to embed a Klout badge on your DotNetNuke website
''' not enabled.
''' </summary>
''' <version>01.00.00</version>
''' <remarks>
''' </remarks>
''' <history>
''' [wstrohl] - 20111027 - Created
''' </history>
''' -----------------------------------------------------------------------------

EXAMPLE:
<object id="ANY-UNIQUE-ID-YOU-WANT" codetype="dotnetnuke/client" codebase="WillStrohl.Widgets.KloutBadge" declare="declare"> 
    <param name="kloutName" value="willstrohl" />
	<param name="size" value="small" />
	<param name="textColor" value="#000000" />
	<param name="debug" value="true" /> 
</object>
*/

////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// BEGIN: KloutBadge class                                                                              //
////////////////////////////////////////////////////////////////////////////////////////////////////////////////

// BEGIN: Namespace management
Type.registerNamespace("WillStrohl.Widgets");
// END: Namespace management

WillStrohl.Widgets.KloutBadge = function (widget)
{
    WillStrohl.Widgets.KloutBadge.initializeBase(this, [widget]);
}

WillStrohl.Widgets.KloutBadge.prototype =
{
    // BEGIN: render
    render: function () {
        var widget = this._widget;

        (function ($, Sys) {
            // Default parameters
            var debugEnabled = 'false';
            var size = 'large';
            var textColor = '#000000';
            var kloutName = '';

            // Parse parameters
            $(widget).children().each(function () {
                if (this.name && this.value) {
                    var paramName = this.name.toLowerCase();
                    var paramValue = this.value;

                    switch (paramName) {
                        case 'debug': debugEnabled = paramValue; break;
                        case 'size': size = paramValue; break;
                        case 'textcolor': textColor = paramValue; break;
                        case 'kloutname': kloutName = paramValue; break;
                    }
                }
            });

            /* INITIATE THE DEBUGGER */
            var runDebug = false;
            if (debugEnabled == 'true') runDebug = true;

            if (runDebug) {
                if ($('#DebugConsole').length == 0) $('body').append('<div id="DebugConsole" class="DebugConsole dnnClear"></div>');
                $DEBUGLINE('<span class="Head">Widget Suite: KloutBadge Debug Report</span><br />');
                $DEBUGLINE('<span class="SubHead">Parameters Values:</span>');
                $DEBUGLINE('debug = ' + debugEnabled);
                $DEBUGLINE('size = ' + size);
                $DEBUGLINE('textColor = ' + textColor);
                $DEBUGLINE('kloutName = ' + kloutName);
                $DEBUGLINE('<br /><span class="SubHead">Activity Log:</span>');
            }

            if (runDebug) $DEBUGLINE('Initiating the Klout Badge');

            if (kloutName == '') {
                alert('The KLOUTNAME parameter is required for the Klout Badge Widget');
            }
            else {
                /*
                LARGE
                <iframe src="http://widgets.klout.com/badge/joefernandez" style="border:0" scrolling="no" allowTransparency="true" frameBorder="0" width="200px" height="98px"></iframe>
                MEDIUM
                <iframe src="http://widgets.klout.com/badge/joefernandez?size=m" style="border:0" scrolling="no" allowTransparency="true" frameBorder="0" width="160px" height="78px"></iframe>
                SMALL
                <iframe src="http://widgets.klout.com/badge/joefernandez?size=s" style="border:0" scrolling="no" allowTransparency="true" frameBorder="0" width="120px" height="59px"></iframe>
                WITH COLOR
                <iframe src="http://widgets.klout.com/badge/joefernandez?size=s&text_color=5e305e" style="border:0" scrolling="no" allowTransparency="true" frameBorder="0" width="120px" height="59px"></iframe>
                */
                var kloutTag = '<iframe src="http://widgets.klout.com/badge/' + kloutName.replace('@', '');

                switch (size) {
                    case 'medium': kloutTag = kloutTag + '?size=m'; break;
                    case 'small': kloutTag = kloutTag + '?size=s'; break;
                    default: kloutTag = kloutTag + '?size=l'; break;
                }

                if (textColor != '' && textColor != '#000000') kloutTag = kloutTag + '&text_color=' + textColor.replace('#', '');

                kloutTag = kloutTag + '" style="border:0" scrolling="no" allowTransparency="true" frameBorder="0"';

                switch (size) {
                    case 'medium': kloutTag = kloutTag + ' width="160px" height="78px"></iframe>'; break;
                    case 'small': kloutTag = kloutTag + ' width="120px" height="59px"></iframe>'; break;
                    default: kloutTag = kloutTag + ' width="200px" height="98px"></iframe>'; break;
                }
            }

            $(widget).replaceWith(kloutTag);

            if (runDebug) $DEBUGLINE('<br /><span class="NormalRed">Widget Suite: KloutBadge Debug Report Complete</span>');

        })(jQuery, window.Sys);
    }
    // END: render
}

WillStrohl.Widgets.KloutBadge.inheritsFrom(DotNetNuke.UI.WebControls.Widgets.BaseWidget);
WillStrohl.Widgets.KloutBadge.registerClass("WillStrohl.Widgets.KloutBadge", DotNetNuke.UI.WebControls.Widgets.BaseWidget);
DotNetNuke.UI.WebControls.Widgets.renderWidgetType("WillStrohl.Widgets.KloutBadge");
// END: KloutBadge class
