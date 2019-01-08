(function () {
    'use strict';

    angular
        .module('common')
        .controller('ConfirmController', ConfirmController);

    ConfirmController.$inject = ['$scope', '$q', '$rootScope', '$compile', '$controller', 'ngDialog', 'options'];

    function ConfirmController($scope, $q, $rootScope, $compile, $controller, ngDialog, options) {
        $scope.yes = yes;
        $scope.no = no;
        $scope.message = options.message;
        $scope.messageTemplateUrl = options.messageTemplateUrl;
        $scope.vm = {};
        angular.extend($scope.vm, options.messageScope);
		
        function yes() {
            if (options.yesClicked != null) {
                options.yesClicked();
            }

            ngDialog.closeAll();
        }

        function no() {
            if (options.noClicked != null) {
                options.noClicked();
            }

            ngDialog.closeAll();
        }
    }

})();