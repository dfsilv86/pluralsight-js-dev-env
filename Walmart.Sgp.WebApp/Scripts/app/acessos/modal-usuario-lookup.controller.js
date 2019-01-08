(function () {
    'use strict';

    angular
		.module('SGP')
		.controller('ModalUsuarioLookupController', ModalUsuarioLookupController);

    ModalUsuarioLookupController.$inject = ['$scope', '$q', '$uibModalInstance', 'ValidationService', 'PagingService', 'UsuarioService', 'userName', 'fullName', 'usuario', 'email' ];

    function ModalUsuarioLookupController($scope, $q, $uibModalInstance, $validation, pagingService, usuarioService, userName, fullName, usuario, email) {

        $validation.prepare($scope);

        $scope.filters = { userName: userName, fullName: fullName, cdUsuario: usuario, email: email };
        $scope.data = { values: null };
        $scope.data.paging = { offset: 0, limit: 10, orderBy: null };

        $scope.search = search;
        $scope.clear = clear;
        $scope.select = select;

        function search(pageNumber) {

            if (!$validation.validate($scope)) return;

            pagingService.calculateOffset($scope.data.paging, pageNumber);

            var userName = null; //$scope.filters.userName;
            var fullName = $scope.filters.fullName;
            var cdUsuario = null; //$scope.filters.cdUsuario;
            var email = null; //$scope.filters.email;

            var deferred = $q
                .when(usuarioService.pesquisarPorUsuario(userName, fullName, email, cdUsuario, $scope.data.paging))
                .then(applyValue);
        }

        function applyValue(data) {
            $scope.data.values = data;

            pagingService.acceptPagingResults($scope.data.paging, data);
        }

        function clear() {
            $scope.filters.userName = null;
            $scope.filters.fullName = null;
            $scope.filters.cdUsuario = null;
            $scope.filters.email = null;
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