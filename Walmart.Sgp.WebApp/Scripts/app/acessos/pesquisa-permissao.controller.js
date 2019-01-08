(function () {
    'use strict';

    // Configuração da controller
    angular
        .module('SGP')
        .config(PesquisaPermissaoRoute)
        .controller('PesquisaPermissaoController', PesquisaPermissaoController);


    // Implementação da controller
    PesquisaPermissaoController.$inject = ['$scope', '$q', '$stateParams', '$state', 'ValidationService', 'PagingService', 'PermissaoService', 'ToastService'];
    function PesquisaPermissaoController($scope, $q, $stateParams, $state, $validation, pagingService, permissaoService, toastService) {

        initialize();

        function initialize() {
            $validation.prepare($scope);

            $scope.zero = { idBandeira: null };
            $scope.filters = $stateParams.filters || { cdSistema: 1 };
            $scope.data = { values: null };
            $scope.data.paging = $stateParams.paging || { offset: 0, limit: 10, orderBy: 'username' };

            $scope.create = create;
            $scope.search = search;
            $scope.clear = clear;
            $scope.orderBy = orderBy;
            $scope.formatLoja = formatLoja;
            $scope.detail = detail;

            if (!!$scope.filters && !!$scope.filters.didSearch) {
                // paging já foi persistido e carregado com a última configuração, não é necessário usar $timeout.
                search();
            }
        }

        $scope.$watch('filters.loja', function (oldV, newV) {
            if (oldV != newV) {
                $scope.zero.idBandeira = null; // a loja em tela está desconectada da dropdown de bandeira, e não deve ter filtro de bandeira na lookup de loja.
            }
        });

        function clear() {
            $scope.filters.userName =
                $scope.filters.cdUsuario =
                $scope.filters.idBandeira =
                $scope.filters.cdLoja = null;

            $scope.data.paging.offset = 0;
            $scope.data.values = [];
            $scope.filters.didSearch = false;
        }

        function exibe(data) {
            $scope.data.values = data;
            pagingService.acceptPagingResults($scope.data.paging, data);

            $scope.filters.didSearch = true;
            $validation.accept($scope);
        }

        function esconde() {
            $scope.data.values = [];
            $scope.filters.didSearch = false;
        }

        function search(pageNumber) {
            if (!$validation.validate($scope)) return;

            pagingService.calculateOffset($scope.data.paging, pageNumber);

            var idUsuario = ($scope.filters.user || {}).id || '';
            var idBandeira = $scope.filters.idBandeira || '';
            var idLoja = ($scope.filters.loja || {}).idLoja || '';

            $q.when(permissaoService.pesquisarComFilhos(idUsuario, idBandeira, idLoja, $scope.data.paging))
                .then(exibe)
                .catch(esconde);
        }

        function create() {
            $state.update({ filters: $scope.filters, paging: $scope.data.paging });
            $state.go('cadastroPermissaoNew');
        }

        function orderBy(field) {
            pagingService.toggleSorting($scope.data.paging, field);
            search();
        }

        function formatLoja(permissao) {
            // TODO: transformar em filter.
            if (permissao.lojas[0].loja.bandeira === null) {
                return permissao.lojas[0].loja.cdLoja + ' - ' + permissao.lojas[0].loja.nmLoja;
            }
            else {
                return permissao.lojas[0].loja.cdLoja + ' - ' + permissao.lojas[0].loja.nmLoja + ' (' + permissao.lojas[0].loja.bandeira.dsBandeira + ')';
            }
        }

        function detail(item) {
            permissaoService.possuiPermissaoManutencao()
                .then(function (possuiPermissao) {
                    if (possuiPermissao) {
                        $state.update({ filters: $scope.filters, paging: $scope.data.paging });
                        $state.go('cadastroPermissaoEdit', { id: item.idPermissao });
                    }
                    else {
                        toastService.warning(globalization.texts.userDoesNotHavePermission);
                    }
                });
        }
    }

    // Configuração do estado
    PesquisaPermissaoRoute.$inject = ['$stateProvider'];
    function PesquisaPermissaoRoute($stateProvider) {

        $stateProvider
            .state('pesquisaPermissao', {
                url: '/acesso/permissao',
                params: {
                    filters: null,
                    paging: null
                },
                templateUrl: 'Scripts/app/acessos/pesquisa-permissao.view.html',
                controller: 'PesquisaPermissaoController'
            });
    }
})();