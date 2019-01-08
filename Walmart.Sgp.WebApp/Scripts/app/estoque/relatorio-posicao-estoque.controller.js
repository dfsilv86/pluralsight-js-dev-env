(function () {
    'use strict';

    // Configuração da controller
    angular
        .module('SGP')
        .config(RelatorioPosicaoEstoqueRoute)
        .controller('RelatorioPosicaoEstoqueController', RelatorioPosicaoEstoqueController);

    // Implementação da controller
    function RelatorioPosicaoEstoqueController() {

    }

    // Configuração do estado
    RelatorioPosicaoEstoqueRoute.$inject = ['$stateProvider'];
    function RelatorioPosicaoEstoqueRoute($stateProvider) {

        $stateProvider
            .state('relatorioPosicaoEstoque', {
                url: '/estoque/relatorios/relatorioPosicaoEstoque',
                templateUrl: 'Scripts/app/estoque/relatorio-posicao-estoque.view.html',
                controller: 'RelatorioPosicaoEstoqueController'
            });
    }
})();