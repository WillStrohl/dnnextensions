"use strict";

userGroupControllers.controller("searchController", ["$scope", "$routeParams", "$location", "$http", "userGroupServiceFactory", function ($scope, $routeParams, $location, $http, userGroupServiceFactory) {
    
    var factory = userGroupServiceFactory;
    factory.init(moduleId, moduleName);

    $scope.userCanEdit = false;
    $scope.searchTerms = "";
    $scope.groupsToShow = false;
    $scope.groups = {};
    $scope.groupCount = -1;
    $scope.noSearchResults = true;

    $scope.LoadData = function() {
        factory.callGetService("GetCurrentUserId")
        .then(function (response) {
            var fullResult = angular.fromJson(response);
            var serviceResponse = JSON.parse(fullResult.data);

            $scope.currentUserId = serviceResponse.Content;

            //$scope.LoadEventDetails();

            LogErrors(serviceResponse.Errors);
        },
        function (data) {
            console.log("Unknown error occurred calling GetCurrentUserId");
            console.log(data);
        });
    }

    $scope.searchForGroups = function () {
        if ($scope.searchTerms == undefined) return;

        if ($scope.searchTerms.length > 2) {
            $scope.executeSearch();
        }
        else if ($scope.searchTerms.length == 0) {
            $scope.resetSearchDefaults();
        }
    }

    $scope.executeSearch = function () {
        var urlFriendlyKeywords = encodeURIComponent($scope.searchTerms);

        factory.callGetService("GetGroupsByKeyword?moduleID=" + moduleId + "&keywords=" + urlFriendlyKeywords)
        .then(function (response) {
            var fullResult = angular.fromJson(response);
            var serviceResponse = JSON.parse(fullResult.data);

            $scope.groups = serviceResponse.Content;

            if ($scope.groups != null) {
                $scope.groupsToShow = ($scope.groups.length > 0);
                $scope.groupCount = $scope.groups.length;
                $scope.noSearchResults = ($scope.groupCount == 0);
            } else {
                $scope.groupsToShow = false;
                $scope.groups = {};
                $scope.groupCount = 0;
                $scope.noSearchResults = true;
            }

            LogErrors(serviceResponse.Errors);
        },
        function (data) {
            console.log("Unknown error occurred calling GetGroupsByKeyword");
            console.log(data);
        });
    }

    $scope.resetSearchDefaults = function() {
        $scope.groupsToShow = false;
        $scope.groups = {};
        $scope.groupCount = -1;
        $scope.noSearchResults = true;
    }

    $scope.goToPage = function(pageName) {
        $location.path(pageName);
    }

    $scope.LoadData();

}]);