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

        // TOOD: require registratin before allowing for a speaker submission OR add email address to the speaker info (then pass it to the modal)

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
            $scope.savedSpeaker = savedSpeaker;
            console.log($scope.savedSpeaker);
        }, function () {
            console.log("Modal dismissed at: " + new Date());
        });
    };

}]);

codeCampApp.controller("AddSpeakerModalController", ["$scope", "$modalInstance", "codeCamp", "codeCampServiceFactory", function ($scope, $modalInstance, codeCamp, codeCampServiceFactory) {

    $scope.speaker = {};
    $scope.savedSpeaker = {};
    $scope.sessions = [];
    $scope.savedSessions = [];

    var factory = codeCampServiceFactory;
    factory.init(moduleId, moduleName);

    $scope.CodeCampId = codeCamp.CodeCampId;

    $scope.sessions.push({
        AudienceLevel: 0,
        Title: "",
        Description: "",
        CodeCampId: $scope.CodeCampId
    });

    $scope.AddSession = function () {
        $scope.sessions.push({
            AudienceLevel: 0,
            Title: "",
            Description: "",
            CodeCampId: $scope.CodeCampId
        });
    }

    $scope.RemoveSession = function(session) {
        var index = $scope.sessions.indexOf(session);
        $scope.sessions.splice(index, 1);
    }

    $scope.SessionIndex = function(session) {
        return $scope.sessions.indexOf(session);
    }

    $scope.ok = function () {
        $scope.speaker.CodeCampId = $scope.CodeCampId;

        console.log("$scope.speaker = " + $scope.speaker);
        console.log("$scope.sessions = " + $scope.sessions);

        // save the speaker
        factory.callPostService("CreateSpeaker", $scope.speaker)
            .success(function (data) {
                var savedSpeaker = angular.fromJson(data);
                $scope.savedSpeaker = savedSpeaker.Content;
                console.log("savedSpeaker = " + savedSpeaker);

                // save the sessions
                $.each($scope.sessions, function (index, session) {
                    factory.callPostService("CreateSession", session)
                        .success(function (data) {
                            var savedSession = angular.fromJson(data);

                            $scope.savedSessions.push(savedSession.Content);
                            console.log("savedSession = " + savedSession.Content);

                            var sessionSpeaker = {
                                SessionId: savedSession.Content.SessionId,
                                SpeakerId: $scope.savedSpeaker.SpeakerId
                            };

                            // save the speaker session joins
                            factory.callPostService("CreateSessionSpeaker", sessionSpeaker)
                                .success(function (data) {
                                    var sessionSpeaker = angular.fromJson(data);
                                    console.log("sessionSpeaker = " + sessionSpeaker.Content);

                                    LogErrors(sessionSpeaker.Errors);
                                })
                                .error(function (data, status) {
                                    $scope.HasErrors = true;
                                    console.log("Unknown error occurred calling CreateSessionSpeaker");
                                    console.log(data);
                                });

                            LogErrors(savedSession.Errors);
                        })
                        .error(function (data, status) {
                            $scope.HasErrors = true;
                            console.log("Unknown error occurred calling CreateSession");
                            console.log(data);
                        });
                });

                LogErrors(savedSpeaker.Errors);
            })
            .error(function (data, status) {
                $scope.HasErrors = true;
                console.log("Unknown error occurred calling CreateSpeaker");
                console.log(data);
            });

        $modalInstance.close($scope.savedSpeaker);
    };

    $scope.cancel = function () {
        $modalInstance.dismiss("cancel");
    };
}]);