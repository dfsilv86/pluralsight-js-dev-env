(function () {
    'use strict';

    angular
        .module('SGP')
        .config(CadMultiSourcingRoute)
        .controller('CadMultiSourcingController', CadMultiSourcingController);

    CadMultiSourcingController.$inject = ['$scope', '$q', 'ItemDetalheService', '$state', '$stateParams', 'UserSessionService', 'PagingService', 'cdCD', 'cdSistema', 'cdDepartamento', 'itemSaida', 'MultisourcingService', 'ToastService'];

    function CadMultiSourcingController($scope, $q, itemDetalheService, $state, $stateParams, userSession, pagingService, cdCD, cdSistema, cdDepartamento, itemSaida, multisourcingService, toastService) {

        $scope.filters = { cdOldNumber: itemSaida.cdItem, cdSistema: cdSistema, cdDepartamento: cdDepartamento, cdCD: cdCD, dsItem: itemSaida.dsItem };
        $scope.data = { values: null };
        $scope.data.paging = { offset: 0, limit: 50, orderBy: null };

        $scope.search = search;
        $scope.back = back;
        $scope.save = save;
        $scope.remove = remove;
        $scope.orderBy = orderBy;
        $scope.recalculate = recalculate;
        $scope.podeRemoverMS = podeRemoverMS;

        search(1);

        function podeRemoverMS() {
            return $scope.totalPercent > 0;
        }

        function recalculate() {

            var total = 0;

            angular.forEach($scope.data.values, function (value, key) {
                total = total + value.vlPercentual;
            });

            $scope.totalPercent = total;
        }

        function remove() {

            var cdItemSaida = $scope.filters.cdOldNumber;
            var cdCD = $scope.filters.cdCD;

            var deferred = $q
                .when(multisourcingService.deletarMultisourcing(cdItemSaida, cdCD))
                .then(function () { 
                    back(); 
                    toastService.success(globalization.texts.recordRemovedSuccessfully);
                });
        }

        function save() {

            var itens = $scope.data.values;

            if (!validate(itens)) {
                return;
            }

            var deferred = $q
                .when(multisourcingService.inserirMultisourcing(itens, userSession.getCurrentUser().id))
                .then(function () {
                    back(true);
                    toastService.success(globalization.texts.savedSuccessfully);
                });
        }

        function validate(itens) {

            var contadorPorcentagem = 0;
            var multiplosDeCinco = true;
            var temCemPorcentoEmUmItem = false;

            itens.some(function (value, index, _itens) {
                if (value.vlPercentual % 5 != 0) {
                    multiplosDeCinco = false;
                }

                if (value.vlPercentual == 100) {
                    temCemPorcentoEmUmItem = true;
                }

                contadorPorcentagem = contadorPorcentagem + value.vlPercentual;
            });

            if (temCemPorcentoEmUmItem) {
                toastService.warning(globalization.texts.notAllowedToHaveOneItemHundredPercent);
                return false;
            }

            if (multiplosDeCinco === false) {
                toastService.warning(globalization.texts.multipleFivePercentuals);
                return false;
            }

            if (contadorPorcentagem != 100) {
                toastService.warning(globalization.texts.incompletePercentageListOfItemDetalheCD);
                return false;
            }

            return true;
        }

        function back() {
            $state.go('multiSourcing');
        }

        function orderBy(field) {
            pagingService.toggleSorting($scope.data.paging, field);
            search();
        }

        function search(pageNumber) {

            escondeConsulta();
            pagingService.calculateOffset($scope.data.paging, pageNumber);

            var cdItem = $scope.filters.cdOldNumber || '';
            var cdCD = $scope.filters.cdCD || '';
            var cdSistema = $scope.filters.cdSistema || 1;

            var deferred = $q
                .when(itemDetalheService.pesquisarItensEntradaPorSaidaCD(cdItem, cdCD, cdSistema, $scope.data.paging))
                .then(exibeConsulta)
                .catch(escondeConsulta);

        }

        function exibeConsulta(data) {
            $scope.data.values = data;
            recalculate();

            angular.forEach($scope.data.values, function (value, key) {
                if (value.vlPercentual == null) {
                    value.vlPercentual = 0;
                }
            });

            return data;
        }

        function escondeConsulta() {
            $scope.data.values = null;
        }
    }

    CadMultiSourcingRoute.$inject = ['$stateProvider'];
    function CadMultiSourcingRoute($stateProvider) {

        $stateProvider
            .state('cadMultiSourcingEdit', {
                url: '/multiSourcing/cadMultiSourcing/edit/:cdSistema/:cdCD/:cdItem/:cdDepartamento',
                templateUrl: 'Scripts/app/reabastecimento/multisourcing/cadastro-multisourcing.view.html',
                controller: 'CadMultiSourcingController',
                resolve: {
                    itemSaida: ['$stateParams', 'ItemDetalheService', function ($stateParams, itemDetalheService) {
                        return itemDetalheService.obterPorOldNumberESistema($stateParams.cdItem, $stateParams.cdSistema);
                    }],
                    cdSistema: ['$stateParams', function ($stateParams) {
                        return $stateParams.cdSistema;
                    }],
                    cdCD: ['$stateParams', function ($stateParams) {
                        return $stateParams.cdCD;
                    }],
                    cdDepartamento: ['$stateParams', function ($stateParams) {
                        return $stateParams.cdDepartamento;
                    }]
                }
            });
    }
})();