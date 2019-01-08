(function () {
    'use strict';

    angular
        .module('SGP')
        .service('ReturnSheetItemLojaService', ReturnSheetItemLojaService);

    ReturnSheetItemLojaService.$inject = ['$q', '$http', 'ApiEndpoints'];

    function ReturnSheetItemLojaService($q, $http, apiEndpoints) {

        function carregarLojasValidasItem(response) {
            // TODO: converter?
            return response.data;
        }

        var self = this;

        self.obterEstadosLojasIncludasNaReturnSheet = function (idReturnSheet, lojasAlteradas) {
            return $http
                .get(apiEndpoints.sgp.returnSheetItemLoja, { params: { idReturnSheet: idReturnSheet, lojasAlteradas: lojasAlteradas } })
                .then(carregarLojasValidasItem);
        };

        self.obterLojasValidasItem = function (cdItem, cdSistema, idReturnSheet, dsEstado, paging) {
            var params = apiEndpoints.createParams(paging, {
                cdItem: cdItem,
                cdSistema: cdSistema,
                idReturnSheet: idReturnSheet,
                dsEstado: dsEstado
            });

            return $http
                .get(apiEndpoints.sgp.returnSheetItemLoja, { params: params })
                .then(carregarLojasValidasItem);
        };

        self.obterEstadosLojasValidasItem = function (cdItem, cdSistema) {
            var params = {
                cdItem: cdItem,
                cdSistema: cdSistema
            };

            return $http
                .get(apiEndpoints.sgp.returnSheetItemLoja, { params: params })
                .then(carregarLojasValidasItem);
        };
    }
})();
