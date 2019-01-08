(function () {
    'use strict';

    angular
        .module('SGP')
        .directive('dropdownTipoStatus', DropdownTipoStatus);

    function DropdownTipoStatus() {
        return {
            restrict: 'E',
            replace: true,
            templateUrl: 'Scripts/app/components/directives/dropdown-fixed-values.template.html',
            scope: {
                ngModel: '='
            },
            link: function ($scope, elem, attr) {
                $scope.dropdownName = attr.name || 'dropdownTipoStatus';
            },
            controller: ['$scope', function ($scope) {
                $scope.data = {
                    values: sgpFixedValues.tipoStatus
                };
            }]
        };
    }
})();