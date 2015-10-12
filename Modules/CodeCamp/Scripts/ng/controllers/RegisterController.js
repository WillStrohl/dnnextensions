"use strict";

codeCampControllers.controller("registerController", ["$scope", "$routeParams", "$location", "$window", "$http", "codeCampServiceFactory", function ($scope, $routeParams, $location, $window, $http, codeCampServiceFactory) {

    $scope.RegistrationErrors = false;
    $scope.UserExistsErrors = false;
    $scope.RegistrationSuccess = false;
    $scope.RegistrationUpdateSuccess = false;
    $scope.RegistrationUpdateError = false;

    var factory = codeCampServiceFactory;
    factory.init(moduleId, moduleName);

    factory.callGetService("GetCurrentUser")
        .then(function (response) {
            var fullResult = angular.fromJson(response);
            var serviceResponse = JSON.parse(fullResult.data);

            $scope.userInfo = serviceResponse.Content;

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

                        factory.callGetService("GetRegistrationByUserId?userId=" + $scope.userInfo.UserID + "&codeCampId=" + $scope.codeCamp.CodeCampId)
                            .then(function (response) {
                                var fullResult = angular.fromJson(response);
                                var serviceResponse = JSON.parse(fullResult.data);

                                $scope.registration = serviceResponse.Content;

                                LogErrors(serviceResponse.Errors);
                            },
                            function (data) {
                                console.log("Unknown error occurred calling GetRegistrationByUserId");
                                console.log(data);
                            });
                    }

                    LogErrors(serviceResponse.Errors);
                },
                function (data) {
                    console.log("Unknown error occurred calling GetEventByModuleId");
                    console.log(data);
                });

            LogErrors(serviceResponse.Errors);
        },
        function (data) {
            console.log("Unknown error occurred calling GetCurrentUser");
            console.log(data);
        });

    $scope.processRegistration = function () {

        if ($scope.userInfo.UserID > -1) {
            // update existing registration
            factory.callPostService("UpdateRegistration", $scope.registration)
                .success(function (data) {
                    var serviceResponse = angular.fromJson(data);
                    console.log("update registration response = " + serviceResponse.Content);

                    if (serviceResponse.Content == "SUCCESS") {
                        $scope.RegistrationUpdateSuccess = true;

                        setTimeout(function() { $window.location.href = pageUrl; }, 3000);
                    } else {
                        $scope.RegistrationUpdateError = true;
                    }

                    LogErrors(serviceResponse.Errors);
                })
                .error(function (data, status) {
                    $scope.HasErrors = true;
                    console.log("Unknown error occurred calling CreateRegistration");
                    console.log(data);
                });
        } else {
            // create new registration
            $scope.registration.CodeCampId = $scope.codeCamp.CodeCampId;
            $scope.registration.UserId = $scope.userInfo.UserID;
            $scope.registration.CustomPropertiesObj = [];

            $scope.registration.CustomPropertiesObj.push({ Name: "FirstName", Value: $scope.userInfo.FirstName });
            $scope.registration.CustomPropertiesObj.push({ Name: "LastName", Value: $scope.userInfo.LastName });
            $scope.registration.CustomPropertiesObj.push({ Name: "Email", Value: $scope.userInfo.Email });
            $scope.registration.CustomPropertiesObj.push({ Name: "PortalId", Value: portalId });

            factory.callPostService("CreateRegistration", $scope.registration)
                .success(function(data) {
                    var savedRegistration = angular.fromJson(data);
                    $scope.savedRegistration = savedRegistration.Content;
                    //console.log("savedRegistration = " + savedRegistration);

                    if (savedRegistration.Errors.length > 0) {
                        $.each(savedRegistration.Errors, function(index, error) {
                            if (error.Code == "USER-CREATE-ERROR" && $scope.UserExistsErrors == false) {
                                $scope.UserExistsErrors = true;
                            }
                            if (error.Code == "UNKNOWN-ERROR" && $scope.RegistrationErrors == false) {
                                $scope.RegistrationErrors = true;
                            }
                        });
                    } else {
                        $scope.RegistrationSuccess = true;

                        setTimeout(function() { $window.location.href = pageUrl; }, 3000);
                    }

                    LogErrors(savedRegistration.Errors);
                })
                .error(function(data, status) {
                    $scope.HasErrors = true;
                    console.log("Unknown error occurred calling CreateRegistration");
                    console.log(data);
                });
        }

    }

    $scope.goToPage = function (pageName) {
        $location.path(pageName);
    }

}]);