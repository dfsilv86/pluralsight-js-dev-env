(function () {
    'use strict';

    // Configuração da controller
    angular
        .module('SGP')
        .config(RelatorioLojasSemMtrRoute)
        .controller('RelatoriolojasSemMtrController', RelatoriolojasSemMtrController);

    // Implementação da controller
    function RelatoriolojasSemMtrController() {

    }

    // Configuração do estado
    RelatorioLojasSemMtrRoute.$inject = ['$stateProvider'];
    function RelatorioLojasSemMtrRoute($stateProvider) {

        $stateProvider
            .state('relatorioLojasSemMtr', {
                url: '/estoque/relatorios/relatorioLojasSemMtr',
                templateUrl: 'Scripts/app/estoque/relatorio-lojas-sem-mtr.view.html',
                controller: 'RelatoriolojasSemMtrController'
            });
    }
})();