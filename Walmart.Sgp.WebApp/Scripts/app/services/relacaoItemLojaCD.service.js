(function () {
    'use strict';

    angular
        .module('SGP')
        .service('RelacaoItemLojaCDService', RelacaoItemLojaCDService);

    RelacaoItemLojaCDService.$inject = ['$q', '$http', 'ApiEndpoints', 'DownloadAttachment'];

    function RelacaoItemLojaCDService($q, $http, ApiEndpoints, downloadAttachment) {

        function carregaRelacaoItemLojaCD(response) {
            // TODO: converter?
            return response.data;
        }

        function carregarLojasValidasItem(response) {
            // TODO: converter?
            return response.data;
        }

        function success(response) {
            return response.data;
        }
        
        var self = this;

        self.obterProcessamentosImportacao = function (userId, processName, state, paging) {

            var params = ApiEndpoints.createParams(paging, { userId: userId || '', processName: processName || '', state: state || '' });

            return $http
                .get(ApiEndpoints.sgp.relacaoItemLojaCD + 'ObterProcessamentosImportacao/', { params: params })
                .then(success);
        };

        self.importarVinculos = function (cdSistema, idUsuario, arquivos) {
            var params = {
                cdSistema: cdSistema || '',
                idUsuario: idUsuario || '',
                arquivos: arquivos || []
            };

            return $http
                .post(ApiEndpoints.sgp.relacaoItemLojaCD + 'ImportarVinculos', params)
                .then(carregaRelacaoItemLojaCD);
        };

        self.importarDesvinculos = function (cdSistema, idUsuario, arquivos) {
            var params = {
                cdSistema: cdSistema || '',
                idUsuario: idUsuario || '',
                arquivos: arquivos || []
            };

            return $http
                .post(ApiEndpoints.sgp.relacaoItemLojaCD + 'ImportarDesvinculos', params)
                .then(carregaRelacaoItemLojaCD);
        };
        
        self.obterModeloVinculo = function () {
            return $http
                .get(ApiEndpoints.sgp.relacaoItemLojaCD + 'ObterModeloVinculo')
                .then(downloadAttachment);
        };

        self.obterModeloDesvinculo = function () {
            return $http
                .get(ApiEndpoints.sgp.relacaoItemLojaCD + 'ObterModeloDesvinculo')
                .then(downloadAttachment);
        };

        self.obterPorFiltro = function (filtro, paging) {

            var params = ApiEndpoints.createParams(paging, {
                cdItemSaida: filtro.cdItemSaida,
                dsEstado: filtro.dsEstado,
                idRegiaoCompra: filtro.idRegiaoCompra,
                idBandeira: filtro.idBandeira,
                blVinculado: filtro.blVinculado,
                idFornecedorParametro: !!filtro.vendor ? filtro.vendor.id : '',
                cdSistema: filtro.cdSistema
            });

            return $http
                .get(ApiEndpoints.sgp.relacaoItemLojaCD, { params: params })
                .then(carregaRelacaoItemLojaCD);
        };

        self.salvar = function (entidade) {
            return $http
                .post(ApiEndpoints.sgp.relacaoItemLojaCD, entidade)
                .then(carregaRelacaoItemLojaCD);
        };

        self.obterLojasValidasItem = function (cdItem, cdSistema, idReturnSheet, paging) {
            var params = ApiEndpoints.createParams(paging, {
                cdItem: cdItem,
                cdSistema: cdSistema,
                idReturnSheet: idReturnSheet
            });

            return $http
                .get(ApiEndpoints.sgp.relacaoItemLojaCD, { params: params })
                .then(carregarLojasValidasItem);
        };

        self.exportar = function exportar(filtro){
            var params = {
                cdV9D: filtro.cdV9D || '',
                idRegiaoCompra: filtro.idRegiaoCompra,
                cdItemSaida: filtro.cdItemSaida || '',
                idBandeira: filtro.idBandeira,
                blVinculado: filtro.blVinculado,
                dsEstado: filtro.dsEstado,
                cdSistema: filtro.cdSistema
            };

            return $http
                .get(ApiEndpoints.sgp.exportacaoRelacaoItemLojaCD, { params: params })
                .then(downloadAttachment);
        }

        //self.salvarSugestoes = function (dtPedido, idLoja, idDepartamento, sugestoes) {

        //    var salvarSugestoesRequest = {
        //        dtPedido: dtPedido,
        //        idLoja: idLoja,
        //        idDepartamento: idDepartamento,
        //        sugestoes: sugestoes
        //    };

        //    return $http
        //        .put(ApiEndpoints.sgp.sugestaoPedido, salvarSugestoesRequest);
        //}
    }
})();