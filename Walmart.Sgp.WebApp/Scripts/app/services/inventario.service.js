(function () {
    'use strict';

    angular
        .module('SGP')
        .service('InventarioService', InventarioService);

    InventarioService.$inject = ['$q', '$http', 'ApiEndpoints', '$window', 'UserSessionService'];

    function InventarioService($q, $http, apiEndpoints, $window, userSession) {

        function obterData(response) {
            return response.data;
        }

        function carregaInventario(response) {
            return response.data;
        }

        function tratarResposta(response) {
            return response.data;
        }

        var self = this;

        self.importarAutomatico = function (cdSistema, idBandeira, idLoja, idDepartamento, cdDepartamento) {
            var params = {
                cdSistema: cdSistema || '',
                idBandeira: idBandeira || '',
                idLoja: idLoja || '',
                idDepartamento: idDepartamento || '',
                cdDepartamento: cdDepartamento || ''
            };
            return $http
                .post("{0}Importar/Automatico/Loja".format(apiEndpoints.sgp.inventario), params)
                .then(tratarResposta);
        };

        self.importarManual = function (cdSistema, idBandeira, idLoja, dataInventario, tickets) {

            var params = {
                cdSistema: cdSistema || '',
                idBandeira: idBandeira || '',
                idLoja: idLoja || '',
                dataInventario: dataInventario || '',
                arquivos: tickets || []
            };

            return $http
                .post("{0}Importar/Manual/HO".format(apiEndpoints.sgp.inventario), params)
                .then(tratarResposta);
        };

        self.obterPrefixosArquivos = function () {

            return $http
                .get("{0}Importar/Manual/HO/PrefixosArquivos".format(apiEndpoints.sgp.inventario))
                .then(tratarResposta);
        };

        self.obterDataInventarioDaLoja = function (idLoja) {
            return $http
                .get("{0}Loja/{1}/data".format(apiEndpoints.sgp.inventario, apiEndpoints.encode(idLoja)))
                .then(obterData);
        };

        self.obterQuantidadeLojasSemAgendamento = function (idLoja) {
            return $http
                .get("{0}/loja/naoAgendados/quantidade".format(apiEndpoints.sgp.inventario))
                .then(obterData);
        };

        self.obterAgendamentos = function (filtro) {
            var params = {
                idBandeira: filtro.idBandeira,
                cdLoja: filtro.cdLoja,
                cdDepartamento: filtro.cdDepartamento,
                dtAgendamento: filtro.dtAgendamento
            };

            return $http
                .get("{0}/loja/agendamento".format(apiEndpoints.sgp.inventario), { params: params })
                .then(obterData);
        };

        self.obterNaoAgendados = function (filtro) {
            var params = {
                idBandeira: filtro.idBandeira,
                cdLoja: filtro.cdLoja,
                cdDepartamento: filtro.cdDepartamento,
                dtAgendamento: filtro.dtAgendamento
            };

            return $http
                .get("{0}/loja/naoAgendados".format(apiEndpoints.sgp.inventario), { params: params })
                .then(obterData);
        };

        self.obterSumarizadoPorFiltro = function (filtro, paging) {
            var params = apiEndpoints.createParams(paging, filtro);

            return $http
                .get(apiEndpoints.sgp.inventario + 'sumario', { params: params })
                .then(obterData);
        };

        self.exportarRelatorioInventarioAgendamento = function (bandeira, loja, departamento, dtAgendamento, naoAgendados) {

            var params = {
                bandeira: bandeira ? bandeira.dsBandeira : '',
                idBandeira: bandeira ? bandeira.idBandeira : '',
                nmLoja: loja ? loja.nmLoja : '',
                idLoja: loja ? loja.idLoja : '',
                dsDepartamento: departamento ? departamento.dsDepartamento : '',
                idDepartamento: departamento ? departamento.idDepartamento : '',
                dtAgendamento: dtAgendamento || '',
                naoAgendados: naoAgendados ? 1 : 0,
                token: userSession.getToken()
            };

            return $http
             .post("{0}ExportarRelatorioInventarioAgendamento".format(apiEndpoints.sgp.inventario), params);            
        };

        self.exportarRelatorioPreparacaoInventario = function (loja, departamento, idTipoRelatorio) {

            var params = {
                nmLoja: loja.nmLoja || '',
                idLoja: loja.idLoja || '',
                cdLoja: loja.cdLoja || '',
                idDepartamento: departamento ? departamento.idDepartamento : '',
                idTipoRelatorio: idTipoRelatorio || ''
            };

            return $http
             .post("{0}ExportarRelatorioPreparacaoInventario".format(apiEndpoints.sgp.inventario), params);      
        };

        self.exportarRelatorioComparacaoEstoque = function (loja, dataInventario, statusInventario, departamento, idInventario) {

            var params = {
                loja: loja || '',
                dataInventario: dataInventario || '',
                statusInventario: statusInventario || '',
                departamento: departamento || '',
                idInventario: idInventario || ''
            };

            return $http
             .post("{0}ExportarRelatorioComparacaoEstoque".format(apiEndpoints.sgp.inventario), params);
        };

        self.exportarRelatorioItensModificados = function (loja, idLoja, deptoCateg, idDepartamento, idCategoria, dtInventario) {

            var params = {
                loja: loja || '',
                idLoja: idLoja || '',
                deptoCateg: deptoCateg || '',
                idDepartamento: idDepartamento || '',
                idCategoria: idCategoria || '',
                dtInventario: dtInventario || ''
            };

            return $http
             .post("{0}ExportarRelatorioItensModificados".format(apiEndpoints.sgp.inventario), params);
        };

        self.exportarRelatorioItensPorInventario = function (loja, idLoja, cdLoja, stInventario, dtInventario) {

            var params = {
                loja: loja || '',
                idLoja: idLoja || '',
                cdLoja: cdLoja || '',
                stInventario: stInventario || '',
                dtInventario: dtInventario || ''
            };

            return $http
               .post("{0}ExportarRelatorioItensPorInventario".format(apiEndpoints.sgp.inventario), params);
        };

        self.obterDataProximoInventarioAberto = function (idLoja, cdSistema, idDepartamento) {
            return $http
                .get("{0}Loja/{1}/Data/ProximoAgendamento".format(apiEndpoints.sgp.inventario, apiEndpoints.encode(idLoja)), { params: { cdSistema: cdSistema || '', idDepartamento: idDepartamento || '' } })
                .then(obterData);
        };
      
        self.obterAgendamentoEstruturadoPorId = function (id) {
            return $http
                .get("{0}/loja/agendamento/{1}/estruturado".format(apiEndpoints.sgp.inventario, apiEndpoints.encode(id)))
                .then(obterData);
        };

        self.obterCustoTotalPorFiltro = function (filtro) {
            var sanitized = apiEndpoints.sanitizeUndefinedValues(filtro);            
            if (sanitized.stInventario === '') {
                sanitized.stInventario = null;
            }

            return $http
                .get(apiEndpoints.sgp.inventario + '/custo/total', { params: sanitized })
                .then(obterData);
        };

        self.validarFiltrosRelatorioItensPorInventario = function (cdLoja, dtInventario) {

            var params = {
                cdLoja: cdLoja || '',
                dtInventario: dtInventario || ''
            };

            return $http
                .get(apiEndpoints.sgp.inventario + 'ValidarFiltrosRelatorioItensPorInventario', { params: params });
        };

        self.validarDataObedeceQuantidadeDiasLimiteExpurgo = function (dtAgendamento) {

            var params = {
                dtAgendamento: dtAgendamento || ''
            };

            return $http
                .get(apiEndpoints.sgp.inventario + 'ValidarDataObedeceQuantidadeDiasLimiteExpurgo', { params: params });
        };

        self.validarDatasAbertasParaImportacaoInventario = function (idLoja) {

            return $http
                .put(apiEndpoints.sgp.inventario + 'Loja/{0}/ValidarDatasAbertasParaImportacaoInventario'.format(apiEndpoints.encode(idLoja)));
        };

        self.removerAgendamentos = function (agendamentos) {
            var ids = agendamentos.map(function (a) { return a.id; });

            return $http
                .post("{0}/loja/agendamento/remover".format(apiEndpoints.sgp.inventario), { 'ids': ids })
                .then(obterData);
        };

        self.inserirAgendamentos = function (agendamento) {
            var params = {
                dtAgendamento: agendamento.dtAgendamento,
                cdSistema: agendamento.bandeira.cdSistema,
                idBandeira: agendamento.bandeira.idBandeira,
                cdLoja: agendamento.loja.cdLoja,
                cdDepartamento: agendamento.departamento.cdDepartamento               
            };
            
            return $http
                .post("{0}agendamentos".format(apiEndpoints.sgp.inventario), params)
                .then(obterData);
        };

        self.atualizarAgendamentos = function (agendamentos) {            
            return $http
                .put("{0}agendamentos".format(apiEndpoints.sgp.inventario), agendamentos)
                .then(obterData);
        };

        self.obterEstruturado = function (id) {
            return $http
                .get("{0}{1}/estruturado".format(apiEndpoints.sgp.inventario, apiEndpoints.encode(id)))
                .then(obterData);
        };

        self.obterPermissoesAlteracao = function(inventario) {
            var params = {
                idInventario: inventario.idInventario,
                stInventario: inventario.stInventario,
                dhInventario: inventario.dhInventario,
                idLoja: inventario.idLoja,
                idDepartamento: inventario.idDepartamento
            };

            params = apiEndpoints.sanitizeUndefinedValues(params);
            return $http
                .get("{0}{1}".format(apiEndpoints.sgp.inventario, 'PermissoesAlteracao'),
                { params: params }).then(obterData);
        };

        self.obterItensEstruturadoPorFiltro = function (idInventario, filtro, paging) {
            filtro = JSON.flatten({ filtro: apiEndpoints.sanitizeUndefinedValues(filtro) });
            var params = apiEndpoints.createParams(
                paging, filtro);

            var url = "{0}{1}/{2}".format(apiEndpoints.sgp.inventario, apiEndpoints.encode(idInventario), 'Itens');


            return $http
                .get(url, { params: params })
                .then(obterData);
        };

        self.obterItemEstruturadoPorId = function(id) {
            var url = "{0}{1}/{2}".format(apiEndpoints.sgp.inventario, 'Item', apiEndpoints.encode(id));
            return $http.get(url).then(obterData);
        };

        self.salvarItem = function(item, inventario) {
            var url = "{0}{1}/Item/".format(apiEndpoints.sgp.inventario, apiEndpoints.encode(inventario.id));
            var params = item;

            if (item.isNew) {
                return $http.post(url, params);                
            }

            url = url + item.id;
            return $http.put(url, params);
        };

        self.removerItem = function(idInventarioItem) {
            var url = "{0}Item/{1}".format(apiEndpoints.sgp.inventario, apiEndpoints.encode(idInventarioItem));
            return $http.delete(url);
        };

        self.obterIrregularidadesInventarioFinalizacao = function(idInventario) {
            var url = "{0}{1}/finalizacao/irregularidades".format(apiEndpoints.sgp.inventario, apiEndpoints.encode(idInventario));
            return $http.get(url).then(obterData);
        };
        
        self.finalizar = function(idInventario) {
            var url = "{0}{1}/finalizar".format(apiEndpoints.sgp.inventario, apiEndpoints.encode(idInventario));
            return $http.put(url);
        };

        self.cancelar = function(idInventario) {
            var url = "{0}{1}/cancelar".format(apiEndpoints.sgp.inventario, apiEndpoints.encode(idInventario));
            return $http.put(url);
        };

        self.voltarStatus = function (idInventario) {
            var url = "{0}{1}/voltarstatus".format(apiEndpoints.sgp.inventario, apiEndpoints.encode(idInventario));
            return $http.put(url);
        };

        self.aprovar = function (idInventario) {
            var url = "{0}{1}/aprovar".format(apiEndpoints.sgp.inventario, apiEndpoints.encode(idInventario));
            return $http.put(url);
        };

        self.obterIrregularidadesInventarioAprovacao = function (idInventario) {
            var url = "{0}{1}/aprovacao/irregularidades".format(apiEndpoints.sgp.inventario, apiEndpoints.encode(idInventario));
            return $http.get(url).then(obterData);
        };

        self.pesquisarCriticas = function (filtro, paging) {
            var params = apiEndpoints.createParams(
                paging, 
                {
                    idBandeira: filtro.idBandeira,
                    idLoja: filtro.loja == null ? null : filtro.loja.idLoja,
                    idDepartamento: filtro.departamento == null ? null : filtro.departamento.idDepartamento,
                    idCategoria: filtro.categoria == null ? null : filtro.categoria.idCategoria,
                    dhInclusao: filtro.dhInclusao
                });

            
            return $http
                .get(apiEndpoints.sgp.inventario + 'Criticas', { params: JSON.flatten(params)})
                .then(obterData);
        };
    }
})();