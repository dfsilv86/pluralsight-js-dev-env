(function () {
    'use strict';

    angular
        .module('SGP')
        .filter('fineLineProfile', theFilter);

    function theFilter() {
        return fineLineProfileFilter;
    }

    function fineLineProfileFilter(input) {
        if (null === input || angular.isUndefined(input)) return input;

        return '{0} - {1}'.format(input.cdFineLine, input.dsFineLine);
    }
})();
