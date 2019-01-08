(function () {
    'use strict';

    angular
        .module('SGP')
        .directive('dropdownCd', DropdownCD);

    DropdownCD.$inject = ['$q', 'CDService'];

    function DropdownCD($q, cdService) {
        return {
            restrict: 'E',
            replace: true,
            templateUrl: 'Scripts/app/components/directives/dropdown-cd.template.html',
            scope: {
                ngModel: '='
            },
            link: function ($scope, elem, attr) {
                $scope.dropdownName = attr.name || 'dropdownCD';
                $scope.$initialValue = $scope.ngModel;

                if (angular.isDefined(attr.showAll)) {
                    $scope.defaultOption = globalization.texts.all;
                    elem.removeAttr('data-placeholder');
                }
            },
            controller: ['$scope', function ($scope) {

                $scope.data = { values: [] };

                var deferred = $q
                    .when(cdService.obterTodosConvertidosAtivos())
                    .then(applyValues);

                function applyValues(values) {
                    $scope.data.values = values;
                    $scope.ngModel = $scope.$initialValue || $scope.ngModel;
                }
            }]
        };
    }
})();
