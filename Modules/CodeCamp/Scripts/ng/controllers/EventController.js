"use strict";

codeCampControllers.controller("eventController", ["$scope", "$routeParams", "$http", "$location", "codeCampServiceFactory", function ($scope, $routeParams, $http, $location, codeCampServiceFactory) {

    $scope.codeCamp = {};

    $scope.HasSuccess = false;
    $scope.HasErrors = false;

    var factory = codeCampServiceFactory;
    factory.init(moduleId, moduleName);

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
            }

            LogErrors(serviceResponse.Errors);
        },
        function (data) {
            console.log("Unknown error occurred calling GetEventByModuleId");
            console.log(data);
        });

    $scope.createEvent = function () {
        console.log($scope.codeCamp);

        var action = "";

        if ($scope.codeCamp.CodeCampId > 0) {
            action = "UpdateEvent";
        } else {
            action = "CreateEvent";
        }

        factory.callPostService(action, $scope.codeCamp)
            .success(function (data) {
                $scope.HasSuccess = true;

                var serviceResponse = angular.fromJson(data);

                LogErrors(serviceResponse.Errors);
            })
            .error(function (data, status) {
                $scope.HasErrors = true;
                console.log("Unknown error occurred calling " + action);
                console.log(data);
            });
    }

    $scope.goToAbout = function () {
        $location.path("/about");
    }

    /* DatePicker */

    $scope.datePicker = (function() {

        var method = {};
        method.instances = [];

        method.open = function ($event, instance) {
            if ($event) {
                $event.preventDefault();
                $event.stopPropagation();
            }

            method.instances[instance] = true;
        };

        method.minDate = new Date() +1;

        method.maxDate = new Date(2023, 12, 24);

        method.options = {
            formatYear: "yyyy",
            startingDay: 1
        };

        method.format = "MM/dd/yyyy";

        return method;
    }());
    
}]);