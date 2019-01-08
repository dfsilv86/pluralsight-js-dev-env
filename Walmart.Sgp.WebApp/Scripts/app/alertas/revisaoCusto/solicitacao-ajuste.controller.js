(function () {
    'use strict';

    // Configuração da controller
    angular
        .module('SGP')
        .config(SolicitacaoAjusteCustoRoute)
        .controller('SolicitacaoAjusteCustoController', SolicitacaoAjusteCustoController);

    // Implementação da controller
    SolicitacaoAjusteCustoController.$inject = ['$scope', '$q', 'ValidationService', 'NotaFiscalService', 'RevisaoCustoService', 'UserSessionService', 'ToastService', 'PagingService', '$state'];
    function SolicitacaoAjusteCustoController($scope, $q, $validation, notaFiscalService, service, userSessionService, toast, pagingService, $state) {

        $validation.prepare($scope);

        setupFilters();
        setupData();

        $scope.clear = clear;
        $scope.save = save;
        $scope.collapse = collapse;
        $scope.$watchGroup(['data.cdLoja', 'data.cdItem', 'data.dtSolicitacao'], refresh);

        function setupFilters() {
            $scope.filters = {
                cdSistema: null,
                idBandeira: null,
            };
        }

        function setupData() {
            $scope.data = {
                custos: {
                    ultimoCustoAtual: 0,
                    custoInventario: 0,
                    ultCustoMesAnterior: 0,
                    posEstoqueAtual: 0
                },
                notasPendentes: false,
                cdLoja: null,
                cdItem: null,

                // TODO: isso aqui na verdade devia ser um objeto $scope.data.revisaoCusto com os campos em tela bindados em cima dele
                // para separar os dados que realmente são salvos dos dados que não interessam à revisão custo, tipo a lista de notas ou a paginação.
                IDLoja: null,
                IDItemDetalhe: null,
                IDStatusRevisaoCusto: 1,
                IDMotivoRevisaoCusto: null,
                IDUsuarioSolicitante: userSessionService.getCurrentUser().id,
                vlCustoSolicitado: 0,
                dsMotivo: null,
                dtCriacao: new Date(),
                dtSolicitacao: new Date()
            };

            zerarEntradas();
        }

        function refresh(newValues, oldValues) {
            if ($scope.data.cdLoja != null && $scope.data.cdItem != null && $scope.data.dtSolicitacao != null) {
                search(1, true);
            }
        }

        function zerarEntradas() {
            $scope.data.values = null;
            $scope.data.paging = {
                offset: 0,
                limit: 10,
                orderBy: 'dtRecebimento DESC'
            }
        }

        function zerarCustos() {

            $scope.data.custos = {
                ultimoCustoAtual: 0,
                custoInventario: 0,
                ultCustoMesAnterior: 0,
                posEstoqueAtual: 0
            };
        }

        function zerarNotas() {
            $scope.data.notasPendentes = null;
        }

        function aplicarEntradas(data) {
            $scope.data.values = data;
            pagingService.acceptPagingResults($scope.data.paging, data);
        }

        function aplicarCustos(data) {
            if (data.custoContab != null && data.custoContab > 0) {
                data.ultCustoMesAnterior = data.custoContab;
            }
            $scope.data.custos = data;
        }

        function aplicarNotas(data) {
            $scope.data.notasPendentes = data;
        }

        function search(pageNumber, skipValidation) {

            if (!skipValidation && !$validation.validate($scope)) return;

            pagingService.calculateOffset($scope.data.paging, pageNumber);

            var idLoja = $scope.data.loja.idLoja;
            var cdLoja = $scope.data.cdLoja;
            var idItemDetalhe = $scope.data.item.idItemDetalhe;
            var cdItem = $scope.data.cdItem;
            var dtSolicitacao = $scope.data.dtSolicitacao;

            // Requisição ao serviço para trazer os custos
            $q.when(notaFiscalService.obterCustosPorItem(cdLoja, cdItem, dtSolicitacao))
                .then(aplicarCustos)
                .catch(zerarCustos);

            // Requisição ao serviço para verificar se existem notas pendentes
            $q.when(notaFiscalService.existeNotasPendentesPorItem(cdLoja, cdItem, dtSolicitacao))
                .then(aplicarNotas)
                .catch(zerarNotas);

            // Requisição ao serviço para carregar o grid
            $q.when(notaFiscalService.pesquisarUltimasEntradasPorFiltro(idItemDetalhe, idLoja, dtSolicitacao, $scope.data.paging))
                .then(aplicarEntradas)
                .catch(zerarEntradas);
        }

        function collapse() {
            $scope.data.detalhe = $scope.data.expanded = null;
        }

        function save() {
            if (!$validation.validate($scope)) return;

            // Requisição ao serviço para verificar se existem notas pendentes
            $scope.data.IDLoja = $scope.data.loja.idLoja;
            $scope.data.IDItemDetalhe = $scope.data.item.idItemDetalhe;
            $scope.data.IDStatusRevisaoCusto = 1;

            // TODO: isso aqui na verdade devia ser um objeto $scope.data.revisaoCusto com os campos em tela bindados em cima dele
            // para separar os dados que realmente são salvos dos dados que não interessam à revisão custo, tipo a lista de notas ou a paginação.
            var revisaoCusto = {
                idLoja: $scope.data.IDLoja,
                idItemDetalhe: $scope.data.IDItemDetalhe,
                idStatusRevisaoCusto: $scope.data.IDStatusRevisaoCusto,
                idMotivoRevisaoCusto: $scope.data.IDMotivoRevisaoCusto,
                dtSolicitacao: $scope.data.dtSolicitacao,
                vlCustoSolicitado: $scope.data.vlCustoSolicitado,
                dsMotivo: $scope.data.dsMotivo,
            };

            service
                .salvar(revisaoCusto)
                .then(function (data) {
                    $state.go('solicitacaoAjusteCusto', null, { reload: true });
                    toast.success(globalization.texts.savedSuccessfully);
                });
        }

        function clear() {
            setupData();
        }
    }

    // Configuração do estado
    SolicitacaoAjusteCustoRoute.$inject = ['$stateProvider'];
    function SolicitacaoAjusteCustoRoute($stateProvider) {

        $stateProvider
            .state('solicitacaoAjusteCusto', {
                url: '/alertas/solicitacaoAjusteCusto',
                templateUrl: 'Scripts/app/alertas/revisaoCusto/solicitacao-ajuste.view.html',
                controller: 'SolicitacaoAjusteCustoController'
            });
    }
})();