/*
Google Analytics Events Widget
Widget Suite for DotNetNuke
Author - Will Strohl (http://www.WillStrohl.com), Mark Allan (http://www.dnngarden.com)
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
''' This widget script allows you to inject event tracking information into your 
''' page for tracking in Google Analytics.
''' not enabled.
''' </summary>
''' <version>01.00.00</version>
''' <remarks>
''' </remarks>
''' <history>
''' [wstrohl] - 20101221 - Created
''' [markxa] - 20101229 - Support for old GA tracking scripts
'''                     - Better handling of optional parameters
''' </history>
''' -----------------------------------------------------------------------------

EXAMPLE:
<object id="ANY-UNIQUE-ID-YOU-WANT" codetype="dotnetnuke/client" codebase="WillStrohl.Widgets.GoogleAnalyticsEvents" declare="declare"> 
<param name="event" value="click" /> 
<param name="selector" value="#lnkName" /> 
<param name="category" value="" /> 
<param name="action" value="" />  
<param name="label" value="" />  
<param name="value" value="" /> 
</object>
*/

////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// BEGIN: GoogleAnalyticsEvents class                                                                         //
////////////////////////////////////////////////////////////////////////////////////////////////////////////////

// BEGIN: Namespace management
Type.registerNamespace("WillStrohl.Widgets");
// END: Namespace management

WillStrohl.Widgets.GoogleAnalyticsEvents = function (widget) {
    WillStrohl.Widgets.GoogleAnalyticsEvents.initializeBase(this, [widget]);
}

WillStrohl.Widgets.GoogleAnalyticsEvents.prototype =
{
    // BEGIN: render
    render: function () {
        var widget = this._widget;

        (function ($) {
            // Default parameters
            var event = 'click';
            var selector = '';
            var category = '';
            var action = '';
            var label = '';
            var value = '';
            var runDebug = false;

            // Parse parameters
            $(widget).children().each(function () {
                if (this.name && this.value) {
                    var paramName = this.name.toLowerCase().trim();
                    var paramValue = this.value.trim();

                    switch (paramName) {
                        case 'event': event = paramValue; break;
                        case 'selector': selector = paramValue; break;
                        case 'category': category = paramValue; break;
                        case 'action': action = paramValue; break;
                        case 'label': label = paramValue; break;
                        case 'value': value = paramValue; break;
                        case 'debug': runDebug = (paramValue.toLowerCase() == 'true'); break;
                    }
                }
            });

            if (label == "") label = null;
            if (value == "") value = null;

            if (runDebug) {
                if ($('#DebugConsole').length == 0) $('body').append('<div id="DebugConsole" class="DebugConsole"></div>');
                $DEBUGLINE('<span class="Head">Widget Suite: GoogleAnalyticsEvents Debug Report</span><br />');
                $DEBUGLINE('<span class="SubHead">Parameters Values:</span>');
                $DEBUGLINE('event = ' + event);
                $DEBUGLINE('selector = ' + selector);
                $DEBUGLINE('category = ' + category);
                $DEBUGLINE('action = ' + action);
                $DEBUGLINE('label = ' + label);
                $DEBUGLINE('value = ' + value);
                $DEBUGLINE('<br /><span class="SubHead">Activity Log:</span>');
            }

            if (runDebug) $DEBUGLINE('Checking for required parameters');
            if (event == '') alert('The EVENT parameter is required for the Google Analytics Events Widget');
            if (selector == '') alert('The SELECTOR parameter is required for the Google Analytics Events Widget');
            if (category == '') alert('The CATEGORY parameter is required for the Google Analytics Events Widget');
            if (action == '') alert('The ACTION parameter is required for the Google Analytics Events Widget');

            var valueOkay = (value == null || value.match(/^\d+$/));
            if (!valueOkay) {
                alert('The VALUE parameter in the Google Analytics Events Widget can only be a positive integer');
            }

            if (event != '' && selector != '' && category != '' && action != '' && valueOkay) {
                if (runDebug) $DEBUGLINE('Attach the Google Analytics Event code to the "selected" elements on the page');
                if (runDebug) $DEBUGLINE('Wiring to the ' + $(selector).length + ' elements found');

                $(selector).live(event, function () {
                    if (window._gaq)
                        _gaq.push(['_trackEvent', category, action, label, value]);
                    else if (window.pageTracker && pageTracker._trackEvent)
                        pageTracker._trackEvent(category, action, label, value);
                    else
                        alert("Google Analytics script not found");
                });
                if (runDebug) $DEBUGLINE('GA Event wired to selected element(s)');
            } else {
                if (runDebug) $DEBUGLINE('Google Analytics Event code cannot be wired');
            }

            $(widget).remove();

            if (runDebug) $DEBUGLINE('<br /><span class="NormalRed">Widget Suite: GoogleAnalyticsEvents Debug Report Complete</span>');
        })(jQuery);
    }
    // END: render
}

WillStrohl.Widgets.GoogleAnalyticsEvents.inheritsFrom(DotNetNuke.UI.WebControls.Widgets.BaseWidget);
WillStrohl.Widgets.GoogleAnalyticsEvents.registerClass("WillStrohl.Widgets.GoogleAnalyticsEvents", DotNetNuke.UI.WebControls.Widgets.BaseWidget);
DotNetNuke.UI.WebControls.Widgets.renderWidgetType("WillStrohl.Widgets.GoogleAnalyticsEvents");
// END: GoogleAnalyticsEvents class
