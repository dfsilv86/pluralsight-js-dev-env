(function () {
    'use strict';

    angular
        .module('SGP')
        .service('FineLineService', FineLineService);

    FineLineService.$inject = ['$q', '$http', 'ApiEndpoints'];

    function FineLineService($q, $http, ApiEndpoints) {

        function carregaFineLine(response) {
            // TODO: converter?
            return response.data;
        }

        function carregaFineLines(response) {
            // TODO: converter?
            return response.data;
        }

        var self = this;

        self.obterPorFineLineESistema = function (cdFineLine, cdSubcategoria, cdCategoria, cdDepartamento, cdSistema, isInitializing) {

            var params = {
                cdFineLine: zeroIsValid(cdFineLine),
                cdSubcategoria: zeroIsValid(cdSubcategoria),
                cdCategoria: zeroIsValid(cdCategoria),
                cdDepartamento: zeroIsValid(cdDepartamento),
                cdSistema: cdSistema || ''
            };

            return $http
                .get(ApiEndpoints.sgp.fineLine, { params: params })
                .then(carregaFineLine);
        };

        self.pesquisarPorFineLineSubcategoriaCategoriaDepartamentoESistema = function (cdFineLine, dsFineLine, cdSubcategoria, cdCategoria, cdDepartamento, cdSistema, paging) {

            var params = ApiEndpoints.createParams(paging, {
                cdFineLine: zeroIsValid(cdFineLine),
                dsFineLine: dsFineLine || '',
                cdSubcategoria: zeroIsValid(cdSubcategoria),
                cdCategoria: zeroIsValid(cdCategoria),
                cdSistema: cdSistema || '',
                cdDepartamento: zeroIsValid(cdDepartamento)
            });

            return $http
                .get(ApiEndpoints.sgp.fineLine, { params: params })
                .then(carregaFineLines);
        };
    }
})();