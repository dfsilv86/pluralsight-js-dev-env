(function () {
    'use strict';


    // Configuração da controller
    angular
        .module('SGP')
        .config(InventarioCriticaRoute)
        .controller('InventarioCriticaController', InventarioCriticaController);


    // Implementação da controller
    InventarioCriticaController.$inject = ['$scope', '$q', '$stateParams', '$state', 'ValidationService', 'PagingService','InventarioService'];
    function InventarioCriticaController($scope, $q, $stateParams, $state, $validation, pagingService, inventarioService) {

        $validation.prepare($scope);
        $scope.zero = { cdDepartamento: null };
        $scope.filters = $stateParams.filters || createDefaultFilters();     
        $scope.data = { values: null };
        $scope.data.paging = $stateParams.paging || { offset: 0, limit: 10, orderBy: 'dhInclusao ASC' };
        $scope.search = search;
        $scope.clear = clear;
        $scope.sistemas = {
            supercenter: sgpFixedValues.tipoSistema.supercenter,
            samsClub: sgpFixedValues.tipoSistema.samsClub
        };
        $scope.cdSistemaSearch = $scope.sistemas.supercenter;

        if ($stateParams.filters) {
            search();
        }

        $scope.$watch('filters.categoria', function (nv, ov) {
            if (null == nv) {
                $scope.zero.cdDepartamento = null;
            }
        });

        function createDefaultFilters()
        {
            return {
                cdSistema: null,
                idBandeira: null,
                cdLoja: null,
                cdDepartamento: null,
                dhInclusao: null
            };
        }

        function clear() {
            $scope.filters = createDefaultFilters();
            $scope.cdSistemaSearch = $scope.sistemas.supercenter;
            $scope.data.paging.offset = 0;
            $scope.data.values = [];
        }
        
        function exibe(data) {
            $scope.data.values = data;            
            pagingService.acceptPagingResults($scope.data.paging, data);
        }

        function esconde(data) {
            $scope.data.values = [];
        }

        function search(pageNumber) {
            if (!$validation.validate($scope)) return;

            pagingService.calculateOffset($scope.data.paging, pageNumber);

            $scope.cdSistemaSearch = $scope.filters.cdSistema;
            // Requisição ao serviço
            var deferred = $q
                .when(inventarioService.pesquisarCriticas($scope.filters, $scope.data.paging))
                .then(exibe)
                .catch(esconde);
        }
    }

    // Configuração do estado
    InventarioCriticaRoute.$inject = ['$stateProvider'];
    function InventarioCriticaRoute($stateProvider) {

        $stateProvider
            .state('pesquisaInventarioCritica', {
                url: '/inventario/critica',
                params: {
                    filters: null,
                    paging: null
                },
                templateUrl: 'Scripts/app/inventario/pesquisa-inventario-critica.view.html',
                controller: 'InventarioCriticaController'
            });
    }
})();