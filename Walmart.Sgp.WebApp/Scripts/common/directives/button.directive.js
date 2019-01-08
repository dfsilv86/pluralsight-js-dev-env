(function () {
    'use strict';

    angular
        .module('common')
        .directive('button', ButtonSubmitPreventionDirective);

    function ButtonSubmitPreventionDirective() {
        return {
            restrict: 'E',
            scope: false,
            link: ButtonLink
        };

        function ButtonLink($scope, elem, attr) {
            if (!elem.attr('type')) {
                elem.attr('type', 'button');
            }
        }
    }
})();