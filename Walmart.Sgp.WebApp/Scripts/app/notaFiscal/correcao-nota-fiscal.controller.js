(function () {
    'use strict';

    // Configuração da controller
    angular
        .module('SGP')
        .config(CorrecaoNotaFiscalRoute)
        .controller('CorrecaoNotaFiscalController', CorrecaoNotaFiscalController);

    // Implementação da controller
    CorrecaoNotaFiscalController.$inject = ['$scope', '$q', '$timeout', '$stateParams', '$state', 'ValidationService', 'NotaFiscalService', 'ToastService', 'ConfirmService', 'PagingService', 'ChangeTrackerFactory'];
    function CorrecaoNotaFiscalController($scope, $q, $timeout, $stateParams, $state, $validation, notaFiscalService, toast, confirm, pagingService, changeTrackerFactory) {

        var ordenacaoPadrao = 'IDLoja asc';

        $validation.prepare($scope);
        
        setupFilters();
        $scope.data = { values: null, liberar: true };
        $scope.data.paging = $stateParams.paging || { offset: 0, limit: 10, orderBy: ordenacaoPadrao };
        $scope.search = search;
        $scope.clear = clear;
        $scope.confirmSave = confirmSave;
        $scope.marcarTodos = marcarTodos;
        $scope.marcarLiberar = marcarLiberar;
        $scope.hasChanges = hasChanges;
        $scope.discardChanges = discardChanges;
        $scope.podeMarcarTodos = false;

        var changeTracker = changeTrackerFactory.createChangeTrackerForProperties(
            ['blLiberar', 'qtAjustada'],
            function (left, right) {
                return !!left && !!right && left.idNotaFiscalItem == right.idNotaFiscalItem;
            });

        if (!!$stateParams.changes) {
            changeTracker.inherit($stateParams.changes);
        }

        $scope.$on('$destroy', function () {
            discardChanges();
            changeTracker.reset();
            changeTracker = null;
        });

        function hasChanges() {

            // Se não houve alterações, desabilita salvar
            if (!changeTracker.hasChanges()) return false;

            var tracked = changeTracker.getChangedItems();

            // Se tem pelo menos um item liberado, habilita salvar
            for (var i = 0; i < tracked.length; i++) {
                if (!!tracked[i].blLiberar) return true;
            }
            
            // Se nenhum foi liberado, desabilita salvar.
            return false;
        }

        function discardChanges() {
            changeTracker.undoAll();
        }

        function marcarTodos() {
            if ($scope.data.values != null) {
                var changedCount = 0;
                for (var i = 0; i < $scope.data.values.length; i++)
                {
                    if ($scope.data.values[i]['blLiberar'] != !!$scope.data.liberar) {
                        $scope.data.values[i]['blLiberar'] = !!$scope.data.liberar;
                        changedCount++;
                    }
                }

                if (changedCount > 0) {
                    toast.warning(globalization.texts.checkAllWarning.format(changedCount, changedCount == 1 ? globalization.texts.itemSingular : globalization.texts.itemsPlural, $scope.data.values.totalCount, changedCount == 1 ? globalization.texts.wasSingular : globalization.texts.werePlural, changedCount == 1 ? (!!$scope.data.liberar ? globalization.texts.checkedSingular : globalization.texts.uncheckedSingular) : (!!$scope.data.liberar ? globalization.texts.checkedPlural : globalization.texts.uncheckedPlural)));
                }
            }
        }

        function marcarLiberar() {
            if ($scope.data.values != null) {
                $scope.data.liberar = true;

                for (var i = 0; i < $scope.data.values.length; i++) {
                    if (!$scope.data.values[i]['blLiberar']) {
                        $scope.data.liberar = false;
                    }
                }
            }
        }

        function setupFilters() {
            $scope.filters = $stateParams.filters || {
                cdSistema: null,
                idBandeira: null,
                cdLoja: null,
                cdDepartamento: null,
                nrNotaFiscal: null,
                cdItem: null,
                item: null,
                dtRecebimento: { startValue: null, endValue: null },
                dtCadastroConcentrador: { startValue: null, endValue: null },
                dtAtualizacaoConcentrador: { startValue: null, endValue: null },
                cdFornecedor: null,
                fornecedor: null,
                idNotaFiscalItemStatus: sgpFixedValues.statusNotaFiscalItem.idPendente
            };
        }

        function clear() {
            escondeCustos();
        }

        function exibeCustos(data) {

            data = changeTracker.track(data);

            $scope.data.values = data;

            pagingService.acceptPagingResults($scope.data.paging, data);

            marcarLiberar();

            $scope.podeMarcarTodos = !!data && !!data.length;
        }

        function escondeCustos() {
            setupFilters();
            $scope.data.values = null;

            discardChanges();
            changeTracker.reset();
        }

        function search(pageNumber) {

            if (!$validation.validate($scope)) return;

            pagingService.calculateOffset($scope.data.paging, pageNumber);
            
            // Requisição ao serviço
            var deferred = $q
                .when(notaFiscalService.pesquisarCustosPorFiltros($scope.filters, $scope.data.paging))
                .then(exibeCustos)
                .catch(escondeCustos);
        }

        function confirmSave() {
            if (!$validation.validate($scope)) return;

            if ($scope.data.values == null || $scope.data.values.length == 0) {
                toast.warning(globalization.texts.thereAreNoRecordsToBeSaved);
                return;
            }

            confirm.open({
                message: globalization.texts.confirmAlteration,
                yes: save
            });
        }

        function save() {

            var itens = [];

            var tracked = changeTracker.getChangedItems();

            angular.forEach(tracked, function (value) {
                // TODO: talvez seja possível remover o idBandeira
                if (!!value.blLiberar) {
                    itens.push(value);
                }
            });

            notaFiscalService
                .corrigirCustos(itens)
                .then(function () {
                    toast.success(globalization.texts.savedSuccessfully);
                    changeTracker.commitAll();
                    search();
                });
        }

        function detail(item) {
            // No momento a tela não detalha registros, mas quando for necessário detalhar, use isto para manter a lista de itens modificados entre transições de página
            var theChanges = changeTracker.getChangedItems();
            changeTracker.reset();
            $state.update({ 'filters': $scope.filters, 'paging': $scope.data.paging, 'changes': theChanges });
            //$state.go('detino', { ... parametros ... });
        }
    }

    // Configuração do estado
    CorrecaoNotaFiscalRoute.$inject = ['$stateProvider'];
    function CorrecaoNotaFiscalRoute($stateProvider) {

        $stateProvider
            .state('correcaoNotaFiscal', {
                url: '/notafiscal/correcao',
                templateUrl: 'Scripts/app/notafiscal/correcao-nota-fiscal.view.html',
                controller: 'CorrecaoNotaFiscalController',
                params: {
                    filters: null,
                    paging: null,
                    changes: null
                }
            });
    }
})();