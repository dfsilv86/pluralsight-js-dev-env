(function () {
    'use strict';

    // <lookup-usuario ng-model="userName" usuario="idUsuario" item-out="item" description-width="xlarge"></lookup-usuario>

    angular
        .module('SGP')
        .directive('lookupUsuario', LookupUsuario);

    LookupUsuario.$inject = ['$q', 'UsuarioService', '$timeout'];

    // Esta lookup é diferente porque ela não depende de um atributo externo que serve como filtro; ao invés disso,
    // ela é dois atributos ao mesmo tempo: o editável (login do usuario) e o não-editável (o id do usuário em uma model externa)
    // Deve conciliar as alterações nos dois atributos.
    function LookupUsuario($q, usuarioService, $timeout) {
        return {
            restrict: 'E',
            replace: true,
            templateUrl: 'Scripts/app/components/directives/lookup-usuario.template.html',
            scope: {
                ngModel: '=',
                usuario: '=?',
                itemOut: '=?',
                descriptionWidth: '@',
                ngRequired: '=?',
                ngDisabled: '=?'
            },
            link: function ($scope, elem, attr) {
                $scope.lookupName = attr.name || 'lookupUsuario';
                $scope.isRequired = angular.isDefined(attr.$attr.required);
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
            controller: ['$scope', 'StackableModalService', UsuarioLookupController]
        };

        function UsuarioLookupController($scope, $modal) {
            $scope.maxWidth = $scope.maxWidth || 50;
            $scope.data = { item: null, value: '', text: '' };
            $scope.$watchGroup(['usuario', 'ngModel'], refresh);
            $scope.lookup = lookup;
            $scope.search = search;
            $scope.check = check;
            $scope.reset = reset;

            var skipReset = true;

            function search() {
                if (($scope.data.value || '') != '') {
                    return $scope.lookup();
                } else {
                    return openModal();
                }
            }

            function openModal() {

                var theModal = $modal.open({
                    templateUrl: 'Scripts/app/acessos/modal-usuario-lookup.view.html',
                    controller: 'ModalUsuarioLookupController',
                    resolve: {
                        userName: $scope.data.value,
                        fullName: null,
                        email: null,
                        usuario: $scope.usuario
                    },
                    opened: function () {
                        // Evita que a modal seja aberta duas vezes quando o usuário tecla enter.
                        reset();
                    }
                });

                return theModal.then(applyValue).finally($scope.focus);
            }

            function check() {
                if ($scope.data.value === '' || $scope.data.value == null) {
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

                if ((newValues[0] || null) === (oldValues[0] || null)
                    && (newValues[1] || null) === (oldValues[1] || null)
                    && !newValues[0] && !oldValues[0]) {
                    return;
                }

                var theValueIndex = 1;

                var didReset = resetIfRelevant(newValues[0], oldValues[0], !!$scope.data.item ? $scope.data.item.id : null, false, false);
                didReset = didReset || resetIfRelevant(newValues[theValueIndex], oldValues[theValueIndex], $scope.data.value, true, false);

                var originalSkipReset = skipReset;

                if (didReset) {

                    skipReset = false;
                }

                // Here be dragons
                if (didReset && !originalSkipReset && !skipReset && oldValues[0] != newValues[0] && oldValues[1] != newValues[1] && !!newValues[0] && !newValues[1]) {
                    // O id do usuário na model fora da lookup (que entra pelo atributo usuario) mudou pra um valor preenchido, aqui busca pelo id
                    // nota: aqui já passou pelo reset, então busca pelo newValues[0] mesmo.
                    getById(newValues.equals(oldValues), newValues[0]);
                } else if ((!didReset && $scope.data.value !== newValues[theValueIndex]) || (didReset && !!oldValues[theValueIndex] && newValues[theValueIndex] !== oldValues[theValueIndex])) {
                    // o login foi preenchido, busca pelo login, procedimento normal
                    $scope.data.value = newValues[theValueIndex];
                    lookup(newValues.equals(oldValues));
                    skipReset = false;
                } else if (originalSkipReset && didReset && newValues[0] == oldValues[0] && newValues[1] == oldValues[1] && !!newValues[0] && !newValues[1]) {
                    // inicializou a tela e o controller, com o id do usuário já preenchido na model fora da lookup, entao inicializa o controler buscando o usuario pelo id
                    // nota: aqui não passou pelo reset por causa do skipReset, podemos obter o id do usuario pelo atributo
                    getById(newValues.equals(oldValues));
                }
            }

            function getById(isInitializing, theId) {
                var id = theId || $scope.usuario;
                if (!!id) {
                    return $q
                        .when(usuarioService.obterPorId(id, isInitializing))
                        .then(applyValue)
                        .catch(invalidValue);
                }
            }

            function lookup(isInitializing) {

                var userName = $scope.data.value;

                if ($scope.data.item != null &&
                    $scope.data.item.userName == userName) {

                    // valor não foi modificado, não deve reagir
                    return $q.resolve($scope.data.item);
                }

                if ((userName || '') !== '') {

                    return $q
                        .when(usuarioService.obterPorUsuario(userName, isInitializing))
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
                $scope.data.value = item.userName;
                if ($scope.usuario != item.id) {
                    $scope.usuario = item.id;
                }
                if ($scope.ngModel != item.userName) {
                    $scope.ngModel = item.userName;
                }
                $scope.data.text = item.fullName;
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
                $scope.usuario = null;
                $scope.ngModel = null;
            }
        }
    }
})();