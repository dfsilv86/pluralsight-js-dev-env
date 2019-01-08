(function () {
    'use strict';

    function sanitize(input) {
        return ('0000' + input.toString()).substr(-4);        
    }

    function formatMilitaryTime(input) {

        if (angular.isUndefined(input) || null == input) {
            return input;
        }

        input = parseInt((input||0).toString().trim(), 10);

        if (!angular.isNumber(input)) {
            return null;
        }

        var result = sanitize(input);
        
        result = result.substr(0, 2) + ':' + result.substr(2, 2);
        return result;
    }

    function theFilter() {
        return formatMilitaryTime;
    }

    window.formatMilitaryTime = formatMilitaryTime;

    angular
        .module('common')
        .filter('militaryTime', theFilter);

})();
