(function () {
    'use strict';

    angular
        .module('common')
        .directive('formGroup', FormGroup);

    function makeTitle(value) {
        return !!value ? value + ':' : '';
    }

    function FormGroup() {
        return {
            restrict: 'E',
            replace: true,
            templateUrl: 'Scripts/common/directives/form-group.template.html',
            transclude: true,
            scope: true,
            link: function ($scope, elem, attr) {
                $scope.formField = {
                    title: makeTitle(attr.caption),
                    labelWidth: attr.labelWidth || 4,
                    controlWidth: attr.controlWidth || 8
                };
                
                attr.$observe('caption', function (value) {
                    $scope.formField.title = makeTitle(value);
                });                
            }
        };
    }
})();