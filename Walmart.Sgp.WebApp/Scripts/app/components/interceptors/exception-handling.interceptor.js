(function () {
    'use strict';

    angular.module('SGP')
        .factory('ExceptionHandlingInterceptor', ['$q', '$log', '$injector', 'ToastService', function ($q, $log, $injector, toastService) {

            return {
                'responseError': function (rejection) {
                    switch (rejection.status) {
                        // API inacessível ou fora do ar.
                        case -1:
                        case 0:
                            toastService.error(globalization.texts.webApiUnavailable);
                            break;

                            // Sem permissões para acessar a rota.
                        case 401:
                            if ($injector.get('UserSessionService').hasStarted()) {
                                var urlParts = rejection.config.url.split('/');
                                var name = globalization.getText(urlParts[urlParts.length - 2], false);
                                var msg;

                                if (name === null) {
                                    msg = globalization.texts.noPermissionToAccessThisOperation;
                                }
                                else {
                                    msg = globalization.texts.noPermissionToAccess.format(name.toLowerCase());
                                }

                                toastService.warning(msg);
                            }
                            break;

                            // Não está autenticado.
                        case 403:
                            var $state = $injector.get('$state'), userSessionService = $injector.get('UserSessionService');
                            userSessionService.end();
                            userSessionService.pushSessionToResume($state.$current.toString(), angular.copy($state.$current.params));
                            $state.go('login');
                            break;

                        case 500:
                            if (rejection.data !== null) {                                
                                // Se rejection.data.message  não for nula, tenta localizar uma globalização para ela, 
                                // se não existir uma globalização, utiliza ela mesmo.
                                var rejectionMsg = rejection.data.message == null ? null : (globalization.getText(rejection.data.message.removeSpaces().removeDots(), false) || rejection.data.message);
                                var msg = rejectionMsg || globalization.getText('HttpStatus' + rejection.status.toString() + 'ErrorMessage');

                                toastService.error(msg);
                            }
                            break;
                        case 400:
                            if (rejection.data !== null) {
                                toastService.error(rejection.data.message || globalization.getText('HttpStatus' + rejection.status.toString() + 'ErrorMessage'));
                            }
                            break;
                        default:
                            if (rejection.data !== null) {
                                toastService.warning(rejection.data.message || globalization.getText('HttpStatus' + rejection.status.toString() + 'ErrorMessage'));
                            }
                            break;
                    }

                    return $q.reject(rejection);
                }
            };
        }]);
})();
