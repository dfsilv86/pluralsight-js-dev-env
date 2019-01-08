(function () {
    'use strict';

    angular
          .module('SGP')
          .config(MultiSourcingRoute)
          .controller('MultiSourcingController', MultiSourcingController);


    MultiSourcingController.$inject = ['$scope', '$q', '$state', '$stateParams', 'ItemDetalheService', 'MultisourcingService', 'UserSessionService', 'StackableModalService', 'ToastService', 'ValidationService', 'PagingService'];
    function MultiSourcingController($scope, $q, $state, $stateParams, itemDetalheService, multisourcingService, userSession, $modal, toastService, $validation, pagingService) {

        initialize();

        function initialize() {
            $validation.prepare($scope);

            $scope.filters = $stateParams.filters || { cdSistema: 1, cdDepartamento: null, cdOldNumber: null, idCD: null, filtroMS: 1, filtroCadastro: 2 };
            $scope.data = { values: null };
            $scope.data.paging = $stateParams.paging || { offset: 0, limit: 50, orderBy: 'cdItem' };
            $scope.search = search;
            $scope.clear = clear;
            $scope.orderBy = orderBy;
            $scope.edit = edit;
            $scope.toImport = toImport;
            $scope.toExport = toExport;

            if ($stateParams.paging) {
                search();
            }
        }

        function edit(item) {

            var possivel = item.multivendor,
                cdItemSaida = item.cdItem,
                cdCD = item.cdCD, dsItem =
                item.dsItem;

            if (possivel) {
                $state.update({ filters: $scope.filters, paging: $scope.data.paging });
                $state.go('cadMultiSourcingEdit', {
                    'cdItem': cdItemSaida,
                    'cdCD': cdCD,
                    'cdSistema': $scope.filters.cdSistema,
                    'cdDepartamento': $scope.filters.cdDepartamento
                });

            } else {
                toastService.warning(globalization.texts.noMultisourcingItem);
            }
        }

        function orderBy(field) {
            pagingService.toggleSorting($scope.data.paging, field);
            search();
        }

        function search(pageNumber) {

            if (!$validation.validate($scope)) return;

            pagingService.calculateOffset($scope.data.paging, pageNumber);

            var cdItem = $scope.filters.cdOldNumber || '';
            var cdDepartamento = $scope.filters.cdDepartamento || '';
            var cdSistema = $scope.filters.cdSistema || 1;
            var idCD = $scope.filters.idCD || '';
            var filtroMS = $scope.filters.filtroMS || 1;
            var filtroCadastro = $scope.filters.filtroCadastro || 1;

            $q.when(itemDetalheService.pesquisarItensSaidaComCDConvertido(cdItem, cdDepartamento, cdSistema, idCD, filtroMS, filtroCadastro, $scope.data.paging))
                  .then(exibeConsulta)
                  .catch(escondeConsulta);
        }

        function exibeConsulta(data) {
            $scope.data.values = data;
            pagingService.acceptPagingResults($scope.data.paging, data);

            $scope.filters.didSearch = true;
            $validation.accept($scope);
        }

        function escondeConsulta() {
            $scope.data.values = [];
            $scope.filters.didSearch = false;
        }

        function clear() {
            $scope.filters.departamento = $scope.filters.item = $scope.filters.cdOldNumber = $scope.data.values = $scope.filters.cdDepartamento = null;
            $scope.filters.filtroMS = 1;
            $scope.filters.filtroCadastro = 2;
            $scope.filters.idCD = null;
            $scope.filters.cdSistema = 1;
            $scope.data.paging = { offset: 0, limit: 50, orderBy: 'cdItem' };
            escondeConsulta();
        }

        function toImport() {
            var theModal = $modal.open({
                templateUrl: 'Scripts/app/reabastecimento/multisourcing/importacao-multisourcing.view.html',
                controller: 'ImpMultiSourcingController',
                resolve: {
                    cdSistema: $scope.filters.cdSistema
                }
            });
        }

        function toExport() {
            if (!$validation.validate($scope)) return;

            var idItem = ($scope.filters.item || {}).idItemDetalhe;
            var idDepartamento = ($scope.filters.departamento || {}).idDepartamento;
            var cdSistema = $scope.filters.cdSistema;
            var idCD = $scope.filters.idCD || '';
            var filtroMS = $scope.filters.filtroMS;
            var filtroCadastro = $scope.filters.filtroCadastro || 1;

            $q.when(multisourcingService.exportar(idItem, idDepartamento, cdSistema, idCD, filtroMS, filtroCadastro));
        }
    }

    MultiSourcingRoute.$inject = ['$stateProvider'];
    function MultiSourcingRoute($stateProvider) {

        $stateProvider
                .state('multiSourcing', {
                    url: '/multiSourcing',
                    templateUrl: 'Scripts/app/reabastecimento/multisourcing/pesquisa-multisourcing.view.html',
                    controller: 'MultiSourcingController',
                    params: {
                        filters: null,
                        paging: null
                    },
                });
    }
})();