(function () {
    'use strict';


    // Configuração da controller
    angular
        .module('SGP')
        .config(PesquisaMonitorCargaRoute)
        .controller('PesquisaMonitorCargaController', PesquisaMonitorCargaController);


    // Implementação da controller
    PesquisaMonitorCargaController.$inject = ['$scope', '$q', '$timeout', '$stateParams', '$state', 'angularMomentConfig', 'ToastService', 'ValidationService', 'PagingService', 'ProcessoService'];
    function PesquisaMonitorCargaController($scope, $q, $timeout, $stateParams, $state, angularMomentConfig, toast, $validation, pagingService, processoService) {
        $validation.prepare($scope);

        $scope.filters = $stateParams.filters || {
            cdSistema: null,
            idBandeira: null,
            cdLoja: null,
            data: new Date()
        };

        $scope.data = { values: [], colunasCargas: [] };
        $scope.data.paging = $stateParams.paging || { offset: 0, limit: 10, orderBy: 'cdLoja' };
        $scope.search = search;
        $scope.clear = clear;
        $scope.detail = detail;

        // Utiliza os filtros enviados as telas de cadastro e edição.
        if (!!$scope.filters && !!$scope.filters.didSearch) {
            // paging já foi persistido e carregado com a última configuração, não é necessário usar $timeout.
            search();
        }

        function detail(item) {

            $state.update({
                filters: $scope.filters,
                paging: $scope.data.paging
            });

            $state.go(
                'detalheMonitorCarga',
                {
                    'cdSistema': $scope.filters.cdSistema,
                    'idBandeira': item.bandeira.idBandeira,
                    'cdLoja': item.loja.cdLoja,
                    'data': moment(item.data).format('YYYY-MM-DD')
                });
        }

        function search(pageNumber) {
            if (!$validation.validate($scope)) return;

            pagingService.calculateOffset($scope.data.paging, pageNumber);

            var deferred = $q
                .when(processoService.pesquisarCargas($scope.filters, $scope.data.paging))
                .then(exibe)
                .catch(esconde);
        }

        function exibe(data) {

            $scope.data.values = data;

            var theArgs = [0, $scope.data.colunasCargas.length];
            if (data.length > 0 && data[0].cargas) {
                theArgs = theArgs.concat(data[0].cargas);
            }
            $scope.data.colunasCargas.splice.apply($scope.data.colunasCargas, theArgs);

            pagingService.acceptPagingResults($scope.data.paging, data);

            $scope.filters.didSearch = true;
            $validation.accept($scope);
        }

        function esconde(data) {
            $scope.data.colunasCargas = $scope.data.values = [];
            $scope.filters.didSearch = false;
        }

        function clear() {
            $scope.filters.cdSistema = $scope.filters.idBandeira = $scope.filters.cdLoja = null;
            $scope.filters.data = new Date();
            $scope.data.paging.offset = 0;
            $scope.data.values = [];
            $scope.data.colunasCargas.splice(0, $scope.data.colunasCargas.length);
            $scope.filters.didSearch = false;
        }
    }

    // Configuração do estado
    PesquisaMonitorCargaRoute.$inject = ['$stateProvider'];
    function PesquisaMonitorCargaRoute($stateProvider) {

        $stateProvider
            .state('pesquisaMonitorCarga', {
                url: '/monitor/carga',
                params: {
                    filters: null,
                    paging: null,
                },
                templateUrl: 'Scripts/app/monitor/pesquisa-monitor-carga.view.html',
                controller: 'PesquisaMonitorCargaController'
            });
    }
})();