(function () {
    'use strict';

    angular
        .module('SGP')
        .service('RegiaoService', RegiaoService);

    RegiaoService.$inject = ['$q', '$http', 'ApiEndpoints'];

    function RegiaoService($q, $http, ApiEndpoints) {

        function carregaRegiao(response) {
            // TODO: converter?
            return response.data;
        }

        var self = this;

        self.obterPorBandeira = function (idBandeira) {

            return $http
                .get(ApiEndpoints.sgp.regiao + 'PorBandeira/' + ApiEndpoints.encode(idBandeira))
                .then(carregaRegiao);
        };
    }
})();