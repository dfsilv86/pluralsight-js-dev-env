(function () {
    'use strict';

    // Configuração da controller
    angular
        .module('SGP')
        .config(RelatorioLogItensInventarioRoute)
        .controller('RelatorioLogItensInventarioController', RelatorioLogItensInventarioController);

    // Implementação da controller
    function RelatorioLogItensInventarioController() {

    }

    // Configuração do estado
    RelatorioLogItensInventarioRoute.$inject = ['$stateProvider'];
    function RelatorioLogItensInventarioRoute($stateProvider) {

        $stateProvider
            .state('relatorioLogItensInventario', {
                url: '/inventario/relatorios/relatorioLogItensInventario',
                templateUrl: 'Scripts/app/inventario/relatorio-log-itens-inventario.view.html',
                controller: 'RelatorioLogItensInventarioController'
            });
    }
})();