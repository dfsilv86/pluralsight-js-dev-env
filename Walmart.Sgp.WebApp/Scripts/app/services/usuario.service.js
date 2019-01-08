(function () {
    'use strict';

    angular
        .module('SGP')
        .service('UsuarioService', UsuarioService);

    UsuarioService.$inject = ['$q', '$http', 'ApiEndpoints'];

    function UsuarioService($q, $http, apiEndpoints) {

        function carregarDados(response) {
            return response.data;
        }

        var self = this;

        self.obterTodos = function (paging) {
            var params = apiEndpoints.createParams(paging, {});

            return $http
                .get(apiEndpoints.sgp.usuario, { params: params })
                .then(carregarDados);
        };

        self.importarUsuarios = function (senha) {
            return $http
                .post(apiEndpoints.sgp.usuario + "Importacao", { password: senha })
                .then(carregarDados);
        };

        // traz apenas userName, fullName e id
        self.obterPorUsuario = function (userName, isInitializing) {
            return $http
                .get(apiEndpoints.sgp.usuario + 'PorUsuario/' + apiEndpoints.encode(userName))
                .then(carregarDados);
        };

        // traz apenas userName, fullName e id
        self.obterPorId = function (id, isInitializing) {
            return $http
                .get(apiEndpoints.sgp.usuario + apiEndpoints.encode(id || ''))
                .then(carregarDados);
        };

        // traz apenas userName, fullName e id
        self.pesquisarPorUsuario = function (userName, fullName, email, cdUsuario, paging) {

            var params = apiEndpoints.createParams(paging, { userName: userName || '', fullName: fullName || '', cdUsuario: cdUsuario || '', email: email || '' });

            return $http
                .get(apiEndpoints.sgp.usuario, { params: params })
                .then(carregarDados);
        };
    }
})();