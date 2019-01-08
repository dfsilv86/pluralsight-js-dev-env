(function () {
    'use strict';

    angular
        .module('SGP')
        .service('StatusRevisaoCustoService', StatusRevisaoCustoService);

    StatusRevisaoCustoService.$inject = ['$q', '$http', 'ApiEndpoints'];

    function StatusRevisaoCustoService($q, $http, apiEndpoints) {

        function carregaStatusRevisaoCusto(response) {
            // TODO: converter?
            return response.data;
        }

        var self = this;

        self.obterTodos = function () {

            return $http
                .get(apiEndpoints.sgp.statusRevisaoCusto)
                .then(carregaStatusRevisaoCusto);
        };
    }
})();