(function () {
    'use strict';


    // Configuração da controller
    angular
        .module('SGP')
        .config(EstoqueAjusteRoute)
        .controller('EstoqueAjusteController', EstoqueAjusteController);


    // Implementação da controller
    EstoqueAjusteController.$inject = ['$scope', '$stateParams', '$state', 'ValidationService', 'ToastService', 'MovimentacaoService', 'EstoqueService'];
    function EstoqueAjusteController($scope, $stateParams, $state, $validation, toast, movimentacaoService, estoqueService) {

        $validation.prepare($scope);        
            
        $scope.clear = clear;
        $scope.confirm = toConfirm;

        function toConfirm() {
            if (!$validation.validate($scope)) return;

            var entidade = $scope.data;

            estoqueService.ajustar(entidade)
            .then(function () {
                toast.success(globalization.texts.itemAdustedWithSuccess);
                clear();
            });
        }

        function clear() {
            $scope.data = {
                cdSistema: null,
                idBandeira: null,
                cdLoja: null,
                motivoAjuste: null,
                tipoAjuste: null,
                qtEstoqueFisico: null,
                datasDeQuebra: null
            };
        }        
        
        clear();

        $scope.$watchGroup(['data.motivoAjuste.id', 'data.loja.id', 'data.itemDetalhe.id'], function (newValues, oldValues) {
            var item = $scope.data.itemDetalhe;
            var loja = $scope.data.loja;

            if (item != null && loja != null) {
                if (newValues[0] != null && newValues[0] == 14) {
                    movimentacaoService.obterDatasDeQuebra(item.id, loja.id)
                        .then(function (data) {
                            $scope.data.datasDeQuebra = data;
                        });

                    return;
                }
            }
            
            $scope.data.datasDeQuebra = null;            
        });
    }

    // Configuração do estado
    EstoqueAjusteRoute.$inject = ['$stateProvider'];
    function EstoqueAjusteRoute($stateProvider) {

        $stateProvider
            .state('estoqueAjuste', {
                url: '/estoque/ajuste',
                templateUrl: 'Scripts/app/estoque/estoque-ajuste.view.html',
                controller: 'EstoqueAjusteController'
            });
    }
})();