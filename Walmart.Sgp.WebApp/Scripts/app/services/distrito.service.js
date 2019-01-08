(function () {
    'use strict';

    angular
        .module('SGP')
        .service('DistritoService', DistritoService);

    DistritoService.$inject = ['$q', '$http', 'ApiEndpoints'];

    function DistritoService($q, $http, ApiEndpoints) {

        function carregaDistrito(response) {
            // TODO: converter?
            return response.data;
        }

        var self = this;

        self.salvar = function (distrito) {
            return $http
                .post(ApiEndpoints.sgp.distrito + 'Salvar/', distrito)
                .then(carregaDistrito);
        };

        self.obterPorId = function (idDistrito) {
            return $http
                .get(ApiEndpoints.sgp.distrito + 'ObterPorId/' + ApiEndpoints.encode(idDistrito))
                .then(carregaDistrito);
        };

        self.obterPorRegiao = function (idRegiao) {

            return $http
                .get(ApiEndpoints.sgp.distrito + 'PorRegiao/' + ApiEndpoints.encode(idRegiao))
                .then(carregaDistrito);
        };

        self.pesquisar = function (cdSistema, idBandeira, idRegiao, idDistrito, paging) {

            var params = ApiEndpoints.createParams(paging, {
                cdSistema: cdSistema || '',
                idBandeira: idBandeira || '',
                idRegiao: idRegiao || '',
                idDistrito: idDistrito || ''
            });

            return $http
                .get(ApiEndpoints.sgp.distrito + 'Pesquisar/', { params: params })
                .then(carregaDistrito);
        };
    }
})();