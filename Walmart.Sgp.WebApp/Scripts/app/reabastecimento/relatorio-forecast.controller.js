(function () {
    'use strict';

    // Configuração da controller
    angular
        .module('SGP')
        .config(RelatorioForecastRoute)
        .controller('RelatorioForecastController', RelatorioForecastController);

    // Implementação da controller
    function RelatorioForecastController() {

    }

    // Configuração do estado
    RelatorioForecastRoute.$inject = ['$stateProvider'];
    function RelatorioForecastRoute($stateProvider) {

        $stateProvider
            .state('relatorioForecast', {
                url: '/reabastecimento/relatorios/relatorioForecast',
                templateUrl: 'Scripts/app/reabastecimento/relatorio-forecast.view.html',
                controller: 'RelatorioForecastController'
            });
    }
})();