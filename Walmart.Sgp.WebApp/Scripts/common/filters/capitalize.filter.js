(function () {
    'use strict';

    angular
        .module('common')
        .filter('capitalize', theFilter);

    function theFilter() {
        return CapitalizeFilter;
    }

    var cache = { true: { true: {}, false: {} }, false: { true: {}, false: {} } };

    function CapitalizeFilter(input, capitalizeExceptions, upperCase) {
        if (angular.isUndefined(input) || null === input) {
            return input;
        }
        if (upperCase === 'false') {
            upperCase = false;
        }
        if (angular.isUndefined(upperCase) || upperCase !== false) {
            upperCase = true;
        }
        if (typeof (input) !== 'string') {
            return input;
        }
        if (!!cache[!!capitalizeExceptions][!!upperCase][input]) {
            return cache[!!capitalizeExceptions][!!upperCase][input];
        }

        var components = input.split(' ');
        var isException = [];
        for (var i = 0; i < components.length; i++) {
            var component = components[i];
            if (components.length === 0) continue;
            if (component.substr(0, 1) === '(' && !capitalizeExceptions) {
                isException.push(true);
            }
            if (isException.length > 0) {
                if (component.substr(-1) === ')') {
                    isException.pop();
                }
                continue;
            }
            if (component.length < 3) continue;
            var n = component.toLowerCase();
            if (n == 'por') continue;
            if (n == 'mtr') {
                components[i] = components[i].toUpperCase();
            } else {
                if (components[i].substr(0, 1) === '(') {
                    components[i] = '(' + component[1].toUpperCase() + component.substr(2);
                } else {
                    components[i] = component[0].toUpperCase() + component.substr(1);
                }
            }
        }
        if (upperCase === false) {
            components[0] = components[0][0].toLowerCase() + components[0].substr(1);
        }
        return cache[!!capitalizeExceptions][!!upperCase][input] = components.join(' ');
    }
    CapitalizeFilter.getCachedValues = function () { return cache; };

})();