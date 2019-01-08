(function () {
    'use strict';

    angular
        .module('SGP')
        .service('ReturnSheetService', ReturnSheetService);

    ReturnSheetService.$inject = ['$q', '$http', 'ApiEndpoints'];

    function ReturnSheetService($q, $http, apiEndpoints) {

        function carregaReturnSheet(response) {
            // TODO: converter?
            return response.data;
        }

        var self = this;

        self.jaComecou = function (id) {
            return $http
                .get(apiEndpoints.sgp.returnSheet + '/JaComecou/', { params: { idRS: id } })
                .then(carregaReturnSheet);
        };

        self.podeSerEditada = function (id) {
            return $http
                .get(apiEndpoints.sgp.returnSheet, { params: { idRS: id } })
                .then(carregaReturnSheet);
        };

        self.consultaReturnSheetLoja = function (idDepartamento, cdLoja, dataSolicitacao, evento, vendor9D, idItemDetalhe, paging) {
            var params = apiEndpoints.createParams(paging, {
                idDepartamento: idDepartamento || 0,
                cdLoja: cdLoja || 0,
                dataSolicitacao: dataSolicitacao,
                evento: evento,
                vendor9D: vendor9D || 0,
                idItemDetalhe: idItemDetalhe || 0
            });

            return $http
                .get(apiEndpoints.sgp.returnSheet, { params: params })
                .then(carregaReturnSheet);
        };

        self.pesquisar = function (inicioReturn, finalReturn, evento, idDepartamento, filtroAtivos, idRegiaoCompra, paging) {

            var params = apiEndpoints.createParams(paging, {
                inicioReturn: inicioReturn,
                finalReturn: finalReturn,
                evento: evento || '',
                idDepartamento: idDepartamento,
                filtroAtivos: filtroAtivos || 1,
                idRegiaoCompra: idRegiaoCompra
            });

            return $http
                .get(apiEndpoints.sgp.returnSheet, { params: params })
                .then(carregaReturnSheet);
        };

        self.remover = function (id) {
            return $http
                .delete(apiEndpoints.sgp.returnSheet, { params: { id: id } })
                .then(carregaReturnSheet);
        };

        self.obter = function (id) {

            return $http
                .get(apiEndpoints.sgp.returnSheet, { params: { id: id } })
                .then(carregaReturnSheet);
        };

        self.inserirOuAtualizar = function (inicioReturn, finalReturn, inicioEvento, finalEvento, horaCorte, descricao, idDepartamento, idRegiaoCompra, idUsuario, idReturnSheet) {

            var returnSheet = {
                "IdReturnSheet": idReturnSheet,
                "HoraCorte": horaCorte,
                "DhInicioReturn": inicioReturn,
                "DhFinalReturn": finalReturn,
                "DhInicioEvento": inicioEvento,
                "DhFinalEvento": finalEvento,
                "Descricao": descricao,
                "IdRegiaoCompra": idRegiaoCompra,
                "idDepartamento": idDepartamento
            };

            if (idReturnSheet === 0) {
                returnSheet.BlAtivo = 1;
                returnSheet.IdUsuarioCriacao = idUsuario;
            }

            return $http.post(apiEndpoints.sgp.returnSheet, returnSheet).then(carregaReturnSheet);
        };
    }
})();
