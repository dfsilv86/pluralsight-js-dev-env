(function () {
    'use strict';

    angular
        .module('common')
        .directive('form', FormValidationExtension);

    FormValidationExtension.$inject = ['$log'];

    function FormValidationExtension($log) {

        return {
            restrict: 'E',
            link: FormValidationLink,
            scope: false
        };

        function FormValidationLink($scope, elem, attr) {

            var formName = attr.name;

            if (!!formName) {

                $scope.formValidationExtension = ($scope.formValidationExtension) ? $scope.formValidationExtension : new FormValidationApi();

                $scope.formValidationExtension.addForm(formName, function () { return $scope[formName]; });

                $scope.$on('$destroy', function () {
                    $scope.formValidationExtension.removeForm(formName);
                });

            } else {
                $log.warn('Foi encontrado um elemento form sem o atributo name, ou o escopo não foi preparado para validação.');
            }
        }
    }

})();