(function () {
    'use strict';

    // Configuração da controller
    angular
        .module('SGP')
        .config(ItensAbcVendasRoute)
        .controller('ItensAbcVendasController', ItensAbcVendasController);


    // Implementação da controller
    ItensAbcVendasController.$inject = ['$scope', '$q', 'ValidationService', '$state', 'ItemDetalheService', 'ToastService'];
    function ItensAbcVendasController($scope, $q, $validation, $state, itemDetalheService, toastService) {

        $validation.prepare($scope);

        $scope.exportIt = exportIt;
        $scope.clear = clear;
        $scope.sistemas = {
            supercenter: sgpFixedValues.tipoSistema.supercenter,
            samsClub: sgpFixedValues.tipoSistema.samsClub
        };

        setupFilters();

        function exportIt() {

            if (!$validation.validate($scope)) return;

            var deferred = $q
                .when(itemDetalheService.validarIntervaloNaoDeveExcederSessentaDias($scope.filters.periodo))
                .then(function () {

                    var loja = "{0} - {1}".format($scope.filters.loja.cdLoja, $scope.filters.loja.nmLoja);
                    var idLoja = $scope.filters.loja.idLoja;
                    var deptoCat = null;
                    var lblDeptoCat = null;
                    var idDepartamento = null;

                    if ($scope.filters.departamento) {
                        deptoCat = "{0} - {1}".format($scope.filters.departamento.dsDepartamento, $scope.filters.departamento.cdDepartamento);
                        idDepartamento = $scope.filters.departamento.idDepartamento;
                    }

                    if ($scope.filters.bandeira.cdSistema == $scope.sistemas.supercenter) {
                        lblDeptoCat = "Depto.:"
                    }
                    else if ($scope.filters.bandeira.cdSistema == $scope.sistemas.samsClub) {
                        lblDeptoCat = "Categ.:"
                    }

                    itemDetalheService.exportarRelatorioItensAbcVendas(loja, idLoja, deptoCat, lblDeptoCat, idDepartamento, null, $scope.filters.periodo);
                });
        }
        
        function clear() {

            setupFilters();
        }

        function setupFilters() {

            $scope.filters = {
                cdSistema: null,
                bandeira: null,
                idBandeira: null,
                loja: null,
                cdLoja: null,
                departamento: null,
                cdDepartamento: null,
                periodo: { startValue: null, endValue: null }
            };
        }
    }

    // Configuração do estado
    ItensAbcVendasRoute.$inject = ['$stateProvider'];
    function ItensAbcVendasRoute($stateProvider) {

        $stateProvider
            .state('ItensAbcVendas', {
                url: '/gerenciamento/relatorios/itensAbcVendas',
                templateUrl: 'Scripts/app/gerenciamento/relatorio-itens-abc-vendas.view.html',
                controller: 'ItensAbcVendasController'
            });
    }
})();