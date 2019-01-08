(function () {
    'use strict';

    // COnfiguração da controller
    angular
        .module('SGP')
        .config(CadastroRelacionamentoTransferenciaRoute)
        .controller('CadastroRelacionamentoTransferenciaController', CadastroRelacionamentoTransferenciaController);

    // Implementação da controller
    CadastroRelacionamentoTransferenciaController.$inject = ['$scope', '$q', '$timeout', '$stateParams', '$state', 'ValidationService', 'RelacionamentoTransferenciaService', 'LojaService', 'relacionamentoTransferencia', 'PagingService'];
    function CadastroRelacionamentoTransferenciaController($scope, $q, $timeout, $stateParams, $state, $validation, relacionamentoTransferenciaService, lojaService, relacionamentoTransferencia, pagingService) {

        $validation.prepare($scope);

        $scope.isManutencaoItemRelacionamento = (relacionamentoTransferencia == null);

        $scope.filters = {
            cdSistema: null,
            cdItemDestino: null,
            cdItemOrigem: null,
            idBandeira: null
        };

        var filtersLojas = {
            cdSistema: null,
            idDestino: null,
            idOrigem: null,
            idLoja: null
        };

        $scope.filters = {
            cdSistema: $stateParams.cdSistema,
            cdItemDestino: $stateParams.cdItem || $stateParams.cdItemDestino,
            cdItemOrigem: $stateParams.cdItemOrigem,
            idBandeira: $stateParams.idBandeira
        };

        if (!$scope.isManutencaoItemRelacionamento) {
            filtersLojas.cdSistema = $stateParams.cdSistema;
            filtersLojas.idDestino = relacionamentoTransferencia.idItemDetalheDestino;
            filtersLojas.idOrigem = relacionamentoTransferencia.idItemDetalheOrigem;
            filtersLojas.idLoja = relacionamentoTransferencia.idLoja;
        }

        setupData();

        function setupData() {
            $scope.data = { lojas: null, itens: null };
            $scope.data.paging = { offset: 0, limit: 10, orderBy: null };
            $scope.data.pagingLojas = { offset: 0, limit: 10, orderBy: null };
        }

        $scope.$watchGroup(['filters.cdLoja', 'filters.cdItemOrigem'], refresh);
        function refresh(newValues, oldValues) {
            if ($scope.filters.itemDestino != null && $scope.filters.itemOrigem) {
                filtersLojas.cdSistema = $scope.filters.cdSistema;
                filtersLojas.idDestino = $scope.filters.itemDestino.id;
                filtersLojas.idOrigem = $scope.filters.itemOrigem.id;
                filtersLojas.idLoja = $scope.filters.loja != null ? $scope.filters.loja.id : null;
                carregaLojas(filtersLojas, 1);
            }
        }

        function back() {
            if (!$scope.isManutencaoItemRelacionamento) {
                $state.go('pesquisaRelacionamentoTransferencia');
            } else {
                $state.go('manutencaoItem');
            }
        }

        $scope.back = back;

        function save() {
        }

        $scope.save = save;

        /* Lojas Trait */

        carregaLojas(filtersLojas, 1);

        function esconderLojas() {
            $scope.data.lojas = null;
        }

        function exibirLojas(data) {
            $scope.data.lojas = data;

            pagingService.acceptPagingResults($scope.data.pagingLojas, data);
        }

        function carregaLojas(filtro, pageNumber) {

            if (filtro.idDestino == null || filtro.idOrigem == null) return;

            pagingService.calculateOffset($scope.data.pagingLojas, pageNumber);

            lojaService
                .pesquisarPorItemDestinoOrigem(filtro, $scope.data.pagingLojas)
                .then(exibirLojas)
                .catch(esconderLojas);
        }

        function marcarTodasLojas() {
            var lojas = $scope.data.lojas;
            lojas.setAll('marcado', lojas.marcado);
        }

        $scope.marcarTodasLojas = marcarTodasLojas;

        /* Itens Relacionados */

        if (!$scope.isManutencaoItemRelacionamento) {
            carregarItensRelacionados(relacionamentoTransferencia.idItemDetalheDestino, 1);
        } else if ($scope.filters.cdItemDestino != null && $scope.filters.cdItemDestino != undefined) {
            carregarItensRelacionadosPorCdItemDestino($scope.filters.cdItemDestino, 1);
        }

        function pageItens(pageNumber) {

            if (!$scope.isManutencaoItemRelacionamento) {
                carregarItensRelacionados(relacionamentoTransferencia.idItemDetalheDestino, pageNumber);
            } else if ($scope.filters.cdItemDestino != null && $scope.filters.cdItemDestino != undefined) {
                carregarItensRelacionadosPorCdItemDestino($scope.filters.cdItemDestino, pageNumber);
            }
        }

        function esconderItens() {
            $scope.data.itens = null;
        }

        function exibirItens(data) {
            $scope.data.itens = data;

            pagingService.acceptPagingResults($scope.data.paging, data);
        }

        function carregarItensRelacionados(idItemDetalheOrigem, pageNumber) {

            if (idItemDetalheOrigem == null || idItemDetalheOrigem == undefined) return;

            pagingService.calculateOffset($scope.data.paging, pageNumber);

            relacionamentoTransferenciaService
                .pesquisarItensRelacionados(idItemDetalheOrigem, $scope.data.paging)
                .then(exibirItens)
                .catch(esconderItens);
        }

        function carregarItensRelacionadosPorCdItemDestino(cdItemDestino, pageNumber) {

            if (cdItemDestino == null || cdItemDestino == undefined) return;

            pagingService.calculateOffset($scope.data.paging, pageNumber);

            relacionamentoTransferenciaService
                .pesquisarItensRelacionadosPorCdItemDestino(cdItemDestino, $scope.data.paging)
                .then(exibirItens)
                .catch(esconderItens);
        }

        function marcarTodos() {
            var itens = $scope.data.itens;
            itens.setAll('marcado', itens.marcado);
        }

        $scope.marcarTodos = marcarTodos;

        /* Relacionar */

        function criarRelacionamento() {

            if (!$validation.validate($scope)) return;

            var idItemDestino = $scope.filters.itemDestino.id;
            var idItemOrigem = $scope.filters.itemOrigem.id;
            var data = $scope.data;
            var lojasMarcadas = [];

            for (var i = 0; i < data.lojas.length; i++)
                if (data.lojas[i].marcado)
                    lojasMarcadas.push(data.lojas[i]);

            relacionamentoTransferenciaService
                .criarTransferencia(idItemDestino, idItemOrigem, lojasMarcadas)
                .then(refreshGrids);
        }

        $scope.criarRelacionamento = criarRelacionamento;

        /* Excluir relacionamento */

        function removerRelacionamento() {
            var data = $scope.data;
            var itensMarcados = [];

            for (var i = 0; i < data.itens.length; i++)
                if (data.itens[i].marcado)
                    itensMarcados.push(data.itens[i]);

            relacionamentoTransferenciaService
                .removerTransferencia(itensMarcados)
                .then(refreshGrids);
        }

        $scope.removerRelacionamento = removerRelacionamento;

        function refreshGrids() {
            esconderLojas();
            carregaLojas(filtersLojas, 1);
            esconderItens();

            if (!$scope.isManutencaoItemRelacionamento) {
                carregarItensRelacionados(relacionamentoTransferencia.idItemDetalheDestino, 1);
            } else if ($scope.filters.cdItemDestino != null && $scope.filters.cdItemDestino != undefined) {
                carregarItensRelacionadosPorCdItemDestino($scope.filters.cdItemDestino, 1);
            }
        }
    }

    // Configuração do estado
    CadastroRelacionamentoTransferenciaRoute.$inject = ['$stateProvider'];
    function CadastroRelacionamentoTransferenciaRoute($stateProvider) {
        $stateProvider
            .state('cadastroRelacionamentoTransferencia', {
                url: '/item/relacionamento-mtr/por-relacionamento/edit/:idRelacionamento?cdItemDestino&cdItemOrigem&cdSistema&idBandeira',
                params: {
                    idRelacionamento: null,
                    cdItemDestino: null,
                    cdItemOrigem: null,
                    cdSistema: null,
                    idBandeira: null
                },
                templateUrl: 'Scripts/app/item/cadastro-item-relacionamento-transferencia.view.html',
                controller: 'CadastroRelacionamentoTransferenciaController',
                resolve: {
                    relacionamentoTransferencia: ['$stateParams', 'RelacionamentoTransferenciaService', function ($stateParams, service) {
                        return service.obterEstruturado($stateParams.idRelacionamento);
                    }]
                }
            })
            .state('manutencaoItemRelacionamento', {
                url: '/informacoes-gerenciais/item/:idBandeira/:cdItem/relacionamento-mtr',
                params: {
                    cdItem: null,
                    idBandeira: null
                },
                templateUrl: 'Scripts/app/item/cadastro-item-relacionamento-transferencia.view.html',
                controller: 'CadastroRelacionamentoTransferenciaController',
                resolve: {
                    relacionamentoTransferencia: ['$stateParams', 'RelacionamentoTransferenciaService', function ($stateParams, service) {
                        return null;
                    }]
                }
            });
    }
})();