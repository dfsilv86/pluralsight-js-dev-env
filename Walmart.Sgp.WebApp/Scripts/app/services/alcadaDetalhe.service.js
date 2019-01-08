(function () {
    'use strict';

    angular
        .module('SGP')
        .service('AlcadaDetalheService', AlcadaDetalheService);

    AlcadaDetalheService.$inject = ['$q', '$http', 'ApiEndpoints'];

    function AlcadaDetalheService($q, $http, ApiEndpoints) {

        function retornarDados(response) {
            return response.data;
        }

        var self = this;

        self.obterPorIdAlcada = function (idAlcada, paging) {
            var params = ApiEndpoints.createParams(paging, {
                idAlcada: idAlcada
            });

            return $http
                .get(ApiEndpoints.sgp.alcadaDetalhe, { params: params })
                .then(retornarDados);
        };
    }
})();