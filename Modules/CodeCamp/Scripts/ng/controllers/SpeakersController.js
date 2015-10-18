"use strict";

codeCampControllers.controller("speakersController", ["$scope", "$routeParams", "$http", "$modal", "codeCampServiceFactory", function ($scope, $routeParams, $http, $modal, codeCampServiceFactory) {

    $scope.speakers = {};
    $scope.hasSpeakers = false;

    var factory = codeCampServiceFactory;
    factory.init(moduleId, moduleName);

    $scope.showSpeakerSubmissionForm = function (spkr, rgstn, sssn) {
        if (sssn != undefined) {
            return (sssn.length == 0);
        } else {
            return (spkr == undefined && rgstn != undefined);
        }
    }

    $scope.updateSpeakersList = function () {
        factory.callGetService("GetSpeakers?codeCampId=" + $scope.codeCamp.CodeCampId)
            .then(function (response) {
                var fullResult = angular.fromJson(response);
                var serviceResponse = JSON.parse(fullResult.data);

                $scope.speakers = serviceResponse.Content;
                console.log("$scope.speakers = " + $scope.speakers);
                console.log("$scope.speakers.length = " + $scope.speakers.length);

                if ($scope.speakers === null) {
                    $scope.hasSpeakers = false;
                } else {
                    $scope.hasSpeakers = ($scope.speakers.length > 0);
                }
                console.log("$scope.hasSpeakers = " + $scope.hasSpeakers);

                LogErrors(serviceResponse.Errors);
            },
            function (data) {
                console.log("Unknown error occurred calling GetSpeakers");
                console.log(data);
            });
    }

    $scope.getSpeakerSessions = function() {
        factory.callGetService("GetSessionsBySpeakerId?codeCampId=" + $scope.codeCamp.CodeCampId + "&speakerId=" + $scope.currentSpeaker.SpeakerId)
            .then(function (response) {
                var fullResult = angular.fromJson(response);
                var serviceResponse = JSON.parse(fullResult.data);

                $scope.currentSpeakerSessions = serviceResponse.Content;
                console.log("$scope.currentSpeakerSessions = " + $scope.currentSpeakerSessions);

                LogErrors(serviceResponse.Errors);
            },
                function (data) {
                    console.log("Unknown error occurred calling GetSessionsBySpeakerId");
                    console.log(data);
                });
    }

    $scope.getSpeaker = function() {
        factory.callGetService("GetSpeakerByRegistrationId?codeCampId=" + $scope.codeCamp.CodeCampId + "&registrationId=" + $scope.currentUserRegistration.RegistrationId)
            .then(function (response) {
                var fullResult = angular.fromJson(response);
                var serviceResponse = JSON.parse(fullResult.data);

                $scope.currentSpeaker = serviceResponse.Content;
                console.log("$scope.currentSpeaker = " + $scope.currentSpeaker);

                if ($scope.currentSpeaker != null) {
                    $scope.getSpeakerSessions();
                }

                LogErrors(serviceResponse.Errors);
            },
                function (data) {
                    console.log("Unknown error occurred calling GetSpeakerByRegistrationId");
                    console.log(data);
                });
    }

    $scope.getRegistration = function() {
        factory.callGetService("GetRegistrationByUserId?userId=" + $scope.currentUserId + "&codeCampId=" + $scope.codeCamp.CodeCampId)
            .then(function (response) {
                var fullResult = angular.fromJson(response);
                var serviceResponse = JSON.parse(fullResult.data);

                $scope.currentUserRegistration = serviceResponse.Content;
                console.log("$scope.currentUserRegistration = " + $scope.currentUserRegistration);

                if ($scope.codeCamp != null && $scope.currentUserRegistration != null) {
                    $scope.getSpeaker();
                }

                LogErrors(serviceResponse.Errors);
            },
            function (data) {
                console.log("Unknown error occurred calling GetRegistrationByUserId");
                console.log(data);
            });
    }

    $scope.getEvent = function() {
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

                    $scope.updateSpeakersList();

                    $scope.getRegistration();
                }

                LogErrors(serviceResponse.Errors);
            },
            function (data) {
                console.log("Unknown error occurred calling GetEventByModuleId");
                console.log(data);
            });
    }

    factory.callGetService("GetCurrentUserId")
        .then(function (response) {
            var fullResult = angular.fromJson(response);
            var serviceResponse = JSON.parse(fullResult.data);

            $scope.currentUserId = serviceResponse.Content;
            console.log("$scope.currentUserId = " + $scope.currentUserId);

            $scope.getEvent();

            LogErrors(serviceResponse.Errors);
        },
        function (data) {
            console.log("Unknown error occurred calling GetCurrentUserId");
            console.log(data);
        });

    $scope.openModal = function (size) {
        var modalInstance = $modal.open({
            templateUrl: "AddSpeakerModal.html",
            controller: "AddSpeakerModalController",
            size: size,
            backdrop: "static",
            scope: $scope, 
            resolve: {
                userId: function() {
                    return $scope.currentUserId;
                },
                currentSpeaker: function () {
                    return ($scope.currentSpeaker) ? $scope.currentSpeaker : null;
                },
                currentSessions: function () {
                    return ($scope.currentSpeakerSessions) ? $scope.currentSpeakerSessions : null;
                },
                codeCamp: function () {
                    return $scope.codeCamp;
                },
                registration: function() {
                    return $scope.currentUserRegistration;
                }
            }
        });

        modalInstance.result.then(function (savedSpeaker) {
            $scope.savedSpeaker = savedSpeaker;
            console.log("$scope.savedSpeaker = " + $scope.savedSpeaker);
        }, function () {
            console.log("Modal dismissed at: " + new Date());
        });
    };

}])
.directive("speakerCards", function() {
    return {
        restrict: "E",
        templateUrl: "/DesktopModules/CodeCamp/Templates/_default/speaker-cards.html",
        scope: {
            speakers: "=data"
        },
        link: function (scope, element, attrs) {
            scope.$watch("speakers", function() {
                processSpeakerCards();
            });
        }
    };
});

