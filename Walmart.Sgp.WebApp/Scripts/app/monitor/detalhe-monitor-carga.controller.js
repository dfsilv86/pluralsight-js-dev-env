(function () {
    'use strict';

    // Configuração da controller
    angular
        .module('SGP')
        .config(DetalheMonitorCargaRoute)
        .controller('DetalheMonitorCargaController', DetalheMonitorCargaController);

    // Implementação da controller
    DetalheMonitorCargaController.$inject = ['$scope', '$q', '$stateParams', '$state', 'ProcessoService', 'monitorCarga'];
    function DetalheMonitorCargaController($scope, $q, $stateParams, $state, service, monitorCarga) {        
        $scope.data = monitorCarga;
        $scope.back = back;

        function back() {
            $state.go('pesquisaMonitorCarga');
        }     
    }

    // Configuração do estado
    DetalheMonitorCargaRoute.$inject = ['$stateProvider'];
    function DetalheMonitorCargaRoute($stateProvider) {

        $stateProvider
            .state('detalheMonitorCarga', {
                url: '/monitor/carga/loja/:cdSistema/:idBandeira/:cdLoja/?data',
                params: {
                    cdSistema: null,
                    idBandeira: null,
                    cdLoja: null,
                    data: null,
                },
                templateUrl: 'Scripts/app/monitor/detalhe-monitor-carga.view.html',
                controller: 'DetalheMonitorCargaController',
                resolve: {
                    monitorCarga: ['$stateParams', 'ProcessoService', function ($stateParams, service) {                        
                        return service.obterCargaPorLoja($stateParams.cdSistema, $stateParams.idBandeira, $stateParams.cdLoja, $stateParams.data);
                    }]
                }
            });
    }
})();