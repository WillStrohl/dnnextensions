(function() {
    "use strict";

    codeCampApp.directive("confirmDelete", confirmDelete);

    confirmDelete.$inject = [];
    
    function confirmDelete() {
        // Usage:
        //     <element confirm-delete></element>
        // Creates:
        var directive = {
            link: link,
            restrict: "A"
        };

        return directive;

        function link(scope, element, attrs) {
            scope.$watch(attrs.ngModel, function (v) {
                if (v.length > 0) {
                    addConfirmationToElements();
                }
            });
        }
    }
})();