(function () {
    'use strict';

    angular
        .module('SGP')
        .directive('dropdownFormato', DropdownFormato);

    DropdownFormato.$inject = ['$q', 'FormatoService'];

    function DropdownFormato($q, formatoService) {
        return {
            restrict: 'E',
            replace: true,
            templateUrl: 'Scripts/app/components/directives/dropdown-formato.template.html',
            scope: {
                sistema: '=?',
                ngModel: '=',
                showSelect: '=?',
                showAll: '=?',
                itemOut: '=?'
            },
            link: function($scope, elem, attr) {
                $scope.dropdownName = attr.name || 'dropdownFormato';
                $scope.$initialValue = $scope.ngModel;
            },
            controller: ['$scope', function ($scope) {

                $scope.data = { values: [], defaultValue: null };

                $scope.$watch('sistema', function (newValue, oldValue) {
                    // Bug 3133 - Não deve comparar por tipo, pois pode vir tanto como string como int.
                    if (newValue != oldValue) { 
                        refresh();
                    }
                });

                $scope.resetValue = resetValue;

                $scope.$watch('ngModel', function (newVal, oldVal) {                    
                    if (newVal !== oldVal && !newVal && !!$scope.data.defaultValue) {
                        resetValue();
                    }

                    setItemOut();
                });

                function setItemOut() {
                    var filtered = $scope.data.values.filter(function (b) {
                        return b.idFormato == $scope.ngModel;
                    });

                    $scope.itemOut = filtered.length === 0 ? null : filtered[0];
                }

                function refresh() {

                    var cdSistema = $scope.sistema;

                    if (angular.isDefined(cdSistema) && !!cdSistema) {

                        $q.when(formatoService.obterPorSistema(cdSistema))
                            .then(applyValues);
                    }
                    else {
                        $q.when(formatoService.obterTodos())
                            .then(applyValues);
                    }
                }

                function applyValues(formatos) {
                    $scope.data.values = formatos;                    
                    $scope.data.defaultValue = (formatos[0] || {}).idFormato;

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