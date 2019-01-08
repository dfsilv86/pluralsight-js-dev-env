(function () {
    'use strict';

    angular
        .module('SGP')
        .service('ReturnSheetItemPrincipalService', ReturnSheetItemPrincipalService);

    ReturnSheetItemPrincipalService.$inject = ['$q', '$http', 'ApiEndpoints'];

    function ReturnSheetItemPrincipalService($q, $http, apiEndpoints) {

        function carregaPesquisar(response) {
            return response.data;
        }

        function carregaObter(response) {
            return response.data;
        }

        function carregaInserirOuAtualizar(response) {
            return response.data;
        }

        function carregaRemover(response) {
            return response.data;
        }

        var self = this;

        self.pesquisar = function (idReturnSheet, paging) {

            var params = apiEndpoints.createParams(paging, {
                idReturnSheet: idReturnSheet
            });

            return $http
                .get(apiEndpoints.sgp.returnSheetItemPrincipal, { params: params })
                .then(carregaPesquisar);
        };

        self.obter = function (id) {

            return $http
                .get(apiEndpoints.sgp.returnSheetItemPrincipal, { params: { id: id } })
                .then(carregaObter);
        };

        self.inserirOuAtualizar = function (lojasAlteradas, idReturnSheet, precoVenda) {
            var params = {
                LojasAlteradas: lojasAlteradas,
                IdReturnSheet: idReturnSheet,
                PrecoVenda: precoVenda
            };

            return $http.post(apiEndpoints.sgp.returnSheetItemPrincipal, params)
                .then(carregaInserirOuAtualizar);
        };

        self.remover = function (idReturnSheet, cdItem) {
            return $http
                .delete(apiEndpoints.sgp.returnSheetItemPrincipal, { params: { idReturnSheet: idReturnSheet, cdItem: cdItem } })
                .then(carregaRemover);
        };
    }
})();
