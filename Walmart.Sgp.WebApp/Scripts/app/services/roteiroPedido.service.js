(function () {
    'use strict';

    angular
        .module('SGP')
        .service('RoteiroPedidoService', RoteiroPedidoService);

    RoteiroPedidoService.$inject = ['$q', '$http', '$window', 'ApiEndpoints', 'UserSessionService', 'DirectRequestService'];

    function RoteiroPedidoService($q, $http, $window, apiEndpoints, userSession, directRequestService) {

        var self = this;

        self.obterDadosAutorizacaoRoteiro = function (idRoteiro, dtPedido) {
            var params = {
                idRoteiro: idRoteiro,
                dtPedido: dtPedido
            };

            return $http
                .get(apiEndpoints.sgp.roteiroPedido + '/DadosAutorizacao/', { params: params })
                .then(function (response) {
                    return response.data;
                });
        };

        self.obterRoteirosPedidosPorRoteiroEdtPedido = function (idRoteiro, dtPedido, paging) {
            var params = apiEndpoints.createParams(paging, {
                idRoteiro: idRoteiro,
                dtPedido: dtPedido
            });

            return $http
                .get(apiEndpoints.sgp.roteiroPedido, { params: params })
                .then(function (response) {
                    return response.data;
                });
        };

        self.calcularTotalPedido = function (idRoteiro, dtPedido, usarQtdRoteiroRA) {
            var params = {
                idRoteiro: idRoteiro,
                dtPedido: dtPedido,
                usarQtdRoteiroRA: usarQtdRoteiroRA
            };

            return $http
                .get(apiEndpoints.sgp.roteiroPedido, { params: params })
                .then(function (response) {
                    return response.data;
                });
        };

        self.obterPorId = function (idRoteiroPedido) {
            var params = {
                idRoteiroPedido: idRoteiroPedido
            };

            return $http
                .get(apiEndpoints.sgp.roteiroPedido, { params: params })
                .then(function (response) {
                    return response.data;
                });
        };

        self.autorizarRoteiroPedido = function (idRoteiro, dtPedido) {
            var params = {
                idRoteiro: idRoteiro,
                dtPedido: dtPedido
            };

            return $http.post(apiEndpoints.sgp.roteiroPedido, params)
                .then(function (response) {
                    return response.data;
                });
        };

        self.autorizarPedidos = function (roteiros, dtPedido) {
            var r = roteiros;
            r[0].dtPedido = dtPedido;

            return $http.put(apiEndpoints.sgp.roteiroPedido, r).then(function (response) {
                return response.data;
            });
        };

        self.obterPedidosRoteirizados = function (dtPedido, idDepartamento, cdV9D, stPedido, roteiro, paging) {
            var params = apiEndpoints.createParams(paging, {
                dtPedido: dtPedido,
                idDepartamento: idDepartamento,
                cdV9D: cdV9D || '',
                stPedido: stPedido || '',
                roteiro: roteiro || ''
            });

            return $http
                .get(apiEndpoints.sgp.roteiroPedido, { params: params })
                .then(function (response) {
                    return response.data;
                });
        };

        self.qtdPedidosNaoAutorizadosParaDataCorrente = function (idRoteiro) {
            var params = {
                idRoteiro: idRoteiro
            };

            return $http
                .get(apiEndpoints.sgp.roteiroPedido + "NaoAutorizados", { params: params })
                .then(function (response) {
                    return response.data;
                });
        };

        self.exportarRelatorio = function (idRoteiro, dsRoteiro, nmVendor, cdVendor, dtPedido, idItemDetalhe) {
            var params = {
                IDRoteiro: idRoteiro,
                DsRoteiro: dsRoteiro,
                NmVendor: nmVendor,
                CdVendor: cdVendor,
                DtPedido: dtPedido,
                IDItemDetalhe: idItemDetalhe,
                token: userSession.getToken()
            };

            $http
                .post("{0}ExportarRelatorio".format(apiEndpoints.sgp.roteiroPedido), params)
                .then(function (response) { if (response !== null) return response.data; });
        };
    }
})();
