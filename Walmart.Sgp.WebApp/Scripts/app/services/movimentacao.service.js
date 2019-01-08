(function () {
    'use strict';

    angular
        .module('SGP')
        .service('MovimentacaoService', MovimentacaoService);

    MovimentacaoService.$inject = ['$q', '$http', 'ApiEndpoints'];

    function MovimentacaoService($q, $http, ApiEndpoints) {

        function carregaMovimentacao(response) {
            // TODO: converter?
            return response.data;
        }

        var self = this;

		// TODO: separar a chamada do inventário?
        self.relExtratoProdutoMovimentacao = function (idLoja, idItemDetalhe, dtIni, dtFim, idInventario) {

            var params = { idLoja: idLoja || 0, idItemDetalhe: idItemDetalhe || 0, dtIni: dtIni, dtFim: dtFim };

            if (angular.isDefined(idInventario)) {
                params.idInventario = idInventario;
            }

            return $http
                .get(ApiEndpoints.sgp.movimentacao, { params: params })
                .then(carregaMovimentacao);
        };

        self.obterEstruturadoPorId = function (id) {

            return $http
                .get("{0}/{1}/estruturado".format(ApiEndpoints.sgp.movimentacao, ApiEndpoints.encode(id)))
                .then(carregaMovimentacao);
        };

        self.obterDatasDeQuebra = function (idItemDetalhe, idLoja) {
            var params = { idItemDetalhe: idItemDetalhe, idLoja: idLoja };

            return $http
                .get("{0}DatasDeQuebra".format(ApiEndpoints.sgp.movimentacao), { params: params })
                .then(function (response) {
                    return response.data;
                });
        };
    }
})();