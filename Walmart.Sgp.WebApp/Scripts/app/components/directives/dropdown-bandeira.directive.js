(function () {
    'use strict';

    angular
        .module('SGP')
        .directive('dropdownBandeira', DropdownBandeira);

    DropdownBandeira.$inject = ['$q', 'BandeiraService'];

    function DropdownBandeira($q, bandeiraService) {
        return {
            restrict: 'E',
            replace: true,
            templateUrl: 'Scripts/app/components/directives/dropdown-bandeira.template.html',
            scope: {
                sistema: '=?',
                regiaoAdministrativa: '=?',
                permiteBuscaPorRegiao: '=?',
                formato: '=?',
                ngModel: '=',
                showSelect: '=?',
                showAll: '=?',
                itemOut: '=?',
                permiteBuscaSemSistema: '=?'
            },
            link: function ($scope, elem, attr) {
                $scope.dropdownName = attr.name || 'dropdownBandeira';
                $scope.$initialValue = $scope.ngModel;
                $scope.possuiFormato = !!attr.$attr.formato;
                if ($scope.isRestricted) {
                    elem.attr('disabled', 'disabled');
                    attr.ngDisabled = 'true';
                }
            },
            controller: ['$scope', 'UserSessionService', function ($scope, userSession) {

                $scope.data = { values: [], defaultValue: null };
                $scope.resetValue = resetValue;

                $scope.$watchGroup(['sistema', 'formato', 'regiaoAdministrativa'], function (newValues, oldValues) {
                    // Bug 3133:Dropdown bandeira não carrega valor no filtro ao voltar da tela de detalhamento
                    // Não deve comparar por tipo, pois pode vir tanto como string como int.
                    if (newValues[0] != oldValues[0] || newValues[1] != oldValues[1] || newValues[2] != oldValues[2]) {
                        refresh();
                    }
                });

                $scope.$watch('ngModel', function (newVal, oldVal) {
                    if (newVal != oldVal && !newVal && !!$scope.data.defaultValue) {
                        resetValue();
                    }

                    setItemOut();
                });

                refresh();

                var restricao = userSession.getRestricaoLoja();

                if (!!restricao && !!restricao.loja) {
                    if (!!restricao.restrict) {
                        $scope.isRestricted = true;
                    }
                    applyValues([restricao.loja.bandeira]);
                }

                function setItemOut() {
                    var filtered = $scope.data.values.filter(function (b) {
                        return b.idBandeira == $scope.ngModel;
                    });

                    if (filtered.length === 0) {
                        $scope.itemOut = null;
                    } else {
                        $scope.itemOut = filtered[0];
                        if ($scope.possuiFormato) {
                            $scope.formato = filtered[0].idFormato.toString();
                        }
                    }
                }

                function refresh() {
                    var idUsuario = userSession.getCurrentUser().id;

                    var cdSistema = $scope.sistema;

                    var idFormato = $scope.formato;

                    var idRegiaoAdministrativa = $scope.regiaoAdministrativa;

                    var permiteBuscaPorRegiao = $scope.permiteBuscaPorRegiao;

                    var permiteBuscaSemSistema = $scope.permiteBuscaSemSistema;

                    if (permiteBuscaPorRegiao) {

                        if (angular.isDefined(cdSistema) && !!cdSistema && angular.isDefined(idRegiaoAdministrativa) && !!idRegiaoAdministrativa && angular.isDefined(idUsuario)) {

                            var deferred = $q
                                .when(bandeiraService.obterPorUsuarioERegiaoAdministrativa(idUsuario, cdSistema, idRegiaoAdministrativa))
                                .then(applyValues);

                        } else if (angular.isUndefined(idRegiaoAdministrativa) || angular.isUndefined(cdSistema)) {

                            $scope.ngModel = null;
                            $scope.data.values.splice(0, $scope.data.values.length);
                        }

                    } if (permiteBuscaSemSistema) {
                        if (angular.isDefined(idUsuario) && (!$scope.possuiFormato || ($scope.possuiFormato && !!idFormato))) {

                            var deferred = $q
                                .when(bandeiraService.obterPorUsuarioESistema(idUsuario, cdSistema, idFormato))
                                .then(applyValues);

                        } else if (($scope.possuiFormato && !idFormato) || angular.isUndefined(cdSistema)) {

                            $scope.ngModel = null;
                            $scope.data.values.splice(0, $scope.data.values.length);
                        }
                    }
                    else {

                        if (angular.isDefined(cdSistema) && !!cdSistema && angular.isDefined(idUsuario) && (!$scope.possuiFormato || ($scope.possuiFormato && !!idFormato))) {

                            var deferred = $q
                                .when(bandeiraService.obterPorUsuarioESistema(idUsuario, cdSistema, idFormato))
                                .then(applyValues);

                        } else if (($scope.possuiFormato && !idFormato) || angular.isUndefined(cdSistema)) {

                            $scope.ngModel = null;
                            $scope.data.values.splice(0, $scope.data.values.length);
                        }
                    }
                }

                function applyValues(bandeiras) {
                    $scope.data.values = bandeiras;
                    $scope.data.defaultValue = (bandeiras[0] || {}).idBandeira;
                   
                    if (!$scope.isRestricted && ($scope.showSelect || $scope.showAll)) {
                        $scope.data.defaultValue = null;
                    }

                    resetValue();
                }

                function resetValue() {

                    var keyAttr = 'idBandeira'
                    var filteredInitial = $scope.data.values.filter(function (b) {
                        return b[keyAttr] == $scope.$initialValue;
                    });
                    var filteredDefault = $scope.data.values.filter(function (b) {
                        return b[keyAttr] == $scope.data.defaultValue;
                    });
                    if (filteredInitial.length != 0) {
                        $scope.ngModel = $scope.$initialValue;
                    } else if (filteredDefault.length != 0) {
                        $scope.ngModel = $scope.data.defaultValue;
                    }
                }
            }]
        };
    }
})();