(function () {
    'use strict';

    var webGuardianCodigoError = {
        senhaExpirada: 14
    };

    angular
        .module('SGP')
        .service('AuthService', AuthService);

    AuthService.$inject = ['$http', 'UserSessionService', 'ApiEndpoints', 'ToastService', '$q', 'ToastService'];

    function AuthService($http, userSession, apiEndpoints, toastService, $q, $toast) {

        this.authenticate = function (username, password, idExternoPapel, idLoja) {

            var params = { UserName: username, Password: password, IdExternoPapel: idExternoPapel, IdLoja: idLoja };

            return $http
                .post(apiEndpoints.sgp.login, params)
                .then(function startSession(response) {
                    var data = response.data;

                    if (data.user.hasPermissions) {
                        data.user.idExternoPapel = idExternoPapel;
                        userSession.start(data);
                    }

                    return data;
                }, function failed(error) {
                    if (error.data && error.data.data && error.data.data.ErrorCode === webGuardianCodigoError.senhaExpirada) {
                        userSession.changePassword(username);
                        $toast.error(error.data.message);
                    }
                    return $q.reject(error);
                });
        };

        this.changePassword = function (request) {

            var params = request;

            return $http
                .put(apiEndpoints.sgp.login, params)
                .then(function () {
                    $toast.success(globalization.texts.passwordSuccessfullyChanged);
                });
        };
    }

})();