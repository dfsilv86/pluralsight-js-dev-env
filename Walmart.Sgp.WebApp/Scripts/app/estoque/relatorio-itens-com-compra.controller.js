(function () {
    'use strict';

    // Configuração da controller
    angular
        .module('SGP')
        .config(RelatorioItensComCompraRoute)
        .controller('RelatorioItensComCompraController', RelatorioItensComCompraController);

    // Implementação da controller
    function RelatorioItensComCompraController() {

    }

    // Configuração do estado
    RelatorioItensComCompraRoute.$inject = ['$stateProvider'];
    function RelatorioItensComCompraRoute($stateProvider) {

        $stateProvider
            .state('relatorioItensComCompra', {
                url: '/estoque/relatorios/relatorioItensComCompra',
                templateUrl: 'Scripts/app/estoque/relatorio-itens-com-compra.view.html',
                controller: 'RelatorioItensComCompraController'
            });
    }
})();