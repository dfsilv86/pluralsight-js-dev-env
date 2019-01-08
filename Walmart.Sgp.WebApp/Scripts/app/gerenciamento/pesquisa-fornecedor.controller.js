(function () {
    'use strict';

    function PesquisaFornecedorController($scope, $q, $timeout, $stateParams, $state, $validation, pagingService, fornecedorService) {
        var ordenacaoPadrao = 'idFornecedor asc';

        $validation.prepare($scope);

        $scope.filters = $stateParams.filters || { cdSistema: null, cdFornecedor: null, nmFornecedor: null };

        $scope.data = { values: null };
        $scope.data.paging = $stateParams.paging || { offset: 0, limit: 10, orderBy: ordenacaoPadrao };

        function esconderRegistros() {
            $scope.data.values = [];
        }

        function exibirRegistros(data) {
            $scope.data.values = data;
            pagingService.acceptPagingResults($scope.data.paging, data);
        }

        function search(pageNumber) {
            if (!$validation.validate($scope)) return;

            pagingService.calculateOffset($scope.data.paging, pageNumber);

            var cdSistema = $scope.filters.cdSistema;
            var cdFornecedor = $scope.filters.cdFornecedor;
            var nmFornecedor = $scope.filters.nmFornecedor;

            fornecedorService.obterListaPorSistemaCodigoNome(cdSistema, cdFornecedor, nmFornecedor, $scope.data.paging)
                .then(exibirRegistros)
                .catch(esconderRegistros);
        }

        $scope.search = search;

        function clear() {
            $scope.filters.cdSistema = null;
            $scope.filters.nmFornecedor = null;
            $scope.filters.cdFornecedor = null;
            $scope.data.values = [];
        }

        $scope.clear = clear;

        function detail(item) {
            $state.go('cadastroFornecedor', {
                'id': item.id,
                filters: $scope.filters,
                paging: $scope.data.paging
            });
        }

        $scope.detail = detail;

        function orderBy(field) {
            pagingService.toggleSorting($scope.data.paging, field);
            search();
        }

        $scope.orderBy = orderBy;
        if ($stateParams.paging) {
            search();
        }
    }

    PesquisaFornecedorController.$inject = ['$scope', '$q', '$timeout', '$stateParams', '$state', 'ValidationService', 'PagingService', 'FornecedorService'];

    function PesquisaFornecedorRoute($stateProvider) {

        $stateProvider
            .state('pesquisaFornecedor', {
                url: '/cadastro/fornecedor',
                params: {
                    filters: null,
                    paging: null
                },
                templateUrl: 'Scripts/app/gerenciamento/pesquisa-fornecedor.view.html',
                controller: 'PesquisaFornecedorController'
            });
    }

    PesquisaFornecedorRoute.$inject = ['$stateProvider'];

    angular
        .module('SGP')
        .config(PesquisaFornecedorRoute)
        .controller('PesquisaFornecedorController', PesquisaFornecedorController);
})();