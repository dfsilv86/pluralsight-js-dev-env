(function () {
    'use strict';

    angular
        .module('SGP')
        .directive('dropdownStatusItemHost', DropdownStatusItemHost);

    DropdownStatusItemHost.$inject = ['$q', 'StatusItemHostService'];

    function DropdownStatusItemHost($q, statusItemHostService) {
        return {
            restrict: 'E',
            replace: true,
            templateUrl: 'Scripts/app/components/directives/dropdown-status-item-host.template.html',
            scope: {
                ngModel: '='
            },
            link: function ($scope, elem, attr) {
                $scope.dropdownName = attr.name || 'itemStatus';
                $scope.$initialValue = $scope.ngModel;
            },
            controller: ['$scope', function ($scope) {

                $scope.data = { values: [] };

                var deferred = $q
                    .when(statusItemHostService.obterTodos())
                    .then(applyValues);

                function applyValues(values) {
                    $scope.data.values = values;
                    $scope.ngModel = $scope.data.value = (values[0] || {}).tpStatus;
                    $scope.ngModel = $scope.$initialValue || $scope.ngModel;
                }
            }]
        };
    }
})();