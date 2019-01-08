(function () {
    'use strict';

    angular
        .module('SGP')
        .directive('dropdownOrigemSugestao', DropdownUnidadeMedida);

    function DropdownUnidadeMedida() {
        return {
            restrict: 'E',
            replace: true,
            templateUrl: 'Scripts/app/components/directives/dropdown-fixed-values.template.html',
            scope: {
                ngModel: '='
            },
            link: function ($scope, elem, attr) {
                $scope.dropdownName = attr.name || 'dropdownOrigemSugestao';
            },
            controller: ['$scope', function ($scope) {

                $scope.data = {
                    values: sgpFixedValues.tipoOrigemSugestao
                };
            }]
        };
    }
})();