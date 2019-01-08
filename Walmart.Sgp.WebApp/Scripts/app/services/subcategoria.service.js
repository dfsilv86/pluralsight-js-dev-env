(function () {
    'use strict';

    angular
        .module('SGP')
        .service('SubcategoriaService', SubcategoriaService);

    SubcategoriaService.$inject = ['$q', '$http', 'ApiEndpoints'];

    function SubcategoriaService($q, $http, ApiEndpoints) {

        function carregaSubcategoria(response) {
            // TODO: converter?
            return response.data;
        }

        function carregaSubcategorias(response) {
            // TODO: converter?
            return response.data;
        }

        var self = this;

        self.obterPorSubcategoriaESistema = function (cdSubcategoria, cdCategoria, cdDepartamento, cdSistema, isInitializing) {

            var params = {
                cdSubcategoria: zeroIsValid(cdSubcategoria),
                cdCategoria: zeroIsValid(cdCategoria),
                cdDepartamento: zeroIsValid(cdDepartamento),
                cdSistema: cdSistema || ''
            };

            return $http
                .get(ApiEndpoints.sgp.subcategoria, { params: params })
                .then(carregaSubcategoria);
        };

        self.pesquisarPorSubcategoriaCategoriaDepartamentoESistema = function (cdSubcategoria, dsSubcategoria, cdCategoria, cdDepartamento, cdSistema, paging) {

            var params = ApiEndpoints.createParams(paging, {
                cdSubcategoria: zeroIsValid(cdSubcategoria),
                dsSubcategoria: zeroIsValid(dsSubcategoria),
                cdCategoria: zeroIsValid(cdCategoria),
                cdSistema: cdSistema || '',
                cdDepartamento: zeroIsValid(cdDepartamento)
            });

            return $http
                .get(ApiEndpoints.sgp.subcategoria, { params: params })
                .then(carregaSubcategorias);
        };
    }
})();