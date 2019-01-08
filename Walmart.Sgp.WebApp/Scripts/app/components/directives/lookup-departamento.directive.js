(function () {
    'use strict';

    // <lookup-departamento ng-model="cdDepartamento" divisao="cdDivisao" sistema="cdSistema" item-out="item" description-width="xlarge" modo-pereciveis="(modo)" excluir-padaria="true|false"></lookup-departamento>

    // modo pereciveis: "qualquer" - qualquer departamento é válido para lookup e modal de pesquisa
    //                  "lookup"   - lookup (digitar o código no campo) aceita apenas perecível, mas permite selecionar departamento não-perecível dentro da modal de pesquisa
    //                  "restrito" - tanto a lookup como a modal de pesquisa filtram por blPerecível, não é possível ver ou selecionar departamento não-perecível.

    angular
        .module('SGP')
        .directive('lookupDepartamento', LookupDepartamento);

    LookupDepartamento.$inject = ['$q', 'DepartamentoService', '$timeout'];

    function LookupDepartamento($q, departamentoService, $timeout) {
        return {
            restrict: 'E',
            replace: true,
            templateUrl: 'Scripts/app/components/directives/lookup.template.html',
            scope: {
                sistema: '=',
                ngModel: '=',
                divisao: '=?',
                itemOut: '=?',
                descriptionWidth: '@',
                ngRequired: '=',
                ngDisabled: '=',
                modoPereciveis: '@?',
                excluirPadaria: '=?'
            },
            link: function ($scope, elem, attr) {
                $scope.lookupName = attr.name || 'lookupDepartamento';
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
            controller: ['$scope', 'StackableModalService', 'PropagationService', DepartamentoLookupController]
        };

        function DepartamentoLookupController($scope, $modal, propagationService) {

            $scope.maxWidth = $scope.maxWidth || 9;
            $scope.data = { item: null, value: '', text: '' };
            $scope.$watchGroup(['sistema', 'divisao', 'ngModel'], refresh);
            $scope.lookup = lookup;
            $scope.search = search;
            $scope.check = check;
            $scope.reset = reset;

            propagationService.prepareLookup($scope);

            var skipReset = true;

            function search() {
                if (zeroIsValid($scope.data.value) !== '') {
                    return $scope.lookup(($scope.modoPereciveis || 'qualquer') != 'restrito'); // Bug #4317 - de false para quando modo qualquer
                } else {
                    return openModal();
                }
            }

            function openModal() {
                var theModal = $modal.open({
                    templateUrl: 'Scripts/app/estruturaMercadologica/modal-departamento-lookup.html',
                    controller: 'ModalDepartamentoLookupController',
                    resolve: {
                        sistema: $scope.sistema,
                        departamento: $scope.data.value,
                        modoPereciveis: $scope.modoPereciveis,
                        excluirPadaria: $scope.excluirPadaria
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

                var theValueIndex = 2;

                var didReset = resetIfRelevant(newValues[0], oldValues[0], !!$scope.data.item ? $scope.data.item.cdSistema : null, true);
                didReset = didReset || resetIfRelevant(newValues[1], oldValues[1], !!$scope.data.item ? $scope.data.item.divisao.cdDivisao : null, false, true);
                didReset = didReset || resetIfRelevant(newValues[theValueIndex], oldValues[theValueIndex], $scope.data.value, true, true);

                if (didReset) {

                    skipReset = false;
                }

                if ((!didReset && $scope.data.value !== newValues[theValueIndex]) || (didReset && !!oldValues[theValueIndex] && newValues[theValueIndex] !== oldValues[theValueIndex])) {
                    $scope.data.value = newValues[theValueIndex];
                    lookup(true);
                    skipReset = false;
                }
            }

            function lookup(forcarQualquerDepartamento) {

                // Obtem o departamento pelo seu cdDepartamento e cdSistema
                // onde blPerecivel = 'S'

                var cdSistema = $scope.sistema;

                var cdDepartamento = $scope.data.value;

                if ($scope.data.item != null && $scope.data.item.cdDepartamento == cdDepartamento) {
                    // valor não foi modificado, não deve reagir
                    return $q.resolve($scope.data.item);
                }

                if (zeroIsValid(cdDepartamento) !== '' && angular.isDefined(cdSistema) && !!cdSistema) {

                    var tmpModo = $scope.modoPereciveis;

                    if (forcarQualquerDepartamento) {
                        tmpModo = 'qualquer';
                    }

                    return $q
                        .when(departamentoService.obterPorDepartamentoESistema(cdDepartamento, cdSistema, tmpModo, $scope.excluirPadaria, false))
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
                $scope.data.value = item.cdDepartamento;
                if ($scope.ngModel !== item.cdDepartamento) {
                    $scope.ngModel = item.cdDepartamento;
                }
                $scope.data.text = item.dsDepartamento;
                if (item.divisao) {
                    $scope.divisao = item.divisao.cdDivisao;
                    $scope.sistema = item.cdSistema;
                }

                $scope.$emit('propagate-request', {
                    owner: 'departamento',
                    level: 3,
                    departamento: item,
                    divisao: item.divisao,
                    sistema: item.cdSistema
                });

                return item;
            }

            function invalidValue(response) {
                // TODO: validador

                if ($scope.modoPereciveis == 'qualquer' && (angular.isUndefined(response) || !response)) {
                    reset();
                    return $q.resolve(null);
                }

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

                if (propagateArgs.level == 3) return;

                if (propagateArgs.level > 3) {

                    var value = propagateArgs.departamento;

                    if (!!value) {
                        $scope.ngModel = $scope.data.value = value.cdDepartamento;
                        $scope.data.text = value.dsDepartamento;
                        $scope.itemOut = $scope.data.item = value;
                    }
                } else {

                    var doReset = false;

                    if (!!propagateArgs.divisao && !!$scope.divisao && propagateArgs.divisao.cdDivisao != $scope.divisao) doReset = true;
                    if (!!propagateArgs.sistema && !!$scope.sistema && propagateArgs.sistema != $scope.sistema) doReset = true;

                    if (doReset) reset();
                }
            });
        }
    }
})();