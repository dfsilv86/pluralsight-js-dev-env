(function () {
    //'use strict';

    // <lookup-item-saida ng-model="cdItem" sistema="cdSistema" fornecedor="cdFornecedor" item-out="item"></lookup-item-saida>

    angular
        .module('SGP')
        .directive('lookupItemSaida', LookupItemSaida);

    LookupItemSaida.$inject = ['$q', 'ItemDetalheService', '$timeout'];

    function LookupItemSaida($q, itemService, $timeout) {
        return {
            restrict: 'E',
            replace: true,
            templateUrl: 'Scripts/app/components/directives/lookup.template.html',
            scope: {
                sistema: '=',
                ngModel: '=',
                fornecedorParam: '=',
                regiao: '=',
                itemOut: '=?',
                descriptionWidth: '@',
                ngRequired: '=',
                ngDisabled: '=',
                departamento: '=?',
                somenteStatus: '@?',
                modoPereciveis: '@?'
            },
            link: function ($scope, elem, attr) {
                $scope.lookupName = attr.name || 'lookupItemSaida';
                $scope.isRequired = angular.isDefined(attr.$attr.required);
                $scope.isDisabled = angular.isDefined(attr.$attr.disabled);
                $scope.hasDepartamento = angular.isDefined(attr.$attr.departamento);
                if (angular.isDefined(attr.maxWidth)) {
                    $scope.maxWidth = attr.maxWidth;
                }
                $scope.maxLength = "10";
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
            controller: ['$scope', 'StackableModalService', ItemSaidaLookupController]
        };

        function ItemSaidaLookupController($scope, $modal) {

            $scope.maxWidth = $scope.maxWidth || 9;
            $scope.data = { item: null, value: '', text: '' };
            $scope.$watchGroup(['sistema', 'regiao', 'departamento', 'ngModel'], refresh);
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
                    controller: 'ModalItemSaidaLookupController',
                    resolve: {
                        sistema: $scope.sistema,
                        fornecedorParam: $scope.fornecedorParam,
                        regiao: $scope.regiao,
                        departamento: $scope.departamento,
                        somenteStatus: $scope.somenteStatus,
                        modoPereciveis: $scope.modoPereciveis
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

                var theValueIndex = 3;

                var didReset = resetIfRelevant(newValues[0], oldValues[0], !!$scope.data.item ? $scope.data.item.cdSistema : null, true);
                didReset = didReset || resetIfRelevant(newValues[1], oldValues[1], !!$scope.data.item ? $scope.data.item.idRegiaoCompra : null, false);
                didReset = didReset || resetIfRelevant(newValues[2], oldValues[2], !!$scope.data.item ? $scope.data.item.departamento.cdDepartamento : null, false, true);
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
                var idFornecedorParam = $scope.fornecedorParam;
                var IDRegiaoCompra = $scope.regiao;
                var somenteStatus = $scope.somenteStatus || '';
                var blPerecivel = '';
                if ($scope.modoPereciveis == 'restrito') {
                    blPerecivel = 'S';
                }

                if ($scope.data.item != null && $scope.data.item.cdItem == cdItem) {
                    // valor não foi modificado, não deve reagir
                    return $q.resolve($scope.data.item);
                }

                if (angular.isDefined(cdItem) && !!cdItem && angular.isDefined(cdSistema) && !!cdSistema) {

                    return $q
                                                                                 
                        .when(itemService.obterItemSaidaPorFornecedorItemEntrada(cdItem, cdSistema, cdDepartamento, idFornecedorParam, IDRegiaoCompra, somenteStatus, blPerecivel, isInitializing))
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
                $scope.data.value = itemDetalhe.cdItem;
                if ($scope.ngModel !== itemDetalhe.cdItem) {
                    $scope.ngModel = itemDetalhe.cdItem;
                }
                if (itemDetalhe.departamento) {
                    if ($scope.hasDepartamento) {
                        $scope.departamento = itemDetalhe.departamento.cdDepartamento;
                    }
                }

                $scope.data.text = itemDetalhe.dsItem;
                //$scope.sistema = itemDetalhe.cdSistema;
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