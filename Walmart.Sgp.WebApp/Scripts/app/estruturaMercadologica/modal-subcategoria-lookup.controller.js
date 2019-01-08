(function () {
    'use strict';

    angular
		.module('SGP')
		.controller('ModalSubcategoriaLookupController', ModalSubcategoriaLookupController);

    ModalSubcategoriaLookupController.$inject = ['$scope', '$q', '$uibModalInstance', 'ValidationService', 'UserSessionService', 'PagingService', 'SubcategoriaService', 'sistema', 'departamento', 'categoria', 'subcategoria', 'requiredOptions'];

    function ModalSubcategoriaLookupController($scope, $q, $uibModalInstance, $validation, userSession, pagingService, subcategoriaService, sistema, departamento, categoria, subcategoria, requiredOptions) {

        $validation.prepare($scope);

        $scope.filters = { cdSistema: sistema, cdDepartamento: departamento, cdCategoria: categoria, cdSubcategoria: subcategoria, dsSubcategoria: null };
        $scope.data = { values: null };
        $scope.data.paging = { offset: 0, limit: 10, orderBy: null };
        $scope.requiredOptions = {
            categoria: requiredOptions.categoria != false,
            departamento: requiredOptions.departamento != false
        };

        $scope.search = search;
        $scope.clear = clear;
        $scope.select = select;

        function search(pageNumber) {

            if (!$validation.validate($scope)) return;

            pagingService.calculateOffset($scope.data.paging, pageNumber);

            var cdSistema = $scope.filters.cdSistema;
            var cdDepartamento = $scope.filters.cdDepartamento;
            var cdCategoria = $scope.filters.cdCategoria;
            var cdSubcategoria = $scope.filters.cdSubcategoria;
            var dsSubcategoria = $scope.filters.dsSubcategoria;

            var deferred = $q
                .when(subcategoriaService.pesquisarPorSubcategoriaCategoriaDepartamentoESistema(cdSubcategoria, dsSubcategoria, cdCategoria, cdDepartamento, cdSistema, $scope.data.paging))
                .then(applyValue);
        }

        function applyValue(data) {
            $scope.data.values = data;

            pagingService.acceptPagingResults($scope.data.paging, data);
        }

        function clear() {
            $scope.filters.cdSistema = null;
            $scope.filters.cdDepartamento = null;
            $scope.filters.cdCategoria = null;
            $scope.filters.dsSubcategoria = null;
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