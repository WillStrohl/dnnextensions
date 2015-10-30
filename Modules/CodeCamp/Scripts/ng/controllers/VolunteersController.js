"use strict";

codeCampControllers.controller("volunteersController", ["$scope", "$routeParams", "$http", "$location", "$uibModal", "codeCampServiceFactory", function ($scope, $routeParams, $http, $location, $uibModal, codeCampServiceFactory) {

    var factory = codeCampServiceFactory;
    factory.init(moduleId, moduleName);

    $scope.hasCodeCamp = false;
    $scope.userCanEdit = false;
    $scope.userIsRegistered = false;
    $scope.userIsVolunteer = false;
    $scope.userHasTasks = false;
    $scope.volunteerTasks = [];
    $scope.tasks = [];

    $scope.LoadData = function () {
        factory.callGetService("GetCurrentUserId")
            .then(function (response) {
                var fullResult = angular.fromJson(response);
                var serviceResponse = JSON.parse(fullResult.data);

                $scope.currentUserId = serviceResponse.Content;

                $scope.LoadEventDetails();

                LogErrors(serviceResponse.Errors);
            },
                function (data) {
                    console.log("Unknown error occurred calling GetCurrentUserId");
                    console.log(data);
                });
    }

    $scope.LoadEventDetails = function () {
        factory.callGetService("GetEventByModuleId")
            .then(function (response) {
                var fullResult = angular.fromJson(response);
                var serviceResponse = JSON.parse(fullResult.data);

                $scope.codeCamp = serviceResponse.Content;

                if ($scope.codeCamp === null) {
                    $scope.hasCodeCamp = false;
                } else {
                    $scope.hasCodeCamp = true;

                    if ($scope.codeCamp != null) {
                        $scope.codeCamp.BeginDate = ParseDate($scope.codeCamp.BeginDate);
                        $scope.codeCamp.EndDate = ParseDate($scope.codeCamp.EndDate);
                    }

                    $scope.LoadEditPermissions();
                }

                LogErrors(serviceResponse.Errors);
            },
            function (data) {
                console.log("Unknown error occurred calling GetEventByModuleId");
                console.log(data);
            });
    }

    $scope.LoadEditPermissions = function () {
        factory.callGetService("UserCanEditEvent?itemId=" + $scope.codeCamp.CodeCampId)
            .then(function (response) {
                var fullResult = angular.fromJson(response);
                var serviceResponse = JSON.parse(fullResult.data);

                $scope.userCanEdit = (serviceResponse.Content == "Success");

                $scope.LoadRegistration();
                $scope.LoadVolunteers();

                LogErrors(serviceResponse.Errors);
            },
            function (data) {
                console.log("Unknown error occurred calling UserCanEditEvent");
                console.log(data);
            });
    }

    $scope.LoadRegistration = function () {
        factory.callGetService("GetRegistrationByUserId?userId=" + $scope.currentUserId + "&codeCampId=" + $scope.codeCamp.CodeCampId)
            .then(function (response) {
                var fullResult = angular.fromJson(response);
                var serviceResponse = JSON.parse(fullResult.data);

                $scope.currentUserRegistration = serviceResponse.Content;

                $scope.userIsRegistered = ($scope.currentUserRegistration != null);

                $scope.LoadVolunteer();

                LogErrors(serviceResponse.Errors);
            },
            function (data) {
                console.log("Unknown error occurred calling GetRegistrationByUserId");
                console.log(data);
            });
    }

    $scope.LoadVolunteer = function () {
        if ($scope.currentUserRegistration != null) {
            factory.callGetService("GetVolunteerByRegistrationId?registrationId=" + $scope.currentUserRegistration.RegistrationId + "&codeCampId=" + $scope.codeCamp.CodeCampId)
                .then(function (response) {
                    var fullResult = angular.fromJson(response);
                    var serviceResponse = JSON.parse(fullResult.data);

                    $scope.volunteer = serviceResponse.Content;

                    $scope.userIsVolunteer = ($scope.volunteer != null);

                    //console.log("$scope.volunteer = " + $scope.volunteer);
                    //console.log("$scope.userIsVolunteer = " + $scope.userIsVolunteer);

                    $scope.LoadTasks();

                    LogErrors(serviceResponse.Errors);
                },
                    function (data) {
                        console.log("Unknown error occurred calling GetVolunteerByRegistrationId");
                        console.log(data);
                    });
        }
    }

    $scope.LoadTasks = function () {
        if ($scope.userIsVolunteer) {
            factory.callGetService("GetVolunteerTasks?volunteerId=" + $scope.volunteer.VolunteerId)
                .then(function (response) {
                    var fullResult = angular.fromJson(response);
                    var serviceResponse = JSON.parse(fullResult.data);

                    $scope.volunteerTasks = serviceResponse.Content;

                    $scope.userHasTasks = ($scope.volunteerTasks.length > 0);

                    LogErrors(serviceResponse.Errors);
                },
                    function (data) {
                        console.log("Unknown error occurred calling GetVolunteerTasks");
                        console.log(data);
                    });
        }
    }

    $scope.LoadVolunteers = function () {
        if ($scope.userCanEdit) {
            factory.callGetService("GetVolunteers?codeCampId=" + $scope.codeCamp.CodeCampId)
                .then(function (response) {
                    var fullResult = angular.fromJson(response);
                    var serviceResponse = JSON.parse(fullResult.data);

                    $scope.volunteers = serviceResponse.Content;

                    LogErrors(serviceResponse.Errors);
                },
                    function (data) {
                        console.log("Unknown error occurred calling GetVolunteers");
                        console.log(data);
                    });
        }
    }

    $scope.goToPage = function (pageName) {
        $location.path(pageName);
    }

    $scope.openVolunteerSubmission = function () {
        var modalInstance = $uibModal.open({
            templateUrl: "AddVolunteerModal.html",
            controller: "AddVolunteerModalController",
            size: "md",
            backdrop: "static",
            scope: $scope,
            resolve: {
                userId: function () {
                    return $scope.currentUserId;
                },
                codeCamp: function () {
                    return $scope.codeCamp;
                },
                volunteerId: function () {
                    if ($scope.volunteer != null) {
                        return $scope.volunteer.VolunteerId;
                    } else {
                        return null;
                    }
                },
                registrationId: function () {
                    return $scope.currentUserRegistration.RegistrationId;
                }
            }
        });

        modalInstance.result.then(function (savedVolunteer) {
            $scope.savedVolunteer = savedVolunteer;
            console.log("$scope.savedVolunteer = " + $scope.savedVolunteer);
            $scope.LoadVolunteer();
        }, function () {
            console.log("Modal dismissed at: " + new Date());
        });
    };

    $scope.LoadData();

}]);

