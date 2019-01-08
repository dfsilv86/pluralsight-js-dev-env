(function () {
    'use strict';

    var sgpApiMethods = {
        login: 'Auth/',
        changePassword: 'User/ChangePassword',
        sistema: 'Sistema/',
        bandeira: 'Bandeira/',
        loja: 'Loja/',        
        itemDetalhe: 'ItemDetalhe/',
        multisourcing: 'Multisourcing/',
        multisourcingUpload: 'Multisourcing/AdicionarArquivoImportacao',
        itemDetalheTipoReabastecimento: 'ItemDetalhe/TipoReabastecimento',
        itemDetalheItemEntradaPorItemSaida: 'ItemDetalhe/ItemEntradaPorItemSaida',
        itemDetalheListaItemSaidaPorFornecedorItemEntrada: 'ItemDetalhe/ListaItemSaidaPorFornecedorItemEntrada',
        itemDetalheItemSaidaPorFornecedorItemEntrada: 'ItemDetalhe/ItemSaidaPorFornecedorItemEntrada',
        itemDetalheTraitsPorItem: 'ItemDetalhe/ObterTraitsPorItem',
        movimentacao: 'Movimentacao/',
        divisao: 'Divisao/',
        departamento: 'Departamento/',
        categoria: 'Categoria/',
        statusItemHost: 'StatusItemHost/',
        subcategoria: 'Subcategoria/',
        fineLine: 'FineLine/',
        fornecedor: 'Fornecedor/',
        fornecedorParametro: 'Fornecedor/Parametro/',
        alcada: 'Alcada/',
        alcadaDetalhe: 'AlcadaDetalhe/',
        papel: 'Papel/',
        itemRelacionamento: 'ItemRelacionamento/',
        regiaoAdministrativa: 'RegiaoAdministrativa',
        regiaoCompra: 'RegiaoCompra/',
        sugestaoPedido: 'SugestaoPedido/',
        lojaCdParametro: 'LojaCdParametro/',
        notaFiscal: 'NotaFiscal/',
        motivoRevisaoCusto: 'MotivoRevisaoCusto/',
        motivoMovimentacao: 'MotivoMovimentacao/',        
        gradeSugestao: 'GradeSugestao/',
        tipoMovimentacao: 'TipoMovimentacao/',
        relacaoItemLojaCD: 'RelacaoItemLojaCD/',        
        uploadVinculoRelacaoItemLojaCD: 'RelacaoItemLojaCD/AdicionarArquivoVinculo',
        uploadDesvinculoRelacaoItemLojaCD: 'RelacaoItemLojaCD/AdicionarArquivoDesvinculo',
        exportacaoRelacaoItemLojaCD: 'RelacaoItemLojaCD/Exportar',
        revisaoCusto: 'RevisaoCusto/',
        notaFiscalItemStatus: 'NotaFiscalItemStatus/',
        estoque: 'Estoque/',
        returnSheet: 'ReturnSheet/',
        returnSheetItemPrincipal: 'ReturnSheetItemPrincipal/',
        returnSheetItemLoja: 'ReturnSheetItemLoja/',
        sugestaoReturnSheet: 'SugestaoReturnSheet/',
        roteiro: 'Roteiro/',
        roteiroPedido: 'RoteiroPedido/',
        roteiroLoja: 'RoteiroLoja/',
        statusRevisaoCusto: 'StatusRevisaoCusto/',
        revisaoCustoPesquisarPorFiltros: 'RevisaoCusto/PesquisarPorFiltros',
        inventario: 'Inventario/',
        inventarioUploadManual: 'Inventario/Importar/Manual/HO/AdicionarArquivo',
        relacionamentoTransferencia: 'RelacionamentoTransferencia/',
        usuario: 'Usuario/',
        processo: 'Processo/',
        autorizaPedido: 'AutorizaPedido/',
        formato: 'Formato/',
        regiao: 'Regiao/',
        distrito: 'Distrito/',
        parametro: 'Parametro/',
        permissao: 'Permissao/',
        fileVault: 'FileVault/',
        sugestaoPedidoCD: "SugestaoPedidoCD/",
        cd: "CD/",
        compraCasada: "CompraCasada/",
        processing: 'Processing/',
        origemCalculo: 'OrigemCalculo/'
    };

    function serialize(obj, prefix) {
        var str = [];
        for (var p in obj) {
            if (obj.hasOwnProperty(p)) {
                var k = prefix ? prefix + "[" + p + "]" : p,
                      v = obj[p];
                str.push(typeof v == "object" ? serialize(v, k) : encodeURIComponent(k) + "=" + encodeURIComponent(v));
            }
        }
        return str.join("&");
    }

    function sanitizeUndefinedValues(target, replaceValue) {
        if (replaceValue == undefined) {
            replaceValue = '';
        }

        var result = {};
        for (var p in target) {
            var val = target[p];
            result[p] = val == undefined ? replaceValue : val;
        }

        return result;
    }

    function ApiEndpointsProvider() {

        var self = this;

        self.$get = ['SpaConfig', 'SgpApiMethods', 'PagingService', function (spaConfig, sgpApiMethods, pagingService) {
            var keys = Object.keys(sgpApiMethods);
            var root = spaConfig.apiHost;
            var result = { sgp: {}};
            if (!root.endsWith('/')) root += '/';
            for (var i = 0; i < keys.length; i++) {
                var theValue = sgpApiMethods[keys[i]];
                if (typeof (theValue) === 'string') {
                    result.sgp[keys[i]] = root + theValue;
                }
            }

            // TODO: manter aqui ou injetar PagingService nos serviços?
            result.createParams = function (pagingInfo, parameters) {
                return pagingService.applyPagingParams(parameters, pagingInfo);
            };

            result.encode = function (value) {
                return angular.isDefined(value) ? encodeURIComponent(value) : value;
            };

            result.serialize = serialize;
            result.sanitizeUndefinedValues = sanitizeUndefinedValues;
            return result;
        }];
    }

    angular
        .module('SGP')
        .constant('SgpApiMethods', sgpApiMethods)
        .provider('ApiEndpoints', ApiEndpointsProvider);

})();