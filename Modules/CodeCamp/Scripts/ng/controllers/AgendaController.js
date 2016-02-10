"use strict";

codeCampControllers.controller("agendaController", ["$scope", "$routeParams", "$http", "$location", "codeCampServiceFactory", function ($scope, $routeParams, $http, $location, codeCampServiceFactory) {
    
    var factory = codeCampServiceFactory;
    factory.init(moduleId, moduleName);

    $scope.eventDays = [];
    $scope.sessions = [];

    $scope.LoadData = function () {
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

                    $scope.LoadEditPermissions();
                }

                var beginMoment = moment($scope.codeCamp.BeginDate);
                var endMoment = moment($scope.codeCamp.EndDate);
                $scope.eventLength = endMoment.diff(beginMoment, "days") + 1;

                for (var i = 0; i < $scope.eventLength; i++) {
                    var newDay = {};
                    var newMoment = moment(beginMoment.format($momentDateFormat));

                    if (i > 0) {
                        newMoment = newMoment.add(i, "days");
                    }

                    newDay.Index = i;
                    newDay.Month = newMoment.format($momentMonthFormat);
                    newDay.DayName = newMoment.format($momentDayNameFormat);
                    newDay.DayNumber = newMoment.format($momentDayNumberFormat);
                    newDay.Date = newMoment.format($momentDateFormat);
                    newDay.Year = newMoment.format($momentYearFormat);

                    $scope.eventDays.push(newDay);
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

                $scope.LoadAgenda();

                LogErrors(serviceResponse.Errors);
            },
            function (data) {
                console.log("Unknown error occurred calling GetTracks");
                console.log(data);
            });
    };

    $scope.LoadAgenda = function () {
        factory.callGetService("GetAgenda?codeCampId=" + $scope.codeCamp.CodeCampId)
            .then(function (response) {
                var fullResult = angular.fromJson(response);
                var serviceResponse = JSON.parse(fullResult.data);

                $scope.agenda = serviceResponse.Content;

                $scope.agenda.CodeCamp.BeginDate = moment($scope.agenda.CodeCamp.BeginDate).format($momentDateFormat);
                $scope.agenda.CodeCamp.EndDate = moment($scope.agenda.CodeCamp.EndDate).format($momentDateFormat);

                angular.forEach($scope.agenda.EventDays, function(eventDay, index) {
                    eventDay.TimeStamp = moment(eventDay.TimeStamp).format($momentDateFormat);
                    eventDay.DayName = moment(eventDay.TimeStamp).format($momentDayNameFormat);

                    angular.forEach(eventDay.TimeSlots, function(timeSlot, index) {
                        timeSlot.BeginTime = moment(timeSlot.BeginTime).format($momentTimeFormat);
                        timeSlot.EndTime = moment(timeSlot.EndTime).format($momentTimeFormat);
                    });
                });

                $scope.hasAgenda = ($scope.agenda != null) && ($scope.agenda.EventDays != null) && ($scope.agenda.EventDays[0].TimeSlots.length > 0);
                
                LogErrors(serviceResponse.Errors);
            },
                function (data) {
                    console.log("Unknown error occurred calling GetTimeSlots");
                    console.log(data);
                });
    }

    $scope.goToPage = function (pageName) {
        $location.path(pageName);
    }

    $scope.LoadData();

}]);