(function () {
    'use strict';

    angular
        .module('SGP')
        .service('AutorizaPedidoService', AutorizaPedidoService);

    AutorizaPedidoService.$inject = ['$q', '$http', 'ApiEndpoints'];

    function AutorizaPedidoService($q, $http, apiEndpoints) {

        function obterData(response) {
            return response.data;
        }

        var self = this;      
      
        self.obterAutorizacoesPorSugestaoPedido = function (id, paging) {
            var filtro = {
                'idSugestaoPedido': id
            };
            var params = apiEndpoints.createParams(paging, filtro);

            return $http
                .get(apiEndpoints.sgp.autorizaPedido, { params: params })
                .then(obterData);
        };
    }
})();