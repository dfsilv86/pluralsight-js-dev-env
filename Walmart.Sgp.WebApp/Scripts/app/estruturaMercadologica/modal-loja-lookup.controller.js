(function () {
    'use strict';

    angular
		.module('SGP')
		.controller('ModalLojaLookupController', ModalLojaLookupController)
        .config(ManutencaoLojaRoute);

    ModalLojaLookupController.$inject = ['$scope', '$q', '$uibModalInstance', 'ValidationService', 'UserSessionService', 'PagingService', 'LojaService', 'sistema', 'bandeira', '$state', '$stateParams', 'loja'];

    function ModalLojaLookupController($scope, $q, $uibModalInstance, $validation, userSession, pagingService, lojaService, sistema, bandeira, $state, $stateParams, loja) {

        $validation.prepare($scope);

        $scope.filters = $stateParams.filters || { cdSistema: sistema, idBandeira: bandeira, cdLoja: loja, nmLoja: null };
        $scope.data = { values: null };
        $scope.data.paging = $stateParams.paging || { offset: 0, limit: 10, orderBy: null };

        $scope.search = search;
        $scope.clear = clear;
        $scope.select = select;
        $scope.detail = detail;

        if (!!$scope.filters && !!$scope.filters.didSearch) {
            // paging já foi persistido e carregado com a última configuração, não é necessário usar $timeout.
            search();
        }

        function search(pageNumber) {

            if (!$validation.validate($scope)) return;

            pagingService.calculateOffset($scope.data.paging, pageNumber);

            var idUsuario = userSession.getCurrentUser().id;
            var idBandeira = $scope.filters.idBandeira;
            var cdSistema = $scope.filters.cdSistema;
            var cdLoja = $scope.filters.cdLoja;
            var nmLoja = $scope.filters.nmLoja;

            var deferred = $q
                .when(lojaService.pesquisar(idUsuario, cdSistema, idBandeira, cdLoja, nmLoja, $scope.data.paging))
                .then(applyValue);
        }

        function applyValue(data) {
            $scope.data.values = data;

            pagingService.acceptPagingResults($scope.data.paging, data);

            $scope.filters.didSearch = true;
            $validation.accept($scope);
        }

        function clear() {
            $scope.filters.cdSistema = null;
            $scope.filters.idBandeira = null;
            $scope.filters.cdLoja = null;
            $scope.filters.nmLoja = null;
            $scope.data.values = [];
            $scope.filters.didSearch = false;
        }

        function select(item) {
            if ((item || null) === null) {
                $uibModalInstance.dismiss();
            } else {
                $uibModalInstance.close(item);
            }
        }

        function detail(item) {
            // TODO: voltar
            $state.update({ filters: $scope.filters, paging: $scope.data.paging });
            $state.go("manutencaoLojaEdit", { id: item.idLoja });
        }
    }

    // Configuração do estado
    ManutencaoLojaRoute.$inject = ['$stateProvider'];
    function ManutencaoLojaRoute($stateProvider) {

        $stateProvider
            .state('manutencaoLoja', {
                url: '/gerenciamento/loja',
                params: {
                    paging: null,
                    filters: null
                },
                templateUrl: 'Scripts/app/estruturaMercadologica/pesquisa-loja.view.html',
                controller: 'ModalLojaLookupController',
                resolve: {
                    '$uibModalInstance': function () { return null; },
                    sistema: function () { return null; },
                    bandeira: function () { return null; },
                    loja: function () { return null; }
                }
            });
    }
})();