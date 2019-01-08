(function () {
    'use strict';

    // Configuração da controller
    angular
        .module('SGP')
        .config(DetalheParametroFornecedorRoute)
        .controller('DetalheParametroFornecedorController', DetalheParametroFornecedorController);

    // Implementação da controller
    DetalheParametroFornecedorController.$inject = ['$scope', '$q', '$stateParams', '$state', 'PagingService', 'FornecedorParametroService', 'fornecedorParametro'];
    function DetalheParametroFornecedorController($scope, $q, $stateParams, $state, pagingService, fornecedorParametroService, fornecedorParametro) {
        $scope.data = fornecedorParametro;
        var limitDetalhamento = 10;
        $scope.detalhamento = {
            tipo: 'Todos',
            values: [],
            paging: { offset: 0, limit: limitDetalhamento }
        };

        $scope.back = back;
        $scope.detalhar = detalhar;

        detalhar();

        function exibeDetalhes(data) {
            $scope.detalhamento.values = data;
            pagingService.acceptPagingResults($scope.detalhamento.paging, data);
        }

        function escondeDetalhes() {
            $scope.detalhamento.values = null;
            resetarPaginacaoDetalhes();
        }

        function resetarPaginacaoDetalhes() {
            $scope.detalhamento.paging.offset = 0;
            $scope.detalhamento.paging.limit = limitDetalhamento;
            $scope.detalhamento.paging.orderBy = null;
        }

        function back() {
            $state.go('pesquisaParametroFornecedor');
        }

        function detalhar(pageNumber) {                        
            if (pageNumber) {
                pagingService.calculateOffset($scope.detalhamento.paging, pageNumber);
            }
            else {
                resetarPaginacaoDetalhes();
            }

            // Requisição ao serviço
            var deferred = $q
                .when(fornecedorParametroService.obterReviewDates($scope.data.idFornecedorParametro, $scope.detalhamento.tipo, $scope.detalhamento.paging))
                .then(exibeDetalhes)
                .catch(escondeDetalhes);
        }
    }

    // Configuração do estado
    DetalheParametroFornecedorRoute.$inject = ['$stateProvider'];
    function DetalheParametroFornecedorRoute($stateProvider) {

        $stateProvider
            .state('detalheParametroFornecedor', {
                url: '/vendor/parametro/:id',
                params: {
                    id: null
                },
                templateUrl: 'Scripts/app/gerenciamento/detalhe-parametro-fornecedor.view.html',
                controller: 'DetalheParametroFornecedorController',
                resolve: {
                    fornecedorParametro: ['$stateParams', 'FornecedorParametroService', function ($stateParams, service) {
                        return service.obterEstruturado($stateParams.id);
                    }]
                }
            });
    }
})();