"use strict";

codeCampControllers.controller("eventController", ["$scope", "$routeParams", "$http", "codeCampServiceFactory", function ($scope, $routeParams, $http, codeCampServiceFactory) {

    $scope.codeCamp = {};

    var ccController = this;

    $scope.HasSuccess = false;
    $scope.HasErrors = false;

    var factory = codeCampServiceFactory;
    factory.init(moduleId, moduleName);

    factory.callGetService("GetEventByModuleId")
        .then(function (response) {
            var fullResult = angular.fromJson(response);
            var serviceResponse = JSON.parse(fullResult.data);

            $scope.codeCamp = serviceResponse.Content;
            if ($scope.codeCamp === null) {
                $scope.hasCodeCamp = false;
            } else {
                $scope.hasCodeCamp = true;
            }

            LogErrors(serviceResponse.Errors);
        },
        function (data) {
            console.log("Unknown error occurred calling GetEventByModuleId");
            console.log(data);
        });

    ccController.createEvent = function () {

        factory.callPostService("CreateEvent", codeCamp)
            .success(function () {
                $scope.HasSuccess = true;
                LogErrors(serviceResponse.Errors);
                // redirect to the about page after a delay
            })
            .error(function () {
                $scope.HasErrors = true;
                console.log("Unknown error occurred calling GetEventByModuleId");
                console.log(data);
            });
    }

}]);