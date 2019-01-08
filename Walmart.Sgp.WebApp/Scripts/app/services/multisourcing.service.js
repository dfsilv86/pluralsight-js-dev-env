(function () {
    'use strict';

    angular
        .module('SGP')
        .service('MultisourcingService', MultisourcingService);

    MultisourcingService.$inject = ['$q', '$http', 'ApiEndpoints', 'DownloadAttachment'];

    function MultisourcingService($q, $http, apiEndpoints, downloadAttachment) {

        function carregaMultisourcing(response) {
            // TODO: converter?
            return response.data;
        }

        var self = this;

        self.importar = function (cdSistema, idUsuario, arquivos) {
            var params = {
                cdSistema: cdSistema || '',
                idUsuario: idUsuario || '',
                arquivos: arquivos || []
            };

            return $http
                .post(apiEndpoints.sgp.multisourcing, params)
                .then(carregaMultisourcing);
        };

        self.obter = function (id) {

            return $http
                .get(apiEndpoints.sgp.multisourcing, { params: { id: id } })
                .then(carregaMultisourcing);
        };

        self.obterRelatorioImportacao = function (reportFilename) {
            $http
                .get(apiEndpoints.sgp.multisourcing + "?reportFileName=" + reportFilename)
                .then(downloadAttachment);
        };

        self.inserirMultisourcing = function (itens, idUsuario) {
            return $http
                .put(apiEndpoints.sgp.multisourcing + "/inserir/" + idUsuario, itens)
                .then(carregaMultisourcing);
        };

        self.deletarMultisourcing = function (cdItemSaida, cdCD) {
                        
            return $http
                .delete(apiEndpoints.sgp.multisourcing + "?cdItemSaida=" + cdItemSaida + "&cdCD=" + cdCD)
                .then(carregaMultisourcing);
        };

        self.exportar = function (idItemDetalhe, idDepartamento, cdSistema, idCD, filtroMS, filtroCadastro) {
            var params = {
                idItemDetalhe: idItemDetalhe || '',
                idDepartamento: idDepartamento || '',
                cdSistema: cdSistema || 1,
                idCD: idCD || '',
                filtroMS: filtroMS || 1,
                filtroCadastro: filtroCadastro || 2
            };

            return $http
                .get(apiEndpoints.sgp.multisourcing, { params: params })
                .then(downloadAttachment);
        };
    }
})();
