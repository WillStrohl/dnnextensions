"use strict";

codeCampControllers.controller("roomsController", ["$scope", "$routeParams", "$http", "$location", "$modal", "codeCampServiceFactory", function ($scope, $routeParams, $http, $location, $modal, codeCampServiceFactory) {
    
    var factory = codeCampServiceFactory;
    factory.init(moduleId, moduleName);

    $scope.rooms = [];
    $scope.userCanEdit = false;
    $scope.classColors = ["purple", "red", "green", "blue", "orange"];

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

                    $scope.LoadRooms();
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

                LogErrors(serviceResponse.Errors);
            },
                function (data) {
                    console.log("Unknown error occurred calling UserCanEditEvent");
                    console.log(data);
                });
    }

    $scope.LoadRooms = function () {
        factory.callGetService("GetRooms?codeCampId=" + $scope.codeCamp.CodeCampId)
            .then(function (response) {
                var fullResult = angular.fromJson(response);
                var serviceResponse = JSON.parse(fullResult.data);

                $scope.rooms = serviceResponse.Content;

                LogErrors(serviceResponse.Errors);
            },
                function (data) {
                    console.log("Unknown error occurred calling GetRooms");
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

    $scope.DeleteRoom = function (roomId) {
        var modalInstance = $modal.open({
            templateUrl: "DeleteRoomModal.html",
            controller: "DeleteRoomModalController",
            size: "sm",
            backdrop: "static",
            scope: $scope,
            resolve: {
                roomId: function () {
                    return roomId;
                },
                codeCampId: function () {
                    return $scope.codeCamp.CodeCampId;
                }
            }
        });

        modalInstance.result.then(function () {
            $scope.LoadRooms();
        }, function () {
            console.log("Modal dismissed at: " + new Date());
        });
    }

    $scope.goToPage = function (pageName) {
        $location.path(pageName);
    }

    $scope.EditRoom = function (roomId) {

        var modalInstance = $modal.open({
            templateUrl: "AddRoomModal.html",
            controller: "AddRoomModalController",
            size: "md",
            backdrop: "static",
            scope: $scope,
            resolve: {
                codeCamp: function () {
                    return $scope.codeCamp;
                },
                roomId: function () {
                    return roomId;
                }
            }
        });

        modalInstance.result.then(function (savedRoom) {
            $scope.savedRoom = savedRoom;
            console.log("$scope.savedRoom = " + $scope.savedRoom);
            $scope.LoadRooms();
        }, function () {
            console.log("Modal dismissed at: " + new Date());
        });
    };

    $scope.LoadData();

}]);

/*
 * Room Modal Controller
 */
codeCampApp.controller("AddRoomModalController", ["$scope", "$rootScope", "$modalInstance", "codeCamp", "roomId", "codeCampServiceFactory", function ($scope, $rootScope, $modalInstance, codeCamp, roomId, codeCampServiceFactory) {

    var factory = codeCampServiceFactory;
    factory.init(moduleId, moduleName);

    $scope.CodeCampId = codeCamp.CodeCampId;

    $scope.room = {};

    $scope.UpdateMode = (roomId != undefined && roomId > -1);

    if ($scope.UpdateMode) {
        factory.callGetService("GetRoom?itemId=" + roomId + "&codeCampId=" + $scope.CodeCampId)
            .then(function (response) {
                var fullResult = angular.fromJson(response);
                var serviceResponse = JSON.parse(fullResult.data);

                $scope.room = serviceResponse.Content;

                LogErrors(serviceResponse.Errors);
            },
            function (data) {
                console.log("Unknown error occurred calling GetRoom");
                console.log(data);
            });
    }

    $scope.ok = function () {
        $scope.room.CodeCampId = $scope.CodeCampId;

        var roomAction = ($scope.UpdateMode) ? "UpdateRoom" : "CreateRoom";

        // save the track
        factory.callPostService(roomAction, $scope.room)
            .success(function (data) {
                var savedRoom = angular.fromJson(data);
                $scope.savedRoom = savedRoom.Content;

                $scope.LoadRooms();

                LogErrors(savedRoom.Errors);
            })
            .error(function (data, status) {
                $scope.HasErrors = true;
                console.log("Unknown error occurred calling " + roomAction);
                console.log(data);
            });

        $modalInstance.close($scope.savedRoom);
    };

    $scope.cancel = function () {
        $modalInstance.dismiss("cancel");
    };
}]);

/*
 * Delete Modal
 */
codeCampApp.controller("DeleteRoomModalController", ["$scope", "$rootScope", "$modalInstance", "roomId", "codeCampId", "codeCampServiceFactory", function ($scope, $rootScope, $modalInstance, roomId, codeCampId, codeCampServiceFactory) {

    var factory = codeCampServiceFactory;
    factory.init(moduleId, moduleName);

    $scope.room = {};

    factory.callGetService("GetRoom?itemId=" + roomId + "&codeCampId=" + codeCampId)
        .then(function (response) {
            var fullResult = angular.fromJson(response);
            var serviceResponse = JSON.parse(fullResult.data);

            $scope.room = serviceResponse.Content;

            LogErrors(serviceResponse.Errors);
        },
        function (data) {
            console.log("Unknown error occurred calling GetRoom");
            console.log(data);
        });

    $scope.ok = function () {
        factory.callDeleteService("DeleteRoom?itemId=" + roomId + "&codeCampId=" + $scope.codeCamp.CodeCampId)
            .then(function (response) {
                var fullResult = angular.fromJson(response);
                var serviceResponse = JSON.parse(fullResult.data);

                $scope.LoadRooms();

                LogErrors(serviceResponse.Errors);
            },
                function (data) {
                    console.log("Unknown error occurred calling DeleteRoom");
                    console.log(data);
                });

        $modalInstance.close();
    };

    $scope.cancel = function () {
        $modalInstance.dismiss("cancel");
    };
}]);