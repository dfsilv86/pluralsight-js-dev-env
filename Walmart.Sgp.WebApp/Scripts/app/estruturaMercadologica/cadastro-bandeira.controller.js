(function () {
    'use strict';

    // Configuração da controller
    angular
        .module('SGP')
        .config(CadastroBandeiraRoute)
        .controller('CadastroBandeiraController', CadastroBandeiraController);

    // Implementação da controller
    CadastroBandeiraController.$inject = ['$scope', '$q', '$stateParams', '$state', 'ValidationService', 'ToastService', 'StackableModalService', 'BandeiraService', 'CategoriaService', 'DepartamentoService', 'bandeira'];
    function CadastroBandeiraController($scope, $q, $stateParams, $state, $validation, toast, $modal, service, categoriaService, departamentoService, bandeira) {
        $scope.data = bandeira;
        $scope.save = save;
        $scope.back = back;
        $scope.adicionarRegiao = adicionarRegiao;
        $scope.removerRegiao = removerRegiao;
        $scope.detalharRegiao = detalharRegiao;

        $validation.prepare($scope);

        $scope.$watch('data.formato', function (newValue, oldValue) {
            if (newValue !== oldValue) {
                carregarDetalhes();
            }
        });

        function carregarDetalhes() {
            var cdSistemaSuperCenter = sgpFixedValues.tipoSistema.supercenter;
            var cdSistema = $scope.data.formato == null ? cdSistemaSuperCenter : $scope.data.formato.cdSistema;

            if (cdSistema == cdSistemaSuperCenter) {
                departamentoService.obterPorSistema(cdSistema, true)
                    .then(carregarDetalhesDisponiveis);
            }
            else {
                categoriaService.obterPorSistema(cdSistema)
                    .then(function (detalhesDisponiveis) {
                        // Seta o idDepartamento para nulo para que não seja enviado quando for categoria o BandeiraDetalhe.
                        detalhesDisponiveis.setAll('idDepartamento', null);
                        carregarDetalhesDisponiveis(detalhesDisponiveis);
                    });
            }
        }

        function carregarDetalhesDisponiveis(detalhesDisponiveis) {
            $scope.data.detalhesDisponiveis = detalhesDisponiveis;
            var detalhesSelecionados = $scope.data.detalhes;

            if (detalhesSelecionados != null) {
                for (var idx in detalhesDisponiveis) {
                    var detalheDisponivel = detalhesDisponiveis[idx];

                    if (detalheDisponivel != null && typeof (detalheDisponivel) == 'object') {
                        detalheDisponivel.selecionado = detalheDisponivel.idCategoria != null
                            ? detalhesSelecionados.containsByProperty('idCategoria', detalheDisponivel.idCategoria)
                            : detalhesSelecionados.containsByProperty('idDepartamento', detalheDisponivel.idDepartamento);
                    }
                }
            }
        }

        if (bandeira != null) {
            bandeira.blAtivoBool = bandeira.blAtivo == 'S';
            carregarDetalhes();
        }

        function adicionarRegiao() {
            var regiao = $scope.data.tempRegiao;

            removerRegiao(regiao);
            $scope.data.regioes.push({ nmRegiao: regiao.nmRegiao, distritos: [] });
            regiao.nmRegiao = '';
        }

        function removerRegiao(regiao) {
            $scope.data.regioes.removeByProperty('nmRegiao', regiao.nmRegiao);
        }

        function adicionarDistrito(regiao) {
            var distrito = $scope.data.tempDistrito;

            removerDistrito(regiao, distrito);
            regiao.distritos.push({ 'nmDistrito': distrito.nmDistrito });
            distrito.nmDistrito = '';
        }

        function save() {
            if (!$validation.validate($scope)) return;

            var tmpbandeira = {};
            angular.copy($scope.data, tmpbandeira);
            tmpbandeira.blAtivo = tmpbandeira.blAtivoBool ? 'S' : 'N';

            var detalhesSelecionados = $scope.data.detalhesDisponiveis.where('selecionado', true);

            // Adicionado o idBandeiraDetalhe nos detalhes selecionados que já estavam selecionados.
            for (var idx in detalhesSelecionados) {
                var selecionado = detalhesSelecionados[idx];
                var bandeiraDetalhe = null;

                if (selecionado.idDepartamento == null) {
                    bandeiraDetalhe = tmpbandeira.detalhes.firstOrDefault('idCategoria', selecionado.idCategoria);
                }
                else {
                    bandeiraDetalhe = tmpbandeira.detalhes.firstOrDefault('idDepartamento', selecionado.idDepartamento);
                }

                if (bandeiraDetalhe != null) {
                    selecionado.idBandeiraDetalhe = bandeiraDetalhe.idBandeiraDetalhe;
                }
            }

            tmpbandeira.detalhes = detalhesSelecionados;

            service
               .salvar(tmpbandeira)
               .then(function (bandeiraSalva) {
                   $state.go('cadastroBandeiraEdit', { id: bandeiraSalva.id }, { reload: true });
                   toast.success(globalization.texts.chainSavedWithSuccess);
               });
        }

        function detalharRegiao(regiao) {
            $modal.open({
                templateUrl: 'Scripts/app/estruturaMercadologica/cadastro-bandeira-regiao.view.html',
                controller: 'CadastroBandeiraRegiaoController',
                resolve: {
                    regiao: regiao
                }
            });
        }

        function back() {
            $state.go('pesquisaBandeira');
        }
    }

    // Configuração do estado
    CadastroBandeiraRoute.$inject = ['$stateProvider'];
    function CadastroBandeiraRoute($stateProvider) {

        $stateProvider
            .state('cadastroBandeiraNew', {
                url: '/cadastro/bandeira/new',
                templateUrl: 'Scripts/app/estruturaMercadologica/cadastro-bandeira.view.html',
                controller: 'CadastroBandeiraController',
                resolve: {
                    bandeira: function () {
                        return { isNew: true, detalhes: [], blImportarTodos: false, blAtivo: false, regioes: [], tempRegiao: { nmRegiao: '' } };
                    }
                }
            })
            .state('cadastroBandeiraEdit', {
                url: '/cadastro/bandeira/edit/:id',
                templateUrl: 'Scripts/app/estruturaMercadologica/cadastro-bandeira.view.html',
                controller: 'CadastroBandeiraController',
                params: {
                    id: null
                },
                resolve: {
                    bandeira: ['$stateParams', 'BandeiraService', function ($stateParams, service) {
                        return service.obterEstruturadoPorId($stateParams.id);
                    }]
                }
            });
    }
})();