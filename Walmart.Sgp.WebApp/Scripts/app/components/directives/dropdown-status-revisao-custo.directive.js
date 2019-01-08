(function () {
    'use strict';

    angular
        .module('SGP')
        .directive('dropdownStatusRevisaoCusto', DropdownStatusRevisaoCusto);

    DropdownStatusRevisaoCusto.$inject = ['$q', 'StatusRevisaoCustoService'];

    function DropdownStatusRevisaoCusto($q, statusRevisaoCustoService) {
        return {
            restrict: 'E',
            replace: true,
            templateUrl: 'Scripts/app/components/directives/dropdown-status-revisao-custo.template.html',
            scope: {
                ngModel: '='
            },
            link: function ($scope, elem, attr) {
                $scope.dropdownName = attr.name || 'dropdownStatusRevisaoCusto';
                $scope.$initialValue = $scope.ngModel;
            },
            controller: ['$scope', function ($scope) {

                $scope.data = { values: [] };

                var deferred = $q
                    .when(statusRevisaoCustoService.obterTodos())
                    .then(applyValues);

                function applyValues(values) {
                    $scope.data.values = values;
                    $scope.data.defaultValue = (values[0] || {}).IDStatusRevisaoCusto;
                    $scope.ngModel = $scope.$initialValue || $scope.ngModel;
                }
            }]
        };
    }
})();