(function () {
    'use strict';

    angular
		.module('SGP')
		.controller('ModalCategoriaLookupController', ModalCategoriaLookupController);

    ModalCategoriaLookupController.$inject = ['$scope', '$q', '$uibModalInstance', 'ValidationService', 'UserSessionService', 'PagingService', 'CategoriaService', 'sistema', 'departamento', 'categoria', 'requiredOptions'];

    function ModalCategoriaLookupController($scope, $q, $uibModalInstance, $validation, userSession, pagingService, categoriaService, sistema, departamento, categoria, requiredOptions) {

        $validation.prepare($scope);

        $scope.filters = { cdSistema: sistema, cdDepartamento: departamento, cdCategoria: categoria, dsCategoria: null };
        $scope.data = { values: null };
        $scope.data.paging = { offset: 0, limit: 10, orderBy: null };
        $scope.requiredOptions = {            
            departamento: requiredOptions.departamento != false
        };

        $scope.search = search;
        $scope.clear = clear;
        $scope.select = select;

        function search(pageNumber) {

            if (!$validation.validate($scope)) return;

            pagingService.calculateOffset($scope.data.paging, pageNumber);

            var idUsuario = userSession.getCurrentUser().id;

            var cdSistema = $scope.filters.cdSistema;
            var cdDepartamento = $scope.filters.cdDepartamento;
            var cdCategoria = $scope.filters.cdCategoria;
            var dsCategoria = $scope.filters.dsCategoria;

            var deferred = $q
                .when(categoriaService.pesquisarPorCategoriaDepartamentoESistema(cdCategoria, dsCategoria, cdSistema, cdDepartamento, $scope.data.paging))
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
            $scope.filters.dsCategoria = null;
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