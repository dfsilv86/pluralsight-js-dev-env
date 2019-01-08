(function () {
    'use strict';

    // Configuração da controller
    angular
        .module('SGP')
        .config(RelatorioItensPluSemRelacionamentoRoute)
        .controller('RelatorioItensPluSemRelacionamentoController', RelatorioItensPluSemRelacionamentoController);

    // Implementação da controller
    function RelatorioItensPluSemRelacionamentoController() {

    }

    // Configuração do estado
    RelatorioItensPluSemRelacionamentoRoute.$inject = ['$stateProvider'];
    function RelatorioItensPluSemRelacionamentoRoute($stateProvider) {

        $stateProvider
            .state('relatorioItensPluSemRelacionamento', {
                url: '/item/relatorios/relatorioItensPluSemRelacionamento',
                templateUrl: 'Scripts/app/item/relatorio-itens-com-plu-sem-relacionamento.view.html',
                controller: 'RelatorioItensPluSemRelacionamentoController'
            });
    }
})();