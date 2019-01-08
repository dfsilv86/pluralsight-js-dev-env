(function () {
    'use strict';

    angular
        .module('SGP')
        .service('NotaFiscalItemStatusService', NotaFiscalItemStatusService);

    NotaFiscalItemStatusService.$inject = ['$q', '$http', 'ApiEndpoints'];

    function NotaFiscalItemStatusService($q, $http, ApiEndpoints) {

        function carregaNotaFiscalItemStatusService(response) {
            // TODO: converter em algo diferente?
            return response.data;
        }

        var self = this;

        self.obterTodos = function () {

            return $http
                .get(ApiEndpoints.sgp.notaFiscalItemStatus)
                .then(carregaNotaFiscalItemStatusService);
        };
    }
})();