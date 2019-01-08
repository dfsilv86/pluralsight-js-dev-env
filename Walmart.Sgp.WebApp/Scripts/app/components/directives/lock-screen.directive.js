(function () {
    'use strict';

    angular
        .module('common')
        .directive('lockScreen', LockScreenDirective);

    function LockScreenDirective() {
        return {
            restrict: 'E',
            priority: 10,
            scope: {},
            terminal: true,
            templateUrl: 'Scripts/app/components/directives/lock-screen.template.html',
            controller: ['$rootScope', '$scope', 'UserSessionService', 'AuthService', 'ValidationService', '$log', '$state', 'SpaConfig', '$timeout', LockScreenController]
        }
    }

    function LockScreenController($rootScope, $scope, userSession, authService, $validation, $log, $state, spaConfig, $timeout) {

        $validation.prepare($scope);

        function reset() {
            $scope.user = {};
            $scope.data = {};
            $scope.settings = { isLock: false, isChangePassword: false, isLogin: false, blockUserName: false };
        }

        function proceed(data) {
            // Já possui permissões?            
            if (data.user.hasPermissions) {
                resumeOrGoHome();
            }
            else {
                // Se ainda não possui permissões, então deve selecionar um papel da lista.
                $scope.data.papeis = data.user.papeis;
                $scope.data.lojas = data.user.lojas;

                // Se tiver apenas um papel, seleciona o primeiro.
                if (data.user.papeis.length == 1) {
                    $scope.user.idExternoPapel = data.user.papeis[0].idExterno;
                }

                // Se tiver apenas uma loja, seleciona a primeira.
                if (data.user.lojas != null && data.user.lojas.length == 1) {
                    $scope.user.idLoja = data.user.lojas[0].id;
                }
            }
        }

        function resumeOrGoHome() {

            close();

            var resumeTo = userSession.popSessionToResume();

            if (!!resumeTo) {
                $state.resetAndRebuildStack(resumeTo.name, resumeTo.params);
                $state.go(resumeTo.name, resumeTo.params);
            } else {
                $state.go('home');
            }

            $rootScope.$broadcast('login-inicial-sucesso');
        }

        function unlock() {

            if (!$validation.validate($scope, 'formUnlock')) return;

            authService
                .authenticate($scope.user.userName, $scope.user.password, $scope.user.idExternoPapel, $scope.user.idLoja)
                .then(close); // finally?
        }

        function login() {

            if (!$validation.validate($scope, 'formLogin')) return;

            authService
                .authenticate($scope.user.userName, $scope.user.password, $scope.user.idExternoPapel, $scope.user.idLoja)
                .then(proceed); // finally?
        }

        function logout() {
            userSession.end();
        }

        function changePassword() {

            if (!$validation.validate($scope, 'formChangePassword')) return;

            authService
                .changePassword($scope.user)
                .then(close);
        }

        $scope.unlock = unlock;
        $scope.login = login;
        $scope.logout = logout;
        $scope.changePassword = changePassword;
        $scope.close = close;

        function selectFocusables() {
            return $('#container input, #container select, #container button, div.header a');
        }

        function close() {
            var elem = $('lock-screen');
            $('body').toggleClass('is-locked', false);
            elem.hide();
            reset();
            selectFocusables().off('focus.lockScreen');
            $scope.lockNotice = null;

            $timeout(function () { // suppress-validator
                if (!userSession.hasStarted() && !$scope.settings.isChangePassword) {
                    open();
                }
            }, 500);
        }

        function open(event, params) {

            $scope.settings.isLogin = !event || event.name == 'session-end';
            $scope.settings.isLocked = !!event && event.name == 'lock-screen';
            $scope.settings.isChangePassword = !!event && event.name == 'change-password';
            $('[ui-select2]').select2('close');
            $scope.lockNotice = ((!!params && !!params.isManual) ? globalization.texts.lockNotice3 : globalization.texts.lockNotice1.format(spaConfig.activityTimeout)) + ' ' + globalization.texts.lockNotice2;
            var currentUser = userSession.getCurrentUser() || (!!params ? { userName: params.userName } : {});
            $scope.settings.blockUserName = !!currentUser && !!currentUser.userName;
            $scope.settings.canCancel = userSession.hasStarted() && (!params || (!!params && !params.userName));
            $scope.user = { userName: currentUser.userName, idLoja: currentUser.idLoja, idExternoPapel: currentUser.idExternoPapel };
            var elem = $('lock-screen');
            $('body').toggleClass('is-locked', !$scope.settings.isLogin);
            elem.show();
            selectFocusables().on('focus.lockScreen', function () { $('lock-screen input:first-of-type').focus(); });
        }

        close();

        $scope.$on('lock-screen', open);

        $scope.$on('change-password', open);

        $scope.$on('session-start', close);
        $scope.$on('session-end', close);

        $scope.$on('$destroy', function () {
            selectFocusables().off('focus.lockScreen');
        });
    }
})();