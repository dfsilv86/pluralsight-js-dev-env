(function () {
    'use strict';

    // Configuração da controller
    angular
        .module('SGP')
        .config(RelatorioItensDeCompraSemRelacionamentoRoute)
        .controller('RelatorioItensDeCompraSemRelacionamentController', RelatorioItensDeCompraSemRelacionamentController);

    // Implementação da controller
    function RelatorioItensDeCompraSemRelacionamentController() {

    }

    // Configuração do estado
    RelatorioItensDeCompraSemRelacionamentoRoute.$inject = ['$stateProvider'];
    function RelatorioItensDeCompraSemRelacionamentoRoute($stateProvider) {

        $stateProvider
            .state('relatorioItensDeCompraSemRelacionamento', {
                url: '/item/relatorios/relatorioItensDeCompraSemRelacionamento',
                templateUrl: 'Scripts/app/item/relatorio-itens-de-compra-sem-relacionamento.view.html',
                controller: 'RelatorioItensDeCompraSemRelacionamentController'
            });
    }
})();