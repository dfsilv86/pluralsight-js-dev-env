(function () {
    'use strict';

    angular
          .module('SGP')
          .config(ConsultaReturnSheetLojaRoute)
          .controller('ConsultaReturnSheetLojaController', ConsultaReturnSheetLojaController);


    ConsultaReturnSheetLojaController.$inject = ['$scope', '$q', 'StackableModalService', 'SugestaoReturnSheetService', 'ToastService', 'UserSessionService', 'ChangeTrackerFactory', 'ValidationService', 'PagingService'];
    function ConsultaReturnSheetLojaController($scope, $q, $modal, sugestaoReturnSheetService, toastService, userSessionService, changeTrackerFactory, $validation, pagingService) {

        var changeTracker = changeTrackerFactory.createChangeTrackerForProperties(['qtdLoja'], function (left, right) {
            return !!left && !!right && left.idSugestaoReturnSheet == right.idSugestaoReturnSheet;
        });

        initialize();

        function initialize() {

            $validation.prepare($scope);

            $scope.filters = { dtSolicitacao: new Date(), evento: null, vendor: null, cdDepartamento: null, cdOldNumber: null, cdLoja: null, cdSistema: 1 };
            $scope.data = { values: null };
            $scope.data.paging = { offset: 0, limit: 50, orderBy: null };
            $scope.search = search;
            $scope.clear = clear;
            $scope.orderBy = orderBy;
            $scope.save = save;
            $scope.hasChanges = hasChanges;
            $scope.discardChanges = discardChanges;
            $scope.temp = {};
        }

        function searchByButton(pgNum) {
            changeTracker.reset();
            search(pgNum);
        }

        function orderBy(field) {
            pagingService.toggleSorting($scope.data.paging, field);
            search();
        }

        function hasChanges() {
            return changeTracker.hasChanges();
        }

        function discardChanges() {
            changeTracker.undoAll();
        }

        function save() {

            var sugestoes = changeTracker.getChangedItems();

            var deferred = $q
                .when(sugestaoReturnSheetService.salvarLoja(sugestoes))
                .then(salvou);
        }

        function salvou(data) {
            toastService.success(globalization.texts.returnSheetSugestionSavedSuccessfully);
            changeTracker.reset();
            search();
        }

        function search(pageNumber) {

            if (!$validation.validate($scope)) return;
            
            escondeConsulta();

            pagingService.calculateOffset($scope.data.paging, pageNumber);

            var dataSolicitacao = $scope.filters.dtSolicitacao;
            var idDepartamento = ($scope.temp.departamento || {}).idDepartamento || '';
            var idLoja = ($scope.temp.loja || {}).idLoja || '';
            var evento = $scope.filters.evento || '';
            var vendor9D = $scope.filters.vendor;
            var idItemDetalhe = ($scope.filters.item || {}).idItemDetalhe || '';

            var deferred = $q
                  .when(sugestaoReturnSheetService.consultaReturnSheetLoja(idDepartamento, idLoja, dataSolicitacao, evento, vendor9D, idItemDetalhe, $scope.data.paging))
                  .then(exibeConsulta)
                  .catch(escondeConsulta);
        }

        function exibeConsulta(data) {
            changeTracker.track(data);
            $scope.data.values = data;
            $validation.accept($scope);
            return data;
        }

        function escondeConsulta() {
            $scope.data.values = null;
        }

        function clear() {
            $scope.filters.dtSolicitacao = new Date();
            $scope.filters.evento = $scope.filters.vendor = $scope.filters.cdDepartamento = $scope.filters.cdOldNumber = $scope.filters.cdLoja = null;
            $scope.filters.cdSistema = 1;
            $scope.data.values = null;
            $scope.data.paging.offset = 0;

            discardChanges();
        }

        $scope.$on('$destroy', function () {
            discardChanges();
            changeTracker.reset();
            changeTracker = null;
        });
    }

    ConsultaReturnSheetLojaRoute.$inject = ['$stateProvider'];
    function ConsultaReturnSheetLojaRoute($stateProvider) {

        $stateProvider
                .state('consultaReturnSheetLoja', {
                    url: '/consultaReturnSheetLoja',
                    templateUrl: 'Scripts/app/reabastecimento/returnSheet/consulta-Return-Sheet-Loja.view.html',
                    controller: 'ConsultaReturnSheetLojaController'
                });
    }
})();