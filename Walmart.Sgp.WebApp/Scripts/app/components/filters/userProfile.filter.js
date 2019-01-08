(function () {
    'use strict';

    angular
        .module('SGP')
        .filter('userProfile', theFilter);

    function theFilter() {
        return userProfileFilter;
    }

    function userProfileFilter(input) {
        if (null === input || angular.isUndefined(input)) return input;

        return '{0} ({1})'.format(input.fullName, input.userName.trim());
    }
})();
