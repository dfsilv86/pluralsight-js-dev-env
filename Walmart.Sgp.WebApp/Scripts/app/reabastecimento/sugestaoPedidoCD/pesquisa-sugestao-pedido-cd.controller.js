(function () {
    'use strict';

    // Configuração da controller
    angular
        .module('SGP')
        .config(PesquisaSugestaoPedidoCDRoute)
        .controller('PesquisaSugestaoPedidoCDController', PesquisaSugestaoPedidoCDController);

    // Implementação da controller
    PesquisaSugestaoPedidoCDController.$inject = ['$scope', '$q', 'StackableModalService', '$timeout', 'UserSessionService', 'ValidationService', 'PagingService', 'ChangeTrackerFactory', 'SugestaoPedidoCDService', 'ToastService'];
    function PesquisaSugestaoPedidoCDController($scope, $q, $modal, $timeout, userSession, $validation, pagingService, changeTrackerFactory, sugestaoPedidoCDService, toastService) {

        var changeTrackerSelected = changeTrackerFactory.createChangeTrackerForProperties(['selecionado'], function (left, right) {
            return !!left && !!right && left.idSugestaoPedidoCD === right.idSugestaoPedidoCD;
        });

        var changeTrackerEdited = changeTrackerFactory.createChangeTrackerForProperties(['dtEnvioPedidoSerialized', 'dtCancelamentoPedidoSerialized', 'qtdPackCompra'], function (left, right) {
            return !!left && !!right && left.idSugestaoPedidoCD === right.idSugestaoPedidoCD;
        });

        initialize();

        function initialize() {
            $validation.prepare($scope);

            $scope.data = { isLoading: false };
            $scope.data.paging = { offset: 0, limit: 40, orderBy: 'cdItemSugestao, cdItem, dsItem' };

            $scope.filters = {
                idCD: null,
                dtSolicitacao: new Date(),
                cdDepartamento: null,
                cdSistema: 1,
                statusPedido: 2,
                cdV9D: null,
                cdOldNumber: null,
                itemPesoVariavel: 2
            };

            $scope.clear = clear;
            $scope.search = search;
            $scope.applySuggest = applySuggest;
            $scope.save = save;
            $scope.finalize = finalize;
            $scope.edit = edit;
            $scope.toExport = toExport;
            $scope.getDateObject = getDateObject;
            $scope.hasChanges = hasChanges;
            $scope.discardChanges = discardChanges;
            $scope.hasSelection = hasSelection;
            $scope.habilitaEdicao = habilitaEdicao;
            $scope.btnSearch = btnSearch;

            $scope.validateAndRowChanged = validateAndRowChanged;
            $scope.validarDataEnvio = validarDataEnvio;
            $scope.validarDataCancelamento = validarDataCancelamento;

            $scope.didLoad = didLoad;
        }

        function btnSearch() {
            discardChanges();
            discardSelection();
            search(1);
        }

        function didLoad() {
            $timeout(function () {  // suppress-validator
                $scope.data.isLoading = false;
            }, 30);
        }

        function hasChanges() {
            return changeTrackerEdited.hasChanges();
        }

        function hasSelection() {
            return changeTrackerSelected.hasChanges();
        }

        function discardChanges() {
            changeTrackerEdited.undoAll();
        }

        function discardSelection() {
            changeTrackerSelected.undoAll();
        }

        function validarDataEnvio(item) {
            if ($scope.data.isLoading === true) {
                return;
            }

            if (getDateObject(item.dtEnvioPedidoSerialized) == null || getDateObject(item.dtCancelamentoPedidoSerialized) == null ||
                validateDateString(item.dtEnvioPedidoSerialized) == false || validateDateString(item.dtCancelamentoPedidoSerialized) == false) {
                changeTrackerEdited.undoByIdentity(function (x) {
                    return x.idSugestaoPedidoCD == item.idSugestaoPedidoCD;
                });
                return;
            }

            sugestaoPedidoCDService.validarDataEnvio(item)
                    .catch(function (reason) {
                        changeTrackerEdited.undoByIdentity(function (x) {
                            return x.idSugestaoPedidoCD == item.idSugestaoPedidoCD;
                        });

                        recalculate(item);
                        toastService.error(reason);
                    });

            sugestaoPedidoCDService.validarDataCancelamento(item)
                .catch(function (reason) {
                    changeTrackerEdited.undoByIdentity(function (x) {
                        return x.idSugestaoPedidoCD == item.idSugestaoPedidoCD;
                    });

                    toastService.error(reason);
                });
        }

        function validarDataCancelamento(item) {
            if ($scope.data.isLoading === true) {
                return;
            }

            if (getDateObject(item.dtEnvioPedidoSerialized) == null || getDateObject(item.dtCancelamentoPedidoSerialized) == null ||
                validateDateString(item.dtEnvioPedidoSerialized) == false || validateDateString(item.dtCancelamentoPedidoSerialized) == false) {
                changeTrackerEdited.undoByIdentity(function (x) {
                    return x.idSugestaoPedidoCD == item.idSugestaoPedidoCD;
                });
                return;
            }

            sugestaoPedidoCDService.validarDataCancelamento(item)
                    .catch(function (reason) {
                        changeTrackerEdited.undoByIdentity(function (x) {
                            return x.idSugestaoPedidoCD == item.idSugestaoPedidoCD;
                        });

                        setarDatas();
                        recalculate(item);

                        toastService.error(reason);
                    });
        }

        function setarDatas() {
            var hoje = new Date();
            hoje.setHours(0, 0, 0, 0);

            angular.forEach($scope.data.values, function (item, key) {

                var dtEnvioPedido = getDateObject(item.dtEnvioPedidoSerialized);

                if (dtEnvioPedido != null) {
                    if (dtEnvioPedido < hoje && !item.blFinalizado && (!item.qtdPackCompra || item.qtdPackCompra == 0)) {
                        item.dtEnvioPedidoSerialized = getDateVM(hoje);

                        var novaDataCancelamento = new Date(hoje.getFullYear(), hoje.getMonth(), hoje.getDate() + item.vlLeadTime);
                        item.dtCancelamentoPedidoSerialized = getDateVM(novaDataCancelamento);
                    }
                }
            });
        }

        function validateDateString(string) {
            try {
                var dia = string.split('/')[0];
                var mes = string.split('/')[1];
                var ano = string.split('/')[2];

                var dt = new Date(ano, mes - 1, dia, 0, 0, 0, 0);
                return true;
            } 
            catch(err) {
                return false;
            }
        }

        function getDateVM(object) {
            if (object != null) {
                return moment(object).format('DD/MM/YYYY');
            }

            return null;
        }

        function getDateObject(vm) {
            if (vm != null) {
                if (validateDateString(vm)) {
                    var dia = vm.split('/')[0];
                    var mes = vm.split('/')[1];
                    var ano = vm.split('/')[2];

                    return new Date(ano, mes - 1, dia, 0, 0, 0, 0);
                }
            }

            return null;
        }

        function calcularTotalKGUnid(item) {

            var valor = item.qtdPackCompra > 0 ? item.qtdPackCompra : item.qtdPackCompraOriginal;

            if (item.tpCaixaFornecedor === 'F') {
                item.totalKgUn = valor * item.qtVendorPackage;
            }
            else if (item.tpCaixaFornecedor === 'V') {
                item.totalKgUn = valor * item.vlPesoLiquido;
            }
            else {
                item.totalKgUn = valor;
            }

            item.totalKgUn = Math.round(item.totalKgUn);
        }

        function validateAndRowChanged(item) {

            validarDataEnvio(item);

            validarDataCancelamento(item);

            recalculate(item);
        }

        function recalculate(item) {

            if (item.qtdPackCompra < 0) {
                item.qtdPackCompra = 0;
            }

            item.qtdPendente = 
                item.qtdPackCompra === null || item.qtdPackCompra === undefined ?
                item.qtdPackCompraOriginal :
                item.qtdPackCompraOriginal - item.qtdPackCompra;

            if (item.qtdPendente < 0) {
                item.qtdPendente = 0;
            }

            calcularTotalKGUnid(item);
        }

        function recalculateAll() {
            angular.forEach($scope.data.values, function (value, key) {
                recalculate(value);
            });
        }

        function clear() {
            $scope.data.paging.offset = 0;
            $scope.data.values = [];
            $scope.filters.cdOldNumber = $scope.filters.cdV9D = $scope.filters.cdDepartamento = $scope.filters.idCD = null;
            $scope.filters.dtSolicitacao = new Date();
            $scope.filters.cdSistema = 1;
            $scope.filters.statusPedido = 2;
            $scope.filters.itemPesoVariavel = 2;
            discardChanges();
            discardSelection();
            $scope.habilitarExportar = false;
        }

        function habilitaEdicao(item) {
            var sug = $scope.data.values.filter(function (i) {
                return i.idItemDetalheSugestao == item.idItemDetalheSugestao
                                && i.blFinalizado == true;
            });

            if (getDateObject(item.dtEnvioPedidoSerialized) == null) {
                return;
            }

            var hoje = new Date();
            hoje.setHours(0, 0, 0, 0);

            return (sug.count() > 0) && (getDateObject(item.dtPedidoVM) <= hoje);
        }

        function search(pageNumber) {
            if (!$validation.validate($scope))
                return;

            pagingService.calculateOffset($scope.data.paging, pageNumber);

            //discardChanges();
            //discardSelection();

            var dtSolicitacao = $scope.filters.dtSolicitacao || '';
            var idDepartamento = ($scope.filters.departamento || {}).idDepartamento || '';
            var idCD = $scope.filters.idCD || '';
            var idItemDetalhe = ($scope.filters.item || {}).idItemDetalhe || '';
            var idFornecedorParametro = ($scope.filters.vendor || {}).idFornecedorParametro || '';
            var statusPedido = $scope.filters.statusPedido || '';
            var itemPesoVariavel = $scope.filters.itemPesoVariavel || '';

            $scope.data.isLoading = true;

            $q.when(sugestaoPedidoCDService.pesquisar(dtSolicitacao, idDepartamento, idCD, idItemDetalhe, idFornecedorParametro, statusPedido, itemPesoVariavel, $scope.data.paging))
                .then(exibe)
                .catch(esconde);
        }

        function exibe(data) {
            //changeTrackerSelected.undoAll();
            //discardSelection();
            $scope.data.values = data;

            pagingService.acceptPagingResults($scope.data.paging, data);

            setarDatas();

            recalculateAll();

            changeTrackerSelected.track(data);
            changeTrackerEdited.track(data);
            $scope.habilitarExportar = true;
            $validation.accept($scope);
        }

        function esconde() {
            $scope.data.values = [];
        }

        function save() {
            var itensAlterados = changeTrackerEdited.getChangedItems();

            $q.when(sugestaoPedidoCDService.salvarPedidos(itensAlterados))
                .then(saved)
                .catch(function (reason) {
                    changeTrackerEdited.undoAll();
                    recalculateAll();
                });
        }

        function finalize() {
            var itensSelecionados = changeTrackerSelected.getChangedItems();

            if (itensSelecionados.filter(function (i) { return i.qtdPackCompra == 0; }).length > 0) {
                toastService.warning(globalization.texts.suggestionCdWithZero);
                return;
            }

            $q.when(sugestaoPedidoCDService.finalizarPedidos(itensSelecionados))
                .then(finalized)
                .catch(function (reason) {
                    discardSelection();
                    recalculateAll();
                });
        }

        function finalized() {
            discardChanges();
            discardSelection();

            toastService.success(globalization.texts.ordersFinalizedSuccessfully);
            search(1);
        }

        function saved() {

            toastService.success(globalization.texts.ordersSavedSuccessfully);
            search();
        }

        function applySuggest() {
            var itensSelecionados = changeTrackerSelected.getChangedItems();

            angular.forEach(itensSelecionados, function (itemSelecionado) {
                itemSelecionado.qtdPackCompra = itemSelecionado.qtdPackCompraOriginal > 9999 ? 9999 : itemSelecionado.qtdPackCompraOriginal;

                if (itemSelecionado.tpCaixaFornecedor === 'F') {
                    itemSelecionado.totalKgUn = itemSelecionado.qtdPackCompra * itemSelecionado.qtVendorPackage;
                }
                else if (itemSelecionado.tpCaixaFornecedor === 'V') {
                    itemSelecionado.totalKgUn = itemSelecionado.qtdPackCompra * itemSelecionado.vlPesoLiquido;
                }
                else {
                    itemSelecionado.totalKgUn = itemSelecionado.qtdPackCompra;
                }

                itemSelecionado.totalKgUn = Math.round(itemSelecionado.totalKgUn);
            });

            var possuiItensQuantidaMaiorPermitida = itensSelecionados.filter(function (item) {
                return item.qtdPackCompraOriginal > 9999;
            }).length > 0;

            if (possuiItensQuantidaMaiorPermitida) {
                toastService.warning(globalization.texts.cannotCopySuggestedQuantity);
            }

            discardSelection();

            recalculateAll();
        }

        function edit(id) {
            sugestaoPedidoCDService.obterPorId(id)
                .then(function (data) {
                    $modal.open({
                        templateUrl: 'Scripts/app/reabastecimento/sugestaoPedidoCD/modal-detalhe-sugestao-pedido-cd.view.html',
                        controller: 'ModalDetalheSugestaoPedidoCDController',
                        resolve: {
                            sugestaoPedidoCD: data
                        }
                    });
                });
        }

        function toExport() {
            if (!$validation.validate($scope)) return;

            var dtSolicitacao = $scope.filters.dtSolicitacao || '';
            var idDepartamento = ($scope.filters.departamento || {}).idDepartamento || '';
            var idCD = $scope.filters.idCD || '';
            var idItemDetalhe = ($scope.filters.item || {}).idItemDetalhe || '';
            var idFornecedorParametro = ($scope.filters.vendor || {}).idFornecedorParametro || '';
            var statusPedido = $scope.filters.statusPedido || '';

            $q.when(sugestaoPedidoCDService.exportar(dtSolicitacao, idDepartamento, idCD, idItemDetalhe, idFornecedorParametro, statusPedido));
        }

        $scope.$on('$destroy', function () {
            discardChanges();
            discardSelection();
            changeTrackerEdited.reset();
            changeTrackerEdited = null;
            changeTrackerSelected.reset();
            changeTrackerSelected = null;
        });
    }

    // Configuração do estado
    PesquisaSugestaoPedidoCDRoute.$inject = ['$stateProvider'];
    function PesquisaSugestaoPedidoCDRoute($stateProvider) {

        $stateProvider
            .state('pesquisaSugestaoPedidoCD', {
                url: '/reabastecimento/sugestaoPedidoCD/sugestao-pedido-cd',
                templateUrl: 'Scripts/app/reabastecimento/sugestaoPedidoCD/pesquisa-sugestao-pedido-cd.view.html',
                controller: 'PesquisaSugestaoPedidoCDController',
            });
    }
})();
