"use strict";

userGroupApp.factory("userGroupServiceFactory", ["$http", function ($http) {

        var $self = this;

        return {
            init: function (moduleId, moduleName) {
                if ($.ServicesFramework) {
                    var _sf = $.ServicesFramework(moduleId);
                    $self.ServiceRoot = _sf.getServiceRoot(moduleName);
                    $self.ServicePath = $self.ServiceRoot + "GroupManagement/";
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
                    url: $self.ServicePath + method,
                    headers: $self.Headers
                });
            },
            callPostService: function (method, data) {
                return $http({
                    method: "POST",
                    url: $self.ServicePath + method,
                    headers: $self.Headers,
                    data: data
                });
            },
            callDeleteService: function (method) {
                return $http({
                    method: "DELETE",
                    url: $self.ServicePath + method,
                    headers: $self.Headers
                });
            }
        }
    
}]);