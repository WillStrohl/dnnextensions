/*
Moderniar Widget
Widget Suite for DotNetNuke
Author - Armand Datema (http://www.schwingsoft.com)
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
''' This widget adds Modernizr to the skin.
''' </summary>
''' <version>01.00.00</version>
''' <remarks>
''' </remarks>
''' <history>
''' [nokiko] - 20101219 - Created
''' </history>
''' -----------------------------------------------------------------------------

EXAMPLE:
<object id="wgtModernizr" codetype="dotnetnuke/client" codebase="WillStrohl.Widgets.Modernizr" declare="declare">
</object>

<object id="wgtModernizr" codetype="dotnetnuke/client" codebase="WillStrohl.Widgets.Modernizr" declare="declare">
</object>
*/

////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// BEGIN: Modernizr class                                                                              //
////////////////////////////////////////////////////////////////////////////////////////////////////////////////

// BEGIN: Namespace management
Type.registerNamespace("WillStrohl.Widgets");
// END: Namespace management

WillStrohl.Widgets.Modernizr = function(widget)
{
    WillStrohl.Widgets.Modernizr.initializeBase(this, [widget]);
}

WillStrohl.Widgets.Modernizr.prototype =
{
    // BEGIN: render
    render:
        function() {

            /* set-up parameters */
            var params = this._widget.childNodes;
            if (params != null) {
                /* default parameters */

                /* parse parameters */
                for (var p = 0; p < params.length; p++) {
                    try {
                        var paramName = params[p].name.toLowerCase();
                        switch (paramName) {

                        }
                    }
                    catch (e) {
                        //alert('An Error Occurred: ' + e);
                    }
                }
            }

            try {
                    jQuery.getScript($dnn.baseResourcesUrl +
                        "Widgets/User/WillStrohl/js/modernizr-1.6.min.js", function() {
                    });
            }
            catch (e) {
                //alert('An Error Occurred: ' + e);
            }

            $(this._widget).remove();

        }
    // END: render
}

WillStrohl.Widgets.Modernizr.inheritsFrom(DotNetNuke.UI.WebControls.Widgets.BaseWidget);
WillStrohl.Widgets.Modernizr.registerClass("WillStrohl.Widgets.Modernizr", DotNetNuke.UI.WebControls.Widgets.BaseWidget);
DotNetNuke.UI.WebControls.Widgets.renderWidgetType("WillStrohl.Widgets.Modernizr");
// END: Modernizr class
