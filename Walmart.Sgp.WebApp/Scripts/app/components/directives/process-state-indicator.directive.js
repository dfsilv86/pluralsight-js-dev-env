(function () {
    'use strict';

    angular
        .module('SGP')
        .directive('processStateIndicator', ProcessStateIndicatorDirective);

    function ProcessStateIndicatorDirective() {

        return {
            restrict: 'E',
            scope: {
                order: '=ngModel'
            },
            templateUrl: 'Scripts/app/components/directives/process-state-indicator.view.html',
        }
    }
})();