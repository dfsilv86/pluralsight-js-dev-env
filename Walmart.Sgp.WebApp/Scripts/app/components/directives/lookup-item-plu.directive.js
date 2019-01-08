(function () {
    //'use strict';

    // <lookup-item ng-model="cdPLU" sistema="cdSistema" item-out="item"></lookup-item>

    angular
        .module('SGP')
        .directive('lookupItemPlu', LookupItem);

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
                ngDisabled: '='
            },
            link: function ($scope, elem, attr) {
                $scope.lookupName = attr.name || 'itemPlu';
                $scope.isRequired = angular.isDefined(attr.$attr.required);
                $scope.isDisabled = angular.isDefined(attr.$attr.disabled);
                $scope.hasDepartamento = angular.isDefined(attr.$attrs.departamento);
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
                        item: null,
                        plu: $scope.data.value
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

                var cdPLU = $scope.data.value;

                if ($scope.data.item != null && $scope.data.item.cdplu == cdPLU) {
                    // valor não foi modificado, não deve reagir
                    return $q.resolve($scope.data.item);
                }

                if (angular.isDefined(cdPLU) && !!cdPLU && angular.isDefined(cdSistema) && !!cdSistema) {

                    return $q
                        .when(itemService.obterPorPluESistema(cdPLU, cdSistema, isInitializing))
                        .then(applyValue)
                        .catch(invalidValue)
                        .finally($scope.focus);
                }
            }

            function applyValue(itemDetalhe) {
                if (angular.isUndefined(itemDetalhe) || !itemDetalhe) {
                    return $q.reject(itemDetalhe);
                }
                $scope.itemOut = $scope.data.item = itemDetalhe;
                $scope.data.value = itemDetalhe.cdplu;
                if ($scope.ngModel !== itemDetalhe.cdplu) {
                    $scope.ngModel = itemDetalhe.cdplu;
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
                }
                if (itemDetalhe.subcategoria) {
                    $scope.subcategoria = itemDetalhe.subcategoria.cdSubcategoria;
                }
                if (itemDetalhe.fineLine) {
                    $scope.fineLine = itemDetalhe.fineLine.cdFineLine;
                }

                // Verificado se os valores estão diferentes, pois no caso de serem os mesmos estava acontecendo o bug:
                // Bug 3218:Está voltando o campo Bandeira para Walmart ao pesquisar por um item.
                if ($scope.sistema != itemDetalhe.cdSistema) {
                    $scope.sistema = itemDetalhe.cdSistema;
                }

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
        }
    }
})();