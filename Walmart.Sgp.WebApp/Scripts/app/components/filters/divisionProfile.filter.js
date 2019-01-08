(function () {
    'use strict';

    angular
        .module('SGP')
        .filter('divisionProfile', theFilter);

    function theFilter() {
        return divisionProfileFilter;
    }

    function divisionProfileFilter(input) {
        if (null === input || angular.isUndefined(input)) return input;

        return '{0} - {1}'.format(input.cdDivisao, input.dsDivisao);
    }
})();
