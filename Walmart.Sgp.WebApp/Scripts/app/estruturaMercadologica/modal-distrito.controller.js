(function () {
    'use strict';

    angular
		.module('SGP')
		.controller('ModalDistritoController', ModalDistritoController);

    ModalDistritoController.$inject = ['$scope', '$q', '$uibModalInstance', 'DistritoService', 'distrito', 'ConfirmService', 'ValidationService'];

    function ModalDistritoController($scope, $q, $uibModalInstance, distritoService, distrito, confirmService, $validation) {

        $validation.prepare($scope);
        $scope.temp = {};
        $scope.distrito = distrito;
        $scope.save = save;
        $scope.back = back;

        function back() {
            $uibModalInstance.close(false);
        }

        function save() {
            if (!$validation.validate($scope)) return;

            confirmService.ask(globalization.texts.confirmSaveChanges)
                .then(doSave);
        }

        function doSave() {
            var deferred = $q
                .when(distritoService.salvar($scope.distrito))
                .then(saved);
        }

        function saved() {
            $uibModalInstance.close(true);
        }
    }
})();