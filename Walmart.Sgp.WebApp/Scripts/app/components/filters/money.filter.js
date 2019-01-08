(function () {
    'use strict';

    angular
        .module('SGP')
        .filter('money', theFilter);

    theFilter.$inject = ['numberFilter'];
    function theFilter(numberFilter) {

        return MoneyFilter;

        function MoneyFilter(input, currency, decimalPlaces) {
            if (null === input || angular.isUndefined(input)) return input;

            // TODO: formatar money
            return (Math.sign(input) === -1 ? '-' : '') + 'R$ ' + numberFilter(Math.abs(input), decimalPlaces);
        }
    }
})();
