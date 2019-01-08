(function () {
    'use strict';

    angular
        .module('SGP')
        .directive('dropdownProcessName', DropdownProcessNameDirective);

    DropdownProcessNameDirective.$inject = ['$q'];

    function DropdownProcessNameDirective($q) {
        return {
            restrict: 'E',
            replace: true,
            templateUrl: 'Scripts/app/components/directives/dropdown-process-name.template.html',
            scope: {
                ngModel: '=',
                itemOut: '=?'
            },
            link: function ($scope, elem, attr) {
                $scope.dropdownName = attr.name || 'dropdownProcessName';
                $scope.$initialValue = $scope.ngModel;
                if ($scope.isRestricted) {
                    elem.attr('disabled', 'disabled');
                    attr.ngDisabled = 'true';
                }
            },
            controller: ['$scope', 'ProcessingService', function ($scope, processingService) {

                $scope.data = { values: [], defaultValue: '' };
                $scope.resetValue = resetValue;

                var deferred = $q
                    .when(processingService.getProcessNames())
                    .then(applyValues);

                $scope.$watch('ngModel', function (newVal, oldVal) {
                    if (newVal != oldVal && !newVal && !!$scope.data.defaultValue) {
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

                function applyValues(data) {
                    $scope.data.values = data;
                    ////$scope.data.defaultValue = (data[0] || {}).value;
                    resetValue();
                }

                function resetValue() {
                    $scope.ngModel = $scope.$initialValue || $scope.data.defaultValue;
                }
            }]
        };
    }
})();