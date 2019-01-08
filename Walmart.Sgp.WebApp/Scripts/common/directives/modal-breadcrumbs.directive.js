(function () {
    'use strict';

    angular
        .module('common')
        .directive('modalBreadcrumbs', ModalBreadcrumbs);

    ModalBreadcrumbs.$inject = ['BreadcrumbService'];
    function ModalBreadcrumbs(breadcrumbService) {
        return {
            restrict: 'E',
            replace: true,
            template: '<div class="crumbs"><ul class="breadcrumb"></ul></div>',
            scope: {
                api: '='
            },
            link: ModalBreadcrumbsLink
        };

        function ModalBreadcrumbsLink($scope, element) {

            var elem = $(element);

            breadcrumbService.register(elem);

            $scope.$on('$destroy', unregister);

            function unregister() {
                breadcrumbService.unregister(elem);
            }
        }
    }
})();