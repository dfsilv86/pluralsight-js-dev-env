(function () {
    'use strict';

    angular
        .module('SGP')
        .filter('plu', theFilter);

    function theFilter() {
        return enumFilter;
    }

    function enumFilter(input) {
        if (null === input || angular.isUndefined(input)) return input;

        // TODO: formatar plu
        return input;
    }
})();
