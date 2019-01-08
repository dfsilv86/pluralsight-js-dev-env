(function () {
    'use strict';

    angular
        .module('SGP')
        .directive('dropdownMotivoMovimentacao', DropdownMotivoMovimentacao);

    DropdownMotivoMovimentacao.$inject = ['$q', 'MotivoMovimentacaoService'];

    function DropdownMotivoMovimentacao($q, motivoMovimentacaoService) {
        return {
            restrict: 'E',
            replace: true,
            templateUrl: 'Scripts/app/components/directives/dropdown-motivo-movimentacao.template.html',
            scope: {
                ngModel: '='
            },
            link: function ($scope, elem, attr) {
                $scope.dropdownName = attr.name || 'dropdownMotivoMovimentacao';
                $scope.$initialValue = $scope.ngModel;
            },
            controller: ['$scope', function ($scope) {

                $scope.data = { values: [] };

                var deferred = $q
                    .when(motivoMovimentacaoService.obterVisiveis())
                    .then(applyValues);

                function applyValues(values) {
                    $scope.data.values = values;
                    $scope.ngModel = $scope.data.value = (values[0] || {}).dsMotivo;
                    $scope.ngModel = $scope.$initialValue || $scope.ngModel;
                }
            }]
        };
    }
})();