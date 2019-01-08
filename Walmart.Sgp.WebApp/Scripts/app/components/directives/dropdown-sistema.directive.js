(function () {
    'use strict';

    angular
        .module('SGP')
        .directive('dropdownSistema', DropdownSistema);

    DropdownSistema.$inject = ['$q', 'SistemaService'];

    function DropdownSistema($q, sistemaService) {
        return {
            restrict: 'E',
            replace: true,
            templateUrl: 'Scripts/app/components/directives/dropdown-sistema.template.html',
            scope: {
                ngModel: '=',
                itemOut: '=?'
            },
            link: function ($scope, elem, attr) {
                $scope.dropdownName = attr.name || 'dropdownSistema';
                $scope.$initialValue = $scope.ngModel;
                if ($scope.isRestricted) {
                    elem.attr('disabled', 'disabled');
                    attr.ngDisabled = 'true';
                }
            },
            controller: ['$scope', 'UserSessionService', function ($scope, userSession) {

                $scope.data = { values: [], defaultValue: null };
                $scope.resetValue = resetValue;

                var restricao = userSession.getRestricaoLoja();

                if (!!restricao && !!restricao.loja) {
                    if (!!restricao.restrict) {
                        $scope.isRestricted = true;
                    }
                    applyValues([{ cdSistema: restricao.loja.bandeira.cdSistema }]);
                }

                var idUsuario = userSession.getCurrentUser().id;

                var deferred = $q
                    .when(sistemaService.obterPorUsuario(idUsuario))
                    .then(applyValues);

                $scope.$watch('ngModel', function (newVal, oldVal) {
                    if (newVal != oldVal && !newVal && !!$scope.data.defaultValue) {
                        resetValue();
                    }

                    setItemOut();
                });

                function setItemOut() {                    
                    var filtered = $scope.data.values.filter(function (b) {
                        return b.cdSistema == $scope.ngModel;
                    });

                    $scope.itemOut = filtered.length === 0 ? null : filtered[0];
                }

                function applyValues(sistemas) {
                    $scope.data.values = sistemas;
                    $scope.data.defaultValue = (sistemas[0] || {}).cdSistema;
                    resetValue();
                }

                function resetValue() {
                    $scope.ngModel = $scope.$initialValue || $scope.data.defaultValue;
                }
            }]
        };
    }
})();