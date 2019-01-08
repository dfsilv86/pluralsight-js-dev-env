(function () {
    'use strict';

    // TODO: isso não deveria estar aqui. reaproveitar a controller dentro da modal de pesquisa (ou fazer a modal de pesquisa utilizar esta aqui)

    function PesquisaDepartamentoController($scope, $q, $timeout, $stateParams, $state, $validation, pagingService, departamentoService) {

        var ordenacaoPadrao = 'idDepartamento asc';
        var modoPereciveis = '';
        var excluirPadaria = false;
        $scope.modoPereciveis = modoPereciveis;
        $validation.prepare($scope);

        $scope.filters = $stateParams.filters || { cdSistema: null, cdDepartamento: null, cdDivisao: null, dsDepartamento: null, blPerecivel: true };
        
        $scope.data = { values: null };
        $scope.data.paging = $stateParams.paging || { offset: 0, limit: 10, orderBy: ordenacaoPadrao };

        function esconderRegistros() {
            $scope.data.values = [];
        }

        function exibirRegistros(data) {
            $scope.data.values = data;
            $scope.filters.didSearch = true;
            pagingService.acceptPagingResults($scope.data.paging, data);
        }

        function search(pageNumber) {
            if (!$validation.validate($scope)) return;

            pagingService.calculateOffset($scope.data.paging, pageNumber);

            var cdSistema = $scope.filters.cdSistema;
            var cdDepartamento = $scope.filters.cdDepartamento;
            var dsDepartamento = $scope.filters.dsDepartamento;
            var cdDivisao = $scope.filters.cdDivisao;
            var blPerecivel = $scope.filters.blPerecivel || (modoPereciveis == 'restrito');
            
            departamentoService.pesquisarPorDivisaoESistema(cdDepartamento, dsDepartamento, blPerecivel, cdDivisao, cdSistema, excluirPadaria, $scope.data.paging)
                .then(exibirRegistros)
                .catch(esconderRegistros);
        }

        $scope.search = search;

        function clear() {
            $scope.filters.cdSistema = null;
            $scope.filters.cdDepartamento = null;
            $scope.filters.dsDepartamento = null;
            $scope.filters.cdDivisao = null;
            $scope.filters.blPerecivel = true;
            $scope.data.values = [];
        }

        $scope.clear = clear;

        function detail(item) {

            $state.update({ filters: $scope.filters, paging: $scope.data.paging });

            $state.go('cadastroDepartamento', {
                'id': item.id
            });
        }

        $scope.detail = detail;

        function orderBy(field) {
            pagingService.toggleSorting($scope.data.paging, field);
            search();
        }

        $scope.orderBy = orderBy;
        if ($scope.filters.didSearch) {
            search();
        }
    }

    PesquisaDepartamentoController.$inject = ['$scope', '$q', '$timeout', '$stateParams', '$state', 'ValidationService', 'PagingService', 'DepartamentoService'];

    function PesquisaDepartamentoRoute($stateProvider) {

        $stateProvider
            .state('pesquisaDepartamento', {
                url: '/cadastro/departamento',
                params: {
                    filters: null,
                    paging: null
                },
                templateUrl: 'Scripts/app/estruturaMercadologica/pesquisa-departamento.view.html',
                controller: 'PesquisaDepartamentoController'
            });
    }

    PesquisaDepartamentoRoute.$inject = ['$stateProvider'];

    angular
        .module('SGP')
        .config(PesquisaDepartamentoRoute)
        .controller('PesquisaDepartamentoController', PesquisaDepartamentoController);
})();