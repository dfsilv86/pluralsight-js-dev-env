(function () {
    'use strict';

    angular
		.module('SGP')
		.controller('ModalFineLineLookupController', ModalFineLineLookupController);

    ModalFineLineLookupController.$inject = ['$scope', '$q', '$uibModalInstance', 'ValidationService', 'UserSessionService', 'PagingService', 'FineLineService', 'sistema', 'departamento', 'categoria', 'subcategoria', 'fineLine', 'requiredOptions'];

    function ModalFineLineLookupController($scope, $q, $uibModalInstance, $validation, userSession, pagingService, fineLineService, sistema, departamento, categoria, subcategoria, fineLine, requiredOptions) {
        
        $validation.prepare($scope);

        $scope.filters = { cdSistema: sistema, cdDepartamento: departamento, cdCategoria: categoria, cdSubcategoria: subcategoria, cdFineLine: fineLine, dsFineLine: null };
        $scope.data = { values: null };
        $scope.data.paging = { offset: 0, limit: 10, orderBy: 'IDFineline' };
        $scope.requiredOptions = {
            categoria: requiredOptions.categoria == true,
            departamento: requiredOptions.departamento == true
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
            var cdFineLine = $scope.filters.cdFineLine;
            var dsFineLine = $scope.filters.dsFineLine;

            var deferred = $q
                .when(fineLineService.pesquisarPorFineLineSubcategoriaCategoriaDepartamentoESistema(cdFineLine, dsFineLine, cdSubcategoria, cdCategoria, cdDepartamento, cdSistema, $scope.data.paging))
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
            $scope.filters.dsFineLine = null;
            $scope.filters.cdSubcategoria = null;
            $scope.filters.cdFineLine = null;
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