(function () {
    'use strict';

    angular
        .module('SGP')
        .service('DepartamentoService', DepartamentoService);

    DepartamentoService.$inject = ['$q', '$http', 'ApiEndpoints'];

    function DepartamentoService($q, $http, ApiEndpoints) {

        function carregaDepartamento(response) {
            // TODO: converter?
            return response.data;
        }

        function carregaDepartamentos(response) {
            // TODO: converter?
            return response.data;
        }

        // Retorna o valor em booleano. 
        function toBoolean(val) {
            if (val == undefined) {
                return false;
            }

            var valType = typeof val;
            if (valType === "boolean") {
                return val;
            }
            
            if (valType === "string") {
                return val.toLowerCase() === "s";
            }

            return !!val;
        }

        var self = this;

        self.obterPorDepartamentoESistema = function (cdDepartamento, cdSistema, modoPereciveis, excluirPadaria, isInitializing) {

            return $http
                .get(ApiEndpoints.sgp.departamento, { params: { cdDepartamento: zeroIsValid(cdDepartamento), cdSistema: cdSistema || '', modoPereciveis: modoPereciveis || '', excluirPadaria: excluirPadaria || '', inicializando: isInitializing || false } })
                .then(carregaDepartamento);
        };

        self.pesquisarPorDivisaoESistema = function (cdDepartamento, dsDepartamento, blPerecivel, cdDivisao, cdSistema, excluirPadaria, paging) {

            var params = ApiEndpoints.createParams(paging, {
                cdDepartamento: zeroIsValid(cdDepartamento),
                dsDepartamento: zeroIsValid(dsDepartamento),
                cdSistema: cdSistema || '',
                blPerecivel: blPerecivel || '',
                cdDivisao: zeroIsValid(cdDivisao),
                excluirPadaria: excluirPadaria || ''
            });

            return $http
                .get(ApiEndpoints.sgp.departamento, { params: params })
                .then(carregaDepartamentos);
        };

        self.obterEstruturadoPorId = function(id) {
            var url = "{0}{1}/Estruturado".format(ApiEndpoints.sgp.departamento, ApiEndpoints.encode(id));
            return $http.get(url).then(carregaDepartamento);
        };

        self.atualizarPerecivel = function(idDepartamento, blPerecivel) {
            blPerecivel = toBoolean(blPerecivel);

            var request = {
                idDepartamento: idDepartamento,
                blPerecivel: blPerecivel
            };

            return $http.put(ApiEndpoints.sgp.departamento, request);
        };

        self.obterPorSistema = function (cdSistema, blPerecivel) {
            var params = { cdSistema: cdSistema, blPerecivel: blPerecivel };
            
            return $http
                .get("{0}/PorSistema".format(ApiEndpoints.sgp.departamento), { params: params })
                .then(carregaDepartamento);
        };
    }
})();