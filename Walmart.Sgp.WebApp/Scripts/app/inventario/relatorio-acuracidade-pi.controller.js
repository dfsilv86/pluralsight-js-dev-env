(function () {
    'use strict';

    // Configuração da controller
    angular
        .module('SGP')
        .config(RelatorioAcuracidadePIRoute)
        .controller('RelatorioAcuracidadePIController', RelatorioAcuracidadePIController);

    // Implementação da controller
    function RelatorioAcuracidadePIController() {

    }

    // Configuração do estado
    RelatorioAcuracidadePIRoute.$inject = ['$stateProvider'];
    function RelatorioAcuracidadePIRoute($stateProvider) {

        $stateProvider
            .state('relatorioAcuracidadePI', {
                url: '/inventario/relatorios/relatorioAcuracidadePI',
                templateUrl: 'Scripts/app/inventario/relatorio-acuracidade-pi.view.html',
                controller: 'RelatorioAcuracidadePIController'
            });
    }
})();