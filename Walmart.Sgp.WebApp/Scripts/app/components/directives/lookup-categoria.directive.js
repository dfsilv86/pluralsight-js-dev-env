(function () {
    'use strict';

    // <lookup-categoria ng-model="cdCategoria" sistema="cdSistema" departamento="cdDepartamento" item-out="item" description-width="xlarge"></lookup-categoria>

    angular
        .module('SGP')
        .directive('lookupCategoria', LookupCategoria);

    LookupCategoria.$inject = ['$q', 'CategoriaService', '$timeout'];

    function LookupCategoria($q, categoriaService, $timeout) {
        return {
            restrict: 'E',
            replace: true,
            templateUrl: 'Scripts/app/components/directives/lookup.template.html',
            scope: {
                sistema: '=',
                ngModel: '=',
                departamento: '=?',
                departamentoRequired: '=?',
                divisao: '=?',
                itemOut: '=?',
                descriptionWidth: '@',
                ngRequired: '=?',
                ngDisabled: '=?'
            },
            link: function ($scope, elem, attr) {
                $scope.lookupName = attr.name || 'lookupCategoria';
                $scope.isRequired = angular.isDefined(attr.$attr.required);
                if (angular.isDefined(attr.maxWidth)) {
                    $scope.maxWidth = attr.maxWidth;
                }
                $scope.maxLength = "4";
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
            controller: ['$scope', 'StackableModalService', 'PropagationService', CategoriaLookupController]
        };

        function CategoriaLookupController($scope, $modal, propagationService) {

            $scope.maxWidth = $scope.maxWidth || 9;
            $scope.data = { item: null, value: '', text: '' };
            $scope.$watchGroup(['sistema', 'departamento', 'divisao', 'ngModel'], refresh);
            $scope.lookup = lookup;
            $scope.search = search;
            $scope.check = check;
            $scope.reset = reset;

            propagationService.prepareLookup($scope);

            var skipReset = true;

            function search() {
                if (zeroIsValid($scope.data.value) !== '') {
                    return $scope.lookup();
                } else {
                    return openModal();
                }
            }

            function openModal() {
                var theModal = $modal.open({
                    templateUrl: 'Scripts/app/estruturaMercadologica/modal-categoria-lookup.html',
                    controller: 'ModalCategoriaLookupController',
                    resolve: {
                        sistema: $scope.sistema,
                        categoria: $scope.data.value,
                        departamento: $scope.departamento,
                        requiredOptions: {
                            departamento: $scope.departamentoRequired
                        }
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

                var theValueIndex = 3;

                var didReset = resetIfRelevant(newValues[0], oldValues[0], !!$scope.data.item ? $scope.data.item.cdSistema : null, true);
                didReset = didReset || resetIfRelevant(newValues[1], oldValues[1], !!$scope.data.item ? $scope.data.item.departamento.cdDepartamento : null, false, true);
                didReset = didReset || resetIfRelevant(newValues[2], oldValues[2], !!$scope.data.item ? $scope.data.item.departamento.divisao.cdDivisao : null, false, true);
                didReset = didReset || resetIfRelevant(newValues[theValueIndex], oldValues[theValueIndex], $scope.data.value, true, true);

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
                var cdDepartamento = $scope.departamento;
                var cdCategoria = $scope.data.value;

                if ($scope.data.item != null &&
                    $scope.data.item.cdCategoria == cdCategoria &&
                    $scope.data.item.departamento.cdDepartamento == cdDepartamento) {

                    // valor não foi modificado, não deve reagir
                    return $q.resolve($scope.data.item);
                }

                if (zeroIsValid(cdCategoria) !== '' && angular.isDefined(cdSistema) && !!cdSistema) {

                    return $q
                        .when(categoriaService.obterPorCategoriaESistema(cdCategoria, cdDepartamento, cdSistema, isInitializing))
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
                $scope.data.value = item.cdCategoria;
                if ($scope.ngModel !== item.cdCategoria) {
                    $scope.ngModel = item.cdCategoria;
                }
                $scope.data.text = item.dsCategoria;
                if (item.departamento) {
                    $scope.departamento = item.departamento.cdDepartamento;
                    if (item.departamento.divisao) {
                        $scope.divisao = item.departamento.divisao.cdDivisao;
                    }
                    $scope.sistema = item.cdSistema;
                }

                $scope.$emit('propagate-request', {
                    owner: 'categoria',
                    level: 4,
                    categoria: item,
                    departamento: item.departamento,
                    divisao: item.departamento.divisao,
                    sistema: item.cdSistema
                });

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

                if (propagateArgs.level == 4) return;

                if (propagateArgs.level > 4) {

                    var value = propagateArgs.categoria;

                    if (!!value) {
                        $scope.ngModel = $scope.data.value = value.cdCategoria;
                        $scope.data.text = value.dsCategoria;
                        $scope.itemOut = $scope.data.item = value;
                    }
                } else {

                    var doReset = false;

                    if (!!propagateArgs.departamento && !!$scope.departamento && propagateArgs.departamento.cdDepartamento != $scope.departamento) doReset = true;
                    if (!!propagateArgs.divisao && !!$scope.divisao && propagateArgs.divisao.cdDivisao != $scope.divisao) doReset = true;
                    if (!!propagateArgs.sistema && !!$scope.sistema && propagateArgs.sistema != $scope.sistema) doReset = true;

                    if (doReset) reset();
                }
            });
        }
    }
})();