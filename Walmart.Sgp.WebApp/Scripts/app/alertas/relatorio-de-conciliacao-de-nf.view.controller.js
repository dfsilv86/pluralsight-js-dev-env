(function () {
    'use strict';

    // Configuração da controller
    angular
        .module('SGP')
        .config(RelatorioDeConciliacaoDeNFRoute)
        .controller('RelatorioDeConciliacaoDeNFController', RelatorioDeConciliacaoDeNFController);

    // Implementação da controller
    function RelatorioDeConciliacaoDeNFController() {

    };

    // Configuração do estado
    RelatorioDeConciliacaoDeNFRoute.$inject = ['$stateProvider'];
    function RelatorioDeConciliacaoDeNFRoute($stateProvider) {

        $stateProvider
            .state('relatorioDeConciliacaoDeNF', {
                url: '/alertas/relatorios/relatorioDeConciliacaoDeNF',
                templateUrl: 'Scripts/app/alertas/relatorio-de-conciliacao-de-nf.view.html',
                controller: 'RelatorioDeConciliacaoDeNFController'
            });
    }
})();