(function () {
    'use strict';

    angular
        .module('SGP')
        .service('FornecedorService', FornecedorService);

    FornecedorService.$inject = ['$q', '$http', 'ApiEndpoints'];

    function FornecedorService($q, $http, ApiEndpoints) {

        function carregaFornecedor(response) {
            // TODO: converter?
            return response.data;
        }

        var self = this;

        self.obterPorSistemaCodigo = function (cdSistema, cdFornecedor, isInitializing) {

            var params = {
                cdFornecedor: zeroIsValid(cdFornecedor),
                cdSistema: cdSistema || ''
            };

            return $http
                .get(ApiEndpoints.sgp.fornecedor, { params: params })
                .then(carregaFornecedor);
        };

        self.obterListaPorSistemaCodigoNome = function (cdSistema, cdFornecedor, nmFornecedor, paging) {

            var params = ApiEndpoints.createParams(paging, {
                cdFornecedor: zeroIsValid(cdFornecedor),
                nmFornecedor: nmFornecedor || '',
                cdSistema: cdSistema
            });

            return $http
                .get(ApiEndpoints.sgp.fornecedor, { params: params })
                .then(carregaFornecedor);
        };
    }
})();