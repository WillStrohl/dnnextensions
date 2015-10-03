"use strict";

codeCampControllers.controller("testController", ["$scope", "$routeParams", "$http", "codeCampServiceFactory", function ($scope, $routeParams, $http, codeCampServiceFactory) {

    var factory = codeCampServiceFactory;
    factory.init(moduleId, moduleName);

    factory.callGetService("Ping")
        .then(function (response) {
            var fullResult = angular.fromJson(response);

            var serviceResponse = JSON.parse(fullResult.data);

            $scope.Ping = serviceResponse.Content;

            LogErrors(serviceResponse.Errors);
        },
        function (data) {
            console.log("Unknown error occurred calling Ping");
            console.log(data);
        });

    factory.callGetService("PingError")
        .then(function (response) {
            var fullResult = angular.fromJson(response);

            var serviceResponse = JSON.parse(fullResult.data);

            $scope.PingError = serviceResponse.Content;

            LogErrors(serviceResponse.Errors);
        },
        function (data) {
            console.log("Unknown error occurred calling PingError");
            console.log(data);
        });

    factory.callGetService("PingException")
        .then(function (response) {
            var fullResult = angular.fromJson(response);

            var serviceResponse = JSON.parse(fullResult.data);

            $scope.PingException = serviceResponse.Content;

            LogErrors(serviceResponse.Errors);
        },
        function (data) {
            console.log("Unknown error occurred calling PingException");
            console.log(data);
        });

    factory.callGetService("PingNotFound")
        .then(function (response) {
            var fullResult = angular.fromJson(response);

            var serviceResponse = JSON.parse(fullResult.data);

            $scope.PingNotFound = serviceResponse.Content;

            LogErrors(serviceResponse.Errors);
        },
        function (data) {
            console.log("Unknown error occurred calling PingNotFound");
            console.log(data);
        });

    factory.callGetService("PingSecurityCheck")
        .then(function (response) {
            var fullResult = angular.fromJson(response);

            var serviceResponse = JSON.parse(fullResult.data);

            $scope.PingSecurityCheck = serviceResponse.Content;

            LogErrors(serviceResponse.Errors);
        },
        function (data) {
            console.log("Unknown error occurred calling PingSecurityCheck");
            console.log(data);
        });

}]);
/*
 EXAMPLE RESPONSE
  
  {
   "data":"{\"Errors\":[],\"Content\":\"Success\"}",
   "status":200,
   "config":{
      "method":"GET",
      "transformRequest":[
         null
      ],
      "transformResponse":[
         null
      ],
      "url":"/DesktopModules/CodeCamp/API/Event/Ping",
      "headers":{
         "ModuleId":483,
         "TabId":"100",
         "RequestVerificationToken":"ykgTsg5GLAkqe1vxPSkOFaFhXVKxjstTEStOYulP3heS5vEaNez0l1oAXwypV2dByR-N0VkcWftRVajQdbqMh5tSIwdA7xeBFkq1rsA1KKlNa1io1iD0OyGCb2NdfhmW6vI1Uw2",
         "Accept":"application/json, text/plain, STAR/STAR"
        }
    },
        "statusText":"OK"
    }

 */