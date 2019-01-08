(function () {
    'use strict';

    angular
        .module('SGP')
        .config(LoginRoute)
        .controller('LoginController', LoginController);

    LoginController.$inject = ['$scope', 'UserSessionService'];

    function LoginController($scope, userSession) {
        userSession.end();
    }

    LoginRoute.$inject = ['$stateProvider', '$urlRouterProvider'];

    function LoginRoute($stateProvider) {

        $stateProvider
            .state('login', {
                url: '/login',
                templateUrl: 'Scripts/app/home/home.view.html',
                controller: 'LoginController'
            });
    }
})();