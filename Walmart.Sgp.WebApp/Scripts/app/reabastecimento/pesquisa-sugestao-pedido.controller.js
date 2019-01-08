(function () {
    'use strict';


    // Configuração da controller
    angular
        .module('SGP')
        .config(PesquisaSugestaoPedidoRoute)
        .controller('PesquisaSugestaoPedidoController', PesquisaSugestaoPedidoController);


    // Implementação da controller
    PesquisaSugestaoPedidoController.$inject = ['$scope', '$q', '$timeout', '$stateParams', '$state', 'UserSessionService', 'ValidationService', 'PagingService', 'ChangeTrackerFactory', 'SugestaoPedidoService', 'ToastService', 'BackgroundRequestFactory', 'OrigemCalculoService'];
    function PesquisaSugestaoPedidoController($scope, $q, $timeout, $stateParams, $state, userSession, $validation, pagingService, changeTrackerFactory, sugestaoPedidoService, toastService, backgroundRequestFactory, origemCalculoService) {

        $validation.prepare($scope);

        $scope.filters = $stateParams.filters || {
            dtPedido: new Date(),
            cdSistema: null,
            cdLoja: null,
            cdVendor: null,
            cdDivisao: null,
            cdDepartamento: null,
            cdCategoria: null,
            cdSubcategoria: null,
            cdFineLine: null,
            cdItem: null,
            cdOrigemCalculo: sgpFixedValues.tipoOrigemSugestao.todos
        };

        $scope.temp = {
            loja: {},
            departamento: {}
        };

        var changeTracker = changeTrackerFactory.createChangeTrackerForProperties(
            ['vlEstoque', 'qtdPackCompra', 'ultimoVlEstoqueAceito', 'ultimoQtdPackCompraAceito'],
            function (left, right) {
                return !!left && !!right && left.idSugestaoPedido == right.idSugestaoPedido;
            });

        if (!!$stateParams.changes) {
            changeTracker.inherit($stateParams.changes);

            if ($stateParams.updated) {
                changeTracker.undoByIdentity(function (item) {
                    return item.idSugestaoPedido == $stateParams.id;
                });
            }
        }

        $scope.data = {
            values: null,
            autorizarPedidoVisivel: false,
            pedidoAutorizado: false,
            podeAutorizarPedido: false,
            gradeSugestoesAberta: false,
            disponibilidade: { automaticoDisponivel: false, manualDisponivel: false }
        };

        $scope.data.paging = $stateParams.paging || { offset: 0, limit: 40, orderBy: 'nmFornecedor ASC, dsItem ASC' };
        $scope.search = search;
        $scope.clear = clear;
        $scope.detail = detail;
        $scope.orderBy = orderBy;
        $scope.save = save;
        $scope.canSave = canSave;
        $scope.hasChanges = hasChanges;
        $scope.validarAlteracaoEstoque = validarAlteracaoEstoque;
        $scope.validarAlteracaoQtd = validarAlteracaoQtd;
        $scope.autorizarPedido = autorizarPedido;
        $scope.podeAutorizarPedidoVisivel = podeAutorizarPedidoVisivel;
        $scope.podeAlterarEstoque = podeAlterarEstoque;
        $scope.podeAlterarQtdPackCompra = podeAlterarQtdPackCompra;
        $scope.discardChanges = discardChanges;

        $scope.data.pesquisado = $stateParams.pesquisado || {
            dtPedido: null,
            loja: {},
            departamento: {},
            cdSistema: null
        };

        $timeout(function () {  // suppress-validator
            // Utiliza os filtros enviados as telas de cadastro e edição.        
            if ($stateParams.filters) {
                search();
            }
            else {
                obterDisponibilidade(new Date(), true);
            }
        }, 2000);

        function obterDisponibilidade(dia, showMessages) {
            return origemCalculoService.obterDisponibilidade(dia)
           .then(function (disponibilidade) {
               $scope.data.disponibilidade = disponibilidade;

               if (showMessages) {
                   if (disponibilidade.automaticoDisponivel) {
                       toastService.success(globalization.texts.thereAreAvailableOrderSuggestion);
                   }
                   else if (disponibilidade.manualDisponivel) {
                       toastService.warning(globalization.texts.thereAreAvailableManualOrderSuggestion);
                   }
                   else {
                       $scope.filters.dtPedido = null;
                       toastService.error(globalization.texts.thereAreNoAvailableOrderSuggestion);
                   }
               }
           });
        }

        function override(newVal, oldVal) {
            if (!newVal && !!oldVal) {
                $scope.filters.cdDivisao = null;
                $scope.filters.cdCategoria = null;
                $scope.filters.cdSubcategoria = null;
                $scope.filters.cdFineLine = null;
                $scope.filters.cdItem = null;
            }
        }

        $scope.$watch('filters.cdDivisao', override);
        $scope.$watch('filters.cdDepartamento', override);
        $scope.$watch('filters.cdCategoria', override);
        $scope.$watch('filters.cdSubcategoria', override);
        $scope.$watch('filters.cdFineLine', override);
        $scope.$watch('filters.cdItem', override);

        function podeAutorizarPedidoVisivel() {
            return $scope.data.autorizarPedidoVisivel && $scope.data.values != null && $scope.data.values.length > 0;
        };

        function verificarStatusPedido(dtPedido, idLoja, idDepartamento, cdSistema) {
            if (dtPedido != null && idDepartamento != null && cdSistema != null) {
                $scope.data.autorizarPedidoVisivel = false;
                $scope.data.podeAutorizarPedido = false;
                $scope.data.pedidoAutorizado = false;

                sugestaoPedidoService
                    .obterStatusAutorizarPedido(dtPedido, idLoja, idDepartamento, cdSistema)
                    .then(function (status) {
                        ajustaStatusAutorizacaoPedido(status);
                    });
            }
        }

        function ajustaStatusAutorizacaoPedido(status) {
            // loja nao permite
            if (status == 'L') {
                $scope.data.autorizarPedidoVisivel = false;
            } else if (status == 'D') {
                // desabilita por algum motivo
                $scope.data.autorizarPedidoVisivel = true;
                $scope.data.podeAutorizarPedido = false;
            } else if (status == 'A') {
                // ja foi autorizado
                $scope.data.autorizarPedidoVisivel = true;
                $scope.data.podeAutorizarPedido = false;
                $scope.data.pedidoAutorizado = true;
            } else if (status == 'P') {
                // pendente - pode autorizar
                $scope.data.autorizarPedidoVisivel = true;
                $scope.data.podeAutorizarPedido = true;
                $scope.data.pedidoAutorizado = false;
            }
        }

        function autorizarPedido() {
            var dtPedido = $scope.data.pesquisado.dtPedido;
            var idLoja = $scope.data.pesquisado.loja.idLoja;
            var idDepartamento = $scope.data.pesquisado.departamento.idDepartamento;
            var cdSistema = $scope.data.pesquisado.cdSistema;

            sugestaoPedidoService
                .autorizarPedido(dtPedido, idLoja, idDepartamento, cdSistema)
                .then(function (status) {
                    ajustaStatusAutorizacaoPedido(status);
                })
                .catch(function () {
                    verificarStatusPedido(dtPedido, idLoja, idDepartamento, cdSistema);
                });
        }

        $scope.$on('$destroy', function () {
            discardChanges();
            changeTracker.reset();
            changeTracker = null;
        });

        var queue = [];

        // TODO: diretiva?
        function validarAlteracaoEstoque(value) {

            queue.push({ value: value, src: 'estoque' });

            dispatch();
        }

        function validarAlteracaoQtd(value) {

            queue.push({ value: value, src: 'qtd' });

            dispatch();
        }

        function dispatch() {

            if (queue.length == 0) return;

            var order = queue.pop();

            var value = order.value;
            var source = order.src;

            if (value.vlEstoque != value.original_vlEstoque || value.qtdPackCompra != value.original_qtdPackCompra) {

                var sugestao = {
                    idSugestaoPedido: value.idSugestaoPedido,
                    idFornecedorParametro: value.idFornecedorParametro,
                    vlEstoque: value.vlEstoque,
                    qtdPackCompra: value.qtdPackCompra,
                    qtdPackCompraAlterado: source === 'qtd'
                };

                $('input[sugestao="' + sugestao.idSugestaoPedido + '"]').addClass('loading');

                sugestaoPedidoService.validarSugestao($scope.filters.dtPedido, $scope.temp.loja.idLoja, $scope.temp.departamento.idDepartamento, sugestao)
                    .catch(function (reason) {
                        // validação de sugestao deve olhar para os valores da sugestao de forma independente
                        if (source === 'estoque') {
                            // Bug 5123:erro de validação do campo Estoque Informado
                            value.vlEstoque = value.original_vlEstoque;
                            value.qtdPackCompra = value.original_qtdPackCompra;
                        } else if (source === 'qtd') {
                            value.qtdPackCompra = value.ultimoQtdPackCompraAceito || value.original_qtdPackCompra;
                        }
                        toastService.warning(reason);
                        if (source === 'estoque') {
                            return $q.reject(reason);
                        }
                    })
                    .then(function (rs) {
                        // validação de sugestao deve olhar para os valores da sugestao de forma independente
                        if (source === 'estoque') {
                            sugestaoPedidoService.recalcular(value)
                            .then(function (recalcularResponse) {
                                value.qtdPackCompra = recalcularResponse.qtdPackCompra;
                                value.ultimoQtdPackCompraAceito = value.qtdPackCompra;
                            });
                        }
                        value.ultimoVlEstoqueAceito = value.vlEstoque;
                        value.ultimoQtdPackCompraAceito = value.qtdPackCompra;
                        return rs;
                    })
                    .finally(function () {
                        $('input[sugestao="' + sugestao.idSugestaoPedido + '"]').removeClass('loading');
                        dispatch();
                    });
            }
        }

        function hasChanges() {
            return changeTracker.hasChanges();
        }

        function discardChanges() {
            // Precisamos esperar por causa da requisição que valida a sugestão no blur dos campos.
            changeTracker.undoAll();
        }

        function clear() {
            $scope.filters.dtPedido = new Date();
            $scope.filters.cdItem = $scope.filters.cdFineLine = $scope.filters.cdSubcategoria =
                $scope.filters.cdCategoria = $scope.filters.cdDepartamento = $scope.filters.cdDivisao = $scope.filters.cdVendor = $scope.filters.cdLoja =
                $scope.filters.cdSistema = null;
            $scope.filters.cdOrigemCalculo = 'T';
            $scope.filters.dataSolicitacao = new Date();
            $scope.data.autorizarPedidoVisivel = false;
            $scope.data.paging.offset = 0;
            $scope.data.values = [];

            discardChanges();
            changeTracker.reset();
        }

        function exibe(data) {

            // Here be dragons
            // validação de sugestao deve olhar para os valores da sugestao de forma independente
            if (!!data) {
                for (var i = 0; i < data.length; i++) {
                    data[i].ultimoVlEstoqueAceito = data[i].vlEstoque;
                    data[i].ultimoQtdPackCompraAceito = data[i].qtdPackCompra;
                }
            }

            data = changeTracker.track(data);

            $scope.data.values = data;

            pagingService.acceptPagingResults($scope.data.paging, data);
        }

        function esconde() {
            $scope.data.values = [];
            $scope.data.autorizarPedidoVisivel = false;

            discardChanges();
            changeTracker.reset();
        }


        function search(pageNumber) {

            if (!$validation.validate($scope)) return;

            obterDisponibilidade($scope.filters.dtPedido, false)
                .then(function () {
                    if (!$scope.data.disponibilidade.manualDisponivel
                    && !$scope.data.disponibilidade.automaticoDisponivel
                    && $scope.filters.dtPedido.isToday()) {
                        toastService.error(globalization.texts.thereAreNoAvailableOrderSuggestion);
                        return;
                    }

                    pagingService.calculateOffset($scope.data.paging, pageNumber);

                    // Prepara os parâmetros de pesquisa
                    $scope.filters.idUsuario = userSession.getCurrentUser().id;

                    var pesquisado = $scope.data.pesquisado;
                    pesquisado.cdSistema = $scope.filters.cdSistema;
                    pesquisado.dtPedido = $scope.filters.dtPedido;
                    pesquisado.departamento = $scope.temp.departamento;
                    pesquisado.loja = $scope.temp.loja;

                    // Requisição ao serviço
                    var deferred = $q
                        .when(sugestaoPedidoService.pesquisarPorFiltros($scope.filters, $scope.data.paging))
                        .then(exibe)
                        .catch(esconde);

                    // Verifica o status do pedido.                    
                    verificarStatusPedido(pesquisado.dtPedido, pesquisado.loja.idLoja, pesquisado.departamento.idDepartamento, pesquisado.cdSistema);
                });
        }

        function save() {

            var sugestoes = [];

            var tracked = changeTracker.getChangedItems();

            angular.forEach(tracked, function (value) {
                sugestoes.push({
                    idSugestaoPedido: value.idSugestaoPedido,
                    idFornecedorParametro: value.idFornecedorParametro,
                    vlEstoque: value.vlEstoque,
                    qtdPackCompra: value.qtdPackCompra
                });
            });

            var deferred = $q
                .when(sugestaoPedidoService.alterarSugestoes($scope.filters.dtPedido, $scope.temp.loja.idLoja, $scope.temp.departamento.idDepartamento, sugestoes))
                .then(function (result) {

                    var idsAlterados = result.idSugestaoPedidoAlterados || [];

                    changeTracker.commitByIdentity(function (item) {
                        return idsAlterados.contains(item.idSugestaoPedido);
                    });

                    changeTracker.undoByIdentity(function (item) {
                        return !idsAlterados.contains(item.idSugestaoPedido);
                    });

                    if (result.sucesso > 0) {
                        toastService.success(result.mensagem);
                    } else {
                        toastService.warning(result.mensagem);
                    }

                    search();
                });
        }

        function orderBy(field) {
            pagingService.toggleSortingUngrouped($scope.data.paging, field);
            search();
        }

        function detail(item) {
            var theChanges = changeTracker.getChangedItems();

            changeTracker.reset();
            $state.update({ 'filters': $scope.filters, 'paging': $scope.data.paging, 'changes': theChanges, 'pesquisado': $scope.data.pesquisado });
            $state.go('cadastroSugestaoPedido', { 'id': item.idSugestaoPedido });
        }

        function recalcular() {
            sugestaoPedidoService.recalcular($scope.data.item).
            then(function (sugestaoPedido) {
                $scope.data.item = sugestaoPedido;
            });
        }

        function podeAlterarEstoque(item) {
            return item.blAlterarInformacaoEstoque && item.cdOrigemCalculo == sgpFixedValues.tipoOrigemSugestao.SGP && !item.blReturnSheet && moment().isSame($scope.filters.dtPedido, 'day');
        }

        function podeAlterarQtdPackCompra(item) {
            return moment().isSame($scope.filters.dtPedido, 'day') && !item.blReturnSheet;
        }

        function canSave() {
            return moment().isSame($scope.filters.dtPedido, 'day');
        }
    }

    // Configuração do estado
    PesquisaSugestaoPedidoRoute.$inject = ['$stateProvider'];
    function PesquisaSugestaoPedidoRoute($stateProvider) {

        $stateProvider
            .state('pesquisaSugestaoPedido', {
                url: '/reabastecimento/sugestao-pedido',
                params: {
                    filters: null,
                    paging: null,
                    changes: null,
                    pesquisado: null,
                    updated: false,
                    id: 0
                },
                templateUrl: 'Scripts/app/reabastecimento/pesquisa-sugestao-pedido.view.html',
                controller: 'PesquisaSugestaoPedidoController'
            });
    }
})();