(function () {
    'use strict';

    angular
        .module('SGP')
        .service('OrigemCalculoService', OrigemCalculoService);

    OrigemCalculoService.$inject = ['$q', '$http', 'ApiEndpoints'];

    function OrigemCalculoService($q, $http, ApiEndpoints) {

        function carregaOrigemCalculo(response) {
            return response.data;
        }

        var self = this;       

        self.obterDisponibilidade = function (dia) {            
            return $http
                .get(ApiEndpoints.sgp.origemCalculo + 'Disponibilidade', { params: { dia: dia } })
                .then(function (response) {
                    return response.data;
                });
        };
    }
})();