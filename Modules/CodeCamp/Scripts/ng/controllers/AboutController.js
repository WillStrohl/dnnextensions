"use strict";

codeCampApp.controller("aboutController", ["$scope", "$routeParams", "$http", "codeCampServiceFactory", function ($scope, $routeParams, $http, codeCampServiceFactory) {
    
    var factory = codeCampServiceFactory;
    factory.init(moduleId, moduleName);

    factory.callGetService("GetEventByModuleId")
        .then(function (response) {
            var fullResult = angular.fromJson(response);
            var serviceResponse = JSON.parse(fullResult.data);

            $scope.CodeCamp = serviceResponse.Content;
            if ($scope.CodeCamp === null) {
                $scope.HasCodeCamp = false;
            } else {
                $scope.HasCodeCamp = true;
            }

            LogErrors(serviceResponse.Errors);
        },
        function (data) {
            console.log("Unknown error occurred calling GetEventByModuleId");
            console.log(data);
        });

}]);