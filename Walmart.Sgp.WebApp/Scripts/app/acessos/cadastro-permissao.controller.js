(function () {
    'use strict';

    // Configuração da controller
    angular
        .module('SGP')
        .config(CadastroPermissaoRoute)
        .controller('CadastroPermissaoController', CadastroPermissaoController);

    // Implementação da controller
    CadastroPermissaoController.$inject = ['$scope', '$q', '$stateParams', '$state', 'ValidationService', 'PagingService', 'ToastService', 'ChangeTrackerFactory', '$filter', 'PermissaoService', 'permissao'];
    function CadastroPermissaoController($scope, $q, $stateParams, $state, $validation, pagingService, toast, changeTrackerFactory, $filter, permissaoService, permissao) {

        initialize();

        function initialize() {
            $validation.prepare($scope);

            $scope.filters = {  };
            $scope.permissao = permissao;

            $scope.permissao.bandeiras.setAll('selecionado', false);
            $scope.permissao.lojas.setAll('selecionado', false);

            $scope.save = save;
            $scope.remove = remove;
            $scope.back = back;
            $scope.backWithoutChanges = backWithoutChanges;
            $scope.hasChanges = hasChanges;
            $scope.adicionarBandeira = adicionarBandeira;
            $scope.podeAdicionarBandeira = podeAdicionarBandeira;
            $scope.removerBandeira = removerBandeira;
            $scope.podeRemoverBandeira = podeRemoverBandeira;
            $scope.marcarTodasBandeiras = marcarTodasBandeiras;
            $scope.adicionarLoja = adicionarLoja;
            $scope.podeAdicionarLoja = podeAdicionarLoja;
            $scope.removerLoja = removerLoja;
            $scope.podeRemoverLoja = podeRemoverLoja;
            $scope.marcarTodasLojas = marcarTodasLojas;

            $scope.changeTrackerPermissao = changeTrackerFactory.createChangeTrackerForProperties(['usuario'], function (left, right) {
                return !!left && !!right && left.idPermissao == right.idPermissao;
            });

            $scope.$watchCollection('permissao.bandeiras', function (newValues, oldValues) {
                if (newValues !== oldValues) {
                    $scope.changed = true;
                }
            });

            $scope.$watchCollection('permissao.lojas', function (newValues, oldValues) {
                if (newValues !== oldValues) {
                    $scope.changed = true;
                }
            });
        }
        
        function save() {            
            permissaoService.salvar($scope.permissao)
               .then(function (permissaoSalva) {
                   $state.go('cadastroPermissaoEdit', { id: permissaoSalva.id }, { reload: true });
                   toast.success(globalization.texts.permissionsSavedWithSuccess);
               });
        }

        function remove() {
            permissaoService.remover($scope.permissao.id)
               .then(function () {
                   back();
                   toast.success(globalization.texts.permissionsRemovedWithSuccess);
               });
        }

        function back() {
            $state.go('pesquisaPermissao');
        }

        function adicionarBandeira() {

            var existe = $filter('filter')($scope.permissao.bandeiras, { bandeira: { 'idBandeira': $scope.filters.bandeira.idBandeira } }, true).length > 0;

            if (existe) {
                $scope.filters.idBandeira = null;
                return;
            }

            $q.when(permissaoService.validarInclusaoBandeira($scope.filters.bandeira.idBandeira))
                .then(inclusaoBandeiraValida);
        }

        function inclusaoBandeiraValida() {
            var permissaoBandeira = {
                bandeira: $scope.filters.bandeira,
                selecionado: false
            };
            
            $scope.permissao.bandeiras.push(permissaoBandeira);
            $scope.filters.idBandeira = null;
        }

        function podeAdicionarBandeira() {
            return $scope.filters.bandeira !== null;
        }

        function removerBandeira() {
            $scope.permissao.bandeiras = $filter('filter')($scope.permissao.bandeiras, { selecionado: false }, true);
        }

        function podeRemoverBandeira() {
            return $scope.permissao.bandeiras.filter(function (bandeira) {
                return bandeira.selecionado === true;
            }).length > 0;
        }

        function marcarTodasBandeiras() {
            angular.forEach($scope.permissao.bandeiras, function (bandeira) {
                bandeira.selecionado = $scope.filters.todasBandeirasSelecionado;
            });
        }

        function adicionarLoja() {

            var existe = $filter('filter')($scope.permissao.lojas, { loja: { 'idLoja': $scope.filters.loja.idLoja } }, true).length > 0;

            if (existe) {
                $scope.filters.cdLoja = null;
                return;
            }

            $q.when(permissaoService.validarInclusaoLoja($scope.filters.loja.idLoja))
                .then(inclusaoLojaValida);
        }

        function inclusaoLojaValida() {
            var permissaoLoja = {
                loja: $scope.filters.loja,
                selecionado: false
            };

            $scope.permissao.lojas.push(permissaoLoja);
            $scope.filters.cdLoja = null;
        }

        function podeAdicionarLoja() {            
            return $scope.filters.loja != null;
        }

        function removerLoja() {
            $scope.permissao.lojas = $filter('filter')($scope.permissao.lojas, { selecionado: false }, true);
        }

        function podeRemoverLoja() {
            return $scope.permissao.lojas.filter(function (lojas) {
                return lojas.selecionado === true;
            }).length > 0;
        }

        function marcarTodasLojas() {
            angular.forEach($scope.permissao.lojas, function (loja) {
                loja.selecionado = $scope.filters.todasLojasSelecionado;
            });
        }

        function backWithoutChanges() {
            if (!$scope.hasChanges()) {
                back();
            }
        }

        function hasChanges() {
            return $scope.changed || $scope.changeTrackerPermissao.hasChanges();
        }
    }

    // Configuração do estado
    CadastroPermissaoRoute.$inject = ['$stateProvider'];
    function CadastroPermissaoRoute($stateProvider) {
        $stateProvider
            .state('cadastroPermissaoNew', {
                url: '/acesso/permissao/new',
                params: {
                    filters: null,
                    paging: null
                },
                templateUrl: 'Scripts/app/acessos/cadastro-permissao.view.html',
                controller: 'CadastroPermissaoController',
                resolve: {
                    permissao: function () {
                        return {
                            usuario: {},
                            lojas: [],
                            bandeiras: [],
                            isNew: true
                        };
                    }
                }
            })
            .state('cadastroPermissaoEdit', {
                url: '/acesso/permissao/edit/:id',
                templateUrl: 'Scripts/app/acessos/cadastro-permissao.view.html',
                controller: 'CadastroPermissaoController',
                params: {
                    filters: null,
                    paging: null
                },
                resolve: {
                    permissao: ['$stateParams', 'PermissaoService', function ($stateParams, service) {
                        return service.obterPorId($stateParams.id)
                            .then(function (data) {                                
                                return data;
                            });
                    }]
                }
            });
    }
})();
