(function () {
    'use strict';

    angular
        .module('SGP')
        .service('SugestaoPedidoCDService', SugestaoPedidoCDService);

    SugestaoPedidoCDService.$inject = ['$q', '$http', 'ApiEndpoints', 'DownloadAttachment'];

    function SugestaoPedidoCDService($q, $http, apiEndpoints, downloadAttachment) {

        var self = this;

        self.salvarPedidos = function (sugestoes) {
            return $http
                .post(apiEndpoints.sgp.sugestaoPedidoCD + 'SalvarPedidos', sugestoes)
                .then(function (response) {
                    return response || response.data;
                });
        };

        self.validarDataCancelamento = function (sugestao) {
            return $http
                .put(apiEndpoints.sgp.sugestaoPedidoCD + 'ValidarDataCancelamento', sugestao).then(function (response) {
                    var data = response.data;

                    if (!data.satisfied) {
                        return $q.reject(data.reason);
                    }

                    return data;
                });
        };

        self.validarDataEnvio = function (sugestao) {
            return $http
                .put(apiEndpoints.sgp.sugestaoPedidoCD + 'ValidarDataEnvio', sugestao).then(function (response) {
                    var data = response.data;

                    if (!data.satisfied) {
                        return $q.reject(data.reason);
                    }

                    return data;
                });
        };

        self.pesquisar = function (dtSolicitacao, idDepartamento, idCD, idItem, idFornecedorParametro, statusPedido, itemPesoVariavel, paging) {

            var filtros = {
                dtSolicitacao: dtSolicitacao,
                idDepartamento: idDepartamento,
                idCD: idCD,
                idItem: idItem,
                idFornecedorParametro: idFornecedorParametro,
                statusPedido: statusPedido,
                itemPesoVariavel: itemPesoVariavel
            };

            var params = apiEndpoints.createParams(paging, filtros);

            return $http
                .get(apiEndpoints.sgp.sugestaoPedidoCD, { params: params })
                .then(function (response) {
                    return response.data;
                });
        };

        self.obterPorId = function (idSugestaoPedidoCD) {
            var params = {
                idSugestaoPedidoCD: idSugestaoPedidoCD
            };

            return $http
                .get(apiEndpoints.sgp.sugestaoPedidoCD, { params: params })
                .then(function (response) {
                    return response.data;
                });
        };

        self.finalizarPedidos = function (sugestoesPedidoCD) {
            return $http
                .post(apiEndpoints.sgp.sugestaoPedidoCD + "FinalizarPedidos", sugestoesPedidoCD)
                .then(function (response) {
                    return response || response.data;
                });
        };

        self.exportar = function (dtSolicitacao, idDepartamento, idCD, idItem, idFornecedorParametro, statusPedido) {
            var params = {
                dtSolicitacao: dtSolicitacao,
                idDepartamento: idDepartamento,
                idCD: idCD,
                idItem: idItem,
                idFornecedorParametro: idFornecedorParametro,
                statusPedido: statusPedido
            };

            return $http
                .get(apiEndpoints.sgp.sugestaoPedidoCD + "Exportar", { params: params })
                .then(downloadAttachment);
        };
    }
})();
