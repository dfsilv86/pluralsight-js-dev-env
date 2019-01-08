(function () {
    'use strict';

    angular
        .module('common')
        .directive('modalBreadcrumb', ModalBreadcrumb);

    ModalBreadcrumb.$inject = ['BreadcrumbService', 'StackableModalService'];
    function ModalBreadcrumb(breadcrumbService, stackableModalService) {
        return {
            restrict: 'E',
            scope: {
                title: '@',
                icon: '@'
            },
            link: ModalBreadcrumbLink
        };

        function ModalBreadcrumbLink($scope, element, attrs) {

            var currentStackSize = stackableModalService.getCurrentStackSize();

            breadcrumbService.push({ title: $scope.title, icon: $scope.icon || 'list-alt', click: function () { $scope.$apply(function () { stackableModalService.popUntilStackSize(currentStackSize); }); } });

            $scope.$on('$destroy', unregister);

            function unregister() {
                breadcrumbService.pop();
            }
        }
    }
})();