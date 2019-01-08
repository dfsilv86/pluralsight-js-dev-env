(function () {
    'use strict';

    angular
        .module('SGP')
        .service('ProcessoService', ProcessoService);

    ProcessoService.$inject = ['$q', '$http', 'ApiEndpoints', '$log'];

    function ProcessoService($q, $http, apiEndpoints, $log) {

        function obterData(response) {
            return response.data;
        }

        var self = this;      
      
        self.pesquisarCargas = function (filtro, paging) {

            var params = apiEndpoints.createParams(paging, {
                cdSistema: filtro.cdSistema || 0,
                idBandeira: filtro.idBandeira || '',
                cdLoja: filtro.cdLoja || '',
                data: filtro.data
            });

            return $http
                .get("{0}/carga".format(apiEndpoints.sgp.processo), { params: params })
                .then(obterData);
        };

        self.obterCargaPorLoja = function (cdSistema, idBandeira, cdLoja, data) {

            return $http
                .get("{0}/carga/loja".format(apiEndpoints.sgp.processo), { params: { 'cdSistema': cdSistema, 'idBandeira': idBandeira, 'cdLoja': cdLoja, 'data': data } })
                .then(obterData);
        };

        self.pesquisarProcessosExecucao = function (filtro, paging) {
            var idLoja = filtro.loja != null ? filtro.loja.idLoja : null;
            var idItemDetalhe = filtro.item != null ? filtro.item.idItemDetalhe : null;
            var data = filtro.dtExecucao;

            if (data != null) {
                data.endValue = data.endValue.endOfDay();
            }
                           
            var params = apiEndpoints.createParams(paging, {
                data: data || '',
                idProcesso: filtro.idProcesso || '',
                cdSistema: filtro.cdSistema,
                idBandeira: filtro.idBandeira,
                idLoja: idLoja || '',
                idItemDetalhe: idItemDetalhe || ''
            });

            return $http
                .get("{0}execucao".format(apiEndpoints.sgp.processo), { params: JSON.flatten(params) })
                .then(obterData);
        };

        self.obterTodos = function () {

            return $http
                .get(apiEndpoints.sgp.processo)
                .then(obterData);
        };
    }
})();