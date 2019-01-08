(function () {
    'use strict';

    angular
        .module('SGP')
        .directive('dropdownMotivoRevisaoCusto', DropdownMotivoRevisaoCusto);

    DropdownMotivoRevisaoCusto.$inject = ['$q', 'MotivoRevisaoCustoService'];

    function DropdownMotivoRevisaoCusto($q, motivoRevisaoCustoService) {
        return {
            restrict: 'E',
            replace: true,
            templateUrl: 'Scripts/app/components/directives/dropdown-motivo-revisao-custo.template.html',
            scope: {
                ngModel: '='
            },
            link: function ($scope, elem, attr) {
                $scope.dropdownName = attr.name || 'dropdownMotivoRevisaoCusto';
                $scope.$initialValue = $scope.ngModel;
            },
            controller: ['$scope', function ($scope) {

                $scope.data = { values: [] };

                var deferred = $q
                    .when(motivoRevisaoCustoService.obterTodos())
                    .then(applyValues);

                function applyValues(values) {
                    $scope.data.values = values;
                    $scope.data.defaultValue = (values[0] || {}).IDMotivo;
                    $scope.ngModel = $scope.$initialValue || $scope.ngModel;
                }
            }]
        };
    }
})();