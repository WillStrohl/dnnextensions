"use strict";

codeCampControllers.controller("speakerController", ["$scope", "$routeParams", "$http", "codeCampServiceFactory", function ($scope, $routeParams, $http, codeCampServiceFactory) {

    $scope.speaker = {};
    $scope.SessionRegistration = [];

    var factory = codeCampServiceFactory;
    factory.init(moduleId, moduleName);

    $scope.speakerId = $routeParams.speakerId;

    $scope.LoadData = function() {
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
    }

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

                $scope.getCurrentUserId();

                LogErrors(serviceResponse.Errors);
            },
            function (data) {
                console.log("Unknown error occurred calling GetEventByModuleId");
                console.log(data);
            });
    }

    $scope.getCurrentUserId = function() {
        factory.callGetService("GetCurrentUserId")
            .then(function (response) {
                var fullResult = angular.fromJson(response);
                var serviceResponse = JSON.parse(fullResult.data);

                $scope.currentUserId = serviceResponse.Content;

                $scope.getEventRegistration();

                LogErrors(serviceResponse.Errors);
            },
            function (data) {
                console.log("Unknown error occurred calling GetCurrentUserId");
                console.log(data);
            });
    }

    $scope.getEventRegistration = function() {
        factory.callGetService("GetRegistrationByUserId?userId=" + $scope.currentUserId + "&codeCampId=" + $scope.codeCamp.CodeCampId)
            .then(function (response) {
                var fullResult = angular.fromJson(response);
                var serviceResponse = JSON.parse(fullResult.data);

                $scope.currentUserRegistration = serviceResponse.Content;

                $scope.UpdateSessionRegistration();

                $scope.DetermineRegisterEnablement();

                LogErrors(serviceResponse.Errors);
            },
            function (data) {
                console.log("Unknown error occurred calling GetRegistrationByUserId");
                console.log(data);
            });
    }

    $scope.UpdateSessionRegistration = function () {
        $scope.SessionRegistration.length = 0;

        if ($scope.currentUserRegistration !== null) {
            $.each($scope.speaker.Sessions, function(index, session) {
                factory.callGetService("GetSessionRegistrationByRegistrantId?sessionId=" + session.SessionId + "&registrantId=" + $scope.currentUserRegistration.RegistrationId)
                    .then(function(response) {
                            var fullResult = angular.fromJson(response);
                            var serviceResponse = JSON.parse(fullResult.data);

                            var result = (serviceResponse.Content != null);

                            if (result) {
                                $scope.SessionRegistration.push(session.SessionId);
                            }

                            LogErrors(serviceResponse.Errors);
                        },
                        function(data) {
                            console.log("Unknown error occurred calling GetSessionRegistrationByRegistrantId");
                            console.log(data);
                        });
            });
        }
    }

    $scope.DetermineRegisterEnablement = function() {
        var validRegistration = ($scope.currentUserRegistration != null);

        if (!validRegistration) {
            $scope.SessionRegistrationIsDisabled = true;
        } else {
            var speakerIsSame = ($scope.speaker.RegistrationId == $scope.currentUserRegistration.RegistrationId);

            $scope.SessionRegistrationIsDisabled = speakerIsSame;
        }
    }

    $scope.IsUserRegisteredForSession = function (sessionId) {
        var result = $scope.SessionRegistration.indexOf(sessionId) >= 0;
        console.log("IsUserRegisteredForSession(" + sessionId + ") = " + result);
        return result;
    }

    $scope.GetSessionRegistrationStatus = function (sessionId) {
        var result = false;

        if ($scope.currentUserRegistration != null) {
            factory.callGetService("GetSessionRegistrationByRegistrantId?sessionId=" + sessionId + "&registrantId=" + $scope.currentUserRegistration.RegistrationId)
                .then(function (response) {
                    var fullResult = angular.fromJson(response);
                    var serviceResponse = JSON.parse(fullResult.data);

                    result = (serviceResponse.Content != null);

                    LogErrors(serviceResponse.Errors);

                    return result;
                },
                function (data) {
                    console.log("Unknown error occurred calling GetSessionRegistrationByRegistrantId");
                    console.log(data);
                    return result;
                });
        }

        return result;
    }

    $scope.GetSessionRegistrationCount = function (sessionId) {
        var count = 0;

        factory.callGetService("GetSessionRegistrations?sessionId=" + sessionId)
            .then(function (response) {
                var fullResult = angular.fromJson(response);
                var serviceResponse = JSON.parse(fullResult.data);

                if (serviceResponse.Content != null) {
                    count = serviceResponse.Content;
                }

                LogErrors(serviceResponse.Errors);

                return count;
            },
            function (data) {
                console.log("Unknown error occurred calling GetSessionRegistrationByRegistrantId");
                console.log(data);
                return null;
            });
    }

    $scope.RegisterForSession = function (sessionId) {
        var registration = {
            SessionId: sessionId,
            RegistrationId: $scope.currentUserRegistration.RegistrationId
        };

        factory.callPostService("CreateSessionRegistration", registration)
            .then(function (response) {
                var fullResult = angular.fromJson(response);
                var serviceResponse = JSON.parse(fullResult.data);

                $scope.UpdateSessionRegistration();

                LogErrors(serviceResponse.Errors);
            },
            function (data) {
                console.log("Unknown error occurred calling CreateSessionRegistration");
                console.log(data);
            });
    }

    $scope.UnregisterForSession = function (sessionId) {
        var registration = {};

        factory.callGetService("GetSessionRegistrationByRegistrantId?sessionId=" + sessionId + "&registrantId=" + $scope.currentUserRegistration.RegistrationId)
            .then(function (response) {
                var fullResult = angular.fromJson(response);
                var serviceResponse = JSON.parse(fullResult.data);

                LogErrors(serviceResponse.Errors);

                registration = serviceResponse.Content;

                if (registration != null) {
                    factory.callDeleteService("DeleteSessionRegistration?itemId=" + registration.SessionRegistrationId + "&sessionId=" + registration.SessionId)
                        .then(function (response) {
                            var fullResult = angular.fromJson(response);
                            var serviceResponse = JSON.parse(fullResult.data);
                            console.log("serviceResponse = " + serviceResponse);

                            $scope.UpdateSessionRegistration();

                            LogErrors(serviceResponse.Errors);
                        },
                        function (data) {
                            console.log("Unknown error occurred calling CreateSessionRegistration");
                            console.log(data);
                        });
                }
            },
            function (data) {
                console.log("Unknown error occurred calling GetSessionRegistrationByRegistrantId");
                console.log(data);
                return null;
            });
    }

    $scope.LoadData();
}])
.directive("sessionAudience", function () {
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
})
.directive("count", function (codeCampServiceFactory) {
    return {
        restrict: "E",
        replace: true,
        scope: {
            session: "=",
            sessionRegistration: "="
        },
        link: function (scope, elem, attrs) {
            scope.$watch("sessionRegistration", function () {
                var factory = codeCampServiceFactory;
                factory.init(moduleId, moduleName);

                scope.registrantCount = 0;

                factory.callGetService("GetSessionRegistrations?sessionId=" + scope.session.SessionId)
                    .then(function (response) {
                        var fullResult = angular.fromJson(response);
                        var serviceResponse = JSON.parse(fullResult.data);

                        if (serviceResponse.Content != null) {
                            scope.registrantCount = serviceResponse.Content.length;
                        }

                        LogErrors(serviceResponse.Errors);
                    },
                    function (data) {
                        console.log("Unknown error occurred calling GetSessionRegistrationByRegistrantId");
                        console.log(data);
                    }, true);
            });
        },
        template: "<span>{{registrantCount}}</span>"
    }
});