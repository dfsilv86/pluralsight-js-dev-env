(function () {
    'use strict';

    // Configuração da controller
    angular
        .module('SGP')
        .config(RelatorioItensExcluidosDoHostRoute)
        .controller('RelatorioItensExcluidosDoHostController', RelatorioItensExcluidosDoHostController);

    // Implementação da controller
    function RelatorioItensExcluidosDoHostController() {

    }

    // Configuração do estado
    RelatorioItensExcluidosDoHostRoute.$inject = ['$stateProvider'];
    function RelatorioItensExcluidosDoHostRoute($stateProvider) {

        $stateProvider
            .state('relatorioItensExcluidosDoHost', {
                url: '/item/relatorios/relatorioItensExcluidosDoHost',
                templateUrl: 'Scripts/app/item/relatorio-itens-excluidos-do-host.view.html',
                controller: 'RelatorioItensExcluidosDoHostController'
            });
    }
})();