(function () {
    'use strict';

    // Configuração da controller
    angular
        .module('SGP')
        .config(RelatorioPrevisaoDemandasRoute)
        .controller('RelatorioPrevisaoDemandasController', RelatorioPrevisaoDemandasController);

    // Implementação da controller
    function RelatorioPrevisaoDemandasController() {

    }

    // Configuração do estado
    RelatorioPrevisaoDemandasRoute.$inject = ['$stateProvider'];
    function RelatorioPrevisaoDemandasRoute($stateProvider) {

        $stateProvider
            .state('relatorioPrevisaoDemandas', {
                url: '/reabastecimento/relatorios/relatorioPrevisaoDemandas',
                templateUrl: 'Scripts/app/reabastecimento/relatorio-previsao-demandas.view.html',
                controller: 'RelatorioPrevisaoDemandasController'
            });
    }
})();