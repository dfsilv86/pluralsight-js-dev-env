(function () {
    'use strict';

    angular
        .module('SGP')
        .controller('ImpMultiSourcingController', ImpMultiSourcingController);

    ImpMultiSourcingController.$inject = ['$q', '$scope', '$uibModalInstance', 'MultisourcingService', '$state', '$stateParams', 'ApiEndpoints', 'UploaderFactory', 'LoaderService', 'ToastService', 'UserSessionService', 'cdSistema'];

    function ImpMultiSourcingController($q, $scope, $uibModalInstance, multisourcingService, $state, $stateParams, apiEndpoints, uploaderFactory, loaderService, toastService, userSession, cdSistema) {

        $scope.save = save;
        $scope.cancel = cancel;
        $scope.uploader = uploaderFactory.createUploaderStaged(apiEndpoints.sgp.multisourcingUpload, itemStored, null, beforeUpload);
        $scope.uploader.onCompleteAll = function (response, status) {
            $scope.$apply(function () {
                importar();
            })
        };

        $scope.data = {
            cdSistema: cdSistema
        };

        function importar() {

            var tickets = [];
            var items = [];

            for (var i = 0; i < $scope.uploader.queue.length; i++) {

                var item = $scope.uploader.queue[i];

                items.push(item);

                if (item.isSuccess) {
                    tickets.push(item.fileVaultTicket);
                }
            }

            for (i = 0; i < items.length; i++) {
                items[i].remove();
            }

            var userId = userSession.getCurrentUser().id;

            $q.when(multisourcingService.importar($scope.data.cdSistema, userId, tickets))
                .then(uploaderSucesso)
                .catch(erroCompartilhado);

            $scope.uploader.queue = [];
            angular.element('input[type=file]').val('');
        }

        function beforeUpload(item) {
            if (item.isInvalid) {
                item.cancel();
            }
        }

        function itemStored(item, fileVaultTicket) {

            item.fileVaultTicket = fileVaultTicket;
        }

        function save() {
            if ($scope.uploader.queue.length == 0 || angular.element('input[type=file]').val() == "") {
                toastService.warning(globalization.texts.uploadFileNotFound);
                return;
            }

            loaderService.push($scope.uploader.url);
            $scope.uploader.queue[0].upload();
        }

        function cancel() {
            $uibModalInstance.close();
        }
        
        function uploaderSucesso(response) {
            loaderService.pop($scope.uploader.url);

            if (response == 'NOK') {
                toastService.warning(globalization.texts.importNotOK);
                return;
            }

            toastService.success(globalization.texts.importSuccess);
        }

        function erroCompartilhado(result) {
            loaderService.pop($scope.uploader.url);

            if (!result) return result;
            if (result.mensagem) {
                toastService.error(result.mensagem);
            }
        }
    }
})();