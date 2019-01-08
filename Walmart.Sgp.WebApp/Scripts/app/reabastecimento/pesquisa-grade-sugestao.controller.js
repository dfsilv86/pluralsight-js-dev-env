(function () {
    'use strict';

    // Configuração da controller
    angular
        .module('SGP')
        .config(PesquisaGradeSugestaoRoute)
        .controller('PesquisaGradeSugestaoController', PesquisaGradeSugestaoController);

    // Implementação da controller
    PesquisaGradeSugestaoController.$inject = ['$scope', '$q', '$timeout', '$stateParams', '$state', 'ValidationService', 'GradeSugestaoService', 'PagingService'];
    function PesquisaGradeSugestaoController($scope, $q, $timeout, $stateParams, $state, $validation, gradeSugestaoService, pagingService) {

        var ordenacaoPadrao = 'nmLoja, dsDepartamento asc';

        $validation.prepare($scope);

        $scope.filters = $stateParams.filters || { cdSistema: null, idBandeira: null, cdLoja: null, cdDepartamento: null };
        
        $scope.data = { values: null };
        $scope.data.paging = $stateParams.paging || { offset: 0, limit: 10, orderBy: ordenacaoPadrao };

        $scope.search = search;
        $scope.clear = clear;
        $scope.detail = detail;
        $scope.orderBy = orderBy;
        $scope.create = create;

        function create() {

            $state.update({
                filters: $scope.filters,
                paging: $scope.data.paging
            });

            $state.go('cadastroGradeSugestaoNew');
        }

        function clear() {
            //$scope.filters.idBandeira = null;
            //$scope.filters.cdLoja = null,
            //$scope.filters.cdDepartamento = null;
            $scope.filters.cdDepartamento = $scope.filters.cdLoja =
                $scope.filters.idBandeira = $scope.filters.cdSistema = null;
            esconderRegistros();
        }

        function orderBy(field) {
            pagingService.toggleSorting($scope.data.paging, field);
            search();
        }

        function exibirRegistros(data) {
            $scope.data.values = data;

            pagingService.acceptPagingResults($scope.data.paging, data);

            $scope.filters.didSearch = true;
            $validation.accept($scope);
        }

        function esconderRegistros() {
            $scope.data.values = null;
            $scope.data.paging.orderBy = ordenacaoPadrao;
            $scope.filters.didSearch = false;
        }

        function search(pageNumber) {

            if (!$validation.validate($scope)) return;

            pagingService.calculateOffset($scope.data.paging, pageNumber);

            // Prepara os parâmetros de pesquisa            
            var cdSistema = $scope.filters.cdSistema;
            var idBandeira = $scope.filters.idBandeira;
            var cdLoja = $scope.filters.cdLoja;
            var cdDepartamento = $scope.filters.cdDepartamento;

            // Requisição ao serviço
            var deferred = $q
                .when(gradeSugestaoService.pesquisarPorFiltro(cdSistema, idBandeira, cdDepartamento, cdLoja, $scope.data.paging))
                .then(exibirRegistros)
                .catch(esconderRegistros);
        }

        function detail(item) {

            $state.update({
                filters: $scope.filters,
                paging: $scope.data.paging
            });

            $state.go('cadastroGradeSugestaoEdit', {
                'id': item.idGradeSugestao
            });
        }

        if (!!$scope.filters && !!$scope.filters.didSearch) {
            // paging já foi persistido e carregado com a última configuração, não é necessário usar $timeout.
            search();
        }        
    }

    // Configuração do estado
    PesquisaGradeSugestaoRoute.$inject = ['$stateProvider'];
    function PesquisaGradeSugestaoRoute($stateProvider) {

        $stateProvider
            .state('pesquisaGradeSugestao', {
                url: '/reabastecimento/grade-sugestao',
                params: {
                    filters: null,
                    paging: null
                },
                templateUrl: 'Scripts/app/reabastecimento/pesquisa-grade-sugestao.view.html',
                controller: 'PesquisaGradeSugestaoController'
            });
    }
})();