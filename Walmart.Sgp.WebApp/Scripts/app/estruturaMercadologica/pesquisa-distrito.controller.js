(function () {
    'use strict';

    // TODO: isso não deveria estar aqui. reaproveitar a controller dentro da modal de pesquisa (ou fazer a modal de pesquisa utilizar esta aqui)

    PesquisaDistritoController.$inject = ['$scope', '$q', 'StackableModalService', '$timeout', '$stateParams', '$state', 'ValidationService', 'PagingService', 'DistritoService', 'ToastService'];

    function PesquisaDistritoController($scope, $q, $modal, $timeout, $stateParams, $state, $validation, pagingService, distritoService, toastService) {

        initialize();

        function initialize() {

            $validation.prepare($scope);

            $scope.search = search;
            $scope.clear = clear;
            $scope.orderBy = orderBy;
            $scope.edit = edit;

            $scope.filters = $stateParams.filters || {};

            $scope.data = { values: null };
            $scope.data.paging = $stateParams.paging || { offset: 0, limit: 10, orderBy: 'dsBandeira' };
        }

        function edit(item) {

            var theModal = $modal.open({
                templateUrl: 'Scripts/app/estruturaMercadologica/modal-distrito.view.html',
                controller: 'ModalDistritoController',
                resolve: {
                    distrito: item
                }
            });

            theModal.then(function (retorno) {
                if (retorno) {
                    toastService.success(globalization.texts.districtSavedSuccessfully);
                }
                search();
            });
        }

        function esconderRegistros() {
            $scope.data.values = [];
        }

        function exibirRegistros(data) {
            $scope.data.values = data;
            pagingService.acceptPagingResults($scope.data.paging, data);
        }

        function search(pageNumber) {
            if (!$validation.validate($scope)) return;

            pagingService.calculateOffset($scope.data.paging, pageNumber);

            var cdSistema = $scope.filters.cdSistema;
            var idBandeira = $scope.filters.idBandeira;
            var idRegiao = $scope.filters.idRegiao;
            var idDistrito = $scope.filters.idDistrito;

            distritoService.pesquisar(cdSistema, idBandeira, idRegiao, idDistrito, $scope.data.paging)
                .then(exibirRegistros)
                .catch(esconderRegistros);
        }

        function clear() {
            $scope.filters.cdSistema = null;
            $scope.filters.idBandeira = null;
            $scope.filters.idRegiao = null;
            $scope.filters.idDistrito = null;

            esconderRegistros();
        }

        function orderBy(field) {
            pagingService.toggleSorting($scope.data.paging, field);
            search();
        }

        if ($stateParams.paging) {
            search();
        }
    }

    function PesquisaDistritoRoute($stateProvider) {

        $stateProvider
            .state('pesquisaDistrito', {
                url: '/cadastro/distrito',
                params: {
                    filters: null,
                    paging: null
                },
                templateUrl: 'Scripts/app/estruturaMercadologica/pesquisa-distrito.view.html',
                controller: 'PesquisaDistritoController'
            });
    }

    PesquisaDistritoRoute.$inject = ['$stateProvider'];

    angular
        .module('SGP')
        .config(PesquisaDistritoRoute)
        .controller('PesquisaDistritoController', PesquisaDistritoController);
})();