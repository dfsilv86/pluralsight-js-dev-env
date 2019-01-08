(function () {
    'use strict';

    // Configuração da controller
    angular
        .module('SGP')
        .config(RelatorioExtratoMtrRoute)
        .controller('RelatorioExtratoMtrController', RelatorioExtratoMtrController);

    // Implementação da controller
    function RelatorioExtratoMtrController() {

    }

    // Configuração do estado
    RelatorioExtratoMtrRoute.$inject = ['$stateProvider'];
    function RelatorioExtratoMtrRoute($stateProvider) {

        $stateProvider
            .state('relatorioExtratoMtr', {
                url: '/estoque/relatorios/relatorioExtratoMtr',
                templateUrl: 'Scripts/app/estoque/relatorio-extrato-mtr.view.html',
                controller: 'RelatorioExtratoMtrController'
            });
    }
})();