/*
Technorati Info Widget
Widget Suite for DotNetNuke
Author - Will Strohl (http://www.WillStrohl.com)
Project Contributors - Will Strohl (http://www.WillStrohl.com), Mark Allan (http://www.dnngarden.com), Armand Datema (http://www.schwingsoft.com)
Support - http://dnnwidgets.codeplex.com
Copyright (c) 2009-2011 by Will Strohl
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
''' This widget script imports Technorati information widgets into a DNN 5+ page.
''' </summary>
''' <remarks>
''' The following Technorati widgets are supported:
''' Horizontal800px
''' Horizontal650px
''' Vertical400px
''' Vertical200px
''' News
''' TopSearches
''' TopTags
''' YourTopTags
''' 
''' Implement in your skins like this:
''' <object id="AnyName" codetype="dotnetnuke/client" codebase="WillStrohl.Widgets.TechnoratiInfoWidget" declare="declare">
''' <param name="widgetType" value="Vertical400px" />
''' </object>
''' </remarks>
''' <history>
''' [WillStrohl] - 20090111 - Created
''' </history>
''' -----------------------------------------------------------------------------
*/

/* BEGIN: Namespace management */
Type.registerNamespace("WillStrohl.Widgets");
/* END: Namespace management */

WillStrohl.Widgets.TechnoratiInfoWidget = function(widget) {
    WillStrohl.Widgets.TechnoratiInfoWidget.initializeBase(this, [widget]);
}

