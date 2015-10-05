"user strict";

codeCampApp.directive('dateFormatter', function (dateFilter, $parse) {
    return {
        restrict: 'EAC',
        require: '?ngModel',
        link: function (scope, element, attrs, ngModel, ctrl) {
            ngModel.$parsers.push(function (viewValue) {
                return dateFilter(viewValue, 'yyyy-MM-dd');
            });
        }
    }
});

codeCampApp.directive("dateLowerThan", ["$filter", function ($filter) {

    return {
        require: "ngModel",
        link: function (scope, elm, attrs, ctrl) {

            var validateDateRange = function (inputValue) {

                var fromDate = $filter("date")(inputValue, "short");
                var toDate = $filter("date")(attrs.dateLowerThan, "short");
                var isValid = isValidDateRange(fromDate, toDate);

                ctrl.$setValidity("dateLowerThan", isValid);

                return inputValue;
            };

            ctrl.$parsers.unshift(validateDateRange);
            ctrl.$formatters.push(validateDateRange);

            attrs.$observe("dateLowerThan", function () {
                validateDateRange(ctrl.$viewValue);
            });
        }
    };

}]);

codeCampApp.directive("dateGreaterThan", ["$filter", function ($filter) {
    return {
        require: "ngModel",
        link: function (scope, elm, attrs, ctrl) {

            var validateDateRange = function (inputValue) {

                var fromDate = $filter("date")(attrs.dateGreaterThan, "short");
                var toDate = $filter("date")(inputValue, "short");
                var isValid = isValidDateRange(fromDate, toDate);

                ctrl.$setValidity("dateGreaterThan", isValid);

                return inputValue;
            };

            ctrl.$parsers.unshift(validateDateRange);
            ctrl.$formatters.push(validateDateRange);

            attrs.$observe("dateGreaterThan", function () {
                validateDateRange(ctrl.$viewValue);
            });
        }
    };
}]);

var isValidDate = function (dateStr) {
    if (dateStr == undefined)
        return false;

    var dateTime = Date.parse(dateStr);

    if (isNaN(dateTime)) {
        return false;
    }

    return true;
};

var getDateDifference = function (fromDate, toDate) {
    return Date.parse(toDate) - Date.parse(fromDate);
};

var isValidDateRange = function (fromDate, toDate) {
    if (fromDate == "" || toDate == "")
        return true;

    if (isValidDate(fromDate) == false) {
        return false;
    }

    if (isValidDate(toDate) == true) {
        var days = getDateDifference(fromDate, toDate);
        if (days < 0) {
            return false;
        }
    }

    return true;
};