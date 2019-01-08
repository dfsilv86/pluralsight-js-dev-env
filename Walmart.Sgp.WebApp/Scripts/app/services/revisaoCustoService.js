(function () {
    'use strict';

    angular
        .module('SGP')
        .service('RevisaoCustoService', RevisaoCustoService);

    RevisaoCustoService.$inject = ['$q', '$http', 'ApiEndpoints'];

    function RevisaoCustoService($q, $http, ApiEndpoints) {

        function carregaRevisaoCusto(response) {
            // TODO: converter?
            return response.data;
        }

        var self = this;

        self.obterEstruturadoPorId = function (id) {
            return $http
                .get(ApiEndpoints.sgp.revisaoCusto, { params: { id: id } })
                .then(carregaRevisaoCusto);
        };

        self.salvar = function (revisaoCusto) {
            return $http.post(ApiEndpoints.sgp.revisaoCusto, revisaoCusto).then(carregaRevisaoCusto);
        };

        self.incluirSolicitacao = function (revisaoCusto) {
            return $http.post(ApiEndpoints.sgp.revisaoCusto, revisaoCusto).then(carregaRevisaoCusto);
        };

        self.pesquisarPorFiltros = function (filtro, paging) {

            var params = ApiEndpoints.createParams(paging, {
                idBandeira: filtro.idBandeira,
                idLoja: filtro.loja != null ? filtro.loja.idLoja : null,
                idDepartamento: filtro.departamento != null ? filtro.departamento.idDepartamento : null,
                cdItem: filtro.cdItem,
                dsItem: filtro.dsItem,
                idStatus: filtro.idStatus
            });

            return $http
                .get(ApiEndpoints.sgp.revisaoCustoPesquisarPorFiltros, { params: JSON.flatten(params) })
                .then(carregaRevisaoCusto);
        };
    }
})();