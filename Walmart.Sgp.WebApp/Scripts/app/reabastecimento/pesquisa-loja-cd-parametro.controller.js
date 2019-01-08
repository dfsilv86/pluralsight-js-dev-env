(function () {
    'use strict';


    // Configuração da controller
    angular
        .module('SGP')
        .config(LojaCdParametroRoute)
        .controller('LojaCdParametroController', LojaCdParametroController);


    // Implementação da controller
    LojaCdParametroController.$inject = ['$scope', '$q', '$timeout', '$stateParams', '$state', 'UserSessionService', 'ValidationService', 'PagingService', 'ChangeTrackerFactory', 'LojaCdParametroService'];
    function LojaCdParametroController($scope, $q, $timeout, $stateParams, $state, userSession, $validation, pagingService, changeTrackerFactory, LojaCdParametroService) {

        $validation.prepare($scope);

        $scope.filters = $stateParams.filters || {
            cdSistema: null,
            idBandeira: null,
            cdLoja: null,
            nmLoja: null,
            tpReabastecimento: 'C'         
        };
     
        $scope.data = { values: null };
        $scope.data.paging = $stateParams.paging || { offset: 0, limit: 10, orderBy: 'idLojaCdParametro' };
        $scope.search = search;
        $scope.clear = clear;
        $scope.detail = detail;
        $scope.orderBy = orderBy;     

        function clear() {
            $scope.filters.cdSistema = $scope.filters.idBandeira = $scope.filters.cdLoja =
                $scope.filters.nmLoja = null;
            $scope.filters.tpReabastecimento = 'C';
            $scope.data.paging.offset = 0;
            $scope.data.values = [];
            $scope.filters.didSearch = false;
        }
        
        function exibe(data) {
            $scope.data.values = data;            
            pagingService.acceptPagingResults($scope.data.paging, data);

            $scope.filters.didSearch = true;
            $validation.accept($scope);
        }

        function esconde(data) {
            $scope.data.values = [];
            $scope.filters.didSearch = false;
        }

        function search(pageNumber) {
            if (!$validation.validate($scope)) return;

            pagingService.calculateOffset($scope.data.paging, pageNumber);

            // Requisição ao serviço
            var deferred = $q
                .when(LojaCdParametroService.pesquisarPorFiltros($scope.filters, $scope.data.paging))
                .then(exibe)
                .catch(esconde);
        }
      

        function orderBy(field) {
            pagingService.toggleSorting($scope.data.paging, field);
            search();
        }

        function detail(item) {

            $state.update({
                filters: $scope.filters,
                paging: $scope.data.paging
            });

            $state.go('detalheLojaCdParametro', {
                'id': item.id,
                'tipoReabastecimento': item.reviewDateCd.tpReabastecimento
            });
        }

        if (!!$scope.filters && !!$scope.filters.didSearch) {
            // paging já foi persistido e carregado com a última configuração, não é necessário usar $timeout.
            search();
        }
    }

    // Configuração do estado
    LojaCdParametroRoute.$inject = ['$stateProvider'];
    function LojaCdParametroRoute($stateProvider) {

        $stateProvider
            .state('pesquisaLojaCdParametro', {
                url: '/reabastecimento/loja-cd/parametro',
                params: {
                    filters: null,
                    paging: null
                },
                templateUrl: 'Scripts/app/reabastecimento/pesquisa-loja-cd-parametro.view.html',
                controller: 'LojaCdParametroController'
            });
    }
})();