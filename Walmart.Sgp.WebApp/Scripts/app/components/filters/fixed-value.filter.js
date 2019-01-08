(function () {
    'use strict';

    angular
        .module('SGP')
        .filter('fixedValue', theFilter);

    function theFilter() {
        return fixedValueFilter;
    }

    function fixedValueFilter(input, fixedValueName) {        
        var fv = sgpFixedValues.getByValue(fixedValueName, input);

        var description = fv === null || fv === undefined ? '' : fv.description;

        return description === globalization.texts.select ? '' : description;
    }
})();
