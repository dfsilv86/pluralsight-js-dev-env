(function () {
    'use strict';

    angular
        .module('SGP')
        .service('StatusItemHostService', StatusItemHostService);

    StatusItemHostService.$inject = ['$q', '$http', 'ApiEndpoints'];

    function StatusItemHostService($q, $http, ApiEndpoints) {

        function carregaStatusItemHostService(response) {
            // TODO: converter em algo diferente?
            return response.data;
        }

        var self = this;

        self.obterTodos = function () {

            return $http
                .get(ApiEndpoints.sgp.statusItemHost)
                .then(carregaStatusItemHostService);
        };
    }
})();