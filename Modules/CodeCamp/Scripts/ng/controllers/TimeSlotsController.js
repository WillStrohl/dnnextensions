"use strict";

codeCampControllers.controller("timeSlotsController", ["$scope", "$routeParams", "$http", "$location", "$modal", "codeCampServiceFactory", function ($scope, $routeParams, $http, $location, $modal, codeCampServiceFactory) {

    var factory = codeCampServiceFactory;
    factory.init(moduleId, moduleName);

    $scope.userCanEdit = false;
    $scope.timeSlots = [];

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

                    $scope.getCurrentUserId();

                    $scope.LoadTimeSlots();
                }

                LogErrors(serviceResponse.Errors);
            },
                function (data) {
                    console.log("Unknown error occurred calling GetEventByModuleId");
                    console.log(data);
                });
    }

    $scope.LoadTimeSlots = function () {
        factory.callGetService("GetTimeSlots?codeCampId=" + $scope.codeCamp.CodeCampId)
            .then(function (response) {
                var fullResult = angular.fromJson(response);
                var serviceResponse = JSON.parse(fullResult.data);

                $scope.timeSlots = serviceResponse.Content;

                LogErrors(serviceResponse.Errors);
            },
                function (data) {
                    console.log("Unknown error occurred calling GetTimeSlots");
                    console.log(data);
                });
    }

    $scope.LoadEditPermissions = function () {
        factory.callGetService("UserCanEditEvent?itemId=" + $scope.codeCamp.CodeCampId)
            .then(function (response) {
                var fullResult = angular.fromJson(response);
                var serviceResponse = JSON.parse(fullResult.data);

                $scope.userCanEdit = (serviceResponse.Content == "Success");

                LogErrors(serviceResponse.Errors);
            },
                function (data) {
                    console.log("Unknown error occurred calling UserCanEditEvent");
                    console.log(data);
                });
    }

    $scope.getCurrentUserId = function () {
        factory.callGetService("GetCurrentUserId")
            .then(function (response) {
                var fullResult = angular.fromJson(response);
                var serviceResponse = JSON.parse(fullResult.data);

                $scope.currentUserId = serviceResponse.Content;

                LogErrors(serviceResponse.Errors);
            },
                function (data) {
                    console.log("Unknown error occurred calling GetCurrentUserId");
                    console.log(data);
                });
    }

    $scope.goToPage = function (pageName) {
        $location.path(pageName);
    }

    $scope.DeleteTimeSlot = function (timeSlotId) {
        var modalInstance = $modal.open({
            templateUrl: "DeleteTimeSlotModal.html",
            controller: "DeleteTimeSlotModalController",
            size: "sm",
            backdrop: "static",
            scope: $scope,
            resolve: {
                timeSlotId: function () {
                    return timeSlotId;
                },
                codeCampId: function () {
                    return $scope.codeCamp.CodeCampId;
                }
            }
        });

        modalInstance.result.then(function () {
            $scope.LoadTimeSlots();
        }, function () {
            console.log("Modal dismissed at: " + new Date());
        });
    }

    $scope.EditTimeSlot = function (timeSlotId) {

        var modalInstance = $modal.open({
            templateUrl: "AddTimeSlotModal.html",
            controller: "AddTimeSlotModalController",
            size: "md",
            backdrop: "static",
            scope: $scope,
            resolve: {
                codeCamp: function () {
                    return $scope.codeCamp;
                },
                timeSlotId: function () {
                    return timeSlotId;
                }
            }
        });

        modalInstance.result.then(function (savedTimeSlot) {
            $scope.savedTimeSlot = savedTimeSlot;
            console.log("$scope.savedTimeSlot = " + $scope.savedTimeSlot);
            $scope.LoadTimeSlots();
        }, function () {
            console.log("Modal dismissed at: " + new Date());
        });
    };

    $scope.LoadData();

}]);

/*
 * Time Slot Modal Controller
 */
codeCampApp.controller("AddTimeSlotModalController", ["$scope", "$rootScope", "$modalInstance", "codeCamp", "timeSlotId", "codeCampServiceFactory", function ($scope, $rootScope, $modalInstance, codeCamp, timeSlotId, codeCampServiceFactory) {

    var factory = codeCampServiceFactory;
    factory.init(moduleId, moduleName);

    $scope.CodeCampId = codeCamp.CodeCampId;
    $scope.timeSlot = {};
    $scope.hourSteps = [1, 2, 3];
    $scope.minuteSteps = [1, 5, 10, 15, 25, 30];
    $scope.UpdateMode = (timeSlotId != undefined && timeSlotId > -1);

    /*
     * TODO: 
     * - validate the times?
     * - ensure that the begin time comes before the end time
     * - prevent timeslot overlap
     *   - time slots can't touch each other
     *   - time slots can't over lap
     *   - time slots can't be duplicated
     */

    $scope.Init = function () {
        var beginDate = new Date();
        beginDate.setHours(8);
        beginDate.setMinutes(0);

        $scope.timeSlot.BeginTime = beginDate;

        var endDate = new Date();
        endDate.setHours(9);
        endDate.setMinutes(0);

        $scope.timeSlot.EndDate = endDate;

        $scope.LoadData();
    }

    $scope.LoadData = function() {
        if ($scope.UpdateMode) {
            factory.callGetService("GetTimeSlot?itemId=" + roomId + "&codeCampId=" + $scope.CodeCampId)
                .then(function(response) {
                    var fullResult = angular.fromJson(response);
                    var serviceResponse = JSON.parse(fullResult.data);

                    $scope.timeSlot = serviceResponse.Content;

                    LogErrors(serviceResponse.Errors);
                },
                    function(data) {
                        console.log("Unknown error occurred calling GetTimeSlot");
                        console.log(data);
                    });
        }
    }

    $scope.ok = function () {
        $scope.timeSlot.CodeCampId = $scope.CodeCampId;

        var timeSlotAction = ($scope.UpdateMode) ? "UpdateTimeSlot" : "CreateTimeSlot";

        factory.callPostService(timeSlotAction, $scope.timeSlot)
            .success(function (data) {
                var savedTimeSlot = angular.fromJson(data);
                $scope.savedTimeSlot = savedTimeSlot.Content;

                LogErrors(savedTimeSlot.Errors);
            })
            .error(function (data, status) {
                $scope.HasErrors = true;
                console.log("Unknown error occurred calling " + timeSlotAction);
                console.log(data);
            });

        $modalInstance.close($scope.savedTimeSlot);
    };

    $scope.cancel = function () {
        $modalInstance.dismiss("cancel");
    };

    $scope.Init();
}]);