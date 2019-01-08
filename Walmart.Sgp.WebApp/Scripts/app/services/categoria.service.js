(function () {
    'use strict';

    angular
        .module('SGP')
        .service('CategoriaService', CategoriaService);

    CategoriaService.$inject = ['$q', '$http', 'ApiEndpoints'];

    function CategoriaService($q, $http, ApiEndpoints) {

        function carregaCategoria(response) {
            // TODO: converter?
            return response.data;
        }

        function carregaCategorias(response) {
            // TODO: converter?
            return response.data;
        }

        var self = this;

        self.obterPorCategoriaESistema = function (cdCategoria, cdDepartamento, cdSistema, isInitializing) {

            var params = {
                cdCategoria: zeroIsValid(cdCategoria),
                cdDepartamento: zeroIsValid(cdDepartamento),
                cdSistema: cdSistema || ''
            };

            return $http
                .get(ApiEndpoints.sgp.categoria, { params: params })
                .then(carregaCategoria);
        };

        self.pesquisarPorCategoriaDepartamentoESistema = function (cdCategoria, dsCategoria, cdSistema, cdDepartamento, paging) {

            var params = ApiEndpoints.createParams(paging, {
                cdCategoria: zeroIsValid(cdCategoria),
                dsCategoria: dsCategoria || '',
                cdSistema: cdSistema || '',
                cdDepartamento: zeroIsValid(cdDepartamento)
            });

            return $http
                .get(ApiEndpoints.sgp.categoria, { params: params })
                .then(carregaCategorias);
        };

        self.obterPorSistema = function (cdSistema) {
            var params = { cdSistema: cdSistema };
            
            return $http
                .get("{0}/PorSistema".format(ApiEndpoints.sgp.categoria), { params: params })
                .then(carregaCategoria);
        };
    }
})();