(function () {
    'use strict';

    // Configuração da controller
    angular
        .module('SGP')
        .config(RelatorioEstoqueContabilizadoRoute)
        .controller('RelatorioEstoqueContabilizadoController', RelatorioEstoqueContabilizadoController);

    // Implementação da controller
    function RelatorioEstoqueContabilizadoController() {

    }

    // Configuração do estado
    RelatorioEstoqueContabilizadoRoute.$inject = ['$stateProvider'];
    function RelatorioEstoqueContabilizadoRoute($stateProvider) {

        $stateProvider
            .state('relatorioEstoqueContabilizado', {
                url: '/estoque/relatorios/relatorioEstoqueContabilizado',
                templateUrl: 'Scripts/app/estoque/relatorio-estoque-contabilizado.view.html',
                controller: 'RelatorioEstoqueContabilizadoController'
            });
    }
})();