(function () {
    'use strict';

    // Configuração da controller
    angular
        .module('SGP')
        .config(PesquisaLogsExecucaoRoute)
        .controller('PesquisaLogsExecucaoController', PesquisaLogsExecucaoController);

    // Implementação da controller
    PesquisaLogsExecucaoController.$inject = ['$scope', '$q', '$timeout', '$stateParams', '$state', 'ValidationService', 'ProcessoService', 'PagingService'];
    function PesquisaLogsExecucaoController($scope, $q, $timeout, $stateParams, $state, $validation, processoService, pagingService) {

        var ordenacaoPadrao = 'data desc';

        $validation.prepare($scope);

        setupFilters();
        $scope.data = { values: null };
        $scope.data.paging = { offset: 0, limit: 10, orderBy: ordenacaoPadrao };
        $scope.search = search;
        $scope.clear = clear;
        $scope.orderBy = orderBy;

        function setupFilters() {
            $scope.filters = {
                cdSistema: null,
                idBandeira: null,
                bandeira: null,
                idProcesso: null,
                cdLoja: null,
                loja: null,
                dtExecucao: { startValue: null, endValue: null },
                idItem: null,
                cdItem: null,
                item: null
            };
        }

        function clear() {
            esconderLogsExecucao();
        }

        function orderBy(field) {
            pagingService.toggleSorting($scope.data.paging, field);
            search();
        }

        function exibirLogsExecucao(data) {
            $scope.data.values = data;

            pagingService.acceptPagingResults($scope.data.paging, data);
        }

        function esconderLogsExecucao() {
            setupFilters();

            $scope.data.values = null;
            $scope.data.paging.orderBy = ordenacaoPadrao;
        }

        function search(pageNumber) {

            if (!$validation.validate($scope)) return;

            pagingService.calculateOffset($scope.data.paging, pageNumber);

            var deferred = $q
                .when(processoService.pesquisarProcessosExecucao($scope.filters, $scope.data.paging))
                .then(exibirLogsExecucao)
                .catch(esconderLogsExecucao);
        }
    }

    // Configuração do estado
    PesquisaLogsExecucaoRoute.$inject = ['$stateProvider'];
    function PesquisaLogsExecucaoRoute($stateProvider) {

        $stateProvider
            .state('pesquisaLogsExecucao', {
                url: '/alertas/logsExecucao',
                templateUrl: 'Scripts/app/alertas/pesquisa-logs-execucao.view.html',
                controller: 'PesquisaLogsExecucaoController'
            });
    }
})();