(function () {
    'use strict';

    // Configuração da controller
    angular
        .module('SGP')
        .config(PesquisaCompraCasadaRoute)
        .controller('PesquisaCompraCasadaController', PesquisaCompraCasadaController);


    // Implementação da controller
    PesquisaCompraCasadaController.$inject = ['$scope', '$q', '$timeout', '$stateParams', '$state', 'UserSessionService', 'ValidationService', 'PagingService', 'ChangeTrackerFactory', 'CompraCasadaService', 'ToastService'];
    function PesquisaCompraCasadaController($scope, $q, $timeout, $stateParams, $state, userSession, $validation, pagingService, changeTrackerFactory, compraCasadaService, toastService) {

        initialize();

        function initialize() {
            $validation.prepare($scope);

            if ($stateParams.filters) {
                $stateParams.filters.vendor = null;
            }

            $scope.filters = $stateParams.filters || {
                cdSistema: 1,
                cdDepartamento: null,
                cdV9D: null,
                cdOldNumber: null,
                filtroCadastro: 2,
                cdTipo: null
            };

            $scope.data = { values: null };
            $scope.data.paging = $stateParams.paging || { offset: 0, limit: 10, orderBy: null };
            $scope.search = search;
            $scope.clear = clear;
            $scope.orderBy = orderBy;
            $scope.edit = edit;
            $scope.toExport = toExport;

            if ($scope.filters.didSearch) {
                search();
            }

            if ($stateParams.msg) {
                toastService.success($stateParams.msg);
            }
        }

        function toExport() {
            if (!$validation.validate($scope)) return;

            var idDepartamento = ($scope.filters.departamento || {}).idDepartamento || '';
            var cdSistema = $scope.filters.cdSistema;
            var idFornecedorParametro = ($scope.filters.vendor || {}).idFornecedorParametro || '';
            var idItemDetalheSaida = ($scope.filters.item || {}).idItemDetalhe || '';
            var blPossuiCadastro = null;
            if ($scope.filters.filtroCadastro == 0) {
                blPossuiCadastro = false;
            } else if ($scope.filters.filtroCadastro == 1) {
                blPossuiCadastro = true;
            }

            $q.when(compraCasadaService.exportar(idDepartamento, cdSistema, idFornecedorParametro, idItemDetalheSaida, blPossuiCadastro));
        }

        function edit(item) {
            if (item.qtItensEntrada < 3) {
                toastService.error(globalization.texts.notEnougthItensToCompraCasada);
            } else {
                $scope.filters.idFP = item.fornecedorParametro.idFornecedorParametro;
                $state.update({ filters: $scope.filters, paging: $scope.data.paging });
                $state.go('cadastroCompraCasada', {
                    'idFornecedorParametro': item.fornecedorParametro.idFornecedorParametro,
                    'idDepartamento': item.departamento.idDepartamento,
                    'cdSistema': item.cdSistema,
                    'cdItem': item.cdItem,
                    'idItemDetalheSaida': item.idItemDetalhe
                });
            }
        }

        function clear() {
            $scope.filters.cdDepartamento = $scope.filters.cdV9D = $scope.filters.cdOldNumber = null;
            $scope.filters.filtroCadastro = 2;
            $scope.filters.cdSistema = 1;
            $scope.data.paging.offset = 0;
            $scope.data.values = [];
            $scope.filters.didSearch = false;
        }

        function exibe(data) {
            $scope.data.values = data;
            pagingService.acceptPagingResults($scope.data.paging, data);

            $scope.filters.didSearch = true;
            $validation.accept($scope);
        }

        function esconde(data) {
            $scope.data.values = [];
            $scope.filters.didSearch = false;
        }

        function search(pageNumber) {
            if (!$validation.validate($scope)) return;

            if ($scope.filters.departamento.cdDepartamento === 0) {
                var msg = '<b>' + globalization.texts.theFollowingErrorsWereFound + '</b>:\n<ul>' +
                    '<li>' + globalization.texts.allMustBeInformedSingular.replace('{0}', '"' + globalization.texts.department + '"') + '</li>';

                toastService.warning(msg);
                return;
            }

            pagingService.calculateOffset($scope.data.paging, pageNumber);

            var idDepartamento = ($scope.filters.departamento || {}).idDepartamento || '';
            var cdSistema = $scope.filters.cdSistema;
            var idFornecedorParametro = (($scope.filters.vendor || {}).idFornecedorParametro || $scope.filters.idFP) || '';
            if (!$scope.filters.cdV9D) {
                idFornecedorParametro = null;
            }
            var idItemDetalheSaida = ($scope.filters.item || {}).idItemDetalhe || '';
            var blPossuiCadastro = null;
            if ($scope.filters.filtroCadastro == 0) {
                blPossuiCadastro = false;
            } else if ($scope.filters.filtroCadastro == 1) {
                blPossuiCadastro = true;
            }

            // Requisição ao serviço
            var deferred = $q
                .when(compraCasadaService.pesquisarItensCompraCasada(idDepartamento, cdSistema, idFornecedorParametro, idItemDetalheSaida, blPossuiCadastro, $scope.data.paging))
                .then(exibe)
                .catch(esconde);
        }

        function orderBy(field) {
            if ($scope.data.values != null && $scope.data.values.length > 0) {
                $scope.data.paging.orderBy = ($scope.data.paging.orderBy || '').indexOf(field + ' asc') >= 0 ?
                    field + ' desc' :
                    field + ' asc';
                search();
            }
        }
    }

    // Configuração do estado
    PesquisaCompraCasadaRoute.$inject = ['$stateProvider'];
    function PesquisaCompraCasadaRoute($stateProvider) {

        $stateProvider
            .state('pesquisaCompraCasada', {
                url: '/reabastecimento/compraCasada',
                params: {
                    filters: null,
                    paging: null,
                    msg: null
                },
                templateUrl: 'Scripts/app/reabastecimento/compraCasada/pesquisa-compra-casada.view.html',
                controller: 'PesquisaCompraCasadaController'
            });
    }
})();