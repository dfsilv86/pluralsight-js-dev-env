(function () {
    'use strict';

    // Configuração da controller
    angular
        .module('SGP')
        .config(PesquisaNotaFiscalRoute)
        .controller('PesquisaNotaFiscalController', PesquisaNotaFiscalController);

    // Implementação da controller
    PesquisaNotaFiscalController.$inject = ['$scope', '$q', '$timeout', '$stateParams', '$state', 'ValidationService', 'NotaFiscalService', 'PagingService'];
    function PesquisaNotaFiscalController($scope, $q, $timeout, $stateParams, $state, $validation, notaFiscalService, pagingService) {

        var ordenacaoPadrao = 'nrNotaFiscal asc';

        $validation.prepare($scope);

        setupFilters();

        $scope.data = { values: null };
        $scope.data.paging = $stateParams.paging || { offset: 0, limit: 10, orderBy: ordenacaoPadrao };

        $scope.search = search;
        $scope.clear = clear;
        $scope.detail = detail;

        function setupFilters() {
            $scope.filters = $stateParams.filters || {
                cdSistema: null,
                idBandeira: null,
                cdLoja: null,
                loja: null,
                cdFornecedor: null,
                fornecedor: null,
                nrNotaFiscal: null,
                cdItem: null,
                item: null,
                dtRecebimento: {},
                dtCadastroConcentrador: {},
                dtAtualizacaoConcentrador: {}
            };
        }

        function clear() {
            escondeNotasFiscais();
        }

        function exibeNotasFiscais(data) {
            $scope.data.values = data;

            pagingService.acceptPagingResults($scope.data.paging, data);

            $scope.filters.didSearch = true;
            $validation.accept($scope);
        }

        function escondeNotasFiscais() {
            setupFilters();
            $scope.data.values = null;
            $scope.data.paging.orderBy = ordenacaoPadrao;
            $scope.filters.didSearch = false;
        }

        function search(pageNumber) {

            if (!$validation.validate($scope)) return;

            pagingService.calculateOffset($scope.data.paging, pageNumber);

            // Requisição ao serviço
            var deferred = $q
                .when(notaFiscalService.pesquisarPorFiltros($scope.filters, $scope.data.paging))
                .then(exibeNotasFiscais)
                .catch(escondeNotasFiscais);
        }

        function detail(item) {

            $state.update({
                filters: $scope.filters,
                paging: $scope.data.paging
            });

            $state.go('notaFiscalEdit', {
                'id': item.idNotaFiscal
            });
        }

        if (!!$scope.filters && !!$scope.filters.didSearch) {
            // paging já foi persistido e carregado com a última configuração, não é necessário usar $timeout.
            search();
        }
    }

    // Configuração do estado
    PesquisaNotaFiscalRoute.$inject = ['$stateProvider'];
    function PesquisaNotaFiscalRoute($stateProvider) {

        $stateProvider
            .state('pesquisaNotaFiscal', {
                url: '/notafiscal/pesquisa',
                params: {
                    filters: null,
                    paging: null
                },
                templateUrl: 'Scripts/app/notafiscal/pesquisa-nota-fiscal.view.html',
                controller: 'PesquisaNotaFiscalController'
            });
    }
})();