/// <reference path="../../bower/angular/angular.js" />
(function () {
    'use strict';

    // Configuração da controller
    angular
        .module('SGP')
        .config(CadastroRoteiroRoute)
        .controller('CadastroRoteiroController', CadastroRoteiroController);

    // Implementação da controller
    CadastroRoteiroController.$inject = ['$scope', '$q', '$timeout', '$stateParams', 'ValidationService', 'PagingService', '$state', 'ToastService', 'ChangeTrackerFactory', 'RoteiroLojaService', 'RoteiroService', 'RoteiroPedidoService', 'ngDialog', 'roteiro'];
    function CadastroRoteiroController($scope, $q, $timeout, $stateParams, $validation, pagingService, $state, toast, changeTrackerFactory, roteiroLojaService, roteiroService, roteiroPedidoService, ngDialog, roteiro) {

        var changeTracker = changeTrackerFactory.createChangeTrackerForProperties(['blativo'], function (left, right) {
            return !!left && !!right && left.idloja === right.idloja;
        });

        var changeTracker2 = changeTrackerFactory.createChangeTrackerForProperties(['cdV9D', 'descricao', 'vlCargaMinima', 'blKgCxStr'], function (left, right) {
            return !!left && !!right && left.idRoteiro === right.idRoteiro;
        });

        initialize();

        function initialize() {
            $validation.prepare($scope);

            $scope.data = {
                paging: {
                    offset: 0,
                    limit: 40,
                    orderBy: 'blAtivo DESC, dsEstado, cdLoja'
                },
                todosMarcado: false
            };
            $scope.filters = { cdSistema: 1 };
            $scope.roteiro = roteiro;
            $scope.search = search;
            $scope.back = back;
            $scope.backWithoutChanges = backWithoutChanges;
            $scope.save = save;
            $scope.remove = remove;
            $scope.marcarTodos = marcarTodos;
            $scope.hasChanges = hasChanges;
            $scope.discardChanges = discardChanges;
            $scope.hasSelection = hasSelection;
            $scope.discardSelection = discardSelection;

            $scope.$watch('roteiro.blKgCxStr', setBlKgCx);
            $scope.$watch('filters.vendor', vendorChanged);

            if (roteiro.idRoteiro && roteiro.idRoteiro > 0) {
                setVendor(roteiro);
                $scope.roteiro.blKgCxStr = $scope.roteiro.blKgCx.toString();

                search();
            }
            else {
                roteiro.blAtivo = true;
            }

            changeTracker2.track($scope.roteiro);
        }

        function hasChanges() {
            return changeTracker2.hasChanges();
        }

        function discardChanges() {
            changeTracker2.undoAll();
        }

        function hasSelection() {
            return changeTracker.hasChanges();
        }

        function discardSelection() {
            changeTracker.undoAll();
        }

        function search(pageNumber) {
            if (!$validation.validate($scope))
                return;

            pagingService.calculateOffset($scope.data.paging, pageNumber);

            var cdV9D = $scope.roteiro.cdV9D;
            var dsEstado = $scope.filters.dsEstado || '';
            var idRoteiro = roteiro.idRoteiro || '';

            $q.when(roteiroLojaService.obterLojasValidas(cdV9D, dsEstado, idRoteiro, $scope.data.paging))
                .then(exibe)
                .catch(esconde);
        }

        function exibe(data) {
            changeTracker.track(data);
            $scope.data.values = data;
            pagingService.acceptPagingResults($scope.data.paging, data);
            $validation.accept($scope);
        }

        function esconde() {
            $scope.data.values = [];
        }

        function back() {
            $state.go('cadastroRoteiroEntrega'); //, { filters: $stateParams.filters, paging: $stateParams.paging });
        }

        function backWithoutChanges() {
            if (!hasSelection() && !hasChanges()) {
                back();
            }
        }

        function save() {
            if (!$validation.validate($scope))
                return;

            $scope.roteiro.lojas = changeTracker.getChangedItems();

            $q.when(roteiroService.salvar($scope.roteiro))
                .then(function () {
                    back(true);
                    toast.success(globalization.texts.scriptSavedSuccessfully);
                });
        }

        function remove() {
            var idRoteiro = $scope.roteiro.idRoteiro;
            
            $q.when(roteiroPedidoService.qtdPedidosNaoAutorizadosParaDataCorrente(idRoteiro))
                .then(validarRemocao);
        }

        function validarRemocao(qtdPedidosNaoAutorizadosParaDataCorrente) {
            if (qtdPedidosNaoAutorizadosParaDataCorrente === 0) {
                doRemove();
            }
            else {
                var dialog = ngDialog.open({
                    template: 'Scripts/app/alertas/confirmar-view.html',
                    controller: ['$scope', function ($scope) {
                        $scope.message = globalization.texts.thereAreOrdersNotApproved;
                    }]
                });

                dialog.closePromise.then(function (data) {
                    if (data.value) {
                        doRemove();
                    }
                });
            }
        }

        function doRemove() {
            var idRoteiro = $scope.roteiro.idRoteiro;

            $q.when(roteiroService.remove(idRoteiro))
                .then(function () {
                    back();
                    toast.success(globalization.texts.recordRemovedSuccessfully);
                });
        }

        function setBlKgCx(newValue, oldValue) {
            if (newValue === oldValue)
                return;

            $scope.roteiro.blKgCx = JSON.parse(newValue);
        }

        function setVendor(roteiro) {
            var vendor = {
                cdSistema: $scope.filters.cdSistema,
                cdV9D: roteiro.cdV9D,
                fornecedor: { 
                    nmFornecedor: roteiro.fornecedor.nmFornecedor,
                    cdFornecedor: roteiro.fornecedor.cdFornecedor
                }
            };

            $scope.filters.vendor = vendor;
            $scope.filters.cdV9D = vendor.cdV9D;
        }

        function vendorChanged() {
            discardSelection();
            $scope.data.values = [];
            $scope.filters.dsEstado = null;
        }

        function marcarTodos() {
            window.event.stopPropagation();

            angular.forEach($scope.data.values, function (item) {
                item.blativo = $scope.data.todosMarcado;
            });
        }
    }

    // Configuração do estado
    CadastroRoteiroRoute.$inject = ['$stateProvider'];
    function CadastroRoteiroRoute($stateProvider) {

        $stateProvider
            .state('cadastroRoteiroNew', {
                url: '/reabastecimento/cadastro-roteiro-entrega/new',
                templateUrl: 'Scripts/app/reabastecimento/roteirizacao/cadastro-roteiro.view.html',
                controller: 'CadastroRoteiroController',
                resolve: {
                    roteiro: function () {
                        return {};
                    }
                }
            })
            .state('cadastroRoteiroEdit', {
                url: '/reabastecimento/cadastro-roteiro-entrega/edit/:id',
                templateUrl: 'Scripts/app/reabastecimento/roteirizacao/cadastro-roteiro.view.html',
                controller: 'CadastroRoteiroController',
                resolve: {
                    roteiro: ['$stateParams', 'RoteiroService', function ($stateParams, roteiroService) {
                        return roteiroService.obterEstruturadoPorId($stateParams.id);
                    }]
                }
            });
    }
})();
