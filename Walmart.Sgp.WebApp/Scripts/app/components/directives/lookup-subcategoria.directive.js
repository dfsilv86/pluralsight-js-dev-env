(function () {
    'use strict';

    // <lookup-subcategoria ng-model="cdSubcategoria" sistema="cdSistema" departamento="cdDepartamento" categoria="cdCategoria" item-out="item" description-width="xlarge"></lookup-subcategoria>

    angular
        .module('SGP')
        .directive('lookupSubcategoria', LookupSubcategoria);

    LookupSubcategoria.$inject = ['$q', 'SubcategoriaService', '$timeout'];

    function LookupSubcategoria($q, subcategoriaService, $timeout) {
        return {
            restrict: 'E',
            replace: true,
            templateUrl: 'Scripts/app/components/directives/lookup.template.html',
            scope: {
                sistema: '=',
                ngModel: '=',
                departamento: '=?',
                departamentoRequired: '=?',
                categoria: '=?',
                categoriaRequired: '=?',
                divisao: '=?',
                itemOut: '=?',
                descriptionWidth: '@',
                ngRequired: '=?',
                ngDisabled: '=?'
            },
            link: function ($scope, elem, attr) {
                $scope.lookupName = attr.name || 'lookupSubcategoria';
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
            controller: ['$scope', 'StackableModalService', 'PropagationService', SubcategoriaLookupController]
        };

        function SubcategoriaLookupController($scope, $modal, propagationService) {

            $scope.maxWidth = $scope.maxWidth || 9;
            $scope.data = { item: null, value: '', text: '' };
            $scope.$watchGroup(['sistema', 'departamento', 'divisao', 'categoria', 'ngModel'], refresh);
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
                    templateUrl: 'Scripts/app/estruturaMercadologica/modal-subcategoria-lookup.html',
                    controller: 'ModalSubcategoriaLookupController',
                    resolve: {
                        sistema: $scope.sistema,
                        categoria: $scope.categoria,
                        subcategoria: $scope.data.value,
                        departamento: $scope.departamento,
                        requiredOptions: {
                            departamento: $scope.departamentoRequired,
                            categoria: $scope.categoriaRequired
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

                var theValueIndex = 4;

                var didReset = resetIfRelevant(newValues[0], oldValues[0], !!$scope.data.item ? $scope.data.item.cdSistema : null, true);
                didReset = didReset || resetIfRelevant(newValues[1], oldValues[1], !!$scope.data.item ? $scope.data.item.categoria.departamento.cdDepartamento : null, false, true);
                didReset = didReset || resetIfRelevant(newValues[2], oldValues[2], !!$scope.data.item ? $scope.data.item.categoria.departamento.divisao.cdDivisao : null, false, true);
                didReset = didReset || resetIfRelevant(newValues[3], oldValues[3], !!$scope.data.item ? $scope.data.item.categoria.cdCategoria : null, false, true);
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

                var cdSubcategoria = $scope.data.value;

                var cdCategoria = $scope.categoria;
                var cdDepartamento = $scope.departamento;

                if ($scope.data.item != null &&
                    $scope.data.item.cdSubcategoria == cdSubcategoria &&
                    $scope.data.item.categoria.cdCategoria == cdCategoria &&
                    $scope.data.item.categoria.departamento.cdDepartamento == cdDepartamento) {

                    // valor não foi modificado, não deve reagir
                    return $q.resolve($scope.data.item);
                }

                if (zeroIsValid(cdSubcategoria) !== '' && angular.isDefined(cdSistema) && !!cdSistema) {

                    return $q
                        .when(subcategoriaService.obterPorSubcategoriaESistema(cdSubcategoria, cdCategoria, cdDepartamento, cdSistema, isInitializing))
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
                $scope.data.value = item.cdSubcategoria;
                if ($scope.ngModel !== item.cdSubcategoria) {
                    $scope.ngModel = item.cdSubcategoria;
                }
                $scope.data.text = item.dsSubcategoria;
                if (item.categoria) {
                    $scope.categoria = item.categoria.cdCategoria;
                    if (item.categoria.departamento) {
                        $scope.departamento = item.categoria.departamento.cdDepartamento;
                        if (item.categoria.departamento.divisao) {
                            $scope.divisao = item.categoria.departamento.divisao.cdDivisao;
                        }
                    }
                    $scope.sistema = item.cdSistema;
                }

                $scope.$emit('propagate-request', {
                    owner: 'subcategoria',
                    level: 5,
                    subcategoria: item,
                    categoria: item.categoria,
                    departamento: item.categoria.departamento,
                    divisao: item.categoria.departamento.divisao,
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

                if (propagateArgs.level == 5) return;

                if (propagateArgs.level > 5) {
                    
                    var value = propagateArgs.subcategoria;

                    if (!!value) {
                        $scope.ngModel = $scope.data.value = value.cdSubcategoria;
                        $scope.data.text = value.dsSubcategoria;
                        $scope.itemOut = $scope.data.item = value;
                    }
                } else {

                    var doReset = false;

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