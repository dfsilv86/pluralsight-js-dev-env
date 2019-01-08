(function () {
    'use strict';

    angular
        .module('SGP')
        .directive('dropdownMesAnoExpurgo', DropdownMesAnoExpurgo);

    DropdownMesAnoExpurgo.$inject = ['$q'];

    function DropdownMesAnoExpurgo($q) {
        return {
            restrict: 'E',
            replace: true,
            require: ['ngModel'],
            templateUrl: 'Scripts/app/components/directives/dropdown-mesAno-expurgo.template.html',
            scope: {
                ngModel: '='
            },
            link: function ($scope, elem, attr) {
                $scope.dropdownName = attr.name || 'dropdownMesAnoExpurgo';
                $scope.$initialValue = $scope.ngModel;
            },
            controller: ['$scope', '$timeout', 'moment', function ($scope, $timeout, moment) {

                $scope.data = { values: [], defaultValue: null };
                $scope.resetValue = resetValue;

                $scope.$watch('ngModel', function (newVal, oldVal) {
                    if (newVal != oldVal && !newVal && !!$scope.data.defaultValue) {
                        resetValue();
                    }
                });

                var reference = moment().startOf('month');

                var result = [];

                for (var i = 0; i < 4; i++) {
                    result.push(moment(reference).subtract(i, 'months'));
                }

                $scope.data.values = result;
                $scope.data.defaultValue = result[0].format().toString();

                resetValue();

                function resetValue() {

                    // TODO: talvez não precisasse usar $initialValue neste componente?

                    $scope.data.value = $scope.ngModel = $scope.$initialValue || $scope.data.defaultValue;
                }
            }]
        };
    }
})();