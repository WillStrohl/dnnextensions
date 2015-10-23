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

                angular.forEach($scope.rooms, function (room, index) {
                    room.Track = [];
                    room.Track = $scope.getTrack(room.RoomId);
                });

                LogErrors(serviceResponse.Errors);
            },
                function (data) {
                    console.log("Unknown error occurred calling GetRooms");
                    console.log(data);
                });
    }

    $scope.getTrack = function (roomId) {
        factory.callGetService("GetTrackByRoomId?roomId=" + roomId + "&codeCampId=" + $scope.codeCamp.CodeCampId)
            .then(function (response) {
                var fullResult = angular.fromJson(response);
                var serviceResponse = JSON.parse(fullResult.data);

                LogErrors(serviceResponse.Errors);

                return serviceResponse.Content;
            },
                function (data) {
                    console.log("Unknown error occurred calling GetTrack");
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
    $scope.availableTracks = [];
    $scope.selectedTrack = {};

    $scope.UpdateMode = (roomId != undefined && roomId > -1);

    /*
     * TODO:
     * - track drop down doesn't load a saved session
     * - load tracks isn't showing an assigned track in the room template
     * - how do we unassigne the track from here?
     */

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

    $scope.GetTrack = function () {
        if ($scope.room.roomId > -1) {
            factory.callGetService("GetTrackByRoomId?roomId=" + $scope.room.roomId + "&codeCampId=" + $scope.codeCamp.CodeCampId)
                .then(function (response) {
                    var fullResult = angular.fromJson(response);
                    var serviceResponse = JSON.parse(fullResult.data);

                    var track = serviceResponse.Content;

                    $scope.selectedTrack = track.TrackId;

                    LogErrors(serviceResponse.Errors);
                },
                    function (data) {
                        console.log("Unknown error occurred calling GetRooms");
                        console.log(data);
                    });
        } else {
            $scope.selectedTrack = -1;
        }
    }

    $scope.LoadTracks = function () {
        factory.callGetService("GetTracksWithoutRooms?codeCampId=" + $scope.codeCamp.CodeCampId)
            .then(function (response) {
                var fullResult = angular.fromJson(response);
                var serviceResponse = JSON.parse(fullResult.data);

                $scope.availableTracks = serviceResponse.Content;

                $scope.GetTrack();

                LogErrors(serviceResponse.Errors);
            },
                function (data) {
                    console.log("Unknown error occurred calling GetRooms");
                    console.log(data);
                });
    }

    $scope.ok = function () {
        $scope.room.CodeCampId = $scope.CodeCampId;

        var roomAction = ($scope.UpdateMode) ? "UpdateRoom" : "CreateRoom";

        factory.callPostService(roomAction, $scope.room)
            .success(function (data) {
                var savedRoom = angular.fromJson(data);
                $scope.savedRoom = savedRoom.Content;

                $scope.AssignRoomToTrack($scope.savedRoom.RoomId);

                LogErrors(savedRoom.Errors);
            })
            .error(function (data, status) {
                $scope.HasErrors = true;
                console.log("Unknown error occurred calling " + roomAction);
                console.log(data);
            });

        $modalInstance.close($scope.savedRoom);
    };

    $scope.AssignRoomToTrack = function (roomId) {
        if ($scope.selectedTrack.TrackId > -1) {
            factory.callPostService("AssignRoomToTrack?roomId=" + roomId + "&trackId=" + $scope.selectedTrack.TrackId + "&codeCampId=" + $scope.CodeCampId)
                .success(function (data) {
                    var fullResult = angular.fromJson(data);
                    var serviceResponse = JSON.parse(fullResult.data);

                    $scope.LoadRooms();

                    LogErrors(serviceResponse.Errors);
                })
                .error(function (data, status) {
                    $scope.HasErrors = true;
                    console.log("Unknown error occurred calling AssignRoomToTrack");
                    console.log(data);
                });
        }
    }

    $scope.cancel = function () {
        $modalInstance.dismiss("cancel");
    };

    $scope.LoadTracks();
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