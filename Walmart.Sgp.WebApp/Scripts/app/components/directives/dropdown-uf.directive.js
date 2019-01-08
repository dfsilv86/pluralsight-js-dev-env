(function () {
    'use strict';

    angular
        .module('SGP')
        .directive('dropdownUf', DropdownUF);

    function DropdownUF() {
        return {
            restrict: 'E',
            replace: true,
            templateUrl: 'Scripts/app/components/directives/dropdown-uf.template.html',
            scope: {
                ngModel: '=',
                regiao: '=?'
            },
            link: function ($scope, elem, attr) {
                $scope.dropdownName = attr.name || 'dropdownUf';
                $scope.$initialValue = $scope.ngModel;
            },
            controller: ['$scope', function ($scope) {

                $scope.data = { values: [], defaultValue: null };

                $scope.$watch('regiao', refresh);

                function refresh() {

                    var IDRegiao = $scope.regiao;

                    if (angular.isDefined(IDRegiao) && !!IDRegiao) {
                        $scope.data.values = sgpFixedValues.uf.filter(filterByRegion);
                    }
                    else {
                        $scope.data.values = sgpFixedValues.uf;
                    }
                    $scope.ngModel = $scope.$initialValue || $scope.ngModel;
                }

                function filterByRegion(uf) {
                    var IDRegiao = $scope.regiao;
                    var arrNE = ['AL', 'BA', 'CE', 'MA', 'PB', 'PE', 'PI', 'RN', 'SE'];
                    var arrSE = ['ES', 'MG', 'RJ', 'SP', 'MS'];
                    var arrSO = ['PR', 'RS', 'SC'];

                    switch (IDRegiao)
                    {
                        case ("1"): {
                            return arrNE.indexOf(uf.value) >= 0;
                        }
                        case ("2"): {
                            return arrSE.indexOf(uf.value) >= 0;
                        }
                        case ("3"): {
                            return arrSO.indexOf(uf.value) >= 0;
                        }
                    }

                    return false;
                }
            }]
        };
    }    
})();