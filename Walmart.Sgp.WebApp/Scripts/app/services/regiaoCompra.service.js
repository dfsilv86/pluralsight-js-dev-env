
(function () {
    'use strict';

    angular
        .module('SGP')
        .service('RegiaoCompraService', RegiaoCompraService);

    RegiaoCompraService.$inject = ['$q', '$http', 'ApiEndpoints'];

    function RegiaoCompraService($q, $http, apiEndpoints) {

        function carregaRegiaoCompra(response) {
            // TODO: converter?
            return response.data;
        }

        var self = this;

        self.obterTodos = function () {

            return $http
                .get(apiEndpoints.sgp.regiaoCompra)
                .then(carregaRegiaoCompra);
        };
    }
})();
