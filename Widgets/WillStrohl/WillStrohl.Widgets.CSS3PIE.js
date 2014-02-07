/*
CSS 3 PIE Widget
Widget Suite for DotNetNuke
Author - Mark Allan (http://www.dnngarden.com)
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
''' This widget script applies the CSS3PIE behaviour to provide some CSS3 support
''' in IE 6-8.
''' </summary>
''' <version>01.00.00</version>
''' <remarks>
''' </remarks>
''' <history>
''' [markxa]  - 20101207 - Created
''' </history>
''' -----------------------------------------------------------------------------

EXAMPLE:
<object id="wgtCSS3PIE" codetype="dotnetnuke/client" codebase="WillStrohl.Widgets.CSS3PIE" declare="declare">
	<param name="selector" value="IMG,DIV,P" />
</object>
*/

////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// BEGIN: CSS3PIE class                                                                                       //
////////////////////////////////////////////////////////////////////////////////////////////////////////////////

// BEGIN: Namespace management
Type.registerNamespace("WillStrohl.Widgets");
// END: Namespace management

WillStrohl.Widgets.CSS3PIE = function (widget) {
	WillStrohl.Widgets.CSS3PIE.initializeBase(this, [widget]);
}

WillStrohl.Widgets.CSS3PIE.prototype =
{
	// BEGIN: render
	render: function () {
		if (/MSIE /.test(navigator.userAgent)) {
			var widget = this._widget;
			var widgetBase = $dnn.baseResourcesUrl + "Widgets/User/WillStrohl/";

			(function ($) {
				// Default parameters
				var selector = "*";

				// Parse parameters
				$(widget).children("param").each(function () {
					if (this.name && this.value) {
						var paramName = this.name.toLowerCase();
						var paramValue = this.value;

						switch (paramName) {
							case "selector":
								selector = paramValue;
								break;
						}
					}
				});

				// Apply behaviour
				var htc = "behavior:url(" + widgetBase + "js/CSS3PIE/PIE.htc)";
				document.styleSheets[0].addRule(selector, htc);

				// Remove widget declaration from DOM for cleaner rendering
				$(widget).remove();
			})(jQuery);
		}
	}
	// END: render
}

WillStrohl.Widgets.CSS3PIE.inheritsFrom(DotNetNuke.UI.WebControls.Widgets.BaseWidget);
WillStrohl.Widgets.CSS3PIE.registerClass("WillStrohl.Widgets.CSS3PIE", DotNetNuke.UI.WebControls.Widgets.BaseWidget);
DotNetNuke.UI.WebControls.Widgets.renderWidgetType("WillStrohl.Widgets.CSS3PIE");
// END: CSS3PIE class
