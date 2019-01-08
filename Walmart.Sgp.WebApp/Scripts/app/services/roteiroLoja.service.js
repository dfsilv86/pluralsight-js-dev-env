(function () {
    'use strict';

    angular
        .module('SGP')
        .service('RoteiroLojaService', RoteiroLojaService);

    RoteiroLojaService.$inject = ['$q', '$http', 'ApiEndpoints'];

    function RoteiroLojaService($q, $http, apiEndpoints) {

        var self = this;

        self.obterLojasValidas = function (cdV9D, dsEstado, idRoteiro, paging) {
            var params = apiEndpoints.createParams(paging, {
                cdV9D: cdV9D,
                dsEstado: dsEstado,
                idRoteiro: idRoteiro,
            });

            return $http
                .get(apiEndpoints.sgp.roteiroLoja, { params: params })
                .then(function (response) {
                    return response.data;
                });
        };
    }
})();
