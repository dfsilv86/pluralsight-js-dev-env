(function () {
    'use strict';

    angular
        .module('SGP')
        .service('LojaService', LojaService);

    LojaService.$inject = ['$q', '$http', 'ApiEndpoints'];

    function LojaService($q, $http, ApiEndpoints) {

        function carregaLoja(response) {
            // TODO: converter?
            return response.data;
        }

        var self = this;

        self.obterPorLojaUsuarioEBandeira = function (cdLoja, idUsuario, idBandeira, isInitializing) {

            return $http
                .get(ApiEndpoints.sgp.loja, { params: { cdLoja: cdLoja, idUsuario: idUsuario, idBandeira: idBandeira || '' } })
                .then(carregaLoja);
        };

        self.obterEstruturadoPorId = function (idLoja) {

            return $http
                .get(ApiEndpoints.sgp.loja + '/' + ApiEndpoints.encode(idLoja) + '/Estruturado')
                .then(carregaLoja);
        };

        self.pesquisar = function (idUsuario, cdSistema, idBandeira, cdLoja, nmLoja, paging) {

            var params = ApiEndpoints.createParams(paging, {
                idUsuario: idUsuario,
                cdSistema: cdSistema,
                idBandeira: idBandeira || '',
                cdLoja: cdLoja || '',
                nmLoja: nmLoja || ''
            });

            return $http
                .get(ApiEndpoints.sgp.loja, { params: params })
                .then(carregaLoja);
        };

        self.pesquisarPorItemDestinoOrigem = function (filtro, paging) {

            var params = ApiEndpoints.createParams(paging, {
                cdSistema: filtro.cdSistema,
                idItemDetalheDestino: filtro.idDestino,
                idItemDetalheOrigem: filtro.idOrigem,
                idLoja: filtro.idLoja
            });

            return $http
                .get(ApiEndpoints.sgp.loja + 'PesquisarPorItemDestinoOrigem', { params: JSON.flatten(params) })
                .then(carregaLoja);
        };

        self.alterarLoja = function (loja) {

            return $http.post(ApiEndpoints.sgp.loja, loja)
                .then(carregaLoja);
        };
    }
})();