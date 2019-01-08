(function () {
    'use strict';

    angular
        .module('SGP')
        .service('PapelService', PapelService);

    PapelService.$inject = ['$q', '$http', 'ApiEndpoints'];

    function PapelService($q, $http, ApiEndpoints) {

        function retornarDados(response) {
            // TODO: converter?
            return response.data;
        }

        var self = this;

        self.obterTodos = function (paging) {
            var params = ApiEndpoints.createParams(paging, {});

            return $http
                .get(ApiEndpoints.sgp.papel, { params: params })
                .then(retornarDados);
        };
    }
})();