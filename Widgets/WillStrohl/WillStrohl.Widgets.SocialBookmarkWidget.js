/*
Social Bookmarks Widget
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
''' This widget script adds social bookmarks into a DNN 5+ page.
''' </summary>
''' <remarks>
''' Implement in your skins like this:
''' <object id="AnyName" codetype="dotnetnuke/client" codebase="WillStrohl.Widgets.SocialBookmarkWidget" declare="declare">
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

WillStrohl.Widgets.SocialBookmarkWidget = function (widget) {
    WillStrohl.Widgets.SocialBookmarkWidget.initializeBase(this, [widget]);
}

WillStrohl.Widgets.SocialBookmarkWidget.prototype =
{
    /* BEGIN: render */
    render:
        function () {
            var params = this._widget.childNodes;
            if (params != null) {
                var baseUrl = '';
                var setStyle = 'false';
                var valWidth = '500px';
                var valHeight = '35px';
                var sbDigg = 'false';
                var sbDelicious = 'false';
                var sbDotNetKicks = 'false';
                var sbFacebook = 'false';
                var sbStumbleUpon = 'false';
                var debugEnabled = 'false';

                for (var p = 0; p < params.length; p++) {
                    try {
                        var paramName = params[p].name.toLowerCase();
                        switch (paramName) {
                            case 'baseurl': baseUrl = params[p].value; break;
                            case 'width': valWidth = params[p].value; break;
                            case 'height': valHeight = params[p].value; break;
                            case 'setstyle': setStyle = params[p].value; break;
                            case 'showdigg': sbDigg = params[p].value; break;
                            case 'showdelicious': sbDelicious = params[p].value; break;
                            case 'showdotnetkicks': sbDotNetKicks = params[p].value; break;
                            case 'showfacebook': sbFacebook = params[p].value; break;
                            case 'showstumbleupon': sbStumbleUpon = params[p].value; break;
                            case 'debug': debugEnabled = params[p].value; break;
                        }
                    }
                    catch (e) {
                        //alert('An Error Occurred: ' + e);
                    }
                }
            }

            /* INITIATE THE DEBUGGER */
            var runDebug = false;
            if (debugEnabled == 'true') {
                runDebug = true;
            }

            if (runDebug) {
                if ($('#DebugConsole').length == 0) $('body').append('<div id="DebugConsole" class="DebugConsole"></div>');
                $DEBUGLINE('<span class="Head">Widget Suite: SocialBookmarks Debug Report</span><br />');
                $DEBUGLINE('<span class="SubHead">Parameter Values:</span>');
                $DEBUGLINE('baseUrl = ' + baseUrl);
                $DEBUGLINE('width = ' + valWidth);
                $DEBUGLINE('height = ' + valHeight);
                $DEBUGLINE('setStyle = ' + setStyle);
                $DEBUGLINE('showDigg = ' + sbDigg);
                $DEBUGLINE('showDelicious = ' + sbDelicious);
                $DEBUGLINE('showDotNetKicks = ' + sbDotNetKicks);
                $DEBUGLINE('showFacebook = ' + sbFacebook);
                $DEBUGLINE('showStumbleUpon = ' + sbStumbleUpon);
                $DEBUGLINE('debug = ' + debugEnabled);
                $DEBUGLINE('<br /><span class="SubHead">Activity Log:</span>');
            }

            try {
                if (baseUrl == '') baseUrl = location.hostname;

                if (runDebug) $DEBUGLINE('baseUrl value is: ' + baseUrl);

                if (baseUrl != '') {
                    if (baseUrl.indexOf('http') == -1) {
                        var httpHost = (("https:" == document.location.protocol) ? 'https:/' : 'http:/');
                        baseUrl = httpHost + '/' + baseUrl;
                        if (runDebug) $DEBUGLINE('baseUrl after parsing for SSL is: ' + baseUrl);
                    }
                    baseUrl = baseUrl + '/Resources/Widgets/User/WillStrohl/';

                    var div = document.createElement('div');
                    div.setAttribute('class', 'WillStrohlSBWrapper');

                    /*
					
                    DIGG
					
                    */

                    if (sbDigg == 'true') {
                        div.innerHTML = div.innerHTML + '&nbsp;';

                        var aDigg = document.createElement('a');
                        aDigg.setAttribute('href', 'http://digg.com/submit?url=' + document.location + '&title=' + document.title);
                        aDigg.setAttribute('target', '_blank');

                        var imgDigg = document.createElement('img');
                        imgDigg.setAttribute('src', 'http://digg.com/img/badges/16x16-digg-guy.gif');
                        imgDigg.setAttribute('alt', 'Digg this');
                        imgDigg.setAttribute('border', '0');
                        imgDigg.setAttribute('style', 'width:16px;width:16px;');
                        imgDigg.setAttribute('class', 'WillStrohlSBImage');
                        aDigg.appendChild(imgDigg);

                        div.appendChild(aDigg);
                        div.innerHTML = div.innerHTML + '&nbsp;';
                    }

                    /*
					
                    DELICIOUS
					
                    */

                    if (sbDelicious == 'true') {
                        // <img src="http://static.delicious.com/img/delicious.small.gif" height="10" width="10" alt="Delicious" />
                        // <a href="http://delicious.com/save" onclick="window.open('http://delicious.com/save?v=5&amp;noui&amp;jump=close&amp;url='+encodeURIComponent(location.href)+'&amp;title='+encodeURIComponent(document.title), 'delicious','toolbar=no,width=550,height=550'); return false;"> Bookmark this on Delicious</a>
                        div.innerHTML = div.innerHTML + '&nbsp;';

                        var aDelicious = document.createElement('a');
                        aDelicious.setAttribute('href', 'http://delicious.com/save');
                        aDelicious.setAttribute('onclick', 'window.open(\'http://delicious.com/save?v=5&amp;noui&amp;jump=close&amp;url=\'+encodeURIComponent(location.href)+\'&amp;title=\'+encodeURIComponent(document.title), \'delicious\',\'toolbar=no,width=550,height=550\'); return false;');

                        var imgDelicious = document.createElement('img');
                        imgDelicious.setAttribute('src', 'http://static.delicious.com/img/delicious.small.gif');
                        imgDelicious.setAttribute('alt', 'Delicious');
                        imgDelicious.setAttribute('border', '0');
                        imgDelicious.setAttribute('style', 'width:10px;height:10px;border:none;');
                        imgDelicious.setAttribute('class', 'WillStrohlSBImage');
                        aDelicious.appendChild(imgDelicious);

                        div.appendChild(aDelicious);
                        div.innerHTML = div.innerHTML + '&nbsp;';
                    }

                    /*
					
                    DOTNETKICKS
					
                    */

                    if (sbDotNetKicks == 'true') {
                        div.innerHTML = div.innerHTML + '&nbsp;';

                        var aDnk = document.createElement('a');
                        aDnk.setAttribute('href', 'http://www.dotnetkicks.com/kick/?url=' + document.location + '&title=' + document.title + '&r=Will+Strohl');
                        aDnk.setAttribute('target', '_blank');

                        var imgDnk = document.createElement('img');
                        imgDnk.setAttribute('src', 'http://www.dotnetkicks.com/Services/Images/KickItImageGenerator.ashx?url=' + document.location);
                        imgDnk.setAttribute('alt', 'DotNetKicks');
                        imgDnk.setAttribute('border', '0');
                        imgDnk.setAttribute('style', 'border:none;');
                        imgDnk.setAttribute('class', 'WillStrohlSBImage');
                        aDnk.appendChild(imgDnk);

                        div.appendChild(aDnk);
                        div.innerHTML = div.innerHTML + '&nbsp;';
                    }

                    /*
					
                    FACEBOOK
					
                    */

                    if (sbFacebook == 'true') {
                        div.innerHTML = div.innerHTML + '&nbsp;';

                        var aFacebook = document.createElement('a');
                        aFacebook.setAttribute('href', 'http://www.facebook.com/sharer.php?u=' + document.location + '&t=' + document.title);
                        aFacebook.setAttribute('target', '_blank');

                        var imgFacebook = document.createElement('img');
                        imgFacebook.setAttribute('src', baseUrl + 'images/i_facebook.gif');
                        imgFacebook.setAttribute('alt', 'Facebook');
                        imgFacebook.setAttribute('border', '0');
                        imgFacebook.setAttribute('style', 'width:16px;height:16px;');
                        imgFacebook.setAttribute('class', 'WillStrohlSBImage');
                        aFacebook.appendChild(imgFacebook);

                        div.appendChild(aFacebook);
                        div.innerHTML = div.innerHTML + '&nbsp;';
                    }

                    /*
					
                    STUMBLEUPON
					
                    */

                    if (sbStumbleUpon == 'true') {
                        // <a href="http://www.stumbleupon.com/submit?url=http%3A%2F%2Fwww.yoursite.com%2Farticle.php%26title%3DThe%2BArticle%2BTitle"> 
                        // <img border=0 src="http://cdn.stumble-upon.com/images/16x16_su_round.gif" alt=""> Stumble It!</a>
                        div.innerHTML = div.innerHTML + '&nbsp;';

                        var aStumbleUpon = document.createElement('a');
                        aStumbleUpon.setAttribute('href', 'http://www.stumbleupon.com/submit?url=' + document.location + '&=title=' + document.title);
                        aStumbleUpon.setAttribute('target', '_blank');

                        var imgStumbleUpon = document.createElement('img');
                        imgStumbleUpon.setAttribute('src', 'http://cdn.stumble-upon.com/images/16x16_su_round.gif');
                        imgStumbleUpon.setAttribute('alt', 'Facebook');
                        imgStumbleUpon.setAttribute('border', '0');
                        imgStumbleUpon.setAttribute('style', 'width:16px;height:16px;');
                        imgStumbleUpon.setAttribute('class', 'WillStrohlSBImage');
                        aStumbleUpon.appendChild(imgStumbleUpon);

                        div.appendChild(aStumbleUpon);
                        div.innerHTML = div.innerHTML + '&nbsp;';
                    }

                    WillStrohl.Widgets.SocialBookmarkWidget.callBaseMethod(this, 'render', [div]);

                    if (setStyle != 'false') {
                        $('.WillStrohlSBWrapper').attr('style', 'width:' + valWidth + ';height:' + valHeight + ';display:block;');
                        // possible additions to the above styles:  border:1px solid #000000;background-color:#ffffff;vertical-align:middle;margin-top:auto;margin-bottom:auto;
                        $('.WillStrohlSBImage').css('border', '0px solid #ffffff');
                    }

                    if (runDebug) $DEBUGLINE('<br /><span class="NormalRed">Widget Suite: SocialBookmarks Debug Report Complete</span>');
                }
            }
            catch (e) {
                alert('An Error Occurred: ' + e);
            }

        }
    /* END: render */
}

WillStrohl.Widgets.SocialBookmarkWidget.inheritsFrom(DotNetNuke.UI.WebControls.Widgets.BaseWidget);
WillStrohl.Widgets.SocialBookmarkWidget.registerClass('WillStrohl.Widgets.SocialBookmarkWidget', DotNetNuke.UI.WebControls.Widgets.BaseWidget);
DotNetNuke.UI.WebControls.Widgets.renderWidgetType('WillStrohl.Widgets.SocialBookmarkWidget');
/* END: WillStrohlWidget class */
