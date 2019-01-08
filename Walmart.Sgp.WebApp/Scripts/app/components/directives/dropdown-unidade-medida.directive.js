(function () {
    'use strict';

    angular
        .module('SGP')
        .directive('dropdownUnidadeMedida', DropdownUnidadeMedida);

    function DropdownUnidadeMedida() {
        return {
            restrict: 'E',
            replace: true,
            templateUrl: 'Scripts/app/components/directives/dropdown-fixed-values.template.html',
            scope: {
                ngModel: '=',
                showSelect: '@?showSelect'
            },
            link: function ($scope, elem, attr) {
                $scope.dropdownName = attr.name || 'dropdownUnidadeMedida';
            },
            controller: ['$scope', function ($scope) {                
                $scope.data = {
                    values: sgpFixedValues.tipoUnidadeMedida
                };
            }]
        };
    }
})();