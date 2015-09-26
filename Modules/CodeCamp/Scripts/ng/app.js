$("body").attr("ng-app", "codeCampApp");

angular
    .module("codeCampApp", [])
	.config(["$routeProvider", "$httpProvider",
		function ($routeProvider) {

		    $routeProvider
			.when("/create", {
			    templateUrl: "/DesktopModules/CodeCamp/Templates/create.html",
			    controller: "createController"
			})
			.when("/register", {
			    templateUrl: "/DesktopModules/CodeCamp/Templates/register.html",
			    controller: "registerController"
			})
			.when("/about", {
			    templateUrl: "/DesktopModules/CodeCamp/Templates/about.html",
			    controller: "aboutController"
			})
			.when("/agenda", {
			    templateUrl: "/DesktopModules/CodeCamp/Templates/agenda.html",
			    controller: "agendaController"
			})
			.when("/speakers", {
			    templateUrl: "/DesktopModules/CodeCamp/Templates/speakers.html",
			    controller: "speakersController"
			})
			.when("/speaker/:speakerId", {
			    templateUrl: "/DesktopModules/CodeCamp/Templates/speaker.html",
			    controller: "speakerController"
			})
			.when("/sessions", {
			    templateUrl: "/DesktopModules/CodeCamp/Templates/sessions.html",
			    controller: "speakersController"
			})
			.when("/session/:sessionId", {
			    templateUrl: "/DesktopModules/CodeCamp/Templates/session.html",
			    controller: "sessionController"
			})
			.otherwise({
			    redirectTo: "/about"
			});
		}]);