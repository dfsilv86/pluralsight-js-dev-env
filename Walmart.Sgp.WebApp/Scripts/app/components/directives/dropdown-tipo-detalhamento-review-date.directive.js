(function () {
    'use strict';

    angular
        .module('SGP')
        .directive('dropdownTipoDetalhamentoReviewDate', dropdownTipoDetalhamentoReviewDate);

    function dropdownTipoDetalhamentoReviewDate() {
        return {
            restrict: 'E',
            replace: true,
            templateUrl: 'Scripts/app/components/directives/dropdown-fixed-values.template.html',
            scope: {
                ngModel: '=',
                ngChange: '&'
            },
            link: function ($scope, elem, attr) {
                $scope.dropdownName = attr.name || 'dropdownTipoDetalhamentoReviewDate';
            },
            controller: ['$scope', function ($scope) {

                $scope.data = {
                    values: sgpFixedValues.tipoDetalhamentoReviewDate
                };
            }]
        };
    }
})();