(function () {
    'use strict';

    angular
        .module('common')
        .directive('radioCditemPlu', BooleanRadioDirective);

    function BooleanRadioDirective() {
        return {
            restrict: 'E',
            replace: true,
            templateUrl: 'Scripts/app/components/directives/radio-cditem-plu.template.html',
            scope: {
                ngModel: '=',
                ngChange: '&',
                ngDisabled: '=?',
                name: '@'
            }
        };
    }
})();