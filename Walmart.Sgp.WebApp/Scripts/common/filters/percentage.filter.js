(function () {
    'use strict';

    angular
        .module('common')
        .filter('percentage', theFilter);

    theFilter.$inject = ['numberFilter'];
    function theFilter(numberFilter) {

        return PercentageFilter;

        function PercentageFilter(input, currency, decimalPlaces) {
            if (null === input || angular.isUndefined(input)) return input;

            return numberFilter(Math.abs(input), decimalPlaces) + ' %';
        }
    }
})();
