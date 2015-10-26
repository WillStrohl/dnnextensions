"use strict";

codeCampControllers.controller("roomsController", ["$scope", "$routeParams", "$http", "$location", "$uibModal", "codeCampServiceFactory", function ($scope, $routeParams, $http, $location, $uibModal, codeCampServiceFactory) {

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
                    factory.callGetService("GetTrackByRoomId?roomId=" + room.RoomId + "&codeCampId=" + $scope.codeCamp.CodeCampId)
                        .then(function (response) {
                            var fullResult = angular.fromJson(response);
                            var serviceResponse = JSON.parse(fullResult.data);

                            room.Track = serviceResponse.Content;
                            if (room.Track != null && room.Track.Title != null) {
                                room.TrackSlug = GetSlugFromValue(room.Track.Title);
                            } else {
                                room.TrackSlug = null;
                            }

                            LogErrors(serviceResponse.Errors);
                        },
                            function (data) {
                                console.log("Unknown error occurred calling GetTrack");
                                console.log(data);
                            });
                });

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
        var modalInstance = $uibModal.open({
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

        var modalInstance = $uibModal.open({
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
codeCampApp.controller("AddRoomModalController", ["$scope", "$rootScope", "$uibModalInstance", "codeCamp", "roomId", "codeCampServiceFactory", function ($scope, $rootScope, $uibModalInstance, codeCamp, roomId, codeCampServiceFactory) {

    var factory = codeCampServiceFactory;
    factory.init(moduleId, moduleName);

    $scope.CodeCampId = codeCamp.CodeCampId;
    $scope.room = {};
    $scope.availableTracks = [];
    $scope.selectedTrack = {};

    $scope.UpdateMode = (roomId != undefined && roomId > -1);

    $scope.LoadData = function() {
        if ($scope.UpdateMode) {
            factory.callGetService("GetRoom?itemId=" + roomId + "&codeCampId=" + $scope.CodeCampId)
                .then(function(response) {
                        var fullResult = angular.fromJson(response);
                        var serviceResponse = JSON.parse(fullResult.data);

                        $scope.room = serviceResponse.Content;

                        $scope.LoadTracks();

                        LogErrors(serviceResponse.Errors);
                    },
                    function(data) {
                        console.log("Unknown error occurred calling GetRoom");
                        console.log(data);
                    });
        }
    }

    $scope.GetTrack = function () {
        factory.callGetService("GetTrackByRoomId?roomId=" + $scope.room.RoomId + "&codeCampId=" + $scope.codeCamp.CodeCampId)
            .then(function (response) {
                var fullResult = angular.fromJson(response);
                var serviceResponse = JSON.parse(fullResult.data);

                var track = serviceResponse.Content;

                $scope.selectedTrack = track;

                LogErrors(serviceResponse.Errors);
            },
                function (data) {
                    console.log("Unknown error occurred calling GetRooms");
                    console.log(data);
                });
    }

    $scope.LoadTracks = function () {
        factory.callGetService("GetTracks?codeCampId=" + $scope.codeCamp.CodeCampId)
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

        $uibModalInstance.close($scope.savedRoom);
    };

    $scope.AssignRoomToTrack = function (roomId) {
        if ($scope.selectedTrack != null && $scope.selectedTrack.TrackId > -1) {
            factory.callPostService("AssignRoomToTrack?roomId=" + roomId + "&trackId=" + $scope.selectedTrack.TrackId + "&codeCampId=" + $scope.CodeCampId)
                .success(function (data) {
                    var serviceResponse = angular.fromJson(data);

                    LogErrors(serviceResponse.Errors);
                })
                .error(function (data, status) {
                    $scope.HasErrors = true;
                    console.log("Unknown error occurred calling AssignRoomToTrack");
                    console.log(data);
                });
        }

        $scope.LoadRooms();
    }

    $scope.cancel = function () {
        $uibModalInstance.dismiss("cancel");
    };

    $scope.LoadData();
}]);

/*
 * Delete Modal
 */
codeCampApp.controller("DeleteRoomModalController", ["$scope", "$rootScope", "$modalInstance", "roomId", "codeCampId", "codeCampServiceFactory", function ($scope, $rootScope, $modalInstance, roomId, codeCampId, codeCampServiceFactory) {

    var factory = codeCampServiceFactory;
    factory.init(moduleId, moduleName);

    $scope.room = {};
    $scope.track = {};

    factory.callGetService("GetRoom?itemId=" + roomId + "&codeCampId=" + codeCampId)
        .then(function (response) {
            var fullResult = angular.fromJson(response);
            var serviceResponse = JSON.parse(fullResult.data);

            $scope.room = serviceResponse.Content;

            factory.callGetService("GetTrackByRoomId?roomId=" + room.RoomId + "&codeCampId=" + codeCampId)
                .then(function (response) {
                    var fullResult = angular.fromJson(response);
                    var serviceResponse = JSON.parse(fullResult.data);

                    $scope.track = serviceResponse.Content;

                    LogErrors(serviceResponse.Errors);
                },
                    function (data) {
                        console.log("Unknown error occurred calling GetRooms");
                        console.log(data);
                    });

            LogErrors(serviceResponse.Errors);
        },
        function (data) {
            console.log("Unknown error occurred calling GetRoom");
            console.log(data);
        });

    $scope.ok = function () {
        if ($scope.track != null && $scope.track.TrackId != null) {
            factory.callPostService("UnassignRoomFromTrack?roomId=" + roomId + "&trackId=" + $scope.track.TrackId + "&codeCampId=" + codeCampId)
                .success(function (data) {
                    var fullResult = angular.fromJson(data);
                    var serviceResponse = JSON.parse(fullResult.data);

                    LogErrors(serviceResponse.Errors);
                })
                .error(function (data, status) {
                    $scope.HasErrors = true;
                    console.log("Unknown error occurred calling AssignRoomToTrack");
                    console.log(data);
                });
        }

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