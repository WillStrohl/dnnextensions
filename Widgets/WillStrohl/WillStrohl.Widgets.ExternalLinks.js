/*
Will Strohl - http://www.WillStrohl.com
Authors - Will Strohl, Mark Allan (http://www.dnngarden.com)
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
''' This widget script adds an image to external links found in the page.
''' </summary>
''' <version>01.00.02</version>
''' <remarks>
''' </remarks>
''' <history>
''' [wstrohl] - 20101129 - Created
''' [markxa]  - 20101203 - Added "newWindow" parameter to add target="_blank"
'''                      - Added "class" parameter to add CSS class to links
'''                      - Used $dnn base URLs to root images
'''                      - Allowed specifying blank image to leave it out
''' [markxa]  - 20101215 - Workrounds for a couple of inconsistencies in IE6-8
''' [markxa]  - 20101229 - Added Google Analytics event tracking
'''                      - "class" parameter adds class instead of replacing
''' </history>
''' -----------------------------------------------------------------------------

EXAMPLE:
<object id="wgtExternalLinks" codetype="dotnetnuke/client" codebase="WillStrohl.Widgets.ExternalLinks" declare="declare">
</object>

<object id="wgtExternalLinks" codetype="dotnetnuke/client" codebase="WillStrohl.Widgets.ExternalLinks" declare="declare">
	<param name="image" value="/images/edit.gif" />
	<param name="altText" value="Opens to New Site" />
	<param name="class" value="externalLink" />
	<param name="newWindow" value="true" />
	<param name="selector" value="#wrapper" />
	<param name="gaCategory" value="Outbound" />
	<param name="gaAction" value="Click" />
</object>
*/

////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// BEGIN: ExternalLinks class                                                                                 //
////////////////////////////////////////////////////////////////////////////////////////////////////////////////

// BEGIN: Namespace management
Type.registerNamespace("WillStrohl.Widgets");
// END: Namespace management

WillStrohl.Widgets.ExternalLinks = function (widget) {
	WillStrohl.Widgets.ExternalLinks.initializeBase(this, [widget]);
}

WillStrohl.Widgets.ExternalLinks.prototype = {
	// BEGIN: render
	render: function () {
		var widget = this._widget;
		var widgetBase = $dnn.baseResourcesUrl + "Widgets/User/WillStrohl/";

		(function ($) {
			var $widget = $(widget);

			// Default parameters
			var image = widgetBase + "images/external-link.png";
			var altText = "External link";
			var cssClass = "";
			var newWindow = false;
			var selector = "BODY";
			var gaCategory = "";
			var gaAction = "";

			// Parse parameters
			$widget.children("param").each(function () {
				if (this.name && this.value) {
					var paramName = this.name.toLowerCase().trim();
					var paramValue = this.value.trim();

					switch (paramName) {
						case "image":
							image = paramValue;
							if (image === "")
								image = null;
							else if (image.indexOf("://") === -1)
								image = ($dnn.hostUrl + image).replace(/^:\/\//g, "/");
							break;
						case "alttext":
							altText = paramValue;
							break;
						case "class":
							cssClass = paramValue;
							break;
						case "newwindow":
							newWindow = (paramValue === "true");
							break;
						case "selector":
							selector = paramValue;
							break;
						case "gacategory":
							gaCategory = paramValue;
							break;
						case "gaaction":
							gaAction = paramValue;
							break;
					}
				}
			});

			// Process links
			var pageHost = document.location.hostname;
			$(selector).find("a").each(function () {
				if (this.hostname && (this.hostname !== pageHost)) {
					var $this = $(this);

					if (image && ($this.find("img").length === 0))
						$this.append($("<img/>").attr({ src: image, alt: altText, title: altText }));

					if (cssClass != "")
						$this.addClass(cssClass);

					if (newWindow)
						$this.attr({ target: "_blank" });

					if (gaCategory != "") {
						$this.click(function () {
							var href = $this.attr("href");
							var action = gaAction;
							var label = href;
							if (action == "") {
								action = href;
								label = null;
							}

							if (window._gaq)
								_gaq.push(['_trackEvent', gaCategory, action, label]);
							else if (window.pageTracker && pageTracker._trackEvent)
								pageTracker._trackEvent(gaCategory, action, label);
							else
								alert("Google Analytics script not found");
						});
					}
				}
			});

			// Remove widget declaration from DOM for cleaner rendering
			$(widget).remove();
		})(jQuery);
	}
	// END: render
}

WillStrohl.Widgets.ExternalLinks.inheritsFrom(DotNetNuke.UI.WebControls.Widgets.BaseWidget);
WillStrohl.Widgets.ExternalLinks.registerClass("WillStrohl.Widgets.ExternalLinks", DotNetNuke.UI.WebControls.Widgets.BaseWidget);
DotNetNuke.UI.WebControls.Widgets.renderWidgetType("WillStrohl.Widgets.ExternalLinks");
// END: ExternalLinks class
