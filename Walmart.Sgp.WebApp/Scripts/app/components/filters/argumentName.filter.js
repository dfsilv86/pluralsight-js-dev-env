(function () {
    'use strict';

    angular
        .module('SGP')
        .filter('argumentName', theFilter);

    function theFilter() {
        return argumentName;
    }

    function argumentName(input) {
        if (null === input || angular.isUndefined(input)) return input;

        // TODO: formatar upc
        var argParts = input.split('.');
        var argPart = argParts[argParts.length - 1];
        var indexerPosition = argPart.indexOf('[');
        if (indexerPosition === -1) indexerPosition = indexerPosition.length;
        var result = argPart.substr(0, indexerPosition);
        return result;
    }
})();
