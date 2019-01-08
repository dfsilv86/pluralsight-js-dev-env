(function () {
    'use strict';

    // Configuração da controller
    angular
        .module('SGP')
        .config(RelatorioPedidosGeradosPorReturnSheetRoute)
        .controller('RelatorioPedidosGeradosPorReturnSheetController', RelatorioPedidosGeradosPorReturnSheetController);

    // Implementação da controller
    function RelatorioPedidosGeradosPorReturnSheetController() {

    }

    // Configuração do estado
    RelatorioPedidosGeradosPorReturnSheetRoute.$inject = ['$stateProvider'];
    function RelatorioPedidosGeradosPorReturnSheetRoute($stateProvider) {

        $stateProvider
            .state('relatorioPedidosGeradosPorReturnSheet', {
                url: '/reabastecimento/relatorios/relatorioPedidosGeradosPorReturnSheet',
                templateUrl: 'Scripts/app/reabastecimento/returnSheet/relatorio-pedidos-gerados-por-return-sheet.view.html',
                controller: 'RelatorioPedidosGeradosPorReturnSheetController'
            });
    }
})();