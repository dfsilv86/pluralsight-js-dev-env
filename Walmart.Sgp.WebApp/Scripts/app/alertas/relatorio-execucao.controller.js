(function () {
    'use strict';

    // Configuração da controller
    angular
        .module('SGP')
        .config(RelatorioExecucaoRoute)
        .controller('RelatorioExecucaoController', RelatorioExecucaoController);

    // Implementação da controller
    function RelatorioExecucaoController() {

    }

    // Configuração do estado
    RelatorioExecucaoRoute.$inject = ['$stateProvider'];
    function RelatorioExecucaoRoute($stateProvider) {

        $stateProvider
            .state('relatorioExecucao', {
                url: '/alertas/relatorios/relatorioExecucao',
                templateUrl: 'Scripts/app/alertas/relatorio-execucao.view.html',
                controller: 'RelatorioExecucaoController'
            });
    }
})();