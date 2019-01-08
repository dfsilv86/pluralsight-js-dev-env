(function () {
    'use strict';

    angular
		.module('SGP')
        .config(PesquisaItensReturnSheetRoute)
		.controller('PesquisaItensReturnSheetController', PesquisaItensReturnSheetController);

    PesquisaItensReturnSheetController.$inject = ['$scope', '$state', '$stateParams', 'StackableModalService', 'ValidationService', 'PagingService', '$q', 'ItemDetalheService', 'ReturnSheetService', 'idReturnSheet', 'cdSistema', 'jaComecou', 'podeSerEditada', 'returnSheet'];
    function PesquisaItensReturnSheetController($scope, $state, $stateParams, $modal, $validation, pagingService, $q, itemDetalheService, returnSheetService, idReturnSheet, cdSistema, jaComecou, podeSerEditada, returnSheet) {

        $validation.prepare($scope);

        $scope.filters = $stateParams.filters || { idReturnSheet: idReturnSheet, sgpRelationship: 2, idRegiaoCompra: returnSheet.idRegiaoCompra, cdSistema: cdSistema, cdDepartamento: (returnSheet.departamento || {}).cdDepartamento };

        $scope.filters.idReturnSheet = idReturnSheet;

        if (idReturnSheet) {
            $scope.filters.idReturnSheet = idReturnSheet;
            $scope.filters.sgpRelationship = 2;
            $scope.filters.idRegiaoCompra = returnSheet.idRegiaoCompra;
            $scope.filters.cdSistema = cdSistema;

        }

        $scope.data = { values: null, podeSerEditada: podeSerEditada, jaComecou: jaComecou, returnSheet: {} };
        $scope.data.paging = $stateParams.paging || { offset: 0, limit: 50, orderBy: 'dsItem, cdItem' };

        $scope.back = back;
        $scope.search = search;
        $scope.select = select;
        $scope.clear = clear;
        $scope.orderBy = orderBy;
        $scope.atualizar = false;

        populaReturn(idReturnSheet);

        if ($scope.filters.didSearch) {
            search();
        }

        function populaReturn(idReturnSheet) {
            $q.when(returnSheetService.obter(idReturnSheet))
                .then(function (data) {
                    if (data !== null) {
                        $scope.data.returnSheet = data;
                    }
                });
        }

        function search(pageNumber) {

            if (!$validation.validate($scope)) return;

            pagingService.calculateOffset($scope.data.paging, pageNumber);

            var relacionamentoSGP = $scope.filters.sgpRelationship;
            var cdDepartamento = $scope.filters.cdDepartamento;
            var cdItemDetalhe = $scope.filters.cdOldNumber || '';
            var cdV9D = $scope.filters.cdV9D || '';
            var idRegiaoCompra = $scope.filters.idRegiaoCompra;
            var cdSistema = $scope.filters.cdSistema;
            var idReturnSheet = $scope.filters.idReturnSheet;

            var deferred = $q
                .when(itemDetalheService.obterItensDetalheReturnSheet(relacionamentoSGP, cdDepartamento, cdItemDetalhe, cdV9D, idRegiaoCompra, cdSistema, idReturnSheet, $scope.data.paging))
                .then(applyValue)
                .catch(fail);
        }

        function applyValue(data) {
            $scope.data.values = data;
            pagingService.acceptPagingResults($scope.data.paging, data);
            $scope.filters.didSearch = true;
            $validation.accept($scope);
        }

        function fail(reason) {
            $scope.data.values = null;
            $scope.filters.didSearch = false;
        }

        function clear() {
            $scope.filters.cdOldNumber = null;
            $scope.filters.cdV9D = null;
            $scope.filters.sgpRelationship = 2;
            $scope.data.values = [];
            $scope.filters.didSearch = false;
        }

        function orderBy(field) {
            pagingService.toggleSorting($scope.data.paging, field);
            search();
        }

        function select(item) {
            $state.update({ id: idReturnSheet, cdSistema: $scope.filters.cdSistema, filters: $scope.filters, paging: $scope.data.paging });
            $state.go('pesquisaItensLojasValidasItemEdit', {
                'id': idReturnSheet,
                'cdItem': item.cdItem,
                'cdSistema': $scope.filters.cdSistema,
            });
        }

        function back() {
            $state.go('cadastroReturnSheetEdit');
        }
    }

    PesquisaItensReturnSheetRoute.$inject = ['$stateProvider'];
    function PesquisaItensReturnSheetRoute($stateProvider) {

        $stateProvider
            .state('pesquisaItensReturnSheetEdit', {
                url: '/pesquisaReturnSheet/itens/edit/:id/:cdSistema',
                templateUrl: 'Scripts/app/reabastecimento/returnSheet/pesquisa-itens-return-sheet.view.html',
                controller: 'PesquisaItensReturnSheetController',
                params: {
                    filters: null,
                    paging: null
                },
                resolve: {
                    idReturnSheet: ['$stateParams', function ($stateParams) {
                        return $stateParams.id;
                    }],
                    cdSistema: ['$stateParams', function ($stateParams) {
                        return $stateParams.cdSistema;
                    }],
                    jaComecou: ['$stateParams', 'ReturnSheetService', function ($stateParams, returnSheetService) {
                        return returnSheetService.jaComecou($stateParams.id);
                    }],
                    podeSerEditada: ['$stateParams', 'ReturnSheetService', function ($stateParams, returnSheetService) {
                        return returnSheetService.podeSerEditada($stateParams.id);
                    }],
                    returnSheet: ['$stateParams', 'ReturnSheetService', function ($stateParams, returnSheetService) {
                        return returnSheetService.obter($stateParams.id);
                    }]
                }
            });
    }
})();