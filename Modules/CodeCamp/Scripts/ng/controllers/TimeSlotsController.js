"use strict";

codeCampControllers.controller("timeSlotsController", ["$scope", "$routeParams", "$http", "$location", "$uibModal", "codeCampServiceFactory", function ($scope, $routeParams, $http, $location, $uibModal, codeCampServiceFactory) {

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

                angular.forEach($scope.timeSlots, function (timeSlot, index) {
                    var beginDateTime = moment(timeSlot.BeginTime);

                    var seconds = beginDateTime.seconds();
                    var minutes = beginDateTime.minutes();
                    var hours = beginDateTime.hours();

                    timeSlot.sortTime = hours * 60 * 60 + minutes * 60 + seconds;
                });

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
        var modalInstance = $uibModal.open({
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

        var modalInstance = $uibModal.open({
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
codeCampApp.controller("AddTimeSlotModalController", ["$scope", "$rootScope", "$uibModalInstance", "codeCamp", "timeSlotId", "codeCampServiceFactory", function ($scope, $rootScope, $uibModalInstance, codeCamp, timeSlotId, codeCampServiceFactory) {

    var factory = codeCampServiceFactory;
    factory.init(moduleId, moduleName);

    $scope.HasTimeSlotError = false;
    $scope.CodeCampId = codeCamp.CodeCampId;
    $scope.timeSlot = {};
    $scope.hourSteps = [1, 2, 3];
    $scope.minuteSteps = [1, 5, 10, 15, 25, 30];
    $scope.UpdateMode = (timeSlotId != undefined && timeSlotId > -1);

    $scope.LoadTimeSlots = function () {
        factory.callGetService("GetTimeSlots?codeCampId=" + $scope.CodeCampId)
            .then(function (response) {
                var fullResult = angular.fromJson(response);
                var serviceResponse = JSON.parse(fullResult.data);

                $scope.timeSlots = serviceResponse.Content;

                angular.forEach($scope.timeSlots, function(timeSlot, index) {
                    timeSlot.BeginTime = ParseDate(timeSlot.BeginTime);
                    timeSlot.EndTime = ParseDate(timeSlot.EndTime);
                });

                LogErrors(serviceResponse.Errors);
            },
                function (data) {
                    console.log("Unknown error occurred calling GetTimeSlots");
                    console.log(data);
                });
    }

    $scope.TimeChanged = function() {
        if ($scope.timeSlot.EndTime <= $scope.timeSlot.BeginTime) {
            $scope.timeSlot.EndTime = addMinutes($scope.timeSlot.BeginTime, 45);
        }
    }

    $scope.InitTimePicker = function () {
        var beginDate = new Date();
        beginDate.setHours(8);
        beginDate.setMinutes(0);

        $scope.timeSlot.BeginTime = beginDate;

        var endDate = new Date();
        endDate.setHours(9);
        endDate.setMinutes(0);

        $scope.timeSlot.EndTime = endDate;
    }

    $scope.LoadData = function() {
        if ($scope.UpdateMode) {
            factory.callGetService("GetTimeSlot?itemId=" + timeSlotId + "&codeCampId=" + $scope.CodeCampId)
                .then(function(response) {
                    var fullResult = angular.fromJson(response);
                    var serviceResponse = JSON.parse(fullResult.data);

                    $scope.timeSlot = serviceResponse.Content;

                    if ($scope.timeSlot == null) {
                        $scope.InitTimePicker();
                    } else {
                        $scope.timeSlot.BeginTime = ParseDate($scope.timeSlot.BeginTime);
                        $scope.timeSlot.EndTime = ParseDate($scope.timeSlot.EndTime);
                    }

                    LogErrors(serviceResponse.Errors);
                },
                    function(data) {
                        console.log("Unknown error occurred calling GetTimeSlot");
                        console.log(data);
                    });
        }

        $scope.LoadTimeSlots();
    }

    $scope.ok = function () {
        $scope.CheckForErrors();
    };

    $scope.SaveTimeSlot = function() {
        $scope.timeSlot.CodeCampId = $scope.CodeCampId;

        var timeSlotAction = ($scope.UpdateMode) ? "UpdateTimeSlot" : "CreateTimeSlot";

        factory.callPostService(timeSlotAction, $scope.timeSlot)
            .success(function (data) {
                var savedTimeSlot = angular.fromJson(data);
                $scope.savedTimeSlot = savedTimeSlot.Content;

                $scope.LoadTimeSlots();

                LogErrors(savedTimeSlot.Errors);

                $uibModalInstance.close($scope.savedTimeSlot);
            })
            .error(function (data, status) {
                $scope.HasErrors = true;
                console.log("Unknown error occurred calling " + timeSlotAction);
                console.log(data);
            });
    }

    $scope.CheckForErrors = function () {
        var hasError = false;

        angular.forEach($scope.timeSlots, function (timeSlot, index) {
            if (timeSlot.timeSlotId == $scope.timeSlot.timeSlotId) return;

            var beginTimeHasIssue = ($scope.timeSlot.BeginTime >= timeSlot.BeginTime && $scope.timeSlot.BeginTime <= timeSlot.EndTime);
            var endTimeHasIssue = ($scope.timeSlot.EndTime >= timeSlot.BeginTime && $scope.timeSlot.EndTime <= timeSlot.EndTime);

            if (beginTimeHasIssue || endTimeHasIssue) {
                hasError = true;
                $scope.HasTimeSlotError = true;
                return;
            } 
        });

        if (!hasError) {
            $scope.SaveTimeSlot();
        }
    }

    $scope.cancel = function () {
        $uibModalInstance.dismiss("cancel");
    };

    $scope.LoadData();
}]);

/*
 * Delete Modal
 */
codeCampApp.controller("DeleteTimeSlotModalController", ["$scope", "$rootScope", "$uibModalInstance", "timeSlotId", "codeCampId", "codeCampServiceFactory", function ($scope, $rootScope, $uibModalInstance, timeSlotId, codeCampId, codeCampServiceFactory) {

    var factory = codeCampServiceFactory;
    factory.init(moduleId, moduleName);

    $scope.LoadData = function () {
        factory.callGetService("GetTimeSlot?itemId=" + timeSlotId + "&codeCampId=" + codeCampId)
            .then(function (response) {
                var fullResult = angular.fromJson(response);
                var serviceResponse = JSON.parse(fullResult.data);

                $scope.timeSlot = serviceResponse.Content;

                if ($scope.timeSlot != null) {
                    $scope.timeSlot.BeginTime = ParseDate($scope.timeSlot.BeginTime);
                    $scope.timeSlot.EndTime = ParseDate($scope.timeSlot.EndTime);
                }

                LogErrors(serviceResponse.Errors);
            },
                function (data) {
                    console.log("Unknown error occurred calling GetTimeSlot");
                    console.log(data);
                });
    }

    $scope.ok = function () {
        factory.callDeleteService("DeleteTimeSlot?itemId=" + timeSlotId + "&codeCampId=" + codeCampId)
            .then(function (response) {
                var fullResult = angular.fromJson(response);
                var serviceResponse = JSON.parse(fullResult.data);
                console.log("serviceResponse = " + serviceResponse);

                $scope.LoadTimeSlots();

                LogErrors(serviceResponse.Errors);
            },
                function (data) {
                    console.log("Unknown error occurred calling DeleteTimeSlot");
                    console.log(data);
                });

        $uibModalInstance.close();
    };

    $scope.cancel = function () {
        $uibModalInstance.dismiss("cancel");
    };

    $scope.LoadData();
}]);