(function () {
    'use strict';


    // Configuração da controller
    angular
        .module('SGP')
        .config(ImportacaoAutomaticaRoute)
        .controller('ImportacaoAutomaticaController', ImportacaoAutomaticaController);


    // Implementação da controller
    ImportacaoAutomaticaController.$inject = ['$scope', '$stateParams', '$state', 'ValidationService', 'ToastService', 'InventarioService', '$q', 'UserSessionService', 'resultado'];
    function ImportacaoAutomaticaController($scope, $stateParams, $state, $validation, toastService, inventarioService, $q, userSession, resultado) {

        $validation.prepare($scope);

        $scope.data = { cdSistema: null, idBandeira: null, cdLoja: null, cdDepartamento: null, dataInventario: null };
        $scope.temp = { loja: null, bandeira: null, departamento: null };

        $scope.importar = importar;

        if (!!resultado) {
            $q.when(resultado).then(sucessoCompartilhado).catch(erroCompartilhado);
            $state.go('ImportacaoAutomatica', null, { reset: true });
        }

        function sucessoCompartilhado(result) {
            if (!result) return result;
            if (result.sucesso) {
                toastService.success(result.mensagem);
            } else {
                toastService.warning(result.mensagem);
            }
        }

        function erroCompartilhado(result) {
            if (!result) return result;
            if (result.mensagem) {
                toastService.error(result.mensagem);
            }
        }

        function importar() {
            if (!$validation.validate($scope)) return;

            inventarioService
                .importarAutomatico($scope.data.cdSistema, $scope.data.idBandeira, ($scope.temp.loja || {}).idLoja, ($scope.temp.departamento || {}).idDepartamento, $scope.data.cdDepartamento)
                .then(sucessoCompartilhado)
                .catch(erroCompartilhado);
        }

        function clear() {
            $scope.data = {
                idBandeira: null,
                cdLoja: null,
                dataInventario: null
            };
        }        
        
        clear();

        $scope.$watchGroup(['temp.loja', 'temp.departamento'], function (newValues, oldValues) {
            var loja = $scope.temp.loja;
            var departamento = $scope.temp.departamento;

            if (loja != null) {
                if (newValues[0] != null) {
                    inventarioService.obterDataProximoInventarioAberto(loja.id, $scope.data.cdSistema, ($scope.temp.departamento || {}).idDepartamento)
                        .then(function (data) {
                            $scope.data.dataInventario = data;
                        });

                    return;
                }
            }
            
            $scope.data.dataInventario = null;
        });
    }

    // Configuração do estado
    ImportacaoAutomaticaRoute.$inject = ['$stateProvider'];
    function ImportacaoAutomaticaRoute($stateProvider) {

        $stateProvider
            .state('ImportacaoAutomatica', {
                url: '/inventario/importacao/automatica',
                templateUrl: 'Scripts/app/inventario/importacao-automatica.view.html',
                controller: 'ImportacaoAutomaticaController',
                resolve: {
                    resultado: angular.noop
                }
            })
            .state('ImportarAutomaticoResults', {
                url: '/inventario/importacao/automatica/resultado/:ticket',
                templateUrl: 'Scripts/app/inventario/importacao-automatica.view.html',
                controller: 'ImportacaoAutomaticaController',
                resolve: {
                    resultado: ['ProcessingService', '$stateParams', function (processingService, $stateParams) {
                        return processingService.getTicketResults($stateParams.ticket);
                    }]
                }
            });
    }
})();