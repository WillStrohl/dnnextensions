"use strict";

$("body").attr("ng-app", "userGroupApp");

var userGroupApp = angular.module("userGroupApp", ["ngRoute", "ngCookies", "ngAnimate", "ui.bootstrap", "ui.sortable", "ngTagsInput", "angularMoment", "flow", "userGroupControllers"]);

var userGroupControllers = angular.module("userGroupControllers", []);

var fullTemplatePath = templatePath + "/Templates/" + templateFolder + "/";

userGroupApp.config(["$routeProvider", 
	function ($routeProvider) {

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
		.when("/edit-group", {
		    templateUrl: fullTemplatePath + "editGroup.html",
		    controller: "editGroupController"
		})
		.when("/edit-group/:groupID", {
		    templateUrl: fullTemplatePath + "editGroup.html",
		    controller: "editGroupController"
		})
		.otherwise({
		    redirectTo: "/groups"
		});
	}]);