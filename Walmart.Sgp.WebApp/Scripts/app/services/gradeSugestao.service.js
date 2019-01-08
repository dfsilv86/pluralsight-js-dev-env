(function () {
    'use strict';

    angular
        .module('SGP')
        .service('GradeSugestaoService', GradeSugestaoService);

    GradeSugestaoService.$inject = ['$q', '$http', 'ApiEndpoints'];

    function GradeSugestaoService($q, $http, ApiEndpoints) {

        function carregarDados(response) {
            // TODO: converter?
            return response.data;
        }

        function limparObjeto(gradeSugestao) {           
            return {
                idGradeSugestao: gradeSugestao.idGradeSugestao,
                idBandeira: gradeSugestao.idBandeira,
                idDepartamento: gradeSugestao.idDepartamento,
                idLoja: gradeSugestao.idLoja,
                cdSistema: gradeSugestao.cdSistema,
                vlHoraInicial: gradeSugestao.vlHoraInicial,
                vlHoraFinal: gradeSugestao.vlHoraFinal
            };
        }

        var self = this;

        self.pesquisarPorFiltro = function (cdSistema, idBandeira, cdDepartamento, cdLoja, paging) {
            var params = ApiEndpoints.createParams(paging, {
                cdSistema: cdSistema || '',
                idBandeira: idBandeira || '',                
                cdDepartamento: cdDepartamento || '',
                cdLoja: cdLoja || ''
            });

            return $http
                .get(ApiEndpoints.sgp.gradeSugestao, { params: params })
                .then(carregarDados);
        };

        self.obterEstruturado = function (id) {
            return $http
                .get(ApiEndpoints.sgp.gradeSugestao + ApiEndpoints.encode(id))
                .then(carregarDados);
        };

        self.atualizar = function (gradeSugestao) {
            gradeSugestao = limparObjeto(gradeSugestao);
            return $http.put(ApiEndpoints.sgp.gradeSugestao, gradeSugestao)
                .then(carregarDados);
        };

        self.salvarNovas = function (sugestoes) {
            sugestoes = sugestoes.map(limparObjeto);
            return $http.post(ApiEndpoints.sgp.gradeSugestao, sugestoes)
                .then(function () { return true; });
        };

        self.remover = function (id) {
            return $http.delete(ApiEndpoints.sgp.gradeSugestao + ApiEndpoints.encode(id));
        };

        self.contarExistentes = function (cdSistema, idBandeira, idLoja, idDepartamento) {
            var params = {
                cdSistema: cdSistema,
                idBandeira: idBandeira,
                idLoja: idLoja,
                idDepartamento: idDepartamento
            };

            return $http.get(ApiEndpoints.sgp.gradeSugestao + 'count', { params: params })
                .then(carregarDados);
        };
    }
})();