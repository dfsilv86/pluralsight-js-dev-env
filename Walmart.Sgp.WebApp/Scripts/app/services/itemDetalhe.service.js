(function () {
    'use strict';

    angular
        .module('SGP')
        .service('ItemDetalheService', ItemDetalheService);

    ItemDetalheService.$inject = ['$q', '$http', 'ApiEndpoints'];

    function ItemDetalheService($q, $http, ApiEndpoints) {

        function sanitizeForGetMethod(obj) {
            var result = {};
            angular.forEach(obj, function (propVal, propName) {
                result[propName] = propVal === undefined ? '' : propVal;
            });

            return result;
        }

        function carregaItemDetalhe(response) {
            // TODO: converter?
            return response.data;
        }

        function carregaCustosMaisRecentes(response) {
            // TODO: converter?
            return response.data;
        }

        function carregaCustosRelacionados(response) {
            // TODO: converter?
            return response.data;
        }

        function carregaItensDetalheReturnSheet(response) {
            // TODO: converter?
            return response.data;
        }

        var self = this;

        self.obter = function (id) {

            return $http
                .get(ApiEndpoints.sgp.itemDetalhe + ApiEndpoints.encode(id))
                .then(carregaItemDetalhe);
        };

        self.obterEstruturado = function (id) {

            return $http
                .get(ApiEndpoints.sgp.itemDetalhe + ApiEndpoints.encode(id) + '/Estruturado')
                .then(carregaItemDetalhe);
        };

        self.obterPorOldNumberESistema = function (cdItem, cdSistema, isInitializing) {

            return $http
                .get(ApiEndpoints.sgp.itemDetalhe, { params: { cdItem: cdItem, cdSistema: cdSistema } })
                .then(carregaItemDetalhe);
        };

        self.obterPorOldNumberESistemaEDepartamento = function (cdItem, cdSistema, cdDepartamento, isInitializing) {

            return $http
                .get(ApiEndpoints.sgp.itemDetalhe, { params: { cdItem: cdItem, cdSistema: cdSistema, cdDepartamento: cdDepartamento } })
                .then(carregaItemDetalhe);
        };

        self.obterPorPluESistema = function (cdPLU, cdSistema, isInitializing) {

            return $http
                .get(ApiEndpoints.sgp.itemDetalhe, { params: { cdPLU: cdPLU, cdSistema: cdSistema } })
                .then(carregaItemDetalhe);
        };

        self.obterItemEntradaPorItemSaida = function (cdSistema, cdItemSaida, idFornecedorParametro, isInitializing) {
            var params = {
                cdSistema: cdSistema || '',
                cdItemSaida: cdItemSaida || '',
                idFornecedorParametro: idFornecedorParametro || ''
            };

            return $http
                .get(ApiEndpoints.sgp.itemDetalheItemEntradaPorItemSaida, { params: params })
                .then(carregaItemDetalhe);
        };

        self.obterItemSaidaPorFornecedorItemEntrada = function (cdItem, cdSistema, cdDepartamento, idFornecedorParametro, IDRegiaoCompra, somenteStatus, blPerecivel) {
            var params = {
                cdItem: cdItem || '',
                cdSistema: cdSistema || '',
                cdDepartamento: cdDepartamento || '',
                idFornecedorParametro: idFornecedorParametro || '',
                IDRegiaoCompra: IDRegiaoCompra || '',
                tpStatus: somenteStatus || '',
                blPerecivel: blPerecivel || ''
            };

            return $http
                .get(ApiEndpoints.sgp.itemDetalheItemSaidaPorFornecedorItemEntrada, { params: params })
                .then(carregaItemDetalhe);
        };

        self.obterListaItemSaidaPorFornecedorItemEntrada = function (filtro, paging) {

            var params = ApiEndpoints.createParams(paging, {
                cdItem: filtro.cdItem,
                cdSistema: filtro.cdSistema,
                idFornecedorParametro: filtro.idFornecedorParam,
                cdPlu: filtro.cdPlu,
                dsItem: filtro.dsItem,
                tpStatus: filtro.tpStatus,
                cdFineline: filtro.cdFineline,
                cdSubcategoria: filtro.cdSubcategoria,
                cdCategoria: filtro.cdCategoria,
                cdDepartamento: filtro.cdDepartamento,
                IDRegiaoCompra: filtro.IDRegiaoCompra,
                blPerecivel: filtro.blPerecivel
            });

            return $http
                .get(ApiEndpoints.sgp.itemDetalheListaItemSaidaPorFornecedorItemEntrada, { params: params })
                .then(carregaItemDetalhe);
        };

	    self.pesquisarItensEntradaPorSaidaCD = function (cdItem, cdCD, cdSistema, paging) {

                var params = ApiEndpoints.createParams(paging, {
                    cdItem: cdItem || '',
                    cdCD: cdCD || '',
                    cdSistema: cdSistema || 1
                });

                return $http
                    .get(ApiEndpoints.sgp.itemDetalhe, { params: params })
                    .then(carregaItemDetalhe);
            };

	    self.pesquisarItensSaidaComCDConvertido = function (cdItem, cdDepartamento, cdSistema, idCD, filtroMS, filtroCadastro, paging) {

                var params = ApiEndpoints.createParams(paging, {
                    cdItem: cdItem || '',
                    cdDepartamento: cdDepartamento || '',
                    cdSistema: cdSistema || 1,
                    idCD: idCD || '',
                    filtroMS: filtroMS || 1,
                    filtroCadastro: filtroCadastro || 1
                });

                return $http
                    .get(ApiEndpoints.sgp.itemDetalhe, { params: params })
                    .then(carregaItemDetalhe);
        };

	    self.pesquisarPorFiltro = function (cdItem, cdPLU, dsItem, tpStatus, cdFineLine, cdSubcategoria, cdCategoria, cdDepartamento, cdSistema, idUsuario, tpVinculado, paging) {

            var params = ApiEndpoints.createParams(paging, {
                cdItem: cdItem || '',
                cdPLU: cdPLU || '',
                dsItem: dsItem || '',
                tpStatus: tpStatus || '',
                cdFineLine: cdFineLine || '',
                cdSubcategoria: cdSubcategoria || '',
                cdCategoria: cdCategoria || '',
                cdDepartamento: cdDepartamento || '',
                cdSistema: cdSistema || '',
                idUsuario: idUsuario || '',
                tpVinculado: tpVinculado || ''
            });

            return $http
                .get(ApiEndpoints.sgp.itemDetalhe, { params: params })
                .then(carregaItemDetalhe);
        };

        // TODO: camel case (p minusculo)
        self.PesquisarPorFiltroTipoReabastecimento = function (cdItem, cdPLU, dsItem, tpStatus, cdFineLine, cdSubcategoria, cdCategoria, cdDepartamento, cdSistema, idUsuario, idRegiaoCompra, paging) {

            var params = ApiEndpoints.createParams(paging, {
                cdItem: cdItem || '',
                cdPLU: cdPLU || '',
                dsItem: dsItem || '',
                tpStatus: tpStatus || '',
                cdFineLine: cdFineLine || '',
                cdSubcategoria: cdSubcategoria || '',
                cdCategoria: cdCategoria || '',
                cdDepartamento: cdDepartamento || '',
                cdSistema: cdSistema || '',
                idUsuario: idUsuario || '',
                idRegiaoCompra: idRegiaoCompra || ''
            });

            return $http
                .get(ApiEndpoints.sgp.itemDetalheTipoReabastecimento, { params: params })
                .then(carregaItemDetalhe);
        };

        self.alterarDadosCustos = function (itemDetalhe) {

            return $http.put(ApiEndpoints.sgp.itemDetalhe + '/' + ApiEndpoints.encode(itemDetalhe.idItemDetalhe || 0) + '/Custos', itemDetalhe).then(function () { return true; });
        };

        self.alterarInformacoesCadastrais = function (itemDetalhe) {
            return $http.put(ApiEndpoints.sgp.itemDetalhe + '/' + ApiEndpoints.encode(itemDetalhe.idItemDetalhe || 0) + '/InformacoesCadastrais', itemDetalhe).then(function () { return true; });
        };

        self.obterUltimoCustoDoItemPorLoja = function (idItemDetalhe, idLoja, paging) {

            var params = ApiEndpoints.createParams(paging, {
                idLoja: idLoja || ''
            });

            return $http
                .get(ApiEndpoints.sgp.itemDetalhe + ApiEndpoints.encode(idItemDetalhe || 0) + '/Custos/MaisRecentesPorLoja', { params: params })
                .then(carregaCustosMaisRecentes);
        };

        self.obterOsCincoUltimosRecebimentosDoItemPorLoja = function (idItemDetalhe, idLoja) {

            return $http
                .get(ApiEndpoints.sgp.itemDetalhe + ApiEndpoints.encode(idItemDetalhe || 0) + '/Custos/Loja/' + (idLoja || 0).toString() + '/UltimosCincoRecebimentos')
                .then(carregaCustosMaisRecentes);
        };

        self.obterUltimoCustoDeItensRelacionadosNaLoja = function (idItemDetalhe, idLoja) {

            return $http
                .get(ApiEndpoints.sgp.itemDetalhe + ApiEndpoints.encode(idItemDetalhe || 0) + '/Custos/Loja/' + (idLoja || 0).toString() + '/CustosRelacionados')
                .then(carregaCustosRelacionados);
        };

        self.obterItensDetalheReturnSheet = function (relacionamentoSGP, cdDepartamento, cdItemDetalhe, cdV9D, idRegiaoCompra, cdSistema, idReturnSheet, paging) {
            var params = ApiEndpoints.createParams(paging, {
                relacionamentoSGP: relacionamentoSGP,
                cdDepartamento: cdDepartamento,
                cdItemDetalhe: cdItemDetalhe,
                cdV9D: cdV9D,
                idRegiaoCompra: idRegiaoCompra,
                cdSistema: cdSistema,
                idReturnSheet: idReturnSheet
            });

            return $http
               .get(ApiEndpoints.sgp.itemDetalhe, { params: params })
               .then(carregaItensDetalheReturnSheet);
        };

        self.consultarPorFiltro = function (filtro, paging) {
            var filtroPronto = sanitizeForGetMethod(filtro);
            var params = ApiEndpoints.createParams(paging, filtroPronto);

            return $http
                .get(ApiEndpoints.sgp.itemDetalhe + 'Estruturado', { params: params })
                .then(carregaItemDetalhe);
        };

        self.obterInformacoesEstoquePorLoja = function (cdItem, idBandeira, idLoja, paging) {
            var filtroPronto = sanitizeForGetMethod({
                cdItem: cdItem,
                idBandeira: idBandeira,
                idLoja: idLoja
            });

            var params = ApiEndpoints.createParams(paging, filtroPronto);

            return $http
                .get(ApiEndpoints.sgp.itemDetalhe + 'Estoque', { params: params })
                .then(carregaItemDetalhe);
        };

        function carregarItemDetalheComBandeira(response) {
            var result = response.data;
            if (response.data && response.data.itemDetalhe && response.data.bandeira) {
                response.data.itemDetalhe.bandeira = response.data.bandeira;
                result = response.data.itemDetalhe;
            }

            return result;
        }

        self.obterInformacoesCadastrais = function (cdItem, idBandeira) {
            var params = {
                cdItem: cdItem,
                idBandeira: idBandeira
            };

            return $http.get(ApiEndpoints.sgp.itemDetalhe + 'InformacoesCadastrais', { params: params })
                .then(carregarItemDetalheComBandeira);
        };

        self.obterItemCustos = function (cdItem, idBandeira, idLoja, paging) {
            var filtroPronto = sanitizeForGetMethod({
                cdItem: cdItem,
                idBandeira: idBandeira,
                idLoja: idLoja
            });
            var params = ApiEndpoints.createParams(paging, filtroPronto);

            return $http
                .get(ApiEndpoints.sgp.itemDetalhe + 'Custos', { params: params })
                .then(carregaItemDetalhe);
        };

        self.validarIntervaloNaoDeveExcederSessentaDias = function (periodo) {

            var params = {
                periodo: periodo || ''
            };

            return $http
                .get(ApiEndpoints.sgp.itemDetalhe + 'ValidarIntervaloNaoDeveExcederSessentaDias', { params: JSON.flatten(params) });
        };

        self.exportarRelatorioItensAbcVendas = function (loja, idLoja, deptoCat, lblDeptoCat, idDepartamento, idCategoria, periodo) {

            var params = {
                loja: loja || '',
                idLoja: idLoja || '',
                deptoCat: deptoCat || '',
                lblDeptoCat: lblDeptoCat || '',
                idDepartamento: idDepartamento || '',
                idCategoria: idCategoria || '',
                periodo: periodo || ''
            };

            return $http
                .post("{0}ExportarRelatorioItensAbcVendas".format(ApiEndpoints.sgp.itemDetalhe), params);
        };

        self.obterTraitsPorItem = function (idItemDetalhe, cdSistema, paging) {

            var params = ApiEndpoints.createParams(paging, { idItemDetalhe: idItemDetalhe, cdSistema: cdSistema });

            return $http
                .get(ApiEndpoints.sgp.itemDetalheTraitsPorItem, { params: params })
                .then(function (response) {
                    return response.data;
                });
        }
    }
})();