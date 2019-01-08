(function () {
    'use strict';

    // Configuração da controller
    angular
        .module('SGP')
        .config(RelatorioEstoqueNegativoRoute)
        .controller('RelatorioEstoqueNegativoController', RelatorioEstoqueNegativoController);

    // Implementação da controller
    function RelatorioEstoqueNegativoController() {

    }

    // Configuração do estado
    RelatorioEstoqueNegativoRoute.$inject = ['$stateProvider'];
    function RelatorioEstoqueNegativoRoute($stateProvider) {

        $stateProvider
            .state('relatorioEstoqueNegativo', {
                url: '/estoque/relatorios/relatorioEstoqueNegativo',
                templateUrl: 'Scripts/app/estoque/relatorio-estoque-negativo.view.html',
                controller: 'RelatorioEstoqueNegativoController'
            });
    }
})();