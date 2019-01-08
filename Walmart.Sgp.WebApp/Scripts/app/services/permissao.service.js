(function () {
    'use strict';

    angular
        .module('SGP')
        .service('PermissaoService', PermissaoService);

    PermissaoService.$inject = ['$q', '$http', 'ApiEndpoints'];

    function PermissaoService($q, $http, ApiEndpoints) {

        var self = this;

        self.pesquisarComFilhos = function (idUsuario, idBandeira, idLoja, paging) {
            var params = ApiEndpoints.createParams(paging, {
                idUsuario: idUsuario,
                idBandeira: idBandeira,
                idLoja: idLoja
            });

            return $http
                .get(ApiEndpoints.sgp.permissao, { params: params })
                .then(function (response) {
                    return response.data;
                });
        };

        self.obterPorId = function (idPermissao) {
            var params = {
                idPermissao: idPermissao
            };

            return $http
                .get(ApiEndpoints.sgp.permissao, { params: params })
                .then(function (response) {
                    return response.data;
                });
        };

        self.validarInclusaoBandeira = function (idBandeira) {
            return $http
                .get(ApiEndpoints.sgp.permissao + ApiEndpoints.encode(idBandeira) + "/ValidarInclusaoBandeira")
                .then(function () {
                    return {};
                });
        };

        self.validarInclusaoLoja = function (idLoja) {
            return $http
                .get(ApiEndpoints.sgp.permissao + ApiEndpoints.encode(idLoja) + "/ValidarInclusaoLoja")
                .then(function () {
                    return {};
                });
        };

        self.possuiPermissaoManutencao = function () {
            return $http
                .get(ApiEndpoints.sgp.permissao + "/PermissaoManutencao")
                .then(function (response) {
                    return response.data;
                });
        };


        self.salvar = function (entidade) {
            
            var permissaoParaSalvar = {
                id: entidade.id,
                idUsuario: entidade.usuario.id,
                bandeiras: entidade.bandeiras.map(function (b) { return { id: b.idPermissaoBandeira, idPermissao: b.idPermissao, idBandeira: b.bandeira.idBandeira }; }),
                lojas: entidade.lojas.map(function (l) { return { id: l.idPermissaoLoja, idPermissao: l.idPermissao, idLoja: l.loja.idLoja }; }),
                blRecebeNotificaoOperacoes: entidade.blRecebeNotificaoOperacoes,
                blRecebeNotificaoFinanceiro: entidade.blRecebeNotificaoFinanceiro
            };            
            
            return $http
                .post(ApiEndpoints.sgp.permissao, permissaoParaSalvar)
                .then(function (response) {
                    return response.data;
                });
        };


        self.remover = function (idPermissao) {
            return $http
                .delete(ApiEndpoints.sgp.permissao, { params: { 'idPermissao': idPermissao } });
        };
    }
})();