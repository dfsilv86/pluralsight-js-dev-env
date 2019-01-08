(function () {
    'use strict';

    // <lookup-vendor-nove-digitos ng-model="cdCategoria" sistema="cdSistema" departamento="cdDepartamento" item-out="item" description-width="xlarge"></lookup-categoria>

    angular
        .module('SGP')
        .directive('lookupVendorNoveDigitos', LookupVendorNoveDigitos);

    LookupVendorNoveDigitos.$inject = ['$q', 'FornecedorParametroService', '$timeout'];

    function LookupVendorNoveDigitos($q, vendorService, $timeout) {

        return {
            restrict: 'E',
            replace: true,
            templateUrl: 'Scripts/app/components/directives/lookup.template.html',
            scope: {
                sistema: '=',
                ngModel: '=',
                fornecedor: '=?',
                itemOut: '=?',
                canal: '=?',
                descriptionWidth: '@',
                ngRequired: '=?',
                ngDisabled: '=?'
            },
            link: function ($scope, elem, attr) {
                $scope.lookupName = attr.name || 'lookupVendorNoveDigitos';
                $scope.isRequired = angular.isDefined(attr.$attr.required);
                if (angular.isDefined(attr.maxWidth)) {
                    $scope.maxWidth = attr.maxWidth;
                }
                $scope.maxLength = "9";
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
            controller: ['$scope', 'StackableModalService', VendorNoveDigitosLookupController]
        };

        function VendorNoveDigitosLookupController($scope, $modal) {

            $scope.maxWidth = $scope.maxWidth || 9;
            $scope.data = { item: null, value: '', text: '' };
            $scope.$watchGroup(['sistema', 'canal', 'ngModel'], refresh);
            $scope.lookup = lookup;
            $scope.search = search;
            $scope.check = check;
            $scope.reset = reset;

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
                    templateUrl: 'Scripts/app/gerenciamento/modal-vendor-nove-digitos-lookup.html',
                    controller: 'ModalVendorNoveDigitosLookupController',
                    resolve: {
                        sistema: $scope.sistema,
                        cdV9D: $scope.data.value,
                        canal: $scope.canal,
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

                var didReset = resetIfRelevant(newValues[0], oldValues[0], !!$scope.data.item ? $scope.data.item.cdSistema : null, true);  // sistema
                didReset = resetIfRelevant(newValues[1], oldValues[1], !!$scope.data.item ? $scope.data.item.cdTipo : null, false);        // canal
                didReset = didReset || resetIfRelevant(newValues[theValueIndex], oldValues[theValueIndex], $scope.data.value, true, true); // cdv9d

                if (didReset) {

                    skipReset = false;

                }

                if ($scope.itemOut !== null && angular.isDefined($scope.itemOut) && $scope.itemOut.cdV9D === newValues[theValueIndex]) {

                    applyValue($scope.itemOut);

                    skipReset = false;
                    return;

                }

                if ((!didReset && $scope.data.value !== newValues[theValueIndex]) || (didReset && !!oldValues[theValueIndex] && newValues[theValueIndex] !== oldValues[theValueIndex])) {
                    $scope.data.value = newValues[theValueIndex];
                    lookup(newValues.equals(oldValues));
                    skipReset = false;
                }
            }

            function lookup(isInitializing) {

                var cdSistema = $scope.sistema;
                var cdV9D = $scope.data.value;
                var cdTipo = $scope.canal;

                if ($scope.data.item != null &&
                    $scope.data.item.cdV9D == cdV9D) {

                    // valor não foi modificado, não deve reagir
                    return $q.resolve($scope.data.item);
                }

                if ($scope.data.item != null && $scope.data.item.cdV9D != cdV9D) {
                    // mudou o vendor sem passar pelo reset, entao limpa o canal
                    $scope.canal = cdTipo = null;

                }

                if (zeroIsValid(cdV9D) !== '' && angular.isDefined(cdSistema) && !!cdSistema) {

                    return $q
                        .when(vendorService.obterPorSistemaECodigo9Digitos(cdSistema, cdTipo, cdV9D, isInitializing))
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
                $scope.data.value = item.cdV9D;
                if ($scope.ngModel !== item.cdV9D) {
                    $scope.ngModel = item.cdV9D;
                }
                $scope.data.text = '';

                if (item.fornecedor) {
                    $scope.data.text = item.fornecedor.nmFornecedor;

                    $scope.fornecedor = item.fornecedor.cdFornecedor;
                }
                    
                $scope.canal = item.cdTipo;

                $scope.sistema = item.cdSistema;

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
                $scope.canal = null;
            }
        }
    }
})();