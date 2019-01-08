(function () {
    'use strict';

    // Configuração da controller
    angular
        .module('SGP')
        .config(RelatorioValorizacaoDesvalorizacaoEstoqueRoute)
        .controller('RelatorioValorizacaoDesvalorizacaoEstoqueController', RelatorioValorizacaoDesvalorizacaoEstoqueController);

    // Implementação da controller
    function RelatorioValorizacaoDesvalorizacaoEstoqueController() {

    }

    // Configuração do estado
    RelatorioValorizacaoDesvalorizacaoEstoqueRoute.$inject = ['$stateProvider'];
    function RelatorioValorizacaoDesvalorizacaoEstoqueRoute($stateProvider) {

        $stateProvider
            .state('relatorioValorizacaoDesvalorizacaoEstoque', {
                url: '/estoque/relatorios/relatorioValorizacaoDesvalorizacaoEstoque',
                templateUrl: 'Scripts/app/estoque/relatorio-valorizacao-desvalorizacao-estoque.view.html',
                controller: 'RelatorioValorizacaoDesvalorizacaoEstoqueController'
            });
    }
})();