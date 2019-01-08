(function () {
    'use strict';

    angular
        .module('common')
        .factory('UploaderFactory', UploaderFactory);

    UploaderFactory.$inject = ['ToastService', 'LoaderService', 'FileUploader', 'UserSessionService'];

    function UploaderFactory(toastService, loaderService, FileUploader, userSession) {

        var uploaderFactory = {};

        uploaderFactory.createUploader = function (uploadUrl, successDelegate, errorDelegate, enableAutoUpload) {

            // TODO: deixar de usar este aqui
            var token = userSession.getToken();

            var uploader = new FileUploader({
                url: uploadUrl,
                autoUpload: enableAutoUpload || false,
                headers: (token) ? { 'Authorization': 'Bearer ' + userSession.getToken() } : null
            });

            uploader.onAfterAddingAll = function () {
                keepLastAddedFile();
            };

            function keepLastAddedFile() {
                uploader.queue = uploader.queue.slice(-1);
            }

            uploader.onCancelItem = function () {
            };

            uploader.onCompleteAll = function () {
                
            };

            uploader.onSuccessItem = function (fileItem, response) {
                // Estamos lendo o erro como 200 no caso do IE9, pois o FileUploader interpreta todos como 200:
                // https://github.com/nervgh/angular-file-upload/blob/master/src/module.js#L546

                if(response !== "")
                {
                var r = angular.fromJson(response);

                if (r.message !== undefined) {
                    toastService.error(r.message.decode());
                    return;
                }
                }

                successDelegate(response);
            };

            uploader.onErrorItem = function (item, response) {
                var message;
                // O FileUploader passa por cima do interceptor que cuida dos erros, e não mostra a mensagem de erro traduzida
                // 1. Tenta identificar o status da resposta do objeto xhtmlhttprequest
                if (item._xhr && item._xhr.status === 401) {
                    message = globalization.texts.noPermissionToAccessThisOperation;
                } else if (item._xhr && item._xhr.status === 401) {
                    message = globalization.texts.youAreNotAuthenticated;
                } else if (response && response.message && response.message === "Authorization has been denied for this request.") {
                    // 2. Caso não tenha sido possível obter o status da resposta do objeto xhr (IE?), tenta conferir a mensagem de resposta
                    // (a mensagem é definida pelo .Net - vide SecurityWebApiActionAttribute.cs linha 109)
                    message = globalization.texts.noPermissionToAccessThisOperation;
                } else {
                    // 3. Caso nenhum método tenha funcionado, tenta mostrar o que veio na resposta
                    message = angular.isUndefined(response.message) ? response : response.message;
                }
                toastService.error(message);

                errorDelegate(response);
            };

            return uploader;
        };

        uploaderFactory.createUploaderStaged = function (uploadUrl, successDelegate, errorDelegate, beforeSendDelegate) {

            var token = userSession.getToken();

            var uploader = new FileUploader({
                url: uploadUrl,
                autoUpload: false,
                headers: (token) ? { 'Authorization': 'Bearer ' + userSession.getToken() } : null
            });

            uploader.onBeforeUploadItem = function (item) {

                if (!!beforeSendDelegate) {
                    beforeSendDelegate(item);
                }

                loaderService.push(uploader.url);
                loaderService.late();
            };

            uploader.onCompleteItem = function (item) {
                loaderService.pop(uploader.url);
            };

            uploader.onSuccessItem = function (fileItem, response) {
                // Estamos lendo o erro como 200 no caso do IE9, pois o FileUploader interpreta todos como 200:
                // https://github.com/nervgh/angular-file-upload/blob/master/src/module.js#L546
                var r = angular.fromJson(response);

                if (r.message !== undefined) {
                    toastService.error(r.message.decode());
                    return;
                }

                if (!!successDelegate) {
                    successDelegate(fileItem, response);
                }
            };

            uploader.onErrorItem = function (item, response) {
                var message;
                // O FileUploader passa por cima do interceptor que cuida dos erros, e não mostra a mensagem de erro traduzida
                // 1. Tenta identificar o status da resposta do objeto xhtmlhttprequest
                if (item._xhr && item._xhr.status === 401) {
                    message = globalization.texts.noPermissionToAccessThisOperation;
                } else if (item._xhr && item._xhr.status === 401) {
                    message = globalization.texts.youAreNotAuthenticated;
                } else if (response && response.message && response.message === "Authorization has been denied for this request.") {
                    // 2. Caso não tenha sido possível obter o status da resposta do objeto xhr (IE?), tenta conferir a mensagem de resposta
                    // (a mensagem é definida pelo .Net - vide SecurityWebApiActionAttribute.cs linha 109)
                    message = globalization.texts.noPermissionToAccessThisOperation;
                } else {
                    // 3. Caso nenhum método tenha funcionado, tenta mostrar o que veio na resposta
                    message = angular.isUndefined(response.message) ? response : response.message;
                }
                toastService.error(message);

                if (!!errorDelegate) {
                    errorDelegate(item, response);
                }
            };

            return uploader;
        };

        return uploaderFactory;
    }
})();