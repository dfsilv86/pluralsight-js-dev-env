(function () {
    'use strict';

    function CadastroDepartamentoController($scope, $q, $stateParams, $state, $validation, toast, service, departamento) {
        $scope.data = departamento;        
        $validation.prepare($scope);        

        $scope.data.booleanPerecivel = departamento.blPerecivel === 'S';

        function back() {
            $state.go('pesquisaDepartamento');
        }

        function save() {
            if (!$validation.validate($scope)) return;

            service.atualizarPerecivel($scope.data.idDepartamento, $scope.data.booleanPerecivel)
                .then(function (data) {
                    toast.success(globalization.texts.savedSuccessfully);
                });
        }

        $scope.save = save;
        $scope.back = back;
    }    

    CadastroDepartamentoController.$inject = ['$scope', '$q', '$stateParams', '$state', 'ValidationService', 'ToastService', 'DepartamentoService', 'departamento'];
    
    function CadastroDepartamentoRoute($stateProvider) {
        $stateProvider
            .state('cadastroDepartamento', {
                url: '/cadastro/departamento/{id}',
                templateUrl: 'Scripts/app/estruturaMercadologica/cadastro-departamento.view.html',
                controller: 'CadastroDepartamentoController',
                resolve: {
                    departamento: ['$stateParams', 'DepartamentoService', function ($stateParams, service) {
                        return service.obterEstruturadoPorId($stateParams.id);
                    }]
                }
            });
    }

    CadastroDepartamentoRoute.$inject = ['$stateProvider'];

    angular
        .module('SGP')
        .config(CadastroDepartamentoRoute)
        .controller('CadastroDepartamentoController', CadastroDepartamentoController);
})();