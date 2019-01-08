(function () {
    'use strict';

    angular
		.module('SGP')
        .config(LojasValidasItemRoute)
		.controller('LojasValidasItemController', LojasValidasItemController);

    LojasValidasItemController.$inject = ['$scope', '$state', '$stateParams', 'ValidationService', 'PagingService', 'ChangeTrackerFactory', '$q', 'ReturnSheetService', 'ReturnSheetItemLojaService', 'ReturnSheetItemPrincipalService', 'idReturnSheet', 'cdItem', 'cdSistema', 'jaComecou', 'podeSerEditada', 'ngDialog', 'estados', 'returnSheet', 'ToastService', '$timeout'];

    function LojasValidasItemController($scope, $state, $stateParams, $validation, pagingService, changeTrackerFactory, $q, returnSheetService, returnSheetItemLojaService, returnSheetItemPrincipalService, idReturnSheet, cdItem, cdSistema, jaComecou, podeSerEditada, ngDialog, estados, returnSheet, toastService, $timeout) {

        initialize();

        var changeTracker = changeTrackerFactory.createChangeTrackerForProperties(['precoVenda', 'selecionado'], function (left, right) {
            return !!left && !!right && left.idItemDetalheEntrada === right.idItemDetalheEntrada && left.idLoja === right.idLoja;
        });

        function initialize() {
            $validation.prepare($scope);

            $scope.data = {
                returnSheet: returnSheet,
                values: null,
                todosMarcado: false,
                paging: {
                    offset: 0, limit: 50, orderBy: 'selecionado DESC, cdLoja',
                },
                precoVenda: null,
                idReturnSheet: idReturnSheet,
                cdItem: cdItem,
                cdSistema: cdSistema,
                enableRemove: false,
                enableRemoveInitialized: false,
                jaComecou: jaComecou,
                podeSerEditada: podeSerEditada,
                dsEstado: '',
                estados: estados
            };

            $scope.changedItens = [];
            $scope.hasChanges = hasChanges;
            $scope.discardChanges = discardChanges;
            $scope.back = back;
            $scope.backWithoutChanges = backWithoutChanges;
            $scope.save = save;
            $scope.orderBy = orderBy;
            $scope.marcarTodos = marcarTodos;
            $scope.search = search;
            $scope.changedItem = changedItem;
            $scope.remove = remove;
            $scope.$watch('data.precoVenda', alterarPrecoVenda);
            $scope.$watch('data.dsEstado', function (newValue, oldValue) {
                if (newValue !== oldValue) {
                    search(1);
                }
            });


            search(1);
        }

        function hasChanges() {
            return changeTracker.hasChanges();
        }

        function discardChanges() {
            changeTracker.undoAll();
        }

        function search(pageNumber) {
            // TODO: se for necessário salvar/restaurar o paging no futuro (via $state.update), retirar isso aqui
            if (pageNumber === undefined) {
                pageNumber = 1;
            }

            pagingService.calculateOffset($scope.data.paging, pageNumber);

            var idReturnSheet = $scope.data.idReturnSheet;
            var cdItem = $scope.data.cdItem;
            var cdSistema = $scope.data.cdSistema;
            var dsEstado = $scope.data.dsEstado || '';

            $q.when(returnSheetItemLojaService.obterLojasValidasItem(cdItem, cdSistema, idReturnSheet, dsEstado, $scope.data.paging))
               .then(applyValue);
        }

        function applyValue(data) {
            changeTracker.track(data);
            setPrecoVenda(data);
            $scope.data.values = data;

            if (!$scope.data.enableRemoveInitialized) {
                $scope.data.enableRemove = $scope.data.values.filter(function (item) { return item.selecionado; }).length > 0;
                $scope.data.enableRemoveInitialized = true;
            }

            pagingService.acceptPagingResults($scope.data.paging, data);

            $scope.data.todosMarcado = true;
            angular.forEach($scope.data.values, function (value) {
                if (value.selecionado === false) {
                    $scope.data.todosMarcado = false;
                }
            });
        }

        function alterarPrecoVenda() {
            angular.forEach($scope.data.values, function (item) {
                item.precoVenda = $scope.data.precoVenda;
            });

            $scope.changedItens = [];
        }

        function setPrecoVenda(data) {
            if ($scope.data.precoVenda === null) return;

            angular.forEach(data, function (item) {
                if (!isChangedItem(item)) {
                    item.precoVenda = $scope.data.precoVenda;
                }
            });
        }

        function isChangedItem(item) {
            return $scope.changedItens.filter(function (element) {
                return item.idItemDetalheEntrada === element.idItemDetalheEntrada
                    && item.idLoja === element.idLoja;
            }).length > 0;
        }

        function back() {
            if ($state.is('lojasValidasItemEdit')) {
                $state.go('cadastroReturnSheetEdit');
            } else if ($state.is('pesquisaItensLojasValidasItemEdit')) {
                $state.go('pesquisaItensReturnSheetEdit');
            }
        }

        function backWithoutChanges() {
            if (!changeTracker.hasChanges()) {
                back();
            }
        }

        function save() {
            if (!$validation.validate($scope)) return;

            var lojasAlteradas = changeTracker.getChangedItems();

            if ($scope.data.jaComecou) {
                var estaExcluindo = false;
                angular.forEach(lojasAlteradas, function (loja) {
                    if ((loja.selecionado !== loja.original_selecionado) && loja.selecionado === false) {
                        estaExcluindo = true;
                    }
                });

                if (estaExcluindo) {
                    var dialog = ngDialog.open({
                        template: 'Scripts/app/alertas/confirmar-view.html',
                        controller: ['$scope', function ($scope) {
                            $scope.message = globalization.texts.runningReturnSheetStoreDeletingWarning;
                        }]
                    });

                    dialog.closePromise.then(function (data) {
                        if (data.value) {
                            persistirDados(lojasAlteradas);
                        }
                    });
                }
                else {
                    persistirDados(lojasAlteradas);
                }
            }
            else {
                persistirDados(lojasAlteradas);
            }
        }

        function persistirDados(lojasAlteradas) {
            $q.when(returnSheetItemPrincipalService.inserirOuAtualizar(lojasAlteradas, $scope.data.idReturnSheet, $scope.data.precoVenda))
                .then(function () {
                    back(true);
                    salvoComSucesso();
                    //toastService.success(globalization.texts.successfullySavedRecords);
                });
        }

        function orderBy(field) {
            pagingService.toggleSorting($scope.data.paging, field);
            search();
        }

        function marcarTodos() {
            window.event.stopPropagation();

            angular.forEach($scope.data.values, function (item) {
                item.selecionado = $scope.data.todosMarcado;
            });
        }

        function changedItem(item) {
            if (isChangedItem(item)) return;

            $scope.changedItens.push({
                idItemDetalheEntrada: item.idItemDetalheEntrada,
                idLoja: item.idLoja
            });
        }

        function remove() {
            $q.when(returnSheetItemPrincipalService.remover($scope.data.idReturnSheet, $scope.data.cdItem))
              .then(function () {
                  back(true);
                  toastService.success(globalization.texts.successfullyDeletedRecords);
              });
        }

        function salvoComSucesso() {
            // DEVERÁ exibir a mensagem quando for marcado ou desmarcado o checkbox do Item ou alterado o preço de um item com o check box marcado.
            var changed = changeTracker.getChangedItems();

            var filtered = changed.filter(function (item) {
                var precoAlteradoEItemMarcado = ((item.original_precoVenda != item.precoVenda) && item.selecionado);
                var itemSelecionadoOuDeselecionado = item.selecionado != item.original_selecionado;

                return (precoAlteradoEItemMarcado || itemSelecionadoOuDeselecionado);
            });

            if (filtered.length) {
                toastService.success(globalization.texts.successfullySavedRecords);
            }
        }
    }

    LojasValidasItemRoute.$inject = ['$stateProvider'];
    function LojasValidasItemRoute($stateProvider) {
        // pode entrar por duas telas diferentes
        $stateProvider
            .state('lojasValidasItemEdit', {
                url: '/pesquisaReturnSheet/lojas/edit/:id/:cdSistema/:cdItem/',
                templateUrl: 'Scripts/app/reabastecimento/returnSheet/lojas-validas-item.view.html',
                controller: 'LojasValidasItemController',
                resolve: {
                    idReturnSheet: ['$stateParams', function ($stateParams) {
                        return $stateParams.id;
                    }],
                    cdSistema: ['$stateParams', function ($stateParams) {
                        return $stateParams.cdSistema;
                    }],
                    cdItem: ['$stateParams', function ($stateParams) {
                        return $stateParams.cdItem;
                    }],
                    jaComecou: ['$stateParams', 'ReturnSheetService', function ($stateParams, returnSheetService) {
                        return returnSheetService.jaComecou($stateParams.id);
                    }],
                    podeSerEditada: ['$stateParams', 'ReturnSheetService', function ($stateParams, returnSheetService) {
                        return returnSheetService.podeSerEditada($stateParams.id);
                    }],
                    returnSheet: ['$stateParams', 'ReturnSheetService', function ($stateParams, returnSheetService) {
                        return returnSheetService.obter($stateParams.id, '');
                    }],
                    estados: ['$stateParams', 'ReturnSheetItemLojaService', function ($stateParams, returnSheetItemLojaService) {
                        return returnSheetItemLojaService.obterEstadosLojasValidasItem($stateParams.cdItem, $stateParams.cdSistema);
                    }]
                }
            })
            .state('pesquisaItensLojasValidasItemEdit', {
                url: '/pesquisaReturnSheet/itens/edit/:id/:cdSistema/:cdItem/',
                templateUrl: 'Scripts/app/reabastecimento/returnSheet/lojas-validas-item.view.html',
                controller: 'LojasValidasItemController',
                resolve: {
                    idReturnSheet: ['$stateParams', function ($stateParams) {
                        return $stateParams.id;
                    }],
                    cdSistema: ['$stateParams', function ($stateParams) {
                        return $stateParams.cdSistema;
                    }],
                    cdItem: ['$stateParams', function ($stateParams) {
                        return $stateParams.cdItem;
                    }],
                    jaComecou: ['$stateParams', 'ReturnSheetService', function ($stateParams, returnSheetService) {
                        return returnSheetService.jaComecou($stateParams.id);
                    }],
                    podeSerEditada: ['$stateParams', 'ReturnSheetService', function ($stateParams, returnSheetService) {
                        return returnSheetService.podeSerEditada($stateParams.id);
                    }],
                    returnSheet: ['$stateParams', 'ReturnSheetService', function ($stateParams, returnSheetService) {
                        return returnSheetService.obter($stateParams.id, '');
                    }],
                    estados: ['$stateParams', 'ReturnSheetItemLojaService', function ($stateParams, returnSheetItemLojaService) {
                        return returnSheetItemLojaService.obterEstadosLojasValidasItem($stateParams.cdItem, $stateParams.cdSistema);
                    }]
                }
            });
    }
})();