(function () {
    'use strict';


    // Configuração da controller
    angular
        .module('SGP')
        .config(ImportacaoUsuarioRoute)
        .controller('ImportacaoUsuarioController', ImportacaoUsuarioController);


    // Implementação da controller
    ImportacaoUsuarioController.$inject = ['$scope', '$q', 'ToastService', 'PagingService', 'ValidationService', 'UsuarioService'];
    function ImportacaoUsuarioController($scope, $q, toast, pagingService, $validation, usuarioService) {
        $validation.prepare($scope);
        $scope.data = { values: null };
        $scope.data.paging = { offset: 0, limit: 10, orderBy: 'fullName' };
        $scope.paginate = paginate;
        $scope.importarUsuarios = importarUsuarios;

        function importarUsuarios() {
            if (!$validation.validate($scope)) return;

            usuarioService.importarUsuarios($scope.data.senha)
            .then(function (qtd) {
                var msg;

                if (qtd == 0) {
                    msg = globalization.texts.noneUserImported;
                } else if (qtd == 1) {
                    msg = globalization.texts.oneUserImported;
                } else {
                    msg = globalization.texts.manyUserImported.format(qtd);
                }

                toast.success(msg);
                paginate(1);
            });
        }

        function exibe(data) {
            $scope.data.values = data;
            pagingService.acceptPagingResults($scope.data.paging, data);
        }

        function esconde(data) {
            $scope.data.values = [];
        }

        function paginate(pageNumber) {
            pagingService.calculateOffset($scope.data.paging, pageNumber);

            // Requisição ao serviço
            var deferred = $q
                .when(usuarioService.obterTodos($scope.data.paging))
                .then(exibe)
                .catch(esconde);
        }


        function orderBy(field) {
            pagingService.toggleSorting($scope.data.paging, field);
            paginate();
        }

        paginate(1);
    }

    // Configuração do estado
    ImportacaoUsuarioRoute.$inject = ['$stateProvider'];
    function ImportacaoUsuarioRoute($stateProvider) {

        $stateProvider
            .state('importacaoUsuario', {
                url: '/acessos/usuario/importacao',
                params: {
                    filters: null,
                    paging: null
                },
                templateUrl: 'Scripts/app/acessos/importacao-usuario.view.html',
                controller: 'ImportacaoUsuarioController'
            });
    }
})();