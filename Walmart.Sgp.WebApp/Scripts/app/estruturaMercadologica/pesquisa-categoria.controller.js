(function () {
    'use strict';

    // TODO: isso não deveria estar aqui. reaproveitar a controller dentro da modal de pesquisa (ou fazer a modal de pesquisa utilizar esta aqui)

    function PesquisaCategoriaController($scope, $q, $timeout, $stateParams, $state, $validation, pagingService, categoriaService) {
        var ordenacaoPadrao = 'idCategoria asc';                
        $validation.prepare($scope);

        $scope.filters = $stateParams.filters || { cdSistema: null, cdDepartamento: null, cdCategoria: null, dsCategoria: null };
        
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
            var cdDepartamento = $scope.filters.cdDepartamento;
            var cdCategoria = $scope.filters.cdCategoria;
            var dsCategoria = $scope.filters.dsCategoria;
            
            categoriaService.pesquisarPorCategoriaDepartamentoESistema(cdCategoria, dsCategoria, cdSistema, cdDepartamento, $scope.data.paging)
                .then(exibirRegistros)
                .catch(esconderRegistros);
        }

        $scope.search = search;

        function clear() {
            $scope.filters.cdSistema = null;
            $scope.filters.cdDepartamento = null;
            $scope.filters.cdCategoria = null;
            $scope.filters.dsCategoria = null;
            $scope.data.values = [];
        }

        $scope.clear = clear;        

        function orderBy(field) {
            pagingService.toggleSorting($scope.data.paging, field);
            search();
        }

        $scope.orderBy = orderBy;
        if ($stateParams.paging) {
            search();
        }
    }

    PesquisaCategoriaController.$inject = ['$scope', '$q', '$timeout', '$stateParams', '$state', 'ValidationService', 'PagingService', 'CategoriaService'];

    function PesquisaDepartamentoRoute($stateProvider) {

        $stateProvider
            .state('pesquisaCategoria', {
                url: '/cadastro/categoria',
                params: {
                    filters: null,
                    paging: null
                },
                templateUrl: 'Scripts/app/estruturaMercadologica/pesquisa-categoria.view.html',
                controller: 'PesquisaCategoriaController'
            });
    }

    PesquisaDepartamentoRoute.$inject = ['$stateProvider'];

    angular
        .module('SGP')
        .config(PesquisaDepartamentoRoute)
        .controller('PesquisaCategoriaController', PesquisaCategoriaController);
})();