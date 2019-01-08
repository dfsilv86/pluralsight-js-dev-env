(function () {
    'use strict';

    // Configuração da controller
    angular
        .module('SGP')
        .config(CadastroInventarioAgendamentoRoute)
        .controller('CadastroInventarioAgendamentoController', CadastroInventarioAgendamentoController);

    // Implementação da controller
    CadastroInventarioAgendamentoController.$inject = ['$scope', '$q', '$stateParams', '$state', 'ValidationService', 'ToastService', 'InventarioService', 'inventarioAgendamento', 'modo', 'ConfirmService'];
    function CadastroInventarioAgendamentoController($scope, $q, $stateParams, $state, $validation, toast, service, inventarioAgendamento, modo, confirm) {
        $scope.data = inventarioAgendamento;
        $scope.data.tempBandeira = {};
        $scope.modo = modo;
        $scope.confirmSave = confirmSave;
        $scope.clear = clear;
        $scope.back = back;
        $validation.prepare($scope);

        function confirmSave() {
            if (!$validation.validate($scope)) return;

            var inventarioAgendamento = {};
            angular.copy($scope.data, inventarioAgendamento);

            var confirmMessage = null;
            var saveMethod = null;

            if (modo == 'new') {
                // Inserir agendamentos.
                saveMethod = inserirAgendamentos;

                var cdLoja = inventarioAgendamento.loja.cdLoja;
                var cdDepartamento = inventarioAgendamento.departamento.cdDepartamento;
                var todasLojasMarcado = inventarioAgendamento.todasLojasMarcado;
                var todosDepartamentosMarcado = inventarioAgendamento.todosDepartamentosMarcado;

                inventarioAgendamento.loja.cdLoja = cdLoja = todasLojasMarcado ? null : cdLoja;
                inventarioAgendamento.departamento.cdDepartamento = cdDepartamento = todosDepartamentosMarcado ? null : cdDepartamento;


                if ((!todasLojasMarcado && cdLoja == null)
                  || (!todosDepartamentosMarcado && cdDepartamento == null)) {
                    toast.warning(globalization.texts.informeStoreDepartamentOrAll);
                    return;
                }

                var dsBandeira = inventarioAgendamento.tempBandeira.dsBandeira;

                if (cdLoja == null) {
                    if (cdDepartamento == null) {
                        confirmMessage = globalization.texts.confirmSaveAllStoresAndDepartments.format(dsBandeira);
                    }
                    else {
                        confirmMessage = globalization.texts.confirmSaveAllStoresAndOneDepartment.format(cdDepartamento, dsBandeira);
                    }
                }
                else if (cdDepartamento == null) {
                    confirmMessage = globalization.texts.confirmSaveOneStoreAndAllDepartments.format(cdLoja);
                }
            }
            else {
                // Atualizar agendamentos.
                saveMethod = atualizarAgendamentos;
                confirmMessage = globalization.texts.confirmUpdateInventoryScheduling;
            }

            if (confirmMessage == null) {
                saveMethod();
            }
            else {
                confirm.open(
                {
                    message: confirmMessage,
                    yes: saveMethod
                });
            }
        }

        function resultadoInsert(dados) {
            var result = resultado(dados, globalization.texts.inventorySchedulingInsertedWithSuccess, globalization.texts.inventorySchedulingNotInsertedWithError);

            if (result.invalidos == 0) {
                clear();
            }

            return result;
        }

        function resultadoUpdate(dados) {
            return resultado(dados, globalization.texts.inventorySchedulingUpdatedWithSuccess, globalization.texts.inventorySchedulingNotUpdatedWithError);
        }

        function resultado(dados, msgA, msgB) {

            var fn, mensagem = [];

            if (dados.total > 0 && dados.validos == 0) {
                fn = toast.error;
            } else if (dados.invalidos > 0) {
                fn = toast.warning;
            } else {
                fn = toast.success;
            }

            if (dados.validos > 0) {
                mensagem.push(msgA.format(dados.validos));
            }

            if (dados.invalidos > 0) {
                mensagem.push(msgB.format(dados.invalidos));
                mensagem.push(dados.mensagem);
            }            

            fn.apply(toast, [mensagem.join('\n\n')]);
            
            return $q.resolve(dados);
        }

        function inserirAgendamentos() {
            service
                .inserirAgendamentos(inventarioAgendamento)
                .then(resultadoInsert);
        }

        function atualizarAgendamentos() {
            service
                .atualizarAgendamentos(inventarioAgendamento)
                .then(resultadoUpdate);
        }

        function clear() {
            $scope.data.bandeira.cdSistema = $scope.data.bandeira.idBandeira = $scope.data.loja.cdLoja =
                $scope.data.departamento.cdDepartamento = $scope.data.dtAgendamento = null;
            $scope.data.todasLojasMarcado = $scope.data.todosDepartamentosMarcado = false;
        }

        function back() {
            $state.go('pesquisaInventarioAgendamento');
        }
    }

    // Configuração do estado
    CadastroInventarioAgendamentoRoute.$inject = ['$stateProvider'];
    function CadastroInventarioAgendamentoRoute($stateProvider) {

        $stateProvider
            .state('cadastroInventarioAgendamentoNew', {
                url: '/inventario/agendamento/new',
                params: {
                    filters: null
                },
                templateUrl: 'Scripts/app/inventario/cadastro-inventario-agendamento.view.html',
                controller: 'CadastroInventarioAgendamentoController',
                resolve: {
                    inventarioAgendamento: ['$stateParams', function ($stateParams) {
                        return {};
                    }],
                    modo: function () { return 'new'; }
                }
            })
            .state('cadastroInventarioAgendamentoEdit', {
                url: '/inventario/agendamento/edit/:id',
                templateUrl: 'Scripts/app/inventario/cadastro-inventario-agendamento.view.html',
                controller: 'CadastroInventarioAgendamentoController',
                params: {
                    filters: null
                },
                resolve: {
                    inventarioAgendamento: ['$stateParams', 'InventarioService', function ($stateParams, service) {
                        return service.obterAgendamentoEstruturadoPorId($stateParams.id)
                                .then(function (data) {
                                    data.inventarioAgendamentoIDs = [$stateParams.id];
                                    return data;
                                });

                    }],
                    modo: function () { return 'edit'; }
                }
            })
            .state('cadastroInventarioAgendamentoManyEdit', {
                url: '/inventario/agendamento/edit/many/:ids',
                templateUrl: 'Scripts/app/inventario/cadastro-inventario-agendamento.view.html',
                controller: 'CadastroInventarioAgendamentoController',
                params: {
                    filters: null
                },
                resolve: {
                    inventarioAgendamento: ['$stateParams', 'InventarioService', function ($stateParams, service) {
                        return { inventarioAgendamentoIDs: $stateParams.ids.split(',') };
                    }],
                    modo: function () { return 'editMany'; }
                }
            });
    }
})();