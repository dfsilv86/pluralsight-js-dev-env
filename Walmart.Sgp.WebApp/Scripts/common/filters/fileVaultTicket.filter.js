(function () {
    'use strict';

    angular
        .module('common')
        .filter('fileVaultTicket', theFilter);

    function theFilter() {
        return FileVaultTicketFilter;
    }

    function FileVaultTicketFilter(input) {

        // spi28.246845.efbff573-e7b0-4230-ac99-d22892d874df|2016-07-29 12:35:37.000


        if (angular.isUndefined(input) || null === input) {
            return input;
        }
        if (typeof (input) !== 'string') {
            return input;
        }
        var components = input.split('|');

        if (components.length != 2) return input;
        if (components[0].substr(-13, 1) !== '-') return input;
        if (components[0].substr(-18, 1) !== '-') return input;
        if (components[0].substr(-23, 1) !== '-') return input;
        if (components[0].substr(-28, 1) !== '-') return input;
        if (components[0].substr(-37, 1) !== '.') return input;
        if (components[1].substr(4, 1) !== '-') return input;
        if (components[1].substr(7, 1) !== '-') return input;
        if (components[1].substr(10, 1) !== ' ') return input;
        if (components[1].substr(13, 1) !== ':') return input;
        if (components[1].substr(16, 1) !== ':') return input;
        if (components[1].substr(19, 1) !== '.') return input;

        return components[0].substr(0, components[0].length - 37);
    }
})();
