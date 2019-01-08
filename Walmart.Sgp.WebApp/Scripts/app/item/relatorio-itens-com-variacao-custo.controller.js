(function () {
    'use strict';

    // Configuração da controller
    angular
        .module('SGP')
        .config(RelatorioItensComVariacaoCustoRoute)
        .controller('RelatorioItensComVariacaoCustoController', RelatorioItensComVariacaoCustoController);

    // Implementação da controller
    function RelatorioItensComVariacaoCustoController() {

    }

    // Configuração do estado
    RelatorioItensComVariacaoCustoRoute.$inject = ['$stateProvider'];
    function RelatorioItensComVariacaoCustoRoute($stateProvider) {

        $stateProvider
            .state('relatorioItensComVariacaoCusto', {
                url: '/item/relatorios/relatorioItensComVariacaoCusto',
                templateUrl: 'Scripts/app/item/relatorio-itens-com-variacao-custo.view.html',
                controller: 'RelatorioItensComVariacaoCustoController'
            });
    }
})();