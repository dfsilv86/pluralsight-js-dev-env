(function () {
    'use strict';

    angular
        .module('common')
        .directive('sortIndicator', SortIndicator);

    SortIndicator.$inject = ['$interpolate', 'PagingService'];
    function SortIndicator($interpolate, pagingService) {

        return {
            restrict: 'E',
            template: '<span class="sort-indicator glyphicon"></span>',
            link: function ($scope, elem, attrs) {

                var interpolateFn = function () { return $scope.$eval(attrs.ngModel); };

                var icon = elem.find('.glyphicon');

                $scope.$watch(interpolateFn, function (newValue) { apply(newValue, attrs.name); });
                attrs.$observe('name', function (newValue) { apply(interpolateFn(), attrs.name); });

                function apply(paging, columnName) {

                    elem.toggle(!!columnName && !!paging);

                    if (!!columnName && !!paging) {

                        var state = pagingService.getSortState(paging, columnName);

                        icon.toggleClass('glyphicon-sort', state === null);
                        icon.toggleClass('glyphicon-sort-by-attributes', state === 'asc');
                        icon.toggleClass('glyphicon-sort-by-attributes-alt', state === 'desc');

                    } else {

                        icon.removeClass('glyphicon-sort');
                        icon.removeClass('glyphicon-sort-by-attributes');
                        icon.removeClass('glyphicon-sort-by-attributes-alt');
                    }
                }
            },
            scope: false
        };


        /*
        return {
            restrict: 'E',
            controller: ['$scope', SortIndicatorController],
            template: '<span ng-if="vm.isVisible" class="sort-indicator glyphicon" ng-class="{\'glyphicon-sort\':(vm.mode==1),\'glyphicon-sort-by-attributes\':(vm.mode==2),\'glyphicon-sort-by-attributes-alt\':(vm.mode==3)}"></span>',
            replace: false,
            scope: {
                ngModel: '=',
                name: '@'
            }
        };

        SortIndicatorController.$inject = ['$scope'];

        function SortIndicatorController($scope) {

            $scope.vm = { isVisible: !!$scope.name, mode: 1 };
            $scope.vm.mode = $scope.vm.isVisible ? 1 : 0;

            $scope.$watchGroup(['ngModel.orderBy', 'name'], function (newValues, oldValues) {
                $scope.vm.isVisible = !!$scope.name;
                if (newValues[0] != null && newValues[0] != oldValues[0] && !!newValues[1]) {
                    // TODO: para suportar multiplas colunas no sort, ajustar aqui para splitar newValues[0] por vírgula para
                    //       obter os atributos e iterar a lista para saber se name está nela.
                    var x = newValues[0].split(' ');
                    if (x.length == 1) x.push('asc');
                    if (x[0].toLowerCase() != $scope.name.toLowerCase()) {
                        $scope.vm.mode = 1;
                    } else {
                        if (x[1].toLowerCase() == 'asc') {
                            $scope.vm.mode = 2;
                        } else {
                            $scope.vm.mode = 3;
                        }
                    }
                }
            });
        }*/
    }

})();