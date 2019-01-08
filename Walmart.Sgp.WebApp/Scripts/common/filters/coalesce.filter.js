(function () {
    'use strict';

    angular
        .module('common')
        .filter('coalesce', theFilter);

    function theFilter() {
        return CoalesceFilter;
    }

    function CoalesceFilter(input) {

        if (arguments.length == 1) return input;

        for (var i = 0; i < arguments.length; i++) {
            if (angular.isDefined(arguments[i]) && null !== arguments[i]) {
                return arguments[i];
            }
        }
        return null;
    }
})();
