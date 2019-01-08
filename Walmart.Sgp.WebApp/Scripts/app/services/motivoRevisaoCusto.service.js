(function () {
    'use strict';

    angular
        .module('SGP')
        .service('MotivoRevisaoCustoService', MotivoRevisaoCustoService);

    MotivoRevisaoCustoService.$inject = ['$q', '$http', 'ApiEndpoints'];

    function MotivoRevisaoCustoService($q, $http, apiEndpoints) {

        function carregaMotivoRevisaoCusto(response) {
            // TODO: converter?
            return response.data;
        }

        var self = this;

        self.obterTodos = function () {

            return $http
                .get(apiEndpoints.sgp.motivoRevisaoCusto)
                .then(carregaMotivoRevisaoCusto);
        };
    }
})();