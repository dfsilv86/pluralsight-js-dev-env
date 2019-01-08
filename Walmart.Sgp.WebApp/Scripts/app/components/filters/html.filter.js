(function () {
    'use strict';

    angular
        .module('SGP')
        .filter('html', theFilter);

    function theFilter() {
        return htmlFilter;
    }

    function htmlFilter(input) {
        if (null === input || angular.isUndefined(input)) return input;

        return input.decode();
    }
})();
