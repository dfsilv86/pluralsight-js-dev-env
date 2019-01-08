(function () {
    'use strict';


    // Configuração da controller
    angular
        .module('SGP')
        .config(ExtratoProdutoRoute)
        .controller('ExtratoProdutoController', ExtratoProdutoController);


    // Implementação da controller
    ExtratoProdutoController.$inject = ['$scope', '$q', 'StackableModalService', '$uibModal', 'ValidationService', 'MovimentacaoService', 'NotaFiscalService', 'comportamentoRelatorio', 'EstoqueService', 'ToastService', '$stateParams', '$timeout'];
    function ExtratoProdutoController($scope, $q, $modal, $uibModal, $validation, movimentacaoService, notaFiscalService, comportamentoRelatorio, estoqueService, toastService, $stateParams, $timeout) {

        $validation.prepare($scope);

        $scope.filters = { mesAno: null, cdSistema: null, idBandeira: null, cdLoja: null, cdItem: null };
        $scope.data = { item: null, extrato: null };
        $scope.search = search;
        $scope.clear = clear;
        $scope.detailNf = detailNf;
        $scope.detailMtr = detailMtr;

        $scope.expand = expand;
        $scope.collapse = collapse;
        $scope.canExpand = canExpand;
        $scope.isExpanded = isExpanded;
        $scope.isInventario = isInventario;
        $scope.exportIt = exportIt;

        $scope.comportamentoRelatorio = comportamentoRelatorio;

        clear();

        if (angular.isDefined($stateParams.mesAno) || angular.isDefined($stateParams.cdSistema) || angular.isDefined($stateParams.idBandeira) || angular.isDefined($stateParams.cdLoja) || angular.isDefined($stateParams.cdItem)) {

            $scope.filters.mesAno = (!!$stateParams.mesAno && $stateParams.mesAno !== '0' ? moment($stateParams.mesAno) || null : null);
            $scope.filters.cdSistema = $stateParams.cdSistema || null;
            $scope.filters.idBandeira = $stateParams.idBandeira || null;
            $scope.filters.cdLoja = $stateParams.cdLoja || null;
            $scope.filters.cdItem = $stateParams.cdItem || null;

            // Caso tenha sido informado filtro na url, deixa as lookups e dropdowns propagarem antes de disparar a pesquisa; 
            // como sao dropdowns e lookups sem propagacao via evento, usamos $timeout.
            $timeout(function () { // suppress-validator 
                search();
            }, 1000);
        }

        function search() {

            if (!$validation.validate($scope)) return;

            // Caso alguma linha da grid tenha sido expandida,
            collapse();

            // Caso já exista extrato sendo exibido,
            escondeExtrato();

            // Prepara os parâmetros de pesquisa
            var idLoja = ($scope.filters.loja || {}).idLoja;
            var idItemDetalhe = ($scope.data.item || {}).idItemDetalhe;
            var mesAno = $scope.filters.mesAno;
            var dtIni = moment(mesAno).startOf('month').format('YYYY-MM-DD HH:mm');
            var dtFim = moment(mesAno).endOf('month').format('YYYY-MM-DD HH:mm');

            // Requisição ao serviço
            var deferred = $q
                .when(movimentacaoService.relExtratoProdutoMovimentacao(idLoja, idItemDetalhe, dtIni, dtFim))
                .then(exibeExtrato)    // sucesso - bind do retorno na grid
                .catch(escondeExtrato); // erro - esconde a grid (exibição do toast ocorre via interceptor)

            return deferred;
        }

        function expand(item) {

            var idLoja = ($scope.filters.loja || {}).idLoja;
            var idItemDetalhe = ($scope.data.item || {}).idItemDetalhe;
            var mesAno = $scope.filters.mesAno;
            var dtIni = moment(mesAno).startOf('month').format('YYYY-MM-DD HH:mm');
            var dtFim = moment(mesAno).endOf('month').format('YYYY-MM-DD HH:mm');

            $scope.data.expanded = item;
            $scope.data.detalhe = null;

            var deferred = $q
                .when(movimentacaoService.relExtratoProdutoMovimentacao(idLoja, idItemDetalhe, dtIni, dtFim, item.idInventario))
                .then(exibeDetalhe)
                .catch(collapse);
        }

        function collapse() {
            $scope.data.detalhe = $scope.data.expanded = null;
        }

        function canExpand(item) {
            // TODO: levar esta regra para domain - importante!
            return !!item.idInventario && item.dsTipoMovimentacao.indexOf('Ajuste de Inventário') > -1 && item.dsTipoMovimentacao.indexOf('(Itens Não Inventariados)') === -1;
        }

        function isExpanded(item) {
            return $scope.data.expanded === item;
        }

        function isInventario(item) {
            // TODO: levar esta regra para domain - importante!
            // TODO: diferente do canExpand pq por esta aqui a cor da linha é diferente
            return !isExpanded(item) && (item.dsTipoMovimentacao.toLowerCase().indexOf('ajuste de invent') === 0) && item.dsTipoMovimentacao.indexOf('(Itens Não Inventariados)') === -1;
        }

        function exibeExtrato(data) {
            $scope.data.extrato = data;

            if (data.length > 0) {
                $scope.data.estoqueInicial = data[0].estoqueTeorico - data[0].qtdMovimentacao;
                $scope.data.qtdMovimentacao = data[0].qtdMovimentacao;
            } else {
                $scope.data.estoqueInicial = null;
                $scope.data.qtdMovimentacao = null;
            }

            return data;
        }

        function escondeExtrato() {
            $scope.data.extrato = null;
        }

        function clear() {
            $scope.filters.cdLoja = null;
            $scope.filters.cdItem = null;
            $scope.filters.cdSistema = null;
            $scope.filters.idBandeira = null;
            $scope.filters.mesAno = null;
            $scope.data.item = null;
            $scope.data.extrato = null;
            $scope.data.estoqueInicial = null;
            $scope.data.extrato = null;
            $scope.data.detalhe = null;
            $scope.data.expanded = null;
        }

        function exibeDetalhe(data) {
            $scope.data.detalhe = data || [];

            return data;
        }

        ////function detail(item) {
        ////    item.idNotaFiscal == null ? detailMtr(item) : detailNF(item);
        ////}         

        function detailMtr(item) {
            movimentacaoService.obterEstruturadoPorId(item.idMovimentacao).
                then(function (data) {
                    $modal.open({
                        templateUrl: 'Scripts/app/estoque/extrato/detalhe-mtr.view.html',
                        controller: 'DetalheMtrController',
                        resolve: {
                            movimentacao: data
                        },
                        options: {
                            modalDialogClass: 'detalhe-mtr'
                        }
                    });
                });
        }

        function detailNf(item) {
            notaFiscalService.obterEstruturado(item.idNotaFiscal).
                then(function (data) {
                    $modal.open({
                        templateUrl: 'Scripts/app/notaFiscal/modal-detalhe-nota-fiscal.view.html',
                        controller: 'DetalheNotaFiscalController',
                        resolve: {
                            notaFiscal: data
                        }
                    });
                });
        }

        function exportIt() {
            // O relatório precisa do valor de estoqueInicial, então é feita a pesquisa primeiro para obter esse valor.
            var promise = search();
            
            if (promise != null) {
                promise.then(function (data) {

                    if (data.length == 0) {
                        toastService.warning(globalization.texts.noDataFound);
                    }
                    else {

                        var loja = $scope.filters.loja;
                        var item = $scope.data.item;
                        var estoqueInicial = $scope.data.estoqueInicial + $scope.data.qtdMovimentacao; // TODO: estoqueInicial foi corrigido, arrumar no relatorio para nao usar mais o qtdMovimentacao ou aceitar zero naquele parametro
                        var qtdeMovimentacao = $scope.data.qtdMovimentacao;
                        var mesAno = $scope.filters.mesAno;
                        var dtIni = moment(mesAno).startOf('month').format('YYYY-MM-DD HH:mm');
                        var dtFim = moment(mesAno).endOf('month').format('YYYY-MM-DD HH:mm');

                        estoqueService.exportarRelatorioExtratoProduto(loja, item, estoqueInicial, qtdeMovimentacao, dtIni, dtFim);
                    }
                });
            }
        }
    }

    // Configuração do estado
    ExtratoProdutoRoute.$inject = ['$stateProvider'];
    function ExtratoProdutoRoute($stateProvider) {

        $stateProvider
            .state('extratoProduto', {
                url: '/estoque/extrato?cdSistema&idBandeira&cdLoja&cdItem&mesAno',
                templateUrl: 'Scripts/app/estoque/extrato/extrato-produto.view.html',
                controller: 'ExtratoProdutoController',
                resolve: {
                    comportamentoRelatorio: function () {
                        return false;
                    }
                }
            })
            .state('relatorioExtratoProduto', {
                url: '/estoque/extrato/relatorio',
                templateUrl: 'Scripts/app/estoque/extrato/extrato-produto.view.html',
                controller: 'ExtratoProdutoController',
                resolve: {
                    comportamentoRelatorio: function () {
                        return true;
                    }
                }
            });
    }
})();