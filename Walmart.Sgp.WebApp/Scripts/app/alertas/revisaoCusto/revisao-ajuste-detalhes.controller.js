(function () {
    'use strict';

    // Configuração da controller
    angular
        .module('SGP')
        .config(RevisaoAjusteDetalhesRoute)
        .controller('RevisaoAjusteDetalhesController', RevisaoAjusteDetalhesController);

    // Implementação da controller
    RevisaoAjusteDetalhesController.$inject = ['$scope', '$q', '$stateParams', '$state', 'ValidationService', 'ToastService', 'NotaFiscalService', 'RevisaoCustoService', 'UserSessionService', 'ItemDetalheService', 'ItemRelacionamentoService', 'revisaoCusto'];
    function RevisaoAjusteDetalhesController($scope, $q, $stateParams, $state, $validation, toast, notaFiscalService, service, userSessionService, itemDetalheService, itemRelacionamentoService, revisaoCusto) {

        $validation.prepare($scope);

        $scope.save = save;
        $scope.back = back;
        $scope.$watchGroup(['data.dtCustoRevisado'], refresh);

        setupData();
        setupFilters();

        carregaCustos($scope.filters);
        verificaNotasPendentes($scope.filters);
        carregaGrid($scope.filters);

        function setupFilters() {
            $scope.filters = {
                cdSistema: revisaoCusto.loja.cdSistema,
                idBandeira: revisaoCusto.loja.idBandeira,
                idLoja: revisaoCusto.idLoja,
                cdLoja: revisaoCusto.loja.cdLoja,
                idItemDetalhe: revisaoCusto.idItemDetalhe,
                cdItem: revisaoCusto.itemDetalhe.cdItem,
                dtSolicitacao: revisaoCusto.dtSolicitacao
            };
        }

        function setupData() {

            // Preenche detalhes da revisão
            $scope.data = {
                loja: revisaoCusto.loja.cdLoja + ' - ' + revisaoCusto.loja.nmLoja,
                departamento: revisaoCusto.itemDetalhe.departamento.cdDepartamento + ' - ' + revisaoCusto.itemDetalhe.departamento.dsDepartamento,
                item: revisaoCusto.itemDetalhe.cdItem + ' - ' + revisaoCusto.itemDetalhe.dsItem,
                dataSolicitacao: revisaoCusto.dtCriacao,
                solicitante: revisaoCusto.usuarioSolicitante.userName,
                idStatus: revisaoCusto.idStatusRevisaoCusto,
                statusSolicitacao: revisaoCusto.statusRevisaoCusto.dsStatus,
                custoSolicitado: revisaoCusto.vlCustoSolicitado,
                dataNovoCusto: revisaoCusto.dtSolicitacao,
                motivo: revisaoCusto.motivoRevisaoCusto.dsMotivo,
                dsMotivo: revisaoCusto.dsMotivo,
                vlCustoRevisado: revisaoCusto.vlCustoRevisado == null ? 0 : revisaoCusto.vlCustoRevisado,
                dtCustoRevisado: revisaoCusto.dtCustoRevisado == null ? null : new Date(revisaoCusto.dtCustoRevisado),
                dsRevisor: revisaoCusto.dsRevisor,
                perdaRendimento: 0,
                notasPendentes: null,
                values: null,
                custos: {
                    ultimoCustoAtual: 0,
                    custoInventario: 0,
                    ultCustoMesAnterior: 0,
                    posEstoqueAtual: 0
                },
                paging: {
                    offset: 0,
                    limit: 10,
                    orderBy: 'dtRecebimento DESC'
                }
            };

            // Preenche tpVinculado, tpReceituario e tpManipulado
            itemDetalheService
                .obterEstruturado(revisaoCusto.idItemDetalhe)
                .then(function (data) {
                    $scope.data.vinculado = data.tpVinculado == 'E' ? 'Entrada' : data.tpVinculado == 'S' ? 'Saída' : null;
                    $scope.data.receituario = data.tpReceituario == 'I' ? 'Insumo' : data.tpReceituario == 'T' ? 'Transformado' : null;
                    $scope.data.manipulado = data.tpManipulado == 'D' ? 'Derivado' : data.tpManipulado == 'P' ? 'Pai' : null;
                });

            // Preenche perda e rendimentos derivado
            itemRelacionamentoService
                .obterPercentualRendimentoDerivado(revisaoCusto.idItemDetalhe, revisaoCusto.loja.cdSistema)
                .then(function (data) {
                    if (data != null)
                        $scope.data.perdaRendimento = data;
                });

            // Preenche perda e rendimentos transformado
            itemRelacionamentoService
                .obterPercentualRendimentoTransformado(revisaoCusto.idItemDetalhe, revisaoCusto.loja.cdSistema)
                .then(function (data) {
                    if (data != null)
                        $scope.data.perdaRendimento = data;
                });
        }

        function carregaCustos(filters) {

            // Requisição ao serviço para trazer os custos
            notaFiscalService
                .obterCustosPorItem(filters.cdLoja, filters.cdItem, filters.dtSolicitacao)
                .then(function (data) {
                    if (data.custoContab != null && data.custoContab > 0) {
                        data.ultCustoMesAnterior = data.custoContab;
                    }
                    $scope.data.custos = data;
                })
                .catch(function () {
                    $scope.data.custos = {
                        ultimoCustoAtual: 0,
                        custoInventario: 0,
                        ultCustoMesAnterior: 0,
                        posEstoqueAtual: 0
                    };
                });
        }

        function verificaNotasPendentes(filters) {

            // Requisição ao serviço para verificar se existem notas pendentes
            notaFiscalService
                .existeNotasPendentesPorItem(filters.cdLoja, filters.cdItem, filters.dtSolicitacao)
                .then(function (data) {
                    $scope.data.notasPendentes = data;
                })
                .catch(function () {
                    $scope.data.notasPendentes = null;
                });
        }

        function carregaGrid(filters) {

            // Requisição ao serviço para carregar o grid            
            $q.when(notaFiscalService.pesquisarUltimasEntradasPorFiltro(filters.idItemDetalhe, filters.idLoja, filters.dtSolicitacao, $scope.data.paging))
                .then(function (data) {
                    $scope.data.values = data;
                    $scope.data.paging = {
                        offset: data.offset,
                        limit: data.limit,
                        orderBy: data.orderBy
                    };
                })
                .catch(function () {
                    $scope.data.values = null;
                    $scope.data.paging = {
                        offset: 0,
                        limit: 10,
                        orderBy: 'dtRecebimento DESC'
                    };
                });
        }

        function refresh(newValues, oldValues) {
            if ($scope.data.dtCustoRevisado != null && newValues[0] != oldValues[0]) {
                $scope.filters.dtSolicitacao = $scope.data.dtCustoRevisado;
                carregaCustos($scope.filters);
                verificaNotasPendentes($scope.filters);
                carregaGrid($scope.filters);
            }
        }

        function save(status) {
            if (!$validation.validate($scope)) return;

            switch (status) {
                case 2:
                    post(2); // Revisado
                    break;
                case 3:
                    post(3); // Em analise
                    break;
                case 4:
                    post(4); // Ajustado
                    break;
            }
        }

        function post(idStatus) {
            revisaoCusto.IDUsuarioRevisor = userSessionService.getCurrentUser().id;
            revisaoCusto.IDStatusRevisaoCusto = idStatus;
            revisaoCusto.dtCustoRevisado = $scope.data.dtCustoRevisado;
            revisaoCusto.vlCustoRevisado = $scope.data.vlCustoRevisado;
            revisaoCusto.dsRevisor = $scope.data.dsRevisor;
            revisaoCusto.dtRevisado = new Date();

            service
                .salvar(revisaoCusto)
                .then(function (data) {
                    refreshData(data);
                    toast.success(globalization.texts.savedSuccessfully);
                });
        }

        function refreshData(data) {
            service.obterEstruturadoPorId(data.idRevisaoCusto).then(function (data) {
                revisaoCusto = data;
                setupData();
            });
        }

        function back() {
            $state.go('revisaoAjusteCusto');
        }
    }

    // Configuração do estado
    RevisaoAjusteDetalhesRoute.$inject = ['$stateProvider'];
    function RevisaoAjusteDetalhesRoute($stateProvider) {
        $stateProvider
            .state('revisaoAjusteCustoEdit', {
                url: '/alertas/revisaoAjusteCusto/edit/:id',
                templateUrl: 'Scripts/app/alertas/revisaoCusto/revisao-ajuste-detalhes.view.html',
                controller: 'RevisaoAjusteDetalhesController',
                resolve: {
                    revisaoCusto: ['$stateParams', 'RevisaoCustoService', function ($stateParams, service) {
                        return service.obterEstruturadoPorId($stateParams.id);
                    }]
                }
            });
    }
})();