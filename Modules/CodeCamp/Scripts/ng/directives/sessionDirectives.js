"use strict";

codeCampApp.directive("count", function(codeCampServiceFactory) {
    return {
        restrict: "E",
        replace: true,
        scope: {
            session: "=",
            sessionRegistration: "="
        },
        link: function(scope, elem, attrs) {
            scope.$watch("sessionRegistration", function() {
                var factory = codeCampServiceFactory;
                factory.init(moduleId, moduleName);

                scope.registrantCount = 0;

                factory.callGetService("GetSessionRegistrations?sessionId=" + scope.session.SessionId)
                    .then(function(response) {
                            var fullResult = angular.fromJson(response);
                            var serviceResponse = JSON.parse(fullResult.data);

                            if (serviceResponse.Content != null) {
                                scope.registrantCount = serviceResponse.Content.length;
                            }

                            LogErrors(serviceResponse.Errors);
                        },
                        function(data) {
                            console.log("Unknown error occurred calling GetSessionRegistrationByRegistrantId");
                            console.log(data);
                        }, true);
            });
        },
        template: "<span>{{registrantCount}}</span>"
    }
});