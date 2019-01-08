(function () {
    'use strict';

    angular
        .module('SGP')
        .service('MotivoMovimentacaoService', MotivoMovimentacaoService);

    MotivoMovimentacaoService.$inject = ['$q', '$http', 'ApiEndpoints'];

    function MotivoMovimentacaoService($q, $http, apiEndpoints) {

        function carregaMotivoMovimentacao(response) {
            // TODO: converter?
            return response.data;
        }

        var self = this;

        self.obterVisiveis = function () {

            return $http
                .get(apiEndpoints.sgp.motivoMovimentacao)
                .then(carregaMotivoMovimentacao);
        };
    }
})();