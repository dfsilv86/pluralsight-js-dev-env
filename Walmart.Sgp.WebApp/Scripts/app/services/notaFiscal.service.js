(function () {
    'use strict';

    angular
        .module('SGP')
        .service('NotaFiscalService', NotaFiscalService);

    NotaFiscalService.$inject = ['$q', '$http', 'ApiEndpoints'];

    function NotaFiscalService($q, $http, ApiEndpoints) {

        function carregaNotaFiscal(response) {
            return response.data;
        }

        function carregaNotaFiscalItemCustos(response) {
            return response.data;
        }

        function carregaUltimasEntradasNotaFiscal(response) {
            return response.data;
        }

        function existeNotasPendentes(response) {
            return response.data;
        }

        var self = this;

        self.obterEstruturado = function (id) {

            return $http
                .get(ApiEndpoints.sgp.notaFiscal + ApiEndpoints.encode(id) + '/Estruturado')
                .then(carregaNotaFiscal);
        };

        self.obterItensDaNotaFiscal = function (id, paging) {
            var params = ApiEndpoints.createParams(paging, {});

            return $http
                .get(ApiEndpoints.sgp.notaFiscal + ApiEndpoints.encode(id) + '/Itens', { params: params })
                .then(carregaNotaFiscal);
        };

        self.obterCustosPorItem = function (cdLoja, cdItem, dtSolicitacao) {

            return $http
                .get(ApiEndpoints.sgp.notaFiscal + 'ObterCustosPorItem', { params: { cdLoja: cdLoja, cdItem: cdItem, dtSolicitacao: dtSolicitacao } })
                .then(carregaNotaFiscalItemCustos);
        };

        self.existeNotasPendentesPorItem = function (cdLoja, cdItem, dtSolicitacao) {

            return $http
                .get(ApiEndpoints.sgp.notaFiscal + 'NotasPendentes', { params: { cdLoja: cdLoja, cdItem: cdItem, dtSolicitacao: dtSolicitacao } })
                .then(existeNotasPendentes);
        };

        self.pesquisarPorFiltros = function (filtro, paging) {
            var params = ApiEndpoints.createParams(paging, {
                cdSistema: filtro.cdSistema,
                idBandeira: filtro.idBandeira,
                cdLoja: filtro.cdLoja,
                cdFornecedor: filtro.cdFornecedor,
                nrNotaFiscal: filtro.nrNotaFiscal,
                cdItem: filtro.cdItem,
                dtRecebimento: filtro.dtRecebimento,
                dtCadastroConcentrador: filtro.dtCadastroConcentrador,
                dtAtualizacaoConcentrador: filtro.dtAtualizacaoConcentrador
            });
                        
            return $http
                .get(ApiEndpoints.sgp.notaFiscal + 'PesquisarPorFiltros', { params: JSON.flatten(params) })
                .then(carregaNotaFiscal);
        };

        self.pesquisarUltimasEntradasPorFiltro = function (idItemDetalhe, idLoja, dtSolicitacao, paging) {

            var params = ApiEndpoints.createParams(paging, {
                idItemDetalhe: idItemDetalhe,
                idLoja: idLoja,
                dtSolicitacao: dtSolicitacao
            });

            return $http
                .get(ApiEndpoints.sgp.notaFiscal + 'PesquisarUltimasEntradas', { params: params })
                .then(carregaUltimasEntradasNotaFiscal);
        };

        self.pesquisarCustosPorFiltros = function (filtro, paging) {

            var params = ApiEndpoints.createParams(paging, {
                idBandeira: filtro.idBandeira, 
                idLoja: !filtro.loja ? null : filtro.loja.idLoja,
                idFornecedor: !filtro.fornecedor ? null : filtro.fornecedor.idFornecedor,
                nrNotaFiscal: filtro.nrNotaFiscal,
                idItemDetalhe: !filtro.item ? null : filtro.item.idItemDetalhe,
                idDepartamento: !filtro.departamento ? null : filtro.departamento.idDepartamento,
                idNotaFiscalItemStatus: filtro.idNotaFiscalItemStatus,          
                dtRecebimento: filtro.dtRecebimento,
                dtCadastroConcentrador: filtro.dtCadastroConcentrador,
                dtAtualizacaoConcentrador: filtro.dtAtualizacaoConcentrador
            });

            return $http
                .get(ApiEndpoints.sgp.notaFiscal + 'PesquisarCustosPorFiltros', { params: JSON.flatten(params) })
                .then(carregaNotaFiscal);
        };

        self.corrigirCustos = function (custos) {

            var itens = [];

            angular.forEach(custos, function (value) {
                // TODO: talvez seja possível remover o idBandeira
                itens.push({
                    idNotaFiscalItem: value.idNotaFiscalItem,
                    idBandeira: value.idBandeira,
                    blLiberar: value.blLiberar,
                    qtAjustada: value.qtAjustada
                });
            });

            return $http
                .post(ApiEndpoints.sgp.notaFiscal + 'CorrigirCustos', itens);
        };
    }
})();