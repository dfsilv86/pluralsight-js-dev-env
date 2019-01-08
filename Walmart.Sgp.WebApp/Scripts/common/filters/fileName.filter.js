(function () {
    'use strict';

    angular
        .module('common')
        .filter('fileName', theFilter);

    function theFilter() {
        return FileNameFilter;
    }

    function FileNameFilter(input) {

        // C:\foo\bar.ext
        // ou
        // \\Share\foo\bar.ext


        if (angular.isUndefined(input) || null === input) {
            return input;
        }
        if (typeof (input) !== 'string' || input.length == 0 || input.indexOf('\\') == -1) {
            return input;
        }
        if (input.startsWith('\\\\') || (input[0].toUpperCase() >= 65 && input[0].toUpperCase() <= 90 && input[1] == ':' && input[2] == '\\')) {
            var components = input.split('\\');

            return components[components.length - 1];
        } else {
            return input;
        }
    }
})();
