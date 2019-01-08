(function () {
    'use strict';

    angular
        .module('SGP')
        .service('SugestaoReturnSheetService', SugestaoReturnSheetService);

    SugestaoReturnSheetService.$inject = ['$q', '$http', 'ApiEndpoints', 'DownloadAttachment'];

    function SugestaoReturnSheetService($q, $http, apiEndpoints, downloadAttachment) {

        function carregaSugestaoReturnSheet(response) {
            // TODO: converter?
            return response.data;
        }

        var self = this;

        self.autorizarExportarPlanilhas = function (dtInicioReturn, dtFinalReturn, cdV9D, evento, cdItemDetalhe, cdDepartamento, cdLoja, idRegiaoCompra, blExportado, blAutorizado) {
            var filtro = {
                dtInicioReturn: dtInicioReturn,
                dtFinalReturn: dtFinalReturn,
                cdV9D: cdV9D,
                evento: evento,
                cdItemDetalhe: cdItemDetalhe,
                cdDepartamento: cdDepartamento,
                cdLoja: cdLoja,
                idRegiaoCompra: idRegiaoCompra,
                blExportado: blExportado,
                blAutorizado: blAutorizado
            };

            return $http.put(apiEndpoints.sgp.sugestaoReturnSheet + '/AutorizarExportarPlanilhas', filtro);
        };

        self.salvarLoja = function (sugestoes) {
            return $http.put(apiEndpoints.sgp.sugestaoReturnSheet + "/SalvarLoja", sugestoes).then(carregaSugestaoReturnSheet);
        };

        self.salvarRA = function (sugestoes) {
            return $http.put(apiEndpoints.sgp.sugestaoReturnSheet + "/SalvarRA", sugestoes).then(carregaSugestaoReturnSheet);
        };

        self.consultaReturnSheetLoja = function (idDepartamento, idLoja, dataSolicitacao, evento, vendor9D, idItemDetalhe, paging) {
            var params = apiEndpoints.createParams(paging, {
                idDepartamento: idDepartamento || 0,
                idLoja: idLoja || 0,
                dataSolicitacao: new Date(dataSolicitacao),
                evento: evento,
                vendor9D: vendor9D || 0,
                idItemDetalhe: idItemDetalhe || 0
            });

            return $http
                .get(apiEndpoints.sgp.sugestaoReturnSheet, { params: params })
                .then(carregaSugestaoReturnSheet);
        };

        self.consultaReturnSheetRA = function (dtInicioReturn, dtFinalReturn, cdV9D, evento, cdItemDetalhe, cdDepartamento, cdLoja, idRegiaoCompra, blExportado, blAutorizado, paging) {

            var filtro = {
                dtInicioReturn: dtInicioReturn,
                dtFinalReturn: dtFinalReturn,
                cdV9D: cdV9D,
                evento: evento,
                cdItemDetalhe: cdItemDetalhe,
                cdDepartamento: cdDepartamento,
                cdLoja: cdLoja,
                idRegiaoCompra: idRegiaoCompra,
                blExportado: blExportado,
                blAutorizado: blAutorizado
            };

            var params = apiEndpoints.createParams(paging, filtro);

            return $http
                .get(apiEndpoints.sgp.sugestaoReturnSheet, { params: params })
                .then(carregaSugestaoReturnSheet);
        };

        self.exportarRA = function (dtInicioReturn, dtFinalReturn, cdV9D, evento, cdItemDetalhe, cdDepartamento, cdLoja, idRegiaoCompra, blExportado, blAutorizado, paging) {

            var filtro = {
                dtInicioReturn: dtInicioReturn,
                dtFinalReturn: dtFinalReturn,
                cdV9D: cdV9D,
                evento: evento,
                cdItemDetalhe: cdItemDetalhe,
                cdDepartamento: cdDepartamento,
                cdLoja: cdLoja,
                idRegiaoCompra: idRegiaoCompra,
                blExportado: blExportado,
                blAutorizado: blAutorizado
            };

            var params = apiEndpoints.createParams(paging, filtro);

            return $http
                .get(apiEndpoints.sgp.sugestaoReturnSheet + "ExportarRA", { params: params })
                .then(downloadAttachment);
        };

        self.possuiReturnsVigentesQuantidadesVazias = function () {
            return $http
                .get(apiEndpoints.sgp.sugestaoReturnSheet + "possuiReturnsVigentesQuantidadesVazias")
                .then(function (response) {
                    return response.data;
                })
        };

        self.registrarLogAvisoReturnSheetsVigentes = function () {
            return $http
                .post(apiEndpoints.sgp.sugestaoReturnSheet + "registrarLogAvisoReturnSheetsVigentes");
        }
    }
})();
