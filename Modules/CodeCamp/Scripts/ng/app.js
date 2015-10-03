"use strict";

$("body").attr("ng-app", "codeCampApp");

var codeCampApp = angular.module("codeCampApp", ["ngRoute", "codeCampControllers"]);

var codeCampControllers = angular.module("codeCampControllers", []);

codeCampApp.config(["$routeProvider", 
		function ($routeProvider) {

            //TODO: dynamically parse and/or replace _default with "templateFolder"

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
			.when("/speakers", {
			    templateUrl: "/DesktopModules/CodeCamp/Templates/_default/speakers.html",
			    controller: "speakersController"
			})
			.when("/speaker/:speakerId", {
			    templateUrl: "/DesktopModules/CodeCamp/Templates/_default/speaker.html",
			    controller: "speakerController"
			})
			.when("/sessions", {
			    templateUrl: "/DesktopModules/CodeCamp/Templates/_default/sessions.html",
			    controller: "speakersController"
			})
			.when("/session/:sessionId", {
			    templateUrl: "/DesktopModules/CodeCamp/Templates/_default/session.html",
			    controller: "sessionController"
			})
			.otherwise({
			    redirectTo: "/about"
			});
		}]);

//codeCampApp.run(["$rootScope", function ($rootScope) {
//        // see what's going on when the route tries to change
//        $rootScope.$on("$routeChangeStart", function (event, next, current) {
//            // next is an object that is the route that we are starting to go to
//            // current is an object that is the route where we are currently
//            var currentPath = current.originalPath;
//            var nextPath = next.originalPath;

//            console.log("Starting to leave %s to go to %s", currentPath, nextPath);
//        });
//    }
//]);