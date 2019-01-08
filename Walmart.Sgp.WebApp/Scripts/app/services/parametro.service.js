(function () {
    'use strict';

    angular
        .module('SGP')
        .service('ParametroService', ParametroService);

    ParametroService.$inject = ['$q', '$http', 'ApiEndpoints', 'ToastService'];

    function ParametroService($q, $http, apiEndpoints, toastService) {

        function carregarParametro(response) {

            var data = response.data;
            
            if (null === data) {
                toastService.error(globalization.texts.missingSystemParameter)
                return $q.reject(globalization.texts.missingSystemParameter);
            }

            return data;
        }

        function parametroSalvo(response) {
            return response.data;
        }

        var self = this;

        self.obterEstruturado = function () {

            return $http
                .get("{0}estruturado".format(apiEndpoints.sgp.parametro))
                .then(carregarParametro);
        };

        self.salvar = function (entidade) {
            return $http
                .post(apiEndpoints.sgp.parametro, entidade)
                .then(parametroSalvo);
        };
    }
})();