(function () {
    'use strict';

    angular
		.module('SGP')
		.controller('ModalDivisaoLookupController', ModalDivisaoLookupController);

    ModalDivisaoLookupController.$inject = ['$scope', '$q', '$uibModalInstance', 'ValidationService', 'PagingService', 'DivisaoService', 'sistema', 'divisao'];

    function ModalDivisaoLookupController($scope, $q, $uibModalInstance, $validation, pagingService, divisaoService, sistema, divisao) {

        $validation.prepare($scope);

        $scope.filters = { cdSistema: sistema, cdDivisao: divisao };
        $scope.data = { values: null };
        $scope.data.paging = { offset: 0, limit: 10, orderBy: null };

        $scope.search = search;
        $scope.clear = clear;
        $scope.select = select;

        function search(pageNumber) {

            if (!$validation.validate($scope)) return;

            pagingService.calculateOffset($scope.data.paging, pageNumber);

            //var idUsuario = userSession.getCurrentUser().id;

            var cdSistema = $scope.filters.cdSistema;
            var cdDivisao = $scope.filters.cdDivisao;
            var dsDivisao = $scope.filters.dsDivisao;

            var deferred = $q
                .when(divisaoService.pesquisarPorDivisaoESistema(cdDivisao, dsDivisao, cdSistema, $scope.data.paging))
                .then(applyValue);
        }

        function applyValue(data) {
            $scope.data.values = data;

            pagingService.acceptPagingResults($scope.data.paging, data);
        }

        function clear() {
            $scope.filters.cdSistema = null;
            $scope.filters.dsDivisao = null;
            $scope.filters.cdDivisao = null;
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