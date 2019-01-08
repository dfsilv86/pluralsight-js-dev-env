(function () {
    'use strict';

    angular
        .module('SGP')
        .directive('dropdownProcessOrderState', DropdownProcessOrderStateDirective);

    function DropdownProcessOrderStateDirective() {
        return {
            restrict: 'E',
            replace: true,
            templateUrl: 'Scripts/app/components/directives/dropdown-fixed-values.template.html',
            scope: {
                ngModel: '='
            },
            link: function ($scope, elem, attr) {
                $scope.showAll2 = true;
                $scope.dropdownName = attr.name || 'dropdownProcessOrderState';
            },
            controller: ['$scope', function ($scope) {

                $scope.data = {
                    values: sgpFixedValues.processOrderState
                };
            }]
        };
    }
})();