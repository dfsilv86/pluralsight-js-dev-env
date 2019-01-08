(function () {
    'use strict';

    angular
        .module('SGP')
        .directive('dropdownStatusInventario', DropdownStatusInventario);

    function DropdownStatusInventario() {
        return {
            restrict: 'E',
            replace: true,
            templateUrl: 'Scripts/app/components/directives/dropdown-fixed-values.template.html',
            scope: {
                ngModel: '=',
                showAll: '=?',
                showSelect: '=?',
            },
            link: function ($scope, elem, attr) {
                $scope.dropdownName = attr.name || 'dropdownStatusInventario';
            },
            controller: ['$scope', function ($scope) {
                $scope.data = {
                    values: sgpFixedValues.inventarioStatus.filter(function (val) {
                        return val.value !== sgpFixedValues.inventarioStatus.contabilizado;
                    })
                };
            }]
        };
    }
})();