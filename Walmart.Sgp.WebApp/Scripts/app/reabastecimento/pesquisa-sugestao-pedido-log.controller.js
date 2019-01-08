(function () {
    'use strict';


    // Configuração da controller
    angular
        .module('SGP')
        .config(PesquisaSugestaoPedidoLogRoute)
        .controller('PesquisaSugestaoPedidoLogController', PesquisaSugestaoPedidoLogController);


    // Implementação da controller
    PesquisaSugestaoPedidoLogController.$inject = ['$scope', '$q', '$timeout', '$stateParams', '$state', 'ValidationService', 'PagingService', 'SugestaoPedidoService'];
    function PesquisaSugestaoPedidoLogController($scope, $q, $timeout, $stateParams, $state, $validation, pagingService, sugestaoPedidoService) {

        $validation.prepare($scope);

        $scope.filters = $stateParams.filters || {
            idEntidade: $stateParams.id,
            intervalo: null
        };

        $scope.data = { values: [] };
        $scope.data.paging = $stateParams.paging || { offset: 0, limit: 10, orderBy: 'DhAuditStamp DESC' };
        $scope.search = search;
        $scope.back = back;
        $scope.clear = clear;
        $scope.autorizacoes = autorizacoes;
        
        if ($stateParams.filters) {
            search();
        }

        function back() {            
            $state.go('cadastroSugestaoPedido');
        }

        function clear() {
            $scope.filters.intervalo = null;
        }

        function search(pageNumber) {
            resetarPaginacao();
            pagingService.calculateOffset($scope.data.paging, pageNumber);

            var deferred = $q
                .when(sugestaoPedidoService.obterLogs($scope.filters, $scope.data.paging))
                .then(exibe)
                .catch(esconde);
        }

        function exibe(data) {
            $scope.data.values = data;       
            pagingService.acceptPagingResults($scope.data.paging, data);
        }

        function esconde(data) {
            $scope.data.values = [];
            resetarPaginacao();
        }

        function resetarPaginacao() {
            $scope.data.paging = { offset: 0, limit: 10, orderBy: 'DhAuditStamp DESC' };
        }

        function autorizacoes() {
            $state.update({ 'filters': $scope.filters, 'paging': $scope.data.paging });
            $state.go('pesquisaAutorizaPedido', { 'id': $stateParams.id });
        }

        // Isto dá a oportunidade ao grid-pager de configurar os itens por pagina da paginação
        // Está aqui porque a tela abre rodando diretamente a pesquisa sem usar botão pesquisar. Não usar isso nas outras telas.
        $timeout(search, 100); // suppress-validator
    }

    // Configuração do estado
    PesquisaSugestaoPedidoLogRoute.$inject = ['$stateProvider'];
    function PesquisaSugestaoPedidoLogRoute($stateProvider) {

        $stateProvider
            .state('pesquisaSugestaoPedidoLog', {
                url: '/reabastecimento/sugestao-pedido/:id/logs',
                params: {
                    id: null,
                    filters: null,
                    paging: null
                },
                templateUrl: 'Scripts/app/reabastecimento/pesquisa-sugestao-pedido-log.view.html',
                controller: 'PesquisaSugestaoPedidoLogController'
            });
    }
})();