(function () {
    'use strict';

    // TODO: isso não deveria estar aqui. reaproveitar a controller dentro da modal de pesquisa (ou fazer a modal de pesquisa utilizar esta aqui)

    PesquisaSubcategoriaController.$inject = ['$scope', '$q', '$timeout', '$stateParams', '$state', 'ValidationService', 'PagingService', 'SubcategoriaService'];

    function PesquisaSubcategoriaController($scope, $q, $timeout, $stateParams, $state, $validation, pagingService, subcategoriaService) {

        initialize();

        function initialize() {

            $validation.prepare($scope);

            $scope.search = search;
            $scope.clear = clear;
            $scope.orderBy = orderBy;

            $scope.filters = $stateParams.filters || {};

            $scope.data = { values: null };
            $scope.data.paging = $stateParams.paging || { offset: 0, limit: 10, orderBy: null };
        }

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


            subcategoriaService.pesquisarPorSubcategoriaCategoriaDepartamentoESistema($scope.filters.code, $scope.filters.description, $scope.filters.cdCategoria, $scope.filters.cdDepartamento, $scope.filters.cdSistema, $scope.data.paging)
                .then(exibirRegistros)
                .catch(esconderRegistros);
        }

        function clear() {
            $scope.filters.cdSistema = null;
            $scope.filters.cdDepartamento = null;
            $scope.filters.cdCategoria = null;
            $scope.filters.code = null;
            $scope.filters.description = null;

            esconderRegistros();
        }

        function orderBy(field) {
            pagingService.toggleSorting($scope.data.paging, field);
            search();
        }

        if ($stateParams.paging) {
            search();
        }
    }

    function PesquisaSubcategoriaRoute($stateProvider) {

        $stateProvider
            .state('pesquisaSubcategoria', {
                url: '/cadastro/subcategoria',
                params: {
                    filters: null,
                    paging: null
                },
                templateUrl: 'Scripts/app/estruturaMercadologica/pesquisa-subcategoria.view.html',
                controller: 'PesquisaSubcategoriaController'
            });
    }

    PesquisaSubcategoriaRoute.$inject = ['$stateProvider'];

    angular
        .module('SGP')
        .config(PesquisaSubcategoriaRoute)
        .controller('PesquisaSubcategoriaController', PesquisaSubcategoriaController);
})();