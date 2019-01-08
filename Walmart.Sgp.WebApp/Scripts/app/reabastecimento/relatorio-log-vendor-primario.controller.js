(function () {
    'use strict';

    // Configuração da controller
    angular
        .module('SGP')
        .config(RelatorioLogVendorPrimarioRoute)
        .controller('RelatorioLogVendorPrimarioController', RelatorioLogVendorPrimarioController);

    // Implementação da controller
    function RelatorioLogVendorPrimarioController() {

    }

    // Configuração do estado
    RelatorioLogVendorPrimarioRoute.$inject = ['$stateProvider'];
    function RelatorioLogVendorPrimarioRoute($stateProvider) {

        $stateProvider
            .state('relatorioLogVendorPrimario', {
                url: '/reabastecimento/relatorios/relatorioLogVendorPrimario',
                templateUrl: 'Scripts/app/reabastecimento/relatorio-log-vendor-primario.view.html',
                controller: 'RelatorioLogVendorPrimarioController'
            });
    }
})();