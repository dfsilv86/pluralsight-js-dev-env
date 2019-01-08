(function () {
    'use strict';

    // Configuração da controller
    angular
        .module('SGP')
        .config(RelatorioSugestaoCompraRoute)
        .controller('RelatorioSugestaoCompraController', RelatorioSugestaoCompraController);

    // Implementação da controller
    function RelatorioSugestaoCompraController() {

    }

    // Configuração do estado
    RelatorioSugestaoCompraRoute.$inject = ['$stateProvider'];
    function RelatorioSugestaoCompraRoute($stateProvider) {

        $stateProvider
            .state('relatorioSugestaoCompra', {
                url: '/reabastecimento/relatorios/relatorioSugestaoCompra',
                templateUrl: 'Scripts/app/reabastecimento/relatorio-sugestao-compra.view.html',
                controller: 'RelatorioSugestaoCompraController'
            });
    }
})();