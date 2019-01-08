(function () {
    'use strict';


    // Configuração da controller
    angular
        .module('SGP')
        .config(CustosItemRoute)
        .controller('CustosItemController', CustosItemController);


    // Implementação da controller
    CustosItemController.$inject = ['$scope', '$q', '$stateParams', '$state', 'ValidationService', 'ItemDetalheService', 'itemDetalhe', 'ItemRelacionamentoService', 'UserSessionService', 'PagingService', 'ToastService', 'StackableModalService', 'capitalizeFilter'];
    function CustosItemController($scope, $q, $stateParams, $state, $validation, itemDetalheService, itemDetalhe, itemRelacionamentoService, userSession, pagingService, toastService, $modal, capitalizeFilter) {

        $validation.prepare($scope);

        $scope.filters = { cdLoja: null, loja: null, cdBandeira: null };

        // Precisa tratar da restrição aqui pra não retirar o if values === null no $watch de cdLoja
        // O if values === null é para pesquisar ao abrir a tela.
        var restricao = userSession.getRestricaoLoja();

        if (!!restricao.loja) {
            $scope.filters.loja = restricao.loja;
        }

        $scope.data = {
            itemDetalhe: itemDetalhe,
            'yield': null,
            values: null,
            paging: { offset: 0, limit: 10, orderBy: null },
            detalhe: null,
            expanded: null
        };

        $scope.back = null;

        if (!!$stateParams.tipoRelacionamento) {
            $scope.back = function () {
                $state.go('manutencaoRelacionamento' + capitalizeFilter($stateParams.tipoRelacionamento) + 'Edit');
            };
        }

        $scope.save = save;

        $scope.search = search;
        $scope.expand = expand;
        $scope.collapse = collapse;
        $scope.canExpand = canExpand;
        $scope.isExpanded = isExpanded;
        $scope.detail = detail;

        if (itemDetalhe && itemDetalhe.idItemDetalhe) {
            // TODO: deveria utilizar valor oriundo do sgpFixedValues.tipoReceituario.
            if (itemDetalhe.tpReceituario == 'T') {
                itemRelacionamentoService
                    .obterPercentualRendimentoTransformado(itemDetalhe.idItemDetalhe, itemDetalhe.cdSistema)
                    .then(exibePercentual);
            }

            // TODO: deveria utilizar valor oriundo do sgpFixedValues.tipoManipulado.
            if (itemDetalhe.tpManipulado == 'D') {
                itemRelacionamentoService
                    .obterPercentualRendimentoDerivado(itemDetalhe.idItemDetalhe, itemDetalhe.cdSistema)
                    .then(exibePercentual);
            }
        }

        //search(1);

        $scope.$watch('filters.cdLoja', function (newValue, oldValue) {
            // O if values === null é para pesquisar ao abrir a tela.
            if (newValue !== oldValue || $scope.data.values === null) {
                search(1);
            }
        });

        function exibePercentual(valor) {
            $scope.data['yield'] = valor;
        }

        function save() {
            itemDetalheService.alterarDadosCustos($scope.data.itemDetalhe).then(function () {
                toastService.success(globalization.texts.savedSuccessfully);
            });
        }

        function search(pageNumber) {

            if (!$validation.validate($scope)) return;

            // Caso alguma linha da grid tenha sido expandida,
            collapse();

            pagingService.calculateOffset($scope.data.paging, pageNumber);

            var idItemDetalhe = $scope.data.itemDetalhe.idItemDetalhe;
            var idLoja = ($scope.filters.loja || {}).idLoja;

            var deferred = $q
                .when(itemDetalheService.obterUltimoCustoDoItemPorLoja(idItemDetalhe, idLoja, $scope.data.paging))
                .then(exibeCustosPorLoja)
                .catch(escondeCustosPorLoja);
        }

        function exibeCustosPorLoja(custos) {
            $scope.data.values = custos;
            $scope.filters.cdBandeira = null;
        }

        function escondeCustosPorLoja() {
            $scope.data.values = [];
        }

        function expand(item) {

            $scope.data.expanded = item;
            $scope.data.detalhe = null;

            var deferred = $q
                .when(itemDetalheService.obterOsCincoUltimosRecebimentosDoItemPorLoja(item.idItemDetalhe, item.idLoja))
                .then(exibeCincoUltimosCustos)
                .catch(collapse);
        }

        function exibeCincoUltimosCustos(custos) {
            var result = [];
            for (var i = 0; i < custos.length; i++) {
                var nf = custos[i];
                for (var j = 0; j < nf.itens.length; j++) {
                    var nfi = nf.itens[j];
                    var id = nfi.itemDetalhe;

                    result.push({
                        notaFiscal: nf,
                        loja: nf.loja,
                        notaFiscalItem: nfi,
                        itemDetalhe: id
                    });
                }
            }
            $scope.data.detalhe = result;
        }

        function collapse() {
            $scope.data.detalhe = $scope.data.expanded = null;
        }

        function canExpand(item) {
            return true;
        }

        function isExpanded(item) {
            return $scope.data.expanded === item;
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
    }

    // Configuração do estado
    CustosItemRoute.$inject = ['$stateProvider'];
    function CustosItemRoute($stateProvider) {

        $stateProvider
            .state('custosItemVinculado', {
                params: {
                    id: null,
                    idItemDetalhe: null,
                    tipoRelacionamento: 'vinculado'
                },
                url: '/item/relacionamento/vinculado/edit/:id/custos/:idItemDetalhe',
                templateUrl: 'Scripts/app/item/custos-item.view.html',
                controller: 'CustosItemController',
                resolve: {
                    itemDetalhe: ['$stateParams', 'ItemDetalheService', function ($stateParams, service) {
                        return service.obterEstruturado($stateParams.idItemDetalhe);
                    }]
                }
            })
            .state('custosItemManipulado', {
                params: {
                    id: null,
                    idItemDetalhe: null,
                    tipoRelacionamento: 'manipulado'
                },
                url: '/item/relacionamento/manipulado/edit/:id/custos/:idItemDetalhe',
                templateUrl: 'Scripts/app/item/custos-item.view.html',
                controller: 'CustosItemController',
                resolve: {
                    itemDetalhe: ['$stateParams', 'ItemDetalheService', function ($stateParams, service) {
                        return service.obterEstruturado($stateParams.idItemDetalhe);
                    }]
                }
            }).state('custosItemReceituario', {
                params: {
                    id: null,
                    idItemDetalhe: null,
                    tipoRelacionamento: 'receituario'
                },
                url: '/item/relacionamento/receituario/edit/:id/custos/:idItemDetalhe',
                templateUrl: 'Scripts/app/item/custos-item.view.html',
                controller: 'CustosItemController',
                resolve: {
                    itemDetalhe: ['$stateParams', 'ItemDetalheService', function ($stateParams, service) {
                        return service.obterEstruturado($stateParams.idItemDetalhe);
                    }]
                }
            }).state('custosItem', {
                params: {
                    idItemDetalhe: null,
                },
                url: '/item/custos/:idItemDetalhe',
                templateUrl: 'Scripts/app/item/custos-item.view.html',
                controller: 'CustosItemController',
                resolve: {
                    itemDetalhe: ['$stateParams', 'ItemDetalheService', function ($stateParams, service) {
                        return service.obterEstruturado($stateParams.idItemDetalhe);
                    }]
                }
            });
    }
})();