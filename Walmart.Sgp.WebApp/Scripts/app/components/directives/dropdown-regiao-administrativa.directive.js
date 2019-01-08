(function () {
    'use strict';
    
    angular
        .module('SGP')
        .directive('dropdownRegiaoAdministrativa', DropdownRegiaoAdministrativa);

    DropdownRegiaoAdministrativa.$inject = ['$q', 'RegiaoAdministrativaService'];

    function DropdownRegiaoAdministrativa($q, regiaoAdministrativaService) {
        return {
            restrict: 'E',
            replace: true,
            templateUrl: 'Scripts/app/components/directives/dropdown-regiao-administrativa.template.html',
            scope: {
                itemOut: '=?',
                ngModel: '='
            },
            link: function ($scope, elem, attr) {
                $scope.dropdownName = attr.name || 'dropdownRegiaoAdministrativa';
                $scope.$initialValue = $scope.ngModel;

                if (angular.isDefined(attr.showAll)) {
                    $scope.defaultOption = globalization.texts.all;
                    elem.removeAttr('data-placeholder');
                }
            },
            controller: ['$scope', function ($scope) {
                $scope.data = { values: [] };

                $scope.$watch('ngModel', function (newVal, oldVal) {
                    var filtered = $scope.data.values.filter(function (b) {
                        return b.idRegiaoAdministrativa == $scope.ngModel;
                    });

                    if (filtered.length === 0) {
                        $scope.itemOut = null;
                    } else {
                        $scope.itemOut = filtered[0];
                    }
                });

                var deferred = $q
                    .when(regiaoAdministrativaService.obterTodos())
                    .then(applyValues);

                function applyValues(values) {
                    $scope.data.values = values;
                    $scope.ngModel = $scope.data.value = (values[0] || {}).IDRegiaoAdministrativa;
                    $scope.itemOut = $scope.data.item = (values[0] || {});
                    $scope.ngModel = $scope.$initialValue || $scope.ngModel;
                }
            }]
        };
    }
})();