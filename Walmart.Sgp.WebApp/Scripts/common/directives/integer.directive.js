(function () {
    'use strict';

    angular
        .module('common')
        .directive('integer', IntegerDirective);

    function IntegerDirective() {

        return {
            restrict: 'A',
            scope: false,
            link: function ($scope, $elem) {
                $elem.on('keypress.integer', function (evt) {
                    if (48 <= evt.charCode && evt.charCode <= 57) {
                        return true;
                    }
                    evt.preventDefault();
                    return false;
                });

                $scope.$on('$destroy', function () {
                    $elem.off('keypress.integer');
                });
            }
        };
    };
})();