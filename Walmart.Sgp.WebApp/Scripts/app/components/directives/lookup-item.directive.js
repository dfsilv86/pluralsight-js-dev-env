(function () {
    //'use strict';

    // <lookup-item ng-model="cdItem" sistema="cdSistema" item-out="item"></lookup-item>

    angular
        .module('SGP')
        .directive('lookupItem', LookupItem);

    LookupItem.$inject = ['$q', 'ItemDetalheService', '$timeout'];

    function LookupItem($q, itemService, $timeout) {
        return {
            restrict: 'E',
            replace: true,
            templateUrl: 'Scripts/app/components/directives/lookup.template.html',
            scope: {
                sistema: '=',
                ngModel: '=',
                fineLine: '=?',
                subcategoria: '=?',
                categoria: '=?',
                departamento: '=?',
                divisao: '=?',
                itemOut: '=?',
                descriptionWidth: '@',
                ngRequired: '=',
                ngDisabled: '=',
                lockDepartamento: '='
            },
            link: function ($scope, elem, attr) {
                $scope.lookupName = attr.name || 'lookupItem';
                $scope.isRequired = angular.isDefined(attr.$attr.required);
                $scope.isDisabled = angular.isDefined(attr.$attr.disabled);
                $scope.hasDepartamento = angular.isDefined(attr.$attr.departamento);

                if (angular.isDefined(attr.maxWidth)) {
                    $scope.maxWidth = attr.maxWidth;
                }
                $scope.focus = function () {
                    $timeout(function () { // suppress-validator
                        if (!!$scope.data.value) {
                            elem.find('button.btn').focus();
                        } else {
                            elem.find('input.lookup-key').focus();
                        }
                    });
                };
                //se for descomentar, avisar no #general - pode ser feito de outro jeito
                //if (angular.isDefined(attr.$attr.disabled)) {
                //    elem.find('input.lookup-component').attr('disabled', 'disabled');
                //    elem.find('button.btn-lookup').css('display', 'none');
                //}
            },
            controller: ['$scope', 'StackableModalService', ItemLookupController]
        };

        function ItemLookupController($scope, $modal) {

            $scope.maxWidth = $scope.maxWidth || 9;
            $scope.data = { item: null, value: '', text: '' };
            $scope.$watchGroup(['sistema', 'departamento', 'divisao', 'categoria', 'subcategoria', 'fineLine', 'ngModel'], refresh);
            $scope.lookup = lookup;
            $scope.search = search;
            $scope.check = check;
            $scope.reset = reset;

            var skipReset = true;

            function search() {
                if (!!$scope.data.value) {
                    return $scope.lookup();
                } else {
                    return openModal();
                }
            }

            function openModal() {
                var theModal = $modal.open({
                    templateUrl: 'Scripts/app/estruturaMercadologica/modal-item-lookup.html',
                    controller: 'ModalItemLookupController',
                    resolve: {
                        sistema: $scope.sistema,
                        item: $scope.data.value,
                        departamento: $scope.departamento,
                        lockDepartamento: $scope.lockDepartamento,
                        plu: null
                    },
                    opened: function () {
                        // Evita que a modal seja aberta duas vezes quando o usuário tecla enter.
                        reset();
                    }
                });

                return theModal.then(applyValue).finally($scope.focus);
            }

            function check() {
                if ($scope.data.value) {
                    search();
                }
                else {
                    reset();
                }
            }

            function resetIfRelevant(newValue, oldValue, itemValue, isRequired, allowZeroes) {
                var theValue = newValue, hasValue = !!newValue;
                var theItemValue = itemValue, hasItemValue = !!itemValue;
                if (allowZeroes === true) {
                    if (zeroIsValid(theValue) !== '') {
                        hasValue = true;
                    }
                    if (zeroIsValid(theItemValue) !== '') {
                        hasItemValue = true;
                    }
                }
                if (isRequired && !hasValue) {
                    if (!skipReset) {
                        reset();
                    }
                    return true;
                }
                if (hasValue && hasItemValue && newValue != itemValue) {
                    if (!skipReset) {
                        reset();
                    }
                    return true;
                }
                return false;
            }

            function refresh(newValues, oldValues) {

                var theValueIndex = 6;

                var didReset = resetIfRelevant(newValues[0], oldValues[0], !!$scope.data.item ? $scope.data.item.cdSistema : null, true);
                didReset = didReset || resetIfRelevant(newValues[1], oldValues[1], !!$scope.data.item ? $scope.data.item.departamento.cdDepartamento : null, false, true);
                didReset = didReset || resetIfRelevant(newValues[2], oldValues[2], !!$scope.data.item ? $scope.data.item.departamento.divisao.cdDivisao : null, false, true);
                didReset = didReset || resetIfRelevant(newValues[3], oldValues[3], !!$scope.data.item ? $scope.data.item.categoria.cdCategoria : null, false, true);
                didReset = didReset || resetIfRelevant(newValues[4], oldValues[4], !!$scope.data.item ? $scope.data.item.subcategoria.cdSubcategoria : null, false, true);
                didReset = didReset || resetIfRelevant(newValues[5], oldValues[5], !!$scope.data.item ? $scope.data.item.fineLine.cdFineLine : null, false, true);
                didReset = didReset || resetIfRelevant(newValues[theValueIndex], oldValues[theValueIndex], $scope.data.value, true);

                if (didReset) {

                    skipReset = false;
                }

                if ((!didReset && $scope.data.value !== newValues[theValueIndex]) || (didReset && !!oldValues[theValueIndex] && newValues[theValueIndex] !== oldValues[theValueIndex])) {
                    $scope.data.value = newValues[theValueIndex];
                    lookup(newValues.equals(oldValues));
                    skipReset = false;
                }
            }

            function lookup(isInitializing) {

                var cdSistema = $scope.sistema;
                var cdItem = $scope.data.value;
                var cdDepartamento = $scope.departamento;
                var lockDepartamento = $scope.lockDepartamento;

                if ($scope.data.item != null && $scope.data.item.cdItem == cdItem) {
                    // valor não foi modificado, não deve reagir
                    return $q.resolve($scope.data.item);
                }

                if (angular.isDefined(cdItem) && !!cdItem && angular.isDefined(cdSistema) && !!cdSistema) {

                    if (angular.isDefined(cdDepartamento) && angular.isDefined(lockDepartamento) && lockDepartamento) {
                        return $q
                            .when(itemService.obterPorOldNumberESistemaEDepartamento(cdItem, cdSistema, cdDepartamento, isInitializing))
                            .then(applyValue)
                            .catch(invalidValue)
                            .finally($scope.focus);
                    }
                    else {
                        return $q
                            .when(itemService.obterPorOldNumberESistema(cdItem, cdSistema, isInitializing))
                            .then(applyValue)
                                .catch(invalidValue)
                                .finally($scope.focus);
                    }
                }
            }

            function applyValue(itemDetalhe) {
                if (angular.isUndefined(itemDetalhe) || !itemDetalhe) {
                    return $q.reject(itemDetalhe);
                }
                $scope.itemOut = $scope.data.item = itemDetalhe;
                $scope.data.value = itemDetalhe.cdItem;
                if ($scope.ngModel !== itemDetalhe.cdItem) {
                    $scope.ngModel = itemDetalhe.cdItem;
                }
                $scope.data.text = itemDetalhe.dsItem;
                if (itemDetalhe.departamento) {

                    if ($scope.hasDepartamento) {
                        $scope.departamento = itemDetalhe.departamento.cdDepartamento;
                    }

                    if (itemDetalhe.departamento.divisao) {
                        $scope.divisao = itemDetalhe.departamento.divisao.cdDivisao;
                    }
                }
                if (itemDetalhe.categoria) {
                    $scope.categoria = itemDetalhe.categoria.cdCategoria;
                    itemDetalhe.categoria.departamento = itemDetalhe.departamento;
                }
                if (itemDetalhe.subcategoria) {
                    $scope.subcategoria = itemDetalhe.subcategoria.cdSubcategoria;
                    itemDetalhe.subcategoria.categoria = itemDetalhe.categoria;
                }
                if (itemDetalhe.fineLine) {
                    $scope.fineLine = itemDetalhe.fineLine.cdFineLine;
                    itemDetalhe.fineLine.subcategoria = itemDetalhe.subcategoria;
                }

                // Verificado se os valores estão diferentes, pois no caso de serem os mesmos estava acontecendo o bug:
                // Bug 3218:Está voltando o campo Bandeira para Walmart ao pesquisar por um item.
                if ($scope.sistema != itemDetalhe.cdSistema) {
                    $scope.sistema = itemDetalhe.cdSistema;
                }

                $scope.$emit('propagate-request', {
                    owner: 'item',
                    level: 7,
                    item: itemDetalhe,
                    fineline: itemDetalhe.fineLine,
                    subcategoria: itemDetalhe.subcategoria,
                    categoria: itemDetalhe.subcategoria.categoria,
                    departamento: itemDetalhe.subcategoria.categoria.departamento,
                    divisao: itemDetalhe.subcategoria.categoria.departamento.divisao,
                    sistema: itemDetalhe.cdSistema
                });

                return itemDetalhe;
            }

            function invalidValue(response) {
                // TODO: validador
                return openModal().catch(function () {
                    reset();
                    return $q.reject(response);
                });
            }

            function reset() {
                $scope.data.value = '';
                $scope.data.text = '';
                $scope.itemOut = $scope.data.item = null;
                $scope.ngModel = null;
            }

            $scope.$on('propagate-data', function (event, propagateArgs) {

                if (propagateArgs.level == 7) return;

                if (propagateArgs.level > 7) {

                    var value = propagateArgs.item;

                    if (!!value) {
                        $scope.ngModel = $scope.data.value = value.cdItem;
                        $scope.data.text = value.dsItem;
                        $scope.itemOut = $scope.data.item = value;
                    }

                } else {

                    var doReset = false;

                    if (!!propagateArgs.fineline && !!$scope.fineLine && propagateArgs.fineline.cdFineLine != $scope.fineLine) doReset = true;
                    if (!!propagateArgs.subcategoria && !!$scope.subcategoria && propagateArgs.subcategoria.cdSubcategoria != $scope.subcategoria) doReset = true;
                    if (!!propagateArgs.categoria && !!$scope.categoria && propagateArgs.categoria.cdCategoria != $scope.categoria) doReset = true;
                    if (!!propagateArgs.departamento && !!$scope.departamento && propagateArgs.departamento.cdDepartamento != $scope.departamento) doReset = true;
                    if (!!propagateArgs.divisao && !!$scope.divisao && propagateArgs.divisao.cdDivisao != $scope.divisao) doReset = true;
                    if (!!propagateArgs.sistema && !!$scope.sistema && propagateArgs.sistema != $scope.sistema) doReset = true;

                    if (doReset) reset();
                }
            });
        }
    }
})();