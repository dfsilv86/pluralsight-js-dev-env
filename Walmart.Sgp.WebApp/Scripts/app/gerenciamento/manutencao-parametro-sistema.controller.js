(function () {
    'use strict';
    // Configuração da controller
    angular
        .module('SGP')
        .config(ManutencaoParametroSistemaRoute)
        .controller('ManutencaoParametroSistemaController', ManutencaoParametroSistemaController);

    // Implementação da controller
    ManutencaoParametroSistemaController.$inject = ['$scope', '$stateParams', 'ToastService', 'ValidationService', 'ParametroService', 'parametro', 'UserSessionService'];
    function ManutencaoParametroSistemaController($scope, $stateParams, toast, $validation, parametroService, parametro, userSessionService) {
        $validation.prepare($scope);

        $scope.data = parametro;
        $scope.save = save;

        resetState();

        $scope.$watch('data.tpArquivoInventario', function (newValue) {
            if (newValue == 1) {
                $scope.data.dsServidorSmartDiretorio = '/usr/tmp';
            }
            else {
                $scope.data.dsServidorSmartDiretorio = '/u/spool/30';
            }
        });

        function save() {
            if (!$validation.validate($scope)) return;

            parametroService.salvar($scope.data).then(function (parametro) {
                $scope.data = parametro;

                resetState();

                toast.success(globalization.texts.savedSuccessfully);
            });
        }

        function resetState() {
            $scope.data.podeAtualizarUsuarioAdministrador = ($scope.data.usuarioAdministrador || {}).id == userSessionService.getCurrentUser().id;

            $scope.data.opcoesTipoArquivoInventario = [
                { tpArquivoInventario: 1, dsTipoArquivoInventario: globalization.texts.tipoFormatoArquivoInventarioFixedValue1.toUpperCase() },
                { tpArquivoInventario: 2, dsTipoArquivoInventario: globalization.texts.tipoFormatoArquivoInventarioFixedValue2.toUpperCase() }
            ];
        }
    }

    // Configuração do estado
    ManutencaoParametroSistemaRoute.$inject = ['$stateProvider'];
    function ManutencaoParametroSistemaRoute($stateProvider) {

        $stateProvider
            .state('manutencaoParametroSistema', {
                url: '/gerenciamento/parametroSistema',
                templateUrl: 'Scripts/app/gerenciamento/manutencao-parametro-sistema.view.html',
                controller: 'ManutencaoParametroSistemaController',
                resolve: {
                    parametro: ['$stateParams', 'ParametroService', function ($stateParams, service) {
                        return service.obterEstruturado();
                    }]
                }
            });
    }
})();