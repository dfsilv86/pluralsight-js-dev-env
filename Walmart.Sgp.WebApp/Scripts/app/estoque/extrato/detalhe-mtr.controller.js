(function () {
    'use strict';

    // Configuração da controller
    angular
        .module('SGP')
        .config(DetalheMtrRoute)
        .controller('DetalheMtrController', DetalheMtrController);

    // Implementação da controller
    DetalheMtrController.$inject = ['$scope', '$q', '$stateParams', '$state', 'ValidationService', 'ToastService', 'movimentacao'];
    function DetalheMtrController($scope, $q, $stateParams, $state, $validation, toast, movimentacao) {
        $scope.data = movimentacao;

        $scope.itemOrigem = movimentacao.qtdMovimentacao < 0 ? (movimentacao.item.cdOldNumber + ' - ' + movimentacao.item.dsItem) : (!movimentacao.itemTransferencia.isNew ? movimentacao.itemTransferencia.cdOldNumber + ' - ' + movimentacao.itemTransferencia.dsItem : '');
        $scope.deptoOrigem = movimentacao.qtdMovimentacao < 0 ? (movimentacao.item.departamento.cdDepartamento + ' - ' + movimentacao.item.departamento.dsDepartamento) : (!movimentacao.itemTransferencia.isNew ? movimentacao.itemTransferencia.departamento.cdDepartamento + ' - ' + movimentacao.itemTransferencia.departamento.dsDepartamento : '');

        $scope.itemDestino = movimentacao.qtdMovimentacao < 0 ? (movimentacao.itemTransferencia.cdOldNumber + ' - ' + movimentacao.itemTransferencia.dsItem) : (!movimentacao.item.isNew ? movimentacao.item.cdOldNumber + ' - ' + movimentacao.item.dsItem : '');
        $scope.deptoDestino = movimentacao.qtdMovimentacao < 0 ? (movimentacao.itemTransferencia.departamento.cdDepartamento + ' - ' + movimentacao.itemTransferencia.departamento.dsDepartamento) : (!movimentacao.item.isNew ? movimentacao.item.departamento.cdDepartamento + ' - ' + movimentacao.item.departamento.dsDepartamento : '');
    }

    // Configuração do estado
    DetalheMtrRoute.$inject = ['$stateProvider'];
    function DetalheMtrRoute($stateProvider) {

        $stateProvider
            .state('detalheMtr', {
                url: '/estoque/extrato/detalhe-mtr/{id}',
                templateUrl: 'Scripts/app/estoque/extrato/detalhe-mtr.view.html',
                controller: 'DetalheMtrController',
                resolve: {
                    movimentacao: ['$stateParams', 'MovimentacaoService', function ($stateParams, service) {
                        return service.obterEstruturadoPorId($stateParams.id);
                    }]
                }
            });
    }
})();