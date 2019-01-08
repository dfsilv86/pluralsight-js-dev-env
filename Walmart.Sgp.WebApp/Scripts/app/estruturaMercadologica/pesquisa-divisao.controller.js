(function () {
    'use strict';

    // TODO: isso não deveria estar aqui. reaproveitar a controller dentro da modal de pesquisa (ou fazer a modal de pesquisa utilizar esta aqui)

    function PesquisaDivisaoController($scope, $q, $timeout, $stateParams, $state, $validation, pagingService, divisaoService) {

        var ordenacaoPadrao = 'idDivisao asc';

        $validation.prepare($scope);

        $scope.filters = $stateParams.filters || {
            cdSistema: null,
            cdDivisao: null,
            dsDivisao: null
        };

        $scope.data = { values: null };
        $scope.data.paging = $stateParams.paging || { offset: 0, limit: 10, orderBy: ordenacaoPadrao };

        function esconderRegistros() {
            $scope.data.values = [];
        }

        function exibirRegistros(data) {
            $scope.data.values = data;
            pagingService.acceptPagingResults($scope.data.paging, data);
        }

        function search(pageNumber) {
            if (!$validation.validate($scope)) return;

            pagingService.calculateOffset($scope.data.paging, pageNumber);

            // Prepara os parâmetros de pesquisa      
            var filtro = {
                cdSistema: $scope.filters.cdSistema,
                cdDivisao: $scope.filters.cdDivisao,
                dsDivisao: $scope.filters.dsDivisao                
            };

            // Requisição ao serviço
            divisaoService.pesquisarPorDivisaoESistema(filtro.cdDivisao, filtro.dsDivisao, filtro.cdSistema, $scope.data.paging)
                .then(exibirRegistros)
                .catch(esconderRegistros);
        }

        $scope.search = search;

        function clear() {
            $scope.filters.cdSistema = null;
            $scope.filters.cdDivisao = null;
            $scope.filters.dsDivisao = null;            
            esconderRegistros();
        }

        $scope.clear = clear;

        function orderBy(field) {
            pagingService.toggleSorting($scope.data.paging, field);
            search();
        }

        $scope.orderBy = orderBy;
        if ($stateParams.paging) {
            search();
        }
    }
    
    PesquisaDivisaoController.$inject = ['$scope', '$q', '$timeout', '$stateParams', '$state', 'ValidationService', 'PagingService', 'DivisaoService'];

    function PesquisaDivisaoRoute($stateProvider) {

        $stateProvider
            .state('pesquisaDivisao', {
                url: '/cadastro/divisao',
                params: {
                    filters: null,
                    paging: null
                },
                templateUrl: 'Scripts/app/estruturaMercadologica/pesquisa-divisao.view.html',
                controller: 'PesquisaDivisaoController'
            });
    }

    PesquisaDivisaoRoute.$inject = ['$stateProvider'];

    angular
        .module('SGP')
        .config(PesquisaDivisaoRoute)
        .controller('PesquisaDivisaoController', PesquisaDivisaoController);    
})();