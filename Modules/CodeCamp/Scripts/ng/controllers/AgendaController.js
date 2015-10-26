"use strict";

codeCampControllers.controller("agendaController", ["$scope", "$routeParams", "$http", "$location", "codeCampServiceFactory", function ($scope, $routeParams, $http, $location, codeCampServiceFactory) {
    
    $scope.goToPage = function (pageName) {
        $location.path(pageName);
    }

}]);