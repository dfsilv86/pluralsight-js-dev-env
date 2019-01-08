(function () {
    'use strict';

    // Configuração da controller
    angular
        .module('SGP')
        .config(RelatorioEstoqueInventariadoRoute)
        .controller('RelatorioEstoqueInventariadoController', RelatorioEstoqueInventariadoController);

    // Implementação da controller
    function RelatorioEstoqueInventariadoController() {

    }

    // Configuração do estado
    RelatorioEstoqueInventariadoRoute.$inject = ['$stateProvider'];
    function RelatorioEstoqueInventariadoRoute($stateProvider) {

        $stateProvider
            .state('relatorioEstoqueInventariado', {
                url: '/estoque/relatorios/relatorioEstoqueInventariado',
                templateUrl: 'Scripts/app/estoque/relatorio-estoque-inventariado.view.html',
                controller: 'RelatorioEstoqueInventariadoController'
            });
    }
})();