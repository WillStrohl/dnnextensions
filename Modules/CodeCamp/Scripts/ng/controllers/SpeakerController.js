"use strict";

codeCampControllers.controller("speakerController", ["$scope", "$routeParams", "$http", "codeCampServiceFactory", function ($scope, $routeParams, $http, codeCampServiceFactory) {

    $scope.speaker = {};

    var factory = codeCampServiceFactory;
    factory.init(moduleId, moduleName);

    $scope.speakerId = $routeParams.speakerId;

    factory.callGetService("GetEventByModuleId")
        .then(function (response) {
            var fullResult = angular.fromJson(response);
            var serviceResponse = JSON.parse(fullResult.data);

            $scope.codeCamp = serviceResponse.Content;
            console.log("serviceResponse.Content = " + serviceResponse.Content);

            if ($scope.codeCamp != null) {
                $scope.codeCamp.BeginDate = ParseDate($scope.codeCamp.BeginDate);
                $scope.codeCamp.EndDate = ParseDate($scope.codeCamp.EndDate);
            }

            if ($scope.codeCamp === null) {
                $scope.hasCodeCamp = false;
            } else {
                $scope.hasCodeCamp = true;

                $scope.getSpeaker();
            }

            LogErrors(serviceResponse.Errors);
        },
        function (data) {
            console.log("Unknown error occurred calling GetEventByModuleId");
            console.log(data);
        });

    $scope.getSpeaker = function () {
        factory.callGetService("GetSpeaker?itemId=" + $scope.speakerId + "&codeCampId=" + $scope.codeCamp.CodeCampId)
            .then(function (response) {
                var fullResult = angular.fromJson(response);
                var serviceResponse = JSON.parse(fullResult.data);

                $scope.speaker = serviceResponse.Content;
                console.log("$scope.speaker = " + $scope.speaker);

                if ($scope.codeCamp === null) {
                    $scope.hasSpeaker = false;
                } else {
                    $scope.hasSpeaker = true;
                }

                LogErrors(serviceResponse.Errors);
            },
            function (data) {
                console.log("Unknown error occurred calling GetEventByModuleId");
                console.log(data);
            });
    }

}]).directive("sessionAudience", function () {
    return {
        restrict: "E",
        scope: {
            level: "="
        },
        link: function (scope, element, attrs) {
            switch (scope.level) {
                case 0:
                    scope.AudienceTitle = "Beginners";
                    break;
                case 1:
                    scope.AudienceTitle = "Intermediate";
                    break;
                case 2:
                    scope.AudienceTitle = "Advanced";
                    break;
                default:
                    scope.AudienceTitle = "UNKNOWN";
                    break;
            }

            element.replaceWith(scope.AudienceTitle);
        }
    }
});