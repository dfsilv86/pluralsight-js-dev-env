(function () {
    'use strict';

    angular
        .module('SGP')
        .filter('upc', theFilter);

    function theFilter() {
        return enumFilter;
    }

    function enumFilter(input) {
        if (null === input || angular.isUndefined(input)) return input;

        // TODO: formatar upc
        return input;
    }
})();
