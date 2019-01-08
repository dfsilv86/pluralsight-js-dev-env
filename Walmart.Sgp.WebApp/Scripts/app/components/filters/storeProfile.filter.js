(function () {
    'use strict';

    angular
        .module('SGP')
        .filter('storeProfile', theFilter);

    function theFilter() {
        return storeProfileFilter;
    }

    function storeProfileFilter(input) {
        if (null === input || angular.isUndefined(input)) return input;

        return '{0} - {1}'.format(input.cdLoja, input.nmLoja);
    }
})();
