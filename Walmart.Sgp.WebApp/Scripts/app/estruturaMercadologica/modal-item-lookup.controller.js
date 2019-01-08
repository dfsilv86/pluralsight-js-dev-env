(function () {
    'use strict';

    angular
		.module('SGP')
		.controller('ModalItemLookupController', ModalItemLookupController);

    ModalItemLookupController.$inject = ['$scope', '$q', '$uibModalInstance', 'ValidationService', 'UserSessionService', 'PagingService', 'ItemDetalheService', 'sistema', 'item', 'plu', 'departamento', 'lockDepartamento'];

    function ModalItemLookupController($scope, $q, $uibModalInstance, $validation, userSession, pagingService, itemDetalheService, sistema, item, plu, departamento, lockDepartamento) {

        $validation.prepare($scope);

        $scope.filters = { cdSistema: sistema, cdDepartamento: departamento, cdCategoria: null, cdFineline: null, cdItem: item, dsItem: null, cdPlu: plu, cdStatus: null };
        $scope.data = { values: null };
        $scope.data.paging = { offset: 0, limit: 10, orderBy: "cdItem" };

        $scope.search = search;
        $scope.clear = clear;
        $scope.select = select;
        $scope.lockDepartamento = lockDepartamento;
        $scope.sortBy = sortBy;

        if (lockDepartamento) {
            $scope.filters.cdItem = null;
        }

        function search(pageNumber) {
            if (!$validation.validate($scope)) return;

            var idUsuario = userSession.getCurrentUser().id;

            pagingService.calculateOffset($scope.data.paging, pageNumber);

            var cdItem = $scope.filters.cdItem;
            var cdPlu = $scope.filters.cdPlu;
            var dsItem = $scope.filters.dsItem;
            var tpStatus = $scope.filters.tpStatus;
            var cdFineLine = $scope.filters.cdFineLine;
            var cdSubcategoria = null; // deve ter sido removido da tela no legado
            var cdCategoria = $scope.filters.cdCategoria;
            var cdDepartamento = $scope.filters.cdDepartamento;
            var cdSistema = $scope.filters.cdSistema;
            var tpVinculado = null;

            var deferred = $q
                .when(itemDetalheService.pesquisarPorFiltro(cdItem, cdPlu, dsItem, tpStatus, cdFineLine, cdSubcategoria, cdCategoria, cdDepartamento, cdSistema, idUsuario, tpVinculado, $scope.data.paging))
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
            if (!lockDepartamento) {
                $scope.filters.cdSistema = null;
                $scope.filters.cdDepartamento = null;
            }

            $scope.filters.cdCategoria = null;
            $scope.filters.cdFineline = null;
            $scope.filters.cdItem = null;
            $scope.filters.dsItem = null;
            $scope.filters.cdPlu = null;
            $scope.filters.cdStatus = null;

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