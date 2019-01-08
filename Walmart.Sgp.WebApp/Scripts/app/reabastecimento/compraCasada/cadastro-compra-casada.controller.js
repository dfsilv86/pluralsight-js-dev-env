(function () {
    'use strict';

    // Configuração da controller
    angular
        .module('SGP')
        .config(CadastroCompraCasadaRoute)
        .controller('CadastroCompraCasadaController', CadastroCompraCasadaController);


    // Implementação da controller
    CadastroCompraCasadaController.$inject = ['$scope', '$q', '$timeout', '$stateParams', '$state', 'UserSessionService', 'ValidationService', 'PagingService', 'ChangeTrackerFactory', 'CompraCasadaService', 'idDepartamento', 'cdSistema', 'cdItem', 'fornecedorParametro', 'item', 'StackableModalService', 'ngDialog', 'ToastService', 'naoPossuiCadastro', 'compraCasada'];
    function CadastroCompraCasadaController($scope, $q, $timeout, $stateParams, $state, userSession, $validation, pagingService, changeTrackerFactory, compraCasadaService, idDepartamento, cdSistema, cdItem, fornecedorParametro, item, $modal, ngDialog, toastService, naoPossuiCadastro, compraCasada) {

        var changeTracker = null;

        initialize();

        function initialize() {
            changeTracker = changeTrackerFactory.createChangeTrackerForProperties(['paiCompraCasada', 'filhoCompraCasada'], function (left, right) {
                return !!left && !!right && left.idItemDetalhe === right.idItemDetalhe;
            });

            changeTracker.track(compraCasada);

            $validation.prepare($scope);

            $scope.filters = {
                naoPossuiCadastro: (naoPossuiCadastro.data != true),
                idDepartamento: idDepartamento,
                cdSistema: cdSistema,
                cdItem: cdItem,
                fornecedorParametro: fornecedorParametro,
                item: item
            };

            $scope.filters.idFornecedorParametro = $scope.filters.fornecedorParametro.idFornecedorParametro;

            $scope.data = { values: null, compraCasadaPaginado: [], compraCasadaTodos: compraCasada };

            $scope.data.paging = { offset: 0, limit: 10, orderBy: null };
            $scope.search = search;
            $scope.orderBy = orderBy;
            $scope.toExport = toExport;
            $scope.toExportWithoutChanges = toExportWithoutChanges;
            $scope.save = save;
            $scope.back = back;
            $scope.viewTraits = viewTraits;
            $scope.hasChanges = hasChanges;
            $scope.discardChanges = discardChanges;
            $scope.backWithoutChanges = backWithoutChanges;
            $scope.marcouUmPai = marcouUmPai;
            $scope.marcouUmFilho = marcouUmFilho;
            $scope.remove = remove;
            $scope.salvarDesabilitado = salvarDesabilitado;
            $scope.applyPagingInMemory = applyPagingInMemory;

            $scope.data.compraCasadaPaginado.offset = 0;
            $scope.data.compraCasadaPaginado.limit = 10;
            $scope.data.compraCasadaPaginado.totalCount = 0;

            setVendor(fornecedorParametro);

            $timeout(function () {  // suppress-validator
                applyPagingInMemory(1);
            }, 30);
        }

        function remove() {
            var idDepartamento = $scope.filters.idDepartamento || '';
            var cdSistema = $scope.filters.cdSistema;
            var idFornecedorParametro = $scope.filters.fornecedorParametro.idFornecedorParametro || '';
            var idItemDetalheSaida = (($scope.filters.item || {}).idItemDetalhe || $scope.filters.item.idItemDetalhe) || '';
            var possuiCadastro = '';

            $q.when(compraCasadaService.excluir(idDepartamento, cdSistema, idFornecedorParametro, idItemDetalheSaida, possuiCadastro))
                .then(excluiu)
                .catch(function (reason) {
                    toastService.error(reason);
                });
        }

        function excluiu() {
            $state.go('pesquisaCompraCasada', { msg: globalization.texts.successRemovingCompraCasada });
        }

        function getQtdItensMarcados() {
            var qtd = 0;
            angular.forEach($scope.data.compraCasadaTodos, function (item) {
                if (item.paiCompraCasada || item.filhoCompraCasada) {
                    qtd = qtd + 1;
                }
            });
            return qtd;
        }

        function marcouUmFilho(item) {
            var qtdItensMarcados = getQtdItensMarcados();
            if (qtdItensMarcados > 0) {

                var itens = changeTracker.getChangedItems();

                var idDepartamento = $scope.filters.idDepartamento || '';
                var cdSistema = $scope.filters.cdSistema;
                var idFornecedorParametro = $scope.filters.fornecedorParametro.idFornecedorParametro || '';
                var idItemDetalheSaida = (($scope.filters.item || {}).idItemDetalhe || $scope.filters.item.idItemDetalhe) || '';
                var blPossuiCadastro = '';

                $q.when(compraCasadaService.validarItemFilhoMarcado(itens, idDepartamento, cdSistema, idFornecedorParametro, idItemDetalheSaida, blPossuiCadastro))
                    .then(function (response) {
                        if (response.data) {
                            item.filhoCompraCasada = null;

                            var mensagens = '<b>' + globalization.texts.theFollowingErrorsWereFound + '</b>:<ul>';
                            angular.forEach(response.data, function (msg) {
                                mensagens = mensagens + '<li>' + msg.reason + '</li>';
                            });
                            mensagens = mensagens + '</ul>';

                            toastService.error(mensagens);
                        }
                });
            }
        }

        function marcouUmPai(item) {
            if (item.paiCompraCasada == true) {
                var itens = changeTracker.getChangedItems();

                var idDepartamento = $scope.filters.idDepartamento || '';
                var cdSistema = $scope.filters.cdSistema;
                var idFornecedorParametro = $scope.filters.fornecedorParametro.idFornecedorParametro || '';
                var idItemDetalheSaida = (($scope.filters.item || {}).idItemDetalhe || $scope.filters.item.idItemDetalhe) || '';
                var naoPossuiCadastro = $scope.filters.naoPossuiCadastro;

                $q.when(compraCasadaService.verificaPossuiPai(item, itens, idDepartamento, cdSistema, idFornecedorParametro, idItemDetalheSaida, ''))
                    .then(function (response) {
                        if (response.data) {
                            confirmarDesmarcarPai(item, response.data);
                        } else {
                            $q.when(compraCasadaService.validarItemFilhoMarcado(itens, idDepartamento, cdSistema, idFornecedorParametro, idItemDetalheSaida, naoPossuiCadastro))
                                .then(function (resp) {

                                    if (resp.data) {
                                        item.paiCompraCasada = null;

                                        var mensagens = '<b>' + globalization.texts.theFollowingErrorsWereFound + '</b>:<ul>';
                                        angular.forEach(resp.data, function (msg) {
                                            mensagens = mensagens + '<li>' + msg.reason + '</li>';
                                        });
                                        mensagens = mensagens + '</ul>';

                                        toastService.error(mensagens);
                                    }
                                });
                        }
                    });
            }
        }

        function confirmarDesmarcarPai(item, elemento) {
            var dialog = ngDialog.open({
                template: 'Scripts/app/alertas/confirmar-view.html',
                controller: ['$scope', function ($scope) {
                    $scope.message = globalization.texts.confirmEditParentItem;
                }]
            });
            dialog.closePromise.then(function (data) {
                if (data.value == true) {
                    var dialog2 = ngDialog.open({
                        template: 'Scripts/app/alertas/confirmar-view.html',
                        controller: ['$scope', function ($scope) {
                            $scope.message = globalization.texts.confirmEditParentItemChangeChild;
                        }]
                    });
                    dialog2.closePromise.then(function (data) {
                        if (data.value == true) {
                            desmarcarPaiMarcado(elemento);
                        } else {
                            item.paiCompraCasada = false;
                        }
                    });
                } else {
                    item.paiCompraCasada = false;
                }
            });
        }

        function desmarcarPaiMarcado(paiAnterior) {

            //desmarca o pai anterior
            angular.forEach($scope.data.compraCasadaTodos, function (elementoFilho) {
                if (paiAnterior.idItemDetalhe == elementoFilho.idItemDetalhe) {
                    elementoFilho.paiCompraCasada = null;
                }
            });

            //funcao que desmarca os filhos
            angular.forEach($scope.data.compraCasadaTodos, function (elementoFilho) {
                elementoFilho.filhoCompraCasada = false;
            });

            applyPagingInMemory();
        }

        function salvarDesabilitado() {
            if (hasChanges() == false) {
                return true;
            } else {
                return false;
            }
        }

        function hasChanges() {
            return changeTracker.hasChanges();
        }

        function discardChanges() {
            changeTracker.undoAll();
        }

        function itemExists(item, listItens) {
            var result = false;

            angular.forEach(listItens, function (val) {
                if (val.idItemDetalhe == item.idItemDetalhe) {
                    result = true;
                }
            });

            return result;
        }

        function save() {

            var itens = changeTracker.getChangedItems();

            var idDepartamento = $scope.filters.idDepartamento || '';
            var cdSistema = $scope.filters.cdSistema;
            var idFornecedorParametro = $scope.filters.fornecedorParametro.idFornecedorParametro || '';
            var idItemDetalheSaida = (($scope.filters.item || {}).idItemDetalhe || $scope.filters.item.idItemDetalhe) || '';
            var naoPossuiCadastro = $scope.filters.naoPossuiCadastro;

            $q.when(compraCasadaService.salvar(itens, idDepartamento, cdSistema, idFornecedorParametro, idItemDetalheSaida, !naoPossuiCadastro))
                .then(salvou)
                .catch(function (reason) {
                    toastService.error(reason);
                });
        }

        function salvou(resp) {
            if (resp.data) {
                var mensagens = '<b>' + globalization.texts.theFollowingErrorsWereFound + '</b>:<ul>';
                angular.forEach(resp.data, function (msg) {
                    mensagens = mensagens + '<li>' + msg.reason + '</li>';
                });
                mensagens = mensagens + '</ul>';

                toastService.error(mensagens);
            } else {
                $state.go('pesquisaCompraCasada', { msg: globalization.texts.compraCasadaSavedSuccessfully });
            }
        }

        function viewTraits(item) {
            if (item.traits > 0) {
                $modal.open({
                    templateUrl: 'Scripts/app/reabastecimento/compraCasada/modal-lista-traits.view.html',
                    controller: 'ModalListaTraitsController',
                    resolve: {
                        itemDetalhe: item
                    }
                });
            }
        }

        function backWithoutChanges() {
            if (!$scope.hasChanges()) {
                back();
            }
        }

        function back() {
            $state.go('pesquisaCompraCasada', { msg: null });
        }

        function toExportWithoutChanges() {
            if (!$scope.hasChanges()) {
                toExport();
            }
        }

        function toExport() {
            if (!$validation.validate($scope)) return;

            var idDepartamento = $scope.filters.idDepartamento || '';
            var cdSistema = $scope.filters.cdSistema;
            var idFornecedorParametro = $scope.filters.fornecedorParametro.idFornecedorParametro || '';
            var idItemDetalheSaida = (($scope.filters.item || {}).idItemDetalhe || $scope.filters.cdItem) || '';

            $q.when(compraCasadaService.exportar(idDepartamento, cdSistema, idFornecedorParametro, idItemDetalheSaida, ''));
        }

        function exibe(data) {
            changeTracker.track(data);
            $scope.data.compraCasadaTodos = data;

            applyPagingInMemory();

            $scope.filters.didSearch = true;
            $validation.accept($scope);
        }

        function verificaPossuiCadastro() {
            var idDepartamento = $scope.filters.idDepartamento || '';
            var cdSistema = $scope.filters.cdSistema;
            var idFornecedorParametro = $scope.filters.fornecedorParametro.idFornecedorParametro || '';
            var idItemDetalheSaida = (($scope.filters.item || {}).idItemDetalhe || $scope.filters.item.idItemDetalhe) || '';

            $q.when(compraCasadaService.verificaPossuiCadastro(idDepartamento, cdSistema, idFornecedorParametro, idItemDetalheSaida, ''))
                .then(function (data) {
                    $scope.filters.naoPossuiCadastro = (data.data.resultado != true);
                })
                .catch(function (reason) {
                    toastService.error(reason);
                });
        }

        function esconde(data) {
            $scope.data.compraCasadaTodos = [];
            $scope.filters.didSearch = false;
        }

        function search(pageNumber) {
            var idDepartamento = $scope.filters.idDepartamento || '';
            var cdSistema = $scope.filters.cdSistema;
            var idFornecedorParametro = $scope.filters.fornecedorParametro.idFornecedorParametro || '';
            var idItemDetalheSaida = (($scope.filters.item || {}).idItemDetalhe || $scope.filters.item.idItemDetalhe) || '';
            var possuiCadastro = '';

            $q.when(compraCasadaService.pesquisarItensEntrada(idDepartamento, cdSistema, idFornecedorParametro, idItemDetalheSaida, possuiCadastro, { offset: 0, limit: 999999999, orderBy: $scope.data.compraCasadaPaginado.orderBy }))
              .then(exibe)
              .catch(esconde);
        }

        function orderBy(field) {
            if ($scope.data.compraCasadaPaginado != null && $scope.data.compraCasadaPaginado.length > 0) {
                $scope.data.compraCasadaPaginado.orderBy = ($scope.data.compraCasadaPaginado.orderBy || '').indexOf(field + ' asc') >= 0 ?
                    field + ' desc' :
                    field + ' asc';

                search();
            }
        }

        function setVendor(roteiro) {
            var vendor = {
                cdSistema: $scope.filters.cdSistema,
                cdV9D: $scope.filters.fornecedorParametro.cdV9D,
                fornecedor: {
                    nmFornecedor: $scope.filters.fornecedorParametro.fornecedor.nmFornecedor,
                    cdFornecedor: $scope.filters.fornecedorParametro.fornecedor.cdFornecedor
                }
            };

            $scope.filters.vendor = vendor;
            $scope.filters.cdV9D = $scope.filters.fornecedorParametro.cdV9D;
        }

        function applyPagingInMemory(pageNumber) {
            var paged = $scope.data.compraCasadaPaginado;
            var source = $scope.data.compraCasadaTodos || [];

            pagingService.inMemoryPaging(pageNumber, source, paged);
        }
    }

    // Configuração do estado
    CadastroCompraCasadaRoute.$inject = ['$stateProvider'];
    function CadastroCompraCasadaRoute($stateProvider) {

        $stateProvider
            .state('cadastroCompraCasada', {
                url: '/reabastecimento/compraCasada/cadastro-compra-casada/:cdSistema/:idDepartamento/:idFornecedorParametro/:cdItem/:idItemDetalheSaida',
                templateUrl: 'Scripts/app/reabastecimento/compraCasada/cadastro-compra-casada.view.html',
                controller: 'CadastroCompraCasadaController',
                resolve: {
                    idDepartamento: ['$stateParams', function ($stateParams) {
                        return $stateParams.idDepartamento;
                    }],
                    cdSistema: ['$stateParams', function ($stateParams) {
                        return $stateParams.cdSistema;
                    }],
                    cdItem: ['$stateParams', function ($stateParams) {
                        return $stateParams.cdItem;
                    }],
                    idItemDetalheSaida: ['$stateParams', function ($stateParams) {
                        return $stateParams.idItemDetalheSaida;
                    }],
                    fornecedorParametro: ['$stateParams', 'FornecedorParametroService', function ($stateParams, fornecedorParametroService) {
                        return fornecedorParametroService.obterEstruturado($stateParams.idFornecedorParametro);
                    }],
                    item: ['$stateParams', 'ItemDetalheService', function ($stateParams, itemDetalheService) {
                        return itemDetalheService.obterPorOldNumberESistema($stateParams.cdItem, $stateParams.cdSistema, true);
                    }],
                    naoPossuiCadastro: ['$stateParams', 'CompraCasadaService', function ($stateParams, compraCasadaService) {
                        return compraCasadaService.verificaPossuiCadastro($stateParams.idDepartamento, $stateParams.cdSistema, $stateParams.idFornecedorParametro, $stateParams.idItemDetalheSaida, '');
                    }],
                    compraCasada: ['$stateParams', 'CompraCasadaService', function ($stateParams, compraCasadaService) {
                        return compraCasadaService.pesquisarItensEntrada($stateParams.idDepartamento, $stateParams.cdSistema, $stateParams.idFornecedorParametro, $stateParams.idItemDetalheSaida, '', { offset: 0, limit: 999999999, orderBy: null });
                    }]
                }
            });
    }
})();