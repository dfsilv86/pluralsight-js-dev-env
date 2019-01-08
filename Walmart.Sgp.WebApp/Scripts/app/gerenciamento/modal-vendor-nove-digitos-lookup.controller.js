(function () {
    'use strict';

    angular
		.module('SGP')
		.controller('ModalVendorNoveDigitosLookupController', ModalVendorNoveDigitosLookupController);

    ModalVendorNoveDigitosLookupController.$inject = ['$scope', '$q', '$uibModalInstance', 'ValidationService', 'PagingService', 'FornecedorParametroService', 'sistema', 'cdV9D'];

    function ModalVendorNoveDigitosLookupController($scope, $q, $uibModalInstance, $validation, pagingService, fornecedorParametroService, sistema, cdV9D) {

        $validation.prepare($scope);

        $scope.filters = { cdSistema: sistema, cdV9D: cdV9D, nmFornecedor: null };
        $scope.data = { values: null };
        $scope.data.paging = { offset: 0, limit: 10, orderBy: 'nmFornecedor' };

        $scope.search = search;
        $scope.clear = clear;
        $scope.select = select;

        function search(pageNumber) {

            if (!$validation.validate($scope)) return;

            pagingService.calculateOffset($scope.data.paging, pageNumber);

            //var idUsuario = userSession.getCurrentUser().id;

            var cdSistema = $scope.filters.cdSistema;
            var cdV9D = $scope.filters.cdV9D;
            var nmFornecedor = $scope.filters.nmFornecedor;
            var cdTipo = null;

            var deferred = $q
                .when(fornecedorParametroService.pesquisarPorSistemaCodigo9DigitosENomeFornecedor(cdSistema, cdTipo, cdV9D, nmFornecedor, $scope.data.paging))
                .then(applyValue);
        }

        function applyValue(data) {
            $scope.data.values = data;

            pagingService.acceptPagingResults($scope.data.paging, data);
        }

        function clear() {
            $scope.filters.cdSistema = null;
            $scope.filters.cdV9D = null;
            $scope.filters.nmFornecedor = null;
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