"use strict";

userGroupControllers.controller("groupsController", ["$scope", "$routeParams", "$location", "$http", "userGroupServiceFactory", function ($scope, $routeParams, $location, $http, userGroupServiceFactory) {
    
    var factory = userGroupServiceFactory;
    factory.init(moduleId, moduleName);

    $scope.userCanEdit = false;
    $scope.userIsLoggedIn = false;
    $scope.userHasGroups = false;

    $scope.LoadData = function() {
        factory.callGetService("GetCurrentUserId")
        .then(function (response) {
            var fullResult = angular.fromJson(response);
            var serviceResponse = JSON.parse(fullResult.data);

            $scope.currentUserId = serviceResponse.Content;

            $scope.userIsLoggedIn = ($scope.currentUserId > -1);

            $scope.GroupCheck();

            LogErrors(serviceResponse.Errors);
        },
        function (data) {
            console.log("Unknown error occurred calling GetCurrentUserId");
            console.log(data);
        });
    }

    $scope.GroupCheck = function () {
        factory.callGetService("UserHasGroups")
        .then(function (response) {
            var fullResult = angular.fromJson(response);
            var serviceResponse = JSON.parse(fullResult.data);

            $scope.userHasGroups = (serviceResponse.Content == "Success");

            if ($scope.userHasGroups) {
                //$scope.LoadMembership();
            }

            LogErrors(serviceResponse.Errors);
        },
        function (data) {
            console.log("Unknown error occurred calling UserHasGroups");
            console.log(data);
        });
    }

    $scope.LoadMembership = function () {
        factory.callGetService("GetGroupsByUser")
        .then(function (response) {
            var fullResult = angular.fromJson(response);
            var serviceResponse = JSON.parse(fullResult.data);

            $scope.userGroups = serviceResponse.Content;

            LogErrors(serviceResponse.Errors);
        },
        function (data) {
            console.log("Unknown error occurred calling GetGroupsByUser");
            console.log(data);
        });
    }

    $scope.goToPage = function(pageName) {
        $location.path(pageName);
    }

    $scope.LoadData();

}]);