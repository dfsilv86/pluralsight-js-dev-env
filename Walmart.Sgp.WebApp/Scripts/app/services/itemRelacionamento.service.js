(function () {
    'use strict';

    angular
        .module('SGP')
        .service('ItemRelacionamentoService', ItemRelacionamentoService);

    ItemRelacionamentoService.$inject = ['$q', '$http', 'ApiEndpoints', '$window', 'UserSessionService'];

    function ItemRelacionamentoService($q, $http, apiEndpoints, $window, userSession) {

        function carregaRelacionamentoItemPrincipal(response) {
            if (!$.isArray(response.data)) {
                var relacionamentoSecundarioInsumo = response.data.relacionamentoSecundario.where("tpItem", 0);
                var relacionamentoSecundarioEmbalagem = response.data.relacionamentoSecundario.where("tpItem", 1);

                response.data.relacionamentoSecundario = relacionamentoSecundarioInsumo;
                response.data.relacionamentoSecundarioEmbalagem = relacionamentoSecundarioEmbalagem;
            }

            return response.data;
        }

        function carregarItensRelacionados(response) {
            return response.data;
        }

        var self = this;

        self.obterNovo = function (tipoRelacionamento) {
            var tipoRelacionamentoTemp = sgpFixedValues.getByDescription("tipoRelacionamento", tipoRelacionamento);

            return {
                id: 0,
                relacionamentoSecundario: [],
                relacionamentoSecundarioEmbalagem: [],
                idTipoRelacionamento: tipoRelacionamentoTemp.value,
                cdSistema: null,
                itemDetalhe: { cdItem: null },
                qtProdutoBruto: null,
                pcRendimentoReceita: null,
                qtProdutoAcabado: null,
                psUnitario: null,
                isNew: true
            };
        };

        self.obter = function (id) {            
            return $http
                .get(apiEndpoints.sgp.itemRelacionamento, { params: { id: id } })
                .then(carregaRelacionamentoItemPrincipal);
        };

        self.obterPercentualRendimentoTransformado = function (idItemDetalhe, cdSistema) {

            idItemDetalhe = idItemDetalhe || 0;

            var params = {
                // idItemDetalhe: idItemDetalhe || '',
                cdSistema: cdSistema || ''
            };

            return $http
                .get(apiEndpoints.sgp.itemRelacionamento + apiEndpoints.encode(idItemDetalhe) + '/PercentualRendimentoTransformado', { params: params })
                .then(function (response) { return response.data; });
        };

        self.obterPercentualRendimentoDerivado = function (idItemDetalhe, cdSistema) {

            idItemDetalhe = idItemDetalhe || 0;

            var params = {
                // idItemDetalhe: idItemDetalhe || '',
                cdSistema: cdSistema || ''
            };

            return $http
                .get(apiEndpoints.sgp.itemRelacionamento + apiEndpoints.encode(idItemDetalhe) + '/PercentualRendimentoDerivado', { params: params })
                .then(function (response) { return response.data; });
        };

        self.validarItem = function (itemRelacionamento, itemDetalhe, utilizadoComoSaida) {
            return $http
                .put(apiEndpoints.sgp.itemRelacionamento + 'ValidarPrincipal', { ItemRelacionamento: itemRelacionamento, ItemDetalhe: itemDetalhe, UtilizadoComoSaida: utilizadoComoSaida });
        };

        self.validarAdicaoItemSecundario = function (itemRelacionamento) {
            return $http
                .put(apiEndpoints.sgp.itemRelacionamento + 'ValidarSecundario', itemRelacionamento).then(function () { return true; });
        };       

        self.pesquisarPorTipoRelacionamento = function (paging, tipoRelacionamento, cdItem, dsItem, cdFineLine, cdSubcategoria, cdCategoria, cdDepartamento, cdSistema, idRegiaoCompra) {

            var params = apiEndpoints.createParams(paging, {
                tipoRelacionamento: tipoRelacionamento,
                cdItem: cdItem || '',
                dsItem: dsItem || '',
                cdFineLine: cdFineLine || '',
                cdSubcategoria: cdSubcategoria || '',
                cdCategoria: cdCategoria || '',
                cdDepartamento: cdDepartamento || '',
                cdSistema: cdSistema,
                idRegiaoCompra: idRegiaoCompra || ''
            });

            return $http
                .get(apiEndpoints.sgp.itemRelacionamento, { params: params })
                .then(carregaRelacionamentoItemPrincipal);
        };

        self.salvar = function (entidade) {
            return $http
                .post(apiEndpoints.sgp.itemRelacionamento, entidade)
                .then(carregaRelacionamentoItemPrincipal);
        };

        self.remover = function (entidade) {
            return $http
                .delete(apiEndpoints.sgp.itemRelacionamento, { params: { id: entidade.id } });
        };

        self.exportarRelatorio = function (cdSistema, tipoRelacionamento, cdItem, dsItem, idDepartamento, idCategoria, idSubcategoria, idFineline, idRegiaoCompra) {

            var params = {
                cdSistema: cdSistema,
                tipoRelacionamento: tipoRelacionamento,
                cdItem: cdItem || '',
                idDepartamento: idDepartamento || '',
                idCategoria: idCategoria || '',
                idSubcategoria: idSubcategoria || '',
                idFineline: idFineline || '',
                idRegiaoCompra: idRegiaoCompra,
                dsItem: dsItem || null
            };

            return $http
            .post("{0}ExportarRelatorio".format(apiEndpoints.sgp.itemRelacionamento), params);
        };

        self.obterItensRelacionados = function (cdItem, idLoja) {
            return $http
                .get('{0}PorCodigo/{1}/Relacionados'.format(apiEndpoints.sgp.itemRelacionamento, cdItem), { params: { idLoja: idLoja || '' } })
                .then(carregarItensRelacionados);
        }
    }
})();