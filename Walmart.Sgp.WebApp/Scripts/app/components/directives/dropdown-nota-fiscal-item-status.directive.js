(function () {
    'use strict';

    angular
        .module('SGP')
        .directive('dropdownNotaFiscalItemStatus', DropdownNotaFiscalItemStatus);

    DropdownNotaFiscalItemStatus.$inject = ['$q', 'NotaFiscalItemStatusService'];

    function DropdownNotaFiscalItemStatus($q, notaFiscalItemStatusService) {
        return {
            restrict: 'E',
            replace: true,
            templateUrl: 'Scripts/app/components/directives/dropdown-nota-fiscal-item-status.template.html',
            scope: {
                ngModel: '='
            },
            link: function ($scope, elem, attr) {
                $scope.dropdownName = attr.name || 'dropdownNotaFiscalItemStatus';
                $scope.$initialValue = $scope.ngModel;
            },
            controller: ['$scope', function ($scope) {

                $scope.data = { values: [] };

                var deferred = $q
                    .when(notaFiscalItemStatusService.obterTodos())
                    .then(applyValues);

                function applyValues(values) {
                    $scope.data.values = values;
                    $scope.data.defaultValue = (values[0] || {}).idNotaFiscalItemStatus;
                    $scope.ngModel = $scope.$initialValue || $scope.ngModel;
                }
            }]
        };
    }
})();