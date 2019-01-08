(function () {
    'use strict';

    // Configuração da controller
    angular
        .module('SGP')
        .config(RelatorioConsistenciaVendor9DigitosRoute)
        .controller('RelatorioConsistenciaVendor9DigitosController', RelatorioConsistenciaVendor9DigitosController);

    // Implementação da controller
    function RelatorioConsistenciaVendor9DigitosController() {

    }

    // Configuração do estado
    RelatorioConsistenciaVendor9DigitosRoute.$inject = ['$stateProvider'];
    function RelatorioConsistenciaVendor9DigitosRoute($stateProvider) {

        $stateProvider
            .state('relatorioConsistenciaVendor9Digitos', {
                url: '/reabastecimento/relatorios/relatorioConsistenciaVendor9Digitos',
                templateUrl: 'Scripts/app/reabastecimento/relatorio-consistencia-vendor-9-digitos.view.html',
                controller: 'RelatorioConsistenciaVendor9DigitosController'
            });
    }
})();