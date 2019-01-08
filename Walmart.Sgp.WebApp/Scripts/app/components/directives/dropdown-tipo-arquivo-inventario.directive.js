(function () {
    'use strict';

    angular
        .module('SGP')
        .directive('dropdownTipoArquivoInventario', DropdownTipoArquivoInventario);

    DropdownTipoArquivoInventario.$inject = ['$q'];

    function DropdownTipoArquivoInventario($q) {
        return {
            restrict: 'E',
            replace: true,
            templateUrl: 'Scripts/app/components/directives/dropdown-tipo-arquivo-inventario.template.html',
            scope: {
                ngModel: '=',
                showSelect: '=?',
                showAll: '=?',
                itemOut: '=?'
            },
            link: function ($scope, elem, attr) {
                $scope.dropdownName = attr.name || 'dropdownTipoArquivoInventario';
                $scope.$initialValue = $scope.ngModel;
            },
            controller: ['$scope', function ($scope) {

                $scope.data = { values: [], defaultValue: null };

                $scope.resetValue = resetValue;

                $scope.$watch('ngModel', function (newVal, oldVal) {
                    if (newVal !== oldVal && !newVal && !!$scope.data.defaultValue) {
                        resetValue();
                    }

                    setItemOut();
                });

                function setItemOut() {
                    var filtered = $scope.data.values.filter(function (b) {
                        return b.value == $scope.ngModel;
                    });

                    $scope.itemOut = filtered.length === 0 ? null : filtered[0];
                }

                function refresh() {

                    var deferred = $q
                        .when(sgpFixedValues.tipoArquivoInventario)
                        .then(applyValues);
                }

                function applyValues(tiposArquivoInventario) {
                    $scope.data.values = tiposArquivoInventario;
                    $scope.data.defaultValue = (tiposArquivoInventario[0] || {}).value;

                    if ($scope.showSelect || $scope.showAll) {
                        $scope.data.defaultValue = null;
                    }

                    resetValue();
                }

                function resetValue() {
                    $scope.ngModel = $scope.$initialValue || $scope.data.defaultValue;
                }

                refresh();
            }]
        };
    }
})();