(function () {
    'use strict';

    angular
        .module('SGP')
        .filter('categoryProfile', theFilter);

    function theFilter() {
        return categoryProfileFilter;
    }

    function categoryProfileFilter(input) {

        if (null === input || angular.isUndefined(input)) return input;

        return '{0} - {1}'.format(input.cdCategoria, input.dsCategoria);
    }
})();
