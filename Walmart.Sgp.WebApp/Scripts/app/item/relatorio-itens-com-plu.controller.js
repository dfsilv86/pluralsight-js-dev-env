(function () {
    'use strict';

    // Configuração da controller
    angular
        .module('SGP')
        .config(RelatorioItensComPLURoute)
        .controller('RelatorioItensComPLUController', RelatorioItensComPLUController);

    // Implementação da controller
    function RelatorioItensComPLUController() {

    }

    // Configuração do estado
    RelatorioItensComPLURoute.$inject = ['$stateProvider'];
    function RelatorioItensComPLURoute($stateProvider) {

        $stateProvider
            .state('relatorioItensComPLU', {
                url: '/item/relatorios/relatorioItensComPLU',
                templateUrl: 'Scripts/app/item/relatorio-itens-com-plu.view.html',
                controller: 'RelatorioItensComPLUController'
            });
    }
})();