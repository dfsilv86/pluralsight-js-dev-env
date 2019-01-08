(function () {
    'use strict';

    // Configuração da controller
    angular
        .module('SGP')
        .config(PesquisaAlcadaRoute)
        .controller('PesquisaAlcadaController', PesquisaAlcadaController);

    // Implementação da controller
    PesquisaAlcadaController.$inject = ['$scope', '$q', '$timeout', '$stateParams', '$state', 'ValidationService', 'PapelService', 'AlcadaService', 'PagingService'];
    function PesquisaAlcadaController($scope, $q, $timeout, $stateParams, $state, $validation, papelService, alcadaService, pagingService) {

        $validation.prepare($scope);

        $scope.data = { values: null };
        $scope.data.paging = $stateParams.paging || { offset: 0, limit: 10, orderBy: 'Id asc' };

        $scope.search = search;
        $scope.clear = clear;
        $scope.detail = detail;
        $scope.orderBy = orderBy;

        // Como os parâmetros de paging podem ser alterados pelo grid-pager (carregando do storage)
        // deve aguardar o próximo ciclo antes de pesquisar
        // Isso só é necessário aqui porque a página abre com a pesquisa pronta,
        // Não fazer isso em outros lugares.
        // TODO: talvez usando um $apply lá e/ou aqui, possa remover o $timeout, testar
        $timeout(search, 100); // suppress-validator

        function clear() {
            escondePapeis();
        }

        function orderBy(field) {
            pagingService.toggleSorting($scope.data.paging, field);
            search();
        }

        function exibePapeis(data) {

            $scope.data.values = data;

            pagingService.acceptPagingResults($scope.data.paging, data);

            $validation.accept($scope);
        }

        function escondePapeis() {
            $scope.data.values = null;
        }

        function search(pageNumber) {

            if (!$validation.validate($scope)) return;

            pagingService.calculateOffset($scope.data.paging, pageNumber);

            // Requisição ao serviço
            $q.when(papelService.obterTodos($scope.data.paging))
                .then(exibePapeis)
                .catch(escondePapeis);
        }

        function detail(item) {

            $state.update({ paging: $scope.data.paging });

            $state.go('manutencaoAlcada', {
                'id': item.id
            });
        }
    }

    // Configuração do estado
    PesquisaAlcadaRoute.$inject = ['$stateProvider'];
    function PesquisaAlcadaRoute($stateProvider) {

        $stateProvider
            .state('pesquisaAlcada', {
                url: '/reabastecimento/alcada',
                params: {                    
                    paging: null
                },
                templateUrl: 'Scripts/app/gerenciamento/pesquisa-alcada.view.html',
                controller: 'PesquisaAlcadaController'
            });
    }
})();