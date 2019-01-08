(function () {
    'use strict';

    angular
        .module('SGP')
        .service('CDService', CDService);

    CDService.$inject = ['$q', '$http', 'ApiEndpoints'];

    function CDService($q, $http, apiEndpoints) {

        var self = this;

        self.obterTodosConvertidosAtivos = function () {
            return $http
                .get(apiEndpoints.sgp.cd + "ConvertidosAtivos")
                .then(function (response) {
                    return response.data;
                });
        };
    }
})();
