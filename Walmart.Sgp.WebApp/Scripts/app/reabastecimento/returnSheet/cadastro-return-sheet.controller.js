(function () {
    'use strict';

    angular
		.module('SGP')
        .config(CadastroReturnSheetRoute)
		.controller('CadastroReturnSheetController', CadastroReturnSheetController);

    CadastroReturnSheetController.$inject = ['$scope', '$state', '$stateParams', 'StackableModalService', 'ReturnSheetService', 'ReturnSheetItemPrincipalService', '$q', 'UserSessionService', 'ToastService', 'ngDialog', 'ValidationService', 'PagingService', 'returnSheet', '$timeout'];

    function CadastroReturnSheetController($scope, $state, $stateParams, $modal, returnSheetService, returnSheetItemPrincipalService, $q, userSession, toastService, ngDialog, $validation, pagingService, returnSheet, $timeout) {

        initialize();

        function initialize() {

            $validation.prepare($scope);

            $scope.data = { values: null, podeSerEditada: true, jaComecou: false, idReturnSheet: returnSheet.idReturnSheet, inicioReturn: null, finalReturn: null, inicioEvento: null, finalEvento: null, horaCorte: null, descricao: null, cdDepartamento: null, departamento: null, idRegiaoCompra: returnSheet.idRegiaoCompra, inativo: false };

            $scope.data.jaComecou = (returnSheet.dhInicioReturn != null && returnSheet.dhInicioReturn != '' && new Date(returnSheet.dhInicioReturn) <= new Date());

            $scope.data.paging = { offset: 0, limit: 20, orderBy: null };
            $scope.addItens = addItens;
            $scope.back = back;
            $scope.search = search;
            $scope.remove = remove;
            $scope.edit = edit;
            $scope.save = save;
            $scope.popular = popular;
            $scope.orderBy = orderBy;
            $scope.backWithoutChanges = backWithoutChanges;
            $scope.hasChanges = hasChanges;

            //if (!$stateParams.original) {
                backupReturnSheetOriginal();
            //} else {
            //    $scope.returnSheetOriginal = $stateParams.original;
            //}

            $scope.$watch('$viewContentLoaded', function () {
                popular();
            });
        }

        function backWithoutChanges() {
            if (!$scope.hasChanges()) {
                back();
            }
        }

        function orderBy(field) {
            pagingService.toggleSorting($scope.data.paging, field);
            search();
        }

        function escondeConsulta() {
            $scope.data.values = null;
        }

        function popular() {
            if (returnSheet.idReturnSheet > 0) {
                atualizar(returnSheet);
                search(1);
            }

            /*if (!!$stateParams.current) {

                $scope.data.inicioReturn = $stateParams.current.inicioReturn;
                $scope.data.finalReturn = $stateParams.current.finalReturn;
                $scope.data.inicioEvento = $stateParams.current.inicioEvento;
                $scope.data.finalEvento = $stateParams.current.finalEvento;
                $scope.data.horaCorte = $stateParams.current.horaCorte;
                $scope.data.descricao = $stateParams.current.descricao;
                $scope.data.idRegiaoCompra = $stateParams.current.idRegiaoCompra;
                $scope.data.cdDepartamento = $stateParams.current.cdDepartamento;
                $scope.data.inativo = $stateParams.current.inativo;
            }*/

            backupReturnSheetOriginal();
        }

        function hasChanges() {
            return ($scope.returnSheetOriginal.inicioReturn != $scope.data.inicioReturn ||
                    $scope.returnSheetOriginal.finalReturn != $scope.data.finalReturn ||
                    $scope.returnSheetOriginal.inicioEvento != $scope.data.inicioEvento ||
                    $scope.returnSheetOriginal.finalEvento != $scope.data.finalEvento ||
                    $scope.returnSheetOriginal.descricao != $scope.data.descricao ||
                    $scope.returnSheetOriginal.idRegiaoCompra != $scope.data.idRegiaoCompra ||
                    $scope.returnSheetOriginal.cdDepartamento != $scope.data.cdDepartamento ||
                    $scope.returnSheetOriginal.horaCorte != $scope.data.horaCorte);
        }

        function backupReturnSheetOriginal() {
            $scope.returnSheetOriginal = {
                inicioReturn: $scope.data.inicioReturn,
                finalReturn: $scope.data.finalReturn,
                inicioEvento: $scope.data.inicioEvento,
                finalEvento: $scope.data.finalEvento,
                descricao: $scope.data.descricao,
                idRegiaoCompra: $scope.data.idRegiaoCompra,
                cdDepartamento: $scope.data.cdDepartamento,
                horaCorte: $scope.data.horaCorte
            };
        }

        function validou() {

            if ($scope.data.idRegiaoCompra == null || $scope.data.idRegiaoCompra == '') {
                toastService.error(globalization.texts.regionMustBeSpecified);
                return false;
            }

            if ($scope.data.descricao.length > 50) {
                toastService.warning(globalization.texts.descriptionMaxLen);
                return false;
            }

            return true;
        }

        function save() {

            if (!$validation.validate($scope)) return;

            if (!validou()) {
                return;
            }

            var idUsuario = userSession.getCurrentUser().id;

            var deferred = $q
                .when(returnSheetService.inserirOuAtualizar($scope.data.inicioReturn, $scope.data.finalReturn, $scope.data.inicioEvento, $scope.data.finalEvento, $scope.data.horaCorte, $scope.data.descricao, $scope.data.departamento.idDepartamento, $scope.data.idRegiaoCompra, idUsuario, $scope.data.idReturnSheet))
                .then(salvou);

        }

        function salvou(data) {
            if ($state.is('cadastroReturnSheetNew')) {
                $state.go('cadastroReturnSheetEdit', { id: data.idReturnSheet });
                $timeout(function () { // suppress-validator
                    toastService.success(globalization.texts.returnSheetSavedSuccessfully);
                }, 100, false);
            } else {
                $scope.data.idReturnSheet = data.idReturnSheet;
                backupReturnSheetOriginal();
                search();
                toastService.success(globalization.texts.returnSheetSavedSuccessfully);
                $validation.accept($scope);
            }
        }

        function atualizar(data) {
            $scope.data.idReturnSheet = data.idReturnSheet;
            $scope.data.inicioReturn = data.dhInicioReturn;
            $scope.data.finalReturn = data.dhFinalReturn;
            $scope.data.inicioEvento = data.dhInicioEvento;
            $scope.data.finalEvento = data.dhFinalEvento;
            $scope.data.horaCorte = new Date(data.horaCorte);
            $scope.data.descricao = data.descricao;
            $scope.data.idRegiaoCompra = data.idRegiaoCompra;
            $scope.data.cdDepartamento = data.departamento.cdDepartamento;
            $scope.data.inativo = !data.blAtivo;

            var deferred = $q
                .when(returnSheetService.podeSerEditada(data.idReturnSheet))
                .then(function (data) {
                    $scope.data.podeSerEditada = data;
                });

        }

        function edit(cdItem, cdSistema) {
            updateState();
            $state.go('lojasValidasItemEdit', {
                'id': $scope.data.idReturnSheet,
                'cdItem': cdItem,
                'cdSistema': cdSistema
            });
        }

        function remove() {
            if ($scope.data.jaComecou) {
                var dialog = ngDialog.open({
                    template: 'Scripts/app/alertas/confirmar-view.html',
                    controller: ['$scope', function ($scope) {
                        $scope.message = globalization.texts.runningReturnSheetDeletingWarning;
                    }]
                });

                dialog.closePromise.then(function (data) {
                    if (data.value) {
                        excluir();
                    }
                });
            }
            else {
                excluir();
            }
        }

        function excluir() {
            var deferred = $q
                .when(returnSheetService.remover($scope.data.idReturnSheet))
                .then(function () {
                    back(true);
                    toastService.success(globalization.texts.recordRemovedSuccessfully);
                });
        }

        function search(pageNumber) {

            pagingService.calculateOffset($scope.data.paging, pageNumber);

            var idReturnSheet = $scope.data.idReturnSheet;

            var deferred = $q
                .when(returnSheetItemPrincipalService.pesquisar(idReturnSheet, $scope.data.paging))
                .then(exibeConsulta)
                .catch(escondeConsulta);
        }

        function exibeConsulta(data) {
            $scope.data.values = data;
            return data;
        }

        function back() {
            $state.go('pesquisaReturnSheet');
        }

        function addItens() {
            updateState();
            $state.go('pesquisaItensReturnSheetEdit', {
                'id': $scope.data.idReturnSheet,
                'cdSistema': 1
            });
        }

        function updateState() {
            /*var current = {
                idReturnSheet: $scope.data.idReturnSheet,
                inicioReturn: $scope.data.inicioReturn,
                finalReturn: $scope.data.finalReturn,
                inicioEvento: $scope.data.inicioEvento,
                finalEvento: $scope.data.finalEvento,
                horaCorte: $scope.data.horaCorte,
                descricao: $scope.data.descricao,
                idRegiaoCompra: $scope.data.idRegiaoCompra,
                cdDepartamento: $scope.data.cdDepartamento,
                inativo: $scope.data.inativo
            };*/
            $state.update({ id: $scope.data.idReturnSheet, filters: $scope.filters, paging: $scope.data.paging /*, current: current, original: $scope.returnSheetOriginal*/ });
        }
    }

    CadastroReturnSheetRoute.$inject = ['$stateProvider'];
    function CadastroReturnSheetRoute($stateProvider) {

        $stateProvider
            .state('cadastroReturnSheetNew', {
                url: '/pesquisaReturnSheet/new',
                templateUrl: 'Scripts/app/reabastecimento/returnSheet/cadastro-return-sheet.view.html',
                controller: 'CadastroReturnSheetController',
                params: {
                    current: null,
                    original: null
                },
                resolve: {
                    returnSheet: ['$stateParams', function ($stateParams) {
                        return { isNew: true };
                    }]
                }
            })
            .state('cadastroReturnSheetEdit', {
                url: '/pesquisaReturnSheet/edit/:id',
                templateUrl: 'Scripts/app/reabastecimento/returnSheet/cadastro-return-sheet.view.html',
                controller: 'CadastroReturnSheetController',
                params: {
                    current: null,
                    original: null
                },
                resolve: {
                    returnSheet: ['$stateParams', 'ReturnSheetService', function ($stateParams, returnSheetService) {
                        return returnSheetService.obter($stateParams.id);
                    }]
                }
            });
    }
})();