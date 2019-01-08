(function () {
    'use strict';
    // Configuração da controller
    angular
        .module('SGP')
        .config(ManutencaoAlcadaRoute)
        .controller('ManutencaoAlcadaController', ManutencaoAlcadaController);

    // Implementação da controller
    ManutencaoAlcadaController.$inject = ['$timeout', '$filter', '$q', '$scope', '$stateParams', '$state', 'ToastService', 'ValidationService', 'AlcadaService', 'AlcadaDetalheService', 'alcada', 'PagingService', 'ChangeTrackerFactory'];
    function ManutencaoAlcadaController($timeout, $filter, $q, $scope, $stateParams, $state, toast, $validation, alcadaService, alcadaDetalheService, alcada, pagingService, changeTrackerFactory) {

        var changeTracker = changeTrackerFactory.createChangeTrackerForProperties(['idRegiaoAdministrativa', 'idBandeira', 'idDepartamento', 'vlPercentualAlterado'], function (left, right) {
            return !!left && !!right && left.idAlcadaDetalhe === right.idAlcadaDetalhe;
        });

        initialize();

        function initialize() {
            $validation.prepare($scope);

            $scope.filters = { cdSistema: 1, bloquearCampos: false, esconderRemover: false, marcouTodos: false, atualizando: false };
            $scope.data = alcada;
            $scope.changedItens = [];

            $scope.back = back;
            $scope.save = save;
            $scope.search = search;
            $scope.orderBy = orderBy;
            $scope.selecionou = selecionou;
            $scope.selecionarTodos = selecionarTodos;
            $scope.inserir = inserir;
            $scope.atualizar = atualizar;
            $scope.remover = remover;
            $scope.limpar = limpar;
            $scope.applyPagingInMemory = applyPagingInMemory;
            $scope.limparEdesemarcar = limparEdesemarcar;
            $scope.camposPreenchidos = camposPreenchidos;
            $scope.marcouAlterarSugestao = marcouAlterarSugestao;

            search(1);
        }

        function marcouAlterarSugestao() {
            if ($scope.data.blAlterarSugestao) {
                $scope.data.blZerarItem = false;
            }
        }

        function camposPreenchidos() {
            return ($scope.filters.idRegiaoAdministrativa != null &&
                     $scope.filters.idBandeira != null &&
                     $scope.filters.idBandeira != 0 &&
                     $scope.filters.cdDepartamento != null &&
                     $scope.filters.vlPercentualAlteradoDetalhado != null);
        }

        function limparEdesemarcar() {
            limpar();

            $scope.filters.marcouTodos = false;
            selecionarTodos($scope.filters.marcouTodos);
        }

        function limpar() {
            $scope.filters.idAlcadaDetalhe = null;
            $scope.filters.idRegiaoAdministrativa = null;
            $scope.filters.idBandeira = null;
            $scope.filters.cdDepartamento = $scope.filters.departamento = null;
            $scope.filters.vlPercentualAlteradoDetalhado = null;
        }

        function remover() {
            var data = $scope.data;
            var detalhe = data.detalhe;

            // Remove os marcados na grid.
            detalhe.removeByProperty('selecionado', true);

            applyPagingInMemory();
            limparEdesemarcar();
        }

        function applyPagingInMemory(pageNumber) {

            var paged = $scope.data.alcadaDetalhePaginado;
            var source = $scope.data.detalhe;

            pagingService.inMemoryPaging(pageNumber, source, paged);
        }

        function inserir() {
            var detalheAlcada = {
                idAlcada: $scope.data.idAlcada,
                idRegiaoAdministrativa: $scope.filters.idRegiaoAdministrativa,
                regiaoAdministrativa: $scope.filters.regiaoAdministrativa,
                idBandeira: $scope.filters.idBandeira,
                bandeira: $scope.filters.bandeira,
                idDepartamento: $scope.filters.departamento.idDepartamento,
                departamento: $scope.filters.departamento,
                vlPercentualAlterado: $scope.filters.vlPercentualAlteradoDetalhado
            };

            var tempDetalhe = [];
            angular.forEach($scope.data.detalhe, function (item) {
                tempDetalhe.push(item);
            });

            tempDetalhe.push(detalheAlcada);

            var deferred = $q
                .when(alcadaService.validarDuplicidadeDetalhe({ Id: $scope.data.idAlcada, idAlcada: $scope.data.idAlcada, detalhe:tempDetalhe }))
                .then(function (data) {
                    if (data.satisfied == false) {
                        toast.error(data.reason);
                    } else {
                        $scope.data.detalhe.push(detalheAlcada);
                        limpar();
                        applyPagingInMemory();
                    }
                });
        }

        function atualizar() {
            angular.forEach($scope.data.detalhe, function (item) {
                if (item.selecionado === true) {
                    item.vlPercentualAlterado = $scope.filters.vlPercentualAlteradoDetalhado;

                    if ($scope.filters.idAlcadaDetalhe) {
                        item.regiaoAdministrativa = $scope.filters.regiaoAdministrativa;
                        item.idRegiaoAdministrativa = $scope.filters.idRegiaoAdministrativa;
                        item.bandeira = $scope.filters.bandeira;
                        item.idBandeira = $scope.filters.idBandeira;
                        item.departamento = $scope.filters.departamento;
                        item.idDepartamento = $scope.filters.departamento.idDepartamento;
                    }
                }
            });

            limparEdesemarcar();
        }

        function setarDetalhe(detalhe) {
            $scope.filters.idAlcadaDetalhe = detalhe.idAlcadaDetalhe;
            $scope.filters.regiaoAdministrativa = detalhe.regiaoAdministrativa;
            $scope.filters.idRegiaoAdministrativa = detalhe.idRegiaoAdministrativa;

            $scope.filters.bandeira = detalhe.bandeira;
            $scope.filters.idBandeira = detalhe.idBandeira;

            $scope.filters.departamento = detalhe.departamento;
            $scope.filters.cdDepartamento = detalhe.departamento.cdDepartamento;

            $scope.filters.vlPercentualAlteradoDetalhado = detalhe.vlPercentualAlterado;
        }

        function selecionarTodos(marcado) {
            limpar();

            angular.forEach($scope.data.alcadaDetalhePaginado, function (item) {
                item.selecionado = marcado;
            });

            setarBotoes();
        }

        function selecionou(detalhe) {

            if (!$scope.filters.bloquearCampos && detalhe.selecionado) {
                setarDetalhe(detalhe);
            } else {
                limpar();
            }

            setarBotoes();

            var todosEstaoSelecionados = true;
            angular.forEach($scope.data.alcadaDetalhePaginado, function (item) {
                if (item.selecionado !== true) {
                    todosEstaoSelecionados = false;
                }
            });

            $scope.filters.marcouTodos = todosEstaoSelecionados;
        }

        function setarBotoes() {
            var contador = 0;
            angular.forEach($scope.data.alcadaDetalhePaginado, function (item) {
                if (item.selecionado === true) {
                    contador = contador + 1;
                }
            });

            $scope.filters.bloquearCampos = contador > 0;
            $scope.filters.esconderRemover = contador > 0;
            $scope.filters.atualizando = contador > 0;
        }

        function orderBy(field) {
            if ($scope.data.alcadaDetalhePaginado != null && $scope.data.alcadaDetalhePaginado.length > 0) {
                $scope.data.alcadaDetalhePaginado.orderBy = ($scope.data.alcadaDetalhePaginado.orderBy || '').indexOf(field + ' asc') >= 0 ?
                    field + ' desc' :
                    field + ' asc';
                search();
            }
        }

        function exibe(data) {
            changeTracker.track(data);
            $scope.data.detalhe = data;

            applyPagingInMemory();

            $validation.accept($scope);
        }

        function esconde(data) {
            $scope.data.detalhe = [];
        }

        function search(pageNumber) {

            if (!$scope.data.alcadaDetalhePaginado) {
                $scope.data.alcadaDetalhePaginado = [];
                $scope.data.alcadaDetalhePaginado.offset = 0;
                $scope.data.alcadaDetalhePaginado.limit = 5;
                $scope.data.alcadaDetalhePaginado.totalCount = 0;
                $scope.data.alcadaDetalhePaginado.orderBy = 'dsRegiaoAdministrativa, dsBandeira, cdDepartamento asc';
            }
            
            if (!$scope.data.idAlcada) {
                return;
            }

            // Requisição ao serviço
            var deferred = $q
                .when(alcadaDetalheService.obterPorIdAlcada($scope.data.idAlcada, { offset: 0, limit: 999999999, orderBy: $scope.data.alcadaDetalhePaginado.orderBy }))
                .then(exibe)
                .catch(esconde);
        }

        function save() {
            if (!$validation.validate($scope)) return;

            alcadaService.salvar($scope.data).then(function (alcada) {
                alcada.alcadaDetalhePaginado = $scope.data.alcadaDetalhePaginado;
                $scope.data = alcada;

                search(1);
                toast.success(globalization.texts.savedSuccessfully);
            });
        }

        function back() {
            $state.go('pesquisaAlcada');
        }

        $scope.$watch('data.blAlterarSugestao', function (newValue) {
            if (!newValue) {
                $scope.data.blAlterarInformacaoEstoque =
                    $scope.data.blAlterarPercentual = false;
            }
        });
    }

    // Configuração do estado
    ManutencaoAlcadaRoute.$inject = ['$stateProvider'];
    function ManutencaoAlcadaRoute($stateProvider) {

        $stateProvider
            .state('manutencaoAlcada', {
                url: '/reabastecimento/alcada/{id}',
                params: {
                    paging: null
                },
                templateUrl: 'Scripts/app/gerenciamento/manutencao-alcada.view.html',
                controller: 'ManutencaoAlcadaController',
                resolve: {
                    alcada: ['$stateParams', 'AlcadaService', '$q', function ($stateParams, service) {
                        return service.obterEstruturadoPorPerfil($stateParams.id);
                    }]
                }
            });
    }
})();