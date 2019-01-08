(function () {
    'use strict';

    angular
		.module('SGP')
		.controller('ModalDepartamentoLookupController', ModalDepartamentoLookupController);

    ModalDepartamentoLookupController.$inject = ['$scope', '$q', '$uibModalInstance', 'ValidationService', 'UserSessionService', 'PagingService', 'DepartamentoService', 'sistema', 'departamento', 'modoPereciveis', 'excluirPadaria'];

    function ModalDepartamentoLookupController($scope, $q, $uibModalInstance, $validation, userSession, pagingService, departamentoService, sistema, departamento, modoPereciveis, excluirPadaria) {
        $validation.prepare($scope);

        $scope.filters = { cdSistema: sistema, cdDepartamento: departamento, cdDivisao: null, dsDepartamento: null, blPerecivel: true };
        $scope.data = { values: null };
        $scope.data.paging = { offset: 0, limit: 10, orderBy: null };

        $scope.search = search;
        $scope.clear = clear;
        $scope.select = select;
        $scope.modoPereciveis = modoPereciveis;

        function search(pageNumber) {

            if (!$validation.validate($scope)) return;

            pagingService.calculateOffset($scope.data.paging, pageNumber);

            var idUsuario = userSession.getCurrentUser().id;

            var cdSistema = $scope.filters.cdSistema;
            var cdDepartamento = $scope.filters.cdDepartamento;
            var dsDepartamento = $scope.filters.dsDepartamento;
            var cdDivisao = $scope.filters.cdDivisao;
            var blPerecivel = $scope.filters.blPerecivel || (modoPereciveis == 'restrito');

            var deferred = $q
                .when(departamentoService.pesquisarPorDivisaoESistema(cdDepartamento, dsDepartamento, blPerecivel, cdDivisao, cdSistema, excluirPadaria, $scope.data.paging))
                .then(applyValue);
        }

        function applyValue(data) {
            $scope.data.values = data;

            pagingService.acceptPagingResults($scope.data.paging, data);
        }

        function clear() {
            $scope.filters.cdSistema = null;
            $scope.filters.cdDepartamento = null;
            $scope.filters.dsDepartamento = null;
            $scope.filters.cdDivisao = null;
            $scope.filters.blPerecivel = true;
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