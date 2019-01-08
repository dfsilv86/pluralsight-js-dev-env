(function () {
    'use strict';

    angular
        .module('SGP')
        .service('CompraCasadaService', CompraCasadaService);

    CompraCasadaService.$inject = ['$q', '$http', 'ApiEndpoints', 'DownloadAttachment'];

    function CompraCasadaService($q, $http, apiEndpoints, downloadAttachment) {

        var self = this;

        self.verificaPossuiPai = function (itemPai, itens, idDepartamento, cdSistema, idFornecedorParametro, idItemDetalheSaida, blPossuiCadastro) {
            var filtros = {
                itens: itens,
                ItemPaiSelecionado: itemPai,
                idDepartamento: idDepartamento,
                cdSistema: cdSistema,
                idFornecedorParametro: idFornecedorParametro,
                idItemDetalheSaida: idItemDetalheSaida,
                blPossuiCadastro: blPossuiCadastro
            };

            return $http
                .post(apiEndpoints.sgp.compraCasada + "/VerificaPossuiPai/", filtros)
                .then(function (response) {
                    return response || response.data;
                });
        };

        self.verificaPossuiCadastro = function (idDepartamento, cdSistema, idFornecedorParametro, idItemDetalheSaida, blPossuiCadastro) {
            var filtros = {
                idDepartamento: idDepartamento,
                cdSistema: cdSistema,
                idFornecedorParametro: idFornecedorParametro,
                idItemDetalheSaida: idItemDetalheSaida,
                blPossuiCadastro: blPossuiCadastro
            };

            return $http
                .get(apiEndpoints.sgp.compraCasada + 'VerificaPossuiCadastro', { params: filtros })
                .then(function (response) {
                    return response || response.data;
                });
        };

        self.excluir = function (idDepartamento, cdSistema, idFornecedorParametro, idItemDetalheSaida, blPossuiCadastro) {
            var filtros = {
                idDepartamento: idDepartamento,
                cdSistema: cdSistema,
                idFornecedorParametro: idFornecedorParametro,
                idItemDetalheSaida: idItemDetalheSaida,
                blPossuiCadastro: blPossuiCadastro
            };

            return $http
                .delete(apiEndpoints.sgp.compraCasada + 'Excluir', { params: filtros })
                .then(function (response) {
                    return response || response.data;
                });
        };

        self.salvar = function (itens, idDepartamento, cdSistema, idFornecedorParametro, idItemDetalheSaida, blPossuiCadastro) {
            var filtros = {
                idDepartamento: idDepartamento,
                cdSistema: cdSistema,
                idFornecedorParametro: idFornecedorParametro,
                idItemDetalheSaida: idItemDetalheSaida,
                blPossuiCadastro: blPossuiCadastro,
                itens: itens
            };

            return $http
                .post(apiEndpoints.sgp.compraCasada + 'Salvar', filtros )
                .then(function (response) {
                    return response || response.data;
                });
        };

        self.validarItemFilhoMarcado = function (itens, idDepartamento, cdSistema, idFornecedorParametro, idItemDetalheSaida, blPossuiCadastro) {
            var filtros = {
                idDepartamento: idDepartamento,
                cdSistema: cdSistema,
                idFornecedorParametro: idFornecedorParametro,
                idItemDetalheSaida: idItemDetalheSaida,
                blPossuiCadastro: blPossuiCadastro,
                itens: itens
            };

            return $http
                .post(apiEndpoints.sgp.compraCasada + "/ValidarItemFilhoMarcado/", filtros)
                .then(function (response) {
                    return response || response.data;
                });
        };

        self.exportar = function (idDepartamento, cdSistema, idFornecedorParametro, idItemDetalheSaida, blPossuiCadastro) {
            var params = {
                idDepartamento: idDepartamento,
                cdSistema: cdSistema,
                idFornecedorParametro: idFornecedorParametro,
                idItemDetalheSaida: idItemDetalheSaida,
                blPossuiCadastro: blPossuiCadastro
            };

            return $http
                .get(apiEndpoints.sgp.compraCasada + "/Exportar/", { params: params })
                .then(downloadAttachment);
        };

        self.pesquisarItensEntrada = function (idDepartamento, cdSistema, idFornecedorParametro, idItemDetalheSaida, blPossuiCadastro, paging) {

            var filtros = {
                idDepartamento: idDepartamento,
                cdSistema: cdSistema,
                idFornecedorParametro: idFornecedorParametro,
                idItemDetalheSaida: idItemDetalheSaida,
                blPossuiCadastro: blPossuiCadastro
            };

            var params = apiEndpoints.createParams(paging, filtros);

            return $http
                .get(apiEndpoints.sgp.compraCasada + "/PesquisarItensEntrada/", { params: params })
                .then(function (response) {
                    return response.data;
                });
        };

        self.pesquisarItensCompraCasada = function (idDepartamento, cdSistema, idFornecedorParametro, idItemDetalheSaida, blPossuiCadastro, paging) {

            var filtros = {
                idDepartamento: idDepartamento,
                cdSistema: cdSistema,
                idFornecedorParametro: idFornecedorParametro,
                idItemDetalheSaida: idItemDetalheSaida,
                blPossuiCadastro: blPossuiCadastro
            };

            var params = apiEndpoints.createParams(paging, filtros);

            return $http
                .get(apiEndpoints.sgp.compraCasada + "/Pesquisar/", { params: params })
                .then(function (response) {
                    return response.data;
                });
        };
    }
})();