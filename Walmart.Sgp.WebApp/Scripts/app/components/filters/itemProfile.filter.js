(function () {
    'use strict';

    angular
        .module('SGP')
        .filter('itemProfile', theFilter);

    function theFilter() {
        return itemProfileFilter;
    }

    function itemProfileFilter(input) {

        if (null === input || angular.isUndefined(input)) return input;

        return '{0} - {1}'.format(input.cdItem, input.dsItem);
    }
})();
