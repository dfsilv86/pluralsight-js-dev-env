(function () {
    'use strict';

    angular
        .module('SGP')
        .config(DetalhePedidoRoute)
        .controller('DetalhePedidoController', DetalhePedidoController);

    DetalhePedidoController.$inject = ['$scope', '$q', 'PagingService', '$timeout', '$stateParams', 'ValidationService', '$state', 'ToastService', 'ngDialog', 'RoteiroService', 'RoteiroPedidoService', 'roteiro', 'dtPedido', 'item', 'ChangeTrackerFactory', 'cdSistema', 'cdDepartamento'];
    function DetalhePedidoController($scope, $q, pagingService, $timeout, $stateParams, $validation, $state, toast, ngDialog, roteiroService, roteiroPedidoService, roteiro, dtPedido, item, changeTrackerFactory, cdSistema, cdDepartamento) {

        var changeTracker = changeTrackerFactory.createChangeTrackerForProperties(['valorEmCaixaRA'], function (left, right) {
            return !!left && !!right && left.idSugestaoPedido == right.idSugestaoPedido;
        })

        initialize();

        function initialize() {

            $scope.data = { cdSistema: cdSistema, cdDepartamento: cdDepartamento, item: item, roteiro: roteiro, dtPedido: dtPedido, values: null };
            $scope.data.paging = { offset: 0, limit: 40, orderBy: null };
            $scope.header = {};

            $scope.save = save;
            $scope.search = search;
            $scope.back = back;
            $scope.calcularRA = calcularRA;
            $scope.orderBy = orderBy;
            $scope.exportReport = exportReport;
            $scope.hasChanges = hasChanges;
            $scope.discardChanges = discardChanges;

            if ($scope.data.roteiro && $scope.data.roteiro.idRoteiro > 0) {
                search();
            }
        }

        function hasChanges() {
            return changeTracker.hasChanges();
        }

        function discardChanges() {
            changeTracker.undoAll();
        }

        function calcularRA() {
            $scope.header.raTotalQtd = 0;
            angular.forEach($scope.data.values, function (value, key) {
                if (value.valorEmCaixaRA == null) {
                    value.valorEmCaixaRA = 0;
                }
                $scope.header.raTotalQtd = $scope.header.raTotalQtd + value.valorEmCaixaRA;
            });
        }

        function orderBy(field) {
            pagingService.toggleSorting($scope.data.paging, field);
            search();
        }

        function save() {
            var sugestoes = changeTracker.getChangedItems();

            var deferred = $q
                .when(roteiroService.salvarSugestaoPedidoConvertidoCaixa(sugestoes, $scope.data.roteiro.idRoteiro))
                .then(salvou);
        }

        function salvou(data) {
            toast.success(globalization.texts.sugestionsSavedSuccessfuly);

            discardChanges();
            search();
        }

        function carregaCabecalho() {
            $scope.header.minimalLoad = $scope.data.roteiro.vlCargaMinima;
            $scope.header.loadType = $scope.data.roteiro.blKgCx ? 'Caixa' : 'KG/UNI';
            $scope.header.cdV9D = $scope.data.roteiro.cdV9D;
            $scope.header.descricao = $scope.data.roteiro.descricao;

            $q.when(roteiroPedidoService.obterDadosAutorizacaoRoteiro($scope.data.roteiro.idRoteiro, $scope.data.dtPedido))
                .then(function (data) {
                    $scope.header.autorizado = false;
                    if (data != null) {
                        if (data.blAutorizado) {
                            $scope.header.autorizado = true;
                        }
                    }
                });
        }

        function back(confirmou) {
            if (changeTracker.hasChanges() && !confirmou) {
                return;
            }

            $state.go('autorizarPedidoEdit'); //, { filters: $stateParams.filters, paging: $stateParams.paging, id: $scope.data.roteiro.idRoteiro, dtPedido: $scope.data.dtPedido, cdSistema: $scope.data.cdSistema, cdDepartamento: $scope.data.cdDepartamento });
        }

        function search(pageNumber) {

            pagingService.calculateOffset($scope.data.paging, pageNumber);

            carregaCabecalho();

            $q.when(roteiroService.obterSugestaoPedidoLoja($scope.data.roteiro.idRoteiro, $scope.data.dtPedido, $scope.data.item.idItemDetalhe, $scope.data.paging))
                .then(exibe)
                .catch(esconde);
        }

        function exibe(data) {
            changeTracker.track(data);
            $scope.data.values = data;
            pagingService.acceptPagingResults($scope.data.paging, data);

            $scope.header.itemTotal = 0;
            angular.forEach($scope.data.values, function (value, key) {
                $scope.header.itemTotal = $scope.header.itemTotal + value.valorEmCaixa;
            });

            calcularRA();
        }

        function esconde() {
            $scope.data.values = null;
        }

        function exportReport() {
            var idRoteiro = $scope.data.roteiro.idRoteiro;
            var dsRoteiro = $scope.data.roteiro.descricao;
            var nmVendor = $scope.data.roteiro.fornecedor.nmFornecedor;
            var cdVendor = $scope.data.roteiro.cdV9D;
            var dtPedido = $scope.data.dtPedido;
            var idItemDetalhe = $scope.data.item.idItemDetalhe;

            roteiroPedidoService.exportarRelatorio(idRoteiro, dsRoteiro, nmVendor, cdVendor, dtPedido, idItemDetalhe);
        }
    }

    DetalhePedidoRoute.$inject = ['$stateProvider'];
    function DetalhePedidoRoute($stateProvider) {

        $stateProvider
            /*.state('detalhePedidoNew', {
                url: '/reabastecimento/pedidos-roteirizados/detalhe/new',
                templateUrl: 'Scripts/app/reabastecimento/roteirizacao/detalhe-pedido.view.html',
                controller: 'DetalhePedidoController',
                params: {
                    dtPedido: null
                },
                resolve: {
                    roteiro: function () {
                        return {};
                    },
                    dtPedido: function () {
                        return null;
                    },
                    item: function () {
                        return null;
                    },
                    cdSistema: function () {
                        return null;
                    },
                    cdDepartamento: function () {
                        return null;
                    }
                }
            })*/ // TODO: nao utilizado, confirmar e remover
            .state('detalhePedidoEdit', {
                url: '/reabastecimento/pedidos-roteirizados/detalhe/:id/:cdSistema/:cdDepartamento/:dtPedido/:idItemDetalhe',
                templateUrl: 'Scripts/app/reabastecimento/roteirizacao/detalhe-pedido.view.html',
                controller: 'DetalhePedidoController',
                params: {
                    dtPedido: null,
                    cdDepartamento: null,
                    cdSistema: null
                },
                resolve: {
                    roteiro: ['$stateParams', 'RoteiroService', function ($stateParams, roteiroService) {
                        return roteiroService.obterEstruturadoPorId($stateParams.id);
                    }],
                    dtPedido: ['$stateParams', function ($stateParams) {
                        return moment($stateParams.dtPedido)._d;
                    }],
                    item: ['$stateParams', 'ItemDetalheService', function ($stateParams, itemDetalheService) {
                        return itemDetalheService.obter($stateParams.idItemDetalhe);
                    }],
                    cdSistema: ['$stateParams', function ($stateParams) {
                        return $stateParams.cdSistema;
                    }],
                    cdDepartamento: ['$stateParams', function ($stateParams) {
                        return $stateParams.cdDepartamento;
                    }]
                }
            });
    }
})();