(function () {
    'use strict';

    angular
        .module('SGP')
        .filter('supplierProfile', theFilter);

    function theFilter() {
        return supplierProfileFilter;
    }

    function supplierProfileFilter(input) {
        if (null === input || angular.isUndefined(input)) return input;

        return '{0} - {1}'.format(input.cdFornecedor, input.nmFornecedor);
    }
})();
