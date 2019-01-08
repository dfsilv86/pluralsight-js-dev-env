(function () {
    'use strict';

    angular
        .module('common')
        .filter('number', theFilter);

    function theFilter() {
        return NumberFilter;
    }

    function FormatNumber(number, n, x, s, c) {
        if (number == undefined) {
            return '';
        }

        if (typeof (number) !== 'number') {
            /*number = replaceAll(number, '.', '#');
            number = replaceAll(number, ',', '.');
            number = replaceAll(number, '#', '');*/
            number = number.replace(/\./gi, '#');
            number = number.replace(/\,/gi, '.');
            number = number.replace(/\#/gi, '');
            number = parseFloat(number);
        }

        // sobre o operador ~:
        // https://www.joezimjs.com/javascript/great-mystery-of-the-tilde/
        var re = '\\d(?=(\\d{' + (x || 3) + '})+' + (n > 0 ? '\\D' : '$') + ')',
            num = number.toFixed(Math.max(0, ~~n));

        return (c ? num.replace('.', c) : num).replace(new RegExp(re, 'g'), '$&' + (s || ','));
    }

    function NumberFilter(input2, decimals2) {
        var input = input2 || 0;
        var decimals = angular.isDefined(decimals2) ? decimals2 : 2;

        if (typeof (input) === 'string') {
            return input;
        }
        
        return FormatNumber(input, decimals, 3, '.', ',');
    }

    window.formatNumber = FormatNumber;
})();
