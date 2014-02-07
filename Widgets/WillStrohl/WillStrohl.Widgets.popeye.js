/*
Popeye Widget
Widget Suite for DotNetNuke
Author - Armand Datema (http://www.schwingsoft.com)
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
''' This widget uses the popeye jQuery plugin to create inline lightbox functionality .
''' Pass it the wrapper item and it will put all the children in a carousel movement.
''' </summary>
''' <version>01.00.00</version>
''' <remarks>
''' </remarks>
''' <history>
''' [Armand Datema] - 2011114 - Created
''' </history>
''' -----------------------------------------------------------------------------

EXAMPLE:
<object id="wgtpopeye" codetype="dotnetnuke/client" codebase="WillStrohl.Widgets.popeye" declare="declare">
</object>

<object id="wgtpopeye" codetype="dotnetnuke/client" codebase="WillStrohl.Widgets.popeye" declare="declare">
<param name="wrapper" value=".popeye" />
<param name="navigation" value="hover" />
<param name="caption" value="hover" />
<param name="zindex" value="10000" />
<param name="direction" value="right" />
<param name="duration" value="240" />
<param name="opacity" value ="0.8" />
<param name="easing" value ="swing" />
<param name="htmlStructure" value="#ppynav" />
</object>
*/

////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// BEGIN: popeye class                                                                              //
////////////////////////////////////////////////////////////////////////////////////////////////////////////////


// BEGIN: Namespace management
Type.registerNamespace("WillStrohl.Widgets");
// END: Namespace management

WillStrohl.Widgets.popeye = function(widget) {
    WillStrohl.Widgets.popeye.initializeBase(this, [widget]);
}

WillStrohl.Widgets.popeye.prototype =
{
    // BEGIN: render
    render: function() {
        var widget = this._widget;

        (function($) {
            // Default parameters
            var wrapper = ".popeye";
            var navigation = "hover";
            var caption = "hover";
            var zindex = "10000";
            var direction = "right";
            var duration = 240;
            var opacity = 0.8;
            var easing = "swing";
            var htmlStructure = "<div class='ppy-outer'><div class='ppy-stage'><div class='ppy-nav'><a class='ppy-prev' title='Previous image'>Previous image</a><a class='ppy-switch-enlarge' title='Enlarge'>Enlarge</a><a class='ppy-switch-compact' title='Close'>Close</a><a class='ppy-next' title='Next image'>Next image</a></div></div></div><div class='ppy-caption'><div class='ppy-counter'>Image <strong class='ppy-current'></strong> of <strong class='ppy-total'></strong> </div><span class='ppy-text'></span></div>";


            // Parse parameters
            $(widget).children().each(function() {
                if (this.name && this.value) {
                    var paramName = this.name.toLowerCase();
                    var paramValue = this.value;

                    switch (paramName) {
                        case "wrapper":
                            wrapper = paramValue;
                            break;
                        case "navigation":
                            navigation = paramValue;
                            break;                            
                        case "caption":
                            caption = paramValue;
                            break;
                        case "zindex":
                            zindex = paramValue;
                            break;
                        case "direction":
                            direction = paramValue;
                            break;
                        case "duration":
                            duration = paramValue;
                            break;
                        case "opacity":
                            opacity = paramValue;
                            break;
                        case "easing":
                            easing = paramValue;
                            break;
                        case "htmlstructure":
                            htmlStructure = paramValue;
                            break;
                    }
                }
            });

            // Get widget location
            var pageHost = document.location.host;

            // Set default values for original jQuery plugin
            var options = {
                navigation: navigation,
                caption: caption,
                zindex: parseInt(zindex),
                direction: direction,
                duration: parseInt(duration),
                opacity: opacity,
                easing: easing
            }

            // remove the <object> from DOM since that is not needed anymore
            jQuery(widget).remove();                      
         
            // Call script
            jQuery.getScript($dnn.baseResourcesUrl +
                    "Widgets/User/WillStrohl/js/jquery.popeye-2.0.4.min.js", function() {
                        $(wrapper).addClass("ppy").append($(htmlStructure)).popeye(options);
                    });
        })(jQuery);
    }
    // END: render
}

WillStrohl.Widgets.popeye.inheritsFrom(DotNetNuke.UI.WebControls.Widgets.BaseWidget);
WillStrohl.Widgets.popeye.registerClass("WillStrohl.Widgets.popeye", DotNetNuke.UI.WebControls.Widgets.BaseWidget);
DotNetNuke.UI.WebControls.Widgets.renderWidgetType("WillStrohl.Widgets.popeye");
// END: popeye class
