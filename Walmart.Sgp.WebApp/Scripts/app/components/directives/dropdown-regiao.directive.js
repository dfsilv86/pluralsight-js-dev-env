(function () {
    'use strict';

    angular
        .module('SGP')
        .directive('dropdownRegiao', DropdownRegiao);

    DropdownRegiao.$inject = ['$q', 'RegiaoService'];

    function DropdownRegiao($q, regiaoService) {
        return {
            restrict: 'E',
            replace: true,
            templateUrl: 'Scripts/app/components/directives/dropdown-regiao.template.html',
            scope: {
                bandeira: '=',
                ngModel: '=',
                showSelect: '=?',
                showAll: '=?',
                itemOut: '=?'
            },
            link: function($scope, elem, attr) {
                $scope.dropdownName = attr.name || 'dropdownRegiao';
                $scope.$initialValue = $scope.ngModel;
            },
            controller: ['$scope', function ($scope) {

                $scope.data = { values: [], defaultValue: null };

                $scope.$watch('bandeira', function (newValue, oldValue) {
                    // Bug 3133 - Não deve comparar por tipo, pois pode vir tanto como string como int.
                    if (newValue != oldValue || !oldValue) { 
                        refresh();
                    }
                });

                $scope.resetValue = resetValue;

                $scope.$watch('ngModel', function (newVal, oldVal) {                    
                    if (newVal != oldVal && !newVal) {
                        resetValue();
                    }

                    setItemOut();
                });

                $scope.$applyAsync(refresh);

                function setItemOut() {
                    var filtered = $scope.data.values.filter(function (b) {
                        return b.idRegiao == $scope.ngModel;
                    });

                    if (filtered.length === 0) {
                        $scope.itemOut = null;
                    } else {
                        $scope.itemOut = filtered[0];
                        $scope.bandeira = filtered[0].idBandeira;
                }
                }

                function refresh() {

                    var idBandeira = $scope.bandeira;

                    if (angular.isDefined(idBandeira) && !!idBandeira) {

                        var deferred = $q
                            .when(regiaoService.obterPorBandeira(idBandeira))
                            .then(applyValues);

                    } else {

                        $scope.ngModel = null;
                        $scope.data.values.splice(0, $scope.data.values.length);
                    }
                }

                function applyValues(regioes) {
                    $scope.data.values = regioes;                    
                    $scope.data.defaultValue = (regioes[0] || {}).idRegiao;

                    if ($scope.showSelect || $scope.showAll) {
                        $scope.data.defaultValue = null;
                    }

                    resetValue();
                }

                function resetValue() {
                    var keyAttr = 'idRegiao'
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
            }]
        };
    }
})();