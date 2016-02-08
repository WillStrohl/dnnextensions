"use strict";

$("body").attr("ng-app", "codeCampApp");

var codeCampApp = angular.module("codeCampApp", ["ngRoute", "ngAnimate", "ui.bootstrap", "ui.sortable", "angularMoment", "codeCampControllers"]);

var codeCampControllers = angular.module("codeCampControllers", []);

codeCampApp.config(["$routeProvider", 
	function ($routeProvider) {

	    //TODO: dynamically parse and/or replace _default with "templateFolder"
	    //TODO: add registrant list view, for printing badges by organizers
	    //TODO: add social networks to registration
	    //TODO: add image upload for event 
	    //TODO: add image upload for registration
	    //TODO: add missing registration fields
	    //TODO: add missing event fields
	    //TODO: add logic to only show approved sessions
	    //TODO: complete the agenda view
	    //TODO: add template editor view
	    //TODO: speakers page is completely blank when a code camp is first created and with no registrations
	    //TODO: ensure that show shirt size and other settings are observed in other views
	    //TODO: first load of Volunteers view by admin gets manage tasks, want to help message, and no tasks message (not registered yet)
	    //TODO: first load of Sessions view is blank, with filter (not registered yet)
        //TODO: first load of Speakers view is is blank (not registered yet)

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