(function () {
    'use strict';

    angular
        .module('common')
        .filter('emptyDefault', theFilter);

    function theFilter() {
        return EmptyDefaultFilter;
    }

    function EmptyDefaultFilter(input) {        
        if (input === null
         || input === globalization.texts.notDefined
         || input === '31/12/1899'
         || input === '31/12/0000') {
            return  '';
        }

        return input;
    }
})();
