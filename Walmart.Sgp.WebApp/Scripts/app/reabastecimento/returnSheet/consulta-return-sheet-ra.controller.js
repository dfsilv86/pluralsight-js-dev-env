(function () {
    'use strict';

    angular
          .module('SGP')
          .config(ConsultaReturnSheetRARoute)
          .controller('ConsultaReturnSheetRAController', ConsultaReturnSheetRAController);

    ConsultaReturnSheetRAController.$inject = ['$scope', '$q', 'StackableModalService', 'SugestaoReturnSheetService', 'ToastService', 'ValidationService', 'ChangeTrackerFactory', 'UserSessionService', 'ngDialog', 'PagingService'];
    function ConsultaReturnSheetRAController($scope, $q, $modal, sugestaoReturnSheetService, toastService, $validation, changeTrackerFactory, userSessionService, ngDialog, pagingService) {
        $validation.prepare($scope);

        $scope.data = { autorizarHabilitado: false, values: null };
        $scope.data.paging = { offset: 0, limit: 50, orderBy: null };

        $scope.filters = { cdSistema: 1, inicioReturn: null, finalReturn: null, cdV9D: null, evento: null, cdOldNumber: null, cdDepartamento: null, cdLoja: null, idRegiaoCompra: null, blExportado: "null", blAutorizado: "null" };

        $scope.search = search;
        $scope.clear = clear;
        $scope.save = save;
        $scope.orderBy = orderBy;
        $scope.authorize = doAuthorize;
        $scope.edit = edit;
        $scope.recalcularSubtotal = recalcularSubtotal;
        $scope.hasChanges = hasChanges;
        $scope.discardChanges = discardChanges;
        $scope.exportGrid = exportGrid;
        $scope.exportGridWithoutChanges = exportGridWithoutChanges;

        var changeTracker = changeTrackerFactory.createChangeTrackerForProperties(['qtdRA', 'subtotal'], function (left, right) {
            return !!left && !!right && left.idSugestaoReturnSheet === right.idSugestaoReturnSheet;
        });

        clear();

        function orderBy(field) {
            pagingService.toggleSorting($scope.data.paging, field);
            search();
        }

        function hasChanges() {
            return changeTracker.hasChanges();
        }

        function discardChanges() {
            changeTracker.undoAll();
        }

        function search(pageNumber) {
            if (!$validation.validate($scope)) return;

            if (!validaCampos()) {
                return;
            }

            //escondeConsulta();

            pagingService.calculateOffset($scope.data.paging, pageNumber);

            $scope.data.autorizarHabilitado = false;

            var dtInicioReturn = $scope.filters.inicioReturn;
            var dtFinalReturn = $scope.filters.finalReturn;
            var cdV9D = $scope.filters.cdV9D;
            var evento = $scope.filters.evento;
            var cdItemDetalhe = $scope.filters.cdOldNumber;
            var cdDepartamento = $scope.filters.cdDepartamento;
            var cdLoja = $scope.filters.cdLoja;
            var idRegiaoCompra = $scope.filters.idRegiaoCompra;
            var blExportado = JSON.parse($scope.filters.blExportado);
            var blAutorizado = JSON.parse($scope.filters.blAutorizado);

            $q.when(sugestaoReturnSheetService.consultaReturnSheetRA(dtInicioReturn, dtFinalReturn, cdV9D, evento, cdItemDetalhe, cdDepartamento, cdLoja, idRegiaoCompra, blExportado, blAutorizado, $scope.data.paging))
              .then(exibeConsulta)
              .catch(escondeConsulta);
        }

        function exibeConsulta(data) {
            changeTracker.track(data);
            $scope.data.values = data;
            $scope.habilitarExportarGrid = data.length > 0;

            angular.forEach($scope.data.values, function (value, key) {
                if (!value.blAutorizado) {
                    $scope.data.autorizarHabilitado = true;
                }
            });
            $validation.accept($scope);
            return data;
        }

        function escondeConsulta() {
            $scope.data.values = null;
        }

        function clear() {
            $scope.filters.inicioReturn = null;
            $scope.filters.finalReturn = null;
            $scope.filters.cdV9D = null;
            $scope.filters.evento = null;
            $scope.filters.cdOldNumber = null;
            $scope.filters.cdDepartamento = null;
            $scope.filters.cdLoja = null;
            $scope.filters.idRegiaoCompra = null;
            $scope.filters.blExportado = "null";
            $scope.filters.blAutorizado = "null";
            $scope.data.autorizarHabilitado = false;
            $scope.habilitarExportarGrid = false;

            $scope.data.values = null;
            $scope.data.paging.offset = 0;
            discardChanges();
        }

        function save() {
            var sugestoes = changeTracker.getChangedItems();

            $q.when(sugestaoReturnSheetService.salvarRA(sugestoes))
                .then(salvou);
        }

        function salvou() {
            toastService.success(globalization.texts.returnSheetSugestionSavedSuccessfully);
            changeTracker.reset();
            search();
        }

        function edit(item) {
            $modal.open({
                templateUrl: 'Scripts/app/reabastecimento/returnsheet/modal-detalhe-consulta-return-sheet-ra.view.html',
                controller: 'ModalDetalheConsultaReturnSheetRAController',
                resolve: {
                    sugestaoReturnSheet: item
                }
            });
        }

        function doAuthorize() {
            if (changeTracker.hasChanges()) {
                var dialog = ngDialog.open({
                    template: 'Scripts/app/alertas/confirmar-view.html',
                    controller: ['$scope', function ($scope) {
                        $scope.message = globalization.texts.confirmUnsavedDataExport;
                    }]
                });

                dialog.closePromise.then(function (data) {
                    if (data.value === true) {
                        doExport1();
                    }
                });
            }
            else {
                doExport1();
            }
        }

        function doExport1() {
            var hoje = new Date();
            var temAberta = false;

            angular.forEach($scope.data.values, function (value, key) {
                if (new Date(value.returnSheet.dhFinalReturn) > hoje) {
                    temAberta = true;
                }
            });

            if (temAberta) {
                var dialog = ngDialog.open({
                    template: 'Scripts/app/alertas/confirmar-view.html',
                    controller: ['$scope', function ($scope) {
                        $scope.message = globalization.texts.openRSRemaingToStoreContinueExport;
                    }]
                });

                dialog.closePromise.then(function (data) {
                    if (data.value === true) {

                        var dialog2 = ngDialog.open({
                            template: 'Scripts/app/alertas/confirmar-view.html',
                            controller: ['$scope', function ($scope) {
                                $scope.message = globalization.texts.returnBecameBlockedForStoresAfterExport;
                            }]
                        });

                        dialog2.closePromise.then(function (data) {
                            if (data.value === true) {
                                doExport2();
                            }
                        });

                    }
                });
            }
            else {
                doExport2();
            }
        }

        function doExport2() {
            var dialog = ngDialog.open({
                template: 'Scripts/app/alertas/confirmar-view.html',
                controller: ['$scope', function ($scope) {
                    $scope.message = globalization.texts.exportProcedureExecutedOK;
                }]
            });

            dialog.closePromise.then(function (data) {
                if (data.value === true) {
                    authorize();
                }
            });
        }

        function authorize() {
            var dtInicioReturn = $scope.filters.inicioReturn;
            var dtFinalReturn = $scope.filters.finalReturn;
            var cdV9D = $scope.filters.cdV9D;
            var evento = $scope.filters.evento;
            var cdItemDetalhe = $scope.filters.cdOldNumber;
            var cdDepartamento = $scope.filters.cdDepartamento;
            var cdLoja = $scope.filters.cdLoja;
            var idRegiaoCompra = $scope.filters.idRegiaoCompra;
            var blExportado = JSON.parse($scope.filters.blExportado);
            var blAutorizado = JSON.parse($scope.filters.blAutorizado);

            $q.when(sugestaoReturnSheetService.autorizarExportarPlanilhas(dtInicioReturn, dtFinalReturn, cdV9D, evento, cdItemDetalhe, cdDepartamento, cdLoja, idRegiaoCompra, blExportado, blAutorizado))
                .then(function (data) {
                    search();
                    toastService.success(globalization.texts.exportProcedureExecutedSuccess);
                });
        }

        function recalcularSubtotal(item) {
            if (item.itemDetalheEntrada.tpCaixaFornecedor === 'F') {
                item.subtotal = item.qtdRA;
            } else {
                if (item.vlPesoLiquidoItemCompra !== null && item.vlPesoLiquidoItemCompra > 0) {
                    item.subtotal = Math.round(item.qtdRA / item.vlPesoLiquidoItemCompra);
                } else {
                    item.subtotal = item.qtdRA;
                }
            }
        }

        function validaCampos() {

            if ($scope.filters.inicioReturn === null || $scope.filters.inicioReturn === undefined
                || $scope.filters.finalReturn === null || $scope.filters.finalReturn === undefined)
                return true;

            if ($scope.filters.inicioReturn > $scope.filters.finalReturn) {
                toastService.warning(globalization.texts.finalDateMustBeGreaterThanStart);
                return false;
            }

            return true;
        }

        function exportGrid() {
            if (!$validation.validate($scope)) return;

            if (!validaCampos()) {
                return;
            }

            var dtInicioReturn = $scope.filters.inicioReturn;
            var dtFinalReturn = $scope.filters.finalReturn;
            var cdV9D = $scope.filters.cdV9D;
            var evento = $scope.filters.evento;
            var cdItemDetalhe = $scope.filters.cdOldNumber;
            var cdDepartamento = $scope.filters.cdDepartamento;
            var cdLoja = $scope.filters.cdLoja;
            var idRegiaoCompra = $scope.filters.idRegiaoCompra;
            var blExportado = JSON.parse($scope.filters.blExportado);
            var blAutorizado = JSON.parse($scope.filters.blAutorizado);

            $q.when(sugestaoReturnSheetService.exportarRA(dtInicioReturn, dtFinalReturn, cdV9D, evento, cdItemDetalhe, cdDepartamento, cdLoja, idRegiaoCompra, blExportado, blAutorizado, $scope.data.paging));
        }

        function exportGridWithoutChanges() {
            if (!$scope.hasChanges()) {
                exportGrid();
            }
        }

        $scope.$on('$destroy', function () {
            discardChanges();
            changeTracker.reset();
            changeTracker = null;
        });
    }

    ConsultaReturnSheetRARoute.$inject = ['$stateProvider'];
    function ConsultaReturnSheetRARoute($stateProvider) {
        $stateProvider
            .state('consultaReturnSheetRA', {
                url: '/consultaReturnSheetRA',
                templateUrl: 'Scripts/app/reabastecimento/returnSheet/consulta-return-sheet-ra.view.html',
                controller: 'ConsultaReturnSheetRAController'
            });
    }
})();
