(function () {
    'use strict';

    angular
        .module('SGP')
        .directive('dropdownItemEntrada', DropdownItemEntrada);

    DropdownItemEntrada.$inject = ['$q', 'ItemDetalheService'];

    function DropdownItemEntrada($q, itemDetalheService) {
        return {
            restrict: 'E',
            replace: true,
            templateUrl: 'Scripts/app/components/directives/dropdown-item-entrada.template.html',
            scope: {
                sistema: '=',
                itemSaida: '=',
                fornecedor: '=',
                itemOut: '=?',
                ngModel: '='
            },
            link: function ($scope, elem, attr) {
                $scope.dropdownName = attr.name || 'dropdownItemEntrada';
                $scope.$initialValue = $scope.ngModel;
            },
            controller: ['$scope', 'UserSessionService', function ($scope, userSession) {

                $scope.data = { values: [], defaultValue: null };

                $scope.$watchGroup(['sistema', 'itemSaida', 'fornecedor'], refresh);
                $scope.resetValue = resetValue;

                function refresh(newValues, oldValues) {

                    var didReset = !!newValues[0] && !!oldValues[0] ? newValues[0] != oldValues[0] : true;
                    didReset = didReset || !!newValues[1] && !!oldValues[1] ? newValues[1] != oldValues[1] : true;

                    if (didReset) {
                        lookup();
                    }
                }

                function lookup() {

                    var cdSistema = $scope.sistema;
                    var cdItemSaida = $scope.itemSaida;
                    var idFornecedorParametro = $scope.fornecedor;

                    if (angular.isDefined(cdSistema) && !!cdSistema && angular.isDefined(cdItemSaida) && !!cdItemSaida) {

                        var deferred = $q
                            .when(itemDetalheService.obterItemEntradaPorItemSaida(cdSistema, cdItemSaida, idFornecedorParametro))
                            .then(applyValues);
                    }
                    else
                    {
                        $scope.data.values = [];
                    }
                }

                function applyValues(itens) {
                    $scope.data.values = itens;
                    $scope.data.values.splice(0, 0, { idItemDetalhe: null, dsItem: 'Selecionar...' });

                    resetValue();
                }

                function resetValue() {
                    $scope.ngModel = $scope.$initialValue;
                }
            }]
        };
    }
})();