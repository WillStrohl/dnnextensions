"use strict";

angular
    .module("codeCampApp")
    .factory("codeCampFactory", ["$http", function ($http) {

        $self = this;

        return {
            init: function (moduleId, moduleName) {
                if ($.ServicesFramework) {
                    var _sf = $.ServicesFramework(moduleId);
                    $self.ServiceRoot = _sf.getServiceRoot(moduleName);
                    $self.Headers = {
                        "ModuleId": moduleId,
                        "TabId": _sf.getTabId(),
                        "RequestVerificationToken": _sf.getAntiForgeryValue()
                    };
                }
            },
            callGetService: function (method) {
                return $http({
                    method: "GET",
                    url: $self.ServiceRoot + method,
                    headers: $self.Headers
                });
            },
            callPostService: function (method, data) {
                return $http({
                    method: "POST",
                    url: $self.ServiceRoot + method,
                    headers: $self.Headers,
                    data: data
                });
            },
            callDeleteService: function (method) {
                return $http({
                    method: "DELETE",
                    url: $self.ServiceRoot + method,
                    headers: $self.Headers
                });
            }
        }
    
}]);