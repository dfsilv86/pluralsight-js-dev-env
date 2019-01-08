(function () {
    'use strict';

    // Configuração da controller
    angular
        .module('SGP')
        .config(CadastroBandeiraRegiaoRoute)
        .controller('CadastroBandeiraRegiaoController', CadastroBandeiraRegiaoController);

    // Implementação da controller
    CadastroBandeiraRegiaoController.$inject = ['$scope', '$q', '$stateParams', '$state', 'ValidationService', 'ToastService', 'regiao'];
    function CadastroBandeiraRegiaoController($scope, $q, $stateParams, $state, $validation, toast, regiao) {
        $scope.data = regiao;
        $scope.data.tempDistrito = { nmDistrito: '' };
        $scope.back = back;
        $scope.adicionarDistrito = adicionarDistrito;
        $scope.removerDistrito = removerDistrito;               
        $validation.prepare($scope);

        function adicionarDistrito() {
            var distrito = $scope.data.tempDistrito;

            removerDistrito(distrito);
            $scope.data.distritos.push({ 'nmDistrito': distrito.nmDistrito });
            distrito.nmDistrito = '';
        }

        function removerDistrito( distrito) {            
            $scope.data.distritos.removeByProperty('nmDistrito', distrito.nmDistrito);
        }              

        function back() {
            $modal.close();
        }
    }

    // Configuração do estado
    CadastroBandeiraRegiaoRoute.$inject = ['$stateProvider'];
    function CadastroBandeiraRegiaoRoute($stateProvider) {

        $stateProvider
            .state('cadastroBandeiraRegiao', {
                url: '/cadastro/bandeira/regiao',
                params: {
                    regiao: null
                },
                templateUrl: 'Scripts/app/estruturaMercadologica/cadastro-bandeira.view.html',
                controller: 'CadastroBandeiraRegiaoController'
            });
    }
})();