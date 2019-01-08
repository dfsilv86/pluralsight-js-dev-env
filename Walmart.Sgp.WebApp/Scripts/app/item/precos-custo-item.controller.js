(function () {
    'use strict';


    // Configuração da controller
    angular
        .module('SGP')
        .config(PrecosCustoItemRoute)
        .controller('PrecosCustoItemController', PrecosCustoItemController);


    // Implementação da controller
    PrecosCustoItemController.$inject = ['$scope', '$q', '$stateParams', '$state', 'ValidationService', 'ItemDetalheService', 'itemDetalhe', 'ItemRelacionamentoService', 'UserSessionService', 'PagingService', 'ToastService', 'StackableModalService', 'EstoqueService'];
    function PrecosCustoItemController($scope, $q, $stateParams, $state, $validation, itemDetalheService, itemDetalhe, itemRelacionamentoService, userSession, pagingService, toastService, $modal, estoqueService) {
        $validation.prepare($scope);

        $scope.filters = { cdLoja: null, loja: null };
        $scope.data = {
            itemDetalhe: itemDetalhe,
            values: [],
            paging: { offset: 0, limit: 10, orderBy: 'cdLoja' }
        };

        // Precisa tratar da restrição aqui por causa do search(1) ali embaixo.
        // Caso contrário não dá tempo da lookup aplicar a restrição.
        var restricao = userSession.getRestricaoLoja();

        if (!!restricao.loja) {
            $scope.filters.loja = restricao.loja;
        }
        else {
            search(1);
        }        

        $scope.back = function () {
            $state.go('pesquisaItem');
        };

        $scope.search = search;
        $scope.expand = expand;
        $scope.collapse = collapse;
        $scope.canExpand = canExpand;
        $scope.isExpanded = isExpanded;
        $scope.sortBy = sortBy;
        $scope.detail = detail;
        
        $scope.$watch('filters.cdLoja', function (newValue, oldValue) {
            if (newValue != oldValue) {
                search(1);
            }
        });    

        function search(pageNumber) {

            // Caso alguma linha da grid tenha sido expandida,
            collapse();

            pagingService.calculateOffset($scope.data.paging, pageNumber);

            var idItemDetalhe = $scope.data.itemDetalhe.idItemDetalhe;
            var idLoja = ($scope.filters.loja || {}).idLoja;

            $q.when(itemDetalheService.obterItemCustos($scope.data.itemDetalhe.cdItem, $stateParams.idBandeira, idLoja, $scope.data.paging))
                .then(exibeCustosPorLoja)
                .catch(escondeCustosPorLoja);
        }

        function sortBy(columnName) {
            pagingService.toggleSorting($scope.data.paging, columnName);
            search();
        }

        function exibeCustosPorLoja(custos) {            
            $scope.data.values = custos;

            pagingService.acceptPagingResults($scope.data.paging, custos);
        }

        function escondeCustosPorLoja() {
            $scope.data.values = [];
        }

        function expand(item) {
            var deferred = $q
                .when(estoqueService.obterOsCincoUltimosCustosDoItemPorLoja(item.itemDetalhe.cdItem, item.loja.idLoja))
                .then(function(cincoUltimosCustos) {
                    item.detalhe = cincoUltimosCustos;
                })
                .catch(collapse);
        }                

        function collapse(item) {
            if (!!item) {
                item.detalhe = null;
            }            
        }

        function canExpand(item) {
            return !item.detalhe;
        }

        function isExpanded(item) {
            return !!item.detalhe;
        }

        function detail(itemDetalhe, loja) {
            var theModal = $modal.open({
                templateUrl: 'Scripts/app/item/informacoes-itens-relacionados.view.html',
                controller: 'InformacoesItensRelacionados',
                resolve: {
                    informacoes: itemRelacionamentoService.obterItensRelacionados(itemDetalhe.cdItem, loja.idLoja),
                    item: itemDetalhe,
                    loja: loja
                }
            });
        }
    }

    // Configuração do estado
    PrecosCustoItemRoute.$inject = ['$stateProvider'];
    function PrecosCustoItemRoute($stateProvider) {

        $stateProvider
            .state('manutencaoItemPrecosCusto', {                
                url: '/informacoes-gerenciais/item/:idBandeira/:cdItem/custos',
                params: {
                    idBandeira: null,
                    cdItem: null,
                    filters: null,
                    paging: null
                },
                templateUrl: 'Scripts/app/item/precos-custo-item.view.html',
                controller: 'PrecosCustoItemController',
                resolve: {
                    itemDetalhe: ['$stateParams', 'ItemDetalheService', function ($stateParams, service) {                        
                        return service.obterInformacoesCadastrais($stateParams.cdItem, $stateParams.idBandeira);
                    }]
                }
            });
    }
})();