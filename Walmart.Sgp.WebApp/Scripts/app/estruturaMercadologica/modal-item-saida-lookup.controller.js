(function () {
    'use strict';

    angular
		.module('SGP')
		.controller('ModalItemSaidaLookupController', ModalItemSaidaLookupController);

    ModalItemSaidaLookupController.$inject = ['$scope', '$q', '$uibModalInstance', 'ValidationService', 'UserSessionService', 'PagingService', 'ItemDetalheService', 'sistema', 'fornecedorParam', 'regiao', 'departamento', 'somenteStatus', 'modoPereciveis'];

    function ModalItemSaidaLookupController($scope, $q, $uibModalInstance, $validation, userSession, pagingService, itemDetalheService, sistema, fornecedorParam, regiao, departamento, somenteStatus, modoPereciveis) {

        $validation.prepare($scope);

        $scope.filters = { blPerecivel: null, cdSistema: sistema, cdDepartamento: departamento, cdCategoria: null, cdFineLine: null, cdItem: null, dsItem: null, cdPlu: null, idFornecedorParam: fornecedorParam, cdSubcategoria: null, IDRegiaoCompra: regiao, tpStatus: somenteStatus };
        $scope.data = { values: null, bloquearStatus: false, modoPereciveis: modoPereciveis };
        $scope.data.paging = { offset: 0, limit: 10, orderBy: "cdItem" };

        if (somenteStatus) {
            $scope.data.bloquearStatus = true;
        }

        if (modoPereciveis == 'restrito') {
            $scope.filters.blPerecivel = 'S';
        }

        $scope.search = search;
        $scope.clear = clear;
        $scope.select = select;
        $scope.sortBy = sortBy;

        function search(pageNumber) {

            if (!$validation.validate($scope)) return;

            pagingService.calculateOffset($scope.data.paging, pageNumber);

            var deferred = $q
                .when(itemDetalheService.obterListaItemSaidaPorFornecedorItemEntrada($scope.filters, $scope.data.paging))
                .then(applyValue);
        }

        function sortBy(columnName) {
            pagingService.toggleSorting($scope.data.paging, columnName);
            search();
        }

        function applyValue(data) {
            $scope.data.values = data;

            pagingService.acceptPagingResults($scope.data.paging, data);
        }

        function clear() {
            $scope.filters.cdSistema = null;
            $scope.filters.cdDepartamento = null;
            $scope.filters.cdCategoria = null;
            $scope.filters.cdSubcategoria = null;
            $scope.filters.cdFineLine = null;
            $scope.filters.cdItem = null;
            $scope.filters.dsItem = null;
            $scope.filters.cdPlu = null;
            $scope.filters.cdStatus = null;
            $scope.filters.cdSubcategoria = null;

            $scope.data.values = [];
        }

        function select(item) {
            if ((item || null) === null) {
                $uibModalInstance.dismiss();
            } else {
                $uibModalInstance.close(item);
            }
        }
    }
})();