(function () {
    'use strict';

    angular
        .module('SGP')
        .config(PedidosRoteirizadosRoute)
        .controller('PedidosRoteirizadosController', PedidosRoteirizadosController);

    PedidosRoteirizadosController.$inject = ['$scope', '$q', '$timeout', '$stateParams', '$state', 'UserSessionService', 'ValidationService', 'PagingService', 'ChangeTrackerFactory', 'ToastService', 'StackableModalService', 'RoteiroPedidoService', 'ngDialog'];
    function PedidosRoteirizadosController($scope, $q, $timeout, $stateParams, $state, userSession, $validation, pagingService, changeTrackerFactory, toastService, $modal, roteiroPedidoService, ngDialog) {

        initialize();

        function initialize() {

            $validation.prepare($scope);

            $scope.data = { values: null };
            $scope.data.paging = $stateParams.paging || { offset: 0, limit: 50, orderBy: null };
            $scope.filters = $stateParams.filters || { cdSistema: 1, dtPedido: new Date(), departamento: null, v9D: null, status: null, script: null, temMarcado: false };

            $scope.clear = clear;
            $scope.authorize = authorize;
            $scope.search = search;
            $scope.edit = edit;
            $scope.orderBy = orderBy;
            $scope.temMarcadoParaAutorizar = temMarcadoParaAutorizar;

            if ($scope.filters.didSearch) {
                search();
            }
        }

        function orderBy(field) {
            pagingService.toggleSorting($scope.data.paging, field);
            search();
        }

        function temMarcadoParaAutorizar() {
            $scope.filters.temMarcado = false;
            angular.forEach($scope.data.values, function (value, key) {
                if (value.chkAutorizar) {
                    $scope.filters.temMarcado = true;
                }
            });
        }

        function edit(item) {
            $state.update({
                filters: $scope.filters,
                paging: $scope.data.paging
            });
            $state.go('autorizarPedidoEdit', {
                'id': item.idRoteiro,
                'dtPedido': $scope.filters.dtPedido.toISOString().substring(0, 10),
                'cdSistema': $scope.filters.cdSistema,
                'cdDepartamento': $scope.filters.cdDepartamento
            });
        }

        function search(pageNumber) {

            if (!$validation.validate($scope)) return;

            pagingService.calculateOffset($scope.data.paging, pageNumber);

            var dtPedido = $scope.filters.dtPedido;
            var idDepartamento = ($scope.filters.departamento || {}).idDepartamento || '';
            var cdV9D = $scope.filters.v9D || '';
            var stPedido = $scope.filters.status || '';
            var roteiro = $scope.filters.script || '';

            $q.when(roteiroPedidoService.obterPedidosRoteirizados(dtPedido, idDepartamento, cdV9D, stPedido, roteiro, $scope.data.paging))
                .then(exibe)
                .catch(esconde);
        }

        function exibe(data) {
            $scope.data.values = data;
            pagingService.acceptPagingResults($scope.data.paging, data);

            temMarcadoParaAutorizar();
            $scope.filters.didSearch = true;
            $validation.accept($scope);
        }

        function esconde() {
            $scope.data.values = null;
            temMarcadoParaAutorizar();
            $scope.filters.didSearch = false;
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

        function verificarRoteirosZeradosRA() {
            var retorno = false;

            angular.forEach($scope.data.values, function (value, key) {
                if (value.chkAutorizar && value.totalPedidoRA == 0) {
                    retorno = true;
                }
            });

            return retorno;
        }

        function doAutorize() {

            var roteiros = [];

            angular.forEach($scope.data.values, function (value, key) {
                if (value.chkAutorizar) {
                    roteiros.push(value);
                }
            });

            $q.when(roteiroPedidoService.autorizarPedidos(roteiros, $scope.filters.dtPedido))
                .then(function (data) {
                    search();
                });
        }

        function clear() {
            $scope.filters = {
                dtPedido: new Date(), departamento: null, v9D: null, status: null, script: null
            };
            $scope.data.values = null;
            $scope.filters.didSearch = false;
        }
    }

    PedidosRoteirizadosRoute.$inject = ['$stateProvider'];
    function PedidosRoteirizadosRoute($stateProvider) {

        $stateProvider
            .state('pedidosRoteirizados', {
                url: '/reabastecimento/pedidos-roteirizados',
                templateUrl: 'Scripts/app/reabastecimento/roteirizacao/pedidos-roteirizados.view.html',
                controller: 'PedidosRoteirizadosController',
                params: {
                    filters: null,
                    paging: null
                },
            });
    }
})();