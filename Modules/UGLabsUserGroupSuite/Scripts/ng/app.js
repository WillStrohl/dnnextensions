"use strict";

$("body").attr("ng-app", "userGroupApp");

var userGroupApp = angular.module("userGroupApp", ["ngRoute", "ngAnimate", "ui.bootstrap", "ui.sortable", "angularMoment", "flow", "userGroupControllers"]);

var userGroupControllers = angular.module("userGroupControllers", []);

userGroupApp.config(["$routeProvider", 
	function ($routeProvider) {

		$routeProvider
		.when("/groups", {
			templateUrl: "/DesktopModules/UserGroupSuite/Templates/_default/groups.html",
			controller: "groupsController"
		})
		.when("/upcoming-meetings", {
		    templateUrl: "/DesktopModules/UserGroupSuite/Templates/_default/upcomingMeetings.html",
		    controller: "upcomingMeetingsController"
		})
		.when("/recently-updated", {
		    templateUrl: "/DesktopModules/UserGroupSuite/Templates/_default/recentlyUpdated.html",
		    controller: "recentlyUpdatedController"
		})
		.when("/in-my-area", {
		    templateUrl: "/DesktopModules/UserGroupSuite/Templates/_default/inMyArea.html",
		    controller: "inMyAreaController"
		})
		.when("/search", {
		    templateUrl: "/DesktopModules/UserGroupSuite/Templates/_default/search.html",
		    controller: "searchController"
		})
		.otherwise({
		    redirectTo: "/groups"
		});
	}]);