"use strict";

codeCampControllers.controller("speakersController", ["$scope", "$routeParams", "$http", "$modal", "codeCampServiceFactory", function ($scope, $routeParams, $http, $modal, codeCampServiceFactory) {

    $scope.speakers = {};
    $scope.hasSpeakers = false;

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

                factory.callGetService("GetSpeakers?codeCampId=" + $scope.codeCamp.CodeCampId)
                    .then(function (response) {
                        var fullResult = angular.fromJson(response);
                        var serviceResponse = JSON.parse(fullResult.data);

                        $scope.speakers = serviceResponse.Content;

                        if ($scope.speakers === null) {
                            $scope.hasSpeakers = false;
                        } else {
                            $scope.hasSpeakers = ($scope.speakers.count > 1);
                        }

                        LogErrors(serviceResponse.Errors);
                    },
                    function (data) {
                        console.log("Unknown error occurred calling GetSpeakers");
                        console.log(data);
                    });
            }

            LogErrors(serviceResponse.Errors);
        },
        function (data) {
            console.log("Unknown error occurred calling GetEventByModuleId");
            console.log(data);
        });

    $scope.openModal = function (size) {

        var modalInstance = $modal.open({
            templateUrl: "AddSpeakerModal.html",
            controller: "AddSpeakerModalController",
            size: size,
            backdrop: "static",
            resolve: {
                codeCamp: function () {
                    return $scope.codeCamp;
                }
            }
        });

        modalInstance.result.then(function (savedSpeaker) {
            console.log(savedSpeaker);
        }, function () {
            console.log("Modal dismissed at: " + new Date());
        });
    };

}]);

codeCampApp.controller("AddSpeakerModalController", function ($scope, $modalInstance, codeCamp) {

    $scope.speaker = {};

    $scope.CodeCampId = codeCamp.CodeCampId;

    $scope.ok = function () {
        $modalInstance.close($scope.speaker);
    };

    $scope.cancel = function () {
        $modalInstance.dismiss("cancel");
    };
});