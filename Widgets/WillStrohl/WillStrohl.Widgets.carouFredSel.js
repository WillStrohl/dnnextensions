/*
Carousel Widget
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
''' This widget uses the carouFredSel jQuery plugin to do all kind of sliders and carousels .
''' Pass it the wrapper item and it will put all the children in a carousel movement.
''' carouselType is used to make multiple option possible ( base carousel advanced carousel etc )
''' </summary>
''' <version>01.00.00</version>
''' <remarks>
''' </remarks>
''' <history>
''' [nokiko] - 20101204 - Created
''' </history>
''' -----------------------------------------------------------------------------

CAROUSELTYPES

1: This is the base slider with no extra features

EXAMPLE:
<object id="wgtcarouFredSel" codetype="dotnetnuke/client" codebase="WillStrohl.Widgets.carouFredSel" declare="declare">
</object>

<object id="wgtcarouFredSel" codetype="dotnetnuke/client" codebase="WillStrohl.Widgets.carouFredSel" declare="declare">
<param name="wrapper" value=".contentslider" />
<param name="direction" value="up" />
<param name="sliderSpeed" value="2500" />
<param name="itemHeight" value="100px" />
<param name="itemsVisible" value="1" />
<param name="carouselType" value="1" />
<param name="buttonPrev" value ="#carouselPrev" />
<param name="buttonNext" value ="#carouselNext" />
<param name="paginationHolder" value="#carouselPagination" />
<param name="autoStart" value="true" />
</object>
*/

////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// BEGIN: carouFredSel class                                                                              //
////////////////////////////////////////////////////////////////////////////////////////////////////////////////


// BEGIN: Namespace management
Type.registerNamespace("WillStrohl.Widgets");
// END: Namespace management

WillStrohl.Widgets.carouFredSel = function(widget) {
    WillStrohl.Widgets.carouFredSel.initializeBase(this, [widget]);
}

WillStrohl.Widgets.carouFredSel.prototype =
{
    // BEGIN: render
    render: function() {
        var widget = this._widget;

        (function($) {
            // Default parameters
            var wrapper = ".contentslider";
            var direction = "up";
            var itemHeight = "100px";
            var sliderSpeed = 1000;
            var itemsVisible = 1;
            var carouselType = 1;
            var buttonNext = null;
            var buttonPrev = null;
            var paginationHolder = "#carouselPagination";
            var autoStart = "true"

            // Parse parameters
            $(widget).children().each(function() {
                if (this.name && this.value) {
                    var paramName = this.name.toLowerCase();
                    var paramValue = this.value;

                    switch (paramName) {
                        case "wrapper":
                            wrapper = paramValue;
                            break;
                        case "direction":
                            direction = paramValue;
                            break;
                        case "sliderspeed":
                            sliderSpeed = paramValue;
                            break;
                        case "itemheight":
                            itemHeight = paramValue;
                            break;
                        case "itemsvisible":
                            itemsVisible = paramValue;
                            break;
                        case "carouseltype":
                            carouselType = paramValue;
                            break;
                        case "buttonnext":
                            buttonNext = paramValue;
                            break;
                        case "buttonprev":
                            buttonPrev = paramValue;
                            break;
                        case "paginationholder":
                            paginationHolder = paramValue;
                            break;
                        case "autostart":
                            autoStart = paramValue;
                            break;
                    }
                }
            });

            // Process links
            var pageHost = document.location.host;

            function startSlider() {
                if (autoStart == "true") {
                    return parseInt(sliderSpeed);
                }
                else {
                    return false;
                }

            }

            // remove the <object> from DOM since that is not needed anymore
            jQuery(widget).remove();

            jQuery.getScript($dnn.baseResourcesUrl +
                    "Widgets/User/WillStrohl/js/jquery.carouFredSel-2.5.4-packed.js", function() {
                        switch (parseInt(carouselType)) {
                            case 1:
                                jQuery(wrapper + " div").css("height", itemHeight);
                                jQuery(wrapper).carouFredSel({
                                    direction: direction,
                                    auto: startSlider(),
                                    items: parseInt(itemsVisible),
                                    prev: {
                                        button: buttonPrev,
                                        key: "left"
                                    },
                                    next: {
                                        button: buttonNext,
                                        key: "right"
                                    },
                                    pagination: paginationHolder
                                });
                                break;
                        }
                    });
        })(jQuery);
    }
    // END: render
}

WillStrohl.Widgets.carouFredSel.inheritsFrom(DotNetNuke.UI.WebControls.Widgets.BaseWidget);
WillStrohl.Widgets.carouFredSel.registerClass("WillStrohl.Widgets.carouFredSel", DotNetNuke.UI.WebControls.Widgets.BaseWidget);
DotNetNuke.UI.WebControls.Widgets.renderWidgetType("WillStrohl.Widgets.carouFredSel");
// END: carouFredSel class
