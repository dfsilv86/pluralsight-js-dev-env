(function () {
    'use strict';

    angular
        .module('SGP')
        .service('SugestaoPedidoService', SugestaoPedidoService);

    SugestaoPedidoService.$inject = ['$q', '$http', 'ApiEndpoints'];

    function SugestaoPedidoService($q, $http, ApiEndpoints) {

        function carregaSugestaoPedido(response) {
            return response.data;
        }

        var self = this;

        self.pesquisarPorFiltros = function (request, paging) {

            var params = ApiEndpoints.createParams(paging, request);

            return $http
                .get(ApiEndpoints.sgp.sugestaoPedido, { params: params })
                .then(carregaSugestaoPedido);
        };

        self.alterarSugestoes = function (dtPedido, idLoja, idDepartamento, sugestoes) {

            var alterarSugestoesRequest = {
                dtPedido: dtPedido,
                idLoja: idLoja,
                idDepartamento: idDepartamento,
                sugestoes: sugestoes
            };

            return $http
                .put(ApiEndpoints.sgp.sugestaoPedido, alterarSugestoesRequest).then(function (response) { return response.data; });
        };

        self.ultimaSugestaoAlterada = null;

        self.alterarSugestao = function (sugestao) {

            var sugestoes = [sugestao].map(function (s) {
                return { idSugestaoPedido: s.idSugestaoPedido, vlEstoque: s.vlEstoque, qtdPackCompra: s.qtdPackCompra, qtdPackCompraAlterado: s.qtdPackCompraAlterado };
            });

            return self.alterarSugestoes(sugestao.dtPedido, sugestao.idLoja, sugestao.itemDetalheSugestao.idDepartamento, sugestoes)
            .then(function (result) {
                if (result.sucesso > 0) {
                    self.ultimaSugestaoAlterada = sugestao;
                }

                return result;
            });
        };

        self.validarSugestao = function (dtPedido, idLoja, idDepartamento, sugestao) {

            var validarSugestaoRequest = {
                dtPedido: dtPedido,
                idLoja: idLoja,
                idDepartamento: idDepartamento,
                sugestao: sugestao
            };

            return $http
                .put(ApiEndpoints.sgp.sugestaoPedido + 'ValidarAlteracaoSugestao', validarSugestaoRequest)
                .then(function (response) {
                    var data = response.data;

                    if (!data.satisfied) {
                        return $q.reject(data.reason);
                    }

                    return data;
                });
        };

        self.obterStatusAutorizarPedido = function (dtPedido, idLoja, idDepartamento, cdSistema) {

            var obterStatusAutorizarPedidoRequest = {
                dtPedido: dtPedido,
                idLoja: idLoja,
                idDepartamento: idDepartamento,
                cdSistema: cdSistema
            };

            return $http
                .put(ApiEndpoints.sgp.sugestaoPedido + 'ObterStatusAutorizarPedido', obterStatusAutorizarPedidoRequest, { keepNotifications: true })
                .then(function (response) {
                    return response.data;
                });
        };

        self.autorizarPedido = function (dtPedido, idLoja, idDepartamento, cdSistema) {

            var autorizarPedidoRequest = {
                dtPedido: dtPedido,
                idLoja: idLoja,
                idDepartamento: idDepartamento,
                cdSistema: cdSistema
            };

            return $http
                .put(ApiEndpoints.sgp.sugestaoPedido + 'AutorizarPedido', autorizarPedidoRequest)
                .then(function (response) {
                    return response.data;
                });
        };

        self.obterEstruturado = function (id) {

            return $http
                .get(ApiEndpoints.sgp.sugestaoPedido + ApiEndpoints.encode(id) + '/ComAlcada/')
                .then(carregaSugestaoPedido);
        };

        self.recalcular = function (sugestaoPedido) {
            return $http
                .put(ApiEndpoints.sgp.sugestaoPedido + 'recalcular', sugestaoPedido, { keepNotifications: true }).then(function (response) {
                    return response.data;
                });
        };

        self.obterLogs = function (filter, paging) {
            filter = {
                idEntidade: filter.idEntidade,
                intervalo: filter.intervalo
            };
            var params = ApiEndpoints.createParams(paging, filter);

            return $http
                .get("{0}/{1}/logs".format(ApiEndpoints.sgp.sugestaoPedido, ApiEndpoints.encode(filter.idEntidade)), { params: JSON.flatten(params) })
                .then(carregaSugestaoPedido);
        };

        self.obterQuantidadeSugestaoPedido = function (data) {            
            return $http
                .get(ApiEndpoints.sgp.sugestaoPedido + 'Quantidade', { params: { data: data } })
                .then(function (response) {
                    return response.data;
                });
        };
    }
})();