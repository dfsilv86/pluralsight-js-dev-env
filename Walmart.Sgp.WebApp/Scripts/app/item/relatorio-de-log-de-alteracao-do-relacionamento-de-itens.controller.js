(function () {
    'use strict';

    // Configuração da controller
    angular
        .module('SGP')
        .config(RelatorioDeLogDeAlteracaoDoRelacionamentoDeItensRoute)
        .controller('RelatorioDeLogDeAlteracaoDoRelacionamentoDeItensController', RelatorioDeLogDeAlteracaoDoRelacionamentoDeItensController);

    // Implementação da controller
    function RelatorioDeLogDeAlteracaoDoRelacionamentoDeItensController() {

    }

    // Configuração do estado
    RelatorioDeLogDeAlteracaoDoRelacionamentoDeItensRoute.$inject = ['$stateProvider'];
    function RelatorioDeLogDeAlteracaoDoRelacionamentoDeItensRoute($stateProvider) {

        $stateProvider
            .state('relatorioDeLogDeAlteracaoDoRelacionamentoDeItens', {
                url: '/item/relatorios/relatorioDeLogDeAlteracaoDoRelacionamentoDeItens',
                templateUrl: 'Scripts/app/item/relatorio-de-log-de-alteracao-do-relacionamento-de-itens.view.html',
                controller: 'RelatorioDeLogDeAlteracaoDoRelacionamentoDeItensController'
            });
    }
})();