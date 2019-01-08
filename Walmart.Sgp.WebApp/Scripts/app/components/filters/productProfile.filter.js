(function () {
    'use strict';

    angular
        .module('SGP')
        .filter('productProfile', theFilter);

    function theFilter() {
        return productProfileFilter;
    }

    function productProfileFilter(input) {
        if (null === input || angular.isUndefined(input)) return input;

        return '{0} - {1}'.format(input.cdItem, input.dsItem);
    }
})();
