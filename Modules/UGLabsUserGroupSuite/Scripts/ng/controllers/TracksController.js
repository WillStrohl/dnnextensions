"use strict";

codeCampControllers.controller("tracksController", ["$scope", "$routeParams", "$http", "$location", "$uibModal", "codeCampServiceFactory", function ($scope, $routeParams, $http, $location, $uibModal, codeCampServiceFactory) {

    var factory = codeCampServiceFactory;
    factory.init(moduleId, moduleName);

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

                $scope.LoadTracks();

                LogErrors(serviceResponse.Errors);
            },
            function (data) {
                console.log("Unknown error occurred calling UserCanEditEvent");
                console.log(data);
            });
    }

    $scope.LoadTracks = function () {
        factory.callGetService("GetTracks?codeCampId=" + $scope.codeCamp.CodeCampId)
            .then(function (response) {
                var fullResult = angular.fromJson(response);
                var serviceResponse = JSON.parse(fullResult.data);

                $scope.tracks = serviceResponse.Content;

                $.each($scope.tracks, function (i, track) {
                    track.TrackSlug = GetSlugFromValue(track.Title);
                });

                LogErrors(serviceResponse.Errors);
            },
            function (data) {
                console.log("Unknown error occurred calling GetTracks");
                console.log(data);
            });
    }

    $scope.goToPage = function (pageName) {
        $location.path(pageName);
    }

    $scope.openModal = function (size, trackId) {
        var modalInstance = $uibModal.open({
            templateUrl: "AddTrackModal.html",
            controller: "AddTrackModalController",
            size: size,
            backdrop: "static",
            scope: $scope,
            resolve: {
                userId: function () {
                    return $scope.currentUserId;
                },
                codeCamp: function () {
                    return $scope.codeCamp;
                },
                trackId: function() {
                    return trackId;
                }
            }
        });

        modalInstance.result.then(function (savedTrack) {
            $scope.savedTrack = savedTrack;
            $scope.LoadTracks();
        }, function () {
            console.log("Modal dismissed at: " + new Date());
        });
    };

    $scope.LoadData();
}]);

/*
 * Speakers Modal Controller
 */
codeCampApp.controller("AddTrackModalController", ["$scope", "$rootScope", "$uibModalInstance", "userId", "codeCamp", "trackId", "codeCampServiceFactory", function ($scope, $rootScope, $uibModalInstance, userId, codeCamp, trackId, codeCampServiceFactory) {

    var factory = codeCampServiceFactory;
    factory.init(moduleId, moduleName);

    $scope.CodeCampId = codeCamp.CodeCampId;

    $scope.track = {};

    $scope.UpdateMode = (trackId != undefined && trackId > -1);

    if ($scope.UpdateMode) {
        factory.callGetService("GetTrack?itemId=" + trackId + "&codeCampId=" + $scope.CodeCampId)
            .then(function (response) {
                var fullResult = angular.fromJson(response);
                var serviceResponse = JSON.parse(fullResult.data);

                $scope.track = serviceResponse.Content;

                LogErrors(serviceResponse.Errors);
            },
            function (data) {
                console.log("Unknown error occurred calling GetTrack");
                console.log(data);
            });
    }

    $scope.ok = function () {
        $scope.track.CodeCampId = $scope.CodeCampId;

        var trackAction = ($scope.UpdateMode) ? "UpdateTrack" : "CreateTrack";

        // save the track
        factory.callPostService(trackAction, $scope.track)
            .success(function (data) {
                var savedTrack = angular.fromJson(data);
                $scope.savedTrack = savedTrack.Content;

                $scope.LoadTracks();

                LogErrors(savedTrack.Errors);
            })
            .error(function (data, status) {
                $scope.HasErrors = true;
                console.log("Unknown error occurred calling " + trackAction);
                console.log(data);
            });

        $uibModalInstance.close($scope.savedTrack);
    };

    $scope.cancel = function () {
        $uibModalInstance.dismiss("cancel");
    };
}]);