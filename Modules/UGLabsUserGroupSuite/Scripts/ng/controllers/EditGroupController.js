"use strict";

userGroupControllers.controller("editGroupController", ["$scope", "$routeParams", "$location", "$http", "userGroupServiceFactory", function ($scope, $routeParams, $location, $http, userGroupServiceFactory) {
    
    var factory = userGroupServiceFactory;
    factory.init(moduleId, moduleName);

    $scope.userCanEdit = false;
    $scope.userIsLoggedIn = false;

    $scope.LoadData = function() {
        factory.callGetService("GetCurrentUserId")
        .then(function (response) {
            var fullResult = angular.fromJson(response);
            var serviceResponse = JSON.parse(fullResult.data);

            $scope.currentUserId = serviceResponse.Content;

            $scope.userIsLoggedIn = ($scope.currentUserId > -1);

            //$scope.GroupCheck();

            LogErrors(serviceResponse.Errors);
        },
        function (data) {
            console.log("Unknown error occurred calling GetCurrentUserId");
            console.log(data);
        });
    }

    $scope.goToPage = function(pageName) {
        $location.path(pageName);
    }

    $scope.LoadData();

}]);