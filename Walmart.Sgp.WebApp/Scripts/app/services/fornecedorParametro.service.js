(function () {
    'use strict';

    angular
        .module('SGP')
        .service('FornecedorParametroService', FornecedorParametroService);

    FornecedorParametroService.$inject = ['$q', '$http', 'ApiEndpoints'];

    function FornecedorParametroService($q, $http, ApiEndpoints) {

        function carregaFornecedorParametro(response) {
            // TODO: converter?
            return response.data;
        }

        function carregaFornecedorParametros(response) {
            // TODO: converter?
            return response.data;
        }

        function carregaReviewDates(response) {
            // TODO: converter?
            return response.data;
        }

        var self = this;

        self.pesquisarPorFiltro = function (cdSistema, stFornecedor, cdV9D, nmFornecedor, paging) {
            var params = ApiEndpoints.createParams(paging, {
                cdSistema: cdSistema || '',
                stFornecedor: stFornecedor || '',
                cdV9D: cdV9D || '',
                nmFornecedor: nmFornecedor || ''
            });

            return $http
                .get(ApiEndpoints.sgp.fornecedorParametro, { params: params })
                .then(carregaFornecedorParametros);
        };

        self.obterEstruturado = function (idFornecedorParametro) {
            return $http
                .get(ApiEndpoints.sgp.fornecedorParametro + ApiEndpoints.encode(idFornecedorParametro))
                .then(carregaFornecedorParametro);
        };

        self.obterReviewDates = function (idFornecedorParametro, detalhamento, paging) {
            var params = ApiEndpoints.createParams(paging, {});

            return $http
                .get(ApiEndpoints.sgp.fornecedorParametro + ApiEndpoints.encode(idFornecedorParametro) + '/ReviewDate/' + ApiEndpoints.encode(detalhamento), { params: params })
                .then(carregaReviewDates);
        };

        self.pesquisarPorSistemaCodigo9DigitosENomeFornecedor = function (cdSistema, cdTipo, cdV9D, nmFornecedor, paging) {

            var params = ApiEndpoints.createParams(paging, {
                cdSistema: cdSistema,
                cdTipo: cdTipo || '',
                cdV9D: cdV9D || '',
                nmFornecedor: nmFornecedor || ''
            });

            return $http
                .get(ApiEndpoints.sgp.fornecedorParametro + 'Vendor/', { params: params })
                .then(carregaFornecedorParametros);
        };

        self.obterPorSistemaECodigo9Digitos = function (cdSistema, cdTipo, cdV9D, isInitializing) {

            return $http
                .get(ApiEndpoints.sgp.fornecedorParametro + 'Vendor/' + ApiEndpoints.encode(angular.isUndefined(cdV9D) ? '' : cdV9D), { params: { cdSistema: cdSistema, cdTipo: cdTipo || '' } })
                .then(carregaFornecedorParametro);
        };
    }
})();