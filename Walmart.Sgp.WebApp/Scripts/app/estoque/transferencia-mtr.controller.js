(function () {
    'use strict';


    // Configuração da controller
    angular
        .module('SGP')
        .config(TransferenciaMtrRoute)
        .controller('TransferenciaMtrController', TransferenciaMtrController);


    // Implementação da controller
    TransferenciaMtrController.$inject = ['$scope', '$stateParams', '$state', 'ValidationService', 'ToastService', 'MovimentacaoService', 'EstoqueService'];
    function TransferenciaMtrController($scope, $stateParams, $state, $validation, toast, movimentacaoService, estoqueService) {

        $validation.prepare($scope);        
            
        $scope.clear = clear;
        $scope.confirm = toConfirm;
        $scope.obterCustos = obterCustos;

        function toConfirm() {
            if (!$validation.validate($scope)) return;
            
            var data = $scope.data;            
            var movimentacaoMtr =
            {
                idItemOrigem: data.itemOrigem.idItemDetalhe,
                idItemDestino: data.itemDestino.idItemDetalhe,
                idLoja: data.loja.idLoja,
                quantidade: data.quantidade
            };

            estoqueService.realizarMtr(movimentacaoMtr)
            .then(function () {
                toast.success(globalization.texts.mtrDoneWithSuccessfully);
                clear();
            });
        }

        function clear() {
            $scope.data = {
                cdSistema: null,
                idBandeira: null,
                cdLoja: null,
                cdItemOrigem: null,
                cdItemDestino: null,
                quantidade: null,
                custoUnitario: null,
                custoMtr: null
            };
        }

        function obterCustos() {
            if (!$validation.validate($scope)) return;

            var data = $scope.data;

            estoqueService.obterUltimoCustoContabilItem(data.itemOrigem.idItemDetalhe, data.loja.idLoja)
            .then(function (custoUnitario) {
                data.custoUnitario = custoUnitario;
                data.custoMtr = custoUnitario * data.quantidade;
            });            
        }
        
        clear();      
    }

    // Configuração do estado
    TransferenciaMtrRoute.$inject = ['$stateProvider'];
    function TransferenciaMtrRoute($stateProvider) {

        $stateProvider
            .state('transferenciaMtr', {
                url: '/estoque/transferencia-mtr',
                templateUrl: 'Scripts/app/estoque/transferencia-mtr.view.html',
                controller: 'TransferenciaMtrController'
            });
    }
})();