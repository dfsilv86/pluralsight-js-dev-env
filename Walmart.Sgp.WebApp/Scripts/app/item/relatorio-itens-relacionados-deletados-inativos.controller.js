(function () {
    'use strict';

    // Configuração da controller
    angular
        .module('SGP')
        .config(RelatorioItensRelacionadosDeletadosInativosRoute)
        .controller('RelatorioItensRelacionadosDeletadosInativosController', RelatorioItensRelacionadosDeletadosInativosController);

    // Implementação da controller
    function RelatorioItensRelacionadosDeletadosInativosController() {

    }

    // Configuração do estado
    RelatorioItensRelacionadosDeletadosInativosRoute.$inject = ['$stateProvider'];
    function RelatorioItensRelacionadosDeletadosInativosRoute($stateProvider) {

        $stateProvider
            .state('relatorioItensRelacionadosDeletadosInativos', {
                url: '/item/relatorios/relatorioItensRelacionadosDeletadosInativos',
                templateUrl: 'Scripts/app/item/relatorio-itens-relacionados-deletados-inativos.view.html',
                controller: 'RelatorioItensRelacionadosDeletadosInativosController'
            });
    }
})();