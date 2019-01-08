(function () {
    'use strict';


    // Configuração da controller
    angular
        .module('SGP')
        .config(InformacoesCadastraisItemRoute)
        .controller('InformacoesCadastraisItemController', InformacoesCadastraisItemController);


    // Implementação da controller
    InformacoesCadastraisItemController.$inject = ['$scope', '$q', '$timeout', '$stateParams', '$state', 'ValidationService', 'ItemDetalheService', 'itemDetalhe', 'ItemRelacionamentoService', 'UserSessionService', 'PagingService', 'ToastService', 'StackableModalService', 'RelacionamentoTransferenciaService'];
    function InformacoesCadastraisItemController($scope, $q, $timeout, $stateParams, $state, $validation, itemDetalheService, itemDetalhe, itemRelacionamentoService, userSession, pagingService, toastService, $modal, relacionamentoTransferenciaService) {

        $validation.prepare($scope);

        $scope.filters = $stateParams.filters || { cdLoja: null, loja: null };
        $scope.data = {
            itemDetalhe: itemDetalhe,
            values: [],
            paging: $stateParams.paging || { offset: 0, limit: 10, orderBy: 'cdLoja' },
            detalhe: null,
            expanded: null
        };

        $scope.back = function () {
            $state.go('pesquisaItem');
        };

        function sortBy(columnName) {
            pagingService.toggleSorting($scope.data.paging, columnName);
            search();
        }

        $scope.sortBy = sortBy;
        $scope.save = save;
        $scope.search = search;
        $scope.detail = detail;
        $scope.relacionamentos = relacionamentos;

        // Permite ao grid-pager tempo para configurar o numero de itens por página
        // Só está aqui porque não tem botão de Pesquisar. Não usar isso em outras telas.
        $timeout(search, 100); // suppress-validator
        
        $scope.$watch('filters.cdLoja', function (newValue, oldValue) {
            if (newValue != oldValue) {
                search(1);
            }
        });

        function save() {
            if (!$validation.validate($scope)) return;
            itemDetalheService.alterarInformacoesCadastrais($scope.data.itemDetalhe).then(function () {
                toastService.success(globalization.texts.savedSuccessfully);
            });
        }

        function search(pageNumber) {
            
            if (!$validation.validate($scope)) return;

            pagingService.calculateOffset($scope.data.paging, pageNumber);

            // TODO: era melhor usar o cdLoja aqui, ajustando a consulta...
            var idLoja = ($scope.filters.loja || {}).idLoja;

            $q.when(itemDetalheService.obterInformacoesEstoquePorLoja($scope.data.itemDetalhe.cdItem, $stateParams.idBandeira, idLoja, $scope.data.paging))
                .then(exibeCustosPorLoja)
                .catch(escondeCustosPorLoja);
        }

        function exibeCustosPorLoja(custos) {
            $scope.data.values = custos;
        }

        function escondeCustosPorLoja() {
            $scope.data.values = [];
        }

        function detail(idItemDetalhe, idLoja) {
            var theModal = $modal.open({
                templateUrl: 'Scripts/app/item/custos-itens-relacionados.view.html',
                controller: 'CustosItensRelacionados',
                resolve: {
                    custos: itemDetalheService.obterUltimoCustoDeItensRelacionadosNaLoja(idItemDetalhe, idLoja)
                }
            });
        }

        function relacionamentos() {
            $state.update({
                filters: $scope.filters,
                paging: $scope.data.paging
            });
            $state.go('manutencaoItemRelacionamento', {
                'cdItem': itemDetalhe.cdItem,
                'idBandeira': $stateParams.idBandeira                
            });
            return false;
        }
    }

    // Configuração do estado
    InformacoesCadastraisItemRoute.$inject = ['$stateProvider'];
    function InformacoesCadastraisItemRoute($stateProvider) {

        $stateProvider
            .state('manutencaoItem', {
                url: '/informacoes-gerenciais/item/:idBandeira/:cdItem',
                params: {
                    cdItem: null,
                    idBandeira: null,
                    filters: null,
                    paging: null
                },
                templateUrl: 'Scripts/app/item/informacoes-cadastrais-item.view.html',
                controller: 'InformacoesCadastraisItemController',
                resolve: {
                    itemDetalhe: ['$stateParams', 'ItemDetalheService', function ($stateParams, service) {
                        return service.obterInformacoesCadastrais($stateParams.cdItem, $stateParams.idBandeira);
                    }]
                }
            });
    }
})();