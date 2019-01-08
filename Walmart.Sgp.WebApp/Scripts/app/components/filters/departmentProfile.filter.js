(function () {
    'use strict';

    angular
        .module('SGP')
        .filter('departmentProfile', theFilter);

    function theFilter() {
        return departmentProfileFilter;
    }

    function departmentProfileFilter(input) {

        if (null === input || angular.isUndefined(input)) return input;

        return '{0} - {1}'.format(input.cdDepartamento, input.dsDepartamento);
    }
})();
