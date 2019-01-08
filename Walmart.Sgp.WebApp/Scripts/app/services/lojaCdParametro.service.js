(function () {
    'use strict';

    angular
        .module('SGP')
        .service('LojaCdParametroService', LojaCdParametroService);

    LojaCdParametroService.$inject = ['$q', '$http', 'ApiEndpoints'];

    function LojaCdParametroService($q, $http, apiEndpoints) {

        function carregaLojaCdParametro(response) {
            return response.data;
        }

        var self = this;

        self.pesquisarPorFiltros = function (filtro, paging) {

            var params = apiEndpoints.createParams(paging, filtro);

            return $http
                .get(apiEndpoints.sgp.lojaCdParametro, { params: params })
                .then(carregaLojaCdParametro);
        };

        self.obterEstruturadoPorId = function (id, tpReabastecimento) {

            return $http
                .get("{0}/{1}/estruturado?tpReabastecimento={2}".format(apiEndpoints.sgp.lojaCdParametro, apiEndpoints.encode(id), tpReabastecimento))
                .then(carregaLojaCdParametro);
        };
      
        self.salvar = function (entidade) {
            return $http
                .post(apiEndpoints.sgp.lojaCdParametro, entidade)
                .then(carregaLojaCdParametro);
        };

    }
})();