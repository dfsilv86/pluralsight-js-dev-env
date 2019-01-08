(function () {
    'use strict';

    // Configuração da controller
    angular
        .module('SGP')
        .config(RelatorioGerencialDeAderenciaRoute)
        .controller('RelatorioGerencialDeAderenciaController', RelatorioGerencialDeAderenciaController);

    // Implementação da controller
    function RelatorioGerencialDeAderenciaController() {

    }

    // Configuração do estado
    RelatorioGerencialDeAderenciaRoute.$inject = ['$stateProvider'];
    function RelatorioGerencialDeAderenciaRoute($stateProvider) {

        $stateProvider
            .state('relatorioGerencialDeAderencia', {
                url: '/reabastecimento/relatorios/relatorioGerencialDeAderencia',
                templateUrl: 'Scripts/app/reabastecimento/relatorio-gerencial-de-aderencia.view.html',
                controller: 'RelatorioGerencialDeAderenciaController'
            });
    }
})();