"use strict";

$("body").attr("ng-app", "userGroupApp");

var userGroupApp = angular.module("userGroupApp", ["ngRoute", "ngAnimate", "ui.bootstrap", "ui.sortable", "angularMoment", "flow", "userGroupControllers"]);

var userGroupControllers = angular.module("userGroupControllers", []);

userGroupApp.config(["$routeProvider", 
	function ($routeProvider) {

	    var fullTemplatePath = templatePath + "/Templates/" + templateFolder + "/";

		$routeProvider
		.when("/groups", {
		    templateUrl: fullTemplatePath + "groups.html",
			controller: "groupsController"
		})
		.when("/upcoming-meetings", {
		    templateUrl: fullTemplatePath + "upcomingMeetings.html",
		    controller: "upcomingMeetingsController"
		})
		.when("/recently-updated", {
		    templateUrl: fullTemplatePath + "recentlyUpdated.html",
		    controller: "recentlyUpdatedController"
		})
		.when("/in-my-area", {
		    templateUrl: fullTemplatePath + "inMyArea.html",
		    controller: "inMyAreaController"
		})
		.when("/find-groups", {
		    templateUrl: fullTemplatePath + "search.html",
		    controller: "searchController"
		})
		.otherwise({
		    redirectTo: "/groups"
		});
	}]);