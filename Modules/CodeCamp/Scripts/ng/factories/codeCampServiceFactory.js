"use strict";

codeCampApp.factory("codeCampServiceFactory", ["$http", function ($http) {

        var $self = this;

        return {
            init: function (moduleId, moduleName) {
                //console.log("codeCampServiceFactory: INIT " + moduleId + " | " + moduleName);
                if ($.ServicesFramework) {
                    var _sf = $.ServicesFramework(moduleId);
                    $self.ServiceRoot = _sf.getServiceRoot(moduleName);
                    $self.ServicePath = $self.ServiceRoot + "Event/";
                    $self.Headers = {
                        "ModuleId": moduleId,
                        "TabId": _sf.getTabId(),
                        "RequestVerificationToken": _sf.getAntiForgeryValue()
                    };
                }
                //console.log("codeCampServiceFactory - $self.ServiceRoot: " + $self.ServiceRoot);
                //console.log("codeCampServiceFactory - $self.ServicePath: " + $self.ServicePath);
            },
            callGetService: function (method) {
                //console.log("codeCampServiceFactory: Calling GET " + method);
                return $http({
                    method: "GET",
                    url: $self.ServicePath + method,
                    headers: $self.Headers
                });
            },
            callPostService: function (method, data) {
                //console.log("codeCampServiceFactory: Calling POST " + method);
                return $http({
                    method: "POST",
                    url: $self.ServicePath + method,
                    headers: $self.Headers,
                    data: data
                });
            },
            callDeleteService: function (method) {
                //console.log("codeCampServiceFactory: Calling DELETE " + method);
                return $http({
                    method: "DELETE",
                    url: $self.ServicePath + method,
                    headers: $self.Headers
                });
            }
        }
    
}]);