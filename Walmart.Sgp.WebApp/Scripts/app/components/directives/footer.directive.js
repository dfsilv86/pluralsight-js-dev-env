(function () {
    'use strict';

    angular
        .module('SGP')
        .directive('footer', FooterDirective);

    function FooterDirective() {
        return {
            restrict: 'E',
            templateUrl: 'Scripts/app/components/directives/footer.template.html',
            controller: ['$scope', '$state', 'UserSessionService', 'SpaConfig', '$timeout', 'ProcessingService', FooterController],
            scope: {}
        };
    }

    FooterController.$inject = ['$scope', '$state', 'UserSessionService', 'SpaConfig', '$timeout', 'ProcessingService'];

    function FooterController($scope, $state, userSession, spaConfig, $timeout, processingService) {

        var longAppVersion = spaConfig.appVersion || '!!';
        var shortAppVersion = longAppVersion.split('.').slice(0, 3).join('.');

        $scope.footer = { show: false, isLoggedIn: !!userSession.hasStarted(), isLogin: $state.is('login'), appVersion: shortAppVersion };

        $scope.lock = function () {
            userSession.lock(true);
        };

        $scope.toggleShortVersion = function () {
            $scope.footer.appVersion = $scope.footer.appVersion == longAppVersion ? shortAppVersion : longAppVersion;
            $('.app-version').text('v{0}'.format($scope.footer.appVersion));
        };

        $scope.viewProcessingManager = function () {
            processingService.viewManager();
        };

        $scope.$on('session-start', sessionModified);
        $scope.$on('session-end', sessionModified);
        $scope.$on('$stateChangeStart', stateChange);
        $scope.$on('$stateChangeSuccess', stateChange);
        $scope.$on('$stateChangeError', stateChange);
        $scope.$on('$stateChangeNotFound', stateChange);
        $scope.$on('$viewContentLoaded', stateChange);

        function sessionModified() {
            $scope.footer.isLoggedIn = userSession.hasStarted();
            apply();
        }

        function stateChange() {
            $scope.footer.isLogin = $state.is('login');
            apply();
        }

        function apply() {
            $scope.footer.show = !$scope.footer.isLogin && $scope.footer.isLoggedIn;
        }

        $timeout(apply, 500); // suppress-validator
    }
})();