/*
 * Volunteer Modal Controller
 */
codeCampApp.controller("AddVolunteerModalController", ["$scope", "$rootScope", "$uibModalInstance", "userId", "codeCamp", "volunteerId", "registrationId", "codeCampServiceFactory", function ($scope, $rootScope, $uibModalInstance, userId, codeCamp, volunteerId, registrationId, codeCampServiceFactory) {

    var factory = codeCampServiceFactory;
    factory.init(moduleId, moduleName);

    $scope.CodeCampId = codeCamp.CodeCampId;

    $scope.volunteer = {};

    $scope.UpdateMode = (volunteerId != undefined && volunteerId > -1);

    if ($scope.UpdateMode) {
        factory.callGetService("GetVolunteer?itemId=" + volunteerId + "&codeCampId=" + $scope.CodeCampId)
            .then(function (response) {
                var fullResult = angular.fromJson(response);
                var serviceResponse = JSON.parse(fullResult.data);

                $scope.volunteer = serviceResponse.Content;

                LogErrors(serviceResponse.Errors);
            },
            function (data) {
                console.log("Unknown error occurred calling GetVolunteer");
                console.log(data);
            });
    }

    $scope.ok = function () {
        $scope.volunteer.RegistrationId = registrationId;
        $scope.volunteer.CodeCampId = $scope.CodeCampId;

        var volunteerAction = ($scope.UpdateMode) ? "UpdateVolunteer" : "CreateVolunteer";

        // save the track
        factory.callPostService(volunteerAction, $scope.volunteer)
            .success(function (data) {
                var savedVolunteer = angular.fromJson(data);
                $scope.savedVolunteer = savedVolunteer.Content;

                $scope.LoadVolunteer();

                LogErrors(savedVolunteer.Errors);
            })
            .error(function (data, status) {
                $scope.HasErrors = true;
                console.log("Unknown error occurred calling " + volunteerAction);
                console.log(data);
            });

        $uibModalInstance.close($scope.savedVolunteer);
    };

    $scope.cancel = function () {
        $uibModalInstance.dismiss("cancel");
    };
}]);