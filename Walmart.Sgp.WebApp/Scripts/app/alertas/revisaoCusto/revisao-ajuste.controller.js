(function () {
    'use strict';

    // Configuração da controller
    angular
        .module('SGP')
        .config(RevisaoAjusteCustoRoute)
        .controller('RevisaoAjusteCustoController', RevisaoAjusteCustoController);

    // Implementação da controller
    RevisaoAjusteCustoController.$inject = ['$scope', '$q', '$stateParams', '$state', 'ValidationService', 'NotaFiscalService', 'RevisaoCustoService', 'UserSessionService', 'ToastService', 'PagingService'];
    function RevisaoAjusteCustoController($scope, $q, $stateParams, $state, $validation, notaFiscalService, revisaoCustoService, userSessionService, toast, pagingService) {

        $validation.prepare($scope);

        //setupFilters();
        //setupData();

        $scope.filters = $stateParams.filters || {
            cdSistema: null,
            idBandeira: null,
            cdLoja: null,
            cdDepartamento: null,
            cdItem: null,
            dsItem: null,
            idStatus: null
        };

        $scope.data = {
            values: null,
            custos: null,
            notasPendentes: false
        };

        $scope.data.paging = $stateParams.paging || {
            offset: 0,
            limit: 10,
            orderBy: null
        }

        if (!!$scope.filters && !!$scope.filters.didSearch) {
            // paging já foi persistido e carregado com a última configuração, não é necessário usar $timeout.
            search();
        }

        $scope.search = search;
        $scope.clear = clear;
        $scope.select = select;
        $scope.collapse = collapse;

        function setupFilters() {
            $scope.filters = {
                cdSistema: null,
                idBandeira: null,
                cdLoja: null,
                cdDepartamento: null,
                cdItem: null,
                dsItem: null,
                idStatus: null
            };
        }

        function setupData() {
            $scope.data = {
                values: null,
                custos: null,
                notasPendentes: false
            };
            $scope.data.paging = {
                offset: 0,
                limit: 10,
                orderBy: null
            };
            $scope.filters.didSearch = false;
        }

        function search(pageNumber) {

            if (!$validation.validate($scope)) {
                return;
            }

            pagingService.calculateOffset($scope.data.paging, pageNumber);

            // Requisição ao serviço para carregar o grid
            var deferred = $q
                .when(revisaoCustoService.pesquisarPorFiltros($scope.filters, $scope.data.paging))
                .then(exibeRevisoesCusto)
                .catch(escondeRevisoesCusto);
        }

        function exibeRevisoesCusto(data) {
            $scope.data.values = data;
            //$scope.data.paging = { offset: data.offset, limit: data.limit, orderBy: data.orderBy };
            pagingService.acceptPagingResults($scope.data.paging, data);

            $scope.filters.didSearch = true;
            $validation.accept($scope);
        }

        function escondeRevisoesCusto() {
            setupData();
        }

        function collapse() {
            $scope.data.detalhe = $scope.data.expanded = null;
        }

        function clear() {
            setupFilters();
            setupData();
        }

        function select(item) {
            $state.update({ filters: $scope.filters, paging: $scope.data.paging });
            $state.go('revisaoAjusteCustoEdit', { 'id': item.idRevisaoCusto });
        }
    }

    // Configuração do estado
    RevisaoAjusteCustoRoute.$inject = ['$stateProvider'];
    function RevisaoAjusteCustoRoute($stateProvider) {
        $stateProvider
            .state('revisaoAjusteCusto', {
                url: '/alertas/revisaoAjusteCusto',
                params: {
                    filters: null,
                    paging: null
                },
                templateUrl: 'Scripts/app/alertas/revisaoCusto/revisao-ajuste.view.html',
                controller: 'RevisaoAjusteCustoController'
            });
    }
})();