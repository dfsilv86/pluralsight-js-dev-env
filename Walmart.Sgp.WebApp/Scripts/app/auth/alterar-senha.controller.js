(function () {
    'use strict';

    angular
        .module('SGP')
        .config(LoginRoute)
        .controller('AlterarSenhaController', AlterarSenhaController);

    AlterarSenhaController.$inject = ['$scope', 'UserSessionService', '$timeout'];

    function AlterarSenhaController($scope, userSession, $timeout) {
        $timeout(function () { // suppress-validator
            userSession.changePassword();
            if (!userSession.hasStarted()) {
                $('body').toggleClass('is-locked', false);
            }
        }, 500);
    }

    LoginRoute.$inject = ['$stateProvider', '$urlRouterProvider'];

    function LoginRoute($stateProvider) {

        $stateProvider
            .state('alterarSenha', {
                url: '/alterar-senha',
                params: { userName: null },
                templateUrl: 'Scripts/app/home/home.view.html',
                controller: 'AlterarSenhaController'
            });
    }
})();