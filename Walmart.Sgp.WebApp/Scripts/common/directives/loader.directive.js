(function () {
    'use strict';

    angular
        .module('common')
        .directive('loader', Loader);

    Loader.$inject = ['LoaderService'];

    function Loader(loaderService) {
        return {
            restrict: 'E',
            replace: true,

            templateUrl: 'Scripts/common/directives/loader.template.html',
            scope: {
            },
            link: LoaderLink
        };

        function LoaderLink($scope, element) {

            var elem = $(element);

            loaderService.register(elem);

            $scope.$on('$destroy', unregister);

            function unregister() {
                loaderService.unregister(elem);
            }
        }
    }
})();