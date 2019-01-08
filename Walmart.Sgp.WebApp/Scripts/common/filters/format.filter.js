(function () {
    'use strict';

    angular
        .module('common')
        .filter('formatter', theFilter);

    function theFilter() {
        return FormatFilter;
    }

    function FormatFilter(input, arg0, arg1, arg2) {
        if (!angular.isDefined(input) || input === null) return input;
        var txt = (input || '').toString();
        if (txt === '') return input;
        txt = txt.replace(/\$0/gi, arg0 || '');
        txt = txt.replace(/\$1/gi, arg1 || '');
        txt = txt.replace(/\$2/gi, arg2 || '');
        return txt;
    }
})();
