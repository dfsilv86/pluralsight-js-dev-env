(function () {
    'use strict';


    // Configuração da controller
    angular
        .module('SGP')
        .config(CadastroSugestaoPedidoRoute)
        .controller('CadastroSugestaoPedidoController', CadastroSugestaoPedidoController);


    // Implementação da controller
    CadastroSugestaoPedidoController.$inject = ['$scope', '$q', '$timeout', '$stateParams', '$state', 'UserSessionService', 'ValidationService', 'PagingService', 'ChangeTrackerFactory', 'SugestaoPedidoService', 'ToastService', 'sugestaoPedido'];
    function CadastroSugestaoPedidoController($scope, $q, $timeout, $stateParams, $state, userSession, $validation, pagingService, changeTrackerFactory, sugestaoPedidoService, toastService, sugestaoPedido) {

        $validation.prepare($scope);

        $scope.data = {
            item: sugestaoPedido,
            updated: false
        };

        if (sugestaoPedido.cdOrigemCalculo != 'S') {

            if (sugestaoPedido.cdOrigemCalculo == 'M') {
                sugestaoPedido.cdReviewDate = '';
                sugestaoPedido.dtInicioForecast = '';
                sugestaoPedido.dtFimForecast = '';
                sugestaoPedido.dsWeek = '';
                sugestaoPedido.dsInterval = '';
                sugestaoPedido.vlLeadTime = '';
                sugestaoPedido.qtVendorPackage = '';
                sugestaoPedido.vlModulo = '';
                sugestaoPedido.vlTotalPedidosAberto = '';
                sugestaoPedido.vlShelfLife = '';
                sugestaoPedido.vlPipeline = '';
                sugestaoPedido.dtProximoReviewDate = '';
                sugestaoPedido.vlForecast = '';
                sugestaoPedido.vlForecastMedio = '';
                sugestaoPedido.vlLeadTimeReal = '';
                sugestaoPedido.vlEstoqueSeguranca = '';
                sugestaoPedido.vlEstoqueSegurancaQtd = '';
                sugestaoPedido.vlPackSugerido1 = '';
                sugestaoPedido.vlQtdDiasEstoque = '';
                sugestaoPedido.vlPackSugerido1 = '';
                sugestaoPedido.vlEstoqueOriginal = '';
                sugestaoPedido.vlPackSugerido1 = '';
                sugestaoPedido.vlSugestaoPedido = '';
            } 
        }
        
        var changeTracker = changeTrackerFactory.createChangeTrackerForProperties(
           ['vlEstoque', 'qtdPackCompra'],
           function (left, right) {
               return !!left && !!right && left.idSugestaoPedido == right.idSugestaoPedido;
           });

        changeTracker.track(sugestaoPedido);

        $scope.$on('$destroy', function () {
            changeTracker.reset();
            changeTracker = null;
        });

        $scope.podeRecalcular = podeRecalcular;
        $scope.save = save;
        $scope.recalcular = recalcular;
        $scope.canSave = canSave;
        $scope.logs = logs;
        $scope.back = back;
        $scope.quantidadeAlterada = quantidadeAlterada;

        function back() {
            $state.go('pesquisaSugestaoPedido', { id: $stateParams.id, updated: $scope.data.updated });
        }      
                
        function save() {            
            var deferred = $q
                .when(sugestaoPedidoService.alterarSugestao($scope.data.item))
                .then(function (result) {
                    if (result.sucesso > 0) {
                        toastService.success(result.mensagem);
                        changeTracker.commitAll();
                        $scope.data.updated = true;
                    } else {
                        toastService.warning(result.mensagem);
                    }                    
                });
        }

        function canSave() {
            return moment().isSame($scope.data.item.dtPedido, 'day') && changeTracker.hasChanges() && !$scope.data.item.blReturnSheet;
        }

        function recalcular() {
            sugestaoPedidoService.recalcular($scope.data.item).
            then(function (sugestaoPedido) {
                // Importante pegar aqui apenas o valor que se espera que mude, por causa do changetracker.
                $scope.data.item.qtdPackCompra = sugestaoPedido.qtdPackCompra;
                $scope.date.item.qtdPackCompraAlterado = false;
            });
        }

        function podeRecalcular() {
            var item = changeTracker.getChangedItems()[0];

            return item !== undefined && item.vlEstoque != item.original_vlEstoque;
        }

        function logs() {
            $state.go('pesquisaSugestaoPedidoLog', { id: $stateParams.id });
        }

        function quantidadeAlterada() {
            $scope.data.item.qtdPackCompraAlterado = true;
        }
    }

    // Configuração do estado
    CadastroSugestaoPedidoRoute.$inject = ['$stateProvider'];
    function CadastroSugestaoPedidoRoute($stateProvider) {

        $stateProvider
            .state('cadastroSugestaoPedido', {
                url: '/reabastecimento/sugestao-pedido/:id',
                templateUrl: 'Scripts/app/reabastecimento/cadastro-sugestao-pedido.view.html',
                controller: 'CadastroSugestaoPedidoController',
                params: {
                    id: null
                },
                resolve: {
                    sugestaoPedido: [ '$stateParams', 'SugestaoPedidoService', function ($stateParams, service) {
                        return service.obterEstruturado($stateParams.id);
                    }]
                }
            });
    }
})();