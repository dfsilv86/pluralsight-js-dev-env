(function () {
    'use strict';

    angular
        .module('SGP')
        .filter('chainProfile', theFilter);

    function theFilter() {
        return chainProfileFilter;
    }

    function chainProfileFilter(input) {

        if (null === input || angular.isUndefined(input)) return input;

        return '{0} - {1}'.format(input.idBandeira, input.dsBandeira);
    }
})();
