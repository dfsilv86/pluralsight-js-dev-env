(function() {

    // <checkbox ng-model="binding para valor (boolean)" id="id do input" name="name do input na form (validação)" label="label interno do controle" show-label="true se for mostrar o texto do label" ng-disabled="binding pra disabled"></checkbox>

    angular
        .module('common')
        .directive('checkbox', CommonCheckbox);

    CommonCheckbox.$inject = [];

    function CommonCheckbox() {
        return {
            restrict: 'E',
            replace: true,
            scope: {
                ngModel: '=',
                id: '@?',
                name: '@?',
                label: '@?',
                ngDisabled: '=?',
                showLabel: '@?',
                ngReadonly: '=?'
            },
            link: function ($scope, elem, attr) {
                $scope.isDisabled = angular.isDefined(attr.$attr.disabled);
                $scope.isReadonly = angular.isDefined(attr.$attr.readonly);
                $scope.labelVisibility = ($scope.showLabel || 'true').toString().toLowerCase() == 'false' ? {'visibility':'hidden'} : {};
            },
            templateUrl: 'Scripts/common/directives/checkbox.template.html',
        };
    }
})();