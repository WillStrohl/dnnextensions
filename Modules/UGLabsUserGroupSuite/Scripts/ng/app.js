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
		.otherwise({
		    redirectTo: "/groups"
		});
	}]);