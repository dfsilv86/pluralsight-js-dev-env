(function () {
    'use strict';

    // Configuração da controller
    angular
        .module('SGP')
        .config(PesquisaRoteiroRoute)
        .controller('PesquisaRoteiroController', PesquisaRoteiroController);

    // Implementação da controller
    PesquisaRoteiroController.$inject = ['$scope', '$q', '$timeout', '$stateParams', '$state', 'UserSessionService', 'ValidationService', 'PagingService', 'ChangeTrackerFactory', 'RoteiroService'];
    function PesquisaRoteiroController($scope, $q, $timeout, $stateParams, $state, userSession, $validation, pagingService, changeTrackerFactory, roteiroService) {

        initialize();

        function initialize() {
            $validation.prepare($scope);

            $scope.data = {};
            $scope.data.paging = $stateParams.paging || { offset: 0, limit: 40, orderBy: null };
            $scope.filters = $stateParams.filters || { cdSistema: 1, cdV9D: null, vendor: null, cdDepartamento: null, cdDivisao: null, cdLoja: null, bandeira: null, script: null };
            $scope.temp = { departamento: null, loja: null };

            $scope.clear = clear;
            $scope.search = search;
            $scope.orderBy = orderBy;
            $scope.new = doNew;
            $scope.edit = edit;

            if ($scope.filters.didSearch) {
                search();
            }
        }

        function filtersHasValue() {
            return $scope.filters.cdV9D !== null
                || $scope.filters.cdLoja !== null
                || $scope.filters.cdDepartamento !== null
                || $scope.filters.script !== null;
        }

        function clear() {
            $scope.filters.cdV9D = null;
            $scope.filters.cdDepartamento = null;
            $scope.filters.cdLoja = null;
            $scope.filters.script = null;
            $scope.data.paging.offset = 0;
            $scope.data.values = [];
            $scope.filters.didSearch = false;
        }

        function search(pageNumber) {
            if (!$validation.validate($scope))
                return;

            pagingService.calculateOffset($scope.data.paging, pageNumber);

            var cdV9D = $scope.filters.cdV9D || '';
            var cdDepartamento = $scope.filters.cdDepartamento || '';
            var cdLoja = $scope.filters.cdLoja || '';
            var script = $scope.filters.script || '';

            $q.when(roteiroService.obterRoteirosPorFornecedor(cdV9D, cdDepartamento, cdLoja, script, $scope.data.paging))
                .then(exibe)
                .catch(esconde);
        }

        function doNew() {
            $state.update({ filters: $scope.filters, paging: $scope.data.paging });
            $state.go('cadastroRoteiroNew');
        }

        function edit(item) {
            $state.update({ filters: $scope.filters, paging: $scope.data.paging });
            $state.go('cadastroRoteiroEdit', {
                'id': item.idRoteiro
            });
        }

        function exibe(data) {
            $scope.data.values = data;
            pagingService.acceptPagingResults($scope.data.paging, data);
            $scope.filters.didSearch = true;
            $validation.accept($scope);
        }

        function esconde() {
            $scope.data.values = [];
            $scope.filters.didSearch = false;
        }

        function orderBy(field) {
            pagingService.toggleSorting($scope.data.paging, field);
            search();
        }
    }

    // Configuração do estado
    PesquisaRoteiroRoute.$inject = ['$stateProvider'];
    function PesquisaRoteiroRoute($stateProvider) {

        $stateProvider
            .state('cadastroRoteiroEntrega', {
                url: '/reabastecimento/cadastro-roteiro-entrega',
                templateUrl: 'Scripts/app/reabastecimento/roteirizacao/pesquisa-roteiro.view.html',
                controller: 'PesquisaRoteiroController',
                params: {
                    filters: null,
                    paging: null
                }
            });
    }
})();