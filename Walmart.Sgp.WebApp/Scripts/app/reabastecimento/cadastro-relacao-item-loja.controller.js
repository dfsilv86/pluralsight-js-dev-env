(function () {
    'use strict';

    // Configuração da controller
    angular
        .module('SGP')
        .config(CadastroRelacaoItemLojaRoute)
        .controller('CadastroRelacaoItemLojaController', CadastroRelacaoItemLojaController);

    // Implementação da controller
    CadastroRelacaoItemLojaController.$inject = ['$scope', '$filter', '$q', '$timeout', '$stateParams', '$state', 'ToastService', 'ValidationService', 'PagingService', 'ChangeTrackerFactory', 'RelacaoItemLojaCDService'];
    function CadastroRelacaoItemLojaController($scope, $filter, $q, $timeout, $stateParams, $state, toastService, $validation, pagingService, changeTrackerFactory, relacaoItemLojaCDService) {

        $validation.prepare($scope);

        $scope.filters = $stateParams.filters || { cdSistema: null, cdV9D: null, cdTipo: null, cdItemSaida: null, idRegiaoCompra: null, dsEstado: null, idBandeira: null, blVinculado: false, vendor: null };
        $scope.data = { values: null };
        $scope.data.paging = $stateParams.paging || { offset: 0, limit: 10, orderBy: 'IdItemEntrada DESC' };
        $scope.search = search;
        $scope.save = save;
        $scope.clear = clear;
        $scope.orderBy = orderBy;
        $scope.discardChanges = discardChanges;
        $scope.toExport = toExport;
        $scope.exportWithoutChanges = exportWithoutChanges;
        $scope.hasChanges = hasChanges;
        $scope.vincular = vincular;
        $scope.desvincular = desvincular;
        $scope.change = change;
        $scope.$watchGroup(['filters.cdItemSaida'], refresh);

        $scope.sortFornecedor = function sortFornecedor(item) {
            return item.fornecedor.nmFornecedor;
        };

        var changeTracker = changeTrackerFactory.createChangeTrackerForProperties(
            ['relacaoItemLojaCD.idItemEntrada'],
            function (left, right) {
                return !!left && !!right && left.relacaoItemLojaCD.idRelacaoItemLojaCD == right.relacaoItemLojaCD.idRelacaoItemLojaCD;
            });

        if ($scope.filters.didSearch) {
            search();
        }

        function desvincular() {
            $state.update({ filters: $scope.filters, paging: $scope.data.paging });

            $state.go('impDesvincularItemLojaCD', {
                cdSistema: $scope.filters.cdSistema
            });
        }

        function vincular() {
            $state.update({ filters: $scope.filters, paging: $scope.data.paging });

            $state.go('impVincularItemLojaCD', {
                cdSistema: $scope.filters.cdSistema
            });
        }

        function refresh(newValues, oldValues) {

            var didReset = !!newValues && !!oldValues ? newValues != oldValues : true;

            if (didReset) {
                esconde();
            }
        }

        function change(item) {
            var it = $filter('filter')(item.itensDisponiveis, { id: item.relacaoItemLojaCD.idItemEntrada })[0];

            if (!!it) {
                item.itemEntrada = it;
                item.relacaoItemLojaCD.vlTipoReabastecimento = it.vlTipoReabastecimento;
                item.relacaoItemLojaCD.cdCrossRef = it.cdCrossRef;
            }
            else {
                item.itemEntrada = null;
                item.relacaoItemLojaCD.vlTipoReabastecimento = null;
                item.relacaoItemLojaCD.cdCrossRef = null;
            }

            item.blPossuiItensXDockDisponiveis = item.itensDisponiveis && item.itensDisponiveis.filter(function (i) {
                return i.idItemDetalhe !== (item.itemEntrada || {}).idItemDetalhe && i.isxDock;
            }).length > 0;

            item.blPossuiItensDSDDisponiveis = item.itensDisponiveis && item.itensDisponiveis.filter(function (i) {
                return i.idItemDetalhe !== (item.itemEntrada || {}).idItemDetalhe && i.isdsd;
            }).length > 0;

            item.blPossuiItensStapleDisponiveis = item.itensDisponiveis && item.itensDisponiveis.filter(function (i) {
                return i.idItemDetalhe !== (item.itemEntrada || {}).idItemDetalhe && i.isStaple;
            }).length > 0;
        }

        function clear() {
            $scope.filters.cdSistema = 1;
            $scope.filters.cdV9D = null;
            $scope.filters.vendor = null;
            $scope.filters.cdItemSaida = null;
            $scope.filters.idRegiaoCompra = null;
            $scope.filters.dsEstado = null;
            $scope.filters.idBandeira = null;
            $scope.filters.blVinculado = false;
            esconde();
        }

        function discardChanges() {
            changeTracker.undoAll();
        }

        $scope.$on('$destroy', function () {
            discardChanges();
            changeTracker.reset();
            changeTracker = null;
        });

        function exibe(data) {
            data = changeTracker.track(data);

            $scope.data.values = data;

            pagingService.acceptPagingResults($scope.data.paging, data);
            $scope.filters.didSearch = true;
        }

        function esconde() {
            $scope.data.values = [];
            changeTracker.undoAll();
            changeTracker.reset();
            $scope.filters.didSearch = false;
        }

        function orderBy(field) {
            pagingService.toggleSorting($scope.data.paging, field);
            search();
        }

        function search(pageNumber) {

            if (!$validation.validate($scope)) return;

            pagingService.calculateOffset($scope.data.paging, pageNumber);

            // Requisição ao serviço
            var deferred = $q
                .when(relacaoItemLojaCDService.obterPorFiltro($scope.filters, $scope.data.paging))
                .then(exibe)
                .catch(esconde);
        }

        function save() {
            var itens = [];

            var tracked = changeTracker.getChangedItems();

            if (tracked.length > 0) {

                angular.forEach(tracked, function (value) {
                    itens.push(value.relacaoItemLojaCD);
                });

                var deferred = $q
                    .when(relacaoItemLojaCDService.salvar(itens))
                    .then(function (result) {

                        changeTracker.commitAll();

                        toastService.success(globalization.texts.savedSuccessfully);

                    });
            }
        }

        function toExport() {
            $q.when(relacaoItemLojaCDService.exportar($scope.filters));
        }

        function exportWithoutChanges() {
            if (!$scope.hasChanges()) {
                toExport();
            }
        }

        function hasChanges() {
            return changeTracker.hasChanges();
        }
    }

    // Configuração do estado
    CadastroRelacaoItemLojaRoute.$inject = ['$stateProvider'];
    function CadastroRelacaoItemLojaRoute($stateProvider) {

        $stateProvider
            .state('cadastroRelacaoItemLoja', {
                url: '/reabastecimento/relacao-item-loja',
                templateUrl: 'Scripts/app/reabastecimento/cadastro-relacao-item-loja.view.html',
                controller: 'CadastroRelacaoItemLojaController',
                params: {
                    filters: null,
                    paging: null
                },
            });
    }
})();