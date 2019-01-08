(function () {
    'use strict';

    // TODO: isso não deveria estar aqui. reaproveitar a controller dentro da modal de pesquisa (ou fazer a modal de pesquisa utilizar esta aqui)

    // Configuração da controller
    angular
        .module('SGP')
        .config(FinelineRoute)
        .controller('FinelineController', FinelineController);


    // Implementação da controller
    FinelineController.$inject = ['$scope', '$q', '$stateParams', '$state', 'ValidationService', 'PagingService', 'FineLineService'];
    function FinelineController($scope, $q, $stateParams, $state, $validation, pagingService, fineLineService) {

        initialize();

        function initialize() {
            $validation.prepare($scope);

            $scope.filters = {};
            $scope.data = { values: null };
            $scope.data.paging = { offset: 0, limit: 10, orderBy: 'cdFineLine' };

            $scope.search = search;
            $scope.clear = clear;
            $scope.orderBy = orderBy;
        }

        function clear() {
            $scope.filters.cdSistema =
                $scope.filters.cdDepartamento =
                $scope.filters.cdCategoria =
                $scope.filters.cdSubcategoria =
                $scope.filters.codigo =
                $scope.filters.descricao = null;

            $scope.data.paging.offset = 0;
            $scope.data.values = [];
        }

        function exibe(data) {
            $scope.data.values = data;
            pagingService.acceptPagingResults($scope.data.paging, data);
        }

        function esconde() {
            $scope.data.values = [];
        }

        function search(pageNumber) {
            if (!$validation.validate($scope)) return;

            pagingService.calculateOffset($scope.data.paging, pageNumber);

            var cdSistema = $scope.filters.cdSistema || '';
            var cdDepartamento = $scope.filters.cdDepartamento || '';
            var cdCategoria = $scope.filters.cdCategoria || '';
            var cdSubcategoria = $scope.filters.cdSubcategoria || '';
            var codigo = $scope.filters.codigo || '';
            var descricao = $scope.filters.descricao || '';

            $q.when(fineLineService.pesquisarPorFineLineSubcategoriaCategoriaDepartamentoESistema(codigo, descricao, cdSubcategoria, cdCategoria, cdDepartamento, cdSistema, $scope.data.paging))
                .then(exibe)
                .catch(esconde);
        }

        function orderBy(field) {
            pagingService.toggleSorting($scope.data.paging, field);
            search();
        }
    }

    // Configuração do estado
    FinelineRoute.$inject = ['$stateProvider'];
    function FinelineRoute($stateProvider) {

        $stateProvider
            .state('pesquisaFineline', {
                url: '/cadastro/fineline',
                params: {
                    filters: null,
                    paging: null
                },
                templateUrl: 'Scripts/app/estruturaMercadologica/pesquisa-fineline.view.html',
                controller: 'FinelineController'
            });
    }
})();