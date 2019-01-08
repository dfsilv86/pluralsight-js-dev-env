(function () {
    'use strict';

    angular
        .module('SGP')
        .service('DivisaoService', DivisaoService);

    DivisaoService.$inject = ['$q', '$http', 'ApiEndpoints'];

    function DivisaoService($q, $http, ApiEndpoints) {

        function carregaDivisao(response) {
            // TODO: converter?
            return response.data;
        }

        function carregaDivisoes(response) {
            // TODO: converter?
            return response.data;
        }

        var self = this;

        self.obterPorDivisaoESistema = function (cdDivisao, cdSistema, isInitializing) {

            return $http
                .get(ApiEndpoints.sgp.divisao, { params: { cdDivisao: zeroIsValid(cdDivisao), cdSistema: cdSistema || '' } })
                .then(carregaDivisao);
        };

        self.pesquisarPorDivisaoESistema = function (cdDivisao, dsDivisao, cdSistema, paging) {

            var params = ApiEndpoints.createParams(paging, {
                cdDivisao: zeroIsValid(cdDivisao),
                dsDivisao: dsDivisao || '',
                cdSistema: cdSistema || ''
            });


            return $http
                .get(ApiEndpoints.sgp.divisao, { params: params })
                .then(carregaDivisoes);
        };
    }
})();