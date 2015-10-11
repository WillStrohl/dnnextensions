"use strict";

codeCampControllers.controller("aboutController", ["$scope", "$routeParams", "$location", "$http", "codeCampServiceFactory", function ($scope, $routeParams, $location, $http, codeCampServiceFactory) {
    
    var factory = codeCampServiceFactory;
    factory.init(moduleId, moduleName);

    $scope.userCanEdit = false;

    factory.callGetService("GetCurrentUserId")
        .then(function (response) {
            var fullResult = angular.fromJson(response);
            var serviceResponse = JSON.parse(fullResult.data);

            $scope.currentUserId = serviceResponse.Content;

            LogErrors(serviceResponse.Errors);
        },
        function (data) {
            console.log("Unknown error occurred calling GetCurrentUserId");
            console.log(data);
        });

    factory.callGetService("GetEventByModuleId")
        .then(function (response) {
            var fullResult = angular.fromJson(response);
            var serviceResponse = JSON.parse(fullResult.data);

            $scope.codeCamp = serviceResponse.Content;

            if ($scope.codeCamp === null) {
                $scope.hasCodeCamp = false;
            } else {
                $scope.hasCodeCamp = true;

                if ($scope.codeCamp != null) {
                    $scope.codeCamp.BeginDate = ParseDate($scope.codeCamp.BeginDate);
                    $scope.codeCamp.EndDate = ParseDate($scope.codeCamp.EndDate);
                }

                factory.callGetService("UserCanEditEvent?itemId=" + $scope.codeCamp.CodeCampId)
                    .then(function (response) {
                        var fullResult = angular.fromJson(response);
                        var serviceResponse = JSON.parse(fullResult.data);

                        $scope.userCanEdit = (serviceResponse.Content == "Success");

                        LogErrors(serviceResponse.Errors);
                    },
                    function (data) {
                        console.log("Unknown error occurred calling UserCanEditEvent");
                        console.log(data);
                    });

                factory.callGetService("GetRegistrationByUserId?userId=" + $scope.currentUserId + "&codeCampId=" + $scope.codeCamp.CodeCampId)
                    .then(function (response) {
                        var fullResult = angular.fromJson(response);
                        var serviceResponse = JSON.parse(fullResult.data);

                        $scope.currentUserRegistration = serviceResponse.Content;

                        LogErrors(serviceResponse.Errors);
                    },
                    function (data) {
                        console.log("Unknown error occurred calling GetRegistrationByUserId");
                        console.log(data);
                    });
            }

            LogErrors(serviceResponse.Errors);
        },
        function (data) {
            console.log("Unknown error occurred calling GetEventByModuleId");
            console.log(data);
        });

    $scope.goToPage = function(pageName) {
        $location.path(pageName);
    }

}]);