(function () {
    'use strict';

    // Configuração da controller
    angular
        .module('SGP')
        .config(RelatorioFinanceiroPorDepartamentoSinteticoRoute)
        .controller('RelatorioFinanceiroPorDepartamentoSinteticoController', RelatorioFinanceiroPorDepartamentoSinteticoController);

    // Implementação da controller
    function RelatorioFinanceiroPorDepartamentoSinteticoController() {

    }

    // Configuração do estado
    RelatorioFinanceiroPorDepartamentoSinteticoRoute.$inject = ['$stateProvider'];
    function RelatorioFinanceiroPorDepartamentoSinteticoRoute($stateProvider) {

        $stateProvider
            .state('relatorioFinanceiroPorDepartamentoSintetico', {
                url: '/gerenciamento/relatorios/relatorioFinanceiroPorDepartamentoSintetico',
                templateUrl: 'Scripts/app/gerenciamento/relatorios/relatorio-financeiro-por-departamento-sintetico.view.html',
                controller: 'RelatorioFinanceiroPorDepartamentoSinteticoController'
            });
    }
})();