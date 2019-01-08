(function () {
    'use strict';

    angular
        .module('common')
        .directive('processoCargaStatus', Semaphore);

    function Semaphore() {
        return {
            restrict: 'E',
            replace: true,
            template: '<span class="glyphicon processo-carga-status" ng-class="statusClass" title="{{::title}}"></span>',
            scope: {
                ngModel: '=?',
                value: '@?'
            },
            link: function (scope) {
                var status = scope.ngModel || scope.value;
                scope.statusClass = "processo-carga-status-{0}".format(status);
                scope.title = globalization.getText("ProcessoCargaStatusFixedValue{0}".format(status));
            }
        };
    }

})();