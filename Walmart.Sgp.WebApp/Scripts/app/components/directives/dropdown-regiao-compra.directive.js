(function () {
    'use strict';

    angular
        .module('SGP')
        .directive('dropdownRegiaoCompra', DropdownRegiaoCompra);

    DropdownRegiaoCompra.$inject = ['$q', 'RegiaoCompraService'];

    function DropdownRegiaoCompra($q, regiaoCompraService) {
        return {
            restrict: 'E',
            replace: true,
            templateUrl: 'Scripts/app/components/directives/dropdown-regiao-compra.template.html',
            scope: {
                ngModel: '='
            },
            link: function ($scope, elem, attr) {
                $scope.dropdownName = attr.name || 'dropdownRegiaoCompra';
                $scope.$initialValue = $scope.ngModel;

                if (angular.isDefined(attr.showAll)) {
                    $scope.defaultOption = globalization.texts.all;
                    elem.removeAttr('data-placeholder');
                }
            },
            controller: ['$scope', function ($scope) {

                $scope.data = { values: [] };

                var deferred = $q
                    .when(regiaoCompraService.obterTodos())
                    .then(applyValues);

                function applyValues(values) {
                    $scope.data.values = values;
                    $scope.ngModel = $scope.data.value = (values[0] || {}).IDRegiaoCompra;
                    $scope.ngModel = $scope.$initialValue || $scope.ngModel;
                }
            }]
        };
    }
})();
