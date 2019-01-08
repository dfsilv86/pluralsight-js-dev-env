(function () {
    'use strict';

    angular
        .module('SGP')
        .directive('dropdownTipoCarga', DropdownTipoCarga);

    function DropdownTipoCarga() {
        return {
            restrict: 'E',
            replace: true,
            templateUrl: 'Scripts/app/components/directives/dropdown-fixed-values.template.html',
            scope: {
                ngModel: '='
            },
            link: function ($scope, elem, attr) {
                $scope.dropdownName = attr.name || 'dropdownTipoCarga';
            },
            controller: ['$scope', function ($scope) {

                $scope.data = {
                    values: sgpFixedValues.tipoCarga
                };
            }]
        };
    }
})();