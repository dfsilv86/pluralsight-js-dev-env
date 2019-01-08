(function () {
    'use strict';
    // Configuração da controller
    function CadastroGradeSugestaoRoute($stateProvider) {

        $stateProvider
            .state('cadastroGradeSugestaoEdit', {
                url: '/reabastecimento/grade-sugestao/editar/:id',
                params: {
                    id: null
                },
                templateUrl: 'Scripts/app/reabastecimento/cadastro-grade-sugestao.view.html',
                controller: 'CadastroGradeSugestaoController',
                resolve: {
                    gradeSugestao: ['$stateParams', 'GradeSugestaoService', function ($stateParams, service) {
                        return service.obterEstruturado($stateParams.id);
                    }]
                }
            })
            .state('cadastroGradeSugestaoNew', {
                url: '/reabastecimento/grade-sugestao/novo',
                params: {
                    id: null
                },
                templateUrl: 'Scripts/app/reabastecimento/cadastro-grade-sugestao.view.html',
                controller: 'CadastroGradeSugestaoController',
                resolve: {
                    gradeSugestao: [function () {
                            return null;
                    }]
                }
            });
    }

    function CadastroGradeSugestaoController($scope, $stateParams, $state, $q, $pagingService, toast, $validation, gradeSugestaoService, gradeSugestao) {
        $validation.prepare($scope);
        $scope.data = gradeSugestao;
        $scope.complementos = {};

        var emEdicao = !!gradeSugestao;

        $scope.emEdicao = emEdicao;

        var ordenacaoPadrao = 'text asc';

        $scope.sugestoesAdicionadas = { values: null, allValues: [] };
        $scope.sugestoesAdicionadas.paging = { offset: 0, limit: 10, orderBy: ordenacaoPadrao };
        $scope.search = search;
        $scope.clear = limparEditor;

        function back() {
            $state.go('pesquisaGradeSugestao');
        }

        function limparEditor() {
            $scope.data = {
                cdSistema: null,
                idBandeira: null,
                cdLoja: null,
                loja: { cdLoja: null },
                departamento: { cdDepartamento: null }
            };
        }

        $scope.back = back;

        function atualizarIdsRelacionamento(target) {
            target.idLoja = $scope.complementos.loja.idLoja;
            target.idDepartamento = $scope.complementos.departamento.idDepartamento;
        }

        function save() {
            if (emEdicao) {
                if (!$validation.validate($scope)) return;
                atualizarIdsRelacionamento($scope.data);
                gradeSugestaoService.atualizar($scope.data).then(function (data) {
                    toast.success(globalization.texts.savedSuccessfully);
                });
            }
            else {
                if (!$scope.sugestoesAdicionadas.allValues.length) {
                    toast.warning(globalization.texts.noSuggestionsToBeSaved);
                    return;
                }

                gradeSugestaoService.salvarNovas($scope.sugestoesAdicionadas.allValues).then(function () {
                    back();
                    toast.success(globalization.texts.savedSuccessfully);
                });
            }
        }

        function search(pageNumber) {
            if ($scope.sugestoesAdicionadas.paging.orderBy === null) {
                $scope.sugestoesAdicionadas.paging.orderBy = ordenacaoPadrao;
            }

            if (!!pageNumber) {
                $scope.sugestoesAdicionadas.paging.offset = ((pageNumber) - 1) * $scope.sugestoesAdicionadas.paging.limit;
            }

            $scope.sugestoesAdicionadas.values = $pagingService.createInMemoryPagedArray(
                $scope.sugestoesAdicionadas.allValues, $scope.sugestoesAdicionadas.paging);

            if ($scope.sugestoesAdicionadas.values.currentPageNumber > 1 && $scope.sugestoesAdicionadas.values.totalCount && !$scope.sugestoesAdicionadas.values.length) {
                search($scope.sugestoesAdicionadas.values.currentPageNumber - 1);
            }
        }

        $scope.save = save;

        function remove() {
            gradeSugestaoService.remover($scope.data.id).then(function () {
                back();
                toast.success(globalization.texts.recordRemovedSuccessfully);
            });
        }

        $scope.remove = remove;

        function removeAddedItem(item) {
            var index = $scope.sugestoesAdicionadas.allValues.indexOf(item);
            if (index >= 0) {
                $scope.sugestoesAdicionadas.allValues.splice(index, 1);
                search();
            }
        }

        $scope.removeAddedItem = removeAddedItem;

        function adicionarComplementos(item) {
            item.bandeira = angular.extend({}, $scope.complementos.bandeira);
            item.loja = angular.extend({}, $scope.complementos.loja);
            item.departamento = angular.extend({}, $scope.complementos.departamento);
            item.sistema = angular.extend({}, $scope.complementos.sistema);
        }

        function verificarUnicidade(todasSugestoes, nova) {
            return $q(function (resolve, reject) {               
                var diferentes = todasSugestoes.length === 0 || todasSugestoes.every(function (s) {
                    return s.cdSistema !== nova.cdSistema ||
                        s.idBandeira !== nova.idBandeira ||
                        s.idDepartamento !== nova.idDepartamento ||
                        s.idLoja !== nova.idLoja;
                });

                if (!diferentes) {
                    reject();
                    return;
                }

                gradeSugestaoService.contarExistentes(
                    nova.cdSistema,
                    nova.idBandeira,
                    nova.idLoja,
                    nova.idDepartamento).then(function (count) {
                        if (count > 0) {
                            reject();
                        }
                        else {
                            resolve();
                        }
                    });
            });
        }

        function add() {
            if (!$validation.validate($scope)) return;
            atualizarIdsRelacionamento($scope.data);
            verificarUnicidade($scope.sugestoesAdicionadas.allValues || [], $scope.data).then(function () {
                var addedItem = {};
                angular.extend(addedItem, $scope.data);                
                limparEditor();
                adicionarComplementos(addedItem);
                $scope.sugestoesAdicionadas.allValues.push(addedItem);
                search();
            }, function () {
                toast.error(globalization.texts.suggestionGridAlreadyExists);
            });            
        }

        $scope.add = add;

        if (!emEdicao) {
            limparEditor();
        }
    }

    angular
        .module('SGP')
        .config(CadastroGradeSugestaoRoute)
        .controller('CadastroGradeSugestaoController', CadastroGradeSugestaoController);

    // Implementação da controller
    CadastroGradeSugestaoController.$inject = ['$scope', '$stateParams', '$state', '$q', 'PagingService', 'ToastService', 'ValidationService', 'GradeSugestaoService', 'gradeSugestao']; // Configuração do estado
    CadastroGradeSugestaoRoute.$inject = ['$stateProvider'];
})();