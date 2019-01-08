(function () {
    'use strict';

    angular
        .module('SGP')
        .directive('dropdownDistrito', DropdownDistrito);

    DropdownDistrito.$inject = ['$q', 'DistritoService'];

    function DropdownDistrito($q, distritoService) {
        return {
            restrict: 'E',
            replace: true,
            templateUrl: 'Scripts/app/components/directives/dropdown-distrito.template.html',
            scope: {
                regiao: '=',
                ngModel: '=',
                showSelect: '=?',
                showAll: '=?',
                itemOut: '=?'
            },
            link: function($scope, elem, attr) {
                $scope.dropdownName = attr.name || 'dropdownDistrito';
                $scope.$initialValue = $scope.ngModel;
            },
            controller: ['$scope', function ($scope) {

                $scope.data = { values: [], defaultValue: null };

                $scope.$watch('regiao', function (newValue, oldValue) {
                    // Bug 3133 - Não deve comparar por tipo, pois pode vir tanto como string como int.
                    if (newValue != oldValue) { 
                        refresh();
                    }
                });

                $scope.resetValue = resetValue;

                $scope.$watch('ngModel', function (newVal, oldVal) {                    
                    if (newVal !== oldVal && !newVal) {
                        resetValue();
                    }

                    setItemOut();
                });

                function setItemOut() {
                    var filtered = $scope.data.values.filter(function (b) {
                        return b.idDistrito == $scope.ngModel;
                    });

                    if (filtered.length === 0) {
                        $scope.itemOut = null;
                    } else {
                        $scope.itemOut = filtered[0];
                        $scope.regiao = filtered[0].idRegiao;
                }
                }

                function refresh() {

                    var idRegiao = $scope.regiao;

                    if (angular.isDefined(idRegiao) && !!idRegiao) {

                        var deferred = $q
                            .when(distritoService.obterPorRegiao(idRegiao))
                            .then(applyValues);

                    } else {

                        $scope.ngModel = null;
                        $scope.data.values.splice(0, $scope.data.values.length);
                    }
                }

                function applyValues(distritos) {
                    $scope.data.values = distritos;                    
                    $scope.data.defaultValue = (distritos[0] || {}).idDistrito;

                    if ($scope.showSelect || $scope.showAll) {
                        $scope.data.defaultValue = null;
                    }

                    resetValue();
                }

                function resetValue() {
                    var keyAttr = 'idDistrito'
                    var filteredInitial = $scope.data.values.filter(function (b) {
                        return b[keyAttr] == $scope.$initialValue;
                    });
                    var filteredDefault = $scope.data.values.filter(function (b) {
                        return b[keyAttr] == $scope.data.defaultValue;
                    });
                    if (filteredInitial.length != 0) {
                        $scope.ngModel = $scope.$initialValue;
                    } else if (filteredDefault.length != 0) {
                        $scope.ngModel = $scope.data.defaultValue;
                    } else {
                        $scope.ngModel = null;
                    }
                }

                refresh();
            }]
        };
    }
})();