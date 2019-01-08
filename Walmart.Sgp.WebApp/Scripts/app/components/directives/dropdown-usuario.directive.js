(function () {
    'use strict';

    angular
        .module('SGP')
        .directive('dropdownUsuario', DropdownUsuario);

    DropdownUsuario.$inject = ['$q', 'UsuarioService'];

    function DropdownUsuario($q, usuarioService) {
        return {
            restrict: 'E',
            replace: true,
            templateUrl: 'Scripts/app/components/directives/dropdown-usuario.template.html',
            scope: {
                ngModel: '=',
                showSelect: '=?',
                showAll: '=?',
                itemOut: '=?'
            },
            link: function ($scope, elem, attr) {
                $scope.dropdownName = attr.name || 'dropdownUsuario';
                $scope.$initialValue = $scope.ngModel;
            },
            controller: ['$scope', 'ApiEndpoints', 'UserSessionService', function ($scope, apiEndpoints, userSession) {

                var pageSize = 30;

                $scope.select2Options = {
                    ajax: {
                        url: apiEndpoints.sgp.usuario,
                        dataType: 'json',
                        delay: 250,
                        headers: { 'Authorization': 'Bearer ' + userSession.getToken() },
                        data: function (params) {
                            return apiEndpoints.createParams({ offset: params.page * pageSize, limit: pageSize }, { userName: params });
                        },
                        results: function (data, params) {
                            params.page = params.page || 1;
                            return {
                                results: data,
                                pagination: {
                                    more: (params.page * pageSize) < data.totalCount
                                }
                            };
                        },
                        cache: true
                    },
                    formatResult: function (result, container, query, escapeMarkup) {
                        var markup = [];
                        Select2.util.markMatch((result.fullName || '') + ' (' + (result.userName || '') + ')', query.term, markup, escapeMarkup);
                        return markup.join("");
                    },
                    escapeMarkup: function (markup) {
                        return markup;
                    },
                    minimumInputLength: 1,
                };

                $scope.data = { values: [], defaultValue: null };

                $scope.resetValue = resetValue;

                $scope.$watch('ngModel', function (newVal, oldVal) {
                    if (newVal !== oldVal && !newVal && !!$scope.data.defaultValue) {
                        resetValue();
                    }

                    setItemOut();
                });

                function setItemOut() {
                    var filtered = $scope.data.values.filter(function (b) {
                        return b.id == $scope.ngModel;
                    });

                    $scope.itemOut = filtered.length === 0 ? null : filtered[0];
                }

                function refresh() {

                    /*var deferred = $q
                        .when(usuarioService.obterTodos({ offset: 0, limit: 10000 }))
                        .then(applyValues);*/
                }

                function applyValues(usuarios) {
                    $scope.data.values = usuarios;
                    $scope.data.defaultValue = (usuarios[0] || {}).id;

                    if ($scope.showSelect || $scope.showAll) {
                        $scope.data.defaultValue = null;
                    }

                    resetValue();
                }

                function resetValue() {
                    $scope.ngModel = $scope.$initialValue || $scope.data.defaultValue;
                }

                refresh();
            }]
        };
    }
})();