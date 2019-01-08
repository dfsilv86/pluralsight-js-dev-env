(function () {
    'use strict';

    angular
        .module('SGP')
        .directive('dropdownTipoMovimentacao', DropdownTipoMovimentacao);

    DropdownTipoMovimentacao.$inject = ['$q', 'TipoMovimentacaoService'];

    function DropdownTipoMovimentacao($q, TipoMovimentacaoService) {
        return {
            restrict: 'E',
            replace: true,
            templateUrl: 'Scripts/app/components/directives/dropdown-tipo-movimentacao.template.html',
            scope: {
                ngModel: '=',
                categoria: '@'
            },
            link: function ($scope, elem, attr) {
                $scope.dropdownName = attr.name || 'dropdownTipoMovimentacao';                
                $scope.$initialValue = $scope.ngModel;
            },
            controller: ['$scope', function ($scope) {

                $scope.data = { values: [] };

                var deferred = $q
                    .when(TipoMovimentacaoService.obterPorCategoria($scope.categoria))
                    .then(applyValues);

                function applyValues(values) {                    
                    $scope.data.values = values;
                    $scope.ngModel = $scope.data.value = (values[0] || {}).dsTipoMovimentacao;
                    $scope.ngModel = $scope.$initialValue || $scope.ngModel;
                }
            }]
        };
    }
})();