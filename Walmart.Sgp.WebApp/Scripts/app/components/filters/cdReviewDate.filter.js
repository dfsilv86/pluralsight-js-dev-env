(function () {
    'use strict';

    angular
        .module('SGP')
        .filter('cdReviewDate', theFilter);

    function theFilter() {
        return cdReviewDateFilter;
    }

    function cdReviewDateFilter(input, hasDay) {
        var str = (input || '').toString();

        // Retorna o formato Seg;Ter;Qua
        if (angular.isUndefined(hasDay)) {

            var result = [];
            for (var i = 0, total = str.length; i < total; i++) {
                result[i] = globalization.getText("cdReviewDate" + str[i]);
            }
            return result.join(';');
        }
        // Retorna um booleano se o cdReviewDate possui o dia informado.
        else {
            return str.indexOf(hasDay.toString()) != -1;
        }
    }
})();
