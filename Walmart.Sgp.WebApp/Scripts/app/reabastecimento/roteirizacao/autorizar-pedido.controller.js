(function () {
    'use strict';

    angular
        .module('SGP')
        .config(AutorizarPedidoRoute)
        .controller('AutorizarPedidoController', AutorizarPedidoController);

    AutorizarPedidoController.$inject = ['$scope', '$q', 'PagingService', '$timeout', '$stateParams', 'ValidationService', '$state', 'ToastService', 'ngDialog', 'RoteiroService', 'RoteiroPedidoService', 'roteiro', 'dtPedido', 'cdSistema', 'cdDepartamento'];
    function AutorizarPedidoController($scope, $q, pagingService, $timeout, $stateParams, $validation, $state, toast, ngDialog, roteiroService, roteiroPedidoService, roteiro, dtPedido, cdSistema, cdDepartamento) {

        initialize();

        function initialize() {

            $scope.data = { cdDepartamento: cdDepartamento, cdSistema: cdSistema, roteiro: roteiro, dtPedido: dtPedido, values: null };
            $scope.data.paging = $stateParams.paging || { offset: 0, limit: 40, orderBy: null };
            $scope.header = {};

            $scope.authorize = authorize;
            $scope.search = search;
            $scope.back = back;
            $scope.edit = edit;
            $scope.orderBy = orderBy;
            $scope.exportReport = exportReport;

            if ($scope.data.roteiro && $scope.data.roteiro.idRoteiro > 0) {
                search();
            }

        }

        function edit(item) {
            $state.update({ paging: $scope.data.paging });
            $state.go('detalhePedidoEdit', {
                'id': item.idRoteiro,
                'dtPedido': $scope.data.dtPedido.toISOString().substring(0, 10),
                'idItemDetalhe': item.itemDetalhe.idItemDetalhe,
                'cdSistema': $scope.data.cdSistema,
                'cdDepartamento': $scope.data.cdDepartamento,
            });
        }

        function orderBy(field) {
            pagingService.toggleSorting($scope.data.paging, field);
            search();
        }

        function carregaCabecalho() {
            $scope.header.minimalLoad = $scope.data.roteiro.vlCargaMinima;
            $scope.header.loadType = $scope.data.roteiro.blKgCx ? 'Caixa' : 'KG/UNI';
            $scope.header.cdV9D = $scope.data.roteiro.cdV9D;
            $scope.header.descricao = $scope.data.roteiro.descricao;

            $q.when(roteiroPedidoService.calcularTotalPedido($scope.data.roteiro.idRoteiro, $scope.data.dtPedido, true))
                .then(function (data) {
                    $scope.header.finishedTotal = data;
                });

            $q.when(roteiroPedidoService.obterDadosAutorizacaoRoteiro($scope.data.roteiro.idRoteiro, $scope.data.dtPedido))
                .then(function (data) {
                    $scope.header.autorizado = false;
                    if (data != null) {
                        if (data.blAutorizado) {
                            $scope.header.autorizado = true;
                        }
                    }
                });

            $scope.header.autorizado = true;
        }

        function back() {
            $state.go('pedidosRoteirizados');
        }

        function search(pageNumber) {

            carregaCabecalho();

            pagingService.calculateOffset($scope.data.paging, pageNumber);

            $q.when(roteiroPedidoService.obterRoteirosPedidosPorRoteiroEdtPedido($scope.data.roteiro.idRoteiro, $scope.data.dtPedido, $scope.data.paging))
                .then(exibe)
                .catch(esconde);
        }

        function exibe(data) {
            $scope.data.values = data;
            pagingService.acceptPagingResults($scope.data.paging, data);
        }

        function esconde() {
            $scope.data.values = null;
        }

        function verificarRoteirosZeradosRA() {
            var retorno = false;

            if ($scope.header.finishedTotal == 0) {
                retorno = true;
            }

            return retorno;
        }

        function doAutorize() {
            $q.when(roteiroPedidoService.autorizarRoteiroPedido($scope.data.roteiro.id, $scope.data.dtPedido))
                        .then(function (data) {
                            search();
                        });
        }

        function authorize() {

            var dialog = ngDialog.open({
                template: 'Scripts/app/alertas/confirmar-view.html',
                controller: ['$scope', function ($scope) {
                    $scope.message = globalization.texts.confirmAuthorizeOrders;
                }]
            });

            dialog.closePromise.then(function (data) {
                if (data.value == true) {
                    if (verificarRoteirosZeradosRA()) {
                        var dialog2 = ngDialog.open({
                            template: 'Scripts/app/alertas/confirmar-view.html',
                            controller: ['$scope', function ($scope) {
                                $scope.message = globalization.texts.scriptsWithZeroRAWarning;
                            }]
                        });

                        dialog2.closePromise.then(function (data) {
                            if (data.value == true) {
                                doAutorize();
                            }
                        });
                    }
                    else {
                        doAutorize();
                    }
                }
            });
        }

        function exportReport() {
            var idRoteiro = $scope.data.roteiro.idRoteiro;
            var dsRoteiro = $scope.data.roteiro.descricao;
            var nmVendor = $scope.data.roteiro.fornecedor.nmFornecedor;
            var cdVendor = $scope.data.roteiro.cdV9D;
            var dtPedido = $scope.data.dtPedido;

            roteiroPedidoService.exportarRelatorio(idRoteiro, dsRoteiro, nmVendor, cdVendor, dtPedido);
        }
    }

    AutorizarPedidoRoute.$inject = ['$stateProvider'];
    function AutorizarPedidoRoute($stateProvider) {

        $stateProvider
            /*.state('autorizarPedidoNew', {
                url: '/reabastecimento/pedidos-roteirizados/new',
                templateUrl: 'Scripts/app/reabastecimento/roteirizacao/autorizar-pedido.view.html',
                controller: 'AutorizarPedidoController',
                params: {
                    filters: null,
                    paging: null,
                    dtPedido: null,
                    cdSistema: null,
                    cdDepartamento: null
                },
                resolve: {
                    roteiro: function () {
                        return {};
                    },
                    dtPedido: function () {
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
            .state('autorizarPedidoEdit', {
                url: '/reabastecimento/pedidos-roteirizados/edit/:id/:cdSistema/:dtPedido/:cdDepartamento',
                templateUrl: 'Scripts/app/reabastecimento/roteirizacao/autorizar-pedido.view.html',
                controller: 'AutorizarPedidoController',
                params: {
                    filters: null,
                    paging: null,
                    dtPedido: null,
                    cdSistema: null,
                    cdDepartamento: null
                },
                resolve: {
                    roteiro: ['$stateParams', 'RoteiroService', function ($stateParams, roteiroService) {
                        return roteiroService.obterEstruturadoPorId($stateParams.id);
                    }],
                    dtPedido: ['$stateParams', function ($stateParams) {
                        return moment($stateParams.dtPedido)._d;
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