(function () {
    'use strict';
    // Configuração da controller
    angular
        .module('SGP')
        .config(ManutencaoLojaRoute)
        .controller('ManutencaoLojaController', ManutencaoLojaController);

    // Implementação da controller
    ManutencaoLojaController.$inject = ['$scope', '$stateParams', '$state', 'ToastService', 'ValidationService', 'LojaService', 'loja', 'ConfirmService'];
    function ManutencaoLojaController($scope, $stateParams, $state, toast, $validation, lojaService, loja, confirmService) {
        $validation.prepare($scope);

        // A dropdown de bandeiras mostra apenas aquelas visiveis para o usuario, cfe cadLoja.aspx.cs linha 285
        load(loja);

        $scope.temp = { user: null };
        $scope.back = back;
        $scope.save = save;

        function load(loja) {
            $scope.filters = { idFormato: (loja.bandeira || {}).idFormato, idRegiao: (loja.distrito || {}).idRegiao };
            $scope.data = loja;

            if ($scope.data.cdSistema == 1 && $scope.data.tipoArquivoInventario > 1) {
                $scope.data.tipoArquivoInventario = 1;
            }            
        }

        function save() {
            if (!$validation.validate($scope)) return;

            $scope.data.bandeira = $scope.data.bandeira || {};
            $scope.data.bandeira.idFormato = $scope.filters.idFormato;
            $scope.data.distrito = $scope.data.distrito || {};
            $scope.data.distrito.idRegiao = $scope.filters.idRegiao;

            lojaService.alterarLoja($scope.data)
                .then(function (lojaAlterada) {                    
                    loja = lojaAlterada;
                    load(loja);
                    toast.success(globalization.texts.savedSuccessfully);
                });              
        }

        function back() {            
            $state.go('manutencaoLoja');
        }
    }

    // Configuração do estado
    ManutencaoLojaRoute.$inject = ['$stateProvider'];
    function ManutencaoLojaRoute($stateProvider) {

        $stateProvider
            .state('manutencaoLojaEdit', {
                url: '/gerenciamento/loja/:id',
                params: {
                    paging: null,
                    id: null,
                    previous: null
                },
                templateUrl: 'Scripts/app/gerenciamento/manutencao-loja.view.html',
                controller: 'ManutencaoLojaController',
                resolve: {
                    loja: ['$stateParams', 'LojaService', '$q', function ($stateParams, service) {
                        return service.obterEstruturadoPorId($stateParams.id);
                    }]
                }
            });
    }
})();