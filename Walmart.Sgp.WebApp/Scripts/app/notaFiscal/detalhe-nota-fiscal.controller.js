(function () {
    'use strict';

    // Configuração da controller
    angular
        .module('SGP')
        .config(DetalheNotaFiscalRoute)
        .controller('DetalheNotaFiscalController', DetalheNotaFiscalController);

    // Implementação da controller
    DetalheNotaFiscalController.$inject = ['$scope', '$q', '$stateParams', '$state', 'NotaFiscalService', 'notaFiscal', 'UserSessionService', 'PagingService'];
    function DetalheNotaFiscalController($scope, $q, $stateParams, $state, notaFiscalService, notaFiscal, userSession, pagingService) {

        var ordenacaoPadrao = 'IDLoja asc';
        
        $scope.data = notaFiscal;
        $scope.data.paging = { offset: 0, limit: 10, orderBy: null };
        $scope.back = $state.is('notaFiscalEdit') ? back : null;
        $scope.paginate = paginate;

        function back() {
            $state.go('pesquisaNotaFiscal');
        }

        function paginate(pageNumber) {
            pagingService.calculateOffset($scope.data.paging, pageNumber);
            notaFiscalService.obterItensDaNotaFiscal(notaFiscal.idNotaFiscal, $scope.data.paging)
                .then(function (itens) {
                    $scope.data.itens = itens;
                    pagingService.acceptPagingResults($scope.data.paging, itens);
                });
        }

        paginate();
    }

    // Configuração do estado
    DetalheNotaFiscalRoute.$inject = ['$stateProvider'];
    function DetalheNotaFiscalRoute($stateProvider) {

        $stateProvider
            .state('notaFiscalEdit', {
                url: '/notafiscal/pesquisa/detalhe/:id',
                params: {
                    id: null
                },
                templateUrl: 'Scripts/app/notaFiscal/detalhe-nota-fiscal.view.html',
                controller: 'DetalheNotaFiscalController',
                resolve: {
                    notaFiscal: ['$stateParams', 'NotaFiscalService', function ($stateParams, service) {
                        return service.obterEstruturado($stateParams.id);
                    }]
                }
            });
    }
})();