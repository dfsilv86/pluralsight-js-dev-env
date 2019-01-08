(function () {
    'use strict';

    angular
        .module('common')
        .directive('applicationBreadcrumbs', ApplicationBreadcrumbs);

    ApplicationBreadcrumbs.$inject = ['UserSessionService', '$state'];

    function ApplicationBreadcrumbs(userSession, $state) {
        return {
            restrict: 'E',
            replace: true,
            templateUrl: 'Scripts/common/directives/application-breadcrumbs.template.html',
            controller: ['$scope', '$state', ApplicationBreadcrumbsController]
        };
    }

    ApplicationBreadcrumbsController.$inject = ['$scope', '$state'];

    function ApplicationBreadcrumbsController($scope, $state) {

        $scope.stack = $state.getNavigationStack();

        $scope.goBackToIndex = function (index) {
            $state.goBack($scope.stack.length - (index + 1));
        };
    }
})();