(function () {
    'use strict';

    angular
        .module('SGP')
        .directive('header', Header);


    function Header() {
        return {
            restrict: 'E',
            templateUrl: 'Scripts/app/components/directives/header.template.html',
            controller: ['$scope', '$state', 'UserSessionService', '$timeout', HeaderController],
            scope: {}
        };
    }

    HeaderController.$inject = ['$scope', '$state', 'UserSessionService', '$timeout'];

    function HeaderController($scope, $state, userSession, $timeout) {

        $scope.header = { show: false, isLoggedIn: !!userSession.hasStarted(), isLogin: $state.is('login') };

        $scope.$on('session-start', sessionModified);
        $scope.$on('session-end', sessionModified);
        $scope.$on('$stateChangeStart', stateChange);
        $scope.$on('$stateChangeSuccess', stateChange);
        $scope.$on('$stateChangeError', stateChange);
        $scope.$on('$stateChangeNotFound', stateChange);
        $scope.$on('$viewContentLoaded', stateChange);

        function sessionModified() {
            $scope.header.isLoggedIn = userSession.hasStarted();
            apply();
        }

        function stateChange() {
            $scope.header.isLogin = $state.is('login');
            apply();
        }

        function apply() {
            $scope.header.show = !$scope.header.isLogin && $scope.header.isLoggedIn;
        }

        $timeout(apply, 500); // suppress-validator
    }
})();