(function () {
    'use strict';

    // Configuração da controller
    angular
        .module('SGP')
        .config(RelatorioFinanceiroPorDepartamentoAnaliticoRoute)
        .controller('RelatorioFinanceiroPorDepartamentoAnaliticoController', RelatorioFinanceiroPorDepartamentoAnaliticoController);

    // Implementação da controller
    function RelatorioFinanceiroPorDepartamentoAnaliticoController() {

    }

    // Configuração do estado
    RelatorioFinanceiroPorDepartamentoAnaliticoRoute.$inject = ['$stateProvider'];
    function RelatorioFinanceiroPorDepartamentoAnaliticoRoute($stateProvider) {

        $stateProvider
            .state('relatorioFinanceiroPorDepartamentoAnalitico', {
                url: '/gerenciamento/relatorios/relatorioFinanceiroPorDepartamentoAnalitico',
                templateUrl: 'Scripts/app/gerenciamento/relatorios/relatorio-financeiro-por-departamento-analitico.view.html',
                controller: 'RelatorioFinanceiroPorDepartamentoAnaliticoController'
            });
    }
})();