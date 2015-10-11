"use strict";

codeCampControllers.controller("registerController", ["$scope", "$routeParams", "$location", "$http", "codeCampServiceFactory", function ($scope, $routeParams, $location, $http, codeCampServiceFactory) {

    $scope.RegistrationErrors = false;
    $scope.UserExistsErrors = false;
    $scope.RegistrationSuccess = false;

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

    $scope.createNewRegistration = function () {

        $scope.registration.CodeCampId = $scope.codeCamp.CodeCampId;
        $scope.registration.UserId = $scope.userInfo.UserID;
        $scope.registration.CustomProperties = [];

        $scope.registration.CustomProperties.push({ Name: "FirstName", Value: $scope.userInfo.FirstName });
        $scope.registration.CustomProperties.push({ Name: "LastName", Value: $scope.userInfo.LastName });
        $scope.registration.CustomProperties.push({ Name: "Email", Value: $scope.userInfo.Email });
        $scope.registration.CustomProperties.push({ Name: "PortalId", Value: portalId });
        
        // save the speaker
        factory.callPostService("CreateRegistration", $scope.registration)
            .success(function (data) {
                var savedRegistration = angular.fromJson(data);
                $scope.savedRegistration = savedSpeaker.Content;
                console.log("savedRegistration = " + savedRegistration);

                if (savedRegistration.Errors != null) {
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
                }

                LogErrors(savedRegistration.Errors);
            })
            .error(function (data, status) {
                $scope.HasErrors = true;
                console.log("Unknown error occurred calling CreateRegistration");
                console.log(data);
            });

    }

    $scope.goToPage = function (pageName) {
        $location.path(pageName);
    }

}]);