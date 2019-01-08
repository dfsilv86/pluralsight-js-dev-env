(function () {
    'use strict';

    // Configuração da controller
    angular
        .module('SGP')
        .config(PesquisaParametroProdutoRoute)
        .controller('PesquisaParametroProdutoController', PesquisaParametroProdutoController);

    // Implementação da controller
    PesquisaParametroProdutoController.$inject = ['$scope', '$q', '$timeout', '$stateParams', '$state', 'ValidationService', 'ItemDetalheService', 'UserSessionService', 'PagingService'];
    function PesquisaParametroProdutoController($scope, $q, $timeout, $stateParams, $state, $validation, itemDetalheService, userSession, pagingService) {

        $validation.prepare($scope);

        $scope.filters = $stateParams.filters || { cdItem: null, dsItem: null, tpStatus: null, cdSistema: null, cdDepartamento: null, cdCategoria: null, cdSubcategoria: null, cdFineLine: null, idRegiaoCompra: null };
        $scope.data = { values: null };
        $scope.data.paging = $stateParams.paging || { offset: 0, limit: 10, orderBy: 'dsItem asc' };
        $scope.search = search;
        $scope.clear = clear;
        $scope.detail = detail;
        $scope.orderBy = orderBy;

        if (!!$scope.filters && !!$scope.filters.didSearch) {
            // paging já foi persistido e carregado com a última configuração, não é necessário usar $timeout.
            search();
        }

        function clear() {
            escondeProdutos();

            //Seta o valor do campo status como "Ativo"

            var ativo = sgpFixedValues.tipoStatusItem[0].value;

            $scope.filters.tpStatus = ativo;

            $scope.filters.cdItem = $scope.filters.dsItem = $scope.filters.cdSistema = $scope.filters.idRegiaoCompra = null;
        }

        function orderBy(field) {
            pagingService.toggleSorting($scope.data.paging, field);
            search();
        }

        function exibeProdutos(data) {
            $scope.data.values = data;

            pagingService.acceptPagingResults($scope.data.paging, data);

            $scope.filters.didSearch = true;
            $validation.accept($scope);
        }

        function escondeProdutos() {
            $scope.data.values = null;
            $scope.filters.didSearch = false;
        }

        function search(pageNumber) {

            if (!$validation.validate($scope)) return;

            pagingService.calculateOffset($scope.data.paging, pageNumber);

            // Prepara os parâmetros de pesquisa
            var idUsuario = userSession.getCurrentUser().id;
            var cdItem = $scope.filters.cdItem;
            var cdPLU = null;
            var dsItem = $scope.filters.dsItem;
            var tpStatus = $scope.filters.tpStatus;
            var cdFineLine = $scope.filters.cdFineLine;
            var cdSubcategoria = $scope.filters.cdSubcategoria;
            var cdCategoria = $scope.filters.cdCategoria;
            var cdDepartamento = $scope.filters.cdDepartamento;
            var cdSistema = $scope.filters.cdSistema;
            var idRegiaoCompra = $scope.filters.idRegiaoCompra;
            
            // Requisição ao serviço
            var deferred = $q
                .when(itemDetalheService.PesquisarPorFiltroTipoReabastecimento(cdItem, cdPLU, dsItem, tpStatus, cdFineLine, cdSubcategoria, cdCategoria, cdDepartamento, cdSistema, idUsuario, idRegiaoCompra, $scope.data.paging))
                .then(exibeProdutos)
                .catch(escondeProdutos);
        }

        function detail(item) {

            $state.update({
                filters: $scope.filters,
                paging: $scope.data.paging
            });

            $state.go('detalheParametroProduto', {
                'id': item.idItemDetalhe
            });
        }
    }

    // Configuração do estado
    PesquisaParametroProdutoRoute.$inject = ['$stateProvider'];
    function PesquisaParametroProdutoRoute($stateProvider) {

        $stateProvider
            .state('pesquisaParametroProduto', {
                url: '/item/parametro',
                params: {
                    filters: null,
                    paging: null
                },
                templateUrl: 'Scripts/app/item/pesquisa-parametro-item.view.html',
                controller: 'PesquisaParametroProdutoController'
            });
    }
})();