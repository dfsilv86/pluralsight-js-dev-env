(function () {
    'use strict';

    angular
        .module('SGP')
        .service('BandeiraService', BandeiraService);

    BandeiraService.$inject = ['$q', '$http', 'ApiEndpoints'];

    function BandeiraService($q, $http, apiEndpoints) {

        function carregaBandeira(response) {
            return response.data;
        }

        var self = this;

        self.obterPorUsuarioESistema = function (idUsuario, cdSistema, idFormato) {
            return $http
                .get(
                    apiEndpoints.sgp.bandeira + 'PorSistema',
                    {
                        params: {
                            idUsuario: idUsuario,
                            cdSistema: cdSistema || '',
                            idFormato: idFormato || '',
                        }
                    }
                )
                .then(carregaBandeira);
        };

        self.obterPorUsuarioERegiaoAdministrativa = function (idUsuario, cdSistema, idRegiaoAdministrativa) {
            return $http
                .get(
                    apiEndpoints.sgp.bandeira + 'PorRegiaoAdministrativa',
                    {
                        params: {
                            idUsuario: idUsuario,
                            cdSistema: cdSistema,
                            idRegiaoAdministrativa: idRegiaoAdministrativa,
                        }
                    }
                )
                .then(carregaBandeira);
        };

        self.obterEstruturadoPorId = function (id) {
            return $http
                .get('{0}/{1}/estruturado'.format(apiEndpoints.sgp.bandeira, id))
                .then(carregaBandeira);
        };

        self.pesquisarPorFiltros = function (filtro, paging) {
            var params = apiEndpoints.createParams(paging, filtro);

            return $http
                .get(apiEndpoints.sgp.bandeira + 'PorFiltro/', { params: params })
                .then(carregaBandeira);
        };

        self.salvar = function (entidade) {            
            return $http
                .post(apiEndpoints.sgp.bandeira, entidade)
                .then(carregaBandeira);
        };

        self.remover = function (idBandeira) {
            return $http
                .delete(apiEndpoints.sgp.bandeira, { params: { 'idBandeira': idBandeira } });
        };
    }
})();