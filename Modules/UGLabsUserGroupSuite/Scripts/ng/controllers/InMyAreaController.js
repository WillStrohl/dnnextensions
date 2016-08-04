"use strict";

userGroupControllers.controller("inMyAreaController", ["$scope", "$routeParams", "$location", "$http", "$cookies", "userGroupServiceFactory", function ($scope, $routeParams, $location, $http, $cookies, userGroupServiceFactory) {
    
    var factory = userGroupServiceFactory;
    factory.init(moduleId, moduleName);

    $scope.userCanEdit = false;
    $scope.geoData = {};
    $scope.geoDataFromCookie = false;

    $scope.loadData = function() {
        factory.callGetService("GetCurrentUserId")
        .then(function (response) {
            var fullResult = angular.fromJson(response);
            var serviceResponse = JSON.parse(fullResult.data);

            $scope.currentUserId = serviceResponse.Content;

            $scope.getVisitorLocation();

            LogErrors(serviceResponse.Errors);
        },
        function (data) {
            console.log("Unknown error occurred calling GetCurrentUserId");
            console.log(data);
        });
    }

    $scope.loadClosestGroups = function() {
        /* TODO: finish this view after I figure out how I'm actually storing/retrieving location data */
    }

    $scope.getVisitorLocation = function () {
        $scope.geoData = $cookies.getObject("geoData");

        if ($scope.geoData == null) {
            factory.callGetService("GetCurrentUserGeoData")
                .then(function(response) {
                        var fullResult = angular.fromJson(response);
                        var serviceResponse = JSON.parse(fullResult.data);

                        $scope.geoData = serviceResponse.Content;

                        var today = new Date();
                        var expirationDate = new Date(today.getFullYear() + 1, today.getMonth(), today.getDate());

                        $cookies.putObject("geoData", $scope.geoData, { expires: expirationDate });

                        $scope.loadClosestGroups();

                        LogErrors(serviceResponse.Errors);
                    },
                    function(data) {
                        console.log("Unknown error occurred calling GetCurrentUserGeoData");
                        console.log(data);
                    });
        } else {
            $scope.geoDataFromCookie = true;
            $scope.loadClosestGroups();
        }
    }

    $scope.refreshVisitorLocation = function() {
        $scope.geoData = {};
        $cookies.remove("geoData");

        $scope.getVisitorLocation();
    }

    $scope.goToPage = function(pageName) {
        $location.path(pageName);
    }

    $scope.loadData();

}]);