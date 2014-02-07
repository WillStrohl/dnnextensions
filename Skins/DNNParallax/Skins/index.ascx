<%@ Control language="vb" AutoEventWireup="false" Explicit="True" Inherits="DotNetNuke.UI.Skins.Skin" %>
<%@ Register TagPrefix="dnn" TagName="NAV" Src="~/Admin/Skins/Nav.ascx" %>
<%@ Register TagPrefix="dnn" TagName="USER" Src="~/Admin/Skins/User.ascx" %>
<%@ Register TagPrefix="dnn" TagName="LOGIN" Src="~/Admin/Skins/Login.ascx" %>
<%@ Register TagPrefix="dnn" TagName="LEFTMENU" Src="~/Admin/Skins/LeftMenu.ascx" %>
<%@ Register TagPrefix="dnn" TagName="STYLES" Src="~/Admin/Skins/Styles.ascx" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.DDRMenu.TemplateEngine" Assembly="DotNetNuke.Web.DDRMenu" %>
<%@ Register TagPrefix="dnn" TagName="MENU" Src="~/DesktopModules/DDRMenu/Menu.ascx" %>
<%@ Register TagPrefix="dnn" TagName="Meta" Src="~/Admin/Skins/Meta.ascx" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.Client.ClientResourceManagement" Assembly="DotNetNuke.Web.Client" %>

<dnn:STYLES runat="server" ID="Signika" Name="Signika" StyleSheet="http://fonts.googleapis.com/css?family=Signika" UseSkinPath="false" />
<dnn:STYLES runat="server" ID="Overlock" Name="Overlock" StyleSheet="http://fonts.googleapis.com/css?family=Overlock:400,400italic,700,700italic" UseSkinPath="false" />

<dnn:Meta runat="server" Name="viewport" Content="width=device-width,initial-scale=1" />

<script type="text/javascript" src="<%=SkinPath%>/scripts/jquery.parallax-1.1.3.js"></script>
<script type="text/javascript" src="<%=SkinPath%>/scripts/jquery.localscroll-1.2.7-min.js"></script>
<script type="text/javascript" src="<%=SkinPath%>/scripts/jquery.scrollTo-1.4.2-min.js"></script>
<script type="text/javascript" src="<%=SkinPath%>/scripts/jquery.inview.js"></script>
<script type="text/javascript">
	$(document).ready(function(){
		$('#navv').localScroll(800);
		/*
		.parallax(xPosition, speedFactor, outerHeight) options:
		
		xPosition - Horizontal position of the element
		inertia - speed to move relative to vertical scroll. Example: 0.1 is one tenth the speed of scrolling, 2 is twice the speed of scrolling
		outerHeight (true/false) - Whether or not jQuery should use it's outerHeight option to determine when a section is in the viewport
		*/
		$('#intro').parallax("50%", 0.1);
		$('#second').parallax("50%", 0.1);
		$('.bg').parallax("45%", 0.4);
		$('#third').parallax("50%", 0.3);
		$('#fourth').parallax("50%", 0.2);
		$('#fifth').parallax("50%", 0.1);
	}) 
</script>


<ul id="navv">
	<li><a href="#intro" title="Next Section"><img src="<%=SkinPath%>/images/dot.png" alt="Link" /></a></li>
    <li><a href="#second" title="Next Section"><img src="<%=SkinPath%>/images/dot.png" alt="Link" /></a></li>
    <li><a href="#third" title="Next Section"><img src="<%=SkinPath%>/images/dot.png" alt="Link" /></a></li>
    <li><a href="#fourth" title="Next Section"><img src="<%=SkinPath%>/images/dot.png" alt="Link" /></a></li>
    <li><a href="#fifth" title="Next Section"><img src="<%=SkinPath%>/images/dot.png" alt="Link" /></a></li>
</ul>
<div class="loginLinks">
				<dnn:USER ID="dnnUser" runat="server" LegacyMode="false" />
				<dnn:LOGIN ID="dnnLogin" CssClass="LoginLink" runat="server" LegacyMode="false" />
			</div>
<div id="header">
	<div class="story">
        <div id="logo"><a href="/" title=""><img src="<%=SkinPath%>/images/greenlogo2.png" alt="Go to Homepage" /></a>
        </div>
    </div>
    
    <div class="navDiv" style="float:right;"><dnn:MENU ID="menuTop" MenuStyle="tidyMenu" runat="server" /></div> <!--.story-->
</div> <!--#header-->

<div id="intro">
	<div class="story">
    	<div class="float-left" id="ContentPane" runat="server" >
		</div>
    </div> <!--.story-->
</div> <!--#intro-->

<div id="second">
	<div class="story"><div class="bg"></div>
    	<div class="float-right" id="ContentPane2" runat="server">
            
        </div>
    </div> <!--.story-->
    
</div> <!--#second-->

<div id="third">
	<div class="story">
    	<div class="float-left" id="ContentPane3" runat="server">
        	
        </div>
    </div> <!--.story-->
</div> <!--#third-->

<div id="fourth">
	<div class="story"><div class="bg"></div>
    	<div class="float-left" id="ContentPane4" runat="server">
            
        </div>
    </div> <!--.story-->
</div> <!--#fourth-->

<div id="fifth">
	<div class="story">
    	<div class="middle" id="ContentPane5" runat="server">
		
        </div>
    </div> <!--.story-->
</div> <!--#fifth-->


