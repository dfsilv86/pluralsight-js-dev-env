(function () {
    'use strict';

    angular
        .module('SGP')
        .service('TipoMovimentacaoService', TipoMovimentacaoService);

    TipoMovimentacaoService.$inject = ['$q', '$http', 'ApiEndpoints'];

    function TipoMovimentacaoService($q, $http, apiEndpoints) {

        function carregaTipoMovimentacao(response) {
            return response.data;
        }

        var self = this;

        self.obterPorCategoria = function (categoria) {           
            return $http
                .get("{0}categoria/{1}".format(apiEndpoints.sgp.tipoMovimentacao, apiEndpoints.encode(categoria)))
                .then(carregaTipoMovimentacao);
        };
    }
})();