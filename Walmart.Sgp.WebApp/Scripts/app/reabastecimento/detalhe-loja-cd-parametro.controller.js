(function () {
    'use strict';

    // Configuração da controller
    angular
        .module('SGP')
        .config(DetalheLojaCdParametroRoute)
        .controller('DetalheLojaCdParametroController', DetalheLojaCdParametroController);

    // Implementação da controller
    DetalheLojaCdParametroController.$inject = ['$scope', '$q', '$stateParams', '$state', 'ValidationService', 'ToastService', 'LojaCdParametroService', 'lojaCdParametro'];
    function DetalheLojaCdParametroController($scope, $q, $stateParams, $state, $validation, toast, service, lojaCdParametro) {
        $scope.data = lojaCdParametro;
        $scope.save = save;
        $scope.back = back;
        $validation.prepare($scope);

        function back() {
            $state.go('pesquisaLojaCdParametro');
        }

        function save() {

            if (!$validation.validate($scope)) return;

            var lojaCdParametro = {};

            angular.copy($scope.data, lojaCdParametro);
     
            service
                .salvar(lojaCdParametro)
                .then(function (data) {
                    toast.success(globalization.texts.savedSuccessfully);
                });
        }
    }

    // Configuração do estado
    DetalheLojaCdParametroRoute.$inject = ['$stateProvider'];
    function DetalheLojaCdParametroRoute($stateProvider) {

        $stateProvider
            .state('detalheLojaCdParametro', {
                url: '/reabastecimento/loja-cd/parametro/{id}/{tipoReabastecimento}',
                params: {
                    id: null,
                    tipoReabastecimento: null
                },
                templateUrl: 'Scripts/app/reabastecimento/detalhe-loja-cd-parametro.view.html',
                controller: 'DetalheLojaCdParametroController',
                resolve: {
                    lojaCdParametro: ['$stateParams', 'LojaCdParametroService', function ($stateParams, service) {                        
                        return service.obterEstruturadoPorId($stateParams.id, $stateParams.tipoReabastecimento);
                    }]
                }
            });
    }
})();