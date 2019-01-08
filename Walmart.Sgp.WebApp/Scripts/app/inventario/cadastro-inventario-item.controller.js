/*global sgpFixedValues*/
(function () {
    'use strict';

    // Implementação da controller    
    function CadastroInventarioItemController($scope, $q, $uibModalInstance, $validation, toastService, inventarioService, inventarioItem, inventario) {
        if (!inventarioItem) {
            inventarioItem = {
                isNew: true,
                itemDetalhe: {
                    cdItem: null,
                    cdplu: null
                },
                idItemDetalhe: 0,
                idInventarioItem: 0
            };
        }
       
        $scope.data = inventarioItem;
        $validation.prepare($scope);        

        $scope.configuracao = {
            tipoPesquisa: 'cdItem',
            cdItem: inventarioItem.itemDetalhe.cdItem,
            cdPLU: null,
            podeAlterarItem: inventarioItem.isNew,
            podeExcluirItem: !inventarioItem.isNew,
            itemDetalhePorCodigo: null,
            itemDetalhePorPLU: null,
            cdSistema: inventario.bandeira.cdSistema
        };

        $scope.$watch('configuracao.tipoPesquisa', function (newVal, oldVal) {
            if (newVal === oldVal) {
                return;
            }

            $scope.configuracao.cdItem =
                $scope.configuracao.cdPLU = null;
        });        

        function sair() {
            $uibModalInstance.dismiss();
        }

        function fechar() {
            $uibModalInstance.close(true);
        }

        function salvar() {
            if (!$validation.validate($scope)) return;
            $scope.data.itemDetalhe = ($scope.configuracao.tipoPesquisa === 'cdItem'
                ? $scope.configuracao.itemDetalhePorCodigo
                : $scope.configuracao.itemDetalhePorPLU);
            $scope.data.idItemDetalhe = $scope.data.itemDetalhe.idItemDetalhe;

            inventarioService.salvarItem($scope.data, inventario).then(function () {
                var message = $scope.data.isNew ?
                    globalization.texts.inventoryItemAddedSuccessfully :
                    globalization.texts.inventoryItemUpdatedSuccessfully;
                toastService.success(message);
                fechar();
            });
        }

        function remover() {
            inventarioService.removerItem($scope.data.idInventarioItem).then(function () {
                toastService.success(globalization.texts.inventoryItemRemovedSuccessfully);
                fechar();
            });
        }

        $scope.sair = sair;
        $scope.remover = remover;
        $scope.salvar = salvar;
    }

    CadastroInventarioItemController.$inject = ['$scope', '$q', '$uibModalInstance', 'ValidationService', 'ToastService', 'InventarioService', 'inventarioItem', 'inventario'];

    // Configuração da controller
    angular
        .module('SGP')        
        .controller('CadastroInventarioItemController', CadastroInventarioItemController);
})();