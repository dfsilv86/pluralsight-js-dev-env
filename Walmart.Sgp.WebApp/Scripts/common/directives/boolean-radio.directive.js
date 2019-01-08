(function () {
    'use strict';

    angular
        .module('common')
        .directive('booleanRadio', BooleanRadioDirective);

    function BooleanRadioDirective() {
        return {
            restrict: 'E',
            replace: true,
            templateUrl: 'Scripts/common/directives/boolean-radio.directive.html',
            scope: {
                ngModel: '=',
                ngChange: '&',
                ngDisabled: '=?',
                name: '@'            
            }            
        };
    }
})();