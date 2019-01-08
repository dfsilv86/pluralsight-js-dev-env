(function () {
    'use strict';

    angular
        .module('SGP')
        .service('RoteiroService', RoteiroService);

    RoteiroService.$inject = ['$q', '$http', 'ApiEndpoints'];

    function RoteiroService($q, $http, apiEndpoints) {

        var self = this;

        self.salvarSugestaoPedidoConvertidoCaixa = function (sugestoesConvertidas, idRoteiro) {
            return $http.post(apiEndpoints.sgp.roteiro + "/" + idRoteiro, sugestoesConvertidas).then(function (response) {
                return response.data;
            });
        };

        self.obterSugestaoPedidoLoja = function (idRoteiro, dtPedido, idItemDetalhe, paging) {
            var params = apiEndpoints.createParams(paging, {
                idRoteiro: idRoteiro,
                dtPedido: dtPedido,
                idItemDetalhe: idItemDetalhe
            });

            return $http
                .get(apiEndpoints.sgp.roteiro, { params: params })
                .then(function (response) {
                    return response.data;
                });
        };

        self.obterRoteirosPorFornecedor = function (cdV9D, cdDepartamento, cdLoja, roteiro, paging) {
            var params = apiEndpoints.createParams(paging, {
                cdV9D: cdV9D,
                cdDepartamento: cdDepartamento,
                cdLoja: cdLoja,
                roteiro: roteiro
            });

            return $http
                .get(apiEndpoints.sgp.roteiro, { params: params })
                .then(function (response) {
                    return response.data;
                });
        };

        self.obterPorId = function (idRoteiro) {
            var params = {
                idRoteiro: idRoteiro
            };

            return $http
                .get(apiEndpoints.sgp.roteiro, { params: params })
                .then(function (response) {
                    return response.data;
                });
        };

        self.obterEstruturadoPorId = function (idRoteiro) {
            var params = {
                idRoteiro: idRoteiro
            };

            return $http
                .get(apiEndpoints.sgp.roteiro + apiEndpoints.encode(idRoteiro) + '/Estruturado')
                .then(function (response) {
                    return response.data;
                });
        };

        self.salvar = function (roteiro) {
            return $http
                .post(apiEndpoints.sgp.roteiro, roteiro)
                .then(function (response) {
                    return response.data;
                });
        };

        self.remove = function (idRoteiro) {
            return $http
                .delete(apiEndpoints.sgp.roteiro + idRoteiro + "/Delete")
                .then(function (response) {
                    return response.data;
                });
        };
    }
})();
