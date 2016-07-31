"use strict";

userGroupControllers.controller("recentlyUpdatedController", ["$scope", "$routeParams", "$location", "$http", "userGroupServiceFactory", function ($scope, $routeParams, $location, $http, userGroupServiceFactory) {
    
    var factory = userGroupServiceFactory;
    factory.init(moduleId, moduleName);

    $scope.userCanEdit = false;
    $scope.userIsLoggedIn = false;
    $scope.groupCount = 0;
    $scope.groupsToShow = false;
    $scope.groups = null;

    $scope.LoadData = function () {
        factory.callGetService("GetCurrentUserId")
        .then(function (response) {
            var fullResult = angular.fromJson(response);
            var serviceResponse = JSON.parse(fullResult.data);

            $scope.currentUserId = serviceResponse.Content;

            $scope.userIsLoggedIn = ($scope.currentUserId > -1);

            $scope.LoadGroups();

            LogErrors(serviceResponse.Errors);
        },
        function (data) {
            console.log("Unknown error occurred calling GetCurrentUserId");
            console.log(data);
        });
    }

    $scope.LoadGroups  = function () {
        factory.callGetService("GetRecentlyUpdatedGroups")
        .then(function (response) {
            var fullResult = angular.fromJson(response);
            var serviceResponse = JSON.parse(fullResult.data);

            $scope.groups = serviceResponse.Content;
            $scope.groupCount = $scope.groups.length;
            $scope.groupsToShow = ($scope.groupCount > 0);

            LogErrors(serviceResponse.Errors);
        },
        function (data) {
            console.log("Unknown error occurred calling GetRecentlyUpdatedGroups");
            console.log(data);
        });
    }

    $scope.goToPage = function (pageName) {
        $location.path(pageName);
    }

    $scope.LoadData();

}]);