/*
Basic Settings Widget
Widget Suite for DotNetNuke
Author - Will Strohl (http://www.WillStrohl.com)
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
''' This widget script allows you to remove advanced settings from your DotNetNuke site.
''' </summary>
''' <version>01.00.00</version>
''' <remarks>
''' </remarks>
''' <history>
''' [wstrohl] - 20110115 - Created
''' </history>
''' -----------------------------------------------------------------------------

EXAMPLE:
<object id="ANY-UNIQUE-ID-YOU-WANT" codetype="dotnetnuke/client" codebase="WillStrohl.Widgets.BasicSettings" declare="declare"> 
    <param name="debug" value="true" /> 
</object>
*/

////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// BEGIN: BasicSettings class                                                                              //
////////////////////////////////////////////////////////////////////////////////////////////////////////////////

// BEGIN: Namespace management
Type.registerNamespace("WillStrohl.Widgets");
// END: Namespace management

WillStrohl.Widgets.BasicSettings = function(widget)
{
	WillStrohl.Widgets.BasicSettings.initializeBase(this, [widget]);
}

WillStrohl.Widgets.BasicSettings.prototype =
{
    // BEGIN: render
    render: function () {
        var widget = this._widget;

        (function ($) {
            // Default parameters
            var debugEnabled = 'false';

            // Parse parameters
            $(widget).children().each(function () {
                if (this.name && this.value) {
                    var paramName = this.name.toLowerCase();
                    var paramValue = this.value;

                    switch (paramName) {
                        case 'debug': debugEnabled = paramValue; break;
                    }
                }
            });

            try {

                /* INITIATE THE DEBUGGER */
                var runDebug = false;
                if (debugEnabled == 'true') runDebug = true;

                if (runDebug) {
                    if ($('#DebugConsole').length == 0) $('body').append('<div id="DebugConsole" class="DebugConsole"></div>');
                    $DEBUGLINE('<span class="Head">Widget Suite: BasicSettings Debug Report</span><br />');
                    $DEBUGLINE('<span class="SubHead">Parameters Values:</span>');
                    $DEBUGLINE('debug = ' + debugEnabled);
                    $DEBUGLINE('<br /><span class="SubHead">Activity Log:</span>');
                }

                /* BEGIN Widget Code */

                if (runDebug) $DEBUGLINE('Basic Settings Widget Enabled');

                if (runDebug) $DEBUGLINE('MODULE SETTINGS: Remove Advanced Settings section');
                $('table[id$=\'_ModuleSettings_tblSecurity\']').prev().hide().end().hide();

                if (runDebug) $DEBUGLINE('MODULE SETTINGS: Remove Added to Pages section');
                $('table[id$=\'_ModuleSettings_tblInstalledOn\']').prev().hide().end().hide();

                if (runDebug) $DEBUGLINE('MODULE SETTINGS: Remove Page Settings section');
                $('table[id$=\'_ModuleSettings_tblPage\']').prev().prev().hide().end().hide().end().hide();

                if (runDebug) $DEBUGLINE('PAGE SETTINGS: Remove Page Settings section');
                $('table#dnn_ctr_ManageTabs_tblAdvanced').prev().prev().hide().end().hide().end().hide();

                if (runDebug) $DEBUGLINE('SITE SETTINGS: Remove Site Details > GUID');
                $('table[id$=\'_SiteSettings_tblSite\'] tr:nth-child(5)').children().hide();

                if (runDebug) $DEBUGLINE('SITE SETTINGS: Remove Marketing section advanced fields');
                $('table[id$=\'_SiteSettings_tblMarketing\'] tr:nth-child(4), table[id$=\'_SiteSettings_tblMarketing\'] tr:nth-child(3), table[id$=\'_SiteSettings_tblMarketing\'] tr:nth-child(2)').children().hide();

                if (runDebug) $DEBUGLINE('SITE SETTINGS: Remove Appearance > Skins section');
                $('table[id$=\'_SiteSettings_tblAppearance\'] tr:nth-child(7), table[id$=\'_SiteSettings_tblAppearance\'] tr:nth-child(6), table[id$=\'_SiteSettings_tblAppearance\'] tr:nth-child(5), table[id$=\'_SiteSettings_tblAppearance\'] tr:nth-child(4)').children().hide();

                if (runDebug) $DEBUGLINE('SITE SETTINGS: Remove Appearance > Body Background section');
                $('table[id$=\'_SiteSettings_tblAppearance\'] tr:nth-child(2)').children().hide();

                if (runDebug) $DEBUGLINE('SITE SETTINGS: Remove Advanced Settings section');
                $('table[id$=\'_SiteSettings_tblAdvanced\']').prev().prev().hide().end().hide().end().hide();

                if (runDebug) $DEBUGLINE('SITE SETTINGS: Remove Stylesheet Editor section');
                $('table[id$=\'_SiteSettings_tblStylesheet\']').prev().prev().hide().end().hide().end().hide();

                /* END Widget Code */

                if (runDebug) $DEBUGLINE('<br /><span class="NormalRed">Widget Suite: BasicSettings Debug Report Complete</span>');

            } catch (e) {
                //alert('An Error Occurred: ' + e);
                if (runDebug) $DEBUGLINE('<br /><span class="NormalRed">' + e.Message + '</span>');
            }

            $(widget).remove();

        })(jQuery);
    }
    // END: render
}

function setWNSCookie(c_name, value, expiredays) { var exdate = new Date(); exdate.setDate(exdate.getDate() + expiredays); document.cookie = c_name + '=' + escape(value) + ((expiredays == null) ? '' : ';expires=' + exdate.toGMTString()); }
function getWNSCookie(c_name) { if (document.cookie.length > 0) { c_start = document.cookie.indexOf(c_name + '='); if (c_start != -1) { c_start = c_start + c_name.length + 1; c_end = document.cookie.indexOf(';', c_start); if (c_end == -1) c_end = document.cookie.length; return unescape(document.cookie.substring(c_start, c_end)); } } return ''; }

WillStrohl.Widgets.BasicSettings.inheritsFrom(DotNetNuke.UI.WebControls.Widgets.BaseWidget);
WillStrohl.Widgets.BasicSettings.registerClass("WillStrohl.Widgets.BasicSettings", DotNetNuke.UI.WebControls.Widgets.BaseWidget);
DotNetNuke.UI.WebControls.Widgets.renderWidgetType("WillStrohl.Widgets.BasicSettings");
// END: BasicSettings class
