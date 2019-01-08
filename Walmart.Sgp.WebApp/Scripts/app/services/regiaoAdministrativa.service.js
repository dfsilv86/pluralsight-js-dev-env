
(function () {
    'use strict';

    angular
        .module('SGP')
        .service('RegiaoAdministrativaService', RegiaoAdministrativaService);

    RegiaoAdministrativaService.$inject = ['$http', 'ApiEndpoints'];

    function RegiaoAdministrativaService($http, apiEndpoints) {

        function carregarRegiaoAdministrativa(response)
        {
            return response.data;
        }

        var self = this;

        self.obterTodos = function () {
            return $http
                .get(apiEndpoints.sgp.regiaoAdministrativa)
                .then(carregarRegiaoAdministrativa);
        }
    }
})();
