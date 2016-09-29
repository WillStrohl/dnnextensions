"use strict";

userGroupControllers.controller("editGroupController", ["$scope", "$routeParams", "$location", "$http", "userGroupServiceFactory", function ($scope, $routeParams, $location, $http, userGroupServiceFactory) {
    
    var factory = userGroupServiceFactory;
    factory.init(moduleId, moduleName);
    
    $scope.groupID = $routeParams.groupID;
    $scope.group = {};

    $scope.userCanEdit = false;
    $scope.userIsLoggedIn = false;

    $scope.userGeoData = {};

    $scope.countries = {};
    $scope.selectedCountry = {};
    $scope.regions = {};
    $scope.selectedRegion = {};
    $scope.hasRegions = false;

    $scope.languages = {};
    $scope.hasLanguages = false;
    $scope.selectedLanguage = {};

    $scope.keywords = [];

    $scope.twitterValidationPattern = "^@[a-zA-Z0-9_]{1,15}$";

    $scope.countriesLoaded = false;
    $scope.regionsLoaded = false;
    $scope.languagesLoaded = false;
    $scope.defaultsLoaded = false;

    $scope.LoadData = function() {
        factory.callGetService("GetCurrentUserId")
            .then(function (response) {
                var fullResult = angular.fromJson(response);
                var serviceResponse = JSON.parse(fullResult.data);

                $scope.currentUserId = serviceResponse.Content;

                $scope.userIsLoggedIn = ($scope.currentUserId > -1);

                $scope.PermissionCheck();

                LogErrors(serviceResponse.Errors);
            },
            function (data) {
                console.log("Unknown error occurred calling GetCurrentUserId");
                console.log(data);
            });
    }

    $scope.PermissionCheck = function () {
        if ($scope.groupID == null || $scope.groupID == undefined) {
            $scope.groupID = -1;
        }

        factory.callGetService("UserCanEditGroup?itemID=" + $scope.groupID)
            .then(function (response) {
                var fullResult = angular.fromJson(response);
                var serviceResponse = JSON.parse(fullResult.data);

                $scope.userCanEdit = (serviceResponse.Content == "Success");

                if ($scope.userCanEdit) {
                    $scope.LoadGroup();
                }

                LogErrors(serviceResponse.Errors);
            },
            function (data) {
                console.log("Unknown error occurred calling UserCanEditGroup");
                console.log(data);
            });
    }

    $scope.LoadGroup = function () {
        if ($scope.groupID > 0) {
            factory.callGetService("GetGroup?itemID=" + $scope.groupID)
                .then(function(response) {
                        var fullResult = angular.fromJson(response);
                        var serviceResponse = JSON.parse(fullResult.data);

                        $scope.group = serviceResponse.Content;

                        $scope.LoadGeoData();

                        LogErrors(serviceResponse.Errors);
                    },
                    function(data) {
                        console.log("Unknown error occurred calling GetCurrentUserGeoData");
                        console.log(data);
                    });
        } else {
            $scope.LoadGeoData();
        }
    }

    $scope.LoadGeoData = function () {
        factory.callGetService("GetCurrentUserGeoData")
        .then(function (response) {
            var fullResult = angular.fromJson(response);
            var serviceResponse = JSON.parse(fullResult.data);

            $scope.userGeoData = serviceResponse.Content;

            $scope.LoadCountries();

            LogErrors(serviceResponse.Errors);
        },
        function (data) {
            console.log("Unknown error occurred calling GetCurrentUserGeoData");
            console.log(data);
        });
    }

    $scope.LoadCountries = function () {
        factory.callGetService("GetCountries")
            .then(function (response) {
                var fullResult = angular.fromJson(response);
                var serviceResponse = JSON.parse(fullResult.data);

                $scope.countries = serviceResponse.Content;

                $scope.countriesLoaded = true;

                $scope.LoadRegions();

                $scope.LoadLanguages();

                $scope.SetDefaults();

                LogErrors(serviceResponse.Errors);
            },
            function (data) {
                console.log("Unknown error occurred calling GetCountries");
                console.log(data);
            });
    }

    $scope.LoadRegions = function () {
        if ($scope.selectedCountry != undefined) {
            $scope.group.Country = $scope.selectedCountry.Value;
            $scope.group.CountryCode = $scope.selectedCountry.Key;
        }

        if ($scope.group != undefined && $scope.selectedCountry != undefined && $scope.group.CountryCode != undefined) {
            factory.callGetService("GetRegions?countryCode=" + $scope.group.CountryCode)
                .then(function(response) {
                        var fullResult = angular.fromJson(response);
                        var serviceResponse = JSON.parse(fullResult.data);

                        $scope.regions = serviceResponse.Content;

                        $scope.hasRegions = ($scope.regions != null && $scope.regions.length > 0);

                        $scope.regionsLoaded = true;

                        $scope.SetDefaults();

                        LogErrors(serviceResponse.Errors);
                    },
                    function(data) {
                        console.log("Unknown error occurred calling GetRegions");
                        console.log(data);
                    });
        }
    }

    $scope.LoadLanguages = function() {
        factory.callGetService("GetLanguages")
            .then(function (response) {
                var fullResult = angular.fromJson(response);
                var serviceResponse = JSON.parse(fullResult.data);

                $scope.languages = serviceResponse.Content;

                $scope.hasLanguages = ($scope.languages != null && $scope.languages.length > 0);

                $scope.languagesLoaded = true;

                $scope.SetDefaults();

                LogErrors(serviceResponse.Errors);
            },
                function (data) {
                    console.log("Unknown error occurred calling GetLanguages");
                    console.log(data);
                });
    }

    $scope.SetDefaults = function() {
        if ($scope.defaultsLoaded) return;
        if (!$scope.countriesLoaded || !$scope.regionsLoaded || !$scope.languagesLoaded) return;

        if ($scope.groupID == -1) {
            $scope.selectedCountry = { Key: $scope.userGeoData.country_code, Value: $scope.userGeoData.country_name };

            $scope.LoadRegions();

            if ($scope.hasRegions) {
                $scope.selectedRegion = { Key: $scope.userGeoData.region_code, Value: $scope.userGeoData.region_name };
            }

            $scope.keywords = ["DNN", "User-Group"];

            $scope.selectedLanguage = { Key: 1, Value: "English" };
        }

        $scope.defaultsLoaded = true;
    }

    $scope.updateRegionSelection = function() {
        if ($scope.selectedRegion != undefined) {
            $scope.group.Region = $scope.selectedRegion.Value;
            $scope.group.RegionCode = $scope.selectedRegion.Key;
        } else {
            $scope.group.Region = "";
            $scope.group.RegionCode = "";
        }
    }

    $scope.goToPage = function(pageName) {
        $location.path(pageName);
    }

    $scope.LoadData();

}]);