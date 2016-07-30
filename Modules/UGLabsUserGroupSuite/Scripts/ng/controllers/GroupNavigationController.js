"use strict";

userGroupControllers.controller("groupNavigationController", ["$scope", "$location", function ($scope, $location) {
    
    $scope.isActive = function (viewLocation) {
        return viewLocation === $location.path();
    };

}]);