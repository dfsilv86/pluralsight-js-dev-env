(function () {
    'use strict';

    // Configuração da controller
    angular
        .module('SGP')
        .config(RelatorioQuebrasRoute)
        .controller('RelatorioquebrasController', RelatorioquebrasController);

    // Implementação da controller
    function RelatorioquebrasController() {

    }

    // Configuração do estado
    RelatorioQuebrasRoute.$inject = ['$stateProvider'];
    function RelatorioQuebrasRoute($stateProvider) {

        $stateProvider
            .state('relatorioQuebras', {
                url: '/gerenciamento/relatorios/relatorioQuebras',
                templateUrl: 'Scripts/app/gerenciamento/relatorios/relatorio-quebras.view.html',
                controller: 'RelatorioquebrasController'
            });
    }
})();