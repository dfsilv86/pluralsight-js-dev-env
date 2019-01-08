(function () {
    'use strict';

    function PesquisaInventarioGeracaoController($scope, $q, $timeout, $stateParams, $state, pagingService, toast, $validation, inventarioService, backgroundRequestFactory) {

        $validation.prepare($scope);

        $scope.carregandoCustos = false;

        $scope.custoTotal = backgroundRequestFactory(inventarioService.obterCustoTotalPorFiltro);

        $scope.tipoSistema = sgpFixedValues.tipoSistema;

        $scope.sistemas = {
            supercenter: sgpFixedValues.tipoSistema.supercenter,
            samsClub: sgpFixedValues.tipoSistema.samsClub
        };
        $scope.zero = { cdDepartamento: null };

        $scope.filters = $stateParams.filters || {
            cdSistema: null,
            idBandeira: null,
            cdLoja: null,
            cdDepartamento: null,
            departamento: null,
            categoria: null,
            cdCategoria: null,
            dhInventario: null,
            stInventario: null
        };

        $scope.data = { values: null };
        $scope.data.paging = $stateParams.paging || { offset: 0, limit: 10, orderBy: 'idInventario' };

        $scope.$watch('filters.categoria', function (nv, ov) {
            if (nv == null) {
                $scope.zero.cdDepartamento = null;
            }
        });

        function create() {
            // TODO: create.
        }

        function criarFiltro() {
            return {
                cdSistema: $scope.filters.cdSistema,
                idBandeira: $scope.filters.idBandeira,
                idLoja: $scope.filters.loja ?
                        $scope.filters.loja.idLoja :
                        null,
                stInventario: $scope.filters.stInventario || null,
                dhInventario: $scope.filters.dhInventario,
                idDepartamento: $scope.filters.departamento ?
                        $scope.filters.departamento.idDepartamento :
                        null,
                idCategoria: $scope.filters.categoria ?
                        $scope.filters.categoria.idCategoria :
                        null
            };
        }


        function search(pageNumber, getTotal) {
            if (!$validation.validate($scope)) {
                return;
            }

            pagingService.calculateOffset($scope.data.paging, pageNumber);

            var filtro = criarFiltro();

            inventarioService.obterSumarizadoPorFiltro(filtro, $scope.data.paging)
                .then(function (data) {
                    $scope.data.values = data;
                    $scope.filters.didSearch = true;
                    $validation.accept($scope);
                    if (!!getTotal) {
                        $scope.custoTotal.perform(filtro);
                    }
                }, function () {
                    $scope.custoTotal.cancel();
                });
        }

        function clear() {
            $scope.filters.cdSistema = $scope.filters.idBandeira =
                $scope.filters.cdLoja = $scope.filters.cdDepartamento =
                $scope.filters.dhInventario = $scope.filters.stInventario = null;

            $scope.custoTotal.cancel();

            $scope.data.values = null;
            $scope.carregandoCustos = false;
            $scope.filters.didSearch = false;
        }

        function detail(item) {
            $state.update({
                filters: $scope.filters,
                paging: $scope.data.paging
            });

            $state.go('detalheInventario', {
                'id': item.id
            });
        }

        function print() {
            var dhInventario = $scope.filters.dhInventario;

            $q.when(inventarioService.validarDataObedeceQuantidadeDiasLimiteExpurgo(dhInventario))
              .then(function () {
                  $q.when(inventarioService.validarFiltrosRelatorioItensPorInventario($scope.filters.cdLoja, dhInventario))
                    .then(function () {
                        var loja = $scope.filters.loja.nmLoja;
                        var idLoja = $scope.filters.loja.idLoja;
                        var cdLoja = $scope.filters.loja.cdLoja;
                        var dtInventario = dhInventario;
                        var stInventario = $scope.filters.stInventario == "" ? -1 : $scope.filters.stInventario;

                        inventarioService.exportarRelatorioItensPorInventario(loja, idLoja, cdLoja, stInventario, dtInventario);
                    });
              });
        }

        $scope.search = search;
        $scope.clear = clear;
        $scope.detail = detail;
        $scope.print = print;

        if (!!$scope.filters && !!$scope.filters.didSearch) {
            // paging já foi persistido e carregado com a última configuração, não é necessário usar $timeout.
            search(null, true);
        }
    }

    // Implementação da controller
    PesquisaInventarioGeracaoController.$inject = ['$scope', '$q', '$timeout',
        '$stateParams', '$state', 'PagingService', 'ToastService',
        'ValidationService', 'InventarioService', 'BackgroundRequestFactory'];

    // Configuração do estado
    PesquisaInventarioGeracaoRoute.$inject = ['$stateProvider'];
    function PesquisaInventarioGeracaoRoute($stateProvider) {
        $stateProvider
            .state('pesquisaInventarioGeracao', {
                url: '/inventario/geracao',
                params: {
                    filters: null,
                    paging: null
                },
                templateUrl: 'Scripts/app/inventario/pesquisa-inventario-geracao.view.html',
                controller: 'PesquisaInventarioGeracaoController'
            });
    }

    // Configuração da controller
    angular
        .module('SGP')
        .config(PesquisaInventarioGeracaoRoute)
        .controller('PesquisaInventarioGeracaoController', PesquisaInventarioGeracaoController);
})();