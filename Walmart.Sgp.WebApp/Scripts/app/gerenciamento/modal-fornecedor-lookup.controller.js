(function () {
    'use strict';

    angular
		.module('SGP')
		.controller('ModalFornecedorLookupController', ModalFornecedorLookupController);

    ModalFornecedorLookupController.$inject = ['$scope', '$q', '$uibModalInstance', 'ValidationService', 'UserSessionService', 'PagingService', 'FornecedorService', 'sistema', 'fornecedor'];

    function ModalFornecedorLookupController($scope, $q, $uibModalInstance, $validation, userSession, pagingService, fornecedorService, sistema, fornecedor) {

        $validation.prepare($scope);

        $scope.filters = { cdSistema: sistema, cdFornecedor: fornecedor, nmFornecedor: null };
        $scope.data = { values: null };
        $scope.data.paging = { offset: 0, limit: 10, orderBy: null };

        $scope.search = search;
        $scope.clear = clear;
        $scope.select = select;

        function search(pageNumber) {

            if (!$validation.validate($scope)) return;

            pagingService.calculateOffset($scope.data.paging, pageNumber);

            var cdSistema = $scope.filters.cdSistema;
            var cdFornecedor = $scope.filters.cdFornecedor;
            var nmFornecedor = $scope.filters.nmFornecedor;

            var deferred = $q
                .when(fornecedorService.obterListaPorSistemaCodigoNome(cdSistema, cdFornecedor, nmFornecedor, $scope.data.paging))
                .then(applyValue);
        }

        function applyValue(data) {
            $scope.data.values = data;

            pagingService.acceptPagingResults($scope.data.paging, data);
        }

        function clear() {
            $scope.filters.cdSistema = null;
            $scope.filters.nmFornecedor = null;            
            $scope.filters.cdFornecedor = null;
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