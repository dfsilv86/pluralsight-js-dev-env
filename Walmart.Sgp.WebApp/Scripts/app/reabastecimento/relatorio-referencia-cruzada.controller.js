(function () {
    'use strict';

    // Configuração da controller
    angular
        .module('SGP')
        .config(RelatorioReferenciaCruzadaRoute)
        .controller('RelatorioReferenciaCruzadaController', RelatorioReferenciaCruzadaController);

    // Implementação da controller
    function RelatorioReferenciaCruzadaController() {

    }

    // Configuração do estado
    RelatorioReferenciaCruzadaRoute.$inject = ['$stateProvider'];
    function RelatorioReferenciaCruzadaRoute($stateProvider) {

        $stateProvider
            .state('relatorioReferenciaCruzada', {
                url: '/reabastecimento/relatorios/relatorioReferenciaCruzada',
                templateUrl: 'Scripts/app/reabastecimento/relatorio-referencia-cruzada.view.html',
                controller: 'RelatorioReferenciaCruzadaController'
            });
    }
})();