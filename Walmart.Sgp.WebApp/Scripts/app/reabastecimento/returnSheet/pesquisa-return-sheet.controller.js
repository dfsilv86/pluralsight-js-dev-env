(function () {
    'use strict';

    angular
          .module('SGP')
          .config(PesquisaReturnSheetRoute)
          .controller('PesquisaReturnSheetController', PesquisaReturnSheetController);


    PesquisaReturnSheetController.$inject = ['$scope', '$q', '$stateParams', '$state', 'StackableModalService', 'ReturnSheetService', 'ToastService', 'ValidationService', 'PagingService'];
    function PesquisaReturnSheetController($scope, $q, $stateParams, $state, $modal, returnSheetService, toastService, $validation, pagingService) {

        initialize();

        function initialize() {
            $validation.prepare($scope);

            $scope.data = { values: null };
            $scope.data.paging = $stateParams.paging || { offset: 0, limit: 50, orderBy: null };

            $scope.filters = $stateParams.filters || { somenteAtivos: null, event: null, finalReturn: null, inicioReturn: null, cdDepartamento: null, idRegiaoCompra: null };

            $scope.search = search;
            $scope.clear = clear;
            $scope.new = doNew;
            $scope.orderBy = orderBy;
            $scope.edit = edit;

            if ($scope.filters.didSearch) {
                search();
            }
        }

        function orderBy(field) {
            pagingService.toggleSorting($scope.data.paging, field);
            search();
        }

        function edit(item) {

            $state.update({ filters: $scope.filters, paging: $scope.data.paging });
            $state.go('cadastroReturnSheetEdit', {
                'id': item.idReturnSheet
            });
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

        function search(pageNumber) {

            //if (!$validation.validate($scope)) return;

            if (!validaCampos()) {
                return;
            }

            pagingService.calculateOffset($scope.data.paging, pageNumber);

            var inicioReturn = $scope.filters.inicioReturn || '';
            var finalReturn = $scope.filters.finalReturn || '';
            var idDepartamento = ($scope.filters.departamento || {}).idDepartamento || '';
            var idRegiaoCompra = $scope.filters.idRegiaoCompra || '';
            var evento = $scope.filters.evento || '';
            var filtroAtivos = $scope.filters.somenteAtivos || 1;

            var deferred = $q
                  .when(returnSheetService.pesquisar(inicioReturn, finalReturn, evento, idDepartamento, filtroAtivos, idRegiaoCompra, $scope.data.paging))
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
            $scope.data.values = null;
            $scope.filters.didSearch = false;
        }

        function clear() {
            $scope.filters.somenteAtivos = $scope.filters.evento = $scope.filters.finalReturn = $scope.filters.inicioReturn = $scope.filters.cdDepartamento = $scope.filters.idRegiaoCompra = null;
            $scope.data.values = null;
            $scope.data.paging.offset = 0;
            $scope.filters.didSearch = false;
        }

        function doNew() {
            $state.update({ filters: $scope.filters, paging: $scope.data.paging });
            $state.go('cadastroReturnSheetNew');
        }
    }

    PesquisaReturnSheetRoute.$inject = ['$stateProvider'];
    function PesquisaReturnSheetRoute($stateProvider) {

        $stateProvider
                .state('pesquisaReturnSheet', {
                    url: '/pesquisaReturnSheet',
                    templateUrl: 'Scripts/app/reabastecimento/returnSheet/pesquisa-return-sheet.view.html',
                    controller: 'PesquisaReturnSheetController',
                    params: {
                        filters: null,
                        paging: null
                    },
                });
    }
})();