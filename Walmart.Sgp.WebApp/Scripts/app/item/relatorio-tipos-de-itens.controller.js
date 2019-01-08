(function () {
    'use strict';

    // Configuração da controller
    angular
        .module('SGP')
        .config(RelatorioTiposItensRoute)
        .controller('RelatorioTiposItensController', RelatorioTiposItensController);

    // Implementação da controller
    function RelatorioTiposItensController() {

    }

    // Configuração do estado
    RelatorioTiposItensRoute.$inject = ['$stateProvider'];
    function RelatorioTiposItensRoute($stateProvider) {

        $stateProvider
            .state('relatorioTiposItens', {
                url: '/item/relatorios/relatorioTiposItens',
                templateUrl: 'Scripts/app/item/relatorio-tipos-de-itens.view.html',
                controller: 'RelatorioTiposItensController'
            });
    }
})();