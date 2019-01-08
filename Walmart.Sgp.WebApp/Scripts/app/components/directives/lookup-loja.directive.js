(function () {
    //'use strict';

    // <lookup-loja ng-model="cdLoja" bandeira="idBandeira" sistema="cdSistema" item-out="loja" description-width="xlarge" max-width="9" required ng-required="foo"></lookup-loja>

    angular
        .module('SGP')
        .directive('lookupLoja', LookupLoja);

    LookupLoja.$inject = ['$q', 'LojaService', '$timeout'];

    function LookupLoja($q, lojaService, $timeout) {
        return {
            restrict: 'E',
            replace: true,
            templateUrl: 'Scripts/app/components/directives/lookup.template.html',
            scope: {
                sistema: '=',
                bandeira: '=?',
                ignoreBandeira: '=',
                ngModel: '=',
                itemOut: '=?',
                descriptionWidth: '@',
                ngRequired: '=',
                ngDisabled: '='
            },
            link: function ($scope, elem, attr) {
                $scope.lookupName = attr.name || 'lookupLoja';
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
            controller: ['$scope', 'StackableModalService', 'UserSessionService', LojaLookupController]
        };

        function LojaLookupController($scope, $modal, userSession) {

            $scope.maxWidth = $scope.maxWidth || 9;
            $scope.data = { item: null, value: '', text: '' };
            $scope.$watchGroup(['sistema', 'bandeira', 'ngModel'], refresh);
            $scope.lookup = lookup;
            $scope.search = search;
            $scope.check = check;
            $scope.reset = reset;
            $scope.isRestricted = false;

            function aplicarRestricaoLoja() {

                var restricao = userSession.getRestricaoLoja();

                if (!!restricao.loja) {
                    applyValue(restricao.loja);
                    if (!!restricao.restrict) {
                        $scope.isRestricted = true;
                    }
                    skipReset = false;
                }
            }

            var skipReset = true;

            aplicarRestricaoLoja();

            function search() {
                if (!!$scope.data.value) {
                    return $scope.lookup();
                } else {
                    return openModal();
                }
            }

            function openModal() {
                var theModal = $modal.open({
                    templateUrl: 'Scripts/app/estruturaMercadologica/modal-loja-lookup.html',
                    controller: 'ModalLojaLookupController',
                    resolve: {
                        bandeira: $scope.bandeira,
                        sistema: $scope.sistema,
                        loja: $scope.data.value,
                        $stateParams: {}
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
                var theValueIndex = 2;

                var didReset = resetIfRelevant(newValues[0], oldValues[0], !!$scope.data.item ? $scope.data.item.cdSistema : null, true);
                didReset = didReset || resetIfRelevant(newValues[1], oldValues[1], !!$scope.data.item ? $scope.data.item.idBandeira : null, false);
                didReset = didReset || resetIfRelevant(newValues[theValueIndex], oldValues[theValueIndex], $scope.data.value, true);

                if (didReset) {

                    skipReset = false;
                }

                if ((!didReset && $scope.data.value !== newValues[theValueIndex]) || (didReset && !!oldValues[theValueIndex] && newValues[theValueIndex] !== oldValues[theValueIndex])) {
                    $scope.data.value = newValues[theValueIndex];
                    lookup(newValues.equals(oldValues));
                    skipReset = false;
                }

                if (didReset && $scope.isRestricted) {
                    aplicarRestricaoLoja();
                }
            }

            function lookup(isInitializing) {

                var idUsuario = userSession.getCurrentUser().id;
                var idBandeira = $scope.ignoreBandeira ? null : $scope.bandeira;
                var cdLoja = $scope.data.value;

                if ($scope.data.item != null && $scope.data.item.cdLoja == cdLoja) {
                    // valor não foi modificado, não deve reagir
                    return $q.resolve($scope.data.item);
                }

                if (angular.isDefined(cdLoja) && !!cdLoja && angular.isDefined(idUsuario)) {

                    return $q
                        .when(lojaService.obterPorLojaUsuarioEBandeira(cdLoja, idUsuario, idBandeira, isInitializing))
                        .then(applyValue)
                        .catch(invalidValue)
                        .finally($scope.focus);
                }
            }

            function applyValue(loja) {
                if (angular.isUndefined(loja) || !loja) {
                    return $q.reject(loja);
                }
                $scope.itemOut = $scope.data.item = loja;
                $scope.data.value = loja.cdLoja;
                if ($scope.ngModel !== loja.cdLoja) {
                    $scope.ngModel = loja.cdLoja;
                }
                $scope.data.text = loja.nmLoja;

                // Verificado se os valores estão diferentes, pois no caso de serem os mesmos estava acontecendo o bug:
                // Bug 2790:Ao acionar a lupa do campo de loja o sistema não carrega a loja.
                if ($scope.sistema != loja.cdSistema) {
                    $scope.sistema = loja.cdSistema;
                }

                if ($scope.bandeira != loja.idBandeira) {
                    $scope.bandeira = loja.idBandeira;
                }

                return loja;
            }

            function invalidValue(response) {
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