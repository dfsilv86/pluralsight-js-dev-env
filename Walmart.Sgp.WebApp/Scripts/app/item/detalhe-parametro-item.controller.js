(function () {
    'use strict';

    // Configuração da controller
    angular
        .module('SGP')
        .config(DetalheParametroProdutoRoute)
        .controller('DetalheParametroProdutoController', DetalheParametroProdutoController);

    // Implementação da controller
    DetalheParametroProdutoController.$inject = ['$scope', '$q', '$stateParams', '$state', 'ItemDetalheService', 'itemDetalhe', 'UserSessionService'];
    function DetalheParametroProdutoController($scope, $q, $stateParams, $state, itemDetalheService, itemDetalhe, userSession) {
        $scope.data = itemDetalhe;
        $scope.back = back;

        function back() {
            $state.go('pesquisaParametroProduto');
        }
    }

    // Configuração do estado
    DetalheParametroProdutoRoute.$inject = ['$stateProvider'];
    function DetalheParametroProdutoRoute($stateProvider) {

        $stateProvider
            .state('detalheParametroProduto', {
                url: '/item/parametro/:id',
                params: {
                    id: null
                },
                templateUrl: 'Scripts/app/item/detalhe-parametro-item.view.html',
                controller: 'DetalheParametroProdutoController',
                resolve: {
                    itemDetalhe: ['$stateParams', 'ItemDetalheService', function ($stateParams, service) {
                        return service.obterEstruturado($stateParams.id);
                    }]
                }
            });
    }
})();