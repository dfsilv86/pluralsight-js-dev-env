(function () {
    'use strict';

    // Configuração da controller
    angular
        .module('SGP')
        .config(PreparacaoInventarioRoute)
        .controller('PreparacaoInventarioController', PreparacaoInventarioController);


    // Implementação da controller
    PreparacaoInventarioController.$inject = ['$scope', '$q', 'ValidationService', '$state', 'InventarioService', 'ToastService'];
    function PreparacaoInventarioController($scope, $q, $validation, $state, inventarioService, toastService) {

        $validation.prepare($scope);

        $scope.exportIt = exportIt;
        $scope.clear = clear;

        setupFilters();

        function exportIt() {

            if (!$validation.validate($scope)) return;

            var loja = $scope.filters.loja;
            var departamento = $scope.filters.departamento;
            var idTipoRelatorio = $scope.filters.idTipoRelatorio;

            var deferred = $q
                .when(inventarioService.validarDatasAbertasParaImportacaoInventario(loja.idLoja))
                .then(function ()
                {
                    inventarioService.exportarRelatorioPreparacaoInventario(loja, departamento, idTipoRelatorio);
                });
        }
        
        function clear() {

            setupFilters();
        }

        function setupFilters() {

            $scope.filters = {
                cdSistema: null,
                idBandeira: null,
                cdLoja: null,
                cdDepartamento: null,
                opcoesTipoRelatorio: [
                    { idTipoRelatorio: 0, dsTipoRelatorio: globalization.texts.exitItems },
                    { idTipoRelatorio: 1, dsTipoRelatorio: globalization.texts.rawMaterialInput }
                ]
            };
        }
    }

    // Configuração do estado
    PreparacaoInventarioRoute.$inject = ['$stateProvider'];
    function PreparacaoInventarioRoute($stateProvider) {

        $stateProvider
            .state('PreparacaoInventario', {
                url: '/inventario/preparacao',
                templateUrl: 'Scripts/app/inventario/preparacao-inventario.view.html',
                controller: 'PreparacaoInventarioController'
            });
    }
})();