WillStrohl.Widgets.TechnoratiInfoWidget.prototype =
{
    /* BEGIN: render */
    render:
        function() {
            var params = this._widget.childNodes;
            if (params != null) {
                var widgetType = 'Vertical400px';
                var blogUrl = '';
                for (var p = 0; p < params.length; p++) {
                    try {
                        var paramName = params[p].name.toLowerCase();
                        switch (paramName) {
                            case 'widgetType': widgetType = params[p].value; break;
                            case 'blogUrl': blogUrl = params[p].value; break;
                        }
                    }
                    catch (e) {
                    }
                }
            }

            if ((widgetType == 'YourTopTags' && blogUrl != '') || (widgetType != 'YourTopTags')) {

                var div = document.createElement('div');
                var tScript = document.createElement('script');
                var tAnchor = document.createElement('a');
                tScript.setAttribute('src', 'http://widgets.technorati.com/t.js');
                tScript.setAttribute('type', 'text/javascript');
                tScript.setAttribute('language', 'JavaScript');
                div.appendChild(tScript);

                switch (widgetType) {
                    case 'Horizontal800px':
                        // <script src="http://widgets.technorati.com/t.js" type="text/javascript"> </script> 
                        // <a href="http://technorati.com/?sub=tr_searches-horizontal-ticker_t_js" class="tr_searches-horizontal-ticker_t_js" style="color:#4261DF">View technorati.com</a>
                        tAnchor.setAttribute('href', 'http://technorati.com/?sub=tr_searches-horizontal-ticker_t_js');
                        tAnchor.setAttribute('class', 'tr_searches-horizontal-ticker_t_js');
                        tAnchor.setAttribute('style', 'color:#4261DF');
                        tAnchor.innerHTML = 'View technorati.com';
                        div.appendChild(tAnchor);
                    case 'Horizontal650px':
                        // <script src="http://widgets.technorati.com/t.js" type="text/javascript"> </script>
                        // <a href="http://technorati.com/?sub=tr_searches-small-horizontal-ticker_t_js" class="tr_searches-small-horizontal-ticker_t_js" style="color:#4261DF">View technorati.com</a>
                        tAnchor.setAttribute('href', 'http://technorati.com/?sub=tr_searches-small-horizontal-ticker_t_js');
                        tAnchor.setAttribute('class', 'tr_searches-small-horizontal-ticker_t_js');
                        tAnchor.setAttribute('style', 'color:#4261DF');
                        tAnchor.innerHTML = 'View technorati.com';
                        div.appendChild(tAnchor);
                    case 'Vertical400px':
                        // <script src="http://widgets.technorati.com/t.js" type="text/javascript"> </script>
                        // <a href="http://technorati.com/?sub=tr_searches-vertical-ticker_t_js" class="tr_searches-vertical-ticker_t_js" style="color:#4261DF">View technorati.com</a>
                        tAnchor.setAttribute('href', 'http://technorati.com/?sub=tr_searches-vertical-ticker_t_js');
                        tAnchor.setAttribute('class', 'tr_searches-vertical-ticker_t_js');
                        tAnchor.setAttribute('style', 'color:#4261DF');
                        tAnchor.innerHTML = 'View technorati.com';
                        div.appendChild(tAnchor);
                    case 'Vertical200px':
                        // <script src="http://widgets.technorati.com/t.js" type="text/javascript"> </script>
                        // <a href="http://technorati.com/?sub=tr_searches-small-vertical-ticker_t_js" class="tr_searches-small-vertical-ticker_t_js" style="color:#4261DF">View technorati.com</a>
                        tAnchor.setAttribute('href', 'http://technorati.com/?sub=tr_searches-small-vertical-ticker_t_js');
                        tAnchor.setAttribute('class', 'tr_searches-small-vertical-ticker_t_js');
                        tAnchor.setAttribute('style', 'color:#4261DF');
                        tAnchor.innerHTML = 'View technorati.com';
                        div.appendChild(tAnchor);
                    case 'News':
                        // <script src="http://widgets.technorati.com/t.js" type="text/javascript"> </script>
                        // <a href="http://technorati.com/?sub=tr_top-news_t_js" class="tr_top-news_t_js" style="color:#4261DF">View top news</a>
                        tAnchor.setAttribute('href', 'http://technorati.com/?sub=tr_top-news_t_js');
                        tAnchor.setAttribute('class', 'tr_top-news_t_js');
                        tAnchor.setAttribute('style', 'color:#4261DF');
                        tAnchor.innerHTML = 'View technorati.com';
                        div.appendChild(tAnchor);
                    case 'TopSearches':
                        // <script src="http://widgets.technorati.com/t.js" type="text/javascript"> </script>
                        // <a href="http://technorati.com/pop/?sub=tr_top-searches_t_ns" class="tr_top-searches_t_js" style="color:#4261DF">Technorati Top Searches</a>
                        tAnchor.setAttribute('href', 'http://technorati.com/pop/?sub=tr_top-searches_t_ns');
                        tAnchor.setAttribute('class', 'tr_top-searches_t_js');
                        tAnchor.setAttribute('style', 'color:#4261DF');
                        tAnchor.innerHTML = 'View technorati.com';
                        div.appendChild(tAnchor);
                    case 'TopTags':
                        // <script src="http://widgets.technorati.com/t.js" type="text/javascript"> </script>
                        // <a href="http://technorati.com/tag/?sub=tr_global-tagcloud_t_ns" class="tr_global-tagcloud_t_js" style="color:#4261DF">Technorati Top Tags</a>
                        tAnchor.setAttribute('href', 'http://technorati.com/tag/?sub=tr_global-tagcloud_t_ns');
                        tAnchor.setAttribute('class', 'tr_global-tagcloud_t_js');
                        tAnchor.setAttribute('style', 'color:#4261DF');
                        tAnchor.innerHTML = 'View technorati.com';
                        div.appendChild(tAnchor);
                    case 'YourTopTags':
                        // <script src="http://widgets.technorati.com/t.js" type="text/javascript"> </script>
                        // <a href="http://technorati.com/blogs/{URL}?sub=tr_tagcloud_t_ns" class="tr_tagcloud_t_js" style="color:#4261DF">View blog top tags</a>
                        tAnchor.setAttribute('href', 'http://technorati.com/blogs/' + blogUrl + '?sub=tr_tagcloud_t_ns');
                        tAnchor.setAttribute('class', 'tr_tagcloud_t_js');
                        tAnchor.setAttribute('style', 'color:#4261DF');
                        tAnchor.innerHTML = 'View blog top tags';
                        div.appendChild(tAnchor);
                }

                //            var tTech = document.createElement('script');
                //            tTech.appendChild('type', 'text/javascript');
                //            tTech.appendChild('language', 'JavaScript');
                //            tTech.innerHTML = 'Technorati.loadFunctions();';
                //            div.appendChild(tTech);

                WillStrohl.Widgets.TechnoratiInfoWidget.callBaseMethod(this, 'render', [div]);
            }
        }
    /* END: render */
}

WillStrohl.Widgets.TechnoratiInfoWidget.inheritsFrom(DotNetNuke.UI.WebControls.Widgets.BaseWidget);
WillStrohl.Widgets.TechnoratiInfoWidget.registerClass('WillStrohl.Widgets.TechnoratiInfoWidget', DotNetNuke.UI.WebControls.Widgets.BaseWidget);
DotNetNuke.UI.WebControls.Widgets.renderWidgetType('WillStrohl.Widgets.TechnoratiInfoWidget');
/* END: WillStrohlWidget class */
