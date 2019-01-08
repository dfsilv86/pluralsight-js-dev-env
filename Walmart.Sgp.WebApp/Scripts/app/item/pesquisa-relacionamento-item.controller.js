(function () {
    'use strict';


    // Configuração da controller
    angular
        .module('SGP')
        .config(PesquisaProdutoRoute)
        .controller('PesquisaProdutoController', PesquisaProdutoController);


    // Implementação da controller
    PesquisaProdutoController.$inject = ['$scope', '$q', '$timeout', '$stateParams', '$state', 'ValidationService', 'PagingService', 'ItemRelacionamentoService', 'capitalizeFilter'];
    function PesquisaProdutoController($scope, $q, $timeout, $stateParams, $state, $validation, pagingService, itemRelacionamentoService, capitalizeFilter) {

        $validation.prepare($scope);

        $scope.filters = $stateParams.filters || { cdItem: null, dsItem: null, cdDepartamento: null, cdSistema: null, cdSubcategoria: null, cdFineLine: null, idRegiaoCompra: null };
        $scope.data = { values: null };
        $scope.data.paging = $stateParams.paging || { offset: 0, limit: 10, orderBy: 'cdItem' };
        $scope.search = search;
        $scope.create = create;
        $scope.clear = clear;
        $scope.detail = detail;
        $scope.exportReport = exportReport;

        $scope.filters.tipoRelacionamento = sgpFixedValues.getByDescription("tipoRelacionamento", $stateParams.tipoRelacionamento);
        $scope.tipoRelacionamentoCapitalizado = capitalizeFilter($stateParams.tipoRelacionamento);

        if (!!$scope.filters && !!$scope.filters.didSearch) {
            // paging já foi persistido e carregado com a última configuração, não é necessário usar $timeout.
            search();
        }

        function exportReport() {
            var cdSistema = $scope.filters.cdSistema;
            var tipoRelacionamento = $scope.filters.tipoRelacionamento.value;
            var cdItem = $scope.filters.cdItem;
            var dsItem = $scope.filters.dsItem;
            var idDepartamento = $scope.filters.departamento ? $scope.filters.departamento.idDepartamento : null;
            var idCategoria = $scope.filters.categoria ? $scope.filters.categoria.idCategoria : null;
            var idSubcategoria = $scope.filters.subcategoria ? $scope.filters.subcategoria.idSubcategoria : null;
            var idFineline = $scope.filters.fineline ? $scope.filters.fineline.id : null;
            var idRegiaoCompra = $scope.filters.idRegiaoCompra;


            itemRelacionamentoService.exportarRelatorio(cdSistema, tipoRelacionamento, cdItem, dsItem, idDepartamento, idCategoria, idSubcategoria, idFineline, idRegiaoCompra);
        }

        function create() {

            $state.update({
                filters: $scope.filters,
                paging: $scope.data.paging
            });
            $state.go('manutencaoRelacionamento' + $scope.tipoRelacionamentoCapitalizado + 'New');
        }

        function clear() {
            escondeProdutos();
            $scope.filters.cdItem = $scope.filters.dsItem = $scope.filters.cdDepartamento = $scope.filters.cdSistema = $scope.filters.idRegiaoCompra = null;
        }

        function exibeProdutos(data) {
            $scope.data.values = data;

            pagingService.acceptPagingResults($scope.data.paging, data);

            $scope.filters.didSearch = true;
            $validation.accept($scope);
        }

        function escondeProdutos() {
            $scope.data.values = [];
            $scope.filters.didSearch = false;
        }

        function search(pageNumber) {

            if (!$validation.validate($scope)) return;

            if (!$validation.requiresAtLeastOne({
                department: $scope.filters.cdDepartamento,
                category: $scope.filters.cdCategoria,
                subcategory: $scope.filters.cdSubcategoria,
                fineline: $scope.filters.cdFineLine,
                itemCode2: $scope.filters.cdItem,
                itemDescription: $scope.filters.dsItem
            })) {
                return;
            }

            pagingService.calculateOffset($scope.data.paging, pageNumber);

            // Prepara os parâmetros de pesquisa
            var tipoRelacionamento = $scope.filters.tipoRelacionamento.value;
            var cdItem = $scope.filters.cdItem;
            var dsItem = $scope.filters.dsItem;
            var cdFineLine = $scope.filters.cdFineLine;
            var cdSubcategoria = $scope.filters.cdSubcategoria;
            var cdCategoria = $scope.filters.cdCategoria;
            var cdDepartamento = $scope.filters.cdDepartamento;
            var cdSistema = $scope.filters.cdSistema;
            var idRegiaoCompra = $scope.filters.idRegiaoCompra;

            // Requisição ao serviço
            var deferred = $q
                .when(itemRelacionamentoService.pesquisarPorTipoRelacionamento($scope.data.paging, tipoRelacionamento, cdItem, dsItem, cdFineLine, cdSubcategoria, cdCategoria, cdDepartamento, cdSistema, idRegiaoCompra))
                .then(exibeProdutos)
                .catch(escondeProdutos);
        }

        function detail(item) {

            $state.update({
                filters: $scope.filters,
                paging: $scope.data.paging
            });
            $state.go('manutencaoRelacionamento' + $scope.tipoRelacionamentoCapitalizado + 'Edit', {
                'id': item.id
            });
        }
        }

    // Configuração do estado
    PesquisaProdutoRoute.$inject = ['$stateProvider'];
    function PesquisaProdutoRoute($stateProvider) {

        $stateProvider
            .state('pesquisaProdutoVinculado', {
                url: '/item/relacionamento/vinculado',
                params: {
                    paging: null,
                    filters: null,
                    tipoRelacionamento: 'vinculado'
                },
                templateUrl: 'Scripts/app/item/pesquisa-relacionamento-item.view.html',
                controller: 'PesquisaProdutoController'
            })
            .state('pesquisaProdutoReceituario', {
                url: '/item/relacionamento/receituario',
                params: {
                    paging: null,
                    filters: null,
                    tipoRelacionamento: 'receituario'
                },
                templateUrl: 'Scripts/app/item/pesquisa-relacionamento-item.view.html',
                controller: 'PesquisaProdutoController'
            })
            .state('pesquisaProdutoManipulado', {
                url: '/item/relacionamento/manipulado',
                params: {
                    paging: null,
                    filters: null,
                    tipoRelacionamento: 'manipulado'
                },
                templateUrl: 'Scripts/app/item/pesquisa-relacionamento-item.view.html',
                controller: 'PesquisaProdutoController'
            });
    }
})();