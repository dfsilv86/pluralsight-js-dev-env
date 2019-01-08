(function () {
    'use strict';

    // Configuração da controller
    angular
        .module('SGP')
        .config(RelatorioFinanceiroPorItemRoute)
        .controller('RelatorioFinanceiroPorItemController', RelatorioFinanceiroPorItemController);

    // Implementação da controller
    function RelatorioFinanceiroPorItemController() {

    }

    // Configuração do estado
    RelatorioFinanceiroPorItemRoute.$inject = ['$stateProvider'];
    function RelatorioFinanceiroPorItemRoute($stateProvider) {

        $stateProvider
            .state('relatorioFinanceiroPorItem', {
                url: '/gerenciamento/relatorios/financeiroPorItem',
                templateUrl: 'Scripts/app/gerenciamento/relatorios/relatorio-financeiro-por-item.view.html',
                controller: 'RelatorioFinanceiroPorItemController'
            });
    }
})();