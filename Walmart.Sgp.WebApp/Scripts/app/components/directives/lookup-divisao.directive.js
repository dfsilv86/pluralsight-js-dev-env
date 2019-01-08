(function () {
    'use strict';

    // <lookup-divisao ng-model="cdDivisao" sistema="cdSistema" item-out="item" description-width="xlarge"></lookup-item>

    angular
        .module('SGP')
        .directive('lookupDivisao', LookupDivisao);

    LookupDivisao.$inject = ['$q', 'DivisaoService', '$timeout'];

    function LookupDivisao($q, departamentoService, $timeout) {
        return {
            restrict: 'E',
            replace: true,
            templateUrl: 'Scripts/app/components/directives/lookup.template.html',
            scope: {
                sistema: '=',
                ngModel: '=',
                itemOut: '=?',
                descriptionWidth: '@',
                ngRequired: '=?',
                ngDisabled: '=?'
            },
            link: function ($scope, elem, attr) {
                $scope.lookupName = attr.name || 'lookupDivisao';
                $scope.isRequired = angular.isDefined(attr.$attr.required);
                if (angular.isDefined(attr.maxWidth)) {
                    $scope.maxWidth = attr.maxWidth;
                }
                $scope.maxLength = "2";
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
            controller: ['$scope', 'StackableModalService', 'UserSessionService', 'PropagationService', DivisaoLookupController]
        };

        function DivisaoLookupController($scope, $modal, userSession, propagationService) {

            $scope.maxWidth = $scope.maxWidth || 9;
            $scope.data = { item: null, value: '', text: '' };
            $scope.$watchGroup(['sistema', 'ngModel'], refresh);
            $scope.lookup = lookup;
            $scope.search = search;
            $scope.check = check;
            $scope.reset = reset;

            propagationService.prepareLookup($scope);

            var skipReset = true;

            // Here be dragons
            // Flag que indica que deve forçar a lookup a manter o valor atual por causa do workaround da sugestao de pedido
            var reapply = false;

            function search() {
                if (zeroIsValid($scope.data.value) !== '') {
                    return $scope.lookup();
                } else {
                    return openModal();
                }
            }

            function openModal() {
                var theModal = $modal.open({
                    templateUrl: 'Scripts/app/estruturaMercadologica/modal-divisao-lookup.html',
                    controller: 'ModalDivisaoLookupController',
                    resolve: {
                        sistema: $scope.sistema,
                        divisao: $scope.data.value
                    },
                    opened: function () {
                        // Evita que a modal seja aberta duas vezes quando o usuário tecla enter.
                        reset();
                    }
                });

                return theModal.then(applyValue).finally($scope.focus);
            }
         
            function check() {
                if (zeroIsValid($scope.data.value) === '') {
                    reset();
                }
                else {
                    search();
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

                var theValueIndex = 1;

                var workaround = $scope.data.item;

                var didReset = resetIfRelevant(newValues[0], oldValues[0], !!$scope.data.item ? $scope.data.item.cdSistema : null, true);
                didReset = didReset || resetIfRelevant(newValues[theValueIndex], oldValues[theValueIndex], $scope.data.value, true, true);

                if (didReset) {

                    skipReset = false;

                    // Here be dragons
                    // Flag que indica que deve forçar a lookup a manter o valor atual por causa do workaround da sugestao de pedido
                    if (reapply) {
                        applyValue(workaround);
                    }
                }

                if ((!didReset && $scope.data.value !== newValues[theValueIndex]) || (didReset && !!oldValues[theValueIndex] && newValues[theValueIndex] !== oldValues[theValueIndex])) {
                    $scope.data.value = newValues[theValueIndex];
                    lookup(newValues.equals(oldValues));
                    skipReset = false;
                }
            }

            function lookup(isInitializing) {

                var cdSistema = $scope.sistema;

                var cdDivisao = $scope.data.value;

                if ($scope.data.item != null && $scope.data.item.cdDivisao == cdDivisao) {
                    // valor não foi modificado, não deve reagir
                    return $q.resolve($scope.data.item);
                }

                if (zeroIsValid(cdDivisao) !== '' && angular.isDefined(cdSistema) && !!cdSistema) {

                    return $q
                        .when(departamentoService.obterPorDivisaoESistema(cdDivisao, cdSistema, isInitializing))
                        .then(applyValue)
                        .catch(invalidValue)
                        .finally($scope.focus);
                }
            }

            function applyValue(item) {
                if (angular.isUndefined(item) || !item) {
                    return $q.reject(item);
                }
                $scope.itemOut = $scope.data.item = item;
                $scope.data.value = item.cdDivisao;
                if ($scope.ngModel !== item.cdDivisao) {
                    $scope.ngModel = item.cdDivisao;
                }

                $scope.$emit('propagate-request', {
                    owner: 'divisao',
                    level: 2,
                    divisao: item,
                    sistema: item.cdSistema
                });

                reapply = true;

                $scope.data.text = item.dsDivisao;
                return item;
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

                if (propagateArgs.level == 2) return;

                if (propagateArgs.level > 2) {

                    var value = propagateArgs.divisao;

                    if (!!value) {
                        $scope.ngModel = $scope.data.value = value.cdDivisao;
                        $scope.data.text = value.dsDivisao;
                        $scope.itemOut = $scope.data.item = value;
                    }

                    reapply = false;

                } else {

                    var doReset = false;

                    if (!!propagateArgs.sistema && !!$scope.sistema && propagateArgs.sistema != $scope.sistema) doReset = true;

                    if (doReset) reset();
                }
            });
        }
    }
})();