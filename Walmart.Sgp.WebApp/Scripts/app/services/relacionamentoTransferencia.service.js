(function () {
    'use strict';

    angular
        .module('SGP')
        .service('RelacionamentoTransferenciaService', RelacionamentoTransferenciaService);

    RelacionamentoTransferenciaService.$inject = ['$q', '$http', 'ApiEndpoints'];

    function RelacionamentoTransferenciaService($q, $http, ApiEndpoints) {

        function carregaRelacionamentoTransferencia(response) {
            // TODO: converter?
            return response.data;
        }

        var self = this;

        self.obterEstruturado = function (id) {

            return $http
                .get(ApiEndpoints.sgp.relacionamentoTransferencia + ApiEndpoints.encode(id) + '/Estruturado')
                .then(carregaRelacionamentoTransferencia);
        };

        self.pesquisarPorFiltro = function (filtro, paging) {

            var params = ApiEndpoints.createParams(paging, {
                idBandeira: filtro.idBandeira,
                idLoja: filtro.idLoja,
                idDepartamento: filtro.idDepartamento,
                cdItem: filtro.cdItem,
                dsItem: filtro.dsItem
            });

            return $http
                .get(ApiEndpoints.sgp.relacionamentoTransferencia + 'PesquisarPorFiltro', { params: JSON.flatten(params) })
                .then(carregaRelacionamentoTransferencia);
        };

        self.pesquisarItensRelacionados = function (idItemDetalheDestino, paging) {

            var params = ApiEndpoints.createParams(paging, {
                idItemDetalheDestino: idItemDetalheDestino
            });

            return $http
                .get(ApiEndpoints.sgp.relacionamentoTransferencia + 'PesquisarItensRelacionados', { params: JSON.flatten(params) })
                .then(carregaRelacionamentoTransferencia);
        };

        self.pesquisarItensRelacionadosPorCdItemDestino = function (cdItemDestino, paging) {

            var params = ApiEndpoints.createParams(paging, {
                cdItemDestino: cdItemDestino
            });

            return $http
                .get(ApiEndpoints.sgp.relacionamentoTransferencia + 'PesquisarItensRelacionadosPorCdItemDestino', { params: JSON.flatten(params) })
                .then(carregaRelacionamentoTransferencia);
        };

        self.criarTransferencia = function (idItemDestino, idItemOrigem, lojas) {
            var params = {
                IDItemDetalheDestino: idItemDestino,
                IDItemDetalheOrigem: idItemOrigem,
                Lojas: lojas
            };
            return $http.post(ApiEndpoints.sgp.relacionamentoTransferencia + 'CriarTransferencia', params).then(function () { return true; });
        };

        self.removerTransferencia = function (items) {
            return $http.put(ApiEndpoints.sgp.relacionamentoTransferencia + 'RemoverTransferencia', items).then(function () { return true; });
        };
    }
})();