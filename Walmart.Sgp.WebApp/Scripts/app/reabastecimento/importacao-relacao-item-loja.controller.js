(function () {
    'use strict';

    // Configuração da controller
    angular
        .module('SGP')
        .config(ImpRelacaoItemLojaRoute)
        .controller('ImpRelacaoItemLojaController', ImpRelacaoItemLojaController);

    // Implementação da controller
    ImpRelacaoItemLojaController.$inject = ['$q', '$scope', 'RelacaoItemLojaCDService', '$state', '$stateParams', 'ApiEndpoints', 'UploaderFactory', 'ToastService', 'UserSessionService', 'cdSistema', 'isVincular', 'ProcessingService', 'ValidationService', 'PagingService', 'UserSessionService', 'FileVaultService', '$interval', 'resultado', '$timeout'];
    function ImpRelacaoItemLojaController($q, $scope, relacaoItemLojaCDService, $state, $stateParams, apiEndpoints, uploaderFactory, toastService, userSession, cdSistema, isVincular, processingService, $validation, pagingService, userSessionService, fileVaultService, $interval, resultado, $timeout) {

        $validation.prepare($scope);

        $scope.save = save;
        $scope.cancel = cancel;
        $scope.baixarTemplate = baixarTemplate;
        $scope.isAdmin = isAdmin;
        $scope.download = download;
        $scope.downloadEnabled = downloadEnabled;
        $scope.orderBy = orderBy;
        $scope.search = search;
        $scope.isVincular = isVincular;

        $scope.data = {
            cdSistema: cdSistema,
            data: { values: null },
            securityAction: isVincular ? "RelacaoItemLojaCD.Vincular" : "RelacaoItemLojaCD.Desvincular",
            paging: { offset: 0, limit: 10, orderBy: 'createdDate desc' }
        };

        if (!!resultado) {
            var theState = 'impVincularItemLojaCD';
            if ($state.is('impDesvincularItemLojaCDResults')) theState = 'impDesvincularItemLojaCD';
            // estamos no meio de uma transicao; aguarda o final desta para iniciar nova transicao de estado.
            $timeout(function () { // suppress-validator
                $state.go(theState, { cdSistema: $scope.data.cdSistema }, { rebuild: true });
            });
        }
        
        var urlUploader = isVincular ? apiEndpoints.sgp.uploadVinculoRelacaoItemLojaCD : apiEndpoints.sgp.uploadDesvinculoRelacaoItemLojaCD;

        $scope.uploader = uploaderFactory.createUploaderStaged(urlUploader, itemStored, null, beforeUpload);
        $scope.uploader.onCompleteAll = function (response, status) {
            $scope.$apply(function () {
                importar();
            })
        };

        search(1);

        // Atualiza a tela caso a infra de processamento detecte alterações
        $scope.$on('processing-notify', function () { search(); });

        function orderBy(field) {
            pagingService.toggleSorting($scope.data.paging, field);
            search();
        }

        function downloadEnabled(item) {
            return (item.state == "ResultsAvailable");
        }

        function download(item) {
            $q.when(processingService.getTicketResults(item.ticket));
        }

        function isAdmin() {
            return userSession.getCurrentUser().role.isAdmin;
        }

        function search(pageNumber) {
            pagingService.calculateOffset($scope.data.paging, pageNumber);

            var processName = isVincular ? 'ImpVincularItemLojaCD' : 'ImpDesvincularItemLojaCD';

            // A consulta já filtra processamentos apenas do usuario caso este nao seja admin.
            $q.when(relacaoItemLojaCDService.obterProcessamentosImportacao(null, processName, null, $scope.data.paging))
                  .then(exibeConsulta)
                  .catch(escondeConsulta);
        }

        function exibeConsulta(data) {
            $scope.data.values = data;
            pagingService.acceptPagingResults($scope.data.paging, data);

            $validation.accept($scope);
        }

        function escondeConsulta() {
            $scope.data.values = [];
        }

        function baixarTemplate() {
            if (isVincular) {
                $q.when(relacaoItemLojaCDService.obterModeloVinculo());
            } else {
                $q.when(relacaoItemLojaCDService.obterModeloDesvinculo());
            }
        }

        function save() {
            if ($scope.uploader.queue.length == 0 || angular.element('input[type=file]').val() == "") {
                toastService.warning(globalization.texts.uploadFileNotFound);
                $scope.uploader.queue = [];
                return;
            }

            if (!angular.element('input[type=file]').val().toUpperCase().endsWith("XLSX")) {
                toastService.warning(globalization.texts.unsupportedFileExtension);
                $scope.uploader.queue = [];
                return;
            }

            $scope.uploader.queue[0].upload();
        }

        function cancel() {
            $state.go('cadastroRelacaoItemLoja');
        }

        function itemStored(item, fileVaultTicket) {

            item.fileVaultTicket = fileVaultTicket;
        }

        function beforeUpload(item) {
            if (item.isInvalid) {
                item.cancel();
            }
        }

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

            var userId = userSessionService.getCurrentUser().id;

            if (isVincular) {
                $q.when(relacaoItemLojaCDService.importarVinculos($scope.data.cdSistema, userId, tickets))
                    .then(sucessoCompartilhado)
                    .catch(erroCompartilhado);
            } else {
                $q.when(relacaoItemLojaCDService.importarDesvinculos($scope.data.cdSistema, userId, tickets))
                    .then(sucessoCompartilhado)
                    .catch(erroCompartilhado);
            }

            search(1);

            $scope.uploader.queue = [];
            angular.element('input[type=file]').val('');
        }

        function sucessoCompartilhado(result) {
            if (!result) return result;
            if (result.sucesso) {
                toastService.success(result.mensagem);
            } else {
                toastService.warning(result.mensagem);
            }
        }

        function erroCompartilhado(result) {
            if (!result) return result;
            if (result.mensagem) {
                toastService.error(result.mensagem);
            }
        }
    }

    // Configuração do estado
    ImpRelacaoItemLojaRoute.$inject = ['$stateProvider'];
    function ImpRelacaoItemLojaRoute($stateProvider) {

        $stateProvider
            .state('impVincularItemLojaCD', {
                url: '/reabastecimento/relacao-item-loja/vincular/:cdSistema',
                templateUrl: 'Scripts/app/reabastecimento/importacao-relacao-item-loja.view.html',
                controller: 'ImpRelacaoItemLojaController',
                resolve: {
                    isVincular: function () {
                        return true;
                    },
                    cdSistema: ['$stateParams', function ($stateParams) {
                        return $stateParams.cdSistema;
                    }],
                    resultado: angular.noop
                }
            })
            .state('impDesvincularItemLojaCD', {
                url: '/reabastecimento/relacao-item-loja/desvincular/:cdSistema',
                templateUrl: 'Scripts/app/reabastecimento/importacao-relacao-item-loja.view.html',
                controller: 'ImpRelacaoItemLojaController',
                resolve: {
                    isVincular: function () {
                        return false;
                    },
                    cdSistema: ['$stateParams', function ($stateParams) {
                        return $stateParams.cdSistema;
                    }],
                    resultado: angular.noop
                }
            })
            .state('impVincularItemLojaCDResults', {
                url: '/reabastecimento/relacao-item-loja/vincular/1/:ticket',
                templateUrl: 'Scripts/app/reabastecimento/importacao-relacao-item-loja.view.html',
                controller: 'ImpRelacaoItemLojaController',
                resolve: {
                    isVincular: function () {
                        return true;
                    },
                    cdSistema: function () {
                        return 1;
                    },
                    resultado: ['ProcessingService', '$stateParams', function (processingService, $stateParams) {
                        return processingService.getTicketResults($stateParams.ticket);
                    }]
                }
            })
            .state('impDesvincularItemLojaCDResults', {
                url: '/reabastecimento/relacao-item-loja/desvincular/1/:ticket',
                templateUrl: 'Scripts/app/reabastecimento/importacao-relacao-item-loja.view.html',
                controller: 'ImpRelacaoItemLojaController',
                resolve: {
                    isVincular: function () {
                        return false;
                    },
                    cdSistema: function () {
                        return 1;
                    },
                    resultado: ['ProcessingService', '$stateParams', function (processingService, $stateParams) {
                        return processingService.getTicketResults($stateParams.ticket);
                    }]
                }
            });
    }
})();