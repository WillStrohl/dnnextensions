"use strict";

codeCampControllers.controller("sessionsController", ["$scope", "$routeParams", "$http", "codeCampServiceFactory", function ($scope, $routeParams, $http, codeCampServiceFactory) {
    
    $scope.sessions = {};
    $scope.hasSessions = false;

    var factory = codeCampServiceFactory;
    factory.init(moduleId, moduleName);

    $scope.getSessions = function () {
        factory.callGetService("GetSessions?codeCampId=" + $scope.codeCamp.CodeCampId)
        .then(function (response) {
            var fullResult = angular.fromJson(response);
            var serviceResponse = JSON.parse(fullResult.data);

            $scope.sessions = serviceResponse.Content;
            console.log("$scope.sessions = " + $scope.sessions);
            console.log("$scope.sessions.length = " + $scope.sessions.length);

            if ($scope.sessions === null) {
                $scope.hasSessions = false;
            } else {
                $scope.hasSessions = ($scope.sessions.length > 0);
            }
            console.log("$scope.hasSessions = " + $scope.hasSessions);

            LogErrors(serviceResponse.Errors);
        },
        function (data) {
            console.log("Unknown error occurred calling GetSessions");
            console.log(data);
        });
    }

    factory.callGetService("GetEventByModuleId")
        .then(function (response) {
            var fullResult = angular.fromJson(response);
            var serviceResponse = JSON.parse(fullResult.data);

            $scope.codeCamp = serviceResponse.Content;

            if ($scope.codeCamp != null) {
                $scope.codeCamp.BeginDate = ParseDate($scope.codeCamp.BeginDate);
                $scope.codeCamp.EndDate = ParseDate($scope.codeCamp.EndDate);
            }

            if ($scope.codeCamp === null) {
                $scope.hasCodeCamp = false;
            } else {
                $scope.hasCodeCamp = true;

                $scope.getSessions();
            }

            LogErrors(serviceResponse.Errors);
        },
        function (data) {
            console.log("Unknown error occurred calling GetEventByModuleId");
            console.log(data);
        });

}]);