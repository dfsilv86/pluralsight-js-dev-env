(function () {
    'use strict';


    // Configuração da controller
    angular
        .module('SGP')
        .config(InformacoesItensRelacionadosRoute)
        .controller('InformacoesItensRelacionados', InformacoesItensRelacionados);


    // Implementação da controller
    InformacoesItensRelacionados.$inject = ['$scope', '$q', 'informacoes', 'item', 'loja', 'PagingService', '$timeout' ];
    function InformacoesItensRelacionados($scope, $q, informacoes, item, loja, pagingService, $timeout) {

        $scope.item = item;
        $scope.loja = loja;

        $q.when(informacoes).then(function (dados) {
            if (dados.entrada || dados.saida || dados.derivado || dados.insumo || dados.transformado) {
                $scope.data = [
                    { tipoRelacionamento: 'entrada', lista: dados.entrada, itens: [] },
                    { tipoRelacionamento: 'saida', lista: dados.saida, itens: [] },
                    { tipoRelacionamento: 'derivado', lista: dados.derivado, itens: [] },
                    { tipoRelacionamento: 'insumo', lista: dados.insumo, itens: [] },
                    { tipoRelacionamento: 'transformado', lista: dados.transformado, itens: [] },
                ];

                // Arruma o percentual porque vem da procedure como texto com '.' ao invés de ','
                angular.forEach($scope.data, function (tipo) {
                    tipo.itens.offset = 0;
                    tipo.itens.limit = 5;
                    tipo.itens.orderBy = null;
                    tipo.paginar = function (pageNumber) {
                        if (!!tipo.lista && tipo.lista.length > 0) {
                            pagingService.inMemoryPaging(pageNumber, tipo.lista, tipo.itens);
                        }
                    };

                    // Espera o componente de pager embaixo de cada grid carregar para restaurar o número de itens por página que o usuário salvou.
                    $timeout(function () { // suppress-validator
                        tipo.paginar(1);
                    }, 10);

                    angular.forEach(tipo.lista, function (item) {
                        if (!!item.pcRendimento && typeof (item.pcRendimento) === 'string') {
                            item.pcRendimento = item.pcRendimento.replace('.', ',');
                        }
                    });
                })
            }
        });
    }


    // Configuração do estado
    InformacoesItensRelacionadosRoute.$inject = ['$stateProvider'];
    function InformacoesItensRelacionadosRoute($stateProvider) {

        $stateProvider
            .state('informacoesItensRelacionados', {
                url: '/item/:id/itens-relacionados?idLoja',
                templateUrl: 'Scripts/app/item/informacoes-itens-relacionados.view.html',
                controller: 'InformacoesItensRelacionados',
                resolve: {
                    informacoes: ['$stateParams', '$q', 'ItemDetalheService', 'ItemRelacionamentoService', function ($stateParams, $q, itemDetalheService, service) {
                        //return service.obterUltimoCustoDeItensRelacionadosNaLoja($stateParams.id, $stateParams.loja);
                        var deferred = $q.defer();

                        itemDetalheService.obter($stateParams.id || 0).then(function (itemDetalhe) { deferred.resolve(service.obterItensRelacionados(itemDetalhe.cdItem, $stateParams.cdLoja)); }, function () { deferred.reject(); });

                        return $q.promise;
                    }]
                }
            });
    }

})();