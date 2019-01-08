(function () {
    'use strict';


    // Configuração da controller
    angular
        .module('SGP')
        .config(BandeiraRoute)
        .controller('BandeiraController', BandeiraController);


    // Implementação da controller
    BandeiraController.$inject = ['$scope', '$q', '$stateParams', '$state', 'ValidationService', 'PagingService','BandeiraService'];
    function BandeiraController($scope, $q, $stateParams, $state, $validation, pagingService, bandeiraService) {

        $validation.prepare($scope);

        $scope.filters = $stateParams.filters || {
            cdSistema: null,            
            dsBandeira: null
        };
     
        $scope.data = { values: null };
        $scope.data.paging = $stateParams.paging || { offset: 0, limit: 10, orderBy: 'dsBandeira ASC' };
        $scope.search = search;
        $scope.clear = clear;
        $scope.orderBy = orderBy;
        $scope.detail = detail;

        function clear() {
            $scope.filters.cdSistema = $scope.filters.dsBandeira = null;
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
                .when(bandeiraService.pesquisarPorFiltros($scope.filters, $scope.data.paging))
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
            $state.go('cadastroBandeiraEdit', {
                id: item.id
            });
        }

        if (!!$scope.filters && !!$scope.filters.didSearch) {
            // paging já foi persistido e carregado com a última configuração, não é necessário usar $timeout.
            search();
        }
    }

    // Configuração do estado
    BandeiraRoute.$inject = ['$stateProvider'];
    function BandeiraRoute($stateProvider) {

        $stateProvider
            .state('pesquisaBandeira', {
                url: '/cadastro/bandeira',
                params: {
                    filters: null,
                    paging: null
                },
                templateUrl: 'Scripts/app/estruturaMercadologica/pesquisa-bandeira.view.html',
                controller: 'BandeiraController'
            });
    }
})();