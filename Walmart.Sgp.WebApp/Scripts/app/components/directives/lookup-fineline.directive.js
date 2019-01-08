(function () {
    'use strict';

    // <lookup-fine-line ng-model="cdFineLine" sistema="cdSistema" departamento="cdDepartamento" categoria="cdCategoria" subcategoria="cdSubcategoria" item-out="item" description-width="xlarge"></lookup-fine-line>

    angular
        .module('SGP')
        .directive('lookupFineLine', LookupFineLine);

    LookupFineLine.$inject = ['$q', 'FineLineService', '$timeout'];

    function LookupFineLine($q, fineLineService, $timeout) {
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
                subcategoria: '=?',
                divisao: '=?',
                itemOut: '=?',
                descriptionWidth: '@',
                ngRequired: '=?',
                ngDisabled: '=?'
            },
            link: function ($scope, elem, attr) {
                $scope.lookupName = attr.name || 'lookupFineLine';
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
            controller: ['$scope', 'StackableModalService', 'PropagationService', FineLineLookupController]
        };

        function FineLineLookupController($scope, $modal, propagationService) {
            

            $scope.maxWidth = $scope.maxWidth || 9;
            $scope.data = { item: null, value: '', text: '' };
            $scope.$watchGroup(['sistema', 'departamento', 'divisao', 'categoria', 'subcategoria', 'ngModel'], refresh);
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
                    templateUrl: 'Scripts/app/estruturaMercadologica/modal-fineLine-lookup.html',
                    controller: 'ModalFineLineLookupController',
                    resolve: {
                        sistema: $scope.sistema,
                        categoria: $scope.categoria,
                        subcategoria: $scope.subcategoria,
                        fineLine: $scope.data.value,
                        departamento: $scope.departamento,
                        requiredOptions: {
                            categoria: $scope.categoriaRequired,
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

                var theValueIndex = 5;

                var didReset = resetIfRelevant(newValues[0], oldValues[0], !!$scope.data.item ? $scope.data.item.cdSistema : null, true);
                didReset = didReset || resetIfRelevant(newValues[1], oldValues[1], !!$scope.data.item ? $scope.data.item.subcategoria.categoria.departamento.cdDepartamento : null, false, true);
                didReset = didReset || resetIfRelevant(newValues[2], oldValues[2], !!$scope.data.item ? $scope.data.item.subcategoria.categoria.departamento.divisao.cdDivisao : null, false, true);
                didReset = didReset || resetIfRelevant(newValues[3], oldValues[3], !!$scope.data.item ? $scope.data.item.subcategoria.categoria.cdCategoria : null, false, true);
                didReset = didReset || resetIfRelevant(newValues[4], oldValues[4], !!$scope.data.item ? $scope.data.item.subcategoria.cdSubcategoria : null, false, true);
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

                var cdFineLine = $scope.data.value;
                var cdSubcategoria = $scope.subcategoria;
                var cdCategoria = $scope.categoria;
                var cdDepartamento = $scope.departamento;

                if ($scope.data.item != null &&
                    $scope.data.item.cdFineLine == cdFineLine &&
                    $scope.data.item.subcategoria.cdSubcategoria == cdSubcategoria &&
                    $scope.data.item.subcategoria.categoria.cdCategoria == cdCategoria &&
                    $scope.data.item.subcategoria.categoria.departamento.cdDepartamento == cdDepartamento) {

                    // valor não foi modificado, não deve reagir
                    return $q.resolve($scope.data.item);
                }

                if (zeroIsValid(cdFineLine) !== '' && angular.isDefined(cdSistema) && !!cdSistema) {

                    return $q
                        .when(fineLineService.obterPorFineLineESistema(cdFineLine, cdSubcategoria, cdCategoria, cdDepartamento, cdSistema, isInitializing))
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
                $scope.data.value = item.cdFineLine;
                if ($scope.ngModel !== item.cdFineLine) {
                    $scope.ngModel = item.cdFineLine;
                }
                $scope.data.text = item.dsFineLine;
                if (item.subcategoria) {
                    $scope.subcategoria = item.subcategoria.cdSubcategoria;
                    if (item.subcategoria.categoria) {
                        $scope.categoria = item.subcategoria.categoria.cdCategoria;
                        if (item.subcategoria.categoria.departamento) {
                            $scope.departamento = item.subcategoria.categoria.departamento.cdDepartamento;
                            if (item.subcategoria.categoria.departamento.divisao) {
                                $scope.divisao = item.subcategoria.categoria.departamento.divisao.cdDivisao;
                            }
                        }
                    }
                    $scope.sistema = item.cdSistema;
                }

                $scope.$emit('propagate-request', {
                    owner: 'fineline',
                    level: 6,
                    fineline: item,
                    subcategoria: item.subcategoria,
                    categoria: item.subcategoria.categoria,
                    departamento: item.subcategoria.categoria.departamento,
                    divisao: item.subcategoria.categoria.departamento.divisao,
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

                if (propagateArgs.level == 6) return;

                if (propagateArgs.level > 6) {

                    var value = propagateArgs.fineline;

                    if (!!value) {
                        $scope.ngModel = $scope.data.value = value.cdFineLine;
                        $scope.data.text = value.dsFineLine;
                        $scope.itemOut = $scope.data.item = value;
                    }

                } else {

                    var doReset = false;

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