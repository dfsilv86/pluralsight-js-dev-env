(function () {
    'use strict';

    // Configuração da controller
    angular
        .module('SGP')
        .config(RelatorioItensCustoZeroRoute)
        .controller('RelatorioItensCustoZeroController', RelatorioItensCustoZeroController);

    // Implementação da controller
    function RelatorioItensCustoZeroController() {

    }

    // Configuração do estado
    RelatorioItensCustoZeroRoute.$inject = ['$stateProvider'];
    function RelatorioItensCustoZeroRoute($stateProvider) {

        $stateProvider
            .state('relatorioItensComCustoZero', {
                url: '/item/relatorios/relatorioItensComCustoZero',
                templateUrl: 'Scripts/app/item/relatorio-itens-custo-zero.view.html',
                controller: 'RelatorioItensCustoZeroController'
            });
    }
})();