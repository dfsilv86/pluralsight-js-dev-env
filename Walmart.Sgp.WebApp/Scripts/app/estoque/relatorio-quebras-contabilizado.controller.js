(function () {
    'use strict';

    // Configuração da controller
    angular
        .module('SGP')
        .config(RelatorioQuebrasContabilizadoRoute)
        .controller('RelatorioQuebrasContabilizadoController', RelatorioQuebrasContabilizadoController);

    // Implementação da controller
    function RelatorioQuebrasContabilizadoController() {

    }

    // Configuração do estado
    RelatorioQuebrasContabilizadoRoute.$inject = ['$stateProvider'];
    function RelatorioQuebrasContabilizadoRoute($stateProvider) {

        $stateProvider
            .state('relatorioQuebrasContabilizado', {
                url: '/estoque/relatorios/relatorioQuebrasContabilizado',
                templateUrl: 'Scripts/app/estoque/relatorio-quebras-contabilizado.view.html',
                controller: 'RelatorioQuebrasContabilizadoController'
            });
    }
})();