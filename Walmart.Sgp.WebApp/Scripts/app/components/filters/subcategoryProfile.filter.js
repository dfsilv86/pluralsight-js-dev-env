(function () {
    'use strict';

    angular
        .module('SGP')
        .filter('subcategoryProfile', theFilter);

    function theFilter() {
        return subcategoryProfileFilter;
    }

    function subcategoryProfileFilter(input) {
        if (null === input || angular.isUndefined(input)) return input;

        return '{0} - {1}'.format(input.cdSubcategoria, input.dsSubcategoria);
    }
})();
