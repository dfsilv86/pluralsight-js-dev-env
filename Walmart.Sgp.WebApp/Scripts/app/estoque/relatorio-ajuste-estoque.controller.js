(function () {
    'use strict';

    // Configuração da controller
    angular
        .module('SGP')
        .config(RelatorioAjusteEstoqueRoute)
        .controller('RelatorioAjusteEstoqueController', RelatorioAjusteEstoqueController);

    // Implementação da controller
    function RelatorioAjusteEstoqueController() {

    }

    // Configuração do estado
    RelatorioAjusteEstoqueRoute.$inject = ['$stateProvider'];
    function RelatorioAjusteEstoqueRoute($stateProvider) {

        $stateProvider
            .state('relatorioAjusteEstoque', {
                url: '/estoque/relatorios/relatorioAjusteEstoque',
                templateUrl: 'Scripts/app/estoque/relatorio-ajuste-estoque.view.html',
                controller: 'RelatorioAjusteEstoqueController'
            });
    }
})();