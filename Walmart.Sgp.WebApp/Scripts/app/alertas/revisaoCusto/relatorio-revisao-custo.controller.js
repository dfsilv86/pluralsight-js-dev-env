(function () {
    'use strict';

    // Configuração da controller
    angular
        .module('SGP')
        .config(RelRevisaoCustoRoute)
        .controller('RelRevisaoCustoController', RelRevisaoCustoController);

    // Implementação da controller
    function RelRevisaoCustoController() {

    }

    // Configuração do estado
    RelRevisaoCustoRoute.$inject = ['$stateProvider'];
    function RelRevisaoCustoRoute($stateProvider) {

        $stateProvider
            .state('relatorioRevisaoCusto', {
                url: '/alertas/relatorioRevisaoCusto',
                templateUrl: 'Scripts/app/alertas/revisaoCusto/relatorio-revisao-custo.view.html',
                controller: 'RelRevisaoCustoController'
            });
    }
})();