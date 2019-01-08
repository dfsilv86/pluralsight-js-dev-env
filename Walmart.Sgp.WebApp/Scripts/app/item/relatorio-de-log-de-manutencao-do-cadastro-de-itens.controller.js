(function () {
    'use strict';

    // Configuração da controller
    angular
        .module('SGP')
        .config(RelatorioDeLogDeManutencaoDoCadastroDeItensRoute)
        .controller('RelatorioDeLogDeManutencaoDoCadastroDeItensController', RelatorioDeLogDeManutencaoDoCadastroDeItensController);

    // Implementação da controller
    function RelatorioDeLogDeManutencaoDoCadastroDeItensController() {

    }

    // Configuração do estado
    RelatorioDeLogDeManutencaoDoCadastroDeItensRoute.$inject = ['$stateProvider'];
    function RelatorioDeLogDeManutencaoDoCadastroDeItensRoute($stateProvider) {

        $stateProvider
            .state('relatorioDeLogDeManutencaoDoCadastroDeItens', {
                url: '/item/relatorios/relatorioDeLogDeManutencaoDoCadastroDeItens',
                templateUrl: 'Scripts/app/item/relatorio-de-log-de-manutencao-do-cadastro-de-itens.view.html',
                controller: 'RelatorioDeLogDeManutencaoDoCadastroDeItensController'
            });
    }
})();