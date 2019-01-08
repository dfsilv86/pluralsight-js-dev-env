(function () {
    'use strict';

    angular
        .module('SGP')
        .config(PesquisaItemRoute)
        .controller('PesquisaItemController', PesquisaItemController);

    PesquisaItemController.$inject = ['$scope', '$q', '$timeout', '$stateParams', '$state', 'ValidationService', 'PagingService', 'ItemDetalheService']; // Configuração do estado
    function PesquisaItemController($scope, $q, $timeout, $stateParams, $state, $validation, pagingService, itemDetalheService) {

        $validation.prepare($scope);

        $scope.filters = $stateParams.filters || {
            cdSistema: null,
            cdDepartamento: null,
            idBandeira: null,
            cdFineLine: null,
            cdCategoria: null,
            cdSubcategoria: null,
            cdItem: null,
            dsItem: null,
            plu: null,
            tpStatus: 'A',
            fineline: null,
            departamento: null
        };

        $scope.data = { values: null };
        $scope.data.paging = $stateParams.paging || { offset: 0, limit: 10, orderBy: 'cdItem asc' };

        $scope.search = search;
        $scope.clear = clear;
        $scope.orderBy = orderBy;
        $scope.mostrarInformacoesCadastrais = mostrarInformacoesCadastrais;
        $scope.mostrarPrecosCusto = mostrarPrecosCusto;

        if (!!$scope.filters && !!$scope.filters.didSearch) {
            // paging já foi persistido e carregado com a última configuração, não é necessário usar $timeout.
            search();
        }

        function esconderRegistros() {            
            $scope.data.values = [];
            $scope.filters.didSearch = false;
        }

        function exibirRegistros(data) {
            $scope.data.values = data;
            pagingService.acceptPagingResults($scope.data.paging, data);

            $scope.filters.didSearch = true;
            $validation.accept($scope);
        }

        function search(pageNumber) {
            if (!$validation.validate($scope)) return;

            pagingService.calculateOffset($scope.data.paging, pageNumber);
            
            // Prepara os parâmetros de pesquisa      
            // TODO: era melhor usar o cdDepartamento e cdFineline aqui, ajustando a consulta...
            var filtro = {
                cdSistema: $scope.filters.cdSistema,
                idBandeira: $scope.filters.idBandeira,
                cdItem: $scope.filters.cdItem,
                dsItem: $scope.filters.dsItem,
                idFineLine: $scope.filters.fineline ? $scope.filters.fineline.id : null,
                cdPlu: $scope.filters.plu,
                idDepartamento: $scope.filters.departamento ? $scope.filters.departamento.id : null,
                status: $scope.filters.tpStatus
            };

            // Requisição ao serviço
            $q.when(itemDetalheService.consultarPorFiltro(filtro, $scope.data.paging))
                .then(exibirRegistros)
                .catch(esconderRegistros);
        }

        function clear() {
            $scope.filters.idBandeira = null;
            $scope.filters.cdItem = null;
            $scope.filters.dsItem = null;
            $scope.filters.cdFineLine = null;
            $scope.filters.fineline = null;
            $scope.filters.cdCategoria = null;
            $scope.filters.cdSubcategoria = null;
            $scope.filters.plu = null;
            $scope.filters.cdDepartamento = null;
            $scope.filters.departamento = null;
            $scope.filters.tpStatus = 'A';
            esconderRegistros();
        }

        function orderBy(field) {
            pagingService.toggleSorting($scope.data.paging, field);
            search();
        }

        function mostrarInformacoesCadastrais(item) {
            $state.update({ filters: $scope.filters, paging: $scope.data.paging });
            $state.go('manutencaoItem', {
                'cdItem': item.cdItem,
                'idBandeira': item.idBandeira
            });
        }

        function mostrarPrecosCusto(item) {
            $state.update({ filters: $scope.filters, paging: $scope.data.paging });
            $state.go('manutencaoItemPrecosCusto', {
                'cdItem': item.cdItem,
                'idBandeira': item.idBandeira
            });
        }
    }

    PesquisaItemRoute.$inject = ['$stateProvider'];
    function PesquisaItemRoute($stateProvider) {

        $stateProvider
            .state('pesquisaItem', {
                url: '/informacoes-gerenciais/item',
                params: {
                    filters: null,
                    paging: null
                },
                templateUrl: 'Scripts/app/item/pesquisa-item.view.html',
                controller: 'PesquisaItemController'
            });
    }

})();