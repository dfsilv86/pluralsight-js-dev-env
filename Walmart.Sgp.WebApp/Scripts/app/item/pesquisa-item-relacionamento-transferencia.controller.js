(function () {
    'use strict';

    // Configuração da controller
    angular
        .module('SGP')
        .config(PesquisaRelacionamentoTransferenciaRoute)
        .controller('PesquisaRelacionamentoTransferenciaController', PesquisaRelacionamentoTransferenciaController);

    // Implementação da controller
    PesquisaRelacionamentoTransferenciaController.$inject = ['$scope', '$q', '$stateParams', '$state', 'ValidationService', 'RelacionamentoTransferenciaService', 'PagingService'];
    function PesquisaRelacionamentoTransferenciaController($scope, $q, $stateParams, $state, $validation, relacionamentoTransferenciaService, pagingService) {

        var ordenacaoPadrao = 'cdItemDestino desc';

        $validation.prepare($scope);

        $scope.filters = $stateParams.filters || {
            cdSistema: null,
            cdLoja: null,
            idBandeira: null,
            cdDepartamento: null,
            cdItem: null,
            dsItem: null
        };

        $scope.data = { values: null };
        $scope.data.paging = $stateParams.paging || { offset: 0, limit: 10, orderBy: ordenacaoPadrao };

        if (!!$scope.filters && !!$scope.filters.didSearch) {
            // paging já foi persistido e carregado com a última configuração, não é necessário usar $timeout.
            search();
        }

        function esconderRegistros() {
            $scope.data.values = null;
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
            var filtro = {
                cdSistema: $scope.filters.cdSistema,
                idLoja: $scope.filters.loja != null ? $scope.filters.loja.idLoja : null,
                idBandeira: $scope.filters.idBandeira,
                idDepartamento: $scope.filters.departamento != null ? $scope.filters.departamento.idDepartamento : null,
                cdItem: $scope.filters.cdItem,
                dsItem: $scope.filters.dsItem
            };

            // Requisição ao serviço
            $q.when(relacionamentoTransferenciaService.pesquisarPorFiltro(filtro, $scope.data.paging))
                .then(exibirRegistros)
                .catch(esconderRegistros);
        }

        $scope.search = search;

        function clear() {
            $scope.filters.cdSistema = null;
            $scope.filters.cdLoja = null;
            $scope.filters.idBandeira = null;
            $scope.filters.cdDepartamento = null;
            $scope.filters.cdItem = null;
            $scope.filters.dsItem = null;
            esconderRegistros();
        }

        $scope.clear = clear;

        function select(item) {
            $state.update({
                filters: $scope.filters,
                paging: $scope.data.paging
            });
            $state.go('cadastroRelacionamentoTransferencia', {
                'idRelacionamento': item.idRelacionamento,
                'cdItemDestino': item.cdItemDestino,
                'cdItemOrigem': item.cdItemOrigem,
                'cdSistema': $scope.filters.cdSistema,
                'idBandeira': $scope.filters.idBandeira
            });
        }

        $scope.select = select;
    }

    // Configuração do estado
    PesquisaRelacionamentoTransferenciaRoute.$inject = ['$stateProvider'];
    function PesquisaRelacionamentoTransferenciaRoute($stateProvider) {

        $stateProvider
            .state('pesquisaRelacionamentoTransferencia', {
                url: '/item/relacionamento-mtr',
                params: {
                    filters: null,
                    paging: null
                },
                templateUrl: 'Scripts/app/item/pesquisa-item-relacionamento-transferencia.view.html',
                controller: 'PesquisaRelacionamentoTransferenciaController'
            });
    }
})();