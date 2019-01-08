(function () {
    'use strict';

    angular
        .module('SGP')
        .service('AlcadaService', AlcadaService);

    AlcadaService.$inject = ['$q', '$http', 'ApiEndpoints'];

    function AlcadaService($q, $http, ApiEndpoints) {

        function retornarDados(response) {
            return response.data;
        }

        var self = this;

        self.validarDuplicidadeDetalhe = function (alcada) {
            return $http.post(ApiEndpoints.sgp.alcada + 'ValidarDuplicidadeDetalhe', alcada).then(retornarDados);
        };

        self.obterEstruturadoPorPerfil = function (idPerfil) {
            return $http
                .get(ApiEndpoints.sgp.alcada + ApiEndpoints.encode(idPerfil))
                .then(retornarDados);
        };

        self.salvar = function (alcada) {
            return $http.post(ApiEndpoints.sgp.alcada, alcada).then(retornarDados);
        };
    }
})();