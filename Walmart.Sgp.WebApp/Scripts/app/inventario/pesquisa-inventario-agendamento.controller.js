(function () {
    'use strict';


    // Configuração da controller
    angular
        .module('SGP')
        .config(PesquisaInventarioAgendamentoRoute)
        .controller('PesquisaInventarioAgendamentoController', PesquisaInventarioAgendamentoController);


    // Implementação da controller
    PesquisaInventarioAgendamentoController.$inject = ['$scope', '$q', '$timeout', '$stateParams', '$state', 'ToastService', 'ValidationService', 'InventarioService'];
    function PesquisaInventarioAgendamentoController($scope, $q, $timeout, $stateParams, $state, toast, $validation, inventarioService) {
        $validation.prepare($scope);

        $scope.filters = $stateParams.filters || {
            cdSistema: null,
            idBandeira: null,
            cdLoja: null,
            cdDepartamento: null,
            dtAgendamento: null,
            naoAgendados: false,           
        }; 

        if (!!$scope.filters && !!$scope.filters.didSearch) {
            // paging já foi persistido e carregado com a última configuração, não é necessário usar $timeout.
            search();
        }
        else {
            // Entrando diretamente na tela.
            inventarioService.obterQuantidadeLojasSemAgendamento()
            .then(function (quantidade) {
                if (quantidade > 0) {
                    toast.warning(globalization.texts.thereIsStoresWithoutInventoryScheduling);
                }
            });
        }

        $scope.data = { values: [], colunasAgendamentoVisiveis: true, today: new Date() };
        $scope.create = create;
        $scope.search = search;
        $scope.clear = clear;
        $scope.remove = remove;
        $scope.change = change;        
        $scope.exportIt = exportIt;
        $scope.marcarTodos = marcarTodos;
        $scope.estaoTodosMarcados = estaoTodosMarcados;
        $scope.isFutureDate = isFutureDate;
        $scope.dtAgendamentoParaExportacao = null;

        function isFutureDate(date)
        {            
            return moment(date).isAfter($scope.data.today);
        }

        function create() {
            $state.update({ filters: $scope.filters });
            $state.go('cadastroInventarioAgendamentoNew');
        }

        function search() {
            if (!$validation.validate($scope)) return;
        
            var searchMethod = $scope.filters.naoAgendados ? inventarioService.obterNaoAgendados : inventarioService.obterAgendamentos;
            var deferred = $q
                .when(searchMethod($scope.filters))
                .then(function (data) {
                    $scope.data.colunasAgendamentoVisiveis = !$scope.filters.naoAgendados;
                    $scope.data.values = data;
                    $scope.dtAgendamentoParaExportacao = data.length > 0 && $scope.filters.dtAgendamento ? $scope.filters.dtAgendamento : null;

                    $scope.filters.didSearch = true;
                    $validation.accept($scope);
                });
        }

        function clear() {
            $scope.filters.cdSistema = $scope.filters.idBandeira = $scope.filters.cdLoja =
                $scope.filters.cdDepartamento = $scope.filters.dtAgendamento = null;
            $scope.filters.naoAgendados = false;
            $scope.colunasAgendamentoVisiveis = true;
            $scope.data.values = [];
            $scope.dtAgendamentoParaExportacao = null;
            $scope.filters.didSearch = false;
        }

        function obterMarcados()
        {
            return $scope.data.values.where('marcado', true);
        }

        function remove() {
            var marcados = obterMarcados();
            inventarioService.removerAgendamentos(marcados)
                .then(function () {
                    toast.success(globalization.texts.inventorySchedulingSuccessfulRemoved);
                    search(1);
                });
        }

        function change() {
            var marcados = obterMarcados();
            
            $state.update({ filters: $scope.filters });

            if (marcados.length == 1) {
                $state.go('cadastroInventarioAgendamentoEdit', {
                    'id': marcados[0].id
                });
            }
            else {
                $state.go('cadastroInventarioAgendamentoManyEdit', {
                    'ids': marcados.map(function (a) { return a.id; })
                });
            }
        }

        function exportIt() {
            var bandeira = $scope.filters.bandeira;
            var loja = $scope.filters.loja;
            var departamento = $scope.filters.departamento;
            var dtAgendamento = $scope.dtAgendamentoParaExportacao;
            var naoAgendados = $scope.filters.naoAgendados;

            var deferred = $q
                .when(inventarioService.validarDataObedeceQuantidadeDiasLimiteExpurgo(dtAgendamento))
                .then(function () {
                    inventarioService.exportarRelatorioInventarioAgendamento(bandeira, loja, departamento, dtAgendamento, naoAgendados);
                });
        }

        function marcarTodos() {
            var values = $scope.data.values;

            for (var i in values) {
                var v = values[i];

                if (v != null && typeof v === 'object' && isFutureDate(v.dtAgendamento)) {
                    v.marcado = $scope.data.marcado;
                }
            }            
        }

        function estaoTodosMarcados() {
            var values = $scope.data.values;

            for (var i in values) {
                var v = values[i];

                if (v != null && typeof v === 'object' && isFutureDate(v.dtAgendamento) && !v.marcado) {
                    return false;
                }
            }

            return values.length > 0;
        }
    }

    // Configuração do estado
    PesquisaInventarioAgendamentoRoute.$inject = ['$stateProvider'];
    function PesquisaInventarioAgendamentoRoute($stateProvider) {

        $stateProvider
            .state('pesquisaInventarioAgendamento', {
                url: '/inventario/agendamento',
                params: {
                    filters: null
                },
                templateUrl: 'Scripts/app/inventario/pesquisa-inventario-agendamento.view.html',
                controller: 'PesquisaInventarioAgendamentoController'
            });
    }
})();