(function () {
    'use strict';

    angular
        .module('SGP')
        .service('EstoqueService', EstoqueService);

    EstoqueService.$inject = ['$q', '$http', 'ApiEndpoints', 'UserSessionService'];

    function EstoqueService($q, $http, apiEndpoints, userSession) {

        function carregaEstoque(response) {
            return response.data;
        }

        var self = this;
       
        self.ajustar = function (entidade) {
            var params = {
                id: entidade.id,
                dhAtualizacao: entidade.dhAtualizacao,
                datasDeQuebra: entidade.datasDeQuebra,                 
                idItemDetalhe: entidade.itemDetalhe.idItemDetalhe,
                idLoja: entidade.loja.idLoja,
                motivoAjuste: entidade.motivoAjuste,
                qtEstoqueFisico: entidade.qtEstoqueFisico,
                tipoAjuste: entidade.tipoAjuste
            };
            
            return $http
                .put(apiEndpoints.sgp.estoque + "ajustar", params)
                .then(carregaEstoque);
        };

        self.realizarMtr = function (movimentacaoMtr) {
            return $http
                .put(apiEndpoints.sgp.estoque + "realizarMtr", movimentacaoMtr);
        };

        self.obterUltimoCustoContabilItem = function (idItemDetalhe, idLoja) {
            var params = { idItemDetalhe: idItemDetalhe, idLoja: idLoja };

            return $http
                .get(apiEndpoints.sgp.estoque + "UltimoCustoContabilItem", { params: params })
                .then(carregaEstoque);
        };

        self.obterOsCincoUltimosCustosDoItemPorLoja = function (cdItem, idLoja) {
            var params = { cdItem: cdItem, idLoja: idLoja };

            return $http
                .get(apiEndpoints.sgp.estoque + "UltimosCustosDoItemPorLoja", { params: params })
                .then(carregaEstoque);
        };

        self.exportarRelatorioExtratoProduto = function (loja, item, estoqueInicial, qtdMovimentacao, dtIni, dtFim) {

            var params = {
                nmloja: loja ? loja.nmLoja : '',
                cdLoja: loja ? loja.cdLoja : '',
                cdItem: item ? item.cdItem : '',
                dsItem: item ? item.dsItem : '',
                estoqueInicial: estoqueInicial ? estoqueInicial : '',
                qtdMovimentacao: qtdMovimentacao ? qtdMovimentacao : '',
                idLoja: loja ? loja.idLoja : '',
                dtIni: dtIni ? dtIni : '',
                dtFim: dtFim ? dtFim : '',
                idItem: item ? item.idItemDetalhe : ''
            };

            return $http
             .post("{0}ExportarRelatorioExtratoProduto".format(apiEndpoints.sgp.estoque), params);
        };
    }
})();