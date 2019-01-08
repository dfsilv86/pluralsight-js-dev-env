(function () {
    'use strict';

    angular
        .module('SGP')
        .config(HomeRoute)
        .controller('HomeController', HomeController);

    HomeController.$inject = ['$scope'];

    function HomeController($scope) {
    }

    HomeRoute.$inject = ['$stateProvider'];

    function HomeRoute($stateProvider) {

        $stateProvider
            .state('home', {
                url: '/',
                templateUrl: 'Scripts/app/home/home.view.html',
                controller: 'HomeController'
            });
    }

})();