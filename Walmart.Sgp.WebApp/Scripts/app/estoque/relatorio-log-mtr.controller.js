(function () {
    'use strict';

    // Configuração da controller
    angular
        .module('SGP')
        .config(RelatorioLogMtrRoute)
        .controller('RelatoriologMtrController', RelatoriologMtrController);

    // Implementação da controller
    function RelatoriologMtrController() {

    }

    // Configuração do estado
    RelatorioLogMtrRoute.$inject = ['$stateProvider'];
    function RelatorioLogMtrRoute($stateProvider) {

        $stateProvider
            .state('relatorioLogMtr', {
                url: '/estoque/relatorios/relatorioLogMtr',
                templateUrl: 'Scripts/app/estoque/relatorio-log-mtr.view.html',
                controller: 'RelatoriologMtrController'
            });
    }
})();