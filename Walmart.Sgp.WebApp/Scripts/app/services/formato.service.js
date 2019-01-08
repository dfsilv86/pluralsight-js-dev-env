(function () {
    'use strict';

    angular
        .module('SGP')
        .service('FormatoService', FormatoService);

    FormatoService.$inject = ['$q', '$http', 'ApiEndpoints'];

    function FormatoService($q, $http, ApiEndpoints) {

        function carregaFormato(response) {
            return response.data;
        }

        var self = this;

        self.obterPorSistema = function (cdSistema) {

            return $http
                .get(ApiEndpoints.sgp.formato + 'PorSistema/' + ApiEndpoints.encode(cdSistema))
                .then(carregaFormato);
        };

        self.obterTodos = function () {

            return $http
                .get(ApiEndpoints.sgp.formato)
                .then(carregaFormato);
        };
    }
})();