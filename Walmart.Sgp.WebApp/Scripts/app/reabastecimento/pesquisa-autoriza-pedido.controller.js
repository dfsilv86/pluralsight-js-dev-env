(function () {
    'use strict';


    // Configuração da controller
    angular
        .module('SGP')
        .config(PesquisaAutorizaPedidoLogRoute)
        .controller('PesquisaAutorizaPedidoController', PesquisaAutorizaPedidoLogController);


    // Implementação da controller
    PesquisaAutorizaPedidoLogController.$inject = ['$scope', '$q', '$timeout', '$stateParams', '$state', 'PagingService', 'AutorizaPedidoService'];
    function PesquisaAutorizaPedidoLogController($scope, $q, $timeout, $stateParams, $state, pagingService, autorizaPedidoService) {        
        $scope.data = { values: [] };
        $scope.data.paging = { offset: 0, limit: 10, orderBy: 'DtAutorizacao DESC' };
        $scope.back = back;
        $scope.search = search;
        
        function back() {         
            $state.go('pesquisaSugestaoPedidoLog');
        }

        function search(pageNumber) {

            pagingService.calculateOffset($scope.data.paging, pageNumber);

            var deferred = $q
                .when(autorizaPedidoService.obterAutorizacoesPorSugestaoPedido($stateParams.id, $scope.data.paging))
                .then(exibe)
                .catch(esconde);
        }

        function exibe(data) {
            $scope.data.values = data;       
            pagingService.acceptPagingResults($scope.data.paging, data);
        }

        function esconde(data) {
            $scope.data.values = [];
        }

        search(1);
    }

    // Configuração do estado
    PesquisaAutorizaPedidoLogRoute.$inject = ['$stateProvider'];
    function PesquisaAutorizaPedidoLogRoute($stateProvider) {

        $stateProvider
            .state('pesquisaAutorizaPedido', {
                url: '/reabastecimento/sugestao-pedido/:id/logs/autorizacoes',
                params: {
                    id: null
                },
                templateUrl: 'Scripts/app/reabastecimento/pesquisa-autoriza-pedido.view.html',
                controller: 'PesquisaAutorizaPedidoController'
            });
    }
})();