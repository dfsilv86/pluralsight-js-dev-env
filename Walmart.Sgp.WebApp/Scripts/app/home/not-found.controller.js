(function () {
    'use strict';

    angular
        .module('SGP')
        .config(NotFoundRoute)
        .controller('NotFoundController', NotFoundController);

    NotFoundController.$inject = ['$scope', '$stateParams', 'translateFilter'];

    function NotFoundController($scope, $stateParams, translateFilter) {
        if (($stateParams.url || '').length > 0) {
            $scope.badUrlMsg = translateFilter('notFound2').format($stateParams.url || '');
        }
    }

    NotFoundRoute.$inject = ['$stateProvider'];

    function NotFoundRoute($stateProvider) {

        $stateProvider
            .state('notFound', {
                url: '/404?url',
                templateUrl: 'Scripts/app/home/not-found.view.html',
                controller: 'NotFoundController',
            });
        ;
    }

})();