"use strict";

userGroupControllers.controller("upcomingMeetingsController", ["$scope", "$routeParams", "$location", "$http", "userGroupServiceFactory", function ($scope, $routeParams, $location, $http, userGroupServiceFactory) {
    
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

            $scope.MeetingCheck();

            LogErrors(serviceResponse.Errors);
        },
        function (data) {
            console.log("Unknown error occurred calling GetCurrentUserId");
            console.log(data);
        });
    }

    $scope.MeetingCheck = function () {
        factory.callGetService("GetUpcomingMeetingCountForGroups")
        .then(function (response) {
            var fullResult = angular.fromJson(response);
            var serviceResponse = JSON.parse(fullResult.data);

            $scope.groupCount = serviceResponse.Content;
            $scope.groupsToShow = ($scope.groupCount > 0);

            if ($scope.groupsToShow) {
                $scope.LoadUpcomingMeetings();
            }

            LogErrors(serviceResponse.Errors);
        },
        function (data) {
            console.log("Unknown error occurred calling GetUpcomingMeetingCountForGroups");
            console.log(data);
        });
    }

    $scope.LoadUpcomingMeetings = function () {
        factory.callGetService("GetUpcomingMeetingsForGroups")
        .then(function (response) {
            var fullResult = angular.fromJson(response);
            var serviceResponse = JSON.parse(fullResult.data);

            $scope.groups = serviceResponse.Content;

            LogErrors(serviceResponse.Errors);
        },
        function (data) {
            console.log("Unknown error occurred calling GetUpcomingMeetingsForGroups");
            console.log(data);
        });
    }

    $scope.goToPage = function (pageName) {
        $location.path(pageName);
    }

    $scope.LoadData();

}]);