(function () {
    'use strict';

    // Configuração da controller
    angular
        .module('SGP')
        .config(RelatorioDeItemVisivelNaoVisivelParaInterferirRoute)
        .controller('RelatorioDeItemVisivelNaoVisivelParaInterferirController', RelatorioDeItemVisivelNaoVisivelParaInterferirController);

    // Implementação da controller
    function RelatorioDeItemVisivelNaoVisivelParaInterferirController() {

    }

    // Configuração do estado
    RelatorioDeItemVisivelNaoVisivelParaInterferirRoute.$inject = ['$stateProvider'];
    function RelatorioDeItemVisivelNaoVisivelParaInterferirRoute($stateProvider) {

        $stateProvider
            .state('relatorioDeItemVisivelNaoVisivelParaInterferir', {
                url: '/reabastecimento/relatorios/relatorioDeItemVisivelNaoVisivelParaInterferir',
                templateUrl: 'Scripts/app/reabastecimento/relatorio-de-item-visivel-nao-visivel-para-interferir.view.html',
                controller: 'RelatorioDeItemVisivelNaoVisivelParaInterferirController'
            });
    }
})();