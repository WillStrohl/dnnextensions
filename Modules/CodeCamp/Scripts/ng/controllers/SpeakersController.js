"use strict";

codeCampControllers.controller("speakersController", ["$scope", "$routeParams", "$http", "$uibModal", "codeCampServiceFactory", function ($scope, $routeParams, $http, $uibModal, codeCampServiceFactory) {

    $scope.currentSpeaker = {};
    $scope.speakers = {};
    $scope.currentUserRegistration = {};
    $scope.currentSpeakerSessions = [];
    $scope.hasSpeakers = false;
    $scope.showSpeakerSubmission = false;

    var factory = codeCampServiceFactory;
    factory.init(moduleId, moduleName);

    $scope.LoadSpeakerSubmission = function () {
        if ($scope.currentSpeakerSessions != undefined) {
            $scope.showSpeakerSubmission = ($scope.currentSpeakerSessions.length == 0);
        } else {
            $scope.showSpeakerSubmission = ($scope.currentSpeaker == undefined && $scope.currentUserRegistration != undefined);
        }
    }

    $scope.LoadData = function() {
        factory.callGetService("GetCurrentUserId")
            .then(function (response) {
                var fullResult = angular.fromJson(response);
                var serviceResponse = JSON.parse(fullResult.data);

                $scope.currentUserId = serviceResponse.Content;

                $scope.getEvent();

                LogErrors(serviceResponse.Errors);
            },
            function (data) {
                console.log("Unknown error occurred calling GetCurrentUserId");
                console.log(data);
            });
    }

    $scope.updateSpeakersList = function () {
        factory.callGetService("GetSpeakers?codeCampId=" + $scope.codeCamp.CodeCampId)
            .then(function (response) {
                var fullResult = angular.fromJson(response);
                var serviceResponse = JSON.parse(fullResult.data);

                $scope.speakers = serviceResponse.Content;

                if ($scope.speakers === null) {
                    $scope.hasSpeakers = false;
                } else {
                    $scope.hasSpeakers = ($scope.speakers.length > 0);

                    $.each($scope.speakers, function (i, obj) {
                        obj.SpeakerSlug = GetSlugFromValue(obj.SpeakerName);
                    });
                }

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

                $scope.LoadSpeakerSubmission();

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

                if ($scope.codeCamp != null && $scope.currentUserRegistration != null) {
                    $scope.getSpeaker();
                }

                $scope.LoadSpeakerSubmission();

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

    $scope.openModal = function (size) {
        var modalInstance = $uibModal.open({
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
            $scope.LoadData();
        }, function () {
            console.log("Modal dismissed at: " + new Date());
        });
    };

    $scope.LoadData();

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
codeCampApp.controller("AddSpeakerModalController", ["$scope", "$rootScope", "$uibModal", "$uibModalInstance", "userId", "currentSpeaker", "currentSessions", "codeCamp", "registration", "codeCampServiceFactory", function ($scope, $rootScope, $uibModal, $uibModalInstance, userId, currentSpeaker, currentSessions, codeCamp, registration, codeCampServiceFactory) {

    $scope.speaker = {};
    $scope.savedSpeaker = {};
    $scope.sessions = [];
    $scope.savedSessions = [];
    $scope.UpdateMode = false;
    $scope.uploader = {};

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
            $scope.ConfirmDeleteSession(session);
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

                    $scope.saveSession(sessionAction, session, sessionSpeakerAction);
                });

                // save the speaker avatar
                $scope.saveAvatar();

                LogErrors(savedSpeaker.Errors);
            })
            .error(function (data, status) {
                $scope.HasErrors = true;
                console.log("Unknown error occurred calling " + speakerAction);
                console.log(data);
            });

        $uibModalInstance.close($scope.savedSpeaker);
    };

    $scope.saveAvatar = function () {

        //var $self = this;

        //if ($.ServicesFramework) {
        //    var _sf = $.ServicesFramework(moduleId);
        //    $self.ServiceRoot = _sf.getServiceRoot(moduleName);
        //    $self.ServicePath = $uploadService.ServiceRoot + "Event/";
        //    $self.Headers = {
        //        "Content-Type": undefined,
        //        "ModuleId": moduleId,
        //        "TabId": _sf.getTabId(),
        //        "RequestVerificationToken": _sf.getAntiForgeryValue()
        //    };
        //}

        angular.forEach($scope.$flow.files, function (file, index) {
            var fd = new FormData();
            fd.append("file", file);
            //$http.post($uploadService.ServicePath + "UpdateSpeakerAvatar", fd, {
            //    withCredentials: true,
            //    headers: $self.Headers,
            //    transformRequest: angular.identity
            //});

            factory.callPostService("UpdateSpeakerAvatar?codeCampId=" + $scope.CodeCampId + "&speakerId=" + $scope.speaker.SpeakerId, fd)
                .success(function (data) {
                    var rawResponse = angular.fromJson(data);
                    $scope.avatarResponse = rawResponse.Content;

                    LogErrors($scope.avatarResponse.Errors);
                })
                .error(function (data, status) {
                    $scope.HasErrors = true;
                    console.log("Unknown error occurred calling " + speakerAction);
                    console.log(data);
                });
        });


        //$scope.f = file;
        //$scope.errFile = errFiles && errFiles[0];
        //if (file) {
        //    file.upload = Upload.upload({
        //        method: "POST",
        //        headers: $uploadService.Headers,
        //        url: $uploadService.ServicePath + "UpdateSpeakerAvatar",
        //        data: { file: file }
        //    });

        //    file.upload.then(function (response) {
        //        console.log(response);
        //        $timeout(function () {
        //            file.result = response.data;
        //        });
        //    }, function (response) {
        //        if (response.status > 0)
        //            $scope.errorMsg = response.status + ": " + response.data;
        //    }, function (evt) {
        //        file.progress = Math.min(100, parseInt(100.0 * evt.loaded / evt.total));
        //    });
        //}
    }

    $scope.saveSession = function (sessionAction, session, sessionSpeakerAction) {
        factory.callPostService(sessionAction, session)
            .success(function (data) {
                var savedSession = angular.fromJson(data);

                $scope.savedSessions.push(savedSession.Content);

                var sessionSpeaker = {
                    SessionId: savedSession.Content.SessionId,
                    SpeakerId: $scope.savedSpeaker.SpeakerId
                };

                // save the speaker session joins
                $scope.saveSpeakerSession(sessionSpeakerAction, sessionSpeaker);

                LogErrors(savedSession.Errors);
            })
            .error(function (data, status) {
                $scope.HasErrors = true;
                console.log("Unknown error occurred calling " + sessionAction);
                console.log(data);
            });
    }

    $scope.saveSpeakerSession = function(sessionSpeakerAction, sessionSpeaker) {
        factory.callPostService(sessionSpeakerAction, sessionSpeaker)
            .success(function (data) {
                var sessionSpeaker = angular.fromJson(data);

                $scope.updateSpeakersList();

                $scope.LoadData();

                LogErrors(sessionSpeaker.Errors);
            })
            .error(function (data, status) {
                $scope.HasErrors = true;
                console.log("Unknown error occurred calling CreateSessionSpeaker");
                console.log(data);
            });
    }

    $scope.cancel = function () {
        $uibModalInstance.dismiss("cancel");
    };

    $scope.ConfirmDeleteSession = function (session) {
        var modalInstance = $uibModal.open({
            templateUrl: "DeleteSessionModal.html",
            controller: "DeleteSessionModalController",
            size: "sm",
            backdrop: "static",
            scope: $scope,
            resolve: {
                session: function () {
                    return $scope.session;
                }
            }
        });

        modalInstance.result.then(function (result) {
            if (result != null) {
                $scope.RemoveSession(result);
            }
        }, function () {
            console.log("Modal dismissed at: " + new Date());
        });
    }

    $scope.uploader = function() {
        $scope.$flow.upload();
    }
}]);

/*
 * Delete Modal
 */
codeCampApp.controller("DeleteSessionModalController", ["$scope", "$rootScope", "$uibModalInstance", "session", function ($scope, $rootScope, $uibModalInstance, session) {

    $scope.ok = function () {
        $scope.RemoveSession(session);
        $uibModalInstance.close(session);
    };

    $scope.cancel = function () {
        $uibModalInstance.dismiss("cancel");
    };

}]);

/*
 * Additional functions 
 */
function processSpeakerCards() {
    var BLUR_RADIUS = 40;
    var sourceImages = [];

    jQuery(".speaker-avatar").each(function() {
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
        var canvas = jQuery(this)[0];

        var image = new Image();
        image.src = sourceImages[index];

        image.onload = function() {
            drawBlur(canvas, image);
        }
    });
}
