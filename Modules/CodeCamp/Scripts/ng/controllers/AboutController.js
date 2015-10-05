"use strict";

codeCampControllers.controller("aboutController", ["$scope", "$routeParams", "$http", "codeCampServiceFactory", function ($scope, $routeParams, $http, codeCampServiceFactory) {
    
    var factory = codeCampServiceFactory;
    factory.init(moduleId, moduleName);

    $scope.userCanEdit = false;

    factory.callGetService("GetEventByModuleId")
        .then(function (response) {
            var fullResult = angular.fromJson(response);
            var serviceResponse = JSON.parse(fullResult.data);

            $scope.codeCamp = serviceResponse.Content;

            if ($scope.codeCamp === null) {
                $scope.hasCodeCamp = false;
            } else {
                $scope.hasCodeCamp = true;

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
            }

            LogErrors(serviceResponse.Errors);
        },
        function (data) {
            console.log("Unknown error occurred calling GetEventByModuleId");
            console.log(data);
        });

}]);