"use strict";

codeCampControllers.controller("sessionsController", ["$scope", "$routeParams", "$http", "codeCampServiceFactory", function ($scope, $routeParams, $http, codeCampServiceFactory) {

    $scope.sessions = [];
    $scope.hasSessions = false;
    $scope.sessionFilter = "";

    var factory = codeCampServiceFactory;
    factory.init(moduleId, moduleName);

    $scope.LoadData = function () {
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
    }

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

    $scope.GetSpeakerName = function (sessionId) {
        factory.callGetService("GetSpeaker?codeCampId=" + $scope.codeCamp.CodeCampId)
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

    $scope.LoadData();

}])
.directive("speakerList", function() {
    return {
        restrict: "E",
        scope: {
            speakers: "="
        },
        replace: true,
        link: function(scope, elem, attrs) {
            scope.speakerText = "";

            for (var i = 0; i < scope.speakers.length; i++) {
                if (i > 0) {
                    scope.speakerText = speakerText + ", ";
                }
                scope.speakerText = scope.speakerText + scope.speakers[i].SpeakerName;
            }
        },
        template: "<span>{{speakerText}}</span>"
    }
});