/*
 * Speakers Modal Controller
 */
codeCampApp.controller("AddSpeakerModalController", ["$scope", "$rootScope", "$modalInstance", "userId", "currentSpeaker", "currentSessions", "codeCamp", "registration", "codeCampServiceFactory", function ($scope, $rootScope, $modalInstance, userId, currentSpeaker, currentSessions, codeCamp, registration, codeCampServiceFactory) {

    $scope.speaker = {};
    $scope.savedSpeaker = {};
    $scope.sessions = [];
    $scope.savedSessions = [];
    $scope.UpdateMode = false;

    var factory = codeCampServiceFactory;
    factory.init(moduleId, moduleName);

    $scope.CodeCampId = codeCamp.CodeCampId;

    if (currentSpeaker) {
        $scope.UpdateMode = true;
        $scope.speaker = currentSpeaker;
        $scope.sessions = currentSessions;
    }

    if ($scope.sessions.length == 0) {
        $scope.sessions.push({
            AudienceLevel: 0,
            Title: "",
            Description: "",
            CodeCampId: $scope.CodeCampId
        });
    }

    $scope.AddSession = function () {
        $scope.sessions.push({
            AudienceLevel: 0,
            Title: "",
            Description: "",
            CodeCampId: $scope.CodeCampId
        });
    }

    $scope.requiresConfirmation = function(session) {
        return (session.SessionId > 0 || (session.Title.length > 0 || session.Description.length > 0));
    }

    $scope.ProcessSessionRemoval = function (elem, session) {
        if ($scope.requiresConfirmation(session)) {
            jQuery(elem).dnnConfirm({
                text: "Are you sure you want to delete this?",
                yesText: "Yes",
                noText: "No",
                title: "Delete Confirmation",
                callbackTrue: function() {
                    if (session.SessionId > 0) {
                        factory.callDeleteService("DeleteSession", session.SessionId)
                            .success(function(data) {
                                var deleteResponse = angular.fromJson(data);
                                console.log("deleteResponse.Content = " + deleteResponse.Content);

                                // TODO: delete sessionspeaker data as well

                                $scope.RemoveSession(session);
                            })
                            .error(function(data, status) {
                                $scope.HasErrors = true;
                                console.log("Unknown error occurred calling DeleteSession");
                                console.log(data);
                            });
                    } else {
                        $scope.RemoveSession(session);
                    }
                    return true;
                },
                callbackFalse: function() {
                    return false;
                }
            });

            jQuery(elem).trigger("click");
            jQuery(elem).click(function(e) {
                return;
            });
        } else {
            $scope.RemoveSession(session);
        }
    }

    $scope.RemoveSession = function(session) {
        var index = $scope.sessions.indexOf(session);
        $scope.sessions.splice(index, 1);
    }

    $scope.SessionIndex = function(session) {
        return $scope.sessions.indexOf(session);
    }

    $scope.ok = function () {
        $scope.speaker.RegistrationId = registration.RegistrationId;
        $scope.speaker.CodeCampId = $scope.CodeCampId;

        var speakerAction = ($scope.UpdateMode) ? "UpdateSpeaker" : "CreateSpeaker";
        
        // save the speaker
        factory.callPostService(speakerAction, $scope.speaker)
            .success(function (data) {
                var savedSpeaker = angular.fromJson(data);
                $scope.savedSpeaker = savedSpeaker.Content;

                // save the sessions
                $.each($scope.sessions, function (index, session) {

                    var sessionAction = (session.SessionId > 0) ? "UpdateSession" : "CreateSession";
                    var sessionSpeakerAction = (session.SessionId > 0) ? "UpdateSessionSpeaker" : "CreateSessionSpeaker";

                    factory.callPostService(sessionAction, session)
                        .success(function (data) {
                            var savedSession = angular.fromJson(data);

                            $scope.savedSessions.push(savedSession.Content);

                            var sessionSpeaker = {
                                SessionId: savedSession.Content.SessionId,
                                SpeakerId: $scope.savedSpeaker.SpeakerId
                            };

                            // save the speaker session joins
                            factory.callPostService(sessionSpeakerAction, sessionSpeaker)
                                .success(function (data) {
                                    var sessionSpeaker = angular.fromJson(data);

                                    $scope.updateSpeakersList();

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

/*
 * Additional functions 
 */
function processSpeakerCards() {
    var BLUR_RADIUS = 40;
    var sourceImages = [];

    jQuery(".speaker-avatar").each(function() {
        console.log("found speaker avatar");
        sourceImages.push(jQuery(this).attr("src"));
    });

    var drawBlur = function(canvas, image) {
        var w = canvas.width;
        var h = canvas.height;
        var canvasContext = canvas.getContext("2d");
        canvasContext.drawImage(image, 0, 0, w, h);
        stackBlurCanvasRGBA(canvas, 0, 0, w, h, BLUR_RADIUS);
    };

    jQuery(".card canvas").each(function(index) {
        console.log("processing canvas");
        var canvas = jQuery(this)[0];

        var image = new Image();
        image.src = sourceImages[index];

        image.onload = function() {
            drawBlur(canvas, image);
        }
    });
}