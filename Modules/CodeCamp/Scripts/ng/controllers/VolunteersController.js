"use strict";

codeCampControllers.controller("volunteersController", ["$scope", "$routeParams", "$http", "$location", "codeCampServiceFactory", function ($scope, $routeParams, $http, $location, codeCampServiceFactory) {

    var factory = codeCampServiceFactory;
    factory.init(moduleId, moduleName);

    $scope.hasCodeCamp = false;
    $scope.userCanEdit = false;
    $scope.userIsRegistered = false;
    $scope.userIsVolunteer = false;
    $scope.userHasTasks = false;

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
        factory.callGetService("GetVolunteerByRegistrationId?registrationId=" + $scope.codeCamp.CodeCampId + "&codeCampId=" + $scope.currentUserRegistration.RegistrationId)
            .then(function (response) {
                var fullResult = angular.fromJson(response);
                var serviceResponse = JSON.parse(fullResult.data);

                $scope.volunteer = serviceResponse.Content;

                $scope.userIsVolunteer = ($scope.volunteer != null);

                //console.log("$scope.volunteer = " + $scope.volunteer);
                //console.log("$scope.userIsVolunteer = " + $scope.userIsVolunteer);

                $scope.LoadVolunteerTasks();

                LogErrors(serviceResponse.Errors);
            },
            function (data) {
                console.log("Unknown error occurred calling UserCanEditEvent");
                console.log(data);
            });
    }

    $scope.LoadVolunteerTasks = function () {
        if ($scope.userIsVolunteer) {
            factory.callGetService("GetVolunteerTasks?volunteerId=" + $scope.volunteer.VolunteerId)
                .then(function (response) {
                    var fullResult = angular.fromJson(response);
                    var serviceResponse = JSON.parse(fullResult.data);

                    $scope.volunteerTasks = serviceResponse.Content;

                    $scope.userHasTasks = ($scope.volunteerTasks.length > 0);

                    //console.log("$scope.volunteerTasks = " + $scope.volunteerTasks);
                    //console.log("$scope.userHasTasks = " + $scope.userHasTasks);

                    LogErrors(serviceResponse.Errors);
                },
                    function (data) {
                        console.log("Unknown error occurred calling UserCanEditEvent");
                        console.log(data);
                    });
        }
    }

    $scope.goToPage = function (pageName) {
        $location.path(pageName);
    }

    $scope.LoadData();

}]);