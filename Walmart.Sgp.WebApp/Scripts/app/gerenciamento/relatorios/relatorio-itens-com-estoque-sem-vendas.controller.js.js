(function () {
    'use strict';

    // Configuração da controller
    angular
        .module('SGP')
        .config(RelatorioItensComEstoqueSemVendasRoute)
        .controller('RelatorioItensComEstoqueSemVendasController', RelatorioItensComEstoqueSemVendasController);

    // Implementação da controller
    function RelatorioItensComEstoqueSemVendasController() {

    }

    // Configuração do estado
    RelatorioItensComEstoqueSemVendasRoute.$inject = ['$stateProvider'];
    function RelatorioItensComEstoqueSemVendasRoute($stateProvider) {

        $stateProvider
            .state('relatorioItensComEstoqueSemVendas', {
                url: '/gerenciamento/relatorios/relatorioItensComEstoqueSemVendas',
                templateUrl: 'Scripts/app/gerenciamento/relatorios/relatorio-itens-com-estoque-sem-vendas.view.html',
                controller: 'RelatorioItensComEstoqueSemVendasController'
            });
    }
})();