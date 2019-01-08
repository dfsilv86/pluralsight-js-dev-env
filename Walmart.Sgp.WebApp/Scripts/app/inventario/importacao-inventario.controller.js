(function () {
    'use strict';


    // Configuração da controller
    angular
        .module('SGP')
        .config(ImportacaoInventarioRoute)
        .controller('ImportacaoInventarioController', ImportacaoInventarioController);


    // Implementação da controller
    ImportacaoInventarioController.$inject = [ '$scope', '$stateParams', '$state', 'ValidationService', 'ToastService', 'InventarioService', 'UploaderFactory', 'ApiEndpoints', '$q', 'prefixosArquivos', 'resultado' ];
    function ImportacaoInventarioController($scope, $stateParams, $state, $validation, toastService, inventarioService, uploaderFactory, apiEndpoints, $q, prefixosArquivos, resultado) {

        $validation.prepare($scope);

        $scope.data = { cdSistema: null, idBandeira: null, cdLoja: null, dataInventario: null, loja: null };

        $scope.importar = importar;
        $scope.removerArquivo = removerArquivo;

        $scope.uploader = uploaderFactory.createUploaderStaged(apiEndpoints.sgp.inventarioUploadManual, itemStored, null, beforeUpload);
        $scope.uploader.onCompleteAll = function () { $scope.$apply(function () { importarManual(); }) };
        $scope.uploader.onAfterAddingFile = function (item) {
            var prefixoValido = false;
            for (var i = 0; i < prefixosArquivos.length && !prefixoValido; i++) {
                prefixoValido = prefixoValido || item.file.name.toUpperCase().startsWith(prefixosArquivos[i].toUpperCase());
            }
            item.isInvalid = !prefixoValido;
            if (item.isInvalid) {
                item.cancel();
            }
        };

        if (!!resultado) {
            $q.when(resultado).then(sucessoCompartilhado).catch(erroCompartilhado);
            $state.go('ImportacaoInventario', null, { reset: true });
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

        function importar() {

            if (!$validation.validate($scope)) return;

            if (!$scope.uploader.queue || !$scope.uploader.queue.length) {

                toastService.warning(globalization.getText('requiresInventoryFiles', true));

            } else {

                $scope.uploader.uploadAll();
            }
        }

        function importarManual() {

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

            $q.when(inventarioService.importarManual($scope.data.cdSistema, $scope.data.idBandeira, $scope.data.loja.idLoja, $scope.data.dataInventario, tickets))
                .then(sucessoCompartilhado)
                .catch(erroCompartilhado);
        }

        function itemStored(item, fileVaultTicket) {

            item.fileVaultTicket = fileVaultTicket;
        }

        function removerArquivo(item) {
            item.remove();
        }

        function beforeUpload(item) {
            if (item.isInvalid) {
                item.cancel();
            }
        }

        function clear() {
            $scope.data = {
                idBandeira: null,
                cdLoja: null,
                dataInventario: null,
                arquivos: []
            };
        }

        clear();

        $scope.$watch('data.loja.id', function (newValue, oldValue) {
            var loja = $scope.data.loja;

            if (loja != null) {
                if (newValue != null) {
                    inventarioService.obterDataInventarioDaLoja(loja.id)
                        .then(function (data) {
                            $scope.data.dataInventario = data;
                        });

                    return;
                }
            }

            $scope.data.dataInventario = null;
        });
    }

    // Configuração do estado
    ImportacaoInventarioRoute.$inject = ['$stateProvider'];
    function ImportacaoInventarioRoute($stateProvider) {

        $stateProvider
            .state('ImportacaoInventario', {
                url: '/inventario/importacao',
                templateUrl: 'Scripts/app/inventario/importacao-inventario.view.html',
                controller: 'ImportacaoInventarioController',
                resolve: {
                    prefixosArquivos: ['InventarioService', function (inventarioService) {
                        return inventarioService.obterPrefixosArquivos();
                    }],
                    resultado: angular.noop
                }
            })
            .state('ImportarManualResults', {
                url: '/inventario/importacao/resultado/:ticket',
                templateUrl: 'Scripts/app/inventario/importacao-inventario.view.html',
                controller: 'ImportacaoInventarioController',
                resolve: {
                    prefixosArquivos: ['InventarioService', function (inventarioService) {
                        return inventarioService.obterPrefixosArquivos();
                    }],
                    resultado: ['ProcessingService', '$stateParams', function (processingService, $stateParams) {
                        return processingService.getTicketResults($stateParams.ticket);
                    }]
                }
            });
    }
})();