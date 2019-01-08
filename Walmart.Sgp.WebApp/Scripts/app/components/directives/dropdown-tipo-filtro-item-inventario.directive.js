(function () {
    'use strict';

    function DropdownTipoFiltroItemInventario() {
        return {
            restrict: 'E',
            replace: true,
            templateUrl: 'Scripts/app/components/directives/dropdown-fixed-values.template.html',
            scope: {
                ngModel: '='
            },
            link: function ($scope, elem, attr) {
                $scope.dropdownName = attr.name || 'dropdownTipoFiltroItemInventario';
            },
            controller: ['$scope', function ($scope) {

                $scope.data = {
                    values: sgpFixedValues.tipoFiltroItemInventario
                };
            }]
        };
    }

    angular
        .module('SGP')
        .directive('dropdownTipoFiltroItemInventario', DropdownTipoFiltroItemInventario);
})();