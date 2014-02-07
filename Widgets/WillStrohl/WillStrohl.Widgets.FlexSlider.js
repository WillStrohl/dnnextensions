/*
FlexSlider Widget
Widget Suite for DotNetNuke
Author - Armand Datema (http://www.2DNN.com)
Project Contributors - Will Strohl (http://www.WillStrohl.com), Mark Allan (http://www.dnngarden.com), Armand Datema (http://www.2DNN.com)
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
''' This widget uses the FlexSlider jQuery plugin to create a fully responsive slider.
''' Pass it the wrapper item and it will put all the children in a sliding or fadig ovement effect.
''' </summary>
''' <version>01.00.00</version>
''' <remarks>
''' </remarks>
''' <history>
''' [Armand Datema] - 20110911 - Created
''' </history>
''' -----------------------------------------------------------------------------

EXAMPLE:
<object id="wgtFlexSlider" codetype="dotnetnuke/client" codebase="WillStrohl.Widgets.FlexSlider" declare="declare">
</object>

<object id="wgtFlexSlider" codetype="dotnetnuke/client" codebase="WillStrohl.Widgets.FlexSlider" declare="declare">
<param name="wrapper" value=".FlexSlider" />
</object>
*/

////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// BEGIN: FlexSlider class                                                                              //
////////////////////////////////////////////////////////////////////////////////////////////////////////////////

// BEGIN: Namespace management
Type.registerNamespace("WillStrohl.Widgets");
// END: Namespace management

WillStrohl.Widgets.FlexSlider = function(widget) {
    WillStrohl.Widgets.FlexSlider.initializeBase(this, [widget]);
}

WillStrohl.Widgets.FlexSlider.prototype =
{
    // BEGIN: render
    render: function () {
        var widget = this._widget;

        (function ($) {
            // Default parameters
            var wrapper = ".flexslider";
            var animation = "fade";               //Select your animation type (fade/slide/show)
            var slideshow = true;                 //Should the slider animate automatically by default? (true/false)
            var slideshowSpeed = 7000;            //Set the speed of the slideshow cycling, in milliseconds
            var animationDuration = 500;          //Set the speed of animations, in milliseconds
            var directionNav = true;              //Create navigation for previous/next navigation? (true/false)
            var controlNav = true;                //Create navigation for paging control of each clide? (true/false)
            var keyboardNav = true;               //Allow for keyboard navigation using left/right keys (true/false)
            var touchSwipe = true;                //Touch swipe gestures for left/right slide navigation (true/false)
            var prevText = "Previous";            //Set the text for the "previous" directionNav item
            var nextText = "Next";                //Set the text for the "next" directionNav item
            var randomize = false;                //Randomize slide order on page load? (true/false)
            var slideToStart = 0;                 //The slide that the slider should start on. Array notation (0 = first slide)
            var pauseOnAction = true;              //Pause the slideshow when interacting with control elements, highly recommended. (true/false)
            var pauseOnHover = false;              //Pause the slideshow when hovering over slider, then resume when no longer hovering (true/false)
            var controlsContainer = "";            //Advanced property= Can declare which container the navigation elements should be appended too. 
            var manualControls = "";               //Advanced property= Can declare custom control navigation. 


            // Parse parameters
            $(widget).children().each(function () {
                if (this.name && this.value) {
                    var paramName = this.name.toLowerCase();
                    var paramValue = this.value;

                    switch (paramName) {
                        case "wrapper":
                            wrapper = paramValue;
                            break;
                        case "animation":
                            animation = paramValue;
                            break;
                        case "slideshow":
                            slideshow = paramValue;
                            break;
                        case "slideshowspeed":
                            slideshowSpeed = parseInt(paramValue);
                            break;
                        case "animationduration":
                            animationDuration = parseInt(paramValue);
                            break;
                        case "directionnav":
                            directionNav = paramValue;
                            break;
                        case "controlnav":
                            controlNav = paramValue;
                            break;
                        case "keyboardnav":
                            keyboardNav = paramValue;
                            break;
                        case "touchswipe":
                            touchSwipe = paramValue;
                            break;
                        case "prevtext":
                            prevText = paramValue;
                            break;
                        case "nexttext":
                            nextText = paramValue;
                            break;
                        case "randomize":
                            randomize = paramValue;
                            break;
                        case "slidetostart":
                            slideToStart = parseInt(paramValue);
                            break;
                        case "pauseonaction":
                            pauseOnAction = paramValue;
                            break;
                        case "pauseonhover":
                            pauseOnHover = paramValue;
                            break;
                        case "controlscontainer":
                            controlsContainer = paramValue;
                            break;
                        case "manualcontrols":
                            manualControls = paramValue;
                            break;
                    }
                }
            });

            // Get widget location
            var pageHost = document.location.host;
            var widgetHost = $dnn.baseResourcesUrl + "Widgets/User/WillStrohl/js/FlexSlider/";

            // Helper functions 

            function injectStylesheet(cssFile) {
                cssUrl = widgetHost + cssFile;
                jQuery("#Head").append("<link rel=\"stylesheet\" href=\"" + cssUrl + "\" type=\"text/css\" />");
            }

            // Set default values for original jQuery plugin
            var options = {
                animation: animation,
                slideshow: slideshow,
                slideShowSpeed: slideshowSpeed,
                animationDuration: animationDuration,
                directionNav: directionNav,
                controlNav: controlNav,
                keyboardNav: keyboardNav,
                touchSwipe: touchSwipe,
                prevText: prevText,
                nextText: nextText,
                randomize: randomize,
                slideToStart: slideToStart,
                pauseOnAction: pauseOnAction,
                pauseOnHover: pauseOnHover,
                controlsContainer: controlsContainer,
                manualControls: manualControls
            }

            // remove the <object> from DOM since that is not needed anymore
            jQuery(widget).remove();

            // Check if plugin is allready loaded
            if (jQuery.fn.FlexSlider) {
                // Plugin allready loaded no need to load script just call the plugin
                jQuery(wrapper).FlexSlider(options);
            } else {
                // Plugin not loaded so first load the plugin and then call it
                injectStylesheet("flexslider.css");
                jQuery.getScript(widgetHost + "jquery.flexslider-min.js", function () {
                    jQuery(wrapper).flexslider(options);
                });
            }

        })(jQuery);
    }
    // END: render
}

WillStrohl.Widgets.FlexSlider.inheritsFrom(DotNetNuke.UI.WebControls.Widgets.BaseWidget);
WillStrohl.Widgets.FlexSlider.registerClass("WillStrohl.Widgets.FlexSlider", DotNetNuke.UI.WebControls.Widgets.BaseWidget);
DotNetNuke.UI.WebControls.Widgets.renderWidgetType("WillStrohl.Widgets.FlexSlider");
// END: FlexSlider class
