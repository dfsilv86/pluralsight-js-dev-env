(function () {
    'use strict';

    angular
        .module('SGP')
        .service('SistemaService', SistemaService);

    SistemaService.$inject = ['$q', '$http', 'ApiEndpoints'];

    function SistemaService($q, $http, ApiEndpoints) {

        function carregaSistema(response) {
            // TODO: converter em algo diferente?
            return response.data;
        }

        var self = this;

        self.obterPorUsuario = function (idUsuario) {

            return $http
                .get(ApiEndpoints.sgp.sistema, { params: { idUsuario: idUsuario } })
                .then(carregaSistema);
        };
    }
})();