/// <reference path="../sgp.fixed-values.js" />
/*global sgpFixedValues*/
/*global globalization*/
(function () {
    'use strict';

    // Implementação da controller    
    function DetalheInventarioController($scope, $q, $stateParams, $state, $modal, toastService, confirm, pagingService, inventarioService, inventario, userSession) {
        var permissoesObtidas = false;
        $scope.data = inventario;
        $scope.inventarioItem = { values: null };
        $scope.inventarioItem.paging = { offset: 0, limit: 10, orderBy: 'vlCustoTotal desc' };

        $scope.sistemas = {
            supercenter: sgpFixedValues.tipoSistema.supercenter,
            samsClub: sgpFixedValues.tipoSistema.samsClub
        };

        $scope.permissoes = {
            exportarComparacaoEstoque: false,
            aprovar: false,
            finalizar: false,
            cancelar: false,
            voltarStatus: false,
            salvarItem: false,
            excluirItem: false,
            alterarItem: {
                reason: '',
                satisfied: false
            }
        };

        $scope.filtroItem = {
            cdOldNumber: null,
            dsItem: null,
            cdPlu: null,
            filtro: 0
        };

        function recarregarDados() {
            inventarioService.obterEstruturado($scope.data.idInventario).then(function (inventario) {
                $scope.data = inventario;
                atualizarStatusAnterior();
                carregarPermissoes();
                buscarItens(1);
            });
        }

        function back() {
            $state.go('pesquisaInventarioGeracao');
        }

        function buscarItens(pageNumber) {
            pagingService.calculateOffset($scope.inventarioItem.paging, pageNumber);
            inventarioService.obterItensEstruturadoPorFiltro(
				$scope.data.idInventario,
				$scope.filtroItem,
				$scope.inventarioItem.paging).then(function (resultado) {
				    $scope.inventarioItem.values = resultado;
				    pagingService.acceptPagingResults($scope.inventarioItem.paging, resultado);
				}).catch(function () {
				    $scope.inventarioItem.values = [];
				});
        }

        function carregarPermissoes() {
            inventarioService.obterPermissoesAlteracao($scope.data).then(function (permissoes) {
                $scope.permissoes = permissoes;
                if (permissoes.mensagemComparacaoEstoque && $scope.data.stInventario !== sgpFixedValues.inventarioStatus.cancelado && $scope.data.stInventario !== sgpFixedValues.inventarioStatus.aprovado && $scope.data.stInventario !== sgpFixedValues.inventarioStatus.finalizado && !permissoesObtidas) {
                    toastService.warning(permissoes.mensagemComparacaoEstoque);
                }

                permissoesObtidas = true;
            });
        }

        function atualizarStatusAnterior() {
            $scope.statusAnterior = $scope.data.stInventario == sgpFixedValues.inventarioStatus.aprovado
            ? sgpFixedValues.inventarioStatus.importado
            : sgpFixedValues.inventarioStatus.aprovado;
        }

        function finalizar() {
            inventarioService.finalizar($scope.data.idInventario).then(function () {
                toastService.success(globalization.texts.inventoryFinishedSuccessfully);
                recarregarDados();
            });
        }

        function aprovar() {
            inventarioService.aprovar($scope.data.idInventario).then(function () {
                toastService.success(globalization.texts.inventoryApprovedSuccessfully);
                recarregarDados();
            });
        }

        function confirmarFinalizacao() {
            inventarioService.obterIrregularidadesInventarioFinalizacao($scope.data.idInventario).then(function (irregularidades) {
                var messageScope = {
                    irregularidades: irregularidades
                };

                confirm.open({
                    messageTemplateUrl: 'Scripts/app/inventario/confirmar-finalizacao.template.html',
                    messageScope: messageScope,
                    yes: finalizar
                });
            });
        }

        function confirmarAprovacao() {
            inventarioService.obterIrregularidadesInventarioAprovacao($scope.data.idInventario).then(function (irregularidades) {
                var messageScope = {
                    irregularidades: irregularidades
                };

                confirm.open({
                    messageTemplateUrl: 'Scripts/app/inventario/confirmar-aprovacao.template.html',
                    messageScope: messageScope,
                    yes: aprovar
                });
            });
        }

        function podeAlterarItem() {
            return $scope.permissoes.excluirItem || $scope.permissoes.salvarItem || $scope.permissoes.cancelar;
        }

        function abrirModal(item) {
            $modal.open({
                templateUrl: 'Scripts/app/inventario/cadastro-inventario-item.view.html',
                controller: 'CadastroInventarioItemController',
                resolve: {
                    inventarioItem: item,
                    inventario: $scope.data
                }
            }).then(function (result) {
                if (result) {
                    recarregarDados();
                }
            });
        }

        function statusValidoParaAlteracao() {
            if (!$scope.permissoes.alterarItem.satisfied) {
                toastService.warning($scope.permissoes.alterarItem.reason);
                return false;
            }

            return true;
        }

        function detalharItem(item) {
            if (!podeAlterarItem()) {
                toastService.warning(globalization.texts.userRoleNotAllowedToAlterItems);
                return;
            }

            if (!statusValidoParaAlteracao()) {
                return;
            }

            inventarioService.obterItemEstruturadoPorId(item.id).then(function (resultado) {
                abrirModal(resultado);
            });
        }

        function adicionarItem() {
            if (!statusValidoParaAlteracao()) {
                return;
            }

            abrirModal(null);
        }

        function cancelarInventario() {
            inventarioService.cancelar($scope.data.idInventario).then(function () {
                toastService.success(globalization.texts.inventoryCanceledSuccessfully);
                recarregarDados();
            });
        }

        function voltarStatus() {
            inventarioService.voltarStatus($scope.data.idInventario).then(function () {
                toastService.success(globalization.texts.inventoryStatusChangedSuccessfully);
                recarregarDados();
            });
        }

        function relatorioComparacaoEstoque() {
            var inventario = $scope.data;

            var loja = "{0} - {1}".format(inventario.loja.cdLoja, inventario.loja.nmLoja);
            var departamento = "{0} - {1}".format(inventario.departamento.cdDepartamento, inventario.departamento.dsDepartamento);

            inventarioService.exportarRelatorioComparacaoEstoque(
                loja,
                inventario.dhInventario,
                inventario.stInventario,
                departamento,
                inventario.idInventario
            );
        }

        function relatorioItensModificados() {
            var inventario = $scope.data;
            var deptoCateg;
            var loja = "{0} - {1}".format(inventario.loja.cdLoja, inventario.loja.nmLoja);

            if (inventario.bandeira.cdSistema == $scope.sistemas.supercenter) {
                deptoCateg = "Depto.:";
            }
            else if (inventario.bandeira.cdSistema == $scope.sistemas.samsClub) {
                deptoCateg = "Categ.:";
            }

            inventarioService.exportarRelatorioItensModificados(
                loja,
                inventario.loja.id,
                deptoCateg,
                inventario.idDepartamento,
                inventario.idCategoria,
                inventario.dhInventario
            );
        }

        $scope.voltarStatus = voltarStatus;
        $scope.cancelarInventario = cancelarInventario;
        $scope.adicionarItem = adicionarItem;
        $scope.confirmarFinalizacao = confirmarFinalizacao;
        $scope.confirmarAprovacao = confirmarAprovacao;
        $scope.detalharItem = detalharItem;
        $scope.carregarPermissoes = carregarPermissoes;
        $scope.back = back;
        $scope.buscarItens = buscarItens;
        $scope.relatorioComparacaoEstoque = relatorioComparacaoEstoque;
        $scope.relatorioItensModificados = relatorioItensModificados;

        atualizarStatusAnterior();
        buscarItens(1);
        carregarPermissoes();
    }

    DetalheInventarioController.$inject = ['$scope', '$q', '$stateParams', '$state', 'StackableModalService', 'ToastService', 'ConfirmService', 'PagingService', 'InventarioService', 'inventario', 'UserSessionService'];

    // Configuração do estado    
    function DetalheItemInventarioRoute($stateProvider) {

        $stateProvider
			.state('detalheInventario', {
			    url: '/inventario/geracao/:id',
			    params: {
			        id: null
			    },
			    templateUrl: 'Scripts/app/inventario/detalhe-inventario.view.html',
			    controller: 'DetalheInventarioController',
			    resolve: {
			        inventario: ['$stateParams', 'InventarioService', function ($stateParams, service) {
			            return service.obterEstruturado($stateParams.id);
			        }]
			    }
			});
    }

    DetalheItemInventarioRoute.$inject = ['$stateProvider'];

    // Configuração da controller
    angular
		.module('SGP')
		.config(DetalheItemInventarioRoute)
		.controller('DetalheInventarioController', DetalheInventarioController);
})();