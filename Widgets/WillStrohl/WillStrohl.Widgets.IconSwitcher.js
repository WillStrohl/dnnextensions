/*
Icon Switcher Widget
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
''' This widget script allows you to change the location that your icons are referenced in your skin.
''' </summary>
''' <version>01.00.00</version>
''' <remarks>
''' </remarks>
''' <history>
''' [wstrohl] - 20101203 - Created
''' </history>
''' -----------------------------------------------------------------------------

EXAMPLE:
<object id="ANY-UNIQUE-ID-YOU-WANT" codetype="dotnetnuke/client" codebase="WillStrohl.Widgets.IconSwitcher" declare="declare"> 
    <param name="defaultIconPath" value="/Resources/Widgets/User/WillStrohl/images/icons/" /> 
</object>
*/

////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// BEGIN: IconSwitcher class                                                                              //
////////////////////////////////////////////////////////////////////////////////////////////////////////////////

// BEGIN: Namespace management
Type.registerNamespace("WillStrohl.Widgets");
// END: Namespace management

WillStrohl.Widgets.IconSwitcher = function(widget)
{
	WillStrohl.Widgets.IconSwitcher.initializeBase(this, [widget]);
}

WillStrohl.Widgets.IconSwitcher.prototype =
{
    // BEGIN: render
    render: function () {
        var widget = this._widget;

        (function ($) {
            // Default parameters
            var defaultIconPath = '';
            var adminIconPath = '';
            var wrapper = '';
            var debugEnabled = 'false';
            var replaceExtension = '';

            // Parse parameters
            $(widget).children().each(function () {
                if (this.name && this.value) {
                    var paramName = this.name.toLowerCase();
                    var paramValue = this.value;

                    switch (paramName) {
                        case 'defaulticonpath': defaultIconPath = paramValue; break;
                        case 'adminiconpath': adminIconPath = paramValue; break;
                        case 'wrapper': wrapper = paramValue; break;
                        case 'debug': debugEnabled = paramValue; break;
                        case 'replaceextension': replaceExtension = paramValue; break;
                    }
                }
            });

            var selector = '';

            /* INITIATE THE DEBUGGER */
            var runDebug = false;
            if (debugEnabled == 'true') {
                runDebug = true;
            }

            if (runDebug) {
                if ($('#DebugConsole').length == 0) $('body').append('<div id="DebugConsole" class="DebugConsole"></div>');
                $DEBUGLINE('<span class="Head">Widget Suite: IconSwitcher Debug Report</span><br />');
                $DEBUGLINE('<span class="SubHead">Parameters Values:</span>');
                $DEBUGLINE('defaultIconPath = ' + defaultIconPath);
                $DEBUGLINE('adminIconPath = ' + adminIconPath);
                $DEBUGLINE('wrapper = ' + wrapper);
                $DEBUGLINE('debug = ' + debugEnabled);
                $DEBUGLINE('replaceExtension = ' + replaceExtension);
                $DEBUGLINE('<br /><span class="SubHead">Activity Log:</span>');
            }

            if (replaceExtension != '') {
                if (replaceExtension.indexOf('.') == -1) replaceExtension = '.' + replaceExtension;
            }

            if (defaultIconPath != '') {
                // parse icon path
                defaultIconPath = parseIconPath(defaultIconPath);

                if (runDebug) $DEBUGLINE('Parsed value of defaultIconPath is: ' + defaultIconPath);

                selector = 'img[src^="/images/"], input[src^="/images/"]';
                if (wrapper != '') {
                    selector = wrapper + ' img[src^="/images/"], ' + wrapper + ' input[src^="/images/"]';
                }

                if (runDebug) $DEBUGLINE('There are <span class="NormalBold">' + $(selector).length + '</span> instances of defaultIcons');

                // get a collection of all images that refer to the /images/* directory
                // replace the /images/ directory with the one in the param
                $(selector).each(function () {
                    replaceValues(this, '/images/', defaultIconPath, replaceExtension);
                });
            }

            if (adminIconPath != '') {
                // parse icon path
                adminIconPath = parseIconPath(adminIconPath);

                if (runDebug) $DEBUGLINE('Parsed value of adminIconPath is: ' + adminIconPath);

                selector = 'img[src^="/admin/"], input[src^="/admin/"]';
                if (wrapper != '') {
                    selector = wrapper + ' img[src^="/admin/"], ' + wrapper + ' input[src^="/admin/"]';
                }

                if (runDebug) $DEBUGLINE('There are <span class="NormalBold">' + $(selector).length + '</span> instances of adminIcons');

                // get a collection of all images that refer to the /images/* directory
                // replace the /images/ directory with the one in the param
                $(selector).each(function () {
                    replaceValues(this, '/admin/', adminIconPath, replaceExtension);
                });
            }

            $(widget).remove();

            if (runDebug) $DEBUGLINE('<br /><span class="NormalRed">Widget Suite: IconSwitcher Debug Report Complete</span>');

        })(jQuery);
    }
    // END: render
}

function replaceValues(obj, pathToReplace, iconPath, replaceExtension){

	if (iconPath != '') $(obj).attr('src', $(obj).attr('src').replace(pathToReplace, iconPath));
	if (replaceExtension != '') $(obj).attr('src', $(obj).attr('src').replace('.gif', replaceExtension));

}

function parseIconPath(path) {

    if (path != '') {
        // check for preceding slash, and fix
        if (path.substr(0, 1) != '/') path = '/' + path;
        // check for trailing slash, and fix
        if (path.substr(path.length - 1, 1) != '/') path = path + '/';
    }

    return path;

}

WillStrohl.Widgets.IconSwitcher.inheritsFrom(DotNetNuke.UI.WebControls.Widgets.BaseWidget);
WillStrohl.Widgets.IconSwitcher.registerClass("WillStrohl.Widgets.IconSwitcher", DotNetNuke.UI.WebControls.Widgets.BaseWidget);
DotNetNuke.UI.WebControls.Widgets.renderWidgetType("WillStrohl.Widgets.IconSwitcher");
// END: IconSwitcher class
