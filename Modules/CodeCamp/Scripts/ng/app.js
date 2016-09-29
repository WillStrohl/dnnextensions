"use strict";

$("body").attr("ng-app", "codeCampApp");

var codeCampApp = angular.module("codeCampApp", ["ngRoute", "ngAnimate", "ui.bootstrap", "ui.sortable", "angularMoment", "flow", "codeCampControllers"]);

var codeCampControllers = angular.module("codeCampControllers", []);

codeCampApp.config(["$routeProvider", 
	function ($routeProvider) {

		$routeProvider
		.when("/update", {
			templateUrl: "/DesktopModules/CodeCamp/Templates/_default/update.html",
			controller: "eventController"
		})
		.when("/register", {
			templateUrl: "/DesktopModules/CodeCamp/Templates/_default/register.html",
			controller: "registerController"
		})
		.when("/about", {
			templateUrl: "/DesktopModules/CodeCamp/Templates/_default/about.html",
			controller: "aboutController"
		})
		.when("/agenda", {
			templateUrl: "/DesktopModules/CodeCamp/Templates/_default/agenda.html",
			controller: "agendaController"
		})
		.when("/rooms", {
		    templateUrl: "/DesktopModules/CodeCamp/Templates/_default/rooms.html",
		    controller: "roomsController"
		})
		.when("/speakers", {
			templateUrl: "/DesktopModules/CodeCamp/Templates/_default/speakers.html",
			controller: "speakersController"
		})
		.when("/speakers/:speakerId/:speakerName", {
			templateUrl: "/DesktopModules/CodeCamp/Templates/_default/speaker.html",
			controller: "speakerController"
		})
		.when("/sessions", {
			templateUrl: "/DesktopModules/CodeCamp/Templates/_default/sessions.html",
			controller: "sessionsController"
		})
		.when("/sessions/:sessionId", {
			templateUrl: "/DesktopModules/CodeCamp/Templates/_default/session.html",
			controller: "sessionController"
		})
		.when("/timeslots", {
		    templateUrl: "/DesktopModules/CodeCamp/Templates/_default/timeslots.html",
		    controller: "timeSlotsController"
		})
		.when("/tracks", {
			templateUrl: "/DesktopModules/CodeCamp/Templates/_default/tracks.html",
			controller: "tracksController"
		})
		.when("/tracks/:trackId/:trackName", {
			templateUrl: "/DesktopModules/CodeCamp/Templates/_default/track.html",
			controller: "trackController"
		})
		.when("/volunteers", {
			templateUrl: "/DesktopModules/CodeCamp/Templates/_default/volunteers.html",
			controller: "volunteersController"
		})
		.otherwise({
			redirectTo: "/about"
		});
	}]);

codeCampApp.config(["flowFactoryProvider", function (flowFactoryProvider) {
    flowFactoryProvider.defaults = {
        target: "/upload",
        permanentErrors: [404, 500, 501],
        maxChunkRetries: 1,
        chunkRetryInterval: 5000,
        simultaneousUploads: 4
    };
    flowFactoryProvider.on("catchAll", function (event) {
        console.log("catchAll", arguments);
    });
}]);