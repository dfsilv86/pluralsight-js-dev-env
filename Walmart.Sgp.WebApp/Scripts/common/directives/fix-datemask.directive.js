(function () {
    'use strict';

    angular
        .module('common')
        .directive('fixDatemask', FixDatemaskDirective);

    FixDatemaskDirective.$inject = [];
    function FixDatemaskDirective() {
        return {
            scope: false,
            restrict: 'A',
            require: 'ngModel',
            link: function ($scope, $elem, attrs, ngModel) {
                $elem.on('blur.fixDatemask', function (evt) {
                    if ($elem.val() === '') {
                        $scope.$apply(function () {
                            //ngModel = ngModel;
                            //debugger;
                            ngModel.$setViewValue('');
                        })
                    }
                });

                $scope.$on('$destroy', function () {
                    $elem.off('blur.fixDatemask');
                })
            }
        };
    }